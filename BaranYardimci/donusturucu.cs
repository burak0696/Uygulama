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

        private ToolStripMenuItem mnuExceliGoster;
        private ToolStripMenuItem mnuCivataYukle;
        private Panel pnlLegend;
        private Button btnExceliAc;

        // ════════════════════════════════════════════════════════════════
        public Donusturucu()
        {
            InitializeComponent();
            KontrolleriOlustur();
        }

        private void KontrolleriOlustur()
        {
            // ── colDurum ─────────────────────────────────────────��───────
            if (!dgvDosyalar.Columns.Contains("colDurum"))
                dgvDosyalar.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "colDurum", HeaderText = "Durum", FillWeight = 60f, ReadOnly = true });

            dgvDosyalar.CellFormatting -= dgvDosyalar_CellFormatting;
            dgvDosyalar.CellFormatting += dgvDosyalar_CellFormatting;
            dgvDosyalar.MouseUp -= dgvDosyalar_MouseUp;
            dgvDosyalar.MouseUp += dgvDosyalar_MouseUp;

            // ── Durum bar ─────────────────────────────────────────────────
            if (pnlDurumBar == null)
            {
                pnlDurumBar = new Panel { Dock = DockStyle.Bottom, Height = 36, BackColor = Color.FromArgb(230, 230, 235), Padding = new Padding(8, 0, 0, 0) };
                lblDurumIkon = new Label { Dock = DockStyle.Left, Width = 34, Font = new Font("Segoe UI", 13f), TextAlign = ContentAlignment.MiddleCenter };
                lblDurumMetin = new Label { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft };
                pnlDurumBar.Controls.Add(lblDurumMetin);
                pnlDurumBar.Controls.Add(lblDurumIkon);
                this.Controls.Add(pnlDurumBar);
            }

            // ── Sağ panel küçült ──────────────────────────────────────────
            try { pnlSag.Width = 120; } catch { }

            // ── Legend paneli (grpSonuc'un altına) ───────────────────────
            pnlLegend = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 112,
                BackColor = Color.FromArgb(245, 245, 250),
                Padding = new Padding(8, 6, 8, 4)
            };

            var legendBaslik = new Label
            {
                Text = "RENK KODLARI",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 80),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var legendFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false,
                BackColor = Color.Transparent
            };

            var legendSatirlar = new[]
            {
                (Color.FromArgb(255,180,180), Color.FromArgb(120,0,0),   "● Kırmızı  —  Dosya yüklendi, ERP aktarımı bekleniyor"),
                (Color.FromArgb(255,243,180), Color.FromArgb(110,80,0),  "● Sarı      —  ERP Exceli oluşturuldu, rota bekleniyor"),
                (Color.FromArgb(180,210,255), Color.FromArgb(0,50,130),  "● Mavi      —  Rotalar girildi, civata bekleniyor"),
                (Color.FromArgb(180,240,200), Color.FromArgb(0,90,0),    "● Yeşil     —  Tüm işlemler tamamlandı"),
            };

            foreach (var (bg, fg, txt) in legendSatirlar)
            {
                var lbl = new Label
                {
                    Text = txt,
                    AutoSize = false,
                    Width = 480,
                    Height = 20,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = fg,
                    BackColor = bg,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(4, 0, 0, 0),
                    Margin = new Padding(0, 1, 0, 1)
                };
                legendFlow.Controls.Add(lbl);
            }

            pnlLegend.Controls.Add(legendFlow);
            pnlLegend.Controls.Add(legendBaslik);

            try { pnlSonucButonlar2.Controls.Add(pnlLegend); pnlLegend.BringToFront(); } catch { }
            // Eğer grpSonuc'un parent'ına ekleyemiyorsak pnlSag'a ekle
            try
            {
                if (grpSonuc != null && grpSonuc.Parent != null)
                {
                    grpSonuc.Parent.Controls.Add(pnlLegend);
                    pnlLegend.BringToFront();
                }
            }
            catch { }

            // ── Exceli Aç butonu ──────────────────────────────────────────
            btnExceliAc = new Button
            {
                Text = "📂  Exceli Aç",
                Dock = DockStyle.Right,
                Width = 160,
                Height = 47,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 120, 180),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnExceliAc.FlatAppearance.BorderSize = 0;
            btnExceliAc.Click += btnExceliAc_Click;
            try { pnlSonucButonlar.Controls.Add(btnExceliAc); } catch { }

            // ── Context menu ──────────────────────────────────────────────
            if (ctxDosya == null) ctxDosya = new ContextMenuStrip { Font = new Font("Segoe UI", 10f) };
            ctxDosya.Items.Clear();

            if (mnuRotaGir == null) mnuRotaGir = new ToolStripMenuItem();
            if (mnuDosyaSil == null) mnuDosyaSil = new ToolStripMenuItem();
            mnuExceliGoster = new ToolStripMenuItem("📊  Exceli Göster");
            mnuCivataYukle = new ToolStripMenuItem("🔩  Civata Listelerini Yükle");

            mnuRotaGir.Text = "✏  Rota Gir  —  İmalat Kartlarını Bas";
            mnuRotaGir.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            mnuDosyaSil.Text = "🗑  Listeden Çıkar";
            mnuDosyaSil.ForeColor = Color.FromArgb(180, 40, 40);

            ctxDosya.Items.AddRange(new ToolStripItem[]
            {
                mnuRotaGir, new ToolStripSeparator(),
                mnuExceliGoster, new ToolStripSeparator(),
                mnuCivataYukle,  new ToolStripSeparator(),
                mnuDosyaSil
            });

            mnuRotaGir.Click -= mnuRotaGir_Click; mnuRotaGir.Click += mnuRotaGir_Click;
            mnuExceliGoster.Click -= mnuExceliGoster_Click; mnuExceliGoster.Click += mnuExceliGoster_Click;
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

        // ════════════════════════════════════════════════════════════════
        //  DESIGNER STUB'LARI
        // ════════════════════════════════════════════════════════════════

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

        // ════════════════════════════════════════════════════════════════
        //  FAVORİLER
        // ════════════════════════════════════════════════════════════════

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

        private void Donusturucu_Load(object sender, EventArgs e) => DurumGuncelle();

        // ═════════════════════════════════════════════════════════��══════
        //  DURUM BAR
        // ════════════════════════════════════════════════════════════════

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
                else if (_rotaKaydedilen.Count < aktarildi) Durum("✏", $"ERP aktarıldı  —  Rota girişi bekleniyor  ({_rotaKaydedilen.Count}/{aktarildi})", Color.FromArgb(0, 70, 160), Color.FromArgb(210, 230, 255));
                else if (_civataEklendi.Count < aktarildi) Durum("🔩", "Rota kaydedildi  —  Civata listesi bekleniyor  (Sağ klik → Civata Listelerini Yükle)", Color.FromArgb(110, 55, 0), Color.FromArgb(255, 240, 200));
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

        // ════════════════════════════════════════════════════════════════
        //  DGV RENK: Kırmızı→Yüklendi  Sarı→ERP  Mavi→Rota  Yeşil→Tam
        // ════════════════════════════════════════════════════════════════

        private void dgvDosyalar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvDosyalar.Rows[e.RowIndex];
            string yol = row.Cells["colDosyaYolu"].Value?.ToString() ?? "";

            if (_civataEklendi.Contains(yol))
            {
                // Yeşil — Tamamlandı
                row.DefaultCellStyle.BackColor = Color.FromArgb(180, 240, 200);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 90, 0);
            }
            else if (_rotaKaydedilen.Contains(yol))
            {
                // Mavi — Rotalar girildi
                row.DefaultCellStyle.BackColor = Color.FromArgb(180, 210, 255);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 50, 130);
            }
            else if (_erpAktarimYapilan.Contains(yol))
            {
                // Sarı — ERP exceli oluşturuldu
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 180);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(110, 80, 0);
            }
            else if (dgvDosyalar.Rows[e.RowIndex].Index >= 0)
            {
                // Kırmızı — sadece yüklendi
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 0, 0);
            }
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
                mnuCivataYukle.Enabled = aktarildi;
                mnuCivataYukle.ForeColor = aktarildi ? Color.FromArgb(120, 55, 0) : Color.Gray;
                mnuCivataYukle.Text = "🔩  Civata Listelerini Yükle"
                    + (!aktarildi ? "  (önce ERP aktarımı yapın)" : civataDone ? "  ✅ zaten eklendi" : "");
            }
            catch { }

            ctxDosya.Show(dgvDosyalar, e.Location);
        }

        // ────────────────────────────────────────────────────────────────
        //  Exceli Göster (sağ klik)
        // ────────────────────────────────────────────────────────────────

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

        // ────────────────────────────────────────────────────────────────
        //  Exceli Aç butonu (alttaki)
        // ────────────────────────────────────────────────────────────────

        private void btnExceliAc_Click(object sender, EventArgs e)
        {
            // Seçili satırın exceli varsa onu, yoksa son kaydedilen exceli aç
            string hedef = "";

            if (_sagKlikSatir >= 0 && _sagKlikSatir < dgvDosyalar.Rows.Count)
            {
                string yol = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
                if (_erpExcelYollari.ContainsKey(yol)) hedef = _erpExcelYollari[yol];
            }

            if (string.IsNullOrEmpty(hedef))
            {
                // Listede seçili satır yoksa son kaydedileni aç
                if (!string.IsNullOrEmpty(_sonKaydedilenExcel)) hedef = _sonKaydedilenExcel;
            }

            if (string.IsNullOrEmpty(hedef) || !File.Exists(hedef))
            {
                // Manuel seç
                var ofd = new OpenFileDialog { Title = "Excel dosyasını seçin", Filter = "Excel|*.xlsx;*.xls" };
                if (ofd.ShowDialog() != DialogResult.OK) return;
                hedef = ofd.FileName;
            }

            if (!DosyaErisebilir(hedef)) AgaBaglan();
            try { System.Diagnostics.Process.Start(hedef); }
            catch (Exception ex) { MessageBox.Show("Açılamadı: " + ex.Message); }
        }

        // ────────────────────────────────────────────────────────────────
        //  Rota Gir
        // ────────────────────────────────────────────────────────────────

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

        // ───────────────────────────────��────────────────────────────────
        //  Dosya Sil
        // ────────────────────────────────────────────────────────────────

        private void mnuDosyaSil_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0 || _sagKlikSatir >= dgvDosyalar.Rows.Count) return;
            string yol = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            string ad = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaAdi"].Value?.ToString() ?? "";
            if (MessageBox.Show($"'{ad}' listeden çıkarılsın mı?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _tumVeriler.RemoveAll(v => v.DosyaId == yol);
            _erpAktarimYapilan.Remove(yol); _rotaKaydedilen.Remove(yol);
            _civataEklendi.Remove(yol); _erpExcelYollari.Remove(yol);
            dgvDosyalar.Rows.RemoveAt(_sagKlikSatir);
            _sagKlikSatir = -1; DurumGuncelle();
        }

        // ════════════════════════════════════════════════════════════════
        //  CİVATA — her parça kendi ağırlığı ve adediyle
        // ════════════════════════════════════════════════════════════════

        private void mnuCivataYukle_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0 || _sagKlikSatir >= dgvDosyalar.Rows.Count) return;
            string dosyaYolu = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            if (!_erpAktarimYapilan.Contains(dosyaYolu))
            { MessageBox.Show("Önce ERP aktarımı yapılmalı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!_erpExcelYollari.ContainsKey(dosyaYolu))
            { MessageBox.Show("Bu dosya için ERP Excel bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            double siparisAdeti = N(dgvDosyalar.Rows[_sagKlikSatir].Cells["colSiparisAdeti"].Value);
            if (siparisAdeti <= 0) siparisAdeti = 1;

            var ofd = new OpenFileDialog
            {
                Title = "Civata listesi dosyalarını seçin  (*_B.doc / *_B.txt)",
                Filter = "Civata Listesi|*_B.doc;*_B.txt;*.doc;*.txt|Hepsi|*.*",
                Multiselect = true
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            var tumCivatalar = new List<CivataSatir>();
            foreach (string yol in ofd.FileNames)
            {
                var liste = CivataDocOku(yol);
                if (liste.Count > 0) tumCivatalar.AddRange(liste);
                else MessageBox.Show("Civata okunamadı:\n" + Path.GetFileName(yol), "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (tumCivatalar.Count == 0) { MessageBox.Show("Hiç civata satırı bulunamadı."); return; }

            // Her unique EslesKey için soru sor — otomatik eşleştirme yok
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
                    ToplamAdet = grp.Sum(c => c.Quantity),          // her grubun kendi adedi
                    ToplamAgirlik = Math.Round(grp.Sum(c => c.ToplamAgirlik), 3), // her grubun kendi ağırlığı
                    Satirlar = grp.Select(c =>
                        $"► {c.BoltListAdi,-22}  {c.Tanim,-30}  Adet:{c.Quantity,6}  Ağ:{c.ToplamAgirlik:0.###} kg").ToList()
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
            if (!DosyaErisebilir(excelYol))
            { MessageBox.Show("ERP Excel'e erişilemiyor:\n" + excelYol, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            Cursor.Current = Cursors.WaitCursor;
            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Open(excelYol, false, false,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                var ws = (Excel.Worksheet)wb.Worksheets[1];

                object projeNoV = null;
                int lastRow = ws.UsedRange.Rows.Count;
                for (int r = 2; r <= lastRow; r++)
                { projeNoV = ((Excel.Range)ws.Cells[r, 1]).Value2; if (projeNoV != null) break; }

                int satirNo = lastRow + 1;

                // Her civata satırı kendi ağırlığı ve adediyle ayrı ayrı yazılır
                foreach (var grp in civatalar.GroupBy(c => c.BoltListAdi))
                {
                    foreach (var cv in grp.OrderBy(c => c.Dia).ThenBy(c => c.Length))
                    {
                        bool eslestiMi = esles.ContainsKey(cv.EslesKey);
                        string hmNo = eslestiMi ? esles[cv.EslesKey].No : "";

                        // Col1  = Proje No
                        // Col2  = Poz No         → BoltList adı (ürün adı)
                        // Col3  = Poz Açıklama   → boş
                        // Col4  = Ana Poz No     → boş
                        // Col5  = Poz Miktar     → sipariş adeti
                        // Col6  = Poz Ağırlık    → bu civata tipinin kendi ağırlığı (total değil)
                        // Col7  = Bileşen Türü   → MADDE
                        // Col8  = Bileşen No     → eşleşen hammadde no
                        // Col9  = Bileşen Miktar → bu civata tipinin kendi adedi
                        // Col10 = İşlem Sırası   → boş
                        Yaz(ws, satirNo, 1, projeNoV);
                        Yaz(ws, satirNo, 2, grp.Key);
                        Yaz(ws, satirNo, 3, null);
                        Yaz(ws, satirNo, 4, null);
                        Yaz(ws, satirNo, 5, siparisAdeti);
                        Yaz(ws, satirNo, 6, cv.ToplamAgirlik);  // ← bu satırın kendi ağırlığı
                        Yaz(ws, satirNo, 7, "MADDE");
                        Yaz(ws, satirNo, 8, hmNo);
                        Yaz(ws, satirNo, 9, cv.Quantity);        // ��� bu satırın kendi adedi
                        Yaz(ws, satirNo, 10, null);
                        satirNo++;
                    }
                }

                wb.Save();
                MessageBox.Show($"{civatalar.Count} civata satırı eklendi.\n\n{excelYol}",
                    "Civata Eklendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // ════════════════════════════════════════════════════════════════
        //  DOSYA OKUMA
        // ════════════════════════════════════════════════════════════════

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
            if (ok > 0) { MessageBox.Show(ok + " dosya, " + top + " parça."); DurumGuncelle(); }
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
                            list.Add(new HamSatir { DosyaId = yol, DosyaAdi = ad, MontajNo = asm, MontajAdeti = madet, ParcaNo = tp[0], BirimAdet = N(tp[1]), ParcaProfil = tp[2], Kalite = tp.Count >= 6 ? tp[3] : "", Uzunluk = N(tp.Count >= 6 ? tp[4] : tp[3]), Agirlik = N(tp.Count >= 6 ? tp[5] : tp[4]) });
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); return 0; }
            _tumVeriler.AddRange(list);
            return list.Count;
        }

        // ════════════════════════════════════════════════════════════════
        //  NORMALİZASYON
        // ════════════════════════════════════════════════════════════════

        private string PG(string p) { if ((p.StartsWith("PL", StringComparison.OrdinalIgnoreCase) || p.StartsWith("FPL", StringComparison.OrdinalIgnoreCase)) && p.Contains("*")) return p.Split('*')[0]; return p; }
        private string KD(string k) { if (string.IsNullOrWhiteSpace(k)) return k; switch (k.Trim().ToUpper()) { case "ST37": return "S235JR"; case "ST52": return "S355JR"; case "ST44": return "S275JR"; default: return k; } }
        private static string TemizleDocSoneki(string ad) => Regex.Replace(ad, @"[-_][A-Za-z]$", "");

        // ════════════════════════════════════════════════════════════════
        //  BUTONLAR
        // ════════════════════════════════════════════════════════════════

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
            int musteriID = -1; string musteriAdi = "", durum = "Uretim Bekliyor";
            using (var frm = new MusteriSecForm()) { frm.StartPosition = FormStartPosition.CenterParent; if (frm.ShowDialog() != DialogResult.OK) return; musteriID = frm.SecilenMusteriID; musteriAdi = (frm.SecilenMusteriAd + " " + frm.SecilenMusteriSoyad).Trim(); durum = frm.SecilenMalzemeDurum; }
            var tf = new FrmTerminSec(); if (tf.ShowDialog() != DialogResult.OK) return;
            DateTime termin = tf.SecilenTarih; Cursor.Current = Cursors.WaitCursor;
            try
            {
                var sm = SM(); int n = 0;
                foreach (var v in _tumVeriler) { double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1; double urt = sip * v.MontajAdeti * v.BirimAdet; DB.Execute(@"INSERT INTO MalzemeKayit(MusteriID,MusteriAdi,DosyaAdi,SiparisAdeti,AssemblyNo,AssemblyAdeti,PozNo,Profil,Kalite,BirimAdet,UretilecekAdet,Uzunluk,ParcaAgirlik,ToplamAgirlik,MalzemeDurum,KayitTarihi,TerminTarihi)VALUES(@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m,@n,@o,GETDATE(),@p)", new SqlParameter("@a", musteriID), new SqlParameter("@b", musteriAdi), new SqlParameter("@c", v.DosyaAdi), new SqlParameter("@d", sip), new SqlParameter("@e", v.MontajNo ?? ""), new SqlParameter("@f", v.MontajAdeti), new SqlParameter("@g", v.ParcaNo ?? ""), new SqlParameter("@h", PG(v.ParcaProfil)), new SqlParameter("@i", KD(v.Kalite)), new SqlParameter("@j", v.BirimAdet), new SqlParameter("@k", urt), new SqlParameter("@l", v.Uzunluk), new SqlParameter("@m", v.Agirlik), new SqlParameter("@n", Math.Round(urt * v.Agirlik, 2)), new SqlParameter("@o", durum), new SqlParameter("@p", termin)); n++; }
                MessageBox.Show(n + " kayit eklendi.");
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { Cursor.Current = Cursors.Default; }
        }

        // ════════════════════════════════════════════════════════════════
        //  ERP AKTARİM
        // ════════════════════════════════════════════════════════════════

        private void btnErpAktarim_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }
            dgvDosyalar.EndEdit();

            string projeNo = "";
            using (var frm = new Form()) { frm.Text = "Proje No"; frm.Size = new Size(360, 140); frm.StartPosition = FormStartPosition.CenterParent; frm.FormBorderStyle = FormBorderStyle.FixedDialog; frm.MaximizeBox = false; frm.MinimizeBox = false; var lbl = new Label { Text = "Proje No giriniz:", Left = 12, Top = 14, AutoSize = true }; var txt = new TextBox { Left = 12, Top = 34, Width = 320 }; var btn = new Button { Text = "Tamam", Left = 255, Top = 62, Width = 77 }; btn.Click += (s2, e2) => frm.DialogResult = DialogResult.OK; frm.Controls.AddRange(new Control[] { lbl, txt, btn }); frm.AcceptButton = btn; if (frm.ShowDialog(this) != DialogResult.OK) return; projeNo = txt.Text.Trim(); }
            if (string.IsNullOrEmpty(projeNo)) { MessageBox.Show("Proje No boş olamaz!"); return; }

            var sm = SM(); var ozet = Ozet(sm); if (ozet.Count == 0) return;

            var hm = new List<HammaddeItem>();
            try { using (var conn = new SqlConnection(DB.ConnStr)) { conn.Open(); using (var cmd = new SqlCommand("SELECT no,adi FROM hammadde", conn)) using (var dr = cmd.ExecuteReader()) while (dr.Read()) hm.Add(new HammaddeItem { No = dr["no"].ToString(), Adi = dr["adi"].ToString() }); } }
            catch (Exception ex) { MessageBox.Show("DB: " + ex.Message); return; }

            var esles = new Dictionary<string, HammaddeItem>(StringComparer.OrdinalIgnoreCase);
            var bulunamadi = new List<BulunamadiItem>();
            var profilSatirlar = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var v in _tumVeriler) { string k2 = PG(v.ParcaProfil) + "|" + KD(v.Kalite); double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1; double urt = sip * v.MontajAdeti * v.BirimAdet; if (!profilSatirlar.ContainsKey(k2)) profilSatirlar[k2] = new List<string>(); profilSatirlar[k2].Add($"► {v.ParcaNo,-14}  Montaj:{v.MontajNo,-12}  Adet:{urt,6:0.##}  Boy:{v.Uzunluk,7:0}mm  [{v.DosyaAdi}]"); }
            foreach (var oz in ozet.Values) { string key = oz.Profil + "|" + oz.Kalite; bool b = false; string vUp = oz.Profil.ToUpper().Replace(" ", ""); foreach (var h in hm) if (h.Adi.ToUpper().Replace(" ", "").Contains(vUp)) { esles[key] = h; b = true; break; } if (!b) bulunamadi.Add(new BulunamadiItem { Profil = oz.Profil, Kalite = oz.Kalite, ToplamAdet = oz.ToplamAdet, ToplamUzunluk = oz.ToplamUzunluk, ToplamAgirlik = Math.Round(oz.ToplamAgirlik, 2), Satirlar = profilSatirlar.ContainsKey(key) ? profilSatirlar[key] : new List<string>() }); }
            if (bulunamadi.Count > 0) { using (var frm = new FrmErpManuelEslestir(bulunamadi, _imalatKartlariKlasoru)) { frm.StartPosition = FormStartPosition.CenterParent; if (frm.ShowDialog(this) != DialogResult.OK) return; foreach (var kv in frm.Sonuclar) esles[kv.Key] = new HammaddeItem { No = kv.Value.No, Adi = kv.Value.Ad }; } }

            string ilkAd = dgvDosyalar.Rows.Count > 0 ? Path.GetFileNameWithoutExtension(dgvDosyalar.Rows[0].Cells["colDosyaAdi"].Value?.ToString() ?? "") : "";
            string ciktiAdi = (string.IsNullOrEmpty(ilkAd) ? "ERP" : ilkAd) + "_ERPAKTARIM1.xlsx";

            string kaydedilenYol = AcExcelVeKaydet(ciktiAdi, xa =>
            {
                string[] headers = { "Proje No", "Poz No", "Poz Açıklaması", "Ana Poz No", "Poz Miktar", "Poz Ağırlık", "Bileşen Türü", "Bileşen No", "Bileşen Miktar", "İşlem Sırası" };
                for (int i = 0; i < headers.Length; i++) Yaz(xa, 1, i + 1, headers[i]);
                int excelRow = 2;
                var yazılanAnaPozlar = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var docGroup in _tumVeriler.GroupBy(v => v.DosyaId).OrderBy(g => g.Key))
                {
                    string docAdi = TemizleDocSoneki(Path.GetFileNameWithoutExtension(docGroup.First().DosyaAdi));
                    double sip = sm.ContainsKey(docGroup.Key) ? sm[docGroup.Key] : 1;

                    foreach (var montajGroup in docGroup.GroupBy(v => (v.MontajNo ?? "").Trim()).OrderBy(g => g.Key))
                    {
                        string montajNo = montajGroup.Key;
                        bool hasMontaj = !string.IsNullOrWhiteSpace(montajNo);
                        bool tekParca = montajGroup.Count() == 1;

                        if (hasMontaj && !tekParca)
                        {
                            string apKey = docGroup.Key + "||" + montajNo;
                            if (!yazılanAnaPozlar.Contains(apKey))
                            {
                                yazılanAnaPozlar.Add(apKey);
                                double mAg = montajGroup.Sum(v => sip * v.MontajAdeti * v.BirimAdet * v.Agirlik);
                                Yaz(xa, excelRow, 1, projeNo); Yaz(xa, excelRow, 2, montajNo); Yaz(xa, excelRow, 3, montajNo);
                                Yaz(xa, excelRow, 4, docAdi);
                                Yaz(xa, excelRow, 5, Math.Round(montajGroup.First().MontajAdeti * sip, 2));
                                Yaz(xa, excelRow, 6, Math.Round(mAg, 2));
                                Yaz(xa, excelRow, 7, "Kaynak"); Yaz(xa, excelRow, 8, "Kaynaklama");
                                Yaz(xa, excelRow, 9, 1); Yaz(xa, excelRow, 10, ""); excelRow++;
                            }
                        }

                        foreach (var v in montajGroup.OrderBy(x => x.ParcaNo))
                        {
                            string anaPoz = (hasMontaj && !tekParca) ? montajNo : docAdi;
                            string key = PG(v.ParcaProfil) + "|" + KD(v.Kalite);
                            bool e2 = esles.ContainsKey(key);
                            double urt = sip * v.MontajAdeti * v.BirimAdet;
                            Yaz(xa, excelRow, 1, projeNo); Yaz(xa, excelRow, 2, (v.ParcaNo ?? "").Trim());
                            Yaz(xa, excelRow, 3, (v.ParcaNo ?? "").Trim()); Yaz(xa, excelRow, 4, anaPoz);
                            Yaz(xa, excelRow, 5, Math.Round(v.MontajAdeti * sip, 2));
                            Yaz(xa, excelRow, 6, Math.Round(urt * v.Agirlik, 2));
                            Yaz(xa, excelRow, 7, "Madde"); Yaz(xa, excelRow, 8, e2 ? esles[key].No : "");
                            Yaz(xa, excelRow, 9, Math.Round(urt, 2)); Yaz(xa, excelRow, 10, 1); excelRow++;
                        }
                    }
                }
            });

            if (!string.IsNullOrEmpty(kaydedilenYol))
            {
                _sonKaydedilenExcel = kaydedilenYol;
                foreach (DataGridViewRow row in dgvDosyalar.Rows)
                {
                    string y = row.Cells["colDosyaYolu"].Value?.ToString() ?? "";
                    if (string.IsNullOrEmpty(y)) continue;
                    _erpAktarimYapilan.Add(y); _erpExcelYollari[y] = kaydedilenYol;
                    try { row.Cells["colDurum"].Value = "🟡 ERP Aktarıldı — rota bekleniyor"; } catch { }
                }
                dgvDosyalar.Refresh(); DurumGuncelle();
            }
        }

        private void btnGalvanizEkran_Click(object sender, EventArgs e) => new GalvanizKontrol().Show();

        private void btnHamDataExcel_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) return; dgvDosyalar.EndEdit(); var sm = SM();
            AcExcel(xa => { int r = 1; Yaz(xa, r, 1, "DOSYA"); Yaz(xa, r, 2, "SIPARIS"); Yaz(xa, r, 3, "MONTAJ"); Yaz(xa, r, 4, "MADET"); Yaz(xa, r, 5, "PARCA"); Yaz(xa, r, 6, "PROFIL"); Yaz(xa, r, 7, "KALITE"); Yaz(xa, r, 8, "BADET"); Yaz(xa, r, 9, "UADET"); Yaz(xa, r, 10, "UZUNLUK"); Yaz(xa, r, 11, "PAG"); Yaz(xa, r, 12, "TAG"); r = 2; foreach (var v in _tumVeriler) { double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1; double urt = sip * v.MontajAdeti * v.BirimAdet; Yaz(xa, r, 1, v.DosyaAdi); Yaz(xa, r, 2, sip); Yaz(xa, r, 3, v.MontajNo); Yaz(xa, r, 4, v.MontajAdeti); Yaz(xa, r, 5, v.ParcaNo); Yaz(xa, r, 6, PG(v.ParcaProfil)); Yaz(xa, r, 7, KD(v.Kalite)); Yaz(xa, r, 8, v.BirimAdet); Yaz(xa, r, 9, urt); Yaz(xa, r, 10, v.Uzunluk); Yaz(xa, r, 11, v.Agirlik); Yaz(xa, r, 12, Math.Round(urt * v.Agirlik, 2)); r++; } });
        }

        private void btnMalzemeExcel_Click(object sender, EventArgs e)
        {
            if (dgvSonuc.Rows.Count == 0) { MessageBox.Show("Önce HESAPLA butonuna bas."); return; }
            dgvDosyalar.EndEdit(); Cursor.Current = Cursors.WaitCursor;
            var sm = SM(); var dict = new Dictionary<string, List<KesimParcasi>>(StringComparer.OrdinalIgnoreCase); var kgPerM = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            foreach (var v in _tumVeriler) { if (v.Uzunluk <= 0) continue; double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1; int adet = (int)Math.Round(sip * v.MontajAdeti * v.BirimAdet); if (adet <= 0) continue; string key = PG(v.ParcaProfil) + "||" + KD(v.Kalite); if (!dict.ContainsKey(key)) dict[key] = new List<KesimParcasi>(); var m2 = dict[key].FirstOrDefault(x => x.ParcaNo == v.ParcaNo && Math.Abs(x.Uzunluk - v.Uzunluk) < 0.1); if (m2 != null) m2.Adet += adet; else dict[key].Add(new KesimParcasi { ParcaNo = v.ParcaNo, Uzunluk = v.Uzunluk, Adet = adet }); if (v.Uzunluk > 0 && v.Agirlik > 0 && !kgPerM.ContainsKey(key)) kgPerM[key] = v.Agirlik / (v.Uzunluk / 1000.0); }
            var sonuclar = new List<ProfilSonuc>();
            foreach (var kv in dict.OrderBy(x => x.Key)) { string[] pts = kv.Key.Split(new[] { "||" }, StringSplitOptions.None); string profil = pts[0], kalite = pts.Length > 1 ? pts[1] : ""; double maxUz = kv.Value.Max(x => x.Uzunluk); double kgM = kgPerM.ContainsKey(kv.Key) ? kgPerM[kv.Key] : 0; double netMm = kv.Value.Sum(x => x.Uzunluk * x.Adet); var bars6 = maxUz <= 6000 ? FFD(kv.Value, 6000) : new List<StokBar>(); var bars12 = FFD(kv.Value, 12000); double ts6 = bars6.Count * 6000.0, ts12 = bars12.Count * 12000.0; double fp6 = ts6 > 0 ? bars6.Sum(b => b.Fire) / ts6 * 100 : 999; double fp12 = ts12 > 0 ? bars12.Sum(b => b.Fire) / ts12 * 100 : 999; List<StokBar> enIyi = bars12; double enIyiBoy = 12000, enIyiFire = fp12; foreach (double aday in STOK_ADAYLARI) { if (aday < maxUz) continue; var b2 = FFD(kv.Value, aday); if (b2.Count == 0) continue; double ts = b2.Count * aday; double fp = ts > 0 ? b2.Sum(x => x.Fire) / ts * 100 : 999; if (fp < enIyiFire - 0.5 || (Math.Abs(fp - enIyiFire) < 0.5 && b2.Count < enIyi.Count)) { enIyiFire = fp; enIyiBoy = aday; enIyi = b2; } } double oneriMetre = enIyi.Count * enIyiBoy / 1000.0; string not2 = Math.Abs(enIyiBoy - 6000) < 1 ? "6m yeterli" : Math.Abs(enIyiBoy - 12000) < 1 ? "12m tercih edildi" : (enIyiBoy / 1000.0).ToString("0.0") + "m alirsan daha az fire"; sonuclar.Add(new ProfilSonuc { Profil = profil, Kalite = kalite, KgPerMetre = Math.Round(kgM, 3), ToplamNetMetre = Math.Round(netMm / 1000.0, 2), ToplamKg = Math.Round(netMm / 1000.0 * kgM, 1), Bar6Adet = bars6.Count, Bar6ToplamMetre = Math.Round(ts6 / 1000.0, 1), Bar6ToplamKg = Math.Round(ts6 / 1000.0 * kgM, 1), Bar6FirePct = Math.Round(fp6, 1), Bar12Adet = bars12.Count, Bar12ToplamMetre = Math.Round(ts12 / 1000.0, 1), Bar12ToplamKg = Math.Round(ts12 / 1000.0 * kgM, 1), Bar12FirePct = Math.Round(fp12, 1), OnerilenAdet = enIyi.Count, OnerilenBoy = enIyiBoy, OnerilenMetre = Math.Round(oneriMetre, 1), OnerilenKg = Math.Round(oneriMetre * kgM, 1), OnerilenFirePct = Math.Round(enIyiFire, 1), OneriNot = not2, OnerilenBarlar = enIyi }); }

            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false; wb = app.Workbooks.Add(Type.Missing); Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1]; int row = 1;
                S(ws, row, 1, "SATIN ALIM LİSTESİ  —  " + DateTime.Now.ToString("dd.MM.yyyy"), bold: true, bg: RenkBaslik, fg: RenkBeyaz, size: 13); H(ws, row, 18); row++;
                string[] h1 = { "PROFİL", "KALİTE", "kg/m", "GEREKEN NET METRE", "GEREKEN NET KG", "6m BAR — ADET", "6m — TOPLAM METRE", "6m — TOPLAM KG", "6m FIRE %", "12m BAR — ADET", "12m — TOPLAM METRE", "12m — TOPLAM KG", "12m FIRE %", "ÖNERİLEN", "NEDEN" };
                for (int i = 0; i < h1.Length; i++) S(ws, row, i + 1, h1[i], bold: true, bg: RenkMavi, fg: RenkBeyaz, center: true); H(ws, row, 20); row++; int sNo = 0;
                foreach (var s in sonuclar) { int bg6 = s.Bar6FirePct > 20 ? RenkKirmizi : s.Bar6FirePct > 10 ? RenkSari : RenkYesil; int bg12 = s.Bar12FirePct > 20 ? RenkKirmizi : s.Bar12FirePct > 10 ? RenkSari : RenkYesil; int bgRow = sNo % 2 == 0 ? RenkAcik : RenkBeyaz; string oneri = s.Bar6Adet == 0 ? s.OnerilenAdet + " adet " + (s.OnerilenBoy / 1000.0).ToString("0.0") + "m" : Math.Abs(s.OnerilenBoy - 6000) < 1 ? s.OnerilenAdet + " adet 6m" : Math.Abs(s.OnerilenBoy - 12000) < 1 ? s.OnerilenAdet + " adet 12m" : s.OnerilenAdet + " adet " + (s.OnerilenBoy / 1000.0).ToString("0.0") + "m"; S(ws, row, 1, s.Profil, bg: bgRow); S(ws, row, 2, s.Kalite, bg: bgRow); S(ws, row, 3, s.KgPerMetre, center: true, bg: bgRow); S(ws, row, 4, s.ToplamNetMetre + " m", center: true, bg: bgRow); S(ws, row, 5, s.ToplamKg + " kg", center: true, bg: bgRow, bold: true); if (s.Bar6Adet > 0) { S(ws, row, 6, s.Bar6Adet, center: true, bg: bgRow, bold: true); S(ws, row, 7, s.Bar6ToplamMetre + " m", center: true, bg: bgRow); S(ws, row, 8, s.Bar6ToplamKg + " kg", center: true, bg: bgRow); S(ws, row, 9, "%" + s.Bar6FirePct.ToString("0.0"), center: true, bg: bg6); } else { S(ws, row, 6, "SIĞMAZ", center: true, bg: RenkKirmizi); S(ws, row, 7, "-", center: true, bg: RenkGri); S(ws, row, 8, "-", center: true, bg: RenkGri); S(ws, row, 9, "-", center: true, bg: RenkGri); } S(ws, row, 10, s.Bar12Adet, center: true, bg: bgRow, bold: true); S(ws, row, 11, s.Bar12ToplamMetre + " m", center: true, bg: bgRow); S(ws, row, 12, s.Bar12ToplamKg + " kg", center: true, bg: bgRow); S(ws, row, 13, "%" + s.Bar12FirePct.ToString("0.0"), center: true, bg: bg12); int bgO = s.OnerilenFirePct > 20 ? RenkKirmizi : s.OnerilenFirePct > 10 ? RenkSari : RenkYesil; S(ws, row, 14, oneri + "  (%" + s.OnerilenFirePct.ToString("0.0") + " fire)", bold: true, bg: bgO, center: true); S(ws, row, 15, s.OneriNot, bg: bgRow); H(ws, row, 18); row++; sNo++; }
                try { int[] w = { 16, 10, 7, 16, 14, 14, 16, 14, 10, 14, 16, 14, 10, 28, 26 }; for (int i = 0; i < w.Length; i++) ((Excel.Range)ws.Columns[i + 1]).ColumnWidth = w[i]; } catch { }
                row += 2; S(ws, row, 1, "KESİM PLANI", bold: true, bg: RenkBaslik, fg: RenkBeyaz, size: 12); H(ws, row, 22); row++;
                string[] h2 = { "PROFİL", "KALİTE", "BAR BOYU", "BAR NO", "KULLANILAN (mm)", "FIRE (mm)", "FIRE %", "PARÇALAR" };
                for (int i = 0; i < h2.Length; i++) S(ws, row, i + 1, h2[i], bold: true, bg: RenkMavi, fg: RenkBeyaz, center: true); try { ((Excel.Range)ws.Columns[8]).ColumnWidth = 70; } catch { }
                H(ws, row, 20); row++; sNo = 0;
                foreach (var s in sonuclar) { string boyStr = (s.OnerilenBoy / 1000.0).ToString("0.0") + " m"; foreach (var bar in s.OnerilenBarlar) { string pstr = string.Join("  |  ", bar.Dilimler.Select(d => d.ParcaNo + " × " + d.Uzunluk.ToString("0") + "mm × " + d.Adet + " adet")); if (pstr.Length > 3000) pstr = pstr.Substring(0, 3000); int bgRow = sNo % 2 == 0 ? RenkAcik : RenkBeyaz; int fBg = bar.FirePct > 20 ? RenkKirmizi : bar.FirePct > 10 ? RenkSari : RenkYesil; S(ws, row, 1, s.Profil, bg: bgRow); S(ws, row, 2, s.Kalite, bg: bgRow); S(ws, row, 3, boyStr, center: true, bg: bgRow, bold: true); S(ws, row, 4, "Bar " + bar.BarNo, center: true, bg: bgRow, bold: true); S(ws, row, 5, Math.Round(bar.Kullanilan, 0), center: true, bg: bgRow); S(ws, row, 6, Math.Round(bar.Fire, 0), center: true, bg: bgRow); S(ws, row, 7, "%" + bar.FirePct.ToString("0.0"), center: true, bg: fBg, bold: bar.FirePct > 20); S(ws, row, 8, pstr, bg: bgRow); H(ws, row, 17); row++; sNo++; } try { ((Excel.Range)ws.Rows[row]).RowHeight = 6; } catch { } row++; }
                app.Visible = true;
            }
            catch (Exception ex) { MessageBox.Show("Excel Hatasi: " + ex.Message); try { if (wb != null) wb.Close(false); } catch { } try { if (app != null) app.Quit(); } catch { } }
            finally { try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { } try { if (app != null) Marshal.ReleaseComObject(app); } catch { } GC.Collect(); GC.WaitForPendingFinalizers(); Cursor.Current = Cursors.Default; }
        }

        // ════════════════════════════════════════════════════════════════
        //  YARDIMCILAR
        // ════════════════════════════════════════════════════════════════

        private List<StokBar> FFD(List<KesimParcasi> parcalar, double stokBoyu)
        {
            var sira = new List<KesimParcasi>(); foreach (var p in parcalar.OrderByDescending(x => x.Uzunluk)) for (int i = 0; i < p.Adet; i++) sira.Add(new KesimParcasi { ParcaNo = p.ParcaNo, Uzunluk = p.Uzunluk, Adet = 1 });
            var barlar = new List<StokBar>();
            foreach (var parca in sira) { if (parca.Uzunluk > stokBoyu) continue; StokBar hedef = null; foreach (var bar in barlar) { int topN = bar.Dilimler.Sum(d => d.Adet); double ekle = parca.Uzunluk + (topN > 0 ? KERF_MM : 0); if (bar.Kullanilan + ekle <= stokBoyu) { hedef = bar; break; } } if (hedef == null) { hedef = new StokBar { BarNo = barlar.Count + 1, StokBoyu = stokBoyu }; barlar.Add(hedef); } var d2 = hedef.Dilimler.FirstOrDefault(x => x.ParcaNo == parca.ParcaNo && Math.Abs(x.Uzunluk - parca.Uzunluk) < 0.1); if (d2 != null) d2.Adet++; else hedef.Dilimler.Add(new KesimParcasi { ParcaNo = parca.ParcaNo, Uzunluk = parca.Uzunluk, Adet = 1 }); }
            return barlar;
        }

        private bool AgaBaglan() { var nr = new NETRESOURCE { dwType = 1, lpRemoteName = NET_PATH }; int ret = WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0); return ret == 0 || ret == 1219; }
        private void AgaBaglantiyiKes() { try { WNetCancelConnection2(NET_PATH, 0, false); } catch { } }
        private bool DosyaErisebilir(string yol) { if (string.IsNullOrEmpty(yol) || !File.Exists(yol)) return false; try { using (var fs = File.Open(yol, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) return true; } catch { return false; } }
        private string KullanicidanExcelSec() { AgaBaglan(); var ofd = new OpenFileDialog { Title = "ERP Excel dosyasını seçin", Filter = "Excel Dosyaları|*.xlsx;*.xls", InitialDirectory = Directory.Exists(NET_PATH) ? NET_PATH : Environment.GetFolderPath(Environment.SpecialFolder.Desktop) }; return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : ""; }

        private string AcExcelVeKaydet(string dosyaAdi, Action<Excel.Worksheet> yaz)
        {
            Cursor.Current = Cursors.WaitCursor; string kaydedilenYol = "";
            Excel.Application app = null; Excel.Workbook wb = null; Excel.Worksheet ws = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false; wb = app.Workbooks.Add(Type.Missing); ws = (Excel.Worksheet)wb.ActiveSheet; yaz(ws);
                if (!AgaBaglan()) { MessageBox.Show("Ağ bağlantısı kurulamadı!\n" + NET_PATH, "Ağ Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning); app.Visible = true; return ""; }
                string hedefYol = Path.Combine(NET_PATH, dosyaAdi);
                if (File.Exists(hedefYol)) { var cevap = MessageBox.Show($"📄 {dosyaAdi}\n\nÜzerine yazılsın mı?", "Dosya Zaten Var", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question); if (cevap == DialogResult.Cancel) { app.Visible = true; return ""; } if (cevap == DialogResult.No) { string yeniIsim = ""; using (var frm = new Form()) { frm.Text = "Yeni Dosya Adı"; frm.Size = new Size(420, 130); frm.StartPosition = FormStartPosition.CenterParent; frm.FormBorderStyle = FormBorderStyle.FixedDialog; frm.MaximizeBox = false; frm.MinimizeBox = false; var txt2 = new TextBox { Left = 12, Top = 12, Width = 380, Text = Path.GetFileNameWithoutExtension(dosyaAdi) + "_yeni" }; var btn2 = new Button { Text = "Kaydet", Left = 320, Top = 48, Width = 80 }; btn2.Click += (s2, e2) => frm.DialogResult = DialogResult.OK; frm.Controls.AddRange(new Control[] { txt2, btn2 }); frm.AcceptButton = btn2; if (frm.ShowDialog() != DialogResult.OK) { app.Visible = true; return ""; } yeniIsim = txt2.Text.Trim(); if (!yeniIsim.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)) yeniIsim += ".xlsx"; } hedefYol = Path.Combine(NET_PATH, yeniIsim); } else { try { File.Delete(hedefYol); } catch (Exception ex2) { MessageBox.Show("Eski dosya silinemedi:\n" + ex2.Message); app.Visible = true; return ""; } } }
                wb.SaveAs(hedefYol, Excel.XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                kaydedilenYol = hedefYol; MessageBox.Show("Excel kaydedildi:\n" + hedefYol, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information); app.Visible = true;
            }
            catch (Exception ex) { bool erisim = ex.HResult == unchecked((int)0x80070020) || ex.HResult == unchecked((int)0x80070005); MessageBox.Show("Excel kaydetme hatası:" + (erisim ? "\nDosya açık olabilir." : "") + "\n\n" + ex.Message); kaydedilenYol = ""; try { if (wb != null) wb.Close(false); } catch { } try { if (app != null) app.Quit(); } catch { } }
            finally { AgaBaglantiyiKes(); try { if (ws != null) Marshal.ReleaseComObject(ws); } catch { } try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { } try { if (app != null) Marshal.ReleaseComObject(app); } catch { } GC.Collect(); GC.WaitForPendingFinalizers(); Cursor.Current = Cursors.Default; }
            return kaydedilenYol;
        }

        private void AcExcel(Action<Excel.Worksheet> yaz)
        {
            Cursor.Current = Cursors.WaitCursor; Excel.Application app = null; Excel.Workbook wb = null; Excel.Worksheet ws = null;
            try { app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false; wb = app.Workbooks.Add(Type.Missing); ws = (Excel.Worksheet)wb.ActiveSheet; yaz(ws); app.Visible = true; }
            catch (Exception ex) { MessageBox.Show("Excel: " + ex.Message); try { if (wb != null) wb.Close(false); } catch { } try { if (app != null) app.Quit(); } catch { } }
            finally { try { if (ws != null) Marshal.ReleaseComObject(ws); } catch { } try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { } try { if (app != null) Marshal.ReleaseComObject(app); } catch { } GC.Collect(); GC.WaitForPendingFinalizers(); Cursor.Current = Cursors.Default; }
        }

        private void S(Excel.Worksheet ws, int r, int c, object v, bool bold = false, int bg = 0, int fg = 0, bool center = false, int size = 0) { try { var cell = (Excel.Range)ws.Cells[r, c]; if (v != null) cell.Value2 = v; if (bold) cell.Font.Bold = true; if (bg != 0) cell.Interior.Color = bg; if (fg != 0) cell.Font.Color = fg; if (size > 0) cell.Font.Size = size; if (center) cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; } catch { } }
        private void H(Excel.Worksheet ws, int r, double h) { try { ((Excel.Range)ws.Rows[r]).RowHeight = h; } catch { } }
        private void Yaz(Excel.Worksheet ws, int r, int c, object v) { try { if (v != null) ((Excel.Range)ws.Cells[r, c]).Value2 = v; } catch { } }

        private Dictionary<string, double> SM() { var map = new Dictionary<string, double>(); foreach (DataGridViewRow r in dgvDosyalar.Rows) { string yol = r.Cells["colDosyaYolu"].Value?.ToString() ?? ""; double s = N(r.Cells["colSiparisAdeti"].Value); if (s <= 0) s = 1; if (!string.IsNullOrEmpty(yol) && !map.ContainsKey(yol)) map[yol] = s; } return map; }
        private Dictionary<string, MalzemeOzet> Ozet(Dictionary<string, double> sm) { var ozet = new Dictionary<string, MalzemeOzet>(StringComparer.OrdinalIgnoreCase); foreach (var v in _tumVeriler) { double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1; double g = sip * v.MontajAdeti * v.BirimAdet; string pro = PG(v.ParcaProfil), kal = KD(v.Kalite); if (string.IsNullOrWhiteSpace(pro)) continue; string key = pro + "|" + kal; if (!ozet.ContainsKey(key)) ozet[key] = new MalzemeOzet { Profil = pro, Kalite = kal }; ozet[key].ToplamAdet += g; ozet[key].ToplamUzunluk += g * v.Uzunluk; ozet[key].ToplamAgirlik += g * v.Agirlik; } return ozet; }
        private bool IsNum(string v) { double d; return double.TryParse(v?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) || double.TryParse(v?.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out d); }
        private double N(object o) { if (o == null) return 0; string s = o.ToString().Trim(); if (string.IsNullOrEmpty(s)) return 0; if (s.Contains(",") && s.Contains(".")) { int a = s.LastIndexOf('.'), b = s.LastIndexOf(','); s = a > b ? s.Replace(",", "") : s.Replace(".", "").Replace(",", "."); } else if (s.Contains(",")) s = s.Replace(",", "."); double r; return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out r) ? r : 0; }

        private void dgvSonuc_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}