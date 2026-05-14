using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BaranYardimci
{
    public partial class FrmMalzemeDegistir : Form
    {
        class MalzemeSatiri
        {
            public int ExcelSatirNo;
            public string PozNo;
            public string PozAciklamasi;
            public string AnaPozNo;
            public string BilesenTuru;
            public string MevcutBilesenNo;
            public string MevcutAciklama;
            public string YeniBilesenNo;
            public string YeniAciklama;
            public bool DegistiMi => !string.Equals((MevcutBilesenNo ?? "").Trim(), (YeniBilesenNo ?? "").Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private readonly string _excelYolu;
        private readonly Func<bool> _agaBaglan;
        private readonly Action _agaBaglantiyiKes;
        private readonly List<MalzemeSatiri> _tumSatirlar = new List<MalzemeSatiri>();
        private readonly List<HammaddeItem> _hammaddeler = new List<HammaddeItem>();
        private readonly Dictionary<string, string> _hammaddeAdlari = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string YeniExcelYolu { get; private set; }

        public FrmMalzemeDegistir(string excelYolu, Func<bool> agaBaglan, Action agaBaglantiyiKes)
        {
            InitializeComponent();
            _excelYolu = excelYolu ?? "";
            _agaBaglan = agaBaglan;
            _agaBaglantiyiKes = agaBaglantiyiKes;
        }

        private void FrmMalzemeDegistir_Load(object sender, EventArgs e)
        {
            txtExcelYolu.Text = _excelYolu;
            HammaddeleriYukle();
            ExceliYukle();
            GridiDoldur();
        }

        private void HammaddeleriYukle()
        {
            _hammaddeler.Clear();
            _hammaddeAdlari.Clear();
            try
            {
                using (var conn = new SqlConnection(DB.ConnStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT no, adi FROM hammadde ORDER BY no", conn))
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                        {
                            var hm = new HammaddeItem
                            {
                                No = dr["no"].ToString(),
                                Adi = dr["adi"].ToString()
                            };
                            _hammaddeler.Add(hm);
                            if (!_hammaddeAdlari.ContainsKey(hm.No ?? ""))
                                _hammaddeAdlari[hm.No ?? ""] = hm.Adi ?? "";
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hammadde listesi alınamadı:\n" + ex.Message,
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExceliYukle()
        {
            _tumSatirlar.Clear();
            if (string.IsNullOrWhiteSpace(_excelYolu)) return;

            Cursor.Current = Cursors.WaitCursor;
            Excel.Application app = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;
            Excel.Range usedRange = null;

            try
            {
                AgErisiminiDene();
                if (!DosyaErisebilir(_excelYolu) && !AgErisiminiDene())
                {
                    MessageBox.Show("Ağ bağlantısı kurulamadı.", "Ağ Hatası",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!DosyaErisebilir(_excelYolu))
                {
                    MessageBox.Show("Excel dosyasına erişilemiyor:\n" + _excelYolu,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                wb = app.Workbooks.Open(_excelYolu, false, true,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                ws = (Excel.Worksheet)wb.Worksheets[1];
                usedRange = ws.UsedRange;
                int sonSatir = usedRange.Rows.Count;

                for (int r = 2; r <= sonSatir; r++)
                {
                    string bilesenTuru = HucreText(ws, r, 7);
                    if (!string.Equals(bilesenTuru.Trim(), "MADDE", StringComparison.OrdinalIgnoreCase))
                        continue;

                    string mevcutNo = HucreText(ws, r, 8);
                    string mevcutAd = HammaddeAdiBul(mevcutNo);
                    _tumSatirlar.Add(new MalzemeSatiri
                    {
                        ExcelSatirNo = r,
                        PozNo = HucreText(ws, r, 2),
                        PozAciklamasi = HucreText(ws, r, 3),
                        AnaPozNo = HucreText(ws, r, 4),
                        BilesenTuru = bilesenTuru,
                        MevcutBilesenNo = mevcutNo,
                        MevcutAciklama = mevcutAd,
                        YeniBilesenNo = mevcutNo,
                        YeniAciklama = mevcutAd
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel okunamadı:\n" + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try { if (wb != null) wb.Close(false); } catch { }
                try { if (app != null) app.Quit(); } catch { }
                try { if (usedRange != null) Marshal.ReleaseComObject(usedRange); } catch { }
                try { if (ws != null) Marshal.ReleaseComObject(ws); } catch { }
                try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { }
                try { if (app != null) Marshal.ReleaseComObject(app); } catch { }
                try { if (_agaBaglantiyiKes != null) _agaBaglantiyiKes(); } catch { }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }

        private string HucreText(Excel.Worksheet ws, int row, int col)
        {
            Excel.Range cell = null;
            try
            {
                cell = (Excel.Range)ws.Cells[row, col];
                object value = cell.Value2;
                return value == null ? "" : value.ToString().Trim();
            }
            catch
            {
                return "";
            }
            finally
            {
                try { if (cell != null) Marshal.ReleaseComObject(cell); } catch { }
            }
        }

        private bool DosyaErisebilir(string yol)
        {
            if (string.IsNullOrWhiteSpace(yol) || !File.Exists(yol)) return false;
            try
            {
                using (var fs = File.Open(yol, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    return true;
            }
            catch { return false; }
        }

        private bool AgErisiminiDene()
        {
            try { return _agaBaglan != null && _agaBaglan(); }
            catch { return false; }
        }

        private string HammaddeAdiBul(string no)
        {
            string anahtar = (no ?? "").Trim();
            if (string.IsNullOrEmpty(anahtar)) return "";
            return _hammaddeAdlari.ContainsKey(anahtar) ? _hammaddeAdlari[anahtar] : "";
        }

        private void txtFiltre_TextChanged(object sender, EventArgs e)
            => GridiDoldur();

        private void GridiDoldur()
        {
            string filtre = (txtFiltre.Text ?? "").Trim();
            dgvMalzemeler.Rows.Clear();

            IEnumerable<MalzemeSatiri> liste = _tumSatirlar;
            if (!string.IsNullOrWhiteSpace(filtre))
                liste = liste.Where(x =>
                    (!string.IsNullOrEmpty(x.PozNo) && x.PozNo.IndexOf(filtre, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(x.MevcutBilesenNo) && x.MevcutBilesenNo.IndexOf(filtre, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(x.YeniBilesenNo) && x.YeniBilesenNo.IndexOf(filtre, StringComparison.OrdinalIgnoreCase) >= 0));

            foreach (var item in liste)
            {
                int idx = dgvMalzemeler.Rows.Add(
                    item.ExcelSatirNo,
                    item.PozNo,
                    item.PozAciklamasi,
                    item.AnaPozNo,
                    item.BilesenTuru,
                    item.MevcutBilesenNo,
                    item.MevcutAciklama,
                    item.YeniBilesenNo,
                    item.YeniAciklama,
                    item.DegistiMi ? "Değiştirildi" : "Aynı",
                    "🔍");
                var row = dgvMalzemeler.Rows[idx];
                row.Tag = item;
                SatirStiliniUygula(row, item);
            }

            OzetiGuncelle();
        }

        private void SatirStiliniUygula(DataGridViewRow row, MalzemeSatiri item)
        {
            Color bg = item.DegistiMi ? Color.FromArgb(255, 242, 204) : Color.White;
            Color fg = item.DegistiMi ? Color.FromArgb(120, 85, 0) : Color.Black;
            row.DefaultCellStyle.BackColor = bg;
            row.DefaultCellStyle.ForeColor = fg;
            row.Cells[9].Value = item.DegistiMi ? "Değiştirildi" : "Aynı";
        }

        private void OzetiGuncelle()
        {
            int degisen = _tumSatirlar.Count(x => x.DegistiMi);
            lblOzet.Text = degisen + " satır değiştirildi";
        }

        private void dgvMalzemeler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            SatiraMalzemeSec(e.RowIndex);
        }

        private void dgvMalzemeler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvMalzemeler.Columns[e.ColumnIndex].Name != "colAra") return;
            SatiraMalzemeSec(e.RowIndex);
        }

        private void SatiraMalzemeSec(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvMalzemeler.Rows.Count) return;
            var row = dgvMalzemeler.Rows[rowIndex];
            var item = row.Tag as MalzemeSatiri;
            if (item == null) return;

            using (var frm = new FrmHammaddeAra(item.YeniBilesenNo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) != DialogResult.OK || frm.Secilen == null) return;

                item.YeniBilesenNo = frm.Secilen.No ?? "";
                item.YeniAciklama = frm.Secilen.Adi ?? "";
                row.Cells[7].Value = item.YeniBilesenNo;
                row.Cells[8].Value = item.YeniAciklama;
                SatirStiliniUygula(row, item);
                OzetiGuncelle();
            }
        }

        private void btnExceliAc_Click(object sender, EventArgs e)
        {
            if (!DosyaErisebilir(_excelYolu)) AgErisiminiDene();
            if (!DosyaErisebilir(_excelYolu))
            {
                MessageBox.Show("Excel dosyasına erişilemiyor:\n" + _excelYolu,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try { System.Diagnostics.Process.Start(_excelYolu); }
            catch (Exception ex) { MessageBox.Show("Excel açılamadı:\n" + ex.Message); }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            var degisenler = _tumSatirlar.Where(x => x.DegistiMi).ToList();
            if (degisenler.Count == 0)
            {
                MessageBox.Show("Kaydedilecek bir malzeme değişikliği yok.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Excel.Application app = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                AgErisiminiDene();
                if (!DosyaErisebilir(_excelYolu) && !AgErisiminiDene())
                {
                    MessageBox.Show("Ağ bağlantısı kurulamadı.", "Ağ Hatası",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!DosyaErisebilir(_excelYolu))
                {
                    MessageBox.Show("Excel dosyasına erişilemiyor:\n" + _excelYolu,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string yeniYol = RevizyonluYolUret(_excelYolu);
                if (string.IsNullOrEmpty(yeniYol)) return;

                if (File.Exists(yeniYol))
                {
                    var cevap = MessageBox.Show("Yeni revizyon dosyası zaten var:\n" + yeniYol + "\n\nÜzerine yazılsın mı?",
                        "Dosya Var", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (cevap != DialogResult.Yes) return;
                    File.Delete(yeniYol);
                }

                File.Copy(_excelYolu, yeniYol, true);

                app = new Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                wb = app.Workbooks.Open(yeniYol, false, false,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                ws = (Excel.Worksheet)wb.Worksheets[1];

                foreach (var item in degisenler)
                    Yaz(ws, item.ExcelSatirNo, 8, item.YeniBilesenNo);

                wb.Save();
                try { wb.Close(true); } catch { }
                try { app.Quit(); } catch { }
                YeniExcelYolu = yeniYol;
                MessageBox.Show($"✅ Revizyon kaydedildi:\n{yeniYol}\n\n{degisenler.Count} malzeme değiştirildi.",
                    "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                bool erisim = ex.HResult == unchecked((int)0x80070020)
                    || ex.HResult == unchecked((int)0x80070005);
                MessageBox.Show("Excel yazma hatası:" + (erisim ? "\nDosya açık olabilir." : "") + "\n\n" + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { if (wb != null) wb.Close(false); } catch { }
                try { if (app != null) app.Quit(); } catch { }
            }
            finally
            {
                try { if (ws != null) Marshal.ReleaseComObject(ws); } catch { }
                try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { }
                try { if (app != null) Marshal.ReleaseComObject(app); } catch { }
                try { if (_agaBaglantiyiKes != null) _agaBaglantiyiKes(); } catch { }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }

        private string RevizyonluYolUret(string mevcutYol)
        {
            string klasor = Path.GetDirectoryName(mevcutYol);
            if (string.IsNullOrWhiteSpace(klasor)) klasor = Environment.CurrentDirectory;
            string dosyaAdi = Path.GetFileName(mevcutYol);
            var m = Regex.Match(dosyaAdi ?? "", @"^(.*)_ERPAKTARIM(\d+)\.xlsx$", RegexOptions.IgnoreCase);
            int revNo = 2;
            string bazAd = Path.GetFileNameWithoutExtension(mevcutYol);
            if (m.Success)
            {
                bazAd = m.Groups[1].Value;
                int mevcutRev;
                if (int.TryParse(m.Groups[2].Value, out mevcutRev)) revNo = mevcutRev + 1;
            }
            return Path.Combine(klasor, bazAd + "_ERPAKTARIM" + revNo + ".xlsx");
        }

        private void Yaz(Excel.Worksheet ws, int r, int c, object v)
        {
            Excel.Range cell = null;
            try
            {
                cell = (Excel.Range)ws.Cells[r, c];
                if (v != null) cell.Value2 = v;
            }
            catch { }
            finally
            {
                try { if (cell != null) Marshal.ReleaseComObject(cell); } catch { }
            }
        }
    }
}
