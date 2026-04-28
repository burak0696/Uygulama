using System;
using System.Windows.Forms;
using BaranYardimci;

namespace BARAN
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        public MainMenu(string user) : this()
        {
            lblBaslik.Text = $"Hoşgeldiniz, Sayın {user}";
        }

        private void btnUrunAra_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ürün Arama modülü yakında aktif olacak.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMusteri_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Müşteri Paneli modülü yakında aktif olacak.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGalvaniz_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Galvaniz Teklif modülü yakında aktif olacak.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSatinalma_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new Sarf())
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dönüştürücü formu hatası: " + ex.Message);
            }
        }

        private void btnTeklif_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Teklif modülü yakında aktif olacak.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBakimOnarim_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bakım / Onarım modülü yakında aktif olacak.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUretim_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Üretim modülü yakında aktif olacak.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnIK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("İnsan Kaynakları modülü yakında aktif olacak.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDonusturucu_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new Donusturucu())
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dönüştürücü formu hatası: " + ex.Message);
            }
        }

        private void btnImalatPlanlama_Click(object sender, EventArgs e)
        {
            MessageBox.Show("İmalat Planlama modülü yakında aktif olacak.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGalvanizKontrol_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new GalvanizKontrol())
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Galvaniz Kontrol formu hatası: " + ex.Message);
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new galvaniz())
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Galvaniz formu hatası: " + ex.Message);
            }
        }
    }
}