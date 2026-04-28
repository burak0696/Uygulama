using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class GalvanizKontrolFormu : Form
    {
        private DataTable _secilenPozlar;

        public GalvanizKontrolFormu(DataTable secilenPozlar)
        {
            InitializeComponent();
            _secilenPozlar = secilenPozlar;
        }

        private void GalvanizKontrolFormu_Load(object sender, EventArgs e)
        {
            // Secilen pozlari listele
            dgvPozlar.DataSource = _secilenPozlar;
            if (dgvPozlar.Columns.Contains("Id"))
                dgvPozlar.Columns["Id"].Visible = false;

            dgvPozlar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPozlar.ReadOnly = true;
            dgvPozlar.AllowUserToAddRows = false;
            dgvPozlar.RowHeadersVisible = false;
            dgvPozlar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPozlar.BackgroundColor = Color.White;

            // Musteri adini otomatik doldur
            if (_secilenPozlar.Rows.Count > 0)
            {
                txtMusteriAdi.Text = _secilenPozlar.Rows[0]["MusteriAdi"]?.ToString() ?? "";
                txtParcaPozu.Text = _secilenPozlar.Rows[0]["PozNo"]?.ToString() ?? "";
                txtMalzemeCinsi.Text = _secilenPozlar.Rows[0]["Profil"]?.ToString() ?? "";
            }

            dtpTarih.Value = DateTime.Now;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            // Validasyon
            if (string.IsNullOrWhiteSpace(txtAskiAdeti.Text))
            {
                MessageBox.Show("Aski adeti giriniz!", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                double olcumBasi = SayiCevir(txtOlcumBasi.Text);
                double olcumOrta = SayiCevir(txtOlcumOrta.Text);
                double olcumSon = SayiCevir(txtOlcumSon.Text);
                double ortalamaKalinlik = 0;
                int olcumSayisi = 0;
                if (olcumBasi > 0) { ortalamaKalinlik += olcumBasi; olcumSayisi++; }
                if (olcumOrta > 0) { ortalamaKalinlik += olcumOrta; olcumSayisi++; }
                if (olcumSon > 0) { ortalamaKalinlik += olcumSon; olcumSayisi++; }
                if (olcumSayisi > 0) ortalamaKalinlik = Math.Round(ortalamaKalinlik / olcumSayisi, 2);

                bool prosesOnay = chkProsesOnay.Checked;
                bool sartliOnay = chkSartliOnay.Checked;

                foreach (DataRow poz in _secilenPozlar.Rows)
                {
                    int malzemeKayitId = Convert.ToInt32(poz["Id"]);

                    // GalvanizKontrol tablosuna kaydet
                    DB.Execute(
                        @"INSERT INTO GalvanizKontrol 
                        (MalzemeKayitId, MusteriAdi, ParcaPozu, SozlesmeNo, MalzemeCinsi, 
                         AskiAdeti, AskiSekli, Sicaklik, Sure, VardiyaNo, Tarih, 
                         NumuneMiktari, OlcumBasi, OlcumOrta, OlcumSon, OrtalamaKalinlik, 
                         KaliteKontrolci, Onaylayan, ProsesOnay, SartliOnay) 
                        VALUES 
                        (@mkId, @musteri, @parca, @sozlesme, @malzeme, 
                         @aski, @askiSekli, @sicaklik, @sure, @vardiya, @tarih, 
                         @numune, @olcBas, @olcOrta, @olcSon, @ortKalinlik, 
                         @kaliteKontrolci, @onaylayan, @prosesOnay, @sartliOnay)",
                        new SqlParameter("@mkId", malzemeKayitId),
                        new SqlParameter("@musteri", txtMusteriAdi.Text.Trim()),
                        new SqlParameter("@parca", txtParcaPozu.Text.Trim()),
                        new SqlParameter("@sozlesme", txtSozlesmeNo.Text.Trim()),
                        new SqlParameter("@malzeme", txtMalzemeCinsi.Text.Trim()),
                        new SqlParameter("@aski", SayiCevirInt(txtAskiAdeti.Text)),
                        new SqlParameter("@askiSekli", txtAskiSekli.Text.Trim()),
                        new SqlParameter("@sicaklik", txtSicaklik.Text.Trim()),
                        new SqlParameter("@sure", txtSure.Text.Trim()),
                        new SqlParameter("@vardiya", txtVardiyaNo.Text.Trim()),
                        new SqlParameter("@tarih", dtpTarih.Value),
                        new SqlParameter("@numune", SayiCevirInt(txtNumuneMiktari.Text)),
                        new SqlParameter("@olcBas", olcumBasi),
                        new SqlParameter("@olcOrta", olcumOrta),
                        new SqlParameter("@olcSon", olcumSon),
                        new SqlParameter("@ortKalinlik", ortalamaKalinlik),
                        new SqlParameter("@kaliteKontrolci", txtKaliteKontrolci.Text.Trim()),
                        new SqlParameter("@onaylayan", txtOnaylayan.Text.Trim()),
                        new SqlParameter("@prosesOnay", prosesOnay),
                        new SqlParameter("@sartliOnay", sartliOnay));

                    // MalzemeKayit durumunu guncelle
                    string yeniDurum = prosesOnay ? "Paketleme Bekliyor" : "Galvaniz Bekliyor";
                    string yeniGalvanizDurum = "Galvaniz Bitti";

                    DB.Execute(
                        "UPDATE MalzemeKayit SET MalzemeDurum = @durum, GalvanizDurum = @gDurum WHERE Id = @id",
                        new SqlParameter("@durum", yeniDurum),
                        new SqlParameter("@gDurum", yeniGalvanizDurum),
                        new SqlParameter("@id", malzemeKayitId));
                }

                MessageBox.Show(_secilenPozlar.Rows.Count + " poz icin galvaniz kontrol kaydi olusturuldu!",
                    "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtOlcum_TextChanged(object sender, EventArgs e)
        {
            double bas = SayiCevir(txtOlcumBasi.Text);
            double orta = SayiCevir(txtOlcumOrta.Text);
            double son = SayiCevir(txtOlcumSon.Text);
            int sayac = 0;
            double toplam = 0;
            if (bas > 0) { toplam += bas; sayac++; }
            if (orta > 0) { toplam += orta; sayac++; }
            if (son > 0) { toplam += son; sayac++; }
            if (sayac > 0)
                lblOrtalamaKalinlik.Text = "Ortalama: " + Math.Round(toplam / sayac, 2) + " um";
            else
                lblOrtalamaKalinlik.Text = "Ortalama: -";
        }

        private double SayiCevir(string deger)
        {
            if (string.IsNullOrWhiteSpace(deger)) return 0;
            string s = deger.Trim().Replace(",", ".");
            if (double.TryParse(s, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double sonuc))
                return sonuc;
            return 0;
        }

        private int SayiCevirInt(string deger)
        {
            if (string.IsNullOrWhiteSpace(deger)) return 0;
            if (int.TryParse(deger.Trim(), out int sonuc)) return sonuc;
            return 0;
        }
    }
}