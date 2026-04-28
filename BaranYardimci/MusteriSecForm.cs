using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class MusteriSecForm : Form
    {
        public int SecilenMusteriID { get; private set; } = -1;
        public string SecilenMusteriAd { get; private set; } = "";
        public string SecilenMusteriSoyad { get; private set; } = "";
        public string SecilenMalzemeDurum { get; private set; } = "";

        public MusteriSecForm()
        {
            InitializeComponent();
        }

        private void MusteriSecForm_Load(object sender, EventArgs e)
        {
            cmbMalzemeDurum.Items.Add("İmalatta");
            cmbMalzemeDurum.Items.Add("Galvaniz Bekliyor");
            cmbMalzemeDurum.Items.Add("Paketleme Bekliyor");
            cmbMalzemeDurum.Items.Add("Paketlendi");
            cmbMalzemeDurum.SelectedIndex = 0;

            MusterileriYukle("");
        }

        private void MusterileriYukle(string aramaMetni)
        {
            string sorgu;
            SqlParameter[] parametreler;

            if (string.IsNullOrWhiteSpace(aramaMetni))
            {
                sorgu = "SELECT MusteriID, Ad, Soyad, Email, Telefon FROM Musteri ORDER BY Ad, Soyad";
                parametreler = null;
            }
            else
            {
                sorgu = @"SELECT MusteriID, Ad, Soyad, Email, Telefon 
                          FROM Musteri 
                          WHERE Ad LIKE @ara OR Soyad LIKE @ara OR Email LIKE @ara OR Telefon LIKE @ara
                          ORDER BY Ad, Soyad";
                parametreler = new SqlParameter[] { new SqlParameter("@ara", "%" + aramaMetni + "%") };
            }

            dgvMusteriler.DataSource = DB.GetTable(sorgu, parametreler);

            if (dgvMusteriler.Columns.Count > 0)
            {
                dgvMusteriler.Columns["MusteriID"].HeaderText = "ID";
                dgvMusteriler.Columns["MusteriID"].Width = 50;
                dgvMusteriler.Columns["Ad"].HeaderText = "Ad";
                dgvMusteriler.Columns["Soyad"].HeaderText = "VN";
                dgvMusteriler.Columns["Email"].HeaderText = "E-Posta";
                dgvMusteriler.Columns["Telefon"].HeaderText = "Telefon";
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            MusterileriYukle(txtAra.Text.Trim());
        }

        private void txtAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAra_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void btnYeniMusteri_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtYeniAd.Text))
            {
                MessageBox.Show("Ad zorunludur!", "Uyari",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DB.Execute(
                    "INSERT INTO Musteri (Ad, Soyad, Email, Telefon) VALUES (@ad, @soyad, @email, @telefon)",
                    new SqlParameter("@ad", txtYeniAd.Text.Trim()),
                    new SqlParameter("@soyad", txtYeniVN.Text.Trim()),
                    new SqlParameter("@email", txtYeniEmail.Text.Trim()),
                    new SqlParameter("@telefon", txtYeniTelefon.Text.Trim())
                );

                MessageBox.Show("Müşteri eklendi!", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtYeniAd.Clear();
                txtYeniVN.Clear();
                txtYeniEmail.Clear();
                txtYeniTelefon.Clear();
                MusterileriYukle("");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOnayla_Click(object sender, EventArgs e)
        {
            if (dgvMusteriler.CurrentRow == null || dgvMusteriler.CurrentRow.Index < 0)
            {
                MessageBox.Show("Lütfen listeden bir müşteri seçin!", "Uyari",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvMusteriler.CurrentRow;
            SecilenMusteriID = Convert.ToInt32(row.Cells["MusteriID"].Value);
            SecilenMusteriAd = row.Cells["Ad"].Value?.ToString() ?? "";
            SecilenMusteriSoyad = row.Cells["Soyad"].Value?.ToString() ?? "";
            SecilenMalzemeDurum = cmbMalzemeDurum.SelectedItem.ToString();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgvMusteriler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnOnayla_Click(sender, e);
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}