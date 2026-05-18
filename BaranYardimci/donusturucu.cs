using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace BaranYardimci
{
    public partial class Donusturucu : Form
    {
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(ref NETRESOURCE nr, string password, string username, int flags);
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);
        [StructLayout(LayoutKind.Sequential)]
        private struct NETRESOURCE { public int dwScope, dwType, dwDisplayType, dwUsage; public string lpLocalName, lpRemoteName, lpComment, lpProvider; }

        const string NET_PATH = @"\\192.168.2.10\erp\Hazir_Exceller";
        const string NET_USER = "admin";
        const string NET_PASS = "asd123asd";

        class HamSatir { public string DosyaId, DosyaAdi, MontajNo, ParcaNo, ParcaProfil, Kalite; public double MontajAdeti, BirimAdet, Uzunluk, Agirlik; }
        class MalzemeOzet { public string Profil, Kalite; public double ToplamAdet, ToplamUzunluk, ToplamAgirlik; }
        class KesimParcasi { public string ParcaNo; public double Uzunluk; public int Adet; }
        class StokBar
        {
            public int BarNo; public double StokBoyu;
            public List<KesimParcasi> Dilimler = new List<KesimParcasi>();
            const double KERF = 3.0;
            public double Kullanilan { get { double t = 0; int n = 0; foreach (var d in Dilimler) { t += d.Uzunluk * d.Adet; n += d.Adet; } if (n > 1) t += (n - 1) * KERF; return t; } }
            public double Fire => Math.Max(0, StokBoyu - Kullanilan);
            public double FirePct => StokBoyu > 0 ? Fire / StokBoyu * 100.0 : 0;
        }
        class ProfilSonuc
        {
            public string Profil, Kalite, OneriNot;
            public double KgPerMetre, ToplamKg, ToplamNetMetre;
            public int Bar6Adet, Bar12Adet, OnerilenAdet;
            public double Bar6ToplamMetre, Bar6ToplamKg, Bar6FirePct;
            public double Bar12ToplamMetre, Bar12ToplamKg, Bar12FirePct;
            public double OnerilenBoy, OnerilenMetre, OnerilenKg, OnerilenFirePct;
            public List<StokBar> OnerilenBarlar = new List<StokBar>();
        }
        class HammaddeItem { public string No, Adi; }

        class CivataSatir
        {
            public string DosyaId, BoltListAdi, Dia, Standard, Grade;
            public int Length, Quantity;
            public double BirimAgirlik, ToplamAgirlik;
            public string PozNo => Dia + "x" + Length;
            public string Tanim => Dia + " x " + Length + "  " + Standard + "  " + Grade;
            public string EslesKey => PozNo + "|" + Standard + "|" + Grade;
        }

        static int RenkBaslik = Ole(31, 73, 125); static int RenkMavi = Ole(68, 114, 196);
        static int RenkAcik = Ole(222, 235, 247); static int RenkYesil = Ole(198, 239, 206);
        static int RenkSari = Ole(255, 235, 156); static int RenkKirmizi = Ole(255, 199, 206);
        static int RenkBeyaz = Ole(255, 255, 255); static int RenkGri = Ole(242, 242, 242);
        static int Ole(int r, int g, int b) => ColorTranslator.ToOle(Color.FromArgb(r, g, b));

        static readonly double[] STOK_ADAYLARI = { 6000, 7000, 7500, 8000, 9000, 10000, 11000, 12000 };
        const double KERF_MM = 3.0;

        private List<HamSatir> _tumVeriler = new List<HamSatir>();
        private string _imalatKartlariKlasoru = "";
        private HashSet<string> _erpAktarimYapilan = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _rotaKaydedilen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _civataEklendi = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> _erpExcelYollari = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private int _sagKlikSatir = -1;
        private List<string> _favoriler = new List<string>();
        private string _sonKaydedilenExcel = "";
        private string _projeNo = ""; // ← Proje No kalıcı saklanır

        private ToolStripMenuItem mnuExceliGoster;
        private ToolStripMenuItem mnuCivataYukle;
        private ToolStripMenuItem mnuMalzemeDegistir;
        private Panel pnlLegend;
        private Button btnExceliAc;
        private Button btnTumExcel;
        private Button btnSil; // ← Seçileni Sil

        public Donusturucu()
        {
            InitializeComponent();
            KontrolleriOlustur();
        }

        private void KontrolleriOlustur()
        {
            if (!dgvDosyalar.Columns.Contains("colDurum"))
                dgvDosyalar.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "colDurum", HeaderText = "Durum", FillWeight = 60f, ReadOnly = true });

            // ── Renk sorununu köklü çöz: dgv'nin kendi default stilini nötr yap ──
            dgvDosyalar.DefaultCellStyle.BackColor = Color.White;
            dgvDosyalar.DefaultCellStyle.ForeColor = Color.Black;
            dgvDosyalar.DefaultCellStyle.SelectionBackColor = Color.FromArgb(180, 180, 180);
            dgvDosyalar.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDosyalar.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvDosyalar.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dgvDosyalar.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(180, 180, 180);
            dgvDosyalar.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgvDosyalar.CellFormatting -= dgvDosyalar_CellFormatting;
            dgvDosyalar.CellFormatting += dgvDosyalar_CellFormatting;
            dgvDosyalar.MouseUp -= dgvDosyalar_MouseUp;
            dgvDosyalar.MouseUp += dgvDosyalar_MouseUp;

            if (pnlDurumBar == null)
            {
                pnlDurumBar = new Panel { Dock = DockStyle.Bottom, Height = 36, BackColor = Color.FromArgb(230, 230, 235), Padding = new Padding(8, 0, 0, 0) };
                lblDurumIkon = new Label { Dock = DockStyle.Left, Width = 34, Font = new Font("Segoe UI", 13f), TextAlign = ContentAlignment.MiddleCenter };
                lblDurumMetin = new Label { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft };
                pnlDurumBar.Controls.Add(lblDurumMetin);
                pnlDurumBar.Controls.Add(lblDurumIkon);
                this.Controls.Add(pnlDurumBar);
            }

            try { pnlSag.Width = 120; } catch { }

            pnlLegend = new Panel { Dock = DockStyle.Bottom, Height = 112, BackColor = Color.FromArgb(245, 245, 250), Padding = new Padding(8, 6, 8, 4) };
            var legendBaslik = new Label { Text = "RENK KODLARI", Dock = DockStyle.Top, Height = 20, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 60, 80), TextAlign = ContentAlignment.MiddleLeft };
            var legendFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = false, BackColor = Color.Transparent };
            var legendSatirlar = new[]
            {
                (Color.FromArgb(255,180,180), Color.FromArgb(120,0,0),   "● Kırmızı  —  Dosya yüklendi, ERP aktarımı bekleniyor"),
                (Color.FromArgb(255,243,180), Color.FromArgb(110,80,0),  "● Sarı      —  ERP Exceli oluşturuldu, rota bekleniyor"),
                (Color.FromArgb(180,210,255), Color.FromArgb(0,50,130),  "● Mavi      —  Rotalar girildi, civata bekleniyor"),
                (Color.FromArgb(180,240,200), Color.FromArgb(0,90,0),    "● Yeşil     —  Tüm işlemler tamamlandı"),
            };
            foreach (var (bg, fg, txt) in legendSatirlar)
            {
                var lbl = new Label { Text = txt, AutoSize = false, Width = 480, Height = 20, Font = new Font("Segoe UI", 8.5f), ForeColor = fg, BackColor = bg, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(4, 0, 0, 0), Margin = new Padding(0, 1, 0, 1) };
                legendFlow.Controls.Add(lbl);
            }
            pnlLegend.Controls.Add(legendFlow);
            pnlLegend.Controls.Add(legendBaslik);
            try { pnlSonucButonlar2.Controls.Add(pnlLegend); pnlLegend.BringToFront(); } catch { }
            try { if (grpSonuc != null && grpSonuc.Parent != null) { grpSonuc.Parent.Controls.Add(pnlLegend); pnlLegend.BringToFront(); } } catch { }

            // ─── EK BUTONLAR ───────────────────────────────────────────
            // pnlSonucButonlar paneline eklenecek ek butonlar
            // Designer'da zaten var olan butonlar: Hesapla, Malzeme İhtiyacı, ERP Aktarım
            // Bunların arasına karışmadan yeni butonları sağa yerleştireceğiz

            if (pnlSonucButonlar != null)
            {
                // 1) SEÇİLENİ SİL - En solda
                btnSil = new Button
                {
                    Text = "🗑  Seçileni Sil",
                    Size = new Size(140, 48),
                    Location = new Point(8, 8),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(160, 40, 40),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left
                };
                btnSil.FlatAppearance.BorderSize = 0;
                btnSil.Click += btnSil_Click;
                pnlSonucButonlar.Controls.Add(btnSil);

                // 2) EXCELİ AÇ - Sağ tarafta
                btnExceliAc = new Button
                {
                    Text = "📂  Exceli Aç",
                    Size = new Size(150, 48),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(0, 120, 180),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnExceliAc.FlatAppearance.BorderSize = 0;
                btnExceliAc.Click += btnExceliAc_Click;
                pnlSonucButonlar.Controls.Add(btnExceliAc);

                // 3) TÜM EXCEL OLUŞTUR - En sağda
                btnTumExcel = new Button
                {
                    Text = "📋  Tüm Excel Oluştur",
                    Size = new Size(190, 48),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(100, 60, 160),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnTumExcel.FlatAppearance.BorderSize = 0;
                btnTumExcel.Click += btnTumExcel_Click;
                pnlSonucButonlar.Controls.Add(btnTumExcel);

                // Resize event'ine bağla - sağ butonların pozisyonlarını ayarlamak için
                pnlSonucButonlar.Resize -= pnlSonucButonlar_Resize;
                pnlSonucButonlar.Resize += pnlSonucButonlar_Resize;

                // İlk yerleşimi yap
                pnlSonucButonlar_Resize(pnlSonucButonlar, EventArgs.Empty);
            }

            if (ctxDosya == null) ctxDosya = new ContextMenuStrip { Font = new Font("Segoe UI", 10f) };
            ctxDosya.Items.Clear();

            if (mnuRotaGir == null) mnuRotaGir = new ToolStripMenuItem();
            if (mnuDosyaSil == null) mnuDosyaSil = new ToolStripMenuItem();
            mnuExceliGoster = new ToolStripMenuItem("📊  Exceli Göster");
            mnuCivataYukle = new ToolStripMenuItem("🔩  Civata Listelerini Yükle");
            mnuMalzemeDegistir = new ToolStripMenuItem("🔄  Malzeme Değiştir");

            mnuRotaGir.Text = "✏  Rota Gir  —  İmalat Kartlarını Bas";
            mnuRotaGir.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            mnuDosyaSil.Text = "🗑  Listeden Çıkar";
            mnuDosyaSil.ForeColor = Color.FromArgb(180, 40, 40);
            mnuMalzemeDegistir.ForeColor = Color.FromArgb(0, 130, 160);

            ctxDosya.Items.AddRange(new ToolStripItem[]
            {
                mnuRotaGir,          new ToolStripSeparator(),
                mnuExceliGoster,     new ToolStripSeparator(),
                mnuMalzemeDegistir,  new ToolStripSeparator(),
                mnuCivataYukle,      new ToolStripSeparator(),
                mnuDosyaSil
            });

            mnuRotaGir.Click -= mnuRotaGir_Click; mnuRotaGir.Click += mnuRotaGir_Click;
            mnuExceliGoster.Click -= mnuExceliGoster_Click; mnuExceliGoster.Click += mnuExceliGoster_Click;
            mnuMalzemeDegistir.Click -= mnuMalzemeDegistir_Click; mnuMalzemeDegistir.Click += mnuMalzemeDegistir_Click;
            mnuCivataYukle.Click -= mnuCivataYukle_Click; mnuCivataYukle.Click += mnuCivataYukle_Click;
            mnuDosyaSil.Click -= mnuDosyaSil_Click; mnuDosyaSil.Click += mnuDosyaSil_Click;

            if (timerSaat != null)
            {
                timerSaat.Interval = 1000;
                timerSaat.Tick -= timerSaat_Tick; timerSaat.Tick += timerSaat_Tick;
                timerSaat.Start();
            }

            FavorileriYukle();
            FavorileriListeyeYaz();
        }

        private void timerSaat_Tick(object sender, EventArgs e)
        { try { lblBaslik.Text = "    BARAN ÇELİK  —  " + DateTime.Now.ToString("HH:mm:ss"); } catch { } }

        private void lbFavoriler_DoubleClick(object sender, EventArgs e)
        {
            var lb = sender as ListBox; if (lb == null) return;
            int sel = lb.SelectedIndex; if (sel < 0 || sel >= _favoriler.Count) return;
            string yol = _favoriler[sel]; if (!File.Exists(yol)) { MessageBox.Show("Dosya bulunamadı:\n" + yol); return; }
            var frm = new FrmImalatKartiRota(yol); frm.StartPosition = FormStartPosition.CenterScreen; frm.Show();
        }

        private void lbGecmis_DoubleClick(object sender, EventArgs e)
        {
            var lb = sender as ListBox; if (lb == null || lb.SelectedItem == null) return;
            string yol = lb.SelectedItem.ToString(); if (!File.Exists(yol)) { MessageBox.Show("Dosya bulunamadı:\n" + yol); return; }
            try { System.Diagnostics.Process.Start(yol); } catch { }
        }

        private void btnFavEkle_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir >= 0 && _sagKlikSatir < dgvDosyalar.Rows.Count)
            {
                string yol = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
                if (_erpExcelYollari.ContainsKey(yol))
                {
                    string excel = _erpExcelYollari[yol];
                    if (!_favoriler.Contains(excel, StringComparer.OrdinalIgnoreCase))
                    { _favoriler.Add(excel); FavorileriKaydet(); FavorileriListeyeYaz(); }
                }
            }
        }

        private void btnFavSil_Click(object sender, EventArgs e)
        {
            try
            {
                var lb = this.Controls.Find("lbFavoriler", true).FirstOrDefault() as ListBox;
                if (lb == null) return;
                int sel = lb.SelectedIndex; if (sel < 0 || sel >= _favoriler.Count) return;
                _favoriler.RemoveAt(sel); FavorileriKaydet(); FavorileriListeyeYaz();
            }
            catch { }
        }

        private void btnGecmisSil_Click(object sender, EventArgs e)
        {
            try
            {
                var lb = this.Controls.Find("lbGecmis", true).FirstOrDefault() as ListBox;
                if (lb == null || lb.SelectedItem == null) return;
                lb.Items.Remove(lb.SelectedItem);
            }
            catch { }
        }

        const string FAV_FILE = "erp_favoriler.txt";

        private void FavorileriYukle()
        {
            _favoriler.Clear(); if (!File.Exists(FAV_FILE)) return;
            try { foreach (var s in File.ReadAllLines(FAV_FILE, Encoding.UTF8).Where(x => !string.IsNullOrWhiteSpace(x))) { string y2 = s.Trim(); if (File.Exists(y2) && !_favoriler.Contains(y2, StringComparer.OrdinalIgnoreCase)) _favoriler.Add(y2); } } catch { }
        }
        private void FavorileriKaydet() { try { File.WriteAllLines(FAV_FILE, _favoriler, Encoding.UTF8); } catch { } }
        private void FavorileriListeyeYaz()
        {
            try
            {
                var lb = this.Controls.Find("lbFavoriler", true).FirstOrDefault() as ListBox;
                if (lb == null) return;
                lb.Items.Clear();
                foreach (var f in _favoriler) lb.Items.Add("📊  " + Path.GetFileNameWithoutExtension(f));
            }
            catch { }
        }

        // ── Responsive Buton Yerleşimi ──────────────────────────────────
        private void pnlSonucButonlar_Resize(object sender, EventArgs e)
        {
            if (pnlSonucButonlar == null) return;

            // Panel genişliğine göre sağ butonları yerleştir
            int panelWidth = pnlSonucButonlar.ClientSize.Width;
            int padding = 8;

            // Sağdan sola sırayla yerleştir
            int rightX = panelWidth - padding;

            if (btnTumExcel != null)
            {
                rightX -= btnTumExcel.Width;
                btnTumExcel.Location = new Point(rightX, 8);
                rightX -= padding;
            }

            if (btnExceliAc != null)
            {
                rightX -= btnExceliAc.Width;
                btnExceliAc.Location = new Point(rightX, 8);
            }
        }

        private void Donusturucu_Load(object sender, EventArgs e) => DurumGuncelle();

        private void DurumGuncelle()
        {
            try
            {
                int toplam = dgvDosyalar.Rows.Count;
                int aktarildi = dgvDosyalar.Rows.Cast<DataGridViewRow>()
                    .Count(r => _erpAktarimYapilan.Contains(r.Cells["colDosyaYolu"].Value?.ToString() ?? ""));

                if (toplam == 0) Durum("📋", "Dosya yüklenmedi", Color.FromArgb(100, 100, 120), Color.FromArgb(230, 230, 235));
                else if (aktarildi == 0) Durum("📂", $"{toplam} dosya yüklendi  —  ERP aktarımı bekleniyor", Color.FromArgb(140, 80, 0), Color.FromArgb(255, 243, 205));
                else if (aktarildi < toplam) Durum("⚠", $"{aktarildi}/{toplam} dosya aktarıldı", Color.FromArgb(140, 80, 0), Color.FromArgb(255, 243, 205));
                else if (_rotaKaydedilen.Count < aktarildi) Durum("✏", $"ERP aktarıldı  —  Rota girişi bekleniyor  ({_rotaKaydedilen.Count}/{aktarildi})", Color.FromArgb(0, 70, 160), Color.FromArgb(220, 232, 255));
                else if (_civataEklendi.Count < aktarildi) Durum("🔩", "Rota kaydedildi  —  Civata listesi bekleniyor", Color.FromArgb(110, 55, 0), Color.FromArgb(255, 240, 210));
                else Durum("✅", "Tüm işlemler tamamlandı", Color.FromArgb(0, 100, 0), Color.FromArgb(210, 245, 215));
            }
            catch { }
        }

        private void Durum(string ikon, string metin, Color fg, Color bg)
        { try { lblDurumIkon.Text = ikon; lblDurumMetin.Text = metin; lblDurumMetin.ForeColor = fg; pnlDurumBar.BackColor = bg; } catch { } }

        private void DurumSatiriGuncelle(string dosyaYolu, string durum)
        {
            try
            {
                foreach (DataGridViewRow row in dgvDosyalar.Rows)
                    if (row.Cells["colDosyaYolu"].Value?.ToString() == dosyaYolu)
                    { row.Cells["colDurum"].Value = durum; break; }
                dgvDosyalar.Refresh();
            }
            catch { }
        }

        // ── DGV RENK — KÖKLü FİX ─────────────────────────────────────────
        // e.FormattingApplied = true → dgv'nin kendi default stilini ezer
        // Her satır TAMAMEN bağımsız renklenir, önceki satırdan taşınma olmaz

        private void dgvDosyalar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvDosyalar.Rows.Count) return;
            string yol = dgvDosyalar.Rows[e.RowIndex].Cells["colDosyaYolu"].Value?.ToString() ?? "";

            if (_civataEklendi.Contains(yol))
            {
                e.CellStyle.BackColor = Color.FromArgb(180, 240, 200);
                e.CellStyle.ForeColor = Color.FromArgb(0, 90, 0);
                e.CellStyle.SelectionBackColor = Color.FromArgb(140, 210, 160);
                e.CellStyle.SelectionForeColor = Color.FromArgb(0, 60, 0);
            }
            else if (_rotaKaydedilen.Contains(yol))
            {
                e.CellStyle.BackColor = Color.FromArgb(180, 210, 255);
                e.CellStyle.ForeColor = Color.FromArgb(0, 50, 130);
                e.CellStyle.SelectionBackColor = Color.FromArgb(140, 175, 230);
                e.CellStyle.SelectionForeColor = Color.FromArgb(0, 30, 100);
            }
            else if (_erpAktarimYapilan.Contains(yol))
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 243, 180);
                e.CellStyle.ForeColor = Color.FromArgb(110, 80, 0);
                e.CellStyle.SelectionBackColor = Color.FromArgb(220, 205, 130);
                e.CellStyle.SelectionForeColor = Color.FromArgb(80, 55, 0);
            }
            else
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 200, 200);
                e.CellStyle.ForeColor = Color.FromArgb(120, 0, 0);
                e.CellStyle.SelectionBackColor = Color.FromArgb(220, 160, 160);
                e.CellStyle.SelectionForeColor = Color.FromArgb(80, 0, 0);
            }
            e.FormattingApplied = true; // ← ANAHTAR SATIR: dgv'nin kendi stilini engelle
        }

        private void dgvDosyalar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var hit = dgvDosyalar.HitTest(e.X, e.Y);
            if (hit.RowIndex < 0) return;
            dgvDosyalar.ClearSelection();
            dgvDosyalar.Rows[hit.RowIndex].Selected = true;
            _sagKlikSatir = hit.RowIndex;

            string yol = dgvDosyalar.Rows[hit.RowIndex].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            bool aktarildi = _erpAktarimYapilan.Contains(yol);
            bool civataDone = _civataEklendi.Contains(yol);
            bool excelVar = aktarildi && _erpExcelYollari.ContainsKey(yol);

            try
            {
                mnuRotaGir.Enabled = aktarildi;
                mnuRotaGir.ForeColor = aktarildi ? Color.FromArgb(0, 100, 0) : Color.Gray;
                mnuExceliGoster.Enabled = excelVar;
                mnuExceliGoster.ForeColor = excelVar ? Color.FromArgb(0, 60, 140) : Color.Gray;
                mnuMalzemeDegistir.Enabled = excelVar;
                mnuMalzemeDegistir.ForeColor = excelVar ? Color.FromArgb(0, 130, 160) : Color.Gray;
                mnuCivataYukle.Enabled = aktarildi;
                mnuCivataYukle.ForeColor = aktarildi ? Color.FromArgb(120, 55, 0) : Color.Gray;
                mnuCivataYukle.Text = "🔩  Civata Listelerini Yükle"
                    + (!aktarildi ? "  (önce ERP aktarımı yapın)" : civataDone ? "  ✅ zaten eklendi" : "");
            }
            catch { }

            ctxDosya.Show(dgvDosyalar, e.Location);
        }

        private void mnuExceliGoster_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0) return;
            string yol = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            if (!_erpExcelYollari.ContainsKey(yol)) { MessageBox.Show("Bu dosya için Excel bulunamadı."); return; }
            string excelYol = _erpExcelYollari[yol];
            if (!DosyaErisebilir(excelYol)) AgaBaglan();
            if (!DosyaErisebilir(excelYol)) { MessageBox.Show("Excel dosyasına erişilemiyor:\n" + excelYol, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            try { System.Diagnostics.Process.Start(excelYol); }
            catch (Exception ex) { MessageBox.Show("Açılamadı: " + ex.Message); }
        }

        private void btnExceliAc_Click(object sender, EventArgs e)
        {
            string hedef = "";
            if (_sagKlikSatir >= 0 && _sagKlikSatir < dgvDosyalar.Rows.Count)
            {
                string yol = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
                if (_erpExcelYollari.ContainsKey(yol)) hedef = _erpExcelYollari[yol];
            }
            if (string.IsNullOrEmpty(hedef) && !string.IsNullOrEmpty(_sonKaydedilenExcel)) hedef = _sonKaydedilenExcel;
            if (string.IsNullOrEmpty(hedef) || !File.Exists(hedef))
            {
                var ofd = new OpenFileDialog { Title = "Excel dosyasını seçin", Filter = "Excel|*.xlsx;*.xls" };
                if (ofd.ShowDialog() != DialogResult.OK) return;
                hedef = ofd.FileName;
            }
            if (!DosyaErisebilir(hedef)) AgaBaglan();
            try { System.Diagnostics.Process.Start(hedef); }
            catch (Exception ex) { MessageBox.Show("Açılamadı: " + ex.Message); }
        }

        private void mnuMalzemeDegistir_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0 || _sagKlikSatir >= dgvDosyalar.Rows.Count) return;
            string dosyaYolu = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            if (!_erpExcelYollari.ContainsKey(dosyaYolu))
            { MessageBox.Show("Bu dosya için ERP Excel bulunamadı.\nÖnce ERP Aktarımı yapın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string excelYol = _erpExcelYollari[dosyaYolu];
            if (!DosyaErisebilir(excelYol)) AgaBaglan();
            if (!DosyaErisebilir(excelYol)) { MessageBox.Show("Excel dosyasına erişilemiyor:\n" + excelYol, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            using (var frm = new FrmMalzemeDegistir(excelYol))
            { frm.StartPosition = FormStartPosition.CenterParent; frm.ShowDialog(this); }
        }

        private void mnuRotaGir_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0 || _sagKlikSatir >= dgvDosyalar.Rows.Count) return;
            RotaGir(dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "");
        }

        private void RotaGir(string dosyaYolu)
        {
            string erpExcel = _erpExcelYollari.ContainsKey(dosyaYolu) ? _erpExcelYollari[dosyaYolu] : "";
            bool cacheCalismiyor = string.IsNullOrEmpty(erpExcel) || !DosyaErisebilir(erpExcel);
            if (cacheCalismiyor)
            {
                AgaBaglan();
                string arananIsim = string.IsNullOrEmpty(erpExcel) ? "" : Path.GetFileName(erpExcel);
                string bulunanYol = null;
                if (!string.IsNullOrEmpty(arananIsim)) { string d = Path.Combine(NET_PATH, arananIsim); if (DosyaErisebilir(d)) bulunanYol = d; }
                if (bulunanYol == null && Directory.Exists(NET_PATH))
                {
                    string dosyaAdSiz = Path.GetFileNameWithoutExtension(
                        dgvDosyalar.Rows.Cast<DataGridViewRow>()
                        .FirstOrDefault(r => r.Cells["colDosyaYolu"].Value?.ToString() == dosyaYolu)
                        ?.Cells["colDosyaAdi"].Value?.ToString() ?? "");
                    if (!string.IsNullOrEmpty(dosyaAdSiz))
                    {
                        var adaylar = Directory.GetFiles(NET_PATH, "*.xlsx")
                            .Where(f => Path.GetFileNameWithoutExtension(f).StartsWith(dosyaAdSiz, StringComparison.OrdinalIgnoreCase))
                            .OrderByDescending(f => File.GetLastWriteTime(f)).ToList();
                        if (adaylar.Count > 0) bulunanYol = adaylar[0];
                    }
                }
                if (bulunanYol != null)
                {
                    var cevap = MessageBox.Show($"ERP Excel bulundu:\n\n📄 {Path.GetFileName(bulunanYol)}\n\n[Evet] → Bu dosyayı kullan\n[Hayır] → Farklı dosya seç",
                        "ERP Excel Bulundu", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (cevap == DialogResult.Cancel) return;
                    erpExcel = cevap == DialogResult.Yes ? bulunanYol : KullanicidanExcelSec();
                    if (string.IsNullOrEmpty(erpExcel)) return;
                    _erpExcelYollari[dosyaYolu] = erpExcel;
                }
                else
                {
                    if (MessageBox.Show("ERP Excel bulunamadı.\nKendiniz seçmek ister misiniz?",
                        "Dosya Bulunamadı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                    erpExcel = KullanicidanExcelSec();
                    if (string.IsNullOrEmpty(erpExcel)) return;
                    _erpExcelYollari[dosyaYolu] = erpExcel;
                }
            }
            using (var frm = new FrmImalatKartiRota(erpExcel))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK)
                { _rotaKaydedilen.Add(dosyaYolu); DurumSatiriGuncelle(dosyaYolu, "✏ Rota Kaydedildi — civata bekleniyor"); DurumGuncelle(); }
            }
        }

        // ── Seçileni Sil ──────────────────────────────────────────────────

        private void btnSil_Click(object sender, EventArgs e)
        {
            int idx = -1;
            if (dgvDosyalar.SelectedRows.Count > 0) idx = dgvDosyalar.SelectedRows[0].Index;
            else if (_sagKlikSatir >= 0 && _sagKlikSatir < dgvDosyalar.Rows.Count) idx = _sagKlikSatir;
            if (idx < 0) { MessageBox.Show("Silinecek dosyayı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string yol = dgvDosyalar.Rows[idx].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            string ad = dgvDosyalar.Rows[idx].Cells["colDosyaAdi"].Value?.ToString() ?? "";
            if (MessageBox.Show($"'{ad}' listeden çıkarılsın mı?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _tumVeriler.RemoveAll(v => v.DosyaId == yol);
            _erpAktarimYapilan.Remove(yol); _rotaKaydedilen.Remove(yol);
            _civataEklendi.Remove(yol); _erpExcelYollari.Remove(yol);
            dgvDosyalar.Rows.RemoveAt(idx);
            _sagKlikSatir = -1; DurumGuncelle();
        }

        private void mnuDosyaSil_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0 || _sagKlikSatir >= dgvDosyalar.Rows.Count) return;
            string yol = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            string ad = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaAdi"].Value?.ToString() ?? "";
            if (MessageBox.Show($"'{ad}' listeden çıkarılsın mı?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _tumVeriler.RemoveAll(v => v.DosyaId == yol);
            _erpAktarimYapilan.Remove(yol); _rotaKaydedilen.Remove(yol);
            _civataEklendi.Remove(yol); _erpExcelYollari.Remove(yol);
            dgvDosyalar.Rows.RemoveAt(_sagKlikSatir);
            _sagKlikSatir = -1; DurumGuncelle();
        }

        // ── CİVATA ────────────────────────────────────────────────────────

        private void mnuCivataYukle_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0 || _sagKlikSatir >= dgvDosyalar.Rows.Count) return;
            string dosyaYolu = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            if (!_erpAktarimYapilan.Contains(dosyaYolu)) { MessageBox.Show("Önce ERP aktarımı yapılmalı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!_erpExcelYollari.ContainsKey(dosyaYolu)) { MessageBox.Show("Bu dosya için ERP Excel bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            double siparisAdeti = N(dgvDosyalar.Rows[_sagKlikSatir].Cells["colSiparisAdeti"].Value);
            if (siparisAdeti <= 0) siparisAdeti = 1;

            var ofd = new OpenFileDialog { Title = "Civata listesi dosyalarını seçin  (*_B.doc / *_B.txt)", Filter = "Civata Listesi|*_B.doc;*_B.txt;*.doc;*.txt|Hepsi|*.*", Multiselect = true };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            var tumCivatalar = new List<CivataSatir>();
            foreach (string yol in ofd.FileNames)
            {
                var liste = CivataDocOku(yol);
                if (liste.Count > 0) tumCivatalar.AddRange(liste);
                else MessageBox.Show("Civata okunamadı:\n" + Path.GetFileName(yol), "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (tumCivatalar.Count == 0) { MessageBox.Show("Hiç civata satırı bulunamadı."); return; }

            var bulunamadi = new List<BulunamadiItem>();
            var displayToEsles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var grp in tumCivatalar.GroupBy(c => c.EslesKey))
            {
                var ornek = grp.First();
                string displayKey = ornek.Tanim + "|" + ornek.Grade;
                displayToEsles[displayKey] = grp.Key;
                bulunamadi.Add(new BulunamadiItem
                {
                    Profil = ornek.Tanim,
                    Kalite = ornek.Grade,
                    ToplamAdet = grp.Sum(c => c.Quantity),
                    ToplamAgirlik = Math.Round(grp.Sum(c => c.ToplamAgirlik), 3),
                    Satirlar = grp.Select(c => $"► {c.BoltListAdi,-22}  {c.Tanim,-30}  Adet:{c.Quantity,6}  Ağ:{c.ToplamAgirlik:0.###} kg").ToList()
                });
            }

            var esles = new Dictionary<string, HammaddeItem>(StringComparer.OrdinalIgnoreCase);
            using (var frm = new FrmErpManuelEslestir(bulunamadi, ""))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) != DialogResult.OK) return;
                foreach (var kv in frm.Sonuclar)
                    if (displayToEsles.ContainsKey(kv.Key))
                        esles[displayToEsles[kv.Key]] = new HammaddeItem { No = kv.Value.No, Adi = kv.Value.Ad };
            }

            CivataExceleEkle(_erpExcelYollari[dosyaYolu], tumCivatalar, esles, dosyaYolu, siparisAdeti);
        }

        private List<CivataSatir> CivataDocOku(string yol)
        {
            var liste = new List<CivataSatir>();
            string boltListAdi = Regex.Replace(Path.GetFileNameWithoutExtension(yol), @"[-_][A-Za-z]$", "");
            try
            {
                string[] satirlar = File.ReadAllLines(yol, Encoding.Default);
                foreach (var satir in satirlar)
                {
                    int idx = satir.IndexOf("BOLT LIST:", StringComparison.OrdinalIgnoreCase);
                    if (idx < 0) continue;
                    string sonrasi = satir.Substring(idx + 10).Trim();
                    var mDate = Regex.Match(sonrasi, @"\s+\d+\s+Date:");
                    if (mDate.Success) sonrasi = sonrasi.Substring(0, mDate.Index).Trim();
                    else { var mNum = Regex.Match(sonrasi, @"\s{3,}\d+\s*$"); if (mNum.Success) sonrasi = sonrasi.Substring(0, mNum.Index).Trim(); }
                    if (!string.IsNullOrEmpty(sonrasi)) boltListAdi = sonrasi;
                    break;
                }
                var rex = new Regex(@"^\s*(M\d+)\s+x\s+(\d+)\s+(\d+)\s+(\S+)\s+(\S+)\s+([\d.]+)\s+([\d.]+)\s*kg", RegexOptions.IgnoreCase);
                foreach (var satir in satirlar)
                {
                    var m = rex.Match(satir); if (!m.Success) continue;
                    liste.Add(new CivataSatir
                    {
                        DosyaId = yol,
                        BoltListAdi = boltListAdi,
                        Dia = m.Groups[1].Value.ToUpper(),
                        Length = int.Parse(m.Groups[2].Value),
                        Quantity = int.Parse(m.Groups[3].Value),
                        Standard = m.Groups[4].Value,
                        Grade = m.Groups[5].Value,
                        BirimAgirlik = double.Parse(m.Groups[6].Value, CultureInfo.InvariantCulture),
                        ToplamAgirlik = double.Parse(m.Groups[7].Value, CultureInfo.InvariantCulture)
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show("Civata okuma hatası:\n" + ex.Message); }
            return liste;
        }

        private void CivataExceleEkle(string excelYol, List<CivataSatir> civatalar,
            Dictionary<string, HammaddeItem> esles, string dosyaYolu, double siparisAdeti)
        {
            if (!DosyaErisebilir(excelYol)) AgaBaglan();
            if (!DosyaErisebilir(excelYol)) { MessageBox.Show("ERP Excel'e erişilemiyor:\n" + excelYol, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            Cursor.Current = Cursors.WaitCursor;
            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Open(excelYol, false, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                var ws = (Excel.Worksheet)wb.Worksheets[1];

                object projeNoV = null;
                int lastRow = ws.UsedRange.Rows.Count;
                for (int r = 2; r <= lastRow; r++) { projeNoV = ((Excel.Range)ws.Cells[r, 1]).Value2; if (projeNoV != null) break; }

                int satirNo = lastRow + 1;
                foreach (var grp in civatalar.GroupBy(c => c.BoltListAdi))
                {
                    foreach (var cv in grp.OrderBy(c => c.Dia).ThenBy(c => c.Length))
                    {
                        bool eslestiMi = esles.ContainsKey(cv.EslesKey);
                        string hmNo = eslestiMi ? esles[cv.EslesKey].No : "";
                        string hmAdi = eslestiMi ? esles[cv.EslesKey].Adi : cv.Tanim; // ← malzeme ismi
                        Yaz(ws, satirNo, 1, projeNoV);
                        Yaz(ws, satirNo, 2, grp.Key);
                        Yaz(ws, satirNo, 3, hmAdi);          // ← Poz Açıklaması = malzeme ismi
                        Yaz(ws, satirNo, 4, null);
                        Yaz(ws, satirNo, 5, siparisAdeti);
                        Yaz(ws, satirNo, 6, cv.ToplamAgirlik);
                        Yaz(ws, satirNo, 7, "MADDE");
                        Yaz(ws, satirNo, 8, hmNo);
                        Yaz(ws, satirNo, 9, cv.Quantity);
                        Yaz(ws, satirNo, 10, null);
                        satirNo++;
                    }
                }

                wb.Save();
                LogHelper.Yaz(excelYol, "CIVATA_EKLENDI", $"{civatalar.Count} civata satırı eklendi. Kaynak: {Path.GetFileName(dosyaYolu)}");
                MessageBox.Show($"{civatalar.Count} civata satırı eklendi.\n\n{excelYol}", "Civata Eklendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _civataEklendi.Add(dosyaYolu);
                DurumSatiriGuncelle(dosyaYolu, "✅ Rota + Civata Tamamlandı");
                DurumGuncelle();
            }
            catch (Exception ex)
            {
                bool erisim = ex.HResult == unchecked((int)0x80070020) || ex.HResult == unchecked((int)0x80070005);
                MessageBox.Show("Excel yazma hatası:" + (erisim ? "\nDosya açık olabilir." : "") + "\n\n" + ex.Message);
                try { if (wb != null) wb.Close(false); } catch { }
                try { if (app != null) app.Quit(); } catch { }
            }
            finally
            {
                try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { }
                try { if (app != null) Marshal.ReleaseComObject(app); } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }

        // ── DOSYA OKUMA ───────────────────────────────────────────────────

        private void btnDosyaEkle_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "Rapor|*.DOC;*.TXT;*.RPT|Hepsi|*.*", Multiselect = true };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            int ok = 0, top = 0;
            foreach (string yol in ofd.FileNames)
            {
                if (DosyaVarMi(yol)) continue;
                string ad = Path.GetFileName(yol); int n = DosyaOku(yol, ad);
                if (n > 0) { dgvDosyalar.Rows.Add(ad, "1", yol, "Yüklendi"); ok++; top += n; }
                else MessageBox.Show("Okunamadi: " + ad);
            }
            if (ok > 0)
            {
                DurumGuncelle();
                MessageBox.Show(
                    $"✅  {ok} dosya yüklendi, {top} parça okundu.\n\n" +
                    "Lütfen önce bu üründen kaç adet sipariş verildiğini giriniz\n" +
                    "ve sonrasında  💾 Miktar Kaydet  butonuna tıklayınız.",
                    "Dosya Yüklendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool DosyaVarMi(string yol)
        { foreach (DataGridViewRow r in dgvDosyalar.Rows) if (r.Cells["colDosyaYolu"].Value?.ToString() == yol) return true; return false; }

        private int DosyaOku(string yol, string ad)
        {
            var list = new List<HamSatir>();
            try
            {
                string[] satirlar = File.ReadAllLines(yol, Encoding.Default);
                if (string.Join(" ", satirlar).Contains("+ACo-")) satirlar = File.ReadAllLines(yol, Encoding.UTF7);
                string asm = ""; double madet = 1;
                foreach (string satir in satirlar)
                {
                    string t = satir.Trim();
                    if (string.IsNullOrEmpty(t) || t.StartsWith("-") || t.ToUpper().Contains("ASSEMBLY") || t.Contains("Total:")) continue;
                    string[] p = Regex.Split(t, @"\s{2,}"); if (p.Length < 2) continue;
                    int bo = 0; for (int k = 0; k < satir.Length; k++) { if (satir[k] == ' ') bo++; else break; }
                    if (bo < 5 && IsNum(p[1])) { asm = p[0]; madet = N(p[1]); if (madet <= 0) madet = 1; }
                    else if (bo >= 5)
                    {
                        var tp = p.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                        if (tp.Count >= 5 && IsNum(tp[1]))
                            list.Add(new HamSatir { DosyaId = yol, DosyaAdi = ad, MontajNo = asm, MontajAdeti = madet, ParcaNo = tp[0], BirimAdet = N(tp[1]), ParcaProfil = tp[2], Kalite = tp.Count > 3 ? tp[3] : "", Uzunluk = tp.Count > 4 ? N(tp[4]) : 0, Agirlik = tp.Count > 5 ? N(tp[5]) : 0 });
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); return 0; }
            _tumVeriler.AddRange(list);
            return list.Count;
        }

        private string PG(string p) { if ((p.StartsWith("PL", StringComparison.OrdinalIgnoreCase) || p.StartsWith("FPL", StringComparison.OrdinalIgnoreCase)) && p.Contains("*")) return p.Split('*')[0].ToUpper() + "*"; return p; }
        private string KD(string k) { if (string.IsNullOrWhiteSpace(k)) return k; switch (k.Trim().ToUpper()) { case "ST37": return "S235JR"; case "ST52": return "S355JR"; case "ST44": return "S275JR"; default: return k.Trim(); } }
        private static string TemizleDocSoneki(string ad) => Regex.Replace(ad, @"[-_][A-Za-z]$", "");

        private void btnMiktarKaydet_Click(object sender, EventArgs e) { dgvDosyalar.EndEdit(); MessageBox.Show("Kaydedildi."); }

        private void btnListeTemizle_Click(object sender, EventArgs e)
        {
            dgvDosyalar.Rows.Clear(); dgvSonuc.Rows.Clear();
            _tumVeriler.Clear(); _erpAktarimYapilan.Clear(); _rotaKaydedilen.Clear();
            _civataEklendi.Clear(); _erpExcelYollari.Clear(); _sonKaydedilenExcel = "";
            DurumGuncelle();
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }
            dgvDosyalar.EndEdit(); Cursor.Current = Cursors.WaitCursor; dgvSonuc.Rows.Clear();
            foreach (var item in Ozet(SM()).Values.OrderBy(x => x.Profil).ThenBy(x => x.Kalite))
                dgvSonuc.Rows.Add(item.Profil, item.Kalite, item.ToplamAdet, item.ToplamUzunluk, Math.Round(item.ToplamAgirlik, 2));
            Cursor.Current = Cursors.Default; MessageBox.Show("Tamam.");
        }

        private void btnVeritabaniKaydet_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }
            dgvDosyalar.EndEdit();
            int musteriID = -1; string durum = "Uretim Bekliyor";
            using (var frm = new MusteriSecForm()) { frm.StartPosition = FormStartPosition.CenterParent; if (frm.ShowDialog() != DialogResult.OK) return; musteriID = frm.SecilenMusteriID; }
            var tf = new FrmTerminSec(); if (tf.ShowDialog() != DialogResult.OK) return; DateTime termin = tf.SecilenTarih;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var sm = SM(); int n = 0;
                using (var conn = new SqlConnection(DB.ConnStr))
                {
                    conn.Open();
                    const string sql = @"INSERT INTO MalzemeKayit (MusteriID,DosyaAdi,MontajNo,ParcaNo,Profil,Kalite,Uzunluk,Agirlik,UretimAdeti,Durum,TerminTarihi) VALUES(@mid,@da,@mn,@pn,@pr,@ka,@uz,@ag,@ua,@du,@tt)";
                    foreach (var v in _tumVeriler)
                    {
                        double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                        double urt = sip * v.MontajAdeti * v.BirimAdet;
                        using (var cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@mid", musteriID); cmd.Parameters.AddWithValue("@da", v.DosyaAdi ?? "");
                            cmd.Parameters.AddWithValue("@mn", v.MontajNo ?? ""); cmd.Parameters.AddWithValue("@pn", v.ParcaNo ?? "");
                            cmd.Parameters.AddWithValue("@pr", PG(v.ParcaProfil)); cmd.Parameters.AddWithValue("@ka", KD(v.Kalite));
                            cmd.Parameters.AddWithValue("@uz", v.Uzunluk); cmd.Parameters.AddWithValue("@ag", Math.Round(urt * v.Agirlik, 3));
                            cmd.Parameters.AddWithValue("@ua", urt); cmd.Parameters.AddWithValue("@du", durum); cmd.Parameters.AddWithValue("@tt", termin);
                            cmd.ExecuteNonQuery();
                        }
                        n++;
                    }
                }
                MessageBox.Show(n + " kayit eklendi.");
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { Cursor.Current = Cursors.Default; }
        }

        // ── ERP AKTARİM — her dosyaya ayrı Excel + proje no dialog ────────

        private void btnErpAktarim_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }
            dgvDosyalar.EndEdit();

            // ── Proje No Dialog ───────────────────────────────────────────
            // ── Proje No Dialog ───────────────────────────────────────────
            using (var frmPrj = new Form())
            {
                frmPrj.Text = "Proje Numarası";
                frmPrj.Size = new Size(500, 210);
                frmPrj.StartPosition = FormStartPosition.CenterParent;
                frmPrj.FormBorderStyle = FormBorderStyle.FixedDialog;
                frmPrj.MaximizeBox = false; frmPrj.MinimizeBox = false;
                frmPrj.BackColor = Color.FromArgb(245, 245, 250);

                var lblAcik = new Label { Text = "Proje numarasını şimdi girmek ister misiniz?", Dock = DockStyle.Top, Height = 42, Font = new Font("Segoe UI", 10f), TextAlign = ContentAlignment.MiddleCenter };
                var txtPrj = new TextBox { Dock = DockStyle.Top, Height = 40, Font = new Font("Segoe UI", 13f), Text = _projeNo };
                var pnlBtn = new Panel { Dock = DockStyle.Bottom, Height = 56 };

                var btnGir = new Button { Text = "✔  Proje No Gir", DialogResult = DialogResult.OK, Dock = DockStyle.Left, Width = 180, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 180), ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
                var btnSonra = new Button { Text = "⏭  Daha Sonra Gireceğim", DialogResult = DialogResult.Ignore, Dock = DockStyle.Right, Width = 200, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(90, 90, 105), ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
                btnGir.FlatAppearance.BorderSize = 0;
                btnSonra.FlatAppearance.BorderSize = 0;

                pnlBtn.Controls.Add(btnSonra);
                pnlBtn.Controls.Add(btnGir);
                frmPrj.Controls.Add(pnlBtn);
                frmPrj.Controls.Add(txtPrj);
                frmPrj.Controls.Add(lblAcik);

                frmPrj.AcceptButton = btnGir;
                frmPrj.CancelButton = btnSonra; // ESC veya X → "Daha sonra" gibi davransın
                frmPrj.Shown += (s, ea) => { try { txtPrj.Focus(); txtPrj.SelectAll(); } catch { } };

                var sonuc = frmPrj.ShowDialog(this);
                if (sonuc == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtPrj.Text))
                    {
                        MessageBox.Show("Proje No boş olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    _projeNo = txtPrj.Text.Trim();
                }
                else
                {
                    // Ignore, Cancel veya X → sonra girilecek
                    _projeNo = "";
                }
            }

            var sm = SM();
            var hm = new List<HammaddeItem>();
            try
            {
                using (var conn = new SqlConnection(DB.ConnStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT no,adi FROM hammadde ORDER BY adi", conn))
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read()) hm.Add(new HammaddeItem { No = dr["no"]?.ToString() ?? "", Adi = dr["adi"]?.ToString() ?? "" });
                }
            }
            catch (Exception ex) { MessageBox.Show("DB: " + ex.Message); return; }

            // ── Tüm dosyalar için eşleştirmeyi BİR KERE yap ──────────────
            var ozet = Ozet(sm);
            var esles = new Dictionary<string, HammaddeItem>(StringComparer.OrdinalIgnoreCase);
            var bulunamadi = new List<BulunamadiItem>();
            var profilSatirlar = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var v in _tumVeriler)
            {
                string k2 = PG(v.ParcaProfil) + "|" + KD(v.Kalite);
                double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                double urt = sip * v.MontajAdeti * v.BirimAdet;
                if (!profilSatirlar.ContainsKey(k2)) profilSatirlar[k2] = new List<string>();
                profilSatirlar[k2].Add($"► {v.DosyaAdi,-20}  {v.ParcaNo,-12}  Adet:{urt,8:0.##}  Boy:{v.Uzunluk,8:0.##}  Ağ:{urt * v.Agirlik,8:0.###} kg");
            }

            foreach (var oz in ozet.Values)
            {
                string key = oz.Profil + "|" + oz.Kalite;
                string profNorm = oz.Profil.ToUpper().Replace(" ", "").Replace("-", "");
                HammaddeItem bulunan = null;
                bulunan = hm.FirstOrDefault(h => h.Adi.ToUpper().Replace(" ", "").Replace("-", "") == profNorm);
                if (bulunan == null) bulunan = hm.FirstOrDefault(h => h.No.ToUpper().Replace(" ", "").Replace("-", "") == profNorm);
                if (bulunan == null) { string prefix = profNorm.TrimEnd('*'); bulunan = hm.FirstOrDefault(h => h.Adi.ToUpper().Replace(" ", "").Replace("-", "").StartsWith(prefix) && prefix.Length >= 2); }
                if (bulunan == null && profNorm.Length >= 3) bulunan = hm.FirstOrDefault(h => h.Adi.ToUpper().Replace(" ", "").Replace("-", "").Contains(profNorm) || (profNorm.Contains(h.Adi.ToUpper().Replace(" ", "").Replace("-", "")) && h.Adi.Length >= 3));
                if (bulunan != null) esles[key] = bulunan;
                else bulunamadi.Add(new BulunamadiItem { Profil = oz.Profil, Kalite = oz.Kalite, ToplamAdet = oz.ToplamAdet, ToplamAgirlik = Math.Round(oz.ToplamAgirlik, 2), Satirlar = profilSatirlar.ContainsKey(key) ? profilSatirlar[key] : new List<string>() });
            }

            if (bulunamadi.Count > 0)
            {
                using (var frm = new FrmErpManuelEslestir(bulunamadi, _imalatKartlariKlasoru))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    if (frm.ShowDialog(this) != DialogResult.OK) return;
                    foreach (var kv in frm.Sonuclar) esles[kv.Key] = new HammaddeItem { No = kv.Value.No, Adi = kv.Value.Ad };
                }
            }

            // ── Her dosya için AYRI Excel oluştur ─────────────────────────
            string sonKaydedilen = "";
            var birlesikVeriler = new List<(string dosyaAdi, string kaydYol)>();

            foreach (DataGridViewRow dgvRow in dgvDosyalar.Rows)
            {
                string dosyaYolu = dgvRow.Cells["colDosyaYolu"].Value?.ToString() ?? "";
                string dosyaAdi = dgvRow.Cells["colDosyaAdi"].Value?.ToString() ?? "";
                if (string.IsNullOrEmpty(dosyaYolu)) continue;

                var dosyaVerileri = _tumVeriler.Where(v => v.DosyaId == dosyaYolu).ToList();
                if (dosyaVerileri.Count == 0) continue;

                double sip = sm.ContainsKey(dosyaYolu) ? sm[dosyaYolu] : 1;
                string dosyaAdSiz = TemizleDocSoneki(Path.GetFileNameWithoutExtension(dosyaAdi));
                string ciktiAdi = dosyaAdSiz + "_ERPAKTARIM.xlsx";

                string projeNoKullan = string.IsNullOrEmpty(_projeNo) ? "PROJE_NO_GIRILMEDI" : _projeNo;

                string kaydYol = AcExcelVeKaydet(ciktiAdi, xa =>
                {
                    string[] headers = { "Proje No", "Poz No", "Poz Açıklaması", "Ana Poz No", "Poz Miktar", "Poz Ağırlık", "Bileşen Türü", "Bileşen No", "Bileşen Miktar", "İşlem Sırası" };
                    for (int i = 0; i < headers.Length; i++) Yaz(xa, 1, i + 1, headers[i]);
                    int excelRow = 2;
                    var yazılanAnaPozlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (var montajGroup in dosyaVerileri.GroupBy(v => (v.MontajNo ?? "").Trim()).OrderBy(g => g.Key))
                    {
                        string montajNo = montajGroup.Key;
                        bool hasMontaj = !string.IsNullOrWhiteSpace(montajNo);
                        bool tekParca = montajGroup.Count() == 1;

                        if (hasMontaj && !tekParca)
                        {
                            string apKey = dosyaYolu + "||" + montajNo;
                            if (!yazılanAnaPozlar.Contains(apKey))
                            {
                                yazılanAnaPozlar.Add(apKey);
                                double mAg = montajGroup.Sum(v => sip * v.MontajAdeti * v.BirimAdet * v.Agirlik);
                                Yaz(xa, excelRow, 1, projeNoKullan); Yaz(xa, excelRow, 2, montajNo); Yaz(xa, excelRow, 3, montajNo);
                                Yaz(xa, excelRow, 4, dosyaAdSiz); Yaz(xa, excelRow, 5, Math.Round(montajGroup.First().MontajAdeti * sip, 2));
                                Yaz(xa, excelRow, 6, Math.Round(mAg, 2)); Yaz(xa, excelRow, 7, "Kaynak"); Yaz(xa, excelRow, 8, "Kaynaklama");
                                Yaz(xa, excelRow, 9, 1); Yaz(xa, excelRow, 10, ""); excelRow++;
                            }
                        }

                        foreach (var v in montajGroup.OrderBy(x => x.ParcaNo))
                        {
                            string anaPoz = (hasMontaj && !tekParca) ? montajNo : dosyaAdSiz;
                            string key = PG(v.ParcaProfil) + "|" + KD(v.Kalite);
                            bool e2 = esles.ContainsKey(key);
                            double urt = sip * v.MontajAdeti * v.BirimAdet;
                            Yaz(xa, excelRow, 1, projeNoKullan); Yaz(xa, excelRow, 2, (v.ParcaNo ?? "").Trim());
                            Yaz(xa, excelRow, 3, (v.ParcaNo ?? "").Trim()); Yaz(xa, excelRow, 4, anaPoz);
                            Yaz(xa, excelRow, 5, Math.Round(v.MontajAdeti * sip, 2)); Yaz(xa, excelRow, 6, Math.Round(urt * v.Agirlik, 2));
                            Yaz(xa, excelRow, 7, "Madde"); Yaz(xa, excelRow, 8, e2 ? esles[key].No : "");
                            Yaz(xa, excelRow, 9, Math.Round(urt, 2)); Yaz(xa, excelRow, 10, 1); excelRow++;
                        }
                    }
                });

                if (!string.IsNullOrEmpty(kaydYol))
                {
                    sonKaydedilen = kaydYol;
                    _erpAktarimYapilan.Add(dosyaYolu);
                    _erpExcelYollari[dosyaYolu] = kaydYol;
                    birlesikVeriler.Add((dosyaAdi, kaydYol));
                    try { dgvRow.Cells["colDurum"].Value = "🟡 ERP Aktarıldı — rota bekleniyor"; } catch { }
                    LogHelper.Yaz(kaydYol, "ERP_AKTARIM", $"Dosya: {dosyaAdi}");
                }
            }

            if (birlesikVeriler.Count == 0) return;

            // ── Birleşik Excel ─────────────────────────────────────────────
            if (birlesikVeriler.Count > 1)
            {
                string projeNoKullan = string.IsNullOrEmpty(_projeNo) ? "PROJE_NO_GIRILMEDI" : _projeNo;
                string birlesikAd = projeNoKullan + "_BIRLESIK_ERPAKTARIM.xlsx";

                AcExcelVeKaydet(birlesikAd, xa =>
                {
                    string[] headers = { "Proje No", "Poz No", "Poz Açıklaması", "Ana Poz No", "Poz Miktar", "Poz Ağırlık", "Bileşen Türü", "Bileşen No", "Bileşen Miktar", "İşlem Sırası" };
                    for (int i = 0; i < headers.Length; i++) Yaz(xa, 1, i + 1, headers[i]);
                    int excelRow = 2;
                    var yazılanAnaPozlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (var docGroup in _tumVeriler.GroupBy(v => v.DosyaId).OrderBy(g => g.Key))
                    {
                        string dosyaYoluG = docGroup.Key;
                        double sip = sm.ContainsKey(dosyaYoluG) ? sm[dosyaYoluG] : 1;
                        string dosyaAdSizG = TemizleDocSoneki(Path.GetFileNameWithoutExtension(docGroup.First().DosyaAdi));

                        foreach (var montajGroup in docGroup.GroupBy(v => (v.MontajNo ?? "").Trim()).OrderBy(g => g.Key))
                        {
                            string montajNo = montajGroup.Key;
                            bool hasMontaj = !string.IsNullOrWhiteSpace(montajNo);
                            bool tekParca = montajGroup.Count() == 1;

                            if (hasMontaj && !tekParca)
                            {
                                string apKey = dosyaYoluG + "||" + montajNo;
                                if (!yazılanAnaPozlar.Contains(apKey))
                                {
                                    yazılanAnaPozlar.Add(apKey);
                                    double mAg = montajGroup.Sum(v => sip * v.MontajAdeti * v.BirimAdet * v.Agirlik);
                                    Yaz(xa, excelRow, 1, projeNoKullan); Yaz(xa, excelRow, 2, montajNo); Yaz(xa, excelRow, 3, montajNo);
                                    Yaz(xa, excelRow, 4, dosyaAdSizG); Yaz(xa, excelRow, 5, Math.Round(montajGroup.First().MontajAdeti * sip, 2));
                                    Yaz(xa, excelRow, 6, Math.Round(mAg, 2)); Yaz(xa, excelRow, 7, "Kaynak"); Yaz(xa, excelRow, 8, "Kaynaklama");
                                    Yaz(xa, excelRow, 9, 1); Yaz(xa, excelRow, 10, ""); excelRow++;
                                }
                            }
                            foreach (var v in montajGroup.OrderBy(x => x.ParcaNo))
                            {
                                string anaPoz = (hasMontaj && !tekParca) ? montajNo : dosyaAdSizG;
                                string key = PG(v.ParcaProfil) + "|" + KD(v.Kalite);
                                bool e2 = esles.ContainsKey(key);
                                double urt = sip * v.MontajAdeti * v.BirimAdet;
                                Yaz(xa, excelRow, 1, projeNoKullan); Yaz(xa, excelRow, 2, (v.ParcaNo ?? "").Trim());
                                Yaz(xa, excelRow, 3, (v.ParcaNo ?? "").Trim()); Yaz(xa, excelRow, 4, anaPoz);
                                Yaz(xa, excelRow, 5, Math.Round(v.MontajAdeti * sip, 2)); Yaz(xa, excelRow, 6, Math.Round(urt * v.Agirlik, 2));
                                Yaz(xa, excelRow, 7, "Madde"); Yaz(xa, excelRow, 8, e2 ? esles[key].No : "");
                                Yaz(xa, excelRow, 9, Math.Round(urt, 2)); Yaz(xa, excelRow, 10, 1); excelRow++;
                            }
                        }
                    }
                });
            }

            if (!string.IsNullOrEmpty(sonKaydedilen)) _sonKaydedilenExcel = sonKaydedilen;
            dgvDosyalar.Refresh(); DurumGuncelle();
        }

        // ── TÜM EXCEL OLUŞTUR ─────────────────────────────────────────────

        private void btnTumExcel_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) { MessageBox.Show("Önce dosya yükleyin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            // Proje no girilmemişse uyar
            if (string.IsNullOrEmpty(_projeNo))
            {
                MessageBox.Show("Proje numarası girilmemiş!\n\nLütfen önce ERP Aktarım butonuna basarak proje numarasını giriniz.", "Proje Numarası Gerekli", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvDosyalar.EndEdit();
            var sm = SM();
            string ciktiAdi = _projeNo + "_TUM_DOSYALAR_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";

            AcExcelVeKaydet(ciktiAdi, ws =>
            {
                string[] headers = { "Dosya", "Montaj No", "Montaj Adeti", "Parça No", "Profil", "Kalite", "Uzunluk (mm)", "Birim Ağırlık (kg)", "Sipariş Adeti", "Üretim Adeti", "Toplam Ağırlık (kg)" };
                for (int i = 0; i < headers.Length; i++)
                    S(ws, 1, i + 1, headers[i], bold: true, bg: RenkBaslik, fg: RenkBeyaz, center: true);
                try { ((Excel.Range)ws.Rows[1]).RowHeight = 22; } catch { }

                int row = 2, sNo = 0;
                foreach (var docGroup in _tumVeriler.GroupBy(v => v.DosyaId).OrderBy(g => g.Key))
                {
                    double sip = sm.ContainsKey(docGroup.Key) ? sm[docGroup.Key] : 1;
                    string docAdi = docGroup.First().DosyaAdi;

                    S(ws, row, 1, "📄  " + docAdi + "   —   Sipariş: " + sip + " adet", bold: true, bg: RenkMavi, fg: RenkBeyaz);
                    try { ((Excel.Range)ws.Range[ws.Cells[row, 1], ws.Cells[row, 11]]).Merge(); } catch { }
                    try { ((Excel.Range)ws.Rows[row]).RowHeight = 22; } catch { }
                    row++;

                    foreach (var v in docGroup.OrderBy(x => x.MontajNo).ThenBy(x => x.ParcaNo))
                    {
                        double urt = sip * v.MontajAdeti * v.BirimAdet;
                        int rowBg = sNo % 2 == 0 ? RenkBeyaz : RenkGri;
                        S(ws, row, 1, v.DosyaAdi, bg: rowBg);
                        S(ws, row, 2, v.MontajNo, bg: rowBg);
                        S(ws, row, 3, v.MontajAdeti, bg: rowBg);
                        S(ws, row, 4, v.ParcaNo, bg: rowBg);
                        S(ws, row, 5, PG(v.ParcaProfil), bg: rowBg);
                        S(ws, row, 6, KD(v.Kalite), bg: rowBg);
                        S(ws, row, 7, v.Uzunluk, bg: rowBg);
                        S(ws, row, 8, v.Agirlik, bg: rowBg);
                        S(ws, row, 9, sip, bg: rowBg);
                        S(ws, row, 10, Math.Round(urt, 2), bg: rowBg);
                        S(ws, row, 11, Math.Round(urt * v.Agirlik, 3), bg: rowBg);
                        row++; sNo++;
                    }
                    row++;
                }
                try { int[] w = { 24, 14, 12, 14, 18, 10, 14, 16, 12, 13, 18 }; for (int i = 0; i < w.Length; i++) ((Excel.Range)ws.Columns[i + 1]).ColumnWidth = w[i]; } catch { }
            });
        }

        private void btnGalvanizEkran_Click(object sender, EventArgs e) => new GalvanizKontrol().Show();

        private void btnHamDataExcel_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) return; dgvDosyalar.EndEdit(); var sm = SM();
            AcExcel(xa =>
            {
                int r = 1; Yaz(xa, r, 1, "DOSYA"); Yaz(xa, r, 2, "SIPARIS"); Yaz(xa, r, 3, "MONTAJ"); Yaz(xa, r, 4, "MADET"); Yaz(xa, r, 5, "PARCA"); Yaz(xa, r, 6, "PROFIL"); Yaz(xa, r, 7, "KALITE"); Yaz(xa, r, 8, "UZUNLUK"); Yaz(xa, r, 9, "AGIRLIK"); r++;
                foreach (var v in _tumVeriler) { double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1; Yaz(xa, r, 1, v.DosyaAdi); Yaz(xa, r, 2, sip); Yaz(xa, r, 3, v.MontajNo); Yaz(xa, r, 4, v.MontajAdeti); Yaz(xa, r, 5, v.ParcaNo); Yaz(xa, r, 6, PG(v.ParcaProfil)); Yaz(xa, r, 7, KD(v.Kalite)); Yaz(xa, r, 8, v.Uzunluk); Yaz(xa, r, 9, v.Agirlik); r++; }
            });
        }

        private void btnMalzemeExcel_Click(object sender, EventArgs e)
        {
            if (dgvSonuc.Rows.Count == 0) { MessageBox.Show("Önce HESAPLA butonuna bas."); return; }
            dgvDosyalar.EndEdit(); Cursor.Current = Cursors.WaitCursor;
            var sm = SM();
            var dict = new Dictionary<string, List<KesimParcasi>>(StringComparer.OrdinalIgnoreCase);
            var kgPerM = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            foreach (var v in _tumVeriler)
            {
                if (v.Uzunluk <= 0) continue;
                double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                int adet = (int)Math.Round(sip * v.MontajAdeti * v.BirimAdet);
                string key = PG(v.ParcaProfil) + "||" + KD(v.Kalite);
                if (!dict.ContainsKey(key)) dict[key] = new List<KesimParcasi>();
                var mevcut = dict[key].FirstOrDefault(x => Math.Abs(x.Uzunluk - v.Uzunluk) < 1);
                if (mevcut != null) mevcut.Adet += adet; else dict[key].Add(new KesimParcasi { ParcaNo = v.ParcaNo, Uzunluk = v.Uzunluk, Adet = adet });
                if (v.Uzunluk > 0 && v.Agirlik > 0 && !kgPerM.ContainsKey(key)) kgPerM[key] = v.Agirlik / (v.Uzunluk / 1000.0);
            }

            var sonuclar = new List<ProfilSonuc>();
            foreach (var kv in dict.OrderBy(x => x.Key))
            {
                string[] pts = kv.Key.Split(new[] { "||" }, StringSplitOptions.None);
                string profil = pts[0], kalite = pts.Length > 1 ? pts[1] : "";
                double kgM = kgPerM.ContainsKey(kv.Key) ? kgPerM[kv.Key] : 0;
                double netMetre = kv.Value.Sum(p => p.Uzunluk * p.Adet) / 1000.0;
                double netKg = netMetre * kgM;
                var b6 = FFD(kv.Value, 6000); var b12 = FFD(kv.Value, 12000);
                double fp6 = b6.Count > 0 ? b6.Average(x => x.FirePct) : 0; double fp12 = b12.Count > 0 ? b12.Average(x => x.FirePct) : 0;
                double enIyiFire = double.MaxValue; double enIyiBoy = 6000; List<StokBar> enIyiBarlar = b6;
                foreach (double boy in STOK_ADAYLARI) { var bb = FFD(kv.Value, boy); double fp = bb.Count > 0 ? bb.Average(x => x.FirePct) : 0; if (fp < enIyiFire) { enIyiFire = fp; enIyiBoy = boy; enIyiBarlar = bb; } }
                sonuclar.Add(new ProfilSonuc { Profil = profil, Kalite = kalite, KgPerMetre = kgM, ToplamNetMetre = netMetre, ToplamKg = netKg, Bar6Adet = b6.Count, Bar6ToplamMetre = b6.Count * 6.0, Bar6ToplamKg = b6.Count * 6.0 * kgM, Bar6FirePct = fp6, Bar12Adet = b12.Count, Bar12ToplamMetre = b12.Count * 12.0, Bar12ToplamKg = b12.Count * 12.0 * kgM, Bar12FirePct = fp12, OnerilenBoy = enIyiBoy, OnerilenAdet = enIyiBarlar.Count, OnerilenMetre = enIyiBarlar.Count * (enIyiBoy / 1000.0), OnerilenKg = enIyiBarlar.Count * (enIyiBoy / 1000.0) * kgM, OnerilenFirePct = enIyiFire, OnerilenBarlar = enIyiBarlar, OneriNot = enIyiBoy != 6000 && enIyiBoy != 12000 ? $"★ {enIyiBoy / 1000.0:0.0}m önerilen" : "" });
            }

            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false; wb = app.Workbooks.Add(Type.Missing); Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1]; int row = 1;
                S(ws, row, 1, "SATIN ALIM LİSTESİ  —  " + DateTime.Now.ToString("dd.MM.yyyy"), bold: true, bg: RenkBaslik, fg: RenkBeyaz, size: 13); H(ws, row, 18); row++;
                string[] h1 = { "PROFİL", "KALİTE", "kg/m", "GEREKEN NET METRE", "GEREKEN NET KG", "6m BAR — ADET", "6m — TOPLAM METRE", "6m — TOPLAM KG", "6m FIRE %", "12m BAR — ADET", "12m — TOPLAM METRE", "12m — TOPLAM KG", "12m FIRE %", "ÖNERİLEN BOY & ADET", "NOT" };
                for (int i = 0; i < h1.Length; i++) S(ws, row, i + 1, h1[i], bold: true, bg: RenkMavi, fg: RenkBeyaz, center: true); H(ws, row, 20); row++; int sNo = 0;
                foreach (var s in sonuclar) { int bg6 = s.Bar6FirePct > 20 ? RenkKirmizi : s.Bar6FirePct > 10 ? RenkSari : RenkYesil; int bg12 = s.Bar12FirePct > 20 ? RenkKirmizi : s.Bar12FirePct > 10 ? RenkSari : RenkYesil; int bgOner = s.OnerilenFirePct > 20 ? RenkKirmizi : s.OnerilenFirePct > 10 ? RenkSari : RenkYesil; int rowBg = sNo % 2 == 0 ? RenkBeyaz : RenkGri; S(ws, row, 1, s.Profil, bg: rowBg); S(ws, row, 2, s.Kalite, bg: rowBg); S(ws, row, 3, Math.Round(s.KgPerMetre, 3), bg: rowBg); S(ws, row, 4, Math.Round(s.ToplamNetMetre, 2), bg: rowBg); S(ws, row, 5, Math.Round(s.ToplamKg, 2), bg: rowBg); S(ws, row, 6, s.Bar6Adet, bg: bg6); S(ws, row, 7, Math.Round(s.Bar6ToplamMetre, 1), bg: bg6); S(ws, row, 8, Math.Round(s.Bar6ToplamKg, 1), bg: bg6); S(ws, row, 9, Math.Round(s.Bar6FirePct, 1), bg: bg6); S(ws, row, 10, s.Bar12Adet, bg: bg12); S(ws, row, 11, Math.Round(s.Bar12ToplamMetre, 1), bg: bg12); S(ws, row, 12, Math.Round(s.Bar12ToplamKg, 1), bg: bg12); S(ws, row, 13, Math.Round(s.Bar12FirePct, 1), bg: bg12); S(ws, row, 14, $"{s.OnerilenBoy / 1000.0:0.0}m × {s.OnerilenAdet} bar  ({s.OnerilenFirePct:0.#}% fire)", bg: bgOner); S(ws, row, 15, s.OneriNot, bg: rowBg); row++; sNo++; }
                try { int[] w = { 16, 10, 7, 16, 14, 14, 16, 14, 10, 14, 16, 14, 10, 28, 26 }; for (int i = 0; i < w.Length; i++) ((Excel.Range)ws.Columns[i + 1]).ColumnWidth = w[i]; } catch { }
                row += 2; S(ws, row, 1, "KESİM PLANI", bold: true, bg: RenkBaslik, fg: RenkBeyaz, size: 12); H(ws, row, 22); row++;
                string[] h2 = { "PROFİL", "KALİTE", "BAR BOYU", "BAR NO", "KULLANILAN (mm)", "FIRE (mm)", "FIRE %", "PARÇALAR" };
                for (int i = 0; i < h2.Length; i++) S(ws, row, i + 1, h2[i], bold: true, bg: RenkMavi, fg: RenkBeyaz, center: true); try { ((Excel.Range)ws.Columns[8]).ColumnWidth = 70; } catch { }
                H(ws, row, 20); row++; sNo = 0;
                foreach (var s in sonuclar) { string boyStr = (s.OnerilenBoy / 1000.0).ToString("0.0") + " m"; foreach (var bar in s.OnerilenBarlar) { string pstr = string.Join("  |  ", bar.Dilimler.Select(d => $"{d.ParcaNo} {d.Uzunluk}mm×{d.Adet}")); int rowBg = sNo % 2 == 0 ? RenkBeyaz : RenkGri; int fireBg = bar.FirePct > 20 ? RenkKirmizi : bar.FirePct > 10 ? RenkSari : RenkYesil; S(ws, row, 1, s.Profil, bg: rowBg); S(ws, row, 2, s.Kalite, bg: rowBg); S(ws, row, 3, boyStr, bg: rowBg, center: true); S(ws, row, 4, bar.BarNo, bg: rowBg, center: true); S(ws, row, 5, Math.Round(bar.Kullanilan, 1), bg: rowBg); S(ws, row, 6, Math.Round(bar.Fire, 1), bg: fireBg); S(ws, row, 7, Math.Round(bar.FirePct, 1), bg: fireBg); S(ws, row, 8, pstr, bg: rowBg); row++; } sNo++; }
                app.Visible = true;
            }
            catch (Exception ex) { MessageBox.Show("Excel Hatasi: " + ex.Message); try { if (wb != null) wb.Close(false); } catch { } try { if (app != null) app.Quit(); } catch { } }
            finally { try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { } try { if (app != null) Marshal.ReleaseComObject(app); } catch { } GC.Collect(); GC.WaitForPendingFinalizers(); Cursor.Current = Cursors.Default; }
        }

        // ── YARDIMCILAR ───────────────────────────────────────────────────

        private List<StokBar> FFD(List<KesimParcasi> parcalar, double stokBoyu)
        {
            var sira = new List<KesimParcasi>(); foreach (var p in parcalar.OrderByDescending(x => x.Uzunluk)) for (int i = 0; i < p.Adet; i++) sira.Add(new KesimParcasi { ParcaNo = p.ParcaNo, Uzunluk = p.Uzunluk, Adet = 1 });
            var barlar = new List<StokBar>();
            foreach (var parca in sira) { if (parca.Uzunluk > stokBoyu) continue; StokBar hedef = null; foreach (var bar in barlar) { int topN = bar.Dilimler.Sum(d => d.Adet); double ekle = parca.Uzunluk + (topN > 0 ? KERF_MM : 0); if (bar.Kullanilan + ekle <= stokBoyu) { hedef = bar; break; } } if (hedef == null) { hedef = new StokBar { BarNo = barlar.Count + 1, StokBoyu = stokBoyu }; barlar.Add(hedef); } var mevcut = hedef.Dilimler.FirstOrDefault(d => Math.Abs(d.Uzunluk - parca.Uzunluk) < 1 && d.ParcaNo == parca.ParcaNo); if (mevcut != null) mevcut.Adet++; else hedef.Dilimler.Add(new KesimParcasi { ParcaNo = parca.ParcaNo, Uzunluk = parca.Uzunluk, Adet = 1 }); }
            return barlar;
        }

        private bool AgaBaglan() { var nr = new NETRESOURCE { dwType = 1, lpRemoteName = NET_PATH }; int ret = WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0); return ret == 0 || ret == 1219; }
        private void AgaBaglantiyiKes() { try { WNetCancelConnection2(NET_PATH, 0, false); } catch { } }
        private bool DosyaErisebilir(string yol) { if (string.IsNullOrEmpty(yol) || !File.Exists(yol)) return false; try { using (var fs = File.Open(yol, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) return true; } catch { return false; } }
        private string KullanicidanExcelSec() { AgaBaglan(); var ofd = new OpenFileDialog { Title = "ERP Excel dosyasını seçin", Filter = "Excel Dosyaları|*.xlsx;*.xls", InitialDirectory = Directory.Exists(NET_PATH) ? NET_PATH : "" }; return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : ""; }

        private string AcExcelVeKaydet(string dosyaAdi, Action<Excel.Worksheet> yaz)
        {
            Cursor.Current = Cursors.WaitCursor; string kaydedilenYol = "";
            Excel.Application app = null; Excel.Workbook wb = null; Excel.Worksheet ws = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Add(Type.Missing); ws = (Excel.Worksheet)wb.ActiveSheet; yaz(ws);
                if (!AgaBaglan()) { MessageBox.Show("Ağ bağlantısı kurulamadı!\n" + NET_PATH, "Ağ Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning); app.Visible = true; return ""; }
                string hedefYol = Path.Combine(NET_PATH, dosyaAdi);
                if (File.Exists(hedefYol))
                {
                    var cevap = MessageBox.Show($"📄 {dosyaAdi}\n\nÜzerine yazılsın mı?", "Dosya Zaten Var", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (cevap == DialogResult.Cancel) { app.Visible = true; return ""; }
                    if (cevap == DialogResult.No) { var sfd = new SaveFileDialog { Title = "Farklı kaydet", Filter = "Excel|*.xlsx", FileName = dosyaAdi }; if (sfd.ShowDialog() != DialogResult.OK) { app.Visible = true; return ""; } hedefYol = sfd.FileName; }
                }
                wb.SaveAs(hedefYol, Excel.XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                kaydedilenYol = hedefYol;
                MessageBox.Show("Excel kaydedildi:\n" + hedefYol + "\n\nExceli açmak için 📂 Exceli Aç butonunu kullanın.", "Kaydedildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                bool erisim = ex.HResult == unchecked((int)0x80070020) || ex.HResult == unchecked((int)0x80070005);
                MessageBox.Show("Excel kaydetme hatası:" + (erisim ? "\nDosya açık olabilir." : "") + "\n\n" + ex.Message);
                try { if (app != null) app.Visible = true; } catch { }
            }
            finally
            {
                AgaBaglantiyiKes();
                try { if (ws != null) Marshal.ReleaseComObject(ws); } catch { }
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
            return kaydedilenYol;
        }

        private void AcExcel(Action<Excel.Worksheet> yaz)
        {
            Cursor.Current = Cursors.WaitCursor; Excel.Application app = null; Excel.Workbook wb = null; Excel.Worksheet ws = null;
            try { app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false; wb = app.Workbooks.Add(Type.Missing); ws = (Excel.Worksheet)wb.ActiveSheet; yaz(ws); app.Visible = true; }
            catch (Exception ex) { MessageBox.Show("Excel: " + ex.Message); try { if (wb != null) wb.Close(false); } catch { } try { if (app != null) app.Quit(); } catch { } }
            finally { try { if (ws != null) Marshal.ReleaseComObject(ws); } catch { } try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { } try { if (app != null) Marshal.ReleaseComObject(app); } catch { } GC.Collect(); GC.WaitForPendingFinalizers(); Cursor.Current = Cursors.Default; }
        }

        private void S(Excel.Worksheet ws, int r, int c, object v, bool bold = false, int bg = 0, int fg = 0, bool center = false, int size = 0)
        { try { var cell = (Excel.Range)ws.Cells[r, c]; if (v != null) cell.Value2 = v; if (bold) cell.Font.Bold = true; if (bg != 0) cell.Interior.Color = bg; if (fg != 0) cell.Font.Color = fg; if (center) cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; if (size > 0) cell.Font.Size = size; } catch { } }
        private void H(Excel.Worksheet ws, int r, double h) { try { ((Excel.Range)ws.Rows[r]).RowHeight = h; } catch { } }
        private void Yaz(Excel.Worksheet ws, int r, int c, object v) { try { if (v != null) ((Excel.Range)ws.Cells[r, c]).Value2 = v; } catch { } }

        private Dictionary<string, double> SM()
        { var map = new Dictionary<string, double>(); foreach (DataGridViewRow r in dgvDosyalar.Rows) { string yol = r.Cells["colDosyaYolu"].Value?.ToString() ?? ""; double sip = N(r.Cells["colSiparisAdeti"].Value); map[yol] = sip > 0 ? sip : 1; } return map; }

        private Dictionary<string, MalzemeOzet> Ozet(Dictionary<string, double> sm)
        { var ozet = new Dictionary<string, MalzemeOzet>(StringComparer.OrdinalIgnoreCase); foreach (var v in _tumVeriler) { string key = PG(v.ParcaProfil) + "|" + KD(v.Kalite); double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1; double urt = sip * v.MontajAdeti * v.BirimAdet; if (!ozet.ContainsKey(key)) ozet[key] = new MalzemeOzet { Profil = PG(v.ParcaProfil), Kalite = KD(v.Kalite) }; ozet[key].ToplamAdet += urt; ozet[key].ToplamUzunluk += urt * v.Uzunluk; ozet[key].ToplamAgirlik += urt * v.Agirlik; } return ozet; }

        private bool IsNum(string v) { double d; return double.TryParse(v?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) || double.TryParse(v?.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out d); }

        private double N(object o)
        { if (o == null) return 0; string s = o.ToString().Trim(); if (string.IsNullOrEmpty(s)) return 0; if (s.Contains(",") && s.Contains(".")) { int a = s.LastIndexOf(','); int b = s.LastIndexOf('.'); s = a > b ? s.Replace(".", "") : s.Replace(",", ""); } double d; if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d)) return d; if (double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out d)) return d; return 0; }

        private void dgvSonuc_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}