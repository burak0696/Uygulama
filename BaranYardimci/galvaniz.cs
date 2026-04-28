using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class galvaniz : Form
    {
        private int aktifMusteriID;
        private string aktifMusteriAdi;
        private int aktifTeklifID;
        private int urunSayac = 1;
        DataTable dt;

        public galvaniz()
        {
            InitializeComponent();
            dt = new DataTable();
            dt.Columns.Add("colKaplama", typeof(string));
            dt.Columns.Add("colKg", typeof(decimal));
            dt.Columns.Add("colTonbasi", typeof(decimal));
            dt.Columns.Add("colTl", typeof(decimal));
            dgvUrunler.AutoGenerateColumns = false;
            dgvUrunler.DataSource = dt;
        }

        private void DgvUrunler_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvUrunler.Rows[e.RowIndex].DataBoundItem is DataRowView drv)
            {
                DataRow row = drv.Row;
                decimal kg = row["colKg"] == DBNull.Value ? 0 : Convert.ToDecimal(row["colKg"]);
                decimal tl = row["colTl"] == DBNull.Value ? 0 : Convert.ToDecimal(row["colTl"]);
                row["colTonbasi"] = (kg > 0) ? (tl / kg) : 0m;
            }
            else
            {
                var kgCell = dgvUrunler.Rows[e.RowIndex].Cells["colKg"]?.Value;
                var tlCell = dgvUrunler.Rows[e.RowIndex].Cells["colTl"]?.Value;
                decimal kg = 0, tl = 0;
                decimal.TryParse(Convert.ToString(kgCell), NumberStyles.Any, CultureInfo.CurrentCulture, out kg);
                decimal.TryParse(Convert.ToString(tlCell), NumberStyles.Any, CultureInfo.CurrentCulture, out tl);
                dgvUrunler.Rows[e.RowIndex].Cells["colTonbasi"].Value = (kg > 0) ? (tl / kg).ToString("N2") : "0.00";
            }
        }

        private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
                tb.Leave += (s, ev) => dgvUrunler.EndEdit();
        }

        private void btnIleri_Click(object sender, EventArgs e)
        {
            if (dgvMusteriler.CurrentRow == null) return;
            aktifMusteriID = Convert.ToInt32(dgvMusteriler.CurrentRow.Cells["MusteriID"].Value);
            string ad = dgvMusteriler.CurrentRow.Cells["Ad"].Value?.ToString() ?? "";
            string soyad = dgvMusteriler.CurrentRow.Cells["Soyad"].Value?.ToString() ?? "";
            aktifMusteriAdi = ad + " " + soyad;
            lblSecilenMusteri.Text = "Musteri: " + aktifMusteriAdi;

            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Teklifler (MusteriID) OUTPUT INSERTED.TeklifID VALUES (@mid)", conn);
                cmd.Parameters.AddWithValue("@mid", aktifMusteriID);
                conn.Open();
                object res = cmd.ExecuteScalar();
                if (res != null) aktifTeklifID = Convert.ToInt32(res);
            }
            pnlTeklif.Visible = true;
            KaplamaListesiniYukle();
        }

        private void KaplamaListesiniYukle()
        {
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT KaplamaAdi FROM Kaplamalar", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                cmbKaplama.Items.Clear();
                foreach (DataRow row in tmp.Rows)
                    cmbKaplama.Items.Add(row["KaplamaAdi"].ToString());
            }
        }

        private void btnTeklifKaydet_Click(object sender, EventArgs e)
        {
            if (dgvMusteriler.SelectedRows.Count == 0) { MessageBox.Show("Lutfen musteri secin."); return; }
            int musteriId = Convert.ToInt32(dgvMusteriler.SelectedRows[0].Cells["MusteriID"].Value);
            if (!decimal.TryParse(txtLME.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal girilenLme) ||
                !decimal.TryParse(txtMaliyet.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal girilenMaliyet))
            { MessageBox.Show("LME ve maliyet gecersiz."); return; }
            string textKur = txtdolar.Text.Replace(',', '.');
            if (!decimal.TryParse(textKur, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal girilenDolarKuru))
            { MessageBox.Show("Dolar kuru gecersiz."); return; }

            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Teklifler (MusteriID, LME, Maliyet, Tarih, [DolarKuru]) VALUES (@mid, @lme, @maliyet, @tarih, @kur); SELECT SCOPE_IDENTITY();", conn))
            {
                cmd.Parameters.AddWithValue("@mid", musteriId);
                cmd.Parameters.AddWithValue("@kur", girilenDolarKuru);
                cmd.Parameters.AddWithValue("@lme", girilenLme);
                cmd.Parameters.AddWithValue("@maliyet", girilenMaliyet);
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                conn.Open();
                object yeniId = cmd.ExecuteScalar();
                if (yeniId != null) aktifTeklifID = Convert.ToInt32(yeniId);
            }
            MessageBox.Show("Teklif basligi kaydedildi.");
        }

        private void btnUrunKaydet_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT TeklifID, Tarih FROM Teklifler WHERE MusteriID=@mid", conn);
                cmd.Parameters.AddWithValue("@mid", aktifMusteriID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                dgvGecmisTeklifler.DataSource = tmp;
                dgvGecmisTeklifler.Visible = true;
            }
        }

        private void dgvGecmisTeklifler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int teklifId = Convert.ToInt32(dgvGecmisTeklifler.Rows[e.RowIndex].Cells["TeklifID"].Value);
            if (dgvMusteriler.SelectedRows.Count == 0) { MessageBox.Show("Lutfen once musteri secin."); return; }
            TeklifDetaylariniGoster(teklifId);

            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            using (SqlCommand cmd = new SqlCommand("SELECT LME, Maliyet, Tarih, [DolarKuru] FROM Teklifler WHERE TeklifID=@tid", conn))
            {
                cmd.Parameters.AddWithValue("@tid", teklifId);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        decimal lme = rdr["LME"] != DBNull.Value ? Convert.ToDecimal(rdr["LME"]) : 0;
                        decimal mali = rdr["Maliyet"] != DBNull.Value ? Convert.ToDecimal(rdr["Maliyet"]) : 0;
                        DateTime tar = rdr["Tarih"] != DBNull.Value ? Convert.ToDateTime(rdr["Tarih"]) : DateTime.MinValue;
                        decimal kur = rdr["DolarKuru"] != DBNull.Value ? Convert.ToDecimal(rdr["DolarKuru"]) : 0;
                        string ad = dgvMusteriler.SelectedRows[0].Cells["Ad"].Value?.ToString() ?? "";
                        string soyad = dgvMusteriler.SelectedRows[0].Cells["Soyad"].Value?.ToString() ?? "";
                        lblDetay.Text = $"Musteri: {ad} {soyad}\nTeklifID: {teklifId}\nTarih: {tar:dd.MM.yyyy}\nLME: {lme:N2}\nMaliyet: {mali:N2}\nDolar Kuru: {kur:N4}";
                        pnlDetay.Visible = true;
                    }
                }
            }
        }

        private void TeklifDetaylariniGoster(int teklifId)
        {
            dt.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Teklifler WHERE TeklifID=@tid", conn))
            {
                cmd.Parameters.AddWithValue("@tid", teklifId);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            object urun = rdr[$"Urun{i}"];
                            object kg = rdr[$"Urun{i}Kg"];
                            object tutar = rdr[$"Urun{i}Tutar"];
                            object tlKarsilik = rdr[$"Urun{i}TlKarsilik"];
                            if (urun != DBNull.Value && !string.IsNullOrWhiteSpace(urun.ToString()))
                            {
                                DataRow newRow = dt.NewRow();
                                newRow["colKaplama"] = urun.ToString();
                                newRow["colKg"] = (kg != DBNull.Value) ? Convert.ToDecimal(kg) : 0m;
                                newRow["colTonbasi"] = (tutar != DBNull.Value) ? Convert.ToDecimal(tutar) : 0m;
                                newRow["colTl"] = (tlKarsilik != DBNull.Value) ? Convert.ToDecimal(tlKarsilik) : 0m;
                                dt.Rows.Add(newRow);
                            }
                        }
                    }
                }
            }
        }

        private void btnMusteriAra_Click(object sender, EventArgs e)
        {
            string aranan = txtMusteriAra.Text.Trim();
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT MusteriID, Ad, Soyad, Email, Telefon, KayitTarihi FROM Musteri WHERE Ad LIKE @p OR Soyad LIKE @p", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@p", "%" + aranan + "%");
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                dgvMusteriler.DataSource = tmp;
            }
        }

        private void dgvMusteriler_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMusteriler.SelectedRows.Count == 0) return;
            int musteriId = Convert.ToInt32(dgvMusteriler.SelectedRows[0].Cells["MusteriID"].Value);
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT TeklifID, Tarih FROM Teklifler WHERE MusteriID=@mid ORDER BY TeklifID DESC", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@mid", musteriId);
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                dgvGecmisTeklifler.DataSource = tmp;
            }
        }

        void KaydetSQL(string kaplama, decimal kg, decimal tlkarsilik, decimal tonbasi)
        {
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            using (SqlCommand cmd = new SqlCommand("INSERT INTO TeklifKalemleri (kaplama, kg, tlkarsilik, tonbasi) VALUES (@kaplama, @kg, @tlkarsilik, @tonbasi)", conn))
            {
                cmd.Parameters.AddWithValue("@kaplama", kaplama);
                cmd.Parameters.AddWithValue("@kg", kg);
                cmd.Parameters.AddWithValue("@tlkarsilik", tlkarsilik);
                cmd.Parameters.AddWithValue("@tonbasi", tonbasi);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            if (aktifTeklifID <= 0) { MessageBox.Show("Once Teklif Kaydet yapin."); return; }
            if (cmbKaplama.SelectedItem == null) { MessageBox.Show("Kaplama secin."); return; }
            if (!decimal.TryParse(txtKg.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal girilenKg) || girilenKg <= 0)
            { MessageBox.Show("Kg gecersiz."); return; }
            if (!decimal.TryParse(txtLME.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal girilenLme) ||
                !decimal.TryParse(txtMaliyet.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal girilenMaliyet))
            { MessageBox.Show("LME veya maliyet hatali."); return; }
            string textKur = txtdolar.Text.Replace(',', '.');
            if (!decimal.TryParse(textKur, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal girilenDolarKuru) || girilenDolarKuru <= 0)
            { MessageBox.Show("Dolar kuru gecersiz."); return; }

            int slot = urunSayac;
            if (slot < 1 || slot > 10) { MessageBox.Show("Maksimum urun adedine ulasildi (10)."); return; }
            string secilenKaplama = cmbKaplama.SelectedItem.ToString();
            var m = Regex.Match(secilenKaplama, @"\d+");
            if (!m.Success || !int.TryParse(m.Value, out int kaplamaDegeri)) { MessageBox.Show("Kaplama degeri sayisal degil."); return; }

            decimal sabit = 225m;
            decimal kaplamaOrani = kaplamaDegeri / 1000m;
            decimal galvanizmaliyeti = (sabit + girilenLme) * kaplamaOrani;
            decimal iscilik = girilenMaliyet - galvanizmaliyeti;
            decimal tonbasi = galvanizmaliyeti + iscilik;
            decimal toplamTutar = (girilenKg / 1000m) * tonbasi;
            decimal tlKarsilik = toplamTutar * girilenDolarKuru;

            string sql = $"UPDATE Teklifler SET Urun{slot}=@u, Urun{slot}Kg=@kg, Urun{slot}Tutar=@tutar, Urun{slot}TlKarsilik=@tlkarsilik WHERE TeklifID=@tid";
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", secilenKaplama);
                cmd.Parameters.AddWithValue("@kg", girilenKg);
                cmd.Parameters.AddWithValue("@tutar", tonbasi);
                cmd.Parameters.AddWithValue("@tlkarsilik", tlKarsilik);
                cmd.Parameters.AddWithValue("@tid", aktifTeklifID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            KaydetSQL(secilenKaplama, girilenKg, tlKarsilik, tonbasi);

            DataRow row = dt.NewRow();
            row["colKaplama"] = secilenKaplama;
            row["colKg"] = girilenKg;
            row["colTonbasi"] = tonbasi;
            row["colTl"] = tlKarsilik;
            dt.Rows.Add(row);
            urunSayac++;
        }

        private void btnUrunSil_Click(object sender, EventArgs e)
        {
            if (dgvUrunler.SelectedRows.Count == 0) return;
            int index = dgvUrunler.SelectedRows[0].Index + 1;
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"UPDATE Teklifler SET Urun{index}=NULL, Urun{index}Kg=NULL, Urun{index}Tutar=NULL, Urun{index}TlKarsilik=NULL WHERE TeklifID=@tid", conn);
                cmd.Parameters.AddWithValue("@tid", aktifTeklifID);
                cmd.ExecuteNonQuery();
            }
            if (index - 1 >= 0 && index - 1 < dt.Rows.Count) dt.Rows.RemoveAt(index - 1);
        }

        private void btnGecmisTeklifler_Click(object sender, EventArgs e)
        {
            if (dgvMusteriler.SelectedRows.Count == 0) { MessageBox.Show("Lutfen once bir musteri secin."); return; }
            int musteriId = Convert.ToInt32(dgvMusteriler.SelectedRows[0].Cells["MusteriID"].Value);
            using (SqlConnection conn = new SqlConnection(DB.ConnStr))
            using (SqlDataAdapter da = new SqlDataAdapter("SELECT TeklifID, Tarih FROM Teklifler WHERE MusteriID=@mid ORDER BY TeklifID DESC", conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@mid", musteriId);
                DataTable tmp = new DataTable();
                da.Fill(tmp);
                dgvGecmisTeklifler.DataSource = tmp;
                dgvGecmisTeklifler.Visible = true;
            }
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PDF cikti modulu hazirlaniyor...");
        }

        private void btnYeniTeklif_Click(object sender, EventArgs e)
        {
            galvaniz yeniForm = new galvaniz();
            yeniForm.Show();
            this.Close();
        }

        private void btnTeklifiKaydet_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Teklif kaydedildi!");
        }
    }
}