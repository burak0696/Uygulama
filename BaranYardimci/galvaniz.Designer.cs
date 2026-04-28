namespace BaranYardimci
{
    partial class galvaniz
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.pnlUstBar = new System.Windows.Forms.Panel();
            this.lblFormBaslik = new System.Windows.Forms.Label();
            this.pnlMusteriUst = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.btnMusteriAra = new System.Windows.Forms.Button();
            this.txtMusteriAra = new System.Windows.Forms.TextBox();
            this.lblMusteriAraLabel = new System.Windows.Forms.Label();
            this.btnIleri = new System.Windows.Forms.Button();
            this.btnGecmisTeklifler = new System.Windows.Forms.Button();
            this.dgvMusteriler = new System.Windows.Forms.DataGridView();
            this.pnlParametreler = new System.Windows.Forms.Panel();
            this.btnTeklifKaydet = new System.Windows.Forms.Button();
            this.txtdolar = new System.Windows.Forms.TextBox();
            this.lblDolarKuru = new System.Windows.Forms.Label();
            this.txtMaliyet = new System.Windows.Forms.TextBox();
            this.lblMaliyet = new System.Windows.Forms.Label();
            this.txtLME = new System.Windows.Forms.TextBox();
            this.lblLME = new System.Windows.Forms.Label();
            this.lblSecilenMusteri = new System.Windows.Forms.Label();
            this.pnlTeklif = new System.Windows.Forms.Panel();
            this.pnlUrunGiris = new System.Windows.Forms.Panel();
            this.lblUrunTuru = new System.Windows.Forms.Label();
            this.cmbKaplama = new System.Windows.Forms.ComboBox();
            this.lblKgLabel = new System.Windows.Forms.Label();
            this.txtKg = new System.Windows.Forms.TextBox();
            this.btnUrunEkle = new System.Windows.Forms.Button();
            this.pnlUrunButonlar = new System.Windows.Forms.Panel();
            this.btnUrunSil = new System.Windows.Forms.Button();
            this.btnUrunKaydet = new System.Windows.Forms.Button();
            this.btnPdf = new System.Windows.Forms.Button();
            this.dgvUrunler = new System.Windows.Forms.DataGridView();
            this.colKaplama = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTonbasi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSagAlt = new System.Windows.Forms.Panel();
            this.pnlDetay = new System.Windows.Forms.Panel();
            this.lblDetay = new System.Windows.Forms.Label();
            this.lblDetayBaslik = new System.Windows.Forms.Label();
            this.dgvGecmisTeklifler = new System.Windows.Forms.DataGridView();
            this.lblGecmisBaslik = new System.Windows.Forms.Label();
            this.pnlAltButonlar = new System.Windows.Forms.Panel();
            this.btnYeniTeklif = new System.Windows.Forms.Button();
            this.btnTeklifiKaydet = new System.Windows.Forms.Button();
            this.pnlUstBar.SuspendLayout();
            this.pnlMusteriUst.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMusteriler)).BeginInit();
            this.pnlParametreler.SuspendLayout();
            this.pnlTeklif.SuspendLayout();
            this.pnlUrunGiris.SuspendLayout();
            this.pnlUrunButonlar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUrunler)).BeginInit();
            this.pnlSagAlt.SuspendLayout();
            this.pnlDetay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGecmisTeklifler)).BeginInit();
            this.pnlAltButonlar.SuspendLayout();
            this.SuspendLayout();
            // ==================== UST BAR ====================
            this.pnlUstBar.BackColor = System.Drawing.Color.FromArgb(30, 36, 40);
            this.pnlUstBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUstBar.Size = new System.Drawing.Size(1280, 55);
            this.pnlUstBar.Controls.Add(this.lblFormBaslik);
            this.lblFormBaslik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFormBaslik.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblFormBaslik.ForeColor = System.Drawing.Color.White;
            this.lblFormBaslik.Text = "  BARAN CELIK & GALVANIZ  |  TEKLIF MODULU";
            this.lblFormBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // ==================== MUSTERI PANEL ====================
            this.pnlMusteriUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMusteriUst.Size = new System.Drawing.Size(1280, 230);
            this.pnlMusteriUst.Padding = new System.Windows.Forms.Padding(15, 10, 15, 5);
            this.pnlMusteriUst.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.pnlMusteriUst.Controls.Add(this.dgvMusteriler);
            this.pnlMusteriUst.Controls.Add(this.label16);
            this.pnlMusteriUst.Controls.Add(this.btnMusteriAra);
            this.pnlMusteriUst.Controls.Add(this.txtMusteriAra);
            this.pnlMusteriUst.Controls.Add(this.lblMusteriAraLabel);
            this.pnlMusteriUst.Controls.Add(this.btnIleri);
            this.pnlMusteriUst.Controls.Add(this.btnGecmisTeklifler);
            // label16
            this.label16.Location = new System.Drawing.Point(20, 12);
            this.label16.Size = new System.Drawing.Size(300, 20);
            this.label16.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label16.ForeColor = System.Drawing.Color.FromArgb(30, 36, 40);
            this.label16.Text = "Musteriye Tiklayip Sec Tusuna Basiniz";
            // dgvMusteriler
            this.dgvMusteriler.Location = new System.Drawing.Point(20, 35);
            this.dgvMusteriler.Size = new System.Drawing.Size(550, 180);
            this.dgvMusteriler.MultiSelect = false;
            this.dgvMusteriler.ReadOnly = true;
            this.dgvMusteriler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMusteriler.AllowUserToAddRows = false;
            this.dgvMusteriler.AllowUserToDeleteRows = false;
            this.dgvMusteriler.RowHeadersVisible = false;
            this.dgvMusteriler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMusteriler.BackgroundColor = System.Drawing.Color.White;
            this.dgvMusteriler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMusteriler.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvMusteriler.SelectionChanged += new System.EventHandler(this.dgvMusteriler_SelectionChanged);
            // lblMusteriAraLabel
            this.lblMusteriAraLabel.Location = new System.Drawing.Point(590, 35);
            this.lblMusteriAraLabel.Size = new System.Drawing.Size(90, 25);
            this.lblMusteriAraLabel.Text = "Musteri Ara:";
            this.lblMusteriAraLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMusteriAraLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // txtMusteriAra
            this.txtMusteriAra.Location = new System.Drawing.Point(590, 62);
            this.txtMusteriAra.Size = new System.Drawing.Size(180, 28);
            this.txtMusteriAra.Font = new System.Drawing.Font("Segoe UI", 10F);
            // btnMusteriAra
            this.btnMusteriAra.Location = new System.Drawing.Point(780, 60);
            this.btnMusteriAra.Size = new System.Drawing.Size(100, 30);
            this.btnMusteriAra.Text = "Ara";
            this.btnMusteriAra.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnMusteriAra.ForeColor = System.Drawing.Color.White;
            this.btnMusteriAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMusteriAra.FlatAppearance.BorderSize = 0;
            this.btnMusteriAra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMusteriAra.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMusteriAra.Click += new System.EventHandler(this.btnMusteriAra_Click);
            // btnIleri
            this.btnIleri.Location = new System.Drawing.Point(590, 105);
            this.btnIleri.Size = new System.Drawing.Size(140, 35);
            this.btnIleri.Text = "Musteriyi Sec";
            this.btnIleri.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            this.btnIleri.ForeColor = System.Drawing.Color.White;
            this.btnIleri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIleri.FlatAppearance.BorderSize = 0;
            this.btnIleri.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnIleri.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIleri.Click += new System.EventHandler(this.btnIleri_Click);
            // btnGecmisTeklifler
            this.btnGecmisTeklifler.Location = new System.Drawing.Point(740, 105);
            this.btnGecmisTeklifler.Size = new System.Drawing.Size(140, 35);
            this.btnGecmisTeklifler.Text = "Gecmis Teklifler";
            this.btnGecmisTeklifler.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnGecmisTeklifler.ForeColor = System.Drawing.Color.White;
            this.btnGecmisTeklifler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGecmisTeklifler.FlatAppearance.BorderSize = 0;
            this.btnGecmisTeklifler.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGecmisTeklifler.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGecmisTeklifler.Click += new System.EventHandler(this.btnGecmisTeklifler_Click);
            // ==================== PARAMETRELER ====================
            this.pnlParametreler.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlParametreler.Size = new System.Drawing.Size(1280, 70);
            this.pnlParametreler.BackColor = System.Drawing.Color.FromArgb(230, 232, 235);
            this.pnlParametreler.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.pnlParametreler.Controls.Add(this.lblSecilenMusteri);
            this.pnlParametreler.Controls.Add(this.lblLME);
            this.pnlParametreler.Controls.Add(this.txtLME);
            this.pnlParametreler.Controls.Add(this.lblMaliyet);
            this.pnlParametreler.Controls.Add(this.txtMaliyet);
            this.pnlParametreler.Controls.Add(this.lblDolarKuru);
            this.pnlParametreler.Controls.Add(this.txtdolar);
            this.pnlParametreler.Controls.Add(this.btnTeklifKaydet);
            // lblSecilenMusteri
            this.lblSecilenMusteri.Location = new System.Drawing.Point(20, 8);
            this.lblSecilenMusteri.Size = new System.Drawing.Size(300, 22);
            this.lblSecilenMusteri.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSecilenMusteri.ForeColor = System.Drawing.Color.FromArgb(255, 140, 0);
            // lblLME
            this.lblLME.Location = new System.Drawing.Point(20, 35);
            this.lblLME.Size = new System.Drawing.Size(70, 25);
            this.lblLME.Text = "LME Fiyati:";
            this.lblLME.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLME.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // txtLME
            this.txtLME.Location = new System.Drawing.Point(95, 37);
            this.txtLME.Size = new System.Drawing.Size(100, 25);
            this.txtLME.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            // lblMaliyet
            this.lblMaliyet.Location = new System.Drawing.Point(210, 35);
            this.lblMaliyet.Size = new System.Drawing.Size(55, 25);
            this.lblMaliyet.Text = "Maliyet:";
            this.lblMaliyet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMaliyet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // txtMaliyet
            this.txtMaliyet.Location = new System.Drawing.Point(270, 37);
            this.txtMaliyet.Size = new System.Drawing.Size(100, 25);
            this.txtMaliyet.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            // lblDolarKuru
            this.lblDolarKuru.Location = new System.Drawing.Point(390, 35);
            this.lblDolarKuru.Size = new System.Drawing.Size(75, 25);
            this.lblDolarKuru.Text = "Dolar Kuru:";
            this.lblDolarKuru.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDolarKuru.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // txtdolar
            this.txtdolar.Location = new System.Drawing.Point(470, 37);
            this.txtdolar.Size = new System.Drawing.Size(100, 25);
            this.txtdolar.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            // btnTeklifKaydet
            this.btnTeklifKaydet.Location = new System.Drawing.Point(590, 33);
            this.btnTeklifKaydet.Size = new System.Drawing.Size(140, 32);
            this.btnTeklifKaydet.Text = "Teklife Basla";
            this.btnTeklifKaydet.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnTeklifKaydet.ForeColor = System.Drawing.Color.White;
            this.btnTeklifKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeklifKaydet.FlatAppearance.BorderSize = 0;
            this.btnTeklifKaydet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTeklifKaydet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTeklifKaydet.Click += new System.EventHandler(this.btnTeklifKaydet_Click);
            // ==================== TEKLIF PANEL ====================
            this.pnlTeklif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTeklif.Visible = false;
            this.pnlTeklif.BackColor = System.Drawing.Color.White;
            this.pnlTeklif.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.pnlTeklif.Controls.Add(this.dgvUrunler);
            this.pnlTeklif.Controls.Add(this.pnlUrunButonlar);
            this.pnlTeklif.Controls.Add(this.pnlUrunGiris);
            this.pnlTeklif.Controls.Add(this.pnlSagAlt);
            this.pnlTeklif.Controls.Add(this.pnlAltButonlar);
            // pnlUrunGiris
            this.pnlUrunGiris.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUrunGiris.Size = new System.Drawing.Size(1250, 50);
            this.pnlUrunGiris.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.pnlUrunGiris.Padding = new System.Windows.Forms.Padding(10, 10, 10, 5);
            this.pnlUrunGiris.Controls.Add(this.lblUrunTuru);
            this.pnlUrunGiris.Controls.Add(this.cmbKaplama);
            this.pnlUrunGiris.Controls.Add(this.lblKgLabel);
            this.pnlUrunGiris.Controls.Add(this.txtKg);
            this.pnlUrunGiris.Controls.Add(this.btnUrunEkle);
            // lblUrunTuru
            this.lblUrunTuru.Location = new System.Drawing.Point(15, 12);
            this.lblUrunTuru.Size = new System.Drawing.Size(80, 25);
            this.lblUrunTuru.Text = "Urun Turu:";
            this.lblUrunTuru.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUrunTuru.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // cmbKaplama
            this.cmbKaplama.Location = new System.Drawing.Point(100, 12);
            this.cmbKaplama.Size = new System.Drawing.Size(220, 28);
            this.cmbKaplama.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKaplama.Font = new System.Drawing.Font("Segoe UI", 10F);
            // lblKgLabel
            this.lblKgLabel.Location = new System.Drawing.Point(340, 12);
            this.lblKgLabel.Size = new System.Drawing.Size(65, 25);
            this.lblKgLabel.Text = "KG Giriniz:";
            this.lblKgLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // txtKg
            this.txtKg.Location = new System.Drawing.Point(410, 12);
            this.txtKg.Size = new System.Drawing.Size(120, 28);
            this.txtKg.Font = new System.Drawing.Font("Segoe UI", 10F);
            // btnUrunEkle
            this.btnUrunEkle.Location = new System.Drawing.Point(550, 8);
            this.btnUrunEkle.Size = new System.Drawing.Size(130, 32);
            this.btnUrunEkle.Text = "Urun Ekle";
            this.btnUrunEkle.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnUrunEkle.ForeColor = System.Drawing.Color.White;
            this.btnUrunEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrunEkle.FlatAppearance.BorderSize = 0;
            this.btnUrunEkle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUrunEkle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUrunEkle.Click += new System.EventHandler(this.btnUrunEkle_Click);
            // pnlUrunButonlar
            this.pnlUrunButonlar.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUrunButonlar.Size = new System.Drawing.Size(150, 300);
            this.pnlUrunButonlar.Padding = new System.Windows.Forms.Padding(10, 60, 10, 10);
            this.pnlUrunButonlar.BackColor = System.Drawing.Color.White;
            this.pnlUrunButonlar.Controls.Add(this.btnPdf);
            this.pnlUrunButonlar.Controls.Add(this.btnUrunKaydet);
            this.pnlUrunButonlar.Controls.Add(this.btnUrunSil);
            // btnUrunSil
            this.btnUrunSil.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUrunSil.Size = new System.Drawing.Size(130, 38);
            this.btnUrunSil.Text = "Urun Sil";
            this.btnUrunSil.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnUrunSil.ForeColor = System.Drawing.Color.White;
            this.btnUrunSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrunSil.FlatAppearance.BorderSize = 0;
            this.btnUrunSil.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUrunSil.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUrunSil.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.btnUrunSil.Click += new System.EventHandler(this.btnUrunSil_Click);
            // btnUrunKaydet
            this.btnUrunKaydet.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUrunKaydet.Size = new System.Drawing.Size(130, 38);
            this.btnUrunKaydet.Text = "Urunleri Kaydet";
            this.btnUrunKaydet.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnUrunKaydet.ForeColor = System.Drawing.Color.White;
            this.btnUrunKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrunKaydet.FlatAppearance.BorderSize = 0;
            this.btnUrunKaydet.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUrunKaydet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUrunKaydet.Click += new System.EventHandler(this.btnUrunKaydet_Click);
            // btnPdf
            this.btnPdf.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPdf.Size = new System.Drawing.Size(130, 38);
            this.btnPdf.Text = "PDF Cikti";
            this.btnPdf.BackColor = System.Drawing.Color.FromArgb(142, 68, 173);
            this.btnPdf.ForeColor = System.Drawing.Color.White;
            this.btnPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPdf.FlatAppearance.BorderSize = 0;
            this.btnPdf.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPdf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            // dgvUrunler
            this.dgvUrunler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUrunler.AllowUserToAddRows = false;
            this.dgvUrunler.AllowUserToDeleteRows = false;
            this.dgvUrunler.RowHeadersVisible = false;
            this.dgvUrunler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUrunler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUrunler.BackgroundColor = System.Drawing.Color.White;
            this.dgvUrunler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUrunler.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvUrunler.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 36, 40);
            this.dgvUrunler.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvUrunler.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dgvUrunler.EnableHeadersVisualStyles = false;
            this.dgvUrunler.ColumnHeadersHeight = 35;
            this.dgvUrunler.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colKaplama, this.colKg, this.colTonbasi, this.colTl });
            // colKaplama
            this.colKaplama.HeaderText = "Kaplama";
            this.colKaplama.Name = "colKaplama";
            this.colKaplama.DataPropertyName = "colKaplama";
            // colKg
            this.colKg.HeaderText = "Kg";
            this.colKg.Name = "colKg";
            this.colKg.DataPropertyName = "colKg";
            // colTonbasi
            this.colTonbasi.HeaderText = "Tonbasi ($)";
            this.colTonbasi.Name = "colTonbasi";
            this.colTonbasi.DataPropertyName = "colTonbasi";
            // colTl
            this.colTl.HeaderText = "TL Karsilik";
            this.colTl.Name = "colTl";
            this.colTl.DataPropertyName = "colTl";
            // ==================== SAG ALT ====================
            this.pnlSagAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSagAlt.Size = new System.Drawing.Size(1250, 180);
            this.pnlSagAlt.BackColor = System.Drawing.Color.White;
            this.pnlSagAlt.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.pnlSagAlt.Controls.Add(this.pnlDetay);
            this.pnlSagAlt.Controls.Add(this.dgvGecmisTeklifler);
            this.pnlSagAlt.Controls.Add(this.lblGecmisBaslik);
            // lblGecmisBaslik
            this.lblGecmisBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGecmisBaslik.Size = new System.Drawing.Size(1230, 22);
            this.lblGecmisBaslik.Text = "Gecmis Teklifler";
            this.lblGecmisBaslik.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGecmisBaslik.ForeColor = System.Drawing.Color.FromArgb(30, 36, 40);
            // dgvGecmisTeklifler
            this.dgvGecmisTeklifler.Location = new System.Drawing.Point(10, 30);
            this.dgvGecmisTeklifler.Size = new System.Drawing.Size(400, 140);
            this.dgvGecmisTeklifler.ReadOnly = true;
            this.dgvGecmisTeklifler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGecmisTeklifler.AllowUserToAddRows = false;
            this.dgvGecmisTeklifler.RowHeadersVisible = false;
            this.dgvGecmisTeklifler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGecmisTeklifler.BackgroundColor = System.Drawing.Color.White;
            this.dgvGecmisTeklifler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGecmisTeklifler.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvGecmisTeklifler.Visible = false;
            this.dgvGecmisTeklifler.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGecmisTeklifler_CellDoubleClick);
            // pnlDetay
            this.pnlDetay.Location = new System.Drawing.Point(420, 30);
            this.pnlDetay.Size = new System.Drawing.Size(300, 140);
            this.pnlDetay.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.pnlDetay.Visible = false;
            this.pnlDetay.Padding = new System.Windows.Forms.Padding(10);
            this.pnlDetay.Controls.Add(this.lblDetay);
            this.pnlDetay.Controls.Add(this.lblDetayBaslik);
            // lblDetayBaslik
            this.lblDetayBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetayBaslik.Size = new System.Drawing.Size(280, 22);
            this.lblDetayBaslik.Text = "Teklif Detayi";
            this.lblDetayBaslik.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDetayBaslik.ForeColor = System.Drawing.Color.FromArgb(255, 140, 0);
            // lblDetay
            this.lblDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetay.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDetay.ForeColor = System.Drawing.Color.FromArgb(30, 36, 40);
            this.lblDetay.Text = "Gecmis Teklif Detayi";
            // ==================== ALT BUTONLAR ====================
            this.pnlAltButonlar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAltButonlar.Size = new System.Drawing.Size(1250, 50);
            this.pnlAltButonlar.BackColor = System.Drawing.Color.White;
            this.pnlAltButonlar.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.pnlAltButonlar.Controls.Add(this.btnTeklifiKaydet);
            this.pnlAltButonlar.Controls.Add(this.btnYeniTeklif);
            // btnYeniTeklif
            this.btnYeniTeklif.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnYeniTeklif.Size = new System.Drawing.Size(140, 40);
            this.btnYeniTeklif.Text = "Yeni Teklif";
            this.btnYeniTeklif.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnYeniTeklif.ForeColor = System.Drawing.Color.White;
            this.btnYeniTeklif.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYeniTeklif.FlatAppearance.BorderSize = 0;
            this.btnYeniTeklif.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnYeniTeklif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYeniTeklif.Click += new System.EventHandler(this.btnYeniTeklif_Click);
            // btnTeklifiKaydet
            this.btnTeklifiKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTeklifiKaydet.Size = new System.Drawing.Size(140, 40);
            this.btnTeklifiKaydet.Text = "Teklifi Kaydet";
            this.btnTeklifiKaydet.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnTeklifiKaydet.ForeColor = System.Drawing.Color.White;
            this.btnTeklifiKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeklifiKaydet.FlatAppearance.BorderSize = 0;
            this.btnTeklifiKaydet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTeklifiKaydet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTeklifiKaydet.Click += new System.EventHandler(this.btnTeklifiKaydet_Click);
            // ==================== FORM ====================
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 750);
            this.Controls.Add(this.pnlTeklif);
            this.Controls.Add(this.pnlParametreler);
            this.Controls.Add(this.pnlMusteriUst);
            this.Controls.Add(this.pnlUstBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BARAN CELIK & GALVANIZ - TEKLIF MODULU";
            this.Name = "galvaniz";
            this.pnlUstBar.ResumeLayout(false);
            this.pnlMusteriUst.ResumeLayout(false);
            this.pnlMusteriUst.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMusteriler)).EndInit();
            this.pnlParametreler.ResumeLayout(false);
            this.pnlParametreler.PerformLayout();
            this.pnlTeklif.ResumeLayout(false);
            this.pnlUrunGiris.ResumeLayout(false);
            this.pnlUrunGiris.PerformLayout();
            this.pnlUrunButonlar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUrunler)).EndInit();
            this.pnlSagAlt.ResumeLayout(false);
            this.pnlDetay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGecmisTeklifler)).EndInit();
            this.pnlAltButonlar.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel pnlUstBar;
        private System.Windows.Forms.Label lblFormBaslik;
        private System.Windows.Forms.Panel pnlMusteriUst;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridView dgvMusteriler;
        private System.Windows.Forms.Label lblMusteriAraLabel;
        private System.Windows.Forms.TextBox txtMusteriAra;
        private System.Windows.Forms.Button btnMusteriAra;
        private System.Windows.Forms.Button btnIleri;
        private System.Windows.Forms.Button btnGecmisTeklifler;
        private System.Windows.Forms.Panel pnlParametreler;
        private System.Windows.Forms.Label lblSecilenMusteri;
        private System.Windows.Forms.Label lblLME;
        private System.Windows.Forms.TextBox txtLME;
        private System.Windows.Forms.Label lblMaliyet;
        private System.Windows.Forms.TextBox txtMaliyet;
        private System.Windows.Forms.Label lblDolarKuru;
        private System.Windows.Forms.TextBox txtdolar;
        private System.Windows.Forms.Button btnTeklifKaydet;
        private System.Windows.Forms.Panel pnlTeklif;
        private System.Windows.Forms.Panel pnlUrunGiris;
        private System.Windows.Forms.Label lblUrunTuru;
        private System.Windows.Forms.ComboBox cmbKaplama;
        private System.Windows.Forms.Label lblKgLabel;
        private System.Windows.Forms.TextBox txtKg;
        private System.Windows.Forms.Button btnUrunEkle;
        private System.Windows.Forms.Panel pnlUrunButonlar;
        private System.Windows.Forms.Button btnUrunSil;
        private System.Windows.Forms.Button btnUrunKaydet;
        private System.Windows.Forms.Button btnPdf;
        private System.Windows.Forms.DataGridView dgvUrunler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKaplama;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTonbasi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTl;
        private System.Windows.Forms.Panel pnlSagAlt;
        private System.Windows.Forms.Label lblGecmisBaslik;
        private System.Windows.Forms.DataGridView dgvGecmisTeklifler;
        private System.Windows.Forms.Panel pnlDetay;
        private System.Windows.Forms.Label lblDetayBaslik;
        private System.Windows.Forms.Label lblDetay;
        private System.Windows.Forms.Panel pnlAltButonlar;
        private System.Windows.Forms.Button btnYeniTeklif;
        private System.Windows.Forms.Button btnTeklifiKaydet;
    }
}