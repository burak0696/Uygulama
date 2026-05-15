using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BaranYardimci
{
    public partial class FrmMalzemeDegistir : Form
    {
        const int COL_PROJENO = 0;
        const int COL_POZNO = 1;
        const int COL_POZACIK = 2;
        const int COL_ANAPOZNO = 3;
        const int COL_POZMIKTAR = 4;
        const int COL_POZAGIRLIK = 5;
        const int COL_BILTUR = 6;
        const int COL_BILNO = 7;
        const int COL_BILMIKTAR = 8;
        const int COL_ISLEMSIRA = 9;

        private readonly string _excelYolu;
        private DataTable _tablo;
        private List<HammaddeItem> _hammaddeler = new List<HammaddeItem>();
        private Dictionary<int, string> _degisiklikler = new Dictionary<int, string>();
        private int _secilenSatir = -1;
        private string _secilenHmNo = "";
        private string _secilenHmAdi = "";

        // UI — SplitContainer YOK, sağ panel sabit 380px Panel
        private Panel pnlUst, pnlAlt, pnlSag, pnlSecilenBar, pnlIcerik;
        private DataGridView dgv;
        private Label lblBaslik, lblDurum, lblSagBaslik, lblMevcutDeger, lblSecilenBaslik, lblSecilenKod;
        private TextBox txtArama;
        private ListView lvHammadde;
        private Button btnKaydet, btnIptal, btnLogGoster, btnUygula;

        private class HammaddeItem { public string No, Adi; }

        public FrmMalzemeDegistir(string excelYolu)
        {
            _excelYolu = excelYolu;
            InitializeComponent();
            InitForm();
            HammaddeleriYukle();
            ExcelOku();
        }

        // ══════════════════════════════════════════════════════════════════
        //  FORM YAPISI  —  SplitContainer YOK, SplitterDistance YOK
        // ══════════════════════════════════════════════════════════════════
        private void InitForm()
        {
            this.Text = "Malzeme Değiştir  —  " + Path.GetFileName(_excelYolu);
            this.Size = new Size(1400, 820);
            this.MinimumSize = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(28, 28, 34);
            this.Font = new Font("Segoe UI", 9.5f);

            // ── ÜST BAR ──────────────────────────────────────────────────
            pnlUst = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = Color.FromArgb(22, 22, 28), Padding = new Padding(12, 6, 12, 6) };
            lblBaslik = new Label { Dock = DockStyle.Left, Width = 700, Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = Color.FromArgb(100, 200, 255), Text = "📋  ERP Excel — Malzeme Değiştir", TextAlign = ContentAlignment.MiddleLeft };
            lblDurum = new Label { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9.5f, FontStyle.Italic), ForeColor = Color.FromArgb(160, 170, 180), Text = "Yükleniyor...", TextAlign = ContentAlignment.MiddleRight };
            pnlUst.Controls.Add(lblDurum);
            pnlUst.Controls.Add(lblBaslik);

            // ── ALT BAR ──────────────────────────────────────────────────
            pnlAlt = new Panel { Dock = DockStyle.Bottom, Height = 54, BackColor = Color.FromArgb(22, 22, 28), Padding = new Padding(12, 8, 12, 8) };
            btnKaydet = MkBtn("💾  Kaydet", Color.FromArgb(0, 150, 100), DockStyle.Right, 160);
            btnIptal = MkBtn("✖  İptal", Color.FromArgb(120, 50, 50), DockStyle.Right, 120);
            btnLogGoster = MkBtn("📄  Değişiklik Logunu Aç", Color.FromArgb(70, 70, 90), DockStyle.Left, 220);
            btnKaydet.Click += btnKaydet_Click;
            btnIptal.Click += (s, ev) => Close();
            btnLogGoster.Click += btnLogGoster_Click;
            pnlAlt.Controls.Add(btnKaydet);
            pnlAlt.Controls.Add(btnIptal);
            pnlAlt.Controls.Add(btnLogGoster);

            // ── İÇERİK ALANI (Top/Bottom/Left/Right hepsi dolu) ──────────
            pnlIcerik = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(28, 28, 34) };

            // ── SAĞ PANEL — sabit 400px, Dock=Right ──────────────────────
            pnlSag = new Panel
            {
                Dock = DockStyle.Right,
                Width = 400,
                BackColor = Color.FromArgb(32, 36, 48),
                Padding = new Padding(10, 10, 10, 6)
            };

            lblSagBaslik = new Label { Dock = DockStyle.Top, Height = 36, Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = Color.FromArgb(100, 180, 255), Text = "🔍  Hammadde Ara", TextAlign = ContentAlignment.MiddleLeft };
            lblMevcutDeger = new Label { Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 9.5f, FontStyle.Italic), ForeColor = Color.FromArgb(180, 180, 200), Text = "← Soldan bir 'Madde' satırına tıklayın", TextAlign = ContentAlignment.MiddleLeft };
            txtArama = new TextBox { Dock = DockStyle.Top, Height = 44, Font = new Font("Segoe UI", 14f), BackColor = Color.FromArgb(44, 48, 62), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            txtArama.TextChanged += txtArama_TextChanged;

            // seçilen bar — altta sabit
            pnlSecilenBar = new Panel { Dock = DockStyle.Bottom, Height = 92, BackColor = Color.FromArgb(22, 50, 28), Padding = new Padding(12, 8, 12, 8) };
            lblSecilenBaslik = new Label { Dock = DockStyle.Top, Height = 22, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(120, 220, 140), Text = "SEÇİLEN HAMMADDE:" };
            lblSecilenKod = new Label { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = Color.FromArgb(100, 240, 130), Text = "(seçilmedi)", TextAlign = ContentAlignment.MiddleLeft };
            pnlSecilenBar.Controls.Add(lblSecilenKod);
            pnlSecilenBar.Controls.Add(lblSecilenBaslik);

            btnUygula = new Button { Dock = DockStyle.Bottom, Height = 48, Text = "✔  Seçilen Hammaddeyi Uygula", Font = new Font("Segoe UI", 11f, FontStyle.Bold), BackColor = Color.FromArgb(39, 140, 76), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnUygula.FlatAppearance.BorderSize = 0;
            btnUygula.Click += btnUygula_Click;

            lvHammadde = new ListView { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f), FullRowSelect = true, GridLines = true, HideSelection = false, MultiSelect = false, View = View.Details, BackColor = Color.FromArgb(36, 40, 52), ForeColor = Color.FromArgb(210, 220, 240), BorderStyle = BorderStyle.None };
            lvHammadde.Columns.Add("Kod", 120);
            lvHammadde.Columns.Add("Hammadde Adı", 600);
            lvHammadde.SelectedIndexChanged += lvHammadde_SelectedIndexChanged;
            lvHammadde.DoubleClick += lvHammadde_DoubleClick;

            // Dock sırası: Fill en son eklenmeli
            pnlSag.Controls.Add(lvHammadde);
            pnlSag.Controls.Add(pnlSecilenBar);
            pnlSag.Controls.Add(btnUygula);
            pnlSag.Controls.Add(txtArama);
            pnlSag.Controls.Add(lblMevcutDeger);
            pnlSag.Controls.Add(lblSagBaslik);

            // ── DGV — kalan tüm alanı kaplar ─────────────────────────────
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.FromArgb(28, 28, 34),
                GridColor = Color.FromArgb(50, 55, 65),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                Font = new Font("Consolas", 9.5f),
                ColumnHeadersHeight = 32,
                RowTemplate = { Height = 26 },
                ScrollBars = ScrollBars.Both
            };
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(32, 32, 40);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(210, 220, 235);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 80, 130);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(38, 42, 55);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(180, 200, 230);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(36, 36, 46);
            dgv.CellClick += dgv_CellClick;
            dgv.CellFormatting += dgv_CellFormatting;

            // Sağ panel önce, sonra DGV (Fill)
            pnlIcerik.Controls.Add(dgv);
            pnlIcerik.Controls.Add(pnlSag);

            this.Controls.Add(pnlIcerik);
            this.Controls.Add(pnlAlt);
            this.Controls.Add(pnlUst);
        }

        private Button MkBtn(string text, Color bg, DockStyle dock, int w)
        {
            var b = new Button { Text = text, Dock = dock, Width = w, BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Cursor = Cursors.Hand };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        // ══════════════════════════════════════════════════════════════════
        //  VERİTABANI
        // ══════════════════════════════════════════════════════════════════
        private void HammaddeleriYukle()
        {
            try
            {
                using (var conn = new SqlConnection(DB.ConnStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT no, adi FROM hammadde ORDER BY adi", conn))
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                            _hammaddeler.Add(new HammaddeItem { No = dr["no"]?.ToString() ?? "", Adi = dr["adi"]?.ToString() ?? "" });
                }
            }
            catch (Exception ex) { MessageBox.Show("Hammadde yüklenemedi:\n" + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        // ══════════════════════════════════════════════════════════════════
        //  EXCEL OKU
        // ══════════════════════════════════════════════════════════════════
        private void ExcelOku()
        {
            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                app = new Excel.Application { Visible = false, DisplayAlerts = false };
                wb = app.Workbooks.Open(_excelYolu, false, true);
                var ws = (Excel.Worksheet)wb.Worksheets[1];
                int lastRow = ws.UsedRange.Rows.Count;

                _tablo = new DataTable();
                foreach (var s in new[] { "Proje No", "Poz No", "Poz Açıklaması", "Ana Poz No", "Poz Miktar", "Poz Ağırlık", "Bileşen Türü", "Bileşen No", "Bileşen Miktar", "İşlem Sırası" })
                    _tablo.Columns.Add(s, typeof(string));

                for (int r = 2; r <= lastRow; r++)
                {
                    var row = _tablo.NewRow();
                    for (int c = 1; c <= 10; c++) { object v = ((Excel.Range)ws.Cells[r, c]).Value2; row[c - 1] = v?.ToString() ?? ""; }
                    _tablo.Rows.Add(row);
                }

                int madde = _tablo.AsEnumerable().Count(r => r[COL_BILTUR].ToString().Trim().ToLower() == "madde");
                lblDurum.Text = $"{_tablo.Rows.Count} satır  |  {madde} madde  |  Değiştirmek için 'Madde' satırına tıklayın";
                lblDurum.ForeColor = Color.FromArgb(160, 170, 180);
            }
            catch (Exception ex) { MessageBox.Show("Excel okunamadı:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); DialogResult = DialogResult.Cancel; return; }
            finally
            {
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
            DgvOlustur();
        }

        // ══════════════════════════════════════════════════════════════════
        //  DGV
        // ══════════════════════════════════════════════════════════════════
        private void DgvOlustur()
        {
            dgv.Columns.Clear();
            string[] basliklar = { "Proje No", "Poz No", "Poz Açıklaması", "Ana Poz No", "Poz Miktar", "Poz Ağırlık", "Bileşen Türü", "Bileşen No ✏", "Bileşen Miktar", "İşlem Sırası" };
            int[] genislikler = { 90, 110, 160, 120, 80, 90, 100, 240, 100, 90 };
            for (int i = 0; i < 10; i++)
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "col" + i, HeaderText = basliklar[i], Width = genislikler[i], ReadOnly = true });

            foreach (DataRow dr in _tablo.Rows)
            {
                bool madde = dr[COL_BILTUR].ToString().Trim().ToLower() == "madde";
                int rowIdx = dgv.Rows.Add();
                var row = dgv.Rows[rowIdx];
                for (int c = 0; c < 10; c++) row.Cells[c].Value = dr[c].ToString();
                row.Tag = madde ? "madde" : "diger";
                if (madde) { row.DefaultCellStyle.BackColor = Color.FromArgb(30, 36, 52); row.DefaultCellStyle.ForeColor = Color.FromArgb(190, 215, 255); }
                else { row.DefaultCellStyle.BackColor = Color.FromArgb(26, 26, 33); row.DefaultCellStyle.ForeColor = Color.FromArgb(110, 120, 140); }
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgv.Rows[e.RowIndex];
            if (row.Tag?.ToString() != "madde") return;

            _secilenSatir = e.RowIndex;
            string mevcutNo = row.Cells[COL_BILNO].Value?.ToString() ?? "";
            string pozNo = row.Cells[COL_POZNO].Value?.ToString() ?? "?";
            string bilTur = row.Cells[COL_BILTUR].Value?.ToString() ?? "";
            lblMevcutDeger.Text = $"Seçili: {pozNo}  |  Tür: {bilTur}  |  Mevcut: {mevcutNo}";
            lblMevcutDeger.ForeColor = Color.FromArgb(255, 200, 100);
            _secilenHmNo = "";
            _secilenHmAdi = "";
            lblSecilenKod.Text = "(seçilmedi)";
            lblSecilenKod.ForeColor = Color.FromArgb(200, 100, 100);
            pnlSecilenBar.BackColor = Color.FromArgb(50, 28, 28);
            txtArama.Text = mevcutNo;
            txtArama.SelectAll();
            txtArama.Focus();
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (_degisiklikler.ContainsKey(e.RowIndex))
            { e.CellStyle.BackColor = Color.FromArgb(70, 60, 10); e.CellStyle.ForeColor = Color.FromArgb(255, 230, 80); }
            if (e.RowIndex == _secilenSatir && dgv.Rows[e.RowIndex].Tag?.ToString() == "madde")
            { e.CellStyle.BackColor = Color.FromArgb(0, 60, 120); e.CellStyle.ForeColor = Color.White; }
        }

        // ══════════════════════════════════════════════════════════════════
        //  ARAMA
        // ══════════════════════════════════════════════════════════════════
        private void txtArama_TextChanged(object sender, EventArgs e) => AramaYap(txtArama.Text);

        private void AramaYap(string q)
        {
            lvHammadde.Items.Clear();
            if (string.IsNullOrWhiteSpace(q)) return;
            string up = q.ToUpper().Replace(" ", "");
            foreach (var h in _hammaddeler)
            {
                if (h.No.ToUpper().Replace(" ", "").Contains(up) || h.Adi.ToUpper().Replace(" ", "").Contains(up))
                {
                    var lvi = new ListViewItem(h.No) { Tag = h };
                    lvi.SubItems.Add(h.Adi);
                    lvHammadde.Items.Add(lvi);
                }
            }
        }

        private void lvHammadde_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvHammadde.SelectedItems.Count == 0) return;
            var h = (HammaddeItem)lvHammadde.SelectedItems[0].Tag;
            _secilenHmNo = h.No;
            _secilenHmAdi = h.Adi;
            lblSecilenKod.Text = $"✔  {h.No}  —  {h.Adi}";
            lblSecilenKod.ForeColor = Color.FromArgb(100, 240, 130);
            pnlSecilenBar.BackColor = Color.FromArgb(22, 50, 28);
        }

        private void lvHammadde_DoubleClick(object sender, EventArgs e) => UygulaSecim();

        // ══════════════════════════════════════════════════════════════════
        //  UYGULA
        // ══════════════════════════════════════════════════════════════════
        private void btnUygula_Click(object sender, EventArgs e) => UygulaSecim();

        private void UygulaSecim()
        {
            if (_secilenSatir < 0) { MessageBox.Show("Önce soldan bir 'Madde' satırına tıklayın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrEmpty(_secilenHmNo)) { MessageBox.Show("Sağ listeden bir hammadde seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var row = dgv.Rows[_secilenSatir];
            string eskiNo = row.Cells[COL_BILNO].Value?.ToString() ?? "";
            if (!_degisiklikler.ContainsKey(_secilenSatir)) _degisiklikler[_secilenSatir] = eskiNo;
            row.Cells[COL_BILNO].Value = _secilenHmNo;
            _tablo.Rows[_secilenSatir][COL_BILNO] = _secilenHmNo;
            dgv.Refresh();
            lblDurum.Text = $"{_degisiklikler.Count} değişiklik yapıldı  |  Kaydetmek için 'Kaydet' butonuna basın";
            lblDurum.ForeColor = Color.FromArgb(255, 200, 80);

            for (int i = _secilenSatir + 1; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Tag?.ToString() == "madde")
                {
                    dgv.ClearSelection();
                    dgv.Rows[i].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = i;
                    dgv_CellClick(null, new DataGridViewCellEventArgs(COL_BILNO, i));
                    break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  KAYDET
        // ══════════════════════════════════════════════════════════════════
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (_degisiklikler.Count == 0) { MessageBox.Show("Herhangi bir değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (MessageBox.Show($"{_degisiklikler.Count} değişiklik kaydedilsin mi?\n\nBu işlem geri alınamaz.", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                app = new Excel.Application { Visible = false, DisplayAlerts = false };
                wb = app.Workbooks.Open(_excelYolu, false, false);
                var ws = (Excel.Worksheet)wb.Worksheets[1];
                var logSatirlar = new List<string>();
                foreach (var kv in _degisiklikler)
                {
                    int rowIdx = kv.Key;
                    string yeniNo = _tablo.Rows[rowIdx][COL_BILNO].ToString();
                    string eskiNo = kv.Value;
                    ((Excel.Range)ws.Cells[rowIdx + 2, COL_BILNO + 1]).Value2 = yeniNo;
                    logSatirlar.Add($"PozNo={_tablo.Rows[rowIdx][COL_POZNO]}  |  BileşenNo: [{eskiNo}] → [{yeniNo}]");
                }
                wb.Save();
                foreach (var satir in logSatirlar) LogHelper.Yaz(_excelYolu, "MALZEME_DEGISTIR", satir);
                MessageBox.Show($"{_degisiklikler.Count} değişiklik kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _degisiklikler.Clear();
                lblDurum.Text = "Kaydedildi.";
                lblDurum.ForeColor = Color.FromArgb(100, 220, 130);
                dgv.Refresh();
            }
            catch (Exception ex)
            {
                bool erisim = ex.HResult == unchecked((int)0x80070020) || ex.HResult == unchecked((int)0x80070005);
                MessageBox.Show("Kayıt hatası:" + (erisim ? "\nDosya açık olabilir." : "") + "\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  LOG
        // ══════════════════════════════════════════════════════════════════
        private void btnLogGoster_Click(object sender, EventArgs e)
        {
            string logYol = LogHelper.LogYoluGetir(_excelYolu);
            if (logYol == null) { MessageBox.Show("Bu dosya için henüz log kaydı yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            try
            {
                string icerik = File.ReadAllText(logYol, Encoding.UTF8);
                var frm = new Form { Text = "Değişiklik Geçmişi", Size = new Size(1000, 600), StartPosition = FormStartPosition.CenterParent, BackColor = Color.FromArgb(24, 24, 30) };
                var rtb = new RichTextBox { Dock = DockStyle.Fill, Text = icerik, ReadOnly = true, BackColor = Color.FromArgb(24, 24, 30), ForeColor = Color.FromArgb(180, 220, 180), Font = new Font("Consolas", 9.5f), BorderStyle = BorderStyle.None, ScrollBars = RichTextBoxScrollBars.Both };
                frm.Controls.Add(rtb);
                frm.Show();
            }
            catch (Exception ex) { MessageBox.Show("Log okunamadı: " + ex.Message); }
        }
    }
}