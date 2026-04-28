namespace BaranYardimci
{
    partial class GalvanizKontrolFormu
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.pnlUst = new System.Windows.Forms.Panel();
            this.lblFormBaslik = new System.Windows.Forms.Label();
            this.pnlSol = new System.Windows.Forms.Panel();
            this.grpGenel = new System.Windows.Forms.GroupBox();
            this.dtpTarih = new System.Windows.Forms.DateTimePicker();
            this.lblTarih = new System.Windows.Forms.Label();
            this.txtVardiyaNo = new System.Windows.Forms.TextBox();
            this.lblVardiyaNo = new System.Windows.Forms.Label();
            this.txtSure = new System.Windows.Forms.TextBox();
            this.lblSure = new System.Windows.Forms.Label();
            this.txtSicaklik = new System.Windows.Forms.TextBox();
            this.lblSicaklik = new System.Windows.Forms.Label();
            this.txtAskiSekli = new System.Windows.Forms.TextBox();
            this.lblAskiSekli = new System.Windows.Forms.Label();
            this.txtAskiAdeti = new System.Windows.Forms.TextBox();
            this.lblAskiAdeti = new System.Windows.Forms.Label();
            this.txtMalzemeCinsi = new System.Windows.Forms.TextBox();
            this.lblMalzemeCinsi = new System.Windows.Forms.Label();
            this.txtSozlesmeNo = new System.Windows.Forms.TextBox();
            this.lblSozlesmeNo = new System.Windows.Forms.Label();
            this.txtParcaPozu = new System.Windows.Forms.TextBox();
            this.lblParcaPozu = new System.Windows.Forms.Label();
            this.txtMusteriAdi = new System.Windows.Forms.TextBox();
            this.lblMusteriAdi = new System.Windows.Forms.Label();
            this.pnlSag = new System.Windows.Forms.Panel();
            this.grpOlcum = new System.Windows.Forms.GroupBox();
            this.lblOrtalamaKalinlik = new System.Windows.Forms.Label();
            this.txtOlcumSon = new System.Windows.Forms.TextBox();
            this.lblOlcumSon = new System.Windows.Forms.Label();
            this.txtOlcumOrta = new System.Windows.Forms.TextBox();
            this.lblOlcumOrta = new System.Windows.Forms.Label();
            this.txtOlcumBasi = new System.Windows.Forms.TextBox();
            this.lblOlcumBasi = new System.Windows.Forms.Label();
            this.txtNumuneMiktari = new System.Windows.Forms.TextBox();
            this.lblNumuneMiktari = new System.Windows.Forms.Label();
            this.grpOnay = new System.Windows.Forms.GroupBox();
            this.chkSartliOnay = new System.Windows.Forms.CheckBox();
            this.chkProsesOnay = new System.Windows.Forms.CheckBox();
            this.txtOnaylayan = new System.Windows.Forms.TextBox();
            this.lblOnaylayan = new System.Windows.Forms.Label();
            this.txtKaliteKontrolci = new System.Windows.Forms.TextBox();
            this.lblKaliteKontrolci = new System.Windows.Forms.Label();
            this.pnlOrta = new System.Windows.Forms.Panel();
            this.dgvPozlar = new System.Windows.Forms.DataGridView();
            this.pnlAlt = new System.Windows.Forms.Panel();
            this.btnIptal = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.pnlUst.SuspendLayout();
            this.pnlSol.SuspendLayout();
            this.grpGenel.SuspendLayout();
            this.pnlSag.SuspendLayout();
            this.grpOlcum.SuspendLayout();
            this.grpOnay.SuspendLayout();
            this.pnlOrta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPozlar)).BeginInit();
            this.pnlAlt.SuspendLayout();
            this.SuspendLayout();
            // pnlUst
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(30, 36, 40);
            this.pnlUst.Controls.Add(this.lblFormBaslik);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Size = new System.Drawing.Size(950, 50);
            // lblFormBaslik
            this.lblFormBaslik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFormBaslik.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblFormBaslik.ForeColor = System.Drawing.Color.White;
            this.lblFormBaslik.Text = "  GALVANIZ KONTROL FORMU";
            this.lblFormBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // pnlSol
            this.pnlSol.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSol.Size = new System.Drawing.Size(320, 500);
            this.pnlSol.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSol.Controls.Add(this.grpGenel);
            // grpGenel
            this.grpGenel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpGenel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpGenel.Text = "Genel Bilgiler";
            this.grpGenel.Padding = new System.Windows.Forms.Padding(10);
            int gY = 25;
            int gH = 28;
            int gA = 32;
            // lblMusteriAdi
            this.lblMusteriAdi.Text = "Musteri Adi:";
            this.lblMusteriAdi.Location = new System.Drawing.Point(15, gY);
            this.lblMusteriAdi.Size = new System.Drawing.Size(90, gH);
            this.lblMusteriAdi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMusteriAdi.Location = new System.Drawing.Point(110, gY);
            this.txtMusteriAdi.Size = new System.Drawing.Size(190, gH);
            this.txtMusteriAdi.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblParcaPozu
            this.lblParcaPozu.Text = "Parca Pozu:";
            this.lblParcaPozu.Location = new System.Drawing.Point(15, gY);
            this.lblParcaPozu.Size = new System.Drawing.Size(90, gH);
            this.lblParcaPozu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtParcaPozu.Location = new System.Drawing.Point(110, gY);
            this.txtParcaPozu.Size = new System.Drawing.Size(190, gH);
            this.txtParcaPozu.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblSozlesmeNo
            this.lblSozlesmeNo.Text = "Sozlesme No:";
            this.lblSozlesmeNo.Location = new System.Drawing.Point(15, gY);
            this.lblSozlesmeNo.Size = new System.Drawing.Size(90, gH);
            this.lblSozlesmeNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSozlesmeNo.Location = new System.Drawing.Point(110, gY);
            this.txtSozlesmeNo.Size = new System.Drawing.Size(190, gH);
            this.txtSozlesmeNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblMalzemeCinsi
            this.lblMalzemeCinsi.Text = "Malzeme Cinsi:";
            this.lblMalzemeCinsi.Location = new System.Drawing.Point(15, gY);
            this.lblMalzemeCinsi.Size = new System.Drawing.Size(90, gH);
            this.lblMalzemeCinsi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMalzemeCinsi.Location = new System.Drawing.Point(110, gY);
            this.txtMalzemeCinsi.Size = new System.Drawing.Size(190, gH);
            this.txtMalzemeCinsi.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblAskiAdeti
            this.lblAskiAdeti.Text = "Aski Adeti:";
            this.lblAskiAdeti.Location = new System.Drawing.Point(15, gY);
            this.lblAskiAdeti.Size = new System.Drawing.Size(90, gH);
            this.lblAskiAdeti.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAskiAdeti.Location = new System.Drawing.Point(110, gY);
            this.txtAskiAdeti.Size = new System.Drawing.Size(190, gH);
            this.txtAskiAdeti.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblAskiSekli
            this.lblAskiSekli.Text = "Aski Sekli:";
            this.lblAskiSekli.Location = new System.Drawing.Point(15, gY);
            this.lblAskiSekli.Size = new System.Drawing.Size(90, gH);
            this.lblAskiSekli.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAskiSekli.Location = new System.Drawing.Point(110, gY);
            this.txtAskiSekli.Size = new System.Drawing.Size(190, gH);
            this.txtAskiSekli.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblSicaklik
            this.lblSicaklik.Text = "Sicaklik (C):";
            this.lblSicaklik.Location = new System.Drawing.Point(15, gY);
            this.lblSicaklik.Size = new System.Drawing.Size(90, gH);
            this.lblSicaklik.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSicaklik.Location = new System.Drawing.Point(110, gY);
            this.txtSicaklik.Size = new System.Drawing.Size(190, gH);
            this.txtSicaklik.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblSure
            this.lblSure.Text = "Sure (dk):";
            this.lblSure.Location = new System.Drawing.Point(15, gY);
            this.lblSure.Size = new System.Drawing.Size(90, gH);
            this.lblSure.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSure.Location = new System.Drawing.Point(110, gY);
            this.txtSure.Size = new System.Drawing.Size(190, gH);
            this.txtSure.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblVardiyaNo
            this.lblVardiyaNo.Text = "Vardiya No:";
            this.lblVardiyaNo.Location = new System.Drawing.Point(15, gY);
            this.lblVardiyaNo.Size = new System.Drawing.Size(90, gH);
            this.lblVardiyaNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtVardiyaNo.Location = new System.Drawing.Point(110, gY);
            this.txtVardiyaNo.Size = new System.Drawing.Size(190, gH);
            this.txtVardiyaNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            gY += gA;
            // lblTarih
            this.lblTarih.Text = "Tarih:";
            this.lblTarih.Location = new System.Drawing.Point(15, gY);
            this.lblTarih.Size = new System.Drawing.Size(90, gH);
            this.lblTarih.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpTarih.Location = new System.Drawing.Point(110, gY);
            this.dtpTarih.Size = new System.Drawing.Size(190, gH);
            this.dtpTarih.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpTarih.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.grpGenel.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblMusteriAdi, this.txtMusteriAdi, this.lblParcaPozu, this.txtParcaPozu,
                this.lblSozlesmeNo, this.txtSozlesmeNo, this.lblMalzemeCinsi, this.txtMalzemeCinsi,
                this.lblAskiAdeti, this.txtAskiAdeti, this.lblAskiSekli, this.txtAskiSekli,
                this.lblSicaklik, this.txtSicaklik, this.lblSure, this.txtSure,
                this.lblVardiyaNo, this.txtVardiyaNo, this.lblTarih, this.dtpTarih });
            // pnlSag
            this.pnlSag.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSag.Size = new System.Drawing.Size(300, 500);
            this.pnlSag.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSag.Controls.Add(this.grpOnay);
            this.pnlSag.Controls.Add(this.grpOlcum);
            // grpOlcum
            this.grpOlcum.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpOlcum.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpOlcum.Text = "Olcum Bilgileri (um)";
            this.grpOlcum.Size = new System.Drawing.Size(280, 220);
            this.grpOlcum.Padding = new System.Windows.Forms.Padding(10);
            int oY = 25;
            this.lblNumuneMiktari.Text = "Numune Miktari:";
            this.lblNumuneMiktari.Location = new System.Drawing.Point(15, oY);
            this.lblNumuneMiktari.Size = new System.Drawing.Size(110, gH);
            this.lblNumuneMiktari.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNumuneMiktari.Location = new System.Drawing.Point(130, oY);
            this.txtNumuneMiktari.Size = new System.Drawing.Size(140, gH);
            this.txtNumuneMiktari.Font = new System.Drawing.Font("Segoe UI", 9F);
            oY += gA;
            this.lblOlcumBasi.Text = "Olcum Basi:";
            this.lblOlcumBasi.Location = new System.Drawing.Point(15, oY);
            this.lblOlcumBasi.Size = new System.Drawing.Size(110, gH);
            this.lblOlcumBasi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOlcumBasi.Location = new System.Drawing.Point(130, oY);
            this.txtOlcumBasi.Size = new System.Drawing.Size(140, gH);
            this.txtOlcumBasi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOlcumBasi.TextChanged += new System.EventHandler(this.txtOlcum_TextChanged);
            oY += gA;
            this.lblOlcumOrta.Text = "Olcum Orta:";
            this.lblOlcumOrta.Location = new System.Drawing.Point(15, oY);
            this.lblOlcumOrta.Size = new System.Drawing.Size(110, gH);
            this.lblOlcumOrta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOlcumOrta.Location = new System.Drawing.Point(130, oY);
            this.txtOlcumOrta.Size = new System.Drawing.Size(140, gH);
            this.txtOlcumOrta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOlcumOrta.TextChanged += new System.EventHandler(this.txtOlcum_TextChanged);
            oY += gA;
            this.lblOlcumSon.Text = "Olcum Son:";
            this.lblOlcumSon.Location = new System.Drawing.Point(15, oY);
            this.lblOlcumSon.Size = new System.Drawing.Size(110, gH);
            this.lblOlcumSon.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOlcumSon.Location = new System.Drawing.Point(130, oY);
            this.txtOlcumSon.Size = new System.Drawing.Size(140, gH);
            this.txtOlcumSon.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOlcumSon.TextChanged += new System.EventHandler(this.txtOlcum_TextChanged);
            oY += gA + 5;
            this.lblOrtalamaKalinlik.Text = "Ortalama: -";
            this.lblOrtalamaKalinlik.Location = new System.Drawing.Point(15, oY);
            this.lblOrtalamaKalinlik.Size = new System.Drawing.Size(255, 30);
            this.lblOrtalamaKalinlik.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblOrtalamaKalinlik.ForeColor = System.Drawing.Color.FromArgb(255, 140, 0);
            this.grpOlcum.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblNumuneMiktari, this.txtNumuneMiktari,
                this.lblOlcumBasi, this.txtOlcumBasi,
                this.lblOlcumOrta, this.txtOlcumOrta,
                this.lblOlcumSon, this.txtOlcumSon,
                this.lblOrtalamaKalinlik });
            // grpOnay
            this.grpOnay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpOnay.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpOnay.Text = "Onay Bilgileri";
            this.grpOnay.Padding = new System.Windows.Forms.Padding(10);
            int onY = 25;
            this.lblKaliteKontrolci.Text = "Kalite Kontrolcu:";
            this.lblKaliteKontrolci.Location = new System.Drawing.Point(15, onY);
            this.lblKaliteKontrolci.Size = new System.Drawing.Size(110, gH);
            this.lblKaliteKontrolci.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtKaliteKontrolci.Location = new System.Drawing.Point(130, onY);
            this.txtKaliteKontrolci.Size = new System.Drawing.Size(140, gH);
            this.txtKaliteKontrolci.Font = new System.Drawing.Font("Segoe UI", 9F);
            onY += gA;
            this.lblOnaylayan.Text = "Onaylayan:";
            this.lblOnaylayan.Location = new System.Drawing.Point(15, onY);
            this.lblOnaylayan.Size = new System.Drawing.Size(110, gH);
            this.lblOnaylayan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOnaylayan.Location = new System.Drawing.Point(130, onY);
            this.txtOnaylayan.Size = new System.Drawing.Size(140, gH);
            this.txtOnaylayan.Font = new System.Drawing.Font("Segoe UI", 9F);
            onY += gA + 5;
            this.chkProsesOnay.Text = "Proses Onay";
            this.chkProsesOnay.Location = new System.Drawing.Point(15, onY);
            this.chkProsesOnay.Size = new System.Drawing.Size(200, 25);
            this.chkProsesOnay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.chkProsesOnay.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
            onY += 30;
            this.chkSartliOnay.Text = "Sartli Onay";
            this.chkSartliOnay.Location = new System.Drawing.Point(15, onY);
            this.chkSartliOnay.Size = new System.Drawing.Size(200, 25);
            this.chkSartliOnay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.chkSartliOnay.ForeColor = System.Drawing.Color.FromArgb(243, 156, 18);
            this.grpOnay.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblKaliteKontrolci, this.txtKaliteKontrolci,
                this.lblOnaylayan, this.txtOnaylayan,
                this.chkProsesOnay, this.chkSartliOnay });
            // pnlOrta
            this.pnlOrta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOrta.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.pnlOrta.Controls.Add(this.dgvPozlar);
            // dgvPozlar
            this.dgvPozlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPozlar.Name = "dgvPozlar";
            this.dgvPozlar.Font = new System.Drawing.Font("Segoe UI", 9F);
            // pnlAlt
            this.pnlAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlt.Size = new System.Drawing.Size(950, 55);
            this.pnlAlt.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlAlt.Controls.Add(this.btnIptal);
            this.pnlAlt.Controls.Add(this.btnKaydet);
            // btnKaydet
            this.btnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnKaydet.Size = new System.Drawing.Size(200, 39);
            this.btnKaydet.Text = "Kaydet ve Onayla";
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnKaydet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // btnIptal
            this.btnIptal.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnIptal.Size = new System.Drawing.Size(120, 39);
            this.btnIptal.Text = "Iptal";
            this.btnIptal.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnIptal.ForeColor = System.Drawing.Color.White;
            this.btnIptal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIptal.FlatAppearance.BorderSize = 0;
            this.btnIptal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnIptal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // GalvanizKontrolFormu
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 600);
            this.Controls.Add(this.pnlOrta);
            this.Controls.Add(this.pnlSag);
            this.Controls.Add(this.pnlSol);
            this.Controls.Add(this.pnlAlt);
            this.Controls.Add(this.pnlUst);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Galvaniz Kontrol Formu";
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.Name = "GalvanizKontrolFormu";
            this.Load += new System.EventHandler(this.GalvanizKontrolFormu_Load);
            this.pnlUst.ResumeLayout(false);
            this.pnlSol.ResumeLayout(false);
            this.grpGenel.ResumeLayout(false);
            this.grpGenel.PerformLayout();
            this.pnlSag.ResumeLayout(false);
            this.grpOlcum.ResumeLayout(false);
            this.grpOlcum.PerformLayout();
            this.grpOnay.ResumeLayout(false);
            this.grpOnay.PerformLayout();
            this.pnlOrta.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPozlar)).EndInit();
            this.pnlAlt.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel pnlUst;
        private System.Windows.Forms.Label lblFormBaslik;
        private System.Windows.Forms.Panel pnlSol;
        private System.Windows.Forms.GroupBox grpGenel;
        private System.Windows.Forms.Label lblMusteriAdi;
        private System.Windows.Forms.TextBox txtMusteriAdi;
        private System.Windows.Forms.Label lblParcaPozu;
        private System.Windows.Forms.TextBox txtParcaPozu;
        private System.Windows.Forms.Label lblSozlesmeNo;
        private System.Windows.Forms.TextBox txtSozlesmeNo;
        private System.Windows.Forms.Label lblMalzemeCinsi;
        private System.Windows.Forms.TextBox txtMalzemeCinsi;
        private System.Windows.Forms.Label lblAskiAdeti;
        private System.Windows.Forms.TextBox txtAskiAdeti;
        private System.Windows.Forms.Label lblAskiSekli;
        private System.Windows.Forms.TextBox txtAskiSekli;
        private System.Windows.Forms.Label lblSicaklik;
        private System.Windows.Forms.TextBox txtSicaklik;
        private System.Windows.Forms.Label lblSure;
        private System.Windows.Forms.TextBox txtSure;
        private System.Windows.Forms.Label lblVardiyaNo;
        private System.Windows.Forms.TextBox txtVardiyaNo;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.DateTimePicker dtpTarih;
        private System.Windows.Forms.Panel pnlSag;
        private System.Windows.Forms.GroupBox grpOlcum;
        private System.Windows.Forms.Label lblNumuneMiktari;
        private System.Windows.Forms.TextBox txtNumuneMiktari;
        private System.Windows.Forms.Label lblOlcumBasi;
        private System.Windows.Forms.TextBox txtOlcumBasi;
        private System.Windows.Forms.Label lblOlcumOrta;
        private System.Windows.Forms.TextBox txtOlcumOrta;
        private System.Windows.Forms.Label lblOlcumSon;
        private System.Windows.Forms.TextBox txtOlcumSon;
        private System.Windows.Forms.Label lblOrtalamaKalinlik;
        private System.Windows.Forms.GroupBox grpOnay;
        private System.Windows.Forms.Label lblKaliteKontrolci;
        private System.Windows.Forms.TextBox txtKaliteKontrolci;
        private System.Windows.Forms.Label lblOnaylayan;
        private System.Windows.Forms.TextBox txtOnaylayan;
        private System.Windows.Forms.CheckBox chkProsesOnay;
        private System.Windows.Forms.CheckBox chkSartliOnay;
        private System.Windows.Forms.Panel pnlOrta;
        private System.Windows.Forms.DataGridView dgvPozlar;
        private System.Windows.Forms.Panel pnlAlt;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;
    }
}