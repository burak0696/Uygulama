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
    public class FrmMalzemeDegistir : Form
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
        private Dictionary<(int, int), string> _degisiklikler = new Dictionary<(int, int), string>();
        private string _duzenlemeOncesiDeger = "";

        private DataGridView dgv;
        private Button btnKaydet;
        private Button btnIptal;
        private Button btnLogGoster;
        private Label lblBaslik;
        private Label lblDurum;
        private Panel pnlUst;
        private Panel pnlAlt;

        private class HammaddeItem
        {
            public string No, Adi;
            public override string ToString() => $"{No}  —  {Adi}";
        }

        public FrmMalzemeDegistir(string excelYolu)
        {
            _excelYolu = excelYolu;
            InitForm();
            HammaddeleriYukle();
            ExcelOku();
        }

        private void InitForm()
        {
            this.Text = "Malzeme Değiştir  —  " + Path.GetFileName(_excelYolu);
            this.Size = new Size(1400, 800);
            this.MinimumSize = new Size(900, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(28, 28, 34);
            this.Font = new Font("Segoe UI", 9.5f);

            pnlUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.FromArgb(22, 22, 28),
                Padding = new Padding(12, 8, 12, 8)
            };

            lblBaslik = new Label
            {
                Dock = DockStyle.Left,
                Width = 600,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 255),
                Text = "📋  ERP Excel — Malzeme Görüntüle & Değiştir",
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblDurum = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(160, 170, 180),
                Text = "Yükleniyor...",
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlUst.Controls.Add(lblDurum);
            pnlUst.Controls.Add(lblBaslik);

            pnlAlt = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.FromArgb(22, 22, 28),
                Padding = new Padding(12, 8, 12, 8)
            };

            btnKaydet = MkBtn("💾  Kaydet", Color.FromArgb(0, 150, 100), DockStyle.Right, 160);
            btnIptal = MkBtn("✖  İptal", Color.FromArgb(120, 50, 50), DockStyle.Right, 120);
            btnLogGoster = MkBtn("📄  Değişiklik Logunu Aç", Color.FromArgb(70, 70, 90), DockStyle.Left, 220);

            btnKaydet.Click += btnKaydet_Click;
            btnIptal.Click += (s, e) => Close();
            btnLogGoster.Click += btnLogGoster_Click;

            pnlAlt.Controls.Add(btnKaydet);
            pnlAlt.Controls.Add(btnIptal);
            pnlAlt.Controls.Add(btnLogGoster);

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

            dgv.CellBeginEdit += dgv_CellBeginEdit;
            dgv.CellEndEdit += dgv_CellEndEdit;
            dgv.CellFormatting += dgv_CellFormatting;

            this.Controls.Add(dgv);
            this.Controls.Add(pnlAlt);
            this.Controls.Add(pnlUst);
        }

        private Button MkBtn(string text, Color bg, DockStyle dock, int w)
        {
            var b = new Button
            {
                Text = text,
                Dock = dock,
                Width = w,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        // ── VERİTABANI ────────────────────────────────────────────────────

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
                            _hammaddeler.Add(new HammaddeItem
                            {
                                No = dr["no"]?.ToString() ?? "",
                                Adi = dr["adi"]?.ToString() ?? ""
                            });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hammadde listesi yüklenemedi:\n" + ex.Message,
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ── EXCEL OKU ─────────────────────────────────────────────────────

        private void ExcelOku()
        {
            Excel.Application app = null;
            Excel.Workbook wb = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                app = new Excel.Application { Visible = false, DisplayAlerts = false };
                wb = app.Workbooks.Open(_excelYolu, false, true);
                var ws = (Excel.Worksheet)wb.Worksheets[1];
                int lastRow = ws.UsedRange.Rows.Count;

                _tablo = new DataTable();
                string[] sutunlar = {
                    "Proje No", "Poz No", "Poz Açıklaması", "Ana Poz No",
                    "Poz Miktar", "Poz Ağırlık", "Bileşen Türü",
                    "Bileşen No", "Bileşen Miktar", "İşlem Sırası"
                };
                foreach (var s in sutunlar) _tablo.Columns.Add(s, typeof(string));

                for (int r = 2; r <= lastRow; r++)
                {
                    var row = _tablo.NewRow();
                    for (int c = 1; c <= 10; c++)
                    {
                        object v = ((Excel.Range)ws.Cells[r, c]).Value2;
                        row[c - 1] = v?.ToString() ?? "";
                    }
                    _tablo.Rows.Add(row);
                }

                int madde = _tablo.AsEnumerable()
                    .Count(r => r[COL_BILTUR].ToString().Trim().ToLower() == "madde");
                lblDurum.Text = $"{_tablo.Rows.Count} satır yüklendi  |  {madde} madde satırı";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel okunamadı:\n" + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                return;
            }
            finally
            {
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }

            DgvOlustur();
        }

        // ── DGV OLUŞTUR ───────────────────────────────────────────────────

        private void DgvOlustur()
        {
            dgv.Columns.Clear();
            dgv.DataSource = null;

            string[] basliklar = {
                "Proje No", "Poz No", "Poz Açıklaması", "Ana Poz No",
                "Poz Miktar", "Poz Ağırlık", "Bileşen Türü",
                "Bileşen No ✏", "Bileşen Miktar", "İşlem Sırası"
            };
            int[] genislikler = { 90, 110, 160, 120, 80, 90, 100, 300, 100, 90 };

            for (int i = 0; i < 10; i++)
            {
                if (i == COL_BILNO)
                {
                    var col = new DataGridViewComboBoxColumn
                    {
                        Name = "col" + i,
                        HeaderText = basliklar[i],
                        Width = genislikler[i],
                        FlatStyle = FlatStyle.Flat,
                        DisplayStyleForCurrentCellOnly = true
                    };
                    col.Items.Add("");
                    foreach (var h in _hammaddeler)
                        col.Items.Add(h.No + "  —  " + h.Adi);
                    dgv.Columns.Add(col);
                }
                else
                {
                    dgv.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "col" + i,
                        HeaderText = basliklar[i],
                        Width = genislikler[i],
                        ReadOnly = true
                    });
                }
            }

            foreach (DataRow dr in _tablo.Rows)
            {
                bool madde = dr[COL_BILTUR].ToString().Trim().ToLower() == "madde";
                int rowIdx = dgv.Rows.Add();
                var row = dgv.Rows[rowIdx];

                for (int c = 0; c < 10; c++)
                {
                    string deger = dr[c].ToString();

                    if (c == COL_BILNO && madde)
                    {
                        var cbCol = dgv.Columns[c] as DataGridViewComboBoxColumn;
                        string eslesme = cbCol?.Items.Cast<string>()
                            .FirstOrDefault(x => x.StartsWith(deger + "  —")) ?? deger;
                        row.Cells[c].Value = eslesme;
                        row.Cells[c].ReadOnly = false;
                    }
                    else
                    {
                        row.Cells[c].Value = deger;
                        row.Cells[c].ReadOnly = true;
                    }
                }

                if (!madde)
                {
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 130, 150);
                    row.DefaultCellStyle.BackColor = Color.FromArgb(26, 26, 33);
                }
            }
        }

        // ── DGV OLAYLAR ───────────────────────────────────────────────────

        private void dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _duzenlemeOncesiDeger = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string yeni = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
            if (yeni != _duzenlemeOncesiDeger)
                _degisiklikler[(e.RowIndex, e.ColumnIndex)] = _duzenlemeOncesiDeger;
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (_degisiklikler.ContainsKey((e.RowIndex, e.ColumnIndex)))
            {
                e.CellStyle.BackColor = Color.FromArgb(80, 70, 20);
                e.CellStyle.ForeColor = Color.FromArgb(255, 230, 100);
            }
        }

        // ── KAYDET ────────────────────────────────────────────────────────

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (_degisiklikler.Count == 0)
            {
                MessageBox.Show("Herhangi bir değişiklik yapılmadı.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(
                $"{_degisiklikler.Count} değişiklik kaydedilsin mi?\n\nBu işlem geri alınamaz.",
                "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Excel.Application app = null;
            Excel.Workbook wb = null;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                app = new Excel.Application { Visible = false, DisplayAlerts = false };
                wb = app.Workbooks.Open(_excelYolu, false, false);
                var ws = (Excel.Worksheet)wb.Worksheets[1];

                var logSatirlar = new List<string>();

                foreach (var kv in _degisiklikler)
                {
                    int satir = kv.Key.Item1;
                    int sutun = kv.Key.Item2;
                    int excelSatir = satir + 2;
                    int excelSutun = sutun + 1;

                    string yeni = dgv.Rows[satir].Cells[sutun].Value?.ToString() ?? "";
                    if (yeni.Contains("  —  "))
                        yeni = yeni.Split(new[] { "  —  " }, StringSplitOptions.None)[0].Trim();

                    string eski = kv.Value;
                    if (eski.Contains("  —  "))
                        eski = eski.Split(new[] { "  —  " }, StringSplitOptions.None)[0].Trim();

                    ((Excel.Range)ws.Cells[excelSatir, excelSutun]).Value2 = yeni;

                    string pozNo = dgv.Rows[satir].Cells[COL_POZNO].Value?.ToString() ?? "?";
                    logSatirlar.Add(
                        $"PozNo={pozNo}  |  Sütun={dgv.Columns[sutun].HeaderText}  |  " +
                        $"Eski=[{eski}]  →  Yeni=[{yeni}]");
                }

                wb.Save();

                foreach (var satir in logSatirlar)
                    LogHelper.Yaz(_excelYolu, "MALZEME_DEGISTIR", satir);

                MessageBox.Show($"{_degisiklikler.Count} değişiklik kaydedildi ve loglandı.",
                    "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _degisiklikler.Clear();
                dgv.Refresh();
            }
            catch (Exception ex)
            {
                bool erisim = ex.HResult == unchecked((int)0x80070020)
                           || ex.HResult == unchecked((int)0x80070005);
                MessageBox.Show(
                    "Kayıt hatası:" +
                    (erisim ? "\nDosya başka bir program tarafından açık olabilir." : "") +
                    "\n\n" + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }

        // ── LOG GÖSTER ────────────────────────────────────────────────────

        private void btnLogGoster_Click(object sender, EventArgs e)
        {
            string logYol = LogHelper.LogYoluGetir(_excelYolu);
            if (logYol == null)
            {
                MessageBox.Show("Bu dosya için henüz log kaydı yok.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string icerik = File.ReadAllText(logYol, Encoding.UTF8);

                var frm = new Form
                {
                    Text = "Değişiklik Geçmişi  —  " + Path.GetFileName(_excelYolu),
                    Size = new Size(1000, 600),
                    StartPosition = FormStartPosition.CenterParent,
                    BackColor = Color.FromArgb(24, 24, 30)
                };

                var rtb = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    Text = icerik,
                    ReadOnly = true,
                    BackColor = Color.FromArgb(24, 24, 30),
                    ForeColor = Color.FromArgb(180, 220, 180),
                    Font = new Font("Consolas", 9.5f),
                    BorderStyle = BorderStyle.None,
                    ScrollBars = RichTextBoxScrollBars.Both
                };

                frm.Controls.Add(rtb);
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Log okunamadı: " + ex.Message);
            }
        }
    }
}