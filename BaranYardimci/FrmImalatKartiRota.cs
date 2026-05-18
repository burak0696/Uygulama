using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ACadSharp;
using ACadSharp.IO;
using ACadSharp.Entities;
using Button = System.Windows.Forms.Button;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using FontStyle = System.Drawing.FontStyle;
using Excel = Microsoft.Office.Interop.Excel;

namespace BaranYardimci
{
    public partial class FrmImalatKartiRota : Form
    {
        private static readonly string[] ROTA_KOD = { "K", "D", "M", "O", "S", "B", "W", "G" };
        private static readonly string[] ROTA_ISIM = { "KESME", "DELME", "MARKALAMA", "OYMA", "SIRT ALMA", "BÜKME", "KAYNAK", "GALVANİZ" };
        private static readonly string[] ROTA_BILESEN = { "kesme", "delme", "markalama", "oyma", "sirt alma", "bükme", "kaynak", "galvaniz" };
        private static readonly int[] VARSAYILAN_ROTA = { 0, 2, 7 };

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(ref NETRESOURCE nr, string password, string username, int flags);
        [StructLayout(LayoutKind.Sequential)]
        private struct NETRESOURCE { public int dwScope, dwType, dwDisplayType, dwUsage; public string lpLocalName, lpRemoteName, lpComment, lpProvider; }

        const string NET_PATH = @"\\192.168.2.10\erp\Hazir_Exceller";
        const string NET_USER = "admin";
        const string NET_PASS = "asd123asd";
        const string FAV_FILE = "erp_favoriler.txt";

        const int COL_PROJENO = 1;
        const int COL_POZNO = 2;
        const int COL_POZACIKLAMA = 3;
        const int COL_ANAPOZNO = 4;
        const int COL_POZMIKTAR = 5;
        const int COL_POZAGIRLIK = 6;
        const int COL_BILTUR = 7;
        const int COL_BILNO = 8;
        const int COL_BILMIKTAR = 9;
        const int COL_ISLEMSIRA = 10;

        private class PozBilgi
        {
            public string PozNo, PozNoOrijinal, DwgYolu, PdfYolu, GoruntuYolu;
            public int ExcelSatir;
            public double UretilecekAdet;
            public object ProjeNo, AnaPozNo, PozMiktar, PozAgirlik;
        }

        private class CadTransform
        {
            public double X, Y, ScaleX = 1, ScaleY = 1, Rotation;
            public PointF Apply(double x, double y)
            {
                double sx = x * ScaleX, sy = y * ScaleY;
                double cos = Math.Cos(Rotation), sin = Math.Sin(Rotation);
                return new PointF((float)(sx * cos - sy * sin + X), (float)(sx * sin + sy * cos + Y));
            }
            public static CadTransform Combine(CadTransform p, CadTransform c)
            {
                var pos = p.Apply(c.X, c.Y);
                return new CadTransform { X = pos.X, Y = pos.Y, ScaleX = p.ScaleX * c.ScaleX, ScaleY = p.ScaleY * c.ScaleY, Rotation = p.Rotation + c.Rotation };
            }
        }

        private class RenderItem
        {
            public string Type, Text;
            public PointF P1, P2;
            public float Radius, StartAngle, EndAngle, Size;
            public List<PointF> Points;
        }

        private readonly string _erpExcelYolu;
        private string _imalatKlasor = "";
        private List<PozBilgi> _pozList = new List<PozBilgi>();
        private List<PozBilgi> _filtreliPozlar = new List<PozBilgi>();
        private List<string> _favoriler = new List<string>();
        private Dictionary<string, bool[]> _rotaDurumlar = new Dictionary<string, bool[]>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _varsayilanUygulandi = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private int _mevcutIndex = -1;
        private Button[] _rotaButonlar = new Button[8];
        private List<string> _geciciDosyalar = new List<string>();

        private Panel _pnlFavoriler;
        private ListBox _lbFavoriler;
        private TextBox _txtArama;

        // ── CONSTRUCTOR ───────────────────────────────────────────────────

        public FrmImalatKartiRota(string erpExcelYolu)
        {
            InitializeComponent();
            _erpExcelYolu = erpExcelYolu;
            this.KeyPreview = true;
            this.KeyDown += FrmImalatKartiRota_KeyDown;
        }

        private void FrmImalatKartiRota_Load(object sender, EventArgs e)
        {
            var nr = new NETRESOURCE { dwType = 1, lpRemoteName = NET_PATH };
            WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0);

            FavorileriYukle();
            EkleFavoriPaneli();
            EkleAramaKutusu();

            // lbPozlar owner draw — seçim mavisi kaldır
            lbPozlar.DrawMode = DrawMode.OwnerDrawFixed;
            lbPozlar.DrawItem += lbPozlar_DrawItem;

            if (string.IsNullOrEmpty(_erpExcelYolu) || !File.Exists(_erpExcelYolu))
            {
                MessageBox.Show("ERP Excel dosyası bulunamadı:\n" + _erpExcelYolu, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close(); return;
            }

            lblErpDosya.Text = "📄 " + Path.GetFileName(_erpExcelYolu);
            OlusturRotaButonlar();
            ExceldenPozOku();

            if (_pozList.Count == 0) { MessageBox.Show("Excel'de poz bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            txtKlasorYol.Focus();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            foreach (var f in _geciciDosyalar)
                try { if (File.Exists(f)) File.Delete(f); } catch { }
        }

        // ── KLAVYE ────────────────────────────────────────────────────────

        private void FrmImalatKartiRota_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl == txtKlasorYol || ActiveControl == _txtArama) return;
            switch (e.KeyCode)
            {
                case Keys.K: RotaToggle(0); e.SuppressKeyPress = true; break;
                case Keys.D: RotaToggle(1); e.SuppressKeyPress = true; break;
                case Keys.M: RotaToggle(2); e.SuppressKeyPress = true; break;
                case Keys.O: RotaToggle(3); e.SuppressKeyPress = true; break;
                case Keys.S: RotaToggle(4); e.SuppressKeyPress = true; break;
                case Keys.B: RotaToggle(5); e.SuppressKeyPress = true; break;
                case Keys.W: RotaToggle(6); e.SuppressKeyPress = true; break;
                case Keys.G: RotaToggle(7); e.SuppressKeyPress = true; break;
                case Keys.Enter: e.SuppressKeyPress = true; if (_mevcutIndex < _pozList.Count - 1) GosterPoz(_mevcutIndex + 1); break;
                case Keys.Left: case Keys.Up: if (!e.Alt) { if (_mevcutIndex > 0) GosterPoz(_mevcutIndex - 1); e.SuppressKeyPress = true; } break;
                case Keys.Right: case Keys.Down: if (!e.Alt) { if (_mevcutIndex < _pozList.Count - 1) GosterPoz(_mevcutIndex + 1); e.SuppressKeyPress = true; } break;
            }
        }

        // ── FAVORİ PANELİ ─────────────────────────────────────────────────

        private void EkleFavoriPaneli()
        {
            _pnlFavoriler = new Panel { Width = 260, Dock = DockStyle.Right, BackColor = Color.FromArgb(18, 18, 26) };
            var lblBaslik = new Label { Text = "⭐  SIKÇA KULLANILANLAR", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(255, 210, 80), Dock = DockStyle.Top, Height = 32, TextAlign = ContentAlignment.MiddleCenter };
            var lblAcikla = new Label { Text = "2× tıkla → Yeni pencerede aç", Font = new Font("Segoe UI", 8f, FontStyle.Italic), ForeColor = Color.FromArgb(110, 120, 140), Dock = DockStyle.Top, Height = 22, TextAlign = ContentAlignment.MiddleCenter };
            var pnlButonlar = new TableLayoutPanel { Dock = DockStyle.Bottom, Height = 42, ColumnCount = 2, RowCount = 1, BackColor = Color.FromArgb(18, 18, 26) };
            pnlButonlar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            pnlButonlar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            var btnEkle = new Button { Text = "➕ Ekle", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(34, 100, 34), ForeColor = Color.White, Cursor = Cursors.Hand };
            btnEkle.FlatAppearance.BorderSize = 0; btnEkle.Click += BtnFavEkle_Click;
            var btnSil = new Button { Text = "🗑 Sil", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(100, 28, 28), ForeColor = Color.White, Cursor = Cursors.Hand };
            btnSil.FlatAppearance.BorderSize = 0; btnSil.Click += BtnFavSil_Click;
            pnlButonlar.Controls.Add(btnEkle, 0, 0); pnlButonlar.Controls.Add(btnSil, 1, 0);
            _lbFavoriler = new ListBox { Dock = DockStyle.Fill, BackColor = Color.FromArgb(24, 24, 36), ForeColor = Color.FromArgb(200, 215, 240), Font = new Font("Segoe UI", 10f), BorderStyle = BorderStyle.None };
            _lbFavoriler.DoubleClick += LbFavoriler_DoubleClick;
            _pnlFavoriler.Controls.Add(_lbFavoriler);
            _pnlFavoriler.Controls.Add(pnlButonlar);
            _pnlFavoriler.Controls.Add(lblAcikla);
            _pnlFavoriler.Controls.Add(lblBaslik);
            pnlAna.Controls.Add(_pnlFavoriler);
            _pnlFavoriler.BringToFront();
            FavorileriListeyeYaz();
        }

        // ── ARAMA KUTUSU ──────────────────────────────────────────────────

        private void EkleAramaKutusu()
        {
            _txtArama = new TextBox { Dock = DockStyle.Bottom, Height = 42, Font = new Font("Segoe UI", 13f), BackColor = Color.FromArgb(30, 32, 48), ForeColor = Color.FromArgb(100, 110, 130), BorderStyle = BorderStyle.None, Text = "🔍 Poz ara..." };
            _txtArama.GotFocus += (s, ev) => { if (_txtArama.Text == "🔍 Poz ara...") { _txtArama.Text = ""; _txtArama.ForeColor = Color.FromArgb(200, 215, 245); } };
            _txtArama.LostFocus += (s, ev) => { if (string.IsNullOrEmpty(_txtArama.Text)) { _txtArama.Text = "🔍 Poz ara..."; _txtArama.ForeColor = Color.FromArgb(100, 110, 130); } };
            _txtArama.TextChanged += TxtArama_TextChanged;
            pnlSol.Controls.Add(_txtArama);
        }

        private void TxtArama_TextChanged(object sender, EventArgs e)
        {
            string ara = (_txtArama?.Text ?? "").Trim();
            if (ara == "🔍 Poz ara...") ara = "";
            _filtreliPozlar = string.IsNullOrEmpty(ara)
                ? _pozList.ToList()
                : _pozList.Where(p => p.PozNo.ToUpper().Contains(ara.ToUpper())).ToList();
            ListeyiGuncelle();
        }

        // ── FAVORİLER ─────────────────────────────────────────────────────

        private void FavorileriYukle()
        {
            _favoriler.Clear();
            if (!File.Exists(FAV_FILE)) return;
            try
            {
                foreach (var s in File.ReadAllLines(FAV_FILE, Encoding.UTF8).Where(x => !string.IsNullOrWhiteSpace(x)))
                { string yol = s.Trim(); if (File.Exists(yol) && !_favoriler.Contains(yol, StringComparer.OrdinalIgnoreCase)) _favoriler.Add(yol); }
            }
            catch { }
        }

        private void FavorileriKaydet() { try { File.WriteAllLines(FAV_FILE, _favoriler, Encoding.UTF8); } catch { } }

        private void FavorileriListeyeYaz()
        {
            if (_lbFavoriler == null) return;
            _lbFavoriler.Items.Clear();
            foreach (var f in _favoriler)
                _lbFavoriler.Items.Add("📊  " + Path.GetFileNameWithoutExtension(f));
        }

        private void BtnFavEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_erpExcelYolu) || !File.Exists(_erpExcelYolu)) { MessageBox.Show("Geçerli ERP dosyası yok."); return; }
            if (_favoriler.Contains(_erpExcelYolu, StringComparer.OrdinalIgnoreCase)) { MessageBox.Show("Bu dosya zaten favorilerde."); return; }
            _favoriler.Add(_erpExcelYolu); FavorileriKaydet(); FavorileriListeyeYaz();
            MessageBox.Show("Favorilere eklendi:\n" + Path.GetFileName(_erpExcelYolu), "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnFavSil_Click(object sender, EventArgs e)
        {
            int sel = _lbFavoriler?.SelectedIndex ?? -1;
            if (sel < 0 || sel >= _favoriler.Count) return;
            _favoriler.RemoveAt(sel); FavorileriKaydet(); FavorileriListeyeYaz();
        }

        private void LbFavoriler_DoubleClick(object sender, EventArgs e)
        {
            int sel = _lbFavoriler?.SelectedIndex ?? -1;
            if (sel < 0 || sel >= _favoriler.Count) return;
            string yol = _favoriler[sel];
            if (!File.Exists(yol)) { MessageBox.Show("Dosya bulunamadı:\n" + yol); return; }
            var frm = new FrmImalatKartiRota(yol);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        // ── KLASÖR SEÇİMİ ─────────────────────────────────────────────────

        private void btnKlasorSec_Click(object sender, EventArgs e)
        { txtKlasorYol.Focus(); txtKlasorYol.SelectAll(); }

        private void btnKlasorGozat_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog { Description = "İmalat kartlarının bulunduğu klasörü seçiniz  (DWG / DXF / PDF)", ShowNewFolderButton = false, SelectedPath = txtKlasorYol.Text.Trim() };
            if (fbd.ShowDialog() != DialogResult.OK) return;
            txtKlasorYol.Text = fbd.SelectedPath;
            KlasorUygula(fbd.SelectedPath);
        }

        private void btnKlasorGit_Click(object sender, EventArgs e)
        {
            string yol = txtKlasorYol.Text.Trim();
            if (string.IsNullOrEmpty(yol)) { MessageBox.Show("Lütfen bir klasör yolu girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtKlasorYol.Focus(); return; }
            if (!Directory.Exists(yol))
            {
                if (yol.StartsWith(@"\\")) { var nr = new NETRESOURCE { dwType = 1, lpRemoteName = yol }; WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0); }
                if (!Directory.Exists(yol)) { txtKlasorYol.BackColor = Color.FromArgb(80, 30, 30); MessageBox.Show("Klasör bulunamadı:\n" + yol, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            txtKlasorYol.BackColor = Color.FromArgb(20, 50, 30);
            KlasorUygula(yol);
        }

        private void txtKlasorYol_KeyDown(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnKlasorGit_Click(sender, EventArgs.Empty); } }

        private void KlasorUygula(string yol)
        {
            _imalatKlasor = yol;
            lblKlasor.Text = "📁 " + yol;
            txtKlasorYol.Text = yol;
            txtKlasorYol.BackColor = Color.FromArgb(20, 50, 30);
            DosyalariEsle();
            if (_pozList.Count > 0) GosterPoz(_mevcutIndex >= 0 ? _mevcutIndex : 0);
        }

        // ── ROTA BUTONLARI ────────────────────────────────────────────────

        private void OlusturRotaButonlar()
        {
            pnlRotaButonlar.Controls.Clear();
            for (int i = 0; i < 8; i++)
            {
                int idx = i;
                var btn = new Button
                {
                    Size = new Size(148, 72),
                    Text = ROTA_KOD[i] + "\r\n" + ROTA_ISIM[i],
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(4),
                    Tag = idx
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, ev) => RotaToggle(idx);
                ButonKirmizi(btn);
                _rotaButonlar[i] = btn;
                pnlRotaButonlar.Controls.Add(btn);
            }
        }

        private void ButonYesil(Button b) { b.BackColor = Color.FromArgb(39, 174, 96); b.ForeColor = Color.White; b.FlatAppearance.BorderColor = Color.FromArgb(30, 130, 76); b.FlatAppearance.BorderSize = 2; }
        private void ButonKirmizi(Button b) { b.BackColor = Color.FromArgb(192, 57, 43); b.ForeColor = Color.White; b.FlatAppearance.BorderColor = Color.FromArgb(150, 40, 30); b.FlatAppearance.BorderSize = 2; }

        private void RotaToggle(int idx)
        {
            if (_mevcutIndex < 0 || _mevcutIndex >= _pozList.Count) return;
            string poz = _pozList[_mevcutIndex].PozNo;
            if (!_rotaDurumlar.ContainsKey(poz)) _rotaDurumlar[poz] = new bool[8];
            _varsayilanUygulandi.Add(poz);
            _rotaDurumlar[poz][idx] = !_rotaDurumlar[poz][idx];
            if (_rotaDurumlar[poz][idx]) ButonYesil(_rotaButonlar[idx]);
            else ButonKirmizi(_rotaButonlar[idx]);
            GuncelleRotaLabel(poz);
            GuncelleRotaOzeti(poz);
            ListeyiGuncelle();
        }

        private string RotaKoduOlustur(string pozNo)
        {
            if (!_rotaDurumlar.ContainsKey(pozNo)) return "";
            var sb = new StringBuilder();
            bool[] d = _rotaDurumlar[pozNo];
            for (int i = 0; i < 8; i++) if (d[i]) sb.Append(ROTA_KOD[i]);
            return sb.ToString();
        }

        private void GuncelleRotaLabel(string pozNo)
        {
            string kod = RotaKoduOlustur(pozNo);
            if (string.IsNullOrEmpty(kod)) { lblRotaKodu.Text = "Rota: (seçilmedi)"; return; }
            var isimler = kod.ToCharArray().Select(c => { int i = Array.IndexOf(ROTA_KOD, c.ToString()); return i >= 0 ? ROTA_ISIM[i] : "?"; });
            lblRotaKodu.Text = "Rota: " + kod + "  →  " + string.Join(" + ", isimler);
        }

        private void GuncelleRotaOzeti(string pozNo)
        {
            pnlRotaOzeti.Controls.Clear();
            bool[] dur = _rotaDurumlar.ContainsKey(pozNo) ? _rotaDurumlar[pozNo] : new bool[8];
            var baslik = new Label { Text = "SEÇİLEN\r\nROTALAR", Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = Color.FromArgb(160, 160, 180), Dock = DockStyle.Top, Height = 34, TextAlign = ContentAlignment.MiddleCenter };
            pnlRotaOzeti.Controls.Add(baslik);
            for (int i = 7; i >= 0; i--)
            {
                int idx = i;
                var lbl = new Label
                {
                    Text = ROTA_KOD[i] + "  " + ROTA_ISIM[i],
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    Height = 34,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(1),
                    BackColor = dur[i] ? Color.FromArgb(39, 174, 96) : Color.FromArgb(60, 40, 40),
                    ForeColor = dur[i] ? Color.White : Color.FromArgb(140, 100, 100)
                };
                lbl.Click += (s, ev) => RotaToggle(idx);
                pnlRotaOzeti.Controls.Add(lbl);
            }
            baslik.BringToFront();
        }

        // ── VARSAYILAN ROTA ───────────────────────────────────────────────

        private void VarsayilanRotaUygula(string pozNo)
        {
            if (_varsayilanUygulandi.Contains(pozNo)) return;
            _varsayilanUygulandi.Add(pozNo);
            if (!_rotaDurumlar.ContainsKey(pozNo)) _rotaDurumlar[pozNo] = new bool[8];
            foreach (int idx in VARSAYILAN_ROTA)
                _rotaDurumlar[pozNo][idx] = true;
        }

        // ── DWG / DXF → PNG ───────────────────────────────────────────────

        private string DwgdenPngYap(string kaynak)
        {
            try
            {
                string gecici = Path.Combine(Path.GetTempPath(),
                    "IMK_" + Path.GetFileNameWithoutExtension(kaynak) + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + ".png");
                CadDocument doc;
                string ext = Path.GetExtension(kaynak).ToLower();
                if (ext == ".dxf") using (var r = new DxfReader(kaynak)) doc = r.Read();
                else using (var r = new DwgReader(kaynak)) doc = r.Read();
                var items = new List<RenderItem>();
                var baseTr = new CadTransform();
                foreach (var entity in doc.Entities) CollectEntity(entity, items, baseTr);
                if (items.Count == 0) return null;
                RenderToPng(items, gecici);
                _geciciDosyalar.Add(gecici);
                return gecici;
            }
            catch (Exception ex)
            {
                lblDosyaBilgi.Text = "⚠ DWG okuma hatası: " + ex.Message;
                lblDosyaBilgi.ForeColor = Color.FromArgb(220, 100, 50);
                return null;
            }
        }

        private void CollectEntity(Entity entity, List<RenderItem> list, CadTransform tr)
        {
            if (entity is Insert ins)
            {
                var bt = new CadTransform { X = ins.InsertPoint.X, Y = ins.InsertPoint.Y, ScaleX = ins.XScale, ScaleY = ins.YScale, Rotation = ins.Rotation };
                var newTr = CadTransform.Combine(tr, bt);
                foreach (var sub in ins.Block.Entities) CollectEntity(sub, list, newTr);
            }
            else if (entity is ACadSharp.Entities.Line ln)
                list.Add(new RenderItem { Type = "Line", P1 = tr.Apply(ln.StartPoint.X, ln.StartPoint.Y), P2 = tr.Apply(ln.EndPoint.X, ln.EndPoint.Y) });
            else if (entity is ACadSharp.Entities.Circle ci)
                list.Add(new RenderItem { Type = "Circle", P1 = tr.Apply(ci.Center.X, ci.Center.Y), Radius = (float)(ci.Radius * tr.ScaleX) });
            else if (entity is LwPolyline pl)
            {
                var pts = pl.Vertices.Select(v => tr.Apply(v.Location.X, v.Location.Y)).ToList();
                if (pl.IsClosed && pts.Count > 0) pts.Add(pts[0]);
                list.Add(new RenderItem { Type = "Polyline", Points = pts });
            }
            else if (entity is ACadSharp.Entities.Arc arc)
                list.Add(new RenderItem { Type = "Arc", P1 = tr.Apply(arc.Center.X, arc.Center.Y), Radius = (float)(arc.Radius * tr.ScaleX), StartAngle = (float)(arc.StartAngle + tr.Rotation), EndAngle = (float)(arc.EndAngle + tr.Rotation) });
            else if (entity is TextEntity txt)
                list.Add(new RenderItem { Type = "Text", P1 = tr.Apply(txt.InsertPoint.X, txt.InsertPoint.Y), Text = txt.Value, Size = (float)(txt.Height * tr.ScaleX) });
            else if (entity is MText mt)
                list.Add(new RenderItem { Type = "Text", P1 = tr.Apply(mt.InsertPoint.X, mt.InsertPoint.Y), Text = mt.Value, Size = (float)(mt.Height * tr.ScaleX) });
        }

        private void RenderToPng(List<RenderItem> items, string cikisYolu)
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            void Check(PointF p) { if (p.X < minX) minX = p.X; if (p.X > maxX) maxX = p.X; if (p.Y < minY) minY = p.Y; if (p.Y > maxY) maxY = p.Y; }
            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case "Line": Check(item.P1); Check(item.P2); break;
                    case "Circle": case "Arc": Check(new PointF(item.P1.X - item.Radius, item.P1.Y - item.Radius)); Check(new PointF(item.P1.X + item.Radius, item.P1.Y + item.Radius)); break;
                    case "Polyline": if (item.Points != null) foreach (var p in item.Points) Check(p); break;
                    case "Text": Check(item.P1); break;
                }
            }
            int W = 3508, H = 2480, PAD = 80;
            float dW = maxX - minX; if (dW <= 0) dW = 1;
            float dH = maxY - minY; if (dH <= 0) dH = 1;
            float sc = Math.Min((W - PAD * 2) / dW, (H - PAD * 2) / dH);
            float offX = (W - dW * sc) / 2f, offY = (H - dH * sc) / 2f;
            PointF ToScreen(PointF p) => new PointF((p.X - minX) * sc + offX, H - ((p.Y - minY) * sc + offY));
            using (var bmp = new System.Drawing.Bitmap(W, H))
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                using (var pen = new Pen(Color.Black, 1.5f))
                using (var br = new SolidBrush(Color.Black))
                {
                    foreach (var item in items)
                    {
                        switch (item.Type)
                        {
                            case "Line": g.DrawLine(pen, ToScreen(item.P1), ToScreen(item.P2)); break;
                            case "Circle": { var c = ToScreen(item.P1); float r = item.Radius * sc; g.DrawEllipse(pen, c.X - r, c.Y - r, r * 2, r * 2); break; }
                            case "Polyline": if (item.Points != null && item.Points.Count > 1) for (int i = 0; i < item.Points.Count - 1; i++) g.DrawLine(pen, ToScreen(item.Points[i]), ToScreen(item.Points[i + 1])); break;
                            case "Arc": { var c = ToScreen(item.P1); float r = item.Radius * sc; float s2 = (float)(item.StartAngle * 180 / Math.PI); float en = (float)(item.EndAngle * 180 / Math.PI); float sweep = en - s2; if (sweep < 0) sweep += 360; g.DrawArc(pen, c.X - r, c.Y - r, r * 2, r * 2, -s2, -sweep); break; }
                            case "Text": { float fs = Math.Max(8f, Math.Min(item.Size * sc, 180f)); var pos = ToScreen(item.P1); pos.Y -= fs; g.DrawString(item.Text, new Font("Arial", fs), br, pos); break; }
                        }
                    }
                }
                bmp.Save(cikisYolu, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        // ── EXCEL OKU ─────────────────────────────────────────────────────

        private void ExceldenPozOku()
        {
            _pozList.Clear();
            lbPozlar.Items.Clear();
            var islenen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Open(_erpExcelYolu, false, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                var ws = (Excel.Worksheet)wb.Worksheets[1];
                int last = ws.UsedRange.Rows.Count;

                var pozAdetler = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
                for (int r = 2; r <= last; r++)
                {
                    object bilTurV = ((Excel.Range)ws.Cells[r, COL_BILTUR]).Value2;
                    if (bilTurV == null || bilTurV.ToString().Trim().ToLower() != "madde") continue;
                    object pv = ((Excel.Range)ws.Cells[r, COL_POZNO]).Value2;
                    if (pv == null) continue;
                    string p = pv.ToString().Replace("/", "").Trim();
                    object av = ((Excel.Range)ws.Cells[r, COL_BILMIKTAR]).Value2;
                    double a = 0;
                    if (av != null) double.TryParse(av.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out a);
                    if (pozAdetler.ContainsKey(p)) pozAdetler[p] += a; else pozAdetler[p] = a;
                }

                for (int r = 2; r <= last; r++)
                {
                    object bilTurV = ((Excel.Range)ws.Cells[r, COL_BILTUR]).Value2;
                    if (bilTurV != null && bilTurV.ToString().Trim().ToLower() == "kaynak")
                    {
                        object pv2 = ((Excel.Range)ws.Cells[r, COL_POZNO]).Value2;
                        if (pv2 == null) continue;
                        string temiz2 = pv2.ToString().Replace("/", "").Replace("\\", "").Trim();
                        object bilNoV = ((Excel.Range)ws.Cells[r, COL_BILNO]).Value2;
                        if (bilNoV == null) continue;
                        string islem = bilNoV.ToString().Trim().ToLower();
                        int rotaIdx = Array.IndexOf(ROTA_BILESEN, islem);
                        if (rotaIdx < 0) continue;
                        if (!_rotaDurumlar.ContainsKey(temiz2)) _rotaDurumlar[temiz2] = new bool[8];
                        _rotaDurumlar[temiz2][rotaIdx] = true;
                        _varsayilanUygulandi.Add(temiz2);
                        continue;
                    }

                    object pozV = ((Excel.Range)ws.Cells[r, COL_POZNO]).Value2;
                    if (pozV == null) continue;
                    string orijinal = pozV.ToString().Trim();
                    if (string.IsNullOrEmpty(orijinal)) continue;
                    string temiz = orijinal.Replace("/", "").Replace("\\", "").Trim();
                    if (islenen.Contains(temiz)) continue;
                    islenen.Add(temiz);

                    _pozList.Add(new PozBilgi
                    {
                        PozNo = temiz,
                        PozNoOrijinal = orijinal,
                        ExcelSatir = r,
                        UretilecekAdet = pozAdetler.ContainsKey(temiz) ? pozAdetler[temiz] : 0,
                        ProjeNo = ((Excel.Range)ws.Cells[r, COL_PROJENO]).Value2,
                        AnaPozNo = ((Excel.Range)ws.Cells[r, COL_ANAPOZNO]).Value2,
                        PozMiktar = ((Excel.Range)ws.Cells[r, COL_POZMIKTAR]).Value2,
                        PozAgirlik = ((Excel.Range)ws.Cells[r, COL_POZAGIRLIK]).Value2
                    });
                }

                foreach (var poz in _pozList)
                    if (!_varsayilanUygulandi.Contains(poz.PozNo))
                        VarsayilanRotaUygula(poz.PozNo);
            }
            catch (Exception ex) { MessageBox.Show("Excel okuma hatası: " + ex.Message); }
            finally
            {
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
            }

            _filtreliPozlar = _pozList.ToList();
            lblIlerleme.Text = $"0 / {_pozList.Count} poz";
            ListeyiGuncelle();
        }

        // ── DOSYA EŞLEŞTİRME ─────────────────────────────────────────────

        private void DosyalariEsle()
        {
            if (string.IsNullOrEmpty(_imalatKlasor)) return;
            var tumDosyalar = new List<string>();
            try { foreach (string ext in new[] { "*.dwg", "*.dxf", "*.pdf" }) tumDosyalar.AddRange(Directory.GetFiles(_imalatKlasor, ext, SearchOption.AllDirectories)); } catch { }
            foreach (var poz in _pozList)
            {
                string aranan = Temizle(poz.PozNo);
                poz.DwgYolu = null; poz.PdfYolu = null; poz.GoruntuYolu = null;
                foreach (var dosya in tumDosyalar)
                {
                    string isim = Temizle(Path.GetFileNameWithoutExtension(dosya));
                    string ext = Path.GetExtension(dosya).ToLower();
                    if ((ext == ".dwg" || ext == ".dxf") && isim == aranan && poz.DwgYolu == null) poz.DwgYolu = dosya;
                    if (ext == ".pdf" && isim == aranan && poz.PdfYolu == null) poz.PdfYolu = dosya;
                }
                if (poz.PdfYolu == null && poz.DwgYolu != null) { string yan = Path.ChangeExtension(poz.DwgYolu, ".pdf"); if (File.Exists(yan)) poz.PdfYolu = yan; }
            }
            ListeyiGuncelle();
        }

        private static string Temizle(string s) =>
            s.Replace("/", "").Replace("\\", "").Replace("-", "").Replace("_", "").Replace(" ", "").ToUpper();

        // ── POZ GÖSTER ────────────────────────────────────────────────────

        private void GosterPoz(int index)
        {
            if (index < 0 || index >= _pozList.Count) return;
            _mevcutIndex = index;
            var poz = _pozList[index];

            VarsayilanRotaUygula(poz.PozNo);

            for (int i = 0; i < _filtreliPozlar.Count; i++)
                if (_filtreliPozlar[i].PozNo == poz.PozNo) { lbPozlar.SelectedIndex = i; break; }

            lblPozAdi.Text = $"[{index + 1}/{_pozList.Count}]   POZ: {poz.PozNo}";
            lblIlerleme.Text = $"{index + 1} / {_pozList.Count} poz";
            lblAdet.Text = poz.UretilecekAdet > 0 ? $"ÜRETİLECEK: {poz.UretilecekAdet:0.##} ADET" : "Adet bilgisi yok";

            bool[] dur = _rotaDurumlar.ContainsKey(poz.PozNo) ? _rotaDurumlar[poz.PozNo] : new bool[8];
            for (int i = 0; i < 8; i++) { if (dur[i]) ButonYesil(_rotaButonlar[i]); else ButonKirmizi(_rotaButonlar[i]); }
            GuncelleRotaLabel(poz.PozNo);
            GuncelleRotaOzeti(poz.PozNo);

            if (poz.PdfYolu != null && File.Exists(poz.PdfYolu))
            { lblDosyaBilgi.Text = "📄 PDF  →  " + Path.GetFileName(poz.PdfYolu); lblDosyaBilgi.ForeColor = Color.FromArgb(160, 220, 160); try { webBrowser.Navigate(new Uri(poz.PdfYolu)); } catch { } }
            else if (poz.GoruntuYolu != null && File.Exists(poz.GoruntuYolu))
                GosterGoruntu(poz, poz.GoruntuYolu);
            else if (poz.DwgYolu != null)
            {
                string uzanti = Path.GetExtension(poz.DwgYolu).ToUpper().TrimStart('.');
                lblDosyaBilgi.Text = $"⏳ {uzanti} → PNG dönüştürülüyor..."; lblDosyaBilgi.ForeColor = Color.FromArgb(200, 160, 0);
                Application.DoEvents();
                string png = DwgdenPngYap(poz.DwgYolu);
                if (png != null) { poz.GoruntuYolu = png; GosterGoruntu(poz, png); }
                else GosterHtml(poz, $"{uzanti} dosyası bulundu fakat görüntüye çevrilemedi.");
            }
            else GosterHtml(poz, "Eşleşen dosya bulunamadı.<br>Klasör yolunu kontrol edin.");

            ListeyiGuncelle();
        }

        private void GosterGoruntu(PozBilgi poz, string pngYolu)
        {
            string uzanti = poz.DwgYolu != null ? Path.GetExtension(poz.DwgYolu).ToUpper().TrimStart('.') : "DWG";
            lblDosyaBilgi.Text = $"🔧 {uzanti} → PNG  |  {Path.GetFileName(pngYolu)}"; lblDosyaBilgi.ForeColor = Color.FromArgb(160, 200, 255);
            string base64;
            try { base64 = Convert.ToBase64String(File.ReadAllBytes(pngYolu)); } catch { GosterHtml(poz, "Görüntü yüklenemedi."); return; }
            webBrowser.DocumentText = $@"<!DOCTYPE html><html><head><style>*{{margin:0;padding:0;box-sizing:border-box;}}body{{background:#1a1a20;display:flex;align-items:center;justify-content:center;height:100vh;}}img{{max-width:100%;max-height:100vh;object-fit:contain;}}</style></head><body><img src='data:image/png;base64,{base64}'/></body></html>";
        }

        private void GosterHtml(PozBilgi poz, string mesaj)
        {
            string dosyaBilgi = poz.DwgYolu != null ? $"<code style='color:#7fb3d3'>{poz.DwgYolu}</code>" : "<span style='color:#e74c3c'>Klasörde eşleşen dosya bulunamadı</span>";
            lblDosyaBilgi.Text = "⚠  " + mesaj.Replace("<br>", " "); lblDosyaBilgi.ForeColor = Color.FromArgb(220, 150, 60);
            webBrowser.DocumentText = $@"<!DOCTYPE html><html><body style='font-family:Segoe UI;background:#1a1a20;color:#ccc;padding:60px;'><h2 style='color:#e67e22;margin-bottom:10px'>⚠  POZ: {poz.PozNo}</h2><p style='margin-bottom:8px'>{mesaj}</p><p>{dosyaBilgi}</p></body></html>";
        }

        private void ListeyiGuncelle()
        {
            string araTxt = (_txtArama != null && _txtArama.Text != "🔍 Poz ara...") ? _txtArama.Text.Trim() : "";
            _filtreliPozlar = string.IsNullOrEmpty(araTxt) ? _pozList.ToList() : _pozList.Where(p => p.PozNo.ToUpper().Contains(araTxt.ToUpper())).ToList();
            int sel = lbPozlar.SelectedIndex;
            lbPozlar.BeginUpdate();
            lbPozlar.Items.Clear();
            foreach (var p in _filtreliPozlar)
            {
                string ikon = p.PdfYolu != null ? "📄" : p.GoruntuYolu != null ? "🖼" : p.DwgYolu != null ? "🔧" : "❌";
                string rota = RotaKoduOlustur(p.PozNo);
                string adet = p.UretilecekAdet > 0 ? $"×{p.UretilecekAdet:0.##}" : "";
                lbPozlar.Items.Add($"{ikon} {p.PozNo,-16} {adet,-6} [{(string.IsNullOrEmpty(rota) ? "—" : rota)}]");
            }
            lbPozlar.EndUpdate();
            if (sel >= 0 && sel < lbPozlar.Items.Count) lbPozlar.SelectedIndex = sel;
        }

        // ── OWNER DRAW — poz listesi rota rengine göre ────────────────────

        private void lbPozlar_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= _filtreliPozlar.Count) return;
            var poz = _filtreliPozlar[e.Index];
            bool[] rota = _rotaDurumlar.ContainsKey(poz.PozNo) ? _rotaDurumlar[poz.PozNo] : new bool[8];
            int rotaSayisi = rota.Count(x => x);
            bool selected = (e.State & DrawItemState.Selected) != 0;

            Color bg, fg;
            if (rotaSayisi == 0)
            { bg = Color.FromArgb(55, 28, 28); fg = Color.FromArgb(220, 140, 140); }
            else if (rota[0] && rota[2] && rota[7])
            { bg = Color.FromArgb(22, 50, 28); fg = Color.FromArgb(120, 220, 130); }
            else
            { bg = Color.FromArgb(52, 46, 18); fg = Color.FromArgb(230, 205, 110); }

            using (var bgBrush = new SolidBrush(bg))
                e.Graphics.FillRectangle(bgBrush, e.Bounds);

            if (selected)
            {
                using (var selBrush = new SolidBrush(Color.White))
                    e.Graphics.FillRectangle(selBrush, e.Bounds.X, e.Bounds.Y, 4, e.Bounds.Height);
                fg = Color.White;
            }

            string text = lbPozlar.Items[e.Index]?.ToString() ?? "";
            using (var fgBrush = new SolidBrush(fg))
                e.Graphics.DrawString(text, e.Font ?? lbPozlar.Font, fgBrush,
                    new RectangleF(e.Bounds.X + 8, e.Bounds.Y + 2, e.Bounds.Width - 12, e.Bounds.Height - 2));
        }

        // ── YAZDIR ────────────────────────────────────────────────────────

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            if (_pozList.Count == 0) { MessageBox.Show("Poz listesi boş."); return; }
            lblDosyaBilgi.Text = "⏳ Tüm pozlar hazırlanıyor..."; lblDosyaBilgi.ForeColor = Color.FromArgb(200, 200, 100);
            Application.DoEvents();
            for (int i = 0; i < _pozList.Count; i++)
            {
                var poz = _pozList[i];
                if (poz.GoruntuYolu != null && File.Exists(poz.GoruntuYolu)) continue;
                if (poz.DwgYolu == null) continue;
                lblDosyaBilgi.Text = $"⏳ [{i + 1}/{_pozList.Count}] {poz.PozNo} dönüştürülüyor..."; Application.DoEvents();
                string png = DwgdenPngYap(poz.DwgYolu);
                if (png != null) poz.GoruntuYolu = png;
            }
            var dr = MessageBox.Show(
                $"Yazdırma kapsamını seçin:\n\n[Evet]   →  Yalnızca mevcut poz  ({(_mevcutIndex >= 0 ? _pozList[_mevcutIndex].PozNo : "—")})\n[Hayır]  →  Tüm {_pozList.Count} poz",
                "İmalat Kartlarını Yazdır", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Cancel) return;
            var liste = dr == DialogResult.Yes
                ? _pozList.Where((p, i) => i == _mevcutIndex).ToList()
                : _pozList.ToList();
            YazdirPozlar(liste);
        }

        private void YazdirPozlar(List<PozBilgi> liste)
        {
            using (var pd = new PrintDialog())
            {
                pd.UseEXDialog = true;
                if (pd.ShowDialog() != DialogResult.OK) return;
                using (var doc = new System.Drawing.Printing.PrintDocument())
                {
                    doc.PrinterSettings = pd.PrinterSettings;
                    doc.DefaultPageSettings.Landscape = true;
                    int idx = 0;
                    doc.PrintPage += (s, ev) =>
                    {
                        var poz = liste[idx];
                        var g = ev.Graphics;
                        var sayfa = ev.PageBounds;
                        g.Clear(Color.White);

                        var headerRect = new Rectangle(sayfa.X, sayfa.Y, sayfa.Width, 52);
                        using (var headerBr = new SolidBrush(Color.FromArgb(240, 242, 246))) g.FillRectangle(headerBr, headerRect);
                        using (var borderPen = new Pen(Color.FromArgb(160, 170, 190), 1.2f)) g.DrawRectangle(borderPen, headerRect);

                        string rotaKod = RotaKoduOlustur(poz.PozNo);
                        string adetStr = poz.UretilecekAdet > 0 ? $"ÜRETİLECEK: {poz.UretilecekAdet:0.##} ADET" : "";

                        using (var fn = new Font("Segoe UI", 15f, FontStyle.Bold))
                        using (var darkBr = new SolidBrush(Color.FromArgb(30, 40, 60)))
                            g.DrawString($"POZ: {poz.PozNo}", fn, darkBr, new PointF(sayfa.X + 14, sayfa.Y + 10));

                        if (!string.IsNullOrEmpty(adetStr))
                            using (var fn2 = new Font("Segoe UI", 12f, FontStyle.Bold))
                            using (var adetBr = new SolidBrush(Color.FromArgb(20, 100, 50)))
                                g.DrawString(adetStr, fn2, adetBr, new PointF(sayfa.X + sayfa.Width / 2f - 120, sayfa.Y + 14));

                        if (!string.IsNullOrEmpty(rotaKod))
                            using (var fn3 = new Font("Segoe UI", 11f, FontStyle.Bold))
                            using (var rotaBr = new SolidBrush(Color.FromArgb(40, 80, 40)))
                                g.DrawString($"ROTA: {rotaKod}", fn3, rotaBr, new PointF(sayfa.X + sayfa.Width - 220, sayfa.Y + 16));

                        using (var sepPen = new Pen(Color.FromArgb(180, 185, 200), 0.8f))
                            g.DrawLine(sepPen, sayfa.X, sayfa.Y + 52, sayfa.Right, sayfa.Y + 52);

                        var imgRect = new Rectangle(sayfa.X + 8, sayfa.Y + 58, sayfa.Width - 16, sayfa.Height - 82);
                        string gorYolu = poz.PdfYolu ?? poz.GoruntuYolu;
                        bool goruntu = false;
                        if (gorYolu != null && File.Exists(gorYolu) && !gorYolu.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                using (var img = Image.FromFile(gorYolu))
                                {
                                    float rW = (float)imgRect.Width / img.Width, rH = (float)imgRect.Height / img.Height;
                                    float rt = Math.Min(rW, rH);
                                    int iW = (int)(img.Width * rt), iH = (int)(img.Height * rt);
                                    int iX = imgRect.X + (imgRect.Width - iW) / 2, iY = imgRect.Y + (imgRect.Height - iH) / 2;
                                    g.DrawImage(img, iX, iY, iW, iH); goruntu = true;
                                }
                            }
                            catch { }
                        }
                        if (!goruntu)
                            using (var fn4 = new Font("Segoe UI", 16f))
                            using (var grBr = new SolidBrush(Color.FromArgb(160, 160, 170)))
                                g.DrawString("Görüntü mevcut değil", fn4, grBr, new PointF(imgRect.X + 40, imgRect.Y + 40));

                        if (!string.IsNullOrEmpty(rotaKod))
                        {
                            var rotaIsimler = rotaKod.ToCharArray().Select(c => { int ii = Array.IndexOf(ROTA_KOD, c.ToString()); return ii >= 0 ? ROTA_ISIM[ii] : c.ToString(); });
                            string rotaTxt = "Rotalar: " + string.Join("  →  ", rotaIsimler);
                            var altRect = new Rectangle(sayfa.X, sayfa.Bottom - 24, sayfa.Width, 24);
                            using (var altBr = new SolidBrush(Color.FromArgb(248, 249, 251))) g.FillRectangle(altBr, altRect);
                            using (var sepPen = new Pen(Color.FromArgb(200, 205, 215), 0.8f)) g.DrawLine(sepPen, sayfa.X, sayfa.Bottom - 24, sayfa.Right, sayfa.Bottom - 24);
                            using (var fns = new Font("Segoe UI", 8.5f))
                            using (var dbr = new SolidBrush(Color.FromArgb(70, 80, 100)))
                                g.DrawString(rotaTxt, fns, dbr, new PointF(sayfa.X + 14, sayfa.Bottom - 20));
                        }

                        idx++;
                        ev.HasMorePages = idx < liste.Count;
                    };
                    try { doc.Print(); }
                    catch (Exception ex) { MessageBox.Show("Yazdırma hatası: " + ex.Message); }
                }
            }
        }

        // ── OLAYLAR ───────────────────────────────────────────────────────

        private void btnGeri_Click(object sender, EventArgs e)
        { if (_mevcutIndex > 0) GosterPoz(_mevcutIndex - 1); }

        private void btnIleri_Click(object sender, EventArgs e)
        { if (_mevcutIndex < _pozList.Count - 1) GosterPoz(_mevcutIndex + 1); }

        private void lbPozlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = lbPozlar.SelectedIndex;
            if (sel < 0 || sel >= _filtreliPozlar.Count) return;
            var poz = _filtreliPozlar[sel];
            int gercekIdx = _pozList.IndexOf(poz);
            if (gercekIdx >= 0 && gercekIdx != _mevcutIndex) GosterPoz(gercekIdx);
        }

        private void btnTumunuSec_Click(object sender, EventArgs e)
        {
            if (_mevcutIndex < 0) return;
            string poz = _pozList[_mevcutIndex].PozNo;
            _rotaDurumlar[poz] = new bool[] { true, true, true, true, true, true, true, true };
            _varsayilanUygulandi.Add(poz);
            for (int i = 0; i < 8; i++) ButonYesil(_rotaButonlar[i]);
            GuncelleRotaLabel(poz); GuncelleRotaOzeti(poz); ListeyiGuncelle();
        }

        private void btnHepsiniTemizle_Click(object sender, EventArgs e)
        {
            if (_mevcutIndex < 0) return;
            string poz = _pozList[_mevcutIndex].PozNo;
            _rotaDurumlar[poz] = new bool[8];
            _varsayilanUygulandi.Add(poz);
            for (int i = 0; i < 8; i++) ButonKirmizi(_rotaButonlar[i]);
            GuncelleRotaLabel(poz); GuncelleRotaOzeti(poz); ListeyiGuncelle();
        }

        private void btnAcDosya_Click(object sender, EventArgs e)
        {
            if (_mevcutIndex < 0 || _mevcutIndex >= _pozList.Count) return;
            var poz = _pozList[_mevcutIndex];
            string d = poz.PdfYolu ?? poz.DwgYolu;
            if (d != null && File.Exists(d)) try { System.Diagnostics.Process.Start(d); } catch (Exception ex) { MessageBox.Show("Açılamadı: " + ex.Message); }
            else MessageBox.Show("Dosya bulunamadı.");
        }

        // ── KAYDET & ÇIKIŞ ────────────────────────────────────────────────

        private void btnKaydetCikis_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                var nr = new NETRESOURCE { dwType = 1, lpRemoteName = NET_PATH };
                WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0);
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Open(_erpExcelYolu, false, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                var ws = (Excel.Worksheet)wb.Worksheets[1];

                var pozNoSet = new HashSet<string>(_pozList.Select(p => p.PozNo), StringComparer.OrdinalIgnoreCase);

                int lastRow = ws.UsedRange.Rows.Count;
                for (int r = lastRow; r >= 2; r--)
                {
                    object bilTurV = ((Excel.Range)ws.Cells[r, COL_BILTUR]).Value2;
                    if (bilTurV == null || bilTurV.ToString().Trim().ToLower() != "kaynak") continue;
                    object pozV = ((Excel.Range)ws.Cells[r, COL_POZNO]).Value2;
                    if (pozV == null) continue;
                    string pozNo = pozV.ToString().Replace("/", "").Trim();
                    if (!pozNoSet.Contains(pozNo)) continue;
                    ((Excel.Range)ws.Rows[r]).Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                }

                lastRow = ws.UsedRange.Rows.Count;
                var pozSonSatir = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int r = 2; r <= lastRow; r++)
                {
                    object pozV = ((Excel.Range)ws.Cells[r, COL_POZNO]).Value2;
                    if (pozV == null) continue;
                    string pozNo = pozV.ToString().Replace("/", "").Trim();
                    if (pozNoSet.Contains(pozNo)) pozSonSatir[pozNo] = r;
                }

                var siraliPozlar = _pozList
                    .Where(p => pozSonSatir.ContainsKey(p.PozNo))
                    .OrderByDescending(p => pozSonSatir[p.PozNo])
                    .ToList();

                foreach (var poz in siraliPozlar)
                {
                    bool[] rotaDur = _rotaDurumlar.ContainsKey(poz.PozNo) ? _rotaDurumlar[poz.PozNo] : new bool[8];
                    var secilenler = new List<int>();
                    for (int i = 0; i < 8; i++) if (rotaDur[i]) secilenler.Add(i);
                    if (secilenler.Count == 0) continue;

                    int insertAfter = pozSonSatir[poz.PozNo];
                    for (int i = secilenler.Count - 1; i >= 0; i--)
                    {
                        int rotaIdx = secilenler[i];
                        int yeniSatir = insertAfter + 1;
                        ((Excel.Range)ws.Rows[yeniSatir]).Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                        ((Excel.Range)ws.Cells[yeniSatir, COL_PROJENO]).Value2 = poz.ProjeNo;
                        ((Excel.Range)ws.Cells[yeniSatir, COL_POZNO]).Value2 = poz.PozNoOrijinal;
                        ((Excel.Range)ws.Cells[yeniSatir, COL_POZACIKLAMA]).Value2 = poz.PozNoOrijinal;
                        ((Excel.Range)ws.Cells[yeniSatir, COL_ANAPOZNO]).Value2 = poz.AnaPozNo;
                        ((Excel.Range)ws.Cells[yeniSatir, COL_POZMIKTAR]).Value2 = poz.PozMiktar;
                        ((Excel.Range)ws.Cells[yeniSatir, COL_POZAGIRLIK]).Value2 = poz.PozAgirlik;
                        ((Excel.Range)ws.Cells[yeniSatir, COL_BILTUR]).Value2 = "Kaynak";
                        ((Excel.Range)ws.Cells[yeniSatir, COL_BILNO]).Value2 = ROTA_BILESEN[rotaIdx];
                        ((Excel.Range)ws.Cells[yeniSatir, COL_BILMIKTAR]).Value2 = 1;
                        ((Excel.Range)ws.Cells[yeniSatir, COL_ISLEMSIRA]).Value2 = rotaIdx + 1;
                    }
                }

                wb.Save();

                LogHelper.Yaz(_erpExcelYolu, "ROTA_KAYIT",
                    $"{_pozList.Count} poz kaydedildi. " +
                    string.Join(" | ", _pozList.Select(p => $"{p.PozNo}=[{RotaKoduOlustur(p.PozNo)}]")));

                MessageBox.Show($"{_pozList.Count} poz için rota satırları kaydedildi.\n\n{_erpExcelYolu}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { MessageBox.Show("Excel yazma hatası: " + ex.Message); }
            finally
            {
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}