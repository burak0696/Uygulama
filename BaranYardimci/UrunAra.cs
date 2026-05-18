using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class UrunAra : Form
    {
        private bool _yukleniyor = false;   // ComboBox event'lerini blokla

        public UrunAra()
        {
            InitializeComponent();
            AttachEvents();
            LoadInitialDesigns();
            DurumYaz("Hazır.  Filtre seçin veya hızlı arama yapın.", Color.FromArgb(180, 200, 180));
        }

        // ════════════════════════════════════════════════════════════
        //  EVENTS
        // ════════════════════════════════════════════════════════════
        private void AttachEvents()
        {
            cmbDesign.SelectedIndexChanged += (s, e) =>
            {
                if (_yukleniyor) return;
                ClearLowerCombos(1);
                FillComboBox(cmbSector, "Sector", "Design=@d", P("@d", cmbDesign.Text));
            };
            cmbSector.SelectedIndexChanged += (s, e) =>
            {
                if (_yukleniyor) return;
                ClearLowerCombos(2);
                FillComboBox(cmbProductType, "ProductType", "Design=@d AND Sector=@s",
                    P("@d", cmbDesign.Text), P("@s", cmbSector.Text));
            };
            cmbProductType.SelectedIndexChanged += (s, e) =>
            {
                if (_yukleniyor) return;
                ClearLowerCombos(3);
                FillComboBox(cmbStructureType, "StructureType",
                    "Design=@d AND Sector=@s AND ProductType=@p",
                    P("@d", cmbDesign.Text), P("@s", cmbSector.Text), P("@p", cmbProductType.Text));
            };
            cmbStructureType.SelectedIndexChanged += (s, e) =>
            {
                if (_yukleniyor) return;
                ClearLowerCombos(4);
                FillComboBox(cmbVoltageAyak, "VoltageAyak",
                    "Design=@d AND Sector=@s AND ProductType=@p AND StructureType=@st",
                    P("@d", cmbDesign.Text), P("@s", cmbSector.Text),
                    P("@p", cmbProductType.Text), P("@st", cmbStructureType.Text));
            };
            cmbVoltageAyak.SelectedIndexChanged += (s, e) =>
            {
                if (_yukleniyor) return;
                ClearLowerCombos(5);
                FillComboBox(cmbFoundation, "Foundation",
                    "Design=@d AND Sector=@s AND ProductType=@p AND StructureType=@st AND VoltageAyak=@v",
                    P("@d", cmbDesign.Text), P("@s", cmbSector.Text),
                    P("@p", cmbProductType.Text), P("@st", cmbStructureType.Text),
                    P("@v", cmbVoltageAyak.Text));
            };
            cmbFoundation.SelectedIndexChanged += (s, e) =>
            {
                if (_yukleniyor) return;
                ClearLowerCombos(6);
                FillComboBox(cmbTower, "Tower",
                    "Design=@d AND Sector=@s AND ProductType=@p AND StructureType=@st AND VoltageAyak=@v AND Foundation=@f",
                    P("@d", cmbDesign.Text), P("@s", cmbSector.Text),
                    P("@p", cmbProductType.Text), P("@st", cmbStructureType.Text),
                    P("@v", cmbVoltageAyak.Text), P("@f", cmbFoundation.Text));
            };

            btnYeniUrun.Click += BtnYeniUrun_Click;
            btnUrunGetir.Click += BtnUrunGetir_Click;
            btnTemizle.Click += (s, e) => { ClearLowerCombos(0); dataGridView1.DataSource = null; lblKayitSayisi.Text = "📊 0 kayıt"; DurumYaz("Filtreler temizlendi.", Color.FromArgb(180, 200, 180)); };
            btnTasarimSearch.Click += BtnTasarimSearch_Click;
            btnKapat.Click += (s, e) => this.Close();

            txtTasarimSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BtnTasarimSearch_Click(s, EventArgs.Empty);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };
        }

        private static SqlParameter P(string name, object val) => new SqlParameter(name, val ?? DBNull.Value);

        // ════════════════════════════════════════════════════════════
        //  COMBOBOX YÜKLEME
        // ════════════════════════════════════════════════════════════
        private void LoadInitialDesigns() => FillComboBox(cmbDesign, "Design", null);

        private void FillComboBox(ComboBox combo, string column, string whereClause, params SqlParameter[] parameters)
        {
            _yukleniyor = true;
            try
            {
                combo.Items.Clear();
                string sql = $"SELECT DISTINCT [{column}] FROM dbo.Urun";
                if (!string.IsNullOrWhiteSpace(whereClause)) sql += " WHERE " + whereClause;
                sql += $" ORDER BY [{column}]";

                var dt = DB.GetTable(sql, parameters);
                foreach (DataRow r in dt.Rows)
                {
                    var v = r[0]?.ToString();
                    if (!string.IsNullOrWhiteSpace(v)) combo.Items.Add(v);
                }
            }
            catch (Exception ex)
            {
                DurumYaz("❌ Combo yükleme hatası: " + ex.Message, Color.FromArgb(255, 140, 140));
            }
            finally { _yukleniyor = false; }
        }

        private void ClearLowerCombos(int level)
        {
            _yukleniyor = true;
            try
            {
                if (level <= 0) { cmbDesign.Items.Clear(); cmbDesign.Text = ""; LoadInitialDesigns(); }
                if (level <= 1) { cmbSector.Items.Clear(); cmbSector.Text = ""; }
                if (level <= 2) { cmbProductType.Items.Clear(); cmbProductType.Text = ""; }
                if (level <= 3) { cmbStructureType.Items.Clear(); cmbStructureType.Text = ""; }
                if (level <= 4) { cmbVoltageAyak.Items.Clear(); cmbVoltageAyak.Text = ""; }
                if (level <= 5) { cmbFoundation.Items.Clear(); cmbFoundation.Text = ""; }
                if (level <= 6) { cmbTower.Items.Clear(); cmbTower.Text = ""; }
            }
            finally { _yukleniyor = false; }
        }

        // ════════════════════════════════════════════════════════════
        //  ÜRÜN GETİR  (kategori filtreli)
        // ════════════════════════════════════════════════════════════
        private void BtnUrunGetir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var whereParts = new List<string>();
                var pars = new List<SqlParameter>();

                void Ekle(ComboBox c, string col, string parName)
                {
                    if (!string.IsNullOrWhiteSpace(c.Text))
                    { whereParts.Add($"{col}={parName}"); pars.Add(P(parName, c.Text)); }
                }

                Ekle(cmbDesign, "Design", "@d");
                Ekle(cmbSector, "Sector", "@s");
                Ekle(cmbProductType, "ProductType", "@p");
                Ekle(cmbStructureType, "StructureType", "@st");
                Ekle(cmbVoltageAyak, "VoltageAyak", "@v");
                Ekle(cmbFoundation, "Foundation", "@f");
                Ekle(cmbTower, "Tower", "@t");

                string sql = @"SELECT Design, Sector, ProductType, StructureType, VoltageAyak, Foundation, Tower, Height, Rev, Tasarim, UrunKodu
                               FROM dbo.Urun";
                if (whereParts.Count > 0) sql += " WHERE " + string.Join(" AND ", whereParts);
                sql += " ORDER BY Design, Sector, ProductType, StructureType, VoltageAyak, Foundation, Tower";

                var dt = DB.GetTable(sql, pars.ToArray());
                BindGrid(dt);

                if (dt.Rows.Count > 0)
                {
                    int last = dataGridView1.Rows.Count - 1;
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[last].Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = last;
                    DurumYaz($"✅ {dt.Rows.Count} kayıt bulundu.", Color.FromArgb(140, 240, 160));
                }
                else
                {
                    DurumYaz("⚠ Filtreye uyan kayıt bulunamadı.", Color.FromArgb(255, 200, 100));
                }
            }
            catch (Exception ex)
            {
                DurumYaz("❌ Hata: " + ex.Message, Color.FromArgb(255, 140, 140));
                MessageBox.Show("Veri çekilemedi:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor.Current = Cursors.Default; }
        }

        // ════════════════════════════════════════════════════════════
        //  HIZLI ARAMA  (Tasarım / Ürün Kodu)
        // ════════════════════════════════════════════════════════════
        private void BtnTasarimSearch_Click(object sender, EventArgs e)
        {
            string raw = txtTasarimSearch.Text ?? "";
            string withHyphen = NormalizeRemoveSpaces(raw);
            if (string.IsNullOrWhiteSpace(withHyphen))
            {
                MessageBox.Show("Arama metni giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTasarimSearch.Focus();
                return;
            }
            string noHyphen = withHyphen.Replace("-", "");

            const string sql = @"
SELECT Design, Sector, ProductType, StructureType, VoltageAyak, Foundation, Tower, Height, Rev, Tasarim, UrunKodu
FROM dbo.Urun
WHERE
    REPLACE(REPLACE(ISNULL(Tasarim , ''), ' ', ''), '-', '') = @noHyphen
    OR REPLACE(ISNULL(Tasarim , ''), ' ', '')               = @withHyphen
    OR ISNULL(Tasarim , '')                                  = @withHyphen
    OR REPLACE(REPLACE(ISNULL(UrunKodu, ''), ' ', ''), '-', '') = @noHyphen
    OR ISNULL(UrunKodu, '')                                  = @withHyphen
ORDER BY Design, Sector, ProductType, StructureType, VoltageAyak, Foundation, Tower;";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var dt = DB.GetTable(sql,
                    new SqlParameter("@withHyphen", SqlDbType.NVarChar, 500) { Value = withHyphen },
                    new SqlParameter("@noHyphen", SqlDbType.NVarChar, 500) { Value = noHyphen });

                BindGrid(dt);
                if (dt.Rows.Count == 0)
                {
                    DurumYaz($"⚠ '{raw}' için eşleşme yok.", Color.FromArgb(255, 200, 100));
                    MessageBox.Show("Eşleşen kayıt bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DurumYaz($"✅ '{raw}' → {dt.Rows.Count} eşleşme.", Color.FromArgb(140, 240, 160));
                }
            }
            catch (SqlException ex)
            {
                DurumYaz("❌ SQL hatası: " + ex.Message, Color.FromArgb(255, 140, 140));
                MessageBox.Show("SQL hatası:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor.Current = Cursors.Default; }
        }

        private string NormalizeRemoveSpaces(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            return Regex.Replace(input.Trim(), @"\s+", "");
        }

        // ════════════════════════════════════════════════════════════
        //  GRID BINDING (ortak)
        // ════════════════════════════════════════════════════════════
        private void BindGrid(DataTable dt)
        {
            dataGridView1.DataSource = dt;

            var headers = new Dictionary<string, string>
            {
                ["Design"] = "Tasarım",
                ["Sector"] = "Sektör",
                ["ProductType"] = "Ürün Tipi",
                ["StructureType"] = "Yapı Tipi",
                ["VoltageAyak"] = "Voltaj / Ayak",
                ["Foundation"] = "Temel",
                ["Tower"] = "Kule",
                ["Height"] = "Yükseklik",
                ["Rev"] = "Rev",
                ["Tasarim"] = "Tasarım Kodu",
                ["UrunKodu"] = "Ürün Kodu"
            };
            foreach (var kv in headers)
                if (dataGridView1.Columns[kv.Key] != null)
                    dataGridView1.Columns[kv.Key].HeaderText = kv.Value;

            // UrunKodu sütununu vurgula
            if (dataGridView1.Columns["UrunKodu"] != null)
            {
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 220);
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.Font = new Font("Consolas", 9.5f, FontStyle.Bold);
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.ForeColor = Color.FromArgb(100, 60, 0);
            }
            if (dataGridView1.Columns["Tasarim"] != null)
                dataGridView1.Columns["Tasarim"].DefaultCellStyle.Font = new Font("Consolas", 9.5f);

            dataGridView1.AutoResizeColumns();
            lblKayitSayisi.Text = $"📊 {dt.Rows.Count} kayıt";
        }

        // ════════════════════════════════════════════════════════════
        //  YENİ ÜRÜN
        // ════════════════════════════════════════════════════════════
        private void BtnYeniUrun_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cmbDesign.Text) ||
                    string.IsNullOrWhiteSpace(cmbSector.Text) ||
                    string.IsNullOrWhiteSpace(cmbProductType.Text) ||
                    string.IsNullOrWhiteSpace(cmbStructureType.Text) ||
                    string.IsNullOrWhiteSpace(cmbVoltageAyak.Text) ||
                    string.IsNullOrWhiteSpace(cmbFoundation.Text) ||
                    string.IsNullOrWhiteSpace(cmbTower.Text))
                {
                    MessageBox.Show("Önce 7 kategoriyi de seçin\n(Tasarım → Sektör → ... → Kule).",
                        "Eksik Seçim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tek diyalogda 3 alan
                string height, rev, tasarim;
                if (!YeniUrunDialog(out height, out rev, out tasarim)) return;

                string urunKodu = GenerateUrunKodu(
                    cmbDesign.Text, cmbSector.Text, cmbProductType.Text, cmbStructureType.Text,
                    cmbVoltageAyak.Text, cmbFoundation.Text, cmbTower.Text, height, rev);

                if (ExistsUrunKodu(urunKodu))
                {
                    MessageBox.Show("Aynı ürün kodu zaten kayıtlı:\n\n" + urunKodu,
                        "Mükerrer Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                const string insertSql = @"
INSERT INTO dbo.Urun (Design, Sector, ProductType, StructureType, VoltageAyak, Foundation, Tower, Height, Rev, Tasarim, UrunKodu)
VALUES (@d,@s,@p,@st,@v,@f,@t,@h,@r,@tas,@kod);";

                int eff = DB.Execute(insertSql,
                    P("@d", cmbDesign.Text), P("@s", cmbSector.Text),
                    P("@p", cmbProductType.Text), P("@st", cmbStructureType.Text),
                    P("@v", cmbVoltageAyak.Text), P("@f", cmbFoundation.Text),
                    P("@t", cmbTower.Text), P("@h", height), P("@r", rev),
                    P("@tas", tasarim), P("@kod", urunKodu));

                if (eff > 0)
                {
                    ShowInsertedProduct(urunKodu);
                    DurumYaz("✅ Ürün eklendi: " + urunKodu, Color.FromArgb(140, 240, 160));
                    MessageBox.Show("✅ Yeni ürün eklendi.\n\nÜrün Kodu:\n" + urunKodu,
                        "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ekleme başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool YeniUrunDialog(out string height, out string rev, out string tasarim)
        {
            height = rev = tasarim = "";
            using (var dlg = new Form())
            {
                dlg.Text = "➕ Yeni Ürün Bilgileri";
                dlg.Size = new Size(480, 320);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false; dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(245, 245, 250);

                var lblInfo = new Label
                {
                    Text = "Seçili kategorilere ek olarak şu 3 alanı doldurun:",
                    Dock = DockStyle.Top,
                    Height = 36,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Italic),
                    ForeColor = Color.FromArgb(80, 80, 100),
                    Padding = new Padding(14, 8, 0, 0)
                };

                Label LBL(string t, int y) => new Label
                {
                    Text = t,
                    Location = new Point(14, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(40, 60, 90)
                };
                TextBox TXT(int y) => new TextBox
                {
                    Location = new Point(140, y - 4),
                    Size = new Size(300, 26),
                    Font = new Font("Segoe UI", 10f),
                    BorderStyle = BorderStyle.FixedSingle
                };

                var tH = TXT(60); var tR = TXT(100); var tT = TXT(140);
                dlg.Controls.Add(LBL("Height (Yükseklik):", 64));
                dlg.Controls.Add(LBL("Rev (Revizyon):", 104));
                dlg.Controls.Add(LBL("Tasarım Kodu:", 144));
                dlg.Controls.Add(tH); dlg.Controls.Add(tR); dlg.Controls.Add(tT);

                var btnOk = new Button
                {
                    Text = "✔ Ekle",
                    Size = new Size(140, 38),
                    Location = new Point(150, 220),
                    BackColor = Color.FromArgb(46, 139, 87),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    DialogResult = DialogResult.OK
                };
                btnOk.FlatAppearance.BorderSize = 0;
                var btnCancel = new Button
                {
                    Text = "✖ İptal",
                    Size = new Size(140, 38),
                    Location = new Point(300, 220),
                    BackColor = Color.FromArgb(160, 60, 60),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    DialogResult = DialogResult.Cancel
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                dlg.Controls.Add(btnOk);
                dlg.Controls.Add(btnCancel);
                dlg.Controls.Add(lblInfo);
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) != DialogResult.OK) return false;

                height = tH.Text?.Trim();
                rev = tR.Text?.Trim();
                tasarim = tT.Text?.Trim();

                if (string.IsNullOrWhiteSpace(height) || string.IsNullOrWhiteSpace(rev) || string.IsNullOrWhiteSpace(tasarim))
                {
                    MessageBox.Show("Tüm alanları doldurun.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
        }

        private string GenerateUrunKodu(params string[] parts)
        {
            string Clean(string s)
            {
                if (string.IsNullOrWhiteSpace(s)) return "NA";
                s = Regex.Replace(s.Trim(), @"\s+", " ").Replace("-", "_");
                return s;
            }
            var temiz = new List<string>();
            foreach (var p in parts) temiz.Add(Clean(p));
            return string.Join("-", temiz);
        }

        private bool ExistsUrunKodu(string urunKodu)
        {
            if (string.IsNullOrWhiteSpace(urunKodu)) return false;
            try
            {
                var res = DB.GetValue("SELECT COUNT(1) FROM dbo.Urun WHERE UrunKodu = @kod",
                    new SqlParameter("@kod", SqlDbType.NVarChar, 500) { Value = urunKodu });
                if (res == null || res == DBNull.Value) return false;
                return Convert.ToInt32(res) > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kontrol hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void ShowInsertedProduct(string urunKodu)
        {
            const string sql = @"SELECT Design, Sector, ProductType, StructureType, VoltageAyak, Foundation, Tower, Height, Rev, Tasarim, UrunKodu
                                 FROM dbo.Urun WHERE UrunKodu = @kod";
            var dt = DB.GetTable(sql, new SqlParameter("@kod", SqlDbType.NVarChar, 500) { Value = urunKodu });
            BindGrid(dt);
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[0].Selected = true;
            }
        }

        // ════════════════════════════════════════════════════════════
        //  STATUS
        // ════════════════════════════════════════════════════════════
        private void DurumYaz(string msg, Color renk)
        {
            try { lblStatus.Text = msg; lblStatus.ForeColor = renk; }
            catch { }
        }
    }
}