namespace BaranYardimci
{
    partial class Donusturucu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlUst = new System.Windows.Forms.Panel();
            this.lblSaat = new System.Windows.Forms.Label();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.pnlSol = new System.Windows.Forms.Panel();
            this.grpDosyalar = new System.Windows.Forms.GroupBox();
            this.dgvDosyalar = new System.Windows.Forms.DataGridView();
            this.colDosyaAdi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSiparisAdeti = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDurum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDosyaYolu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDosyaButonlar = new System.Windows.Forms.Panel();
            this.btnListeTemizle = new System.Windows.Forms.Button();
            this.btnMiktarKaydet = new System.Windows.Forms.Button();
            this.btnDosyaEkle = new System.Windows.Forms.Button();
            this.pnlDurumBar = new System.Windows.Forms.Panel();
            this.lblDurumIkon = new System.Windows.Forms.Label();
            this.lblDurumMetin = new System.Windows.Forms.Label();
            this.grpGecmis = new System.Windows.Forms.GroupBox();
            this.lbGecmis = new System.Windows.Forms.ListBox();
            this.btnGecmisSil = new System.Windows.Forms.Button();
            this.splitter = new System.Windows.Forms.Splitter();
            this.pnlSag = new System.Windows.Forms.Panel();
            this.grpSonuc = new System.Windows.Forms.GroupBox();
            this.dgvSonuc = new System.Windows.Forms.DataGridView();
            this.colProfil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKalite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToplamAdet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToplamUzunluk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToplamAgirlik = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSonucButonlar = new System.Windows.Forms.Panel();
            this.btnErpAktarim = new System.Windows.Forms.Button();
            this.btnMalzemeExcel = new System.Windows.Forms.Button();
            this.btnHamDataExcel = new System.Windows.Forms.Button();
            this.btnHesapla = new System.Windows.Forms.Button();
            this.pnlSonucButonlar2 = new System.Windows.Forms.Panel();
            this.btnGalvanizEkran = new System.Windows.Forms.Button();
            this.btnVeritabaniKaydet = new System.Windows.Forms.Button();
            this.timerSaat = new System.Windows.Forms.Timer();
            this.ctxDosya = new System.Windows.Forms.ContextMenuStrip();
            this.mnuRotaGir = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDosyaSil = new System.Windows.Forms.ToolStripMenuItem();

            this.pnlUst.SuspendLayout();
            this.pnlSol.SuspendLayout();
            this.grpDosyalar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDosyalar)).BeginInit();
            this.pnlDosyaButonlar.SuspendLayout();
            this.pnlDurumBar.SuspendLayout();
            this.grpGecmis.SuspendLayout();
            this.pnlSag.SuspendLayout();
            this.grpSonuc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSonuc)).BeginInit();
            this.pnlSonucButonlar.SuspendLayout();
            this.pnlSonucButonlar2.SuspendLayout();
            this.ctxDosya.SuspendLayout();
            this.SuspendLayout();

            // ── timerSaat ────────────────────────────────────────────────
            this.timerSaat.Interval = 1000;
            this.timerSaat.Tick += new System.EventHandler(this.timerSaat_Tick);
            this.timerSaat.Start();

            // ── pnlUst ───────────────────────────────────────────────────
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(30, 30, 32);
            this.pnlUst.Controls.Add(this.lblSaat);
            this.pnlUst.Controls.Add(this.lblBaslik);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Size = new System.Drawing.Size(1920, 64);
            this.pnlUst.TabIndex = 3;

            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Size = new System.Drawing.Size(620, 64);
            this.lblBaslik.Text = "    BARAN ÇELİK  —  Dönüştürücü";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblSaat.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSaat.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblSaat.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.lblSaat.Size = new System.Drawing.Size(300, 64);
            this.lblSaat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSaat.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);

            // ── ctxDosya (sağ klik menüsü) ───────────────────────────────
            this.ctxDosya.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ctxDosya.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuRotaGir, this.mnuSep1, this.mnuDosyaSil });

            this.mnuRotaGir.Text = "✏  Rota Gir  —  İmalat Kartlarını Bas";
            this.mnuRotaGir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.mnuRotaGir.Click += new System.EventHandler(this.mnuRotaGir_Click);

            this.mnuDosyaSil.Text = "🗑  Listeden Çıkar";
            this.mnuDosyaSil.ForeColor = System.Drawing.Color.FromArgb(180, 40, 40);
            this.mnuDosyaSil.Click += new System.EventHandler(this.mnuDosyaSil_Click);

            // ── pnlSol ───────────────────────────────────────────────────
            this.pnlSol.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlSol.Controls.Add(this.grpDosyalar);
            this.pnlSol.Controls.Add(this.pnlDosyaButonlar);
            this.pnlSol.Controls.Add(this.pnlDurumBar);
            this.pnlSol.Controls.Add(this.grpGecmis);
            this.pnlSol.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSol.Padding = new System.Windows.Forms.Padding(12, 12, 6, 12);
            this.pnlSol.Size = new System.Drawing.Size(580, 1016);
            this.pnlSol.TabIndex = 2;

            // ── grpGecmis ────────────────────────────────────────────────
            this.grpGecmis.Controls.Add(this.lbGecmis);
            this.grpGecmis.Controls.Add(this.btnGecmisSil);
            this.grpGecmis.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpGecmis.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpGecmis.ForeColor = System.Drawing.Color.FromArgb(0, 100, 160);
            this.grpGecmis.Padding = new System.Windows.Forms.Padding(8, 4, 8, 6);
            this.grpGecmis.Size = new System.Drawing.Size(556, 220);
            this.grpGecmis.TabIndex = 3;
            this.grpGecmis.TabStop = false;
            this.grpGecmis.Text = "Son ERP Aktarımları";

            this.lbGecmis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbGecmis.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.lbGecmis.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbGecmis.BackColor = System.Drawing.Color.FromArgb(250, 252, 255);
            this.lbGecmis.ItemHeight = 20;
            this.lbGecmis.TabIndex = 0;

            this.btnGecmisSil.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGecmisSil.Text = "Geçmişi Temizle";
            this.btnGecmisSil.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGecmisSil.BackColor = System.Drawing.Color.FromArgb(180, 60, 60);
            this.btnGecmisSil.ForeColor = System.Drawing.Color.White;
            this.btnGecmisSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGecmisSil.FlatAppearance.BorderSize = 0;
            this.btnGecmisSil.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGecmisSil.Size = new System.Drawing.Size(540, 30);
            this.btnGecmisSil.TabIndex = 1;
            this.btnGecmisSil.Click += new System.EventHandler(this.btnGecmisSil_Click);

            // ── pnlDurumBar ──────────────────────────────────────────────
            this.pnlDurumBar.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.pnlDurumBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDurumBar.Controls.Add(this.lblDurumMetin);
            this.pnlDurumBar.Controls.Add(this.lblDurumIkon);
            this.pnlDurumBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDurumBar.Size = new System.Drawing.Size(556, 42);
            this.pnlDurumBar.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.pnlDurumBar.TabIndex = 2;

            this.lblDurumIkon.AutoSize = false;
            this.lblDurumIkon.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDurumIkon.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblDurumIkon.Size = new System.Drawing.Size(36, 32);
            this.lblDurumIkon.Text = "📋";
            this.lblDurumIkon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblDurumMetin.AutoSize = false;
            this.lblDurumMetin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDurumMetin.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDurumMetin.ForeColor = System.Drawing.Color.FromArgb(40, 80, 140);
            this.lblDurumMetin.Text = "Dosya yüklenmedi";
            this.lblDurumMetin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── grpDosyalar ───────────────────────────────────────────────
            this.grpDosyalar.Controls.Add(this.dgvDosyalar);
            this.grpDosyalar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDosyalar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpDosyalar.ForeColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.grpDosyalar.Padding = new System.Windows.Forms.Padding(8);
            this.grpDosyalar.TabIndex = 0;
            this.grpDosyalar.TabStop = false;
            this.grpDosyalar.Text = "Yüklenen Dosyalar  (sağ klik → işlemler)";

            // ── dgvDosyalar ───────────────────────────────────────────────
            this.dgvDosyalar.AllowUserToAddRows = false;
            this.dgvDosyalar.AllowUserToDeleteRows = false;
            this.dgvDosyalar.AllowUserToResizeRows = false;
            this.dgvDosyalar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDosyalar.BackgroundColor = System.Drawing.Color.White;
            this.dgvDosyalar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDosyalar.ColumnHeadersHeight = 38;
            this.dgvDosyalar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDosyalar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colDosyaAdi, this.colSiparisAdeti, this.colDurum, this.colDosyaYolu });
            this.dgvDosyalar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDosyalar.RowHeadersVisible = false;
            this.dgvDosyalar.RowTemplate.Height = 34;
            this.dgvDosyalar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDosyalar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvDosyalar.TabIndex = 0;
            this.dgvDosyalar.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDosyalar_CellFormatting);
            this.dgvDosyalar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvDosyalar_MouseUp);

            this.colDosyaAdi.FillWeight = 52F;
            this.colDosyaAdi.HeaderText = "Dosya Adı";
            this.colDosyaAdi.Name = "colDosyaAdi";
            this.colDosyaAdi.ReadOnly = true;

            this.colSiparisAdeti.FillWeight = 14F;
            this.colSiparisAdeti.HeaderText = "Sipariş";
            this.colSiparisAdeti.Name = "colSiparisAdeti";

            this.colDurum.FillWeight = 34F;
            this.colDurum.HeaderText = "Durum";
            this.colDurum.Name = "colDurum";
            this.colDurum.ReadOnly = true;

            this.colDosyaYolu.HeaderText = "Yol";
            this.colDosyaYolu.Name = "colDosyaYolu";
            this.colDosyaYolu.Visible = false;

            // ── pnlDosyaButonlar ──────────────────────────────────────────
            this.pnlDosyaButonlar.Controls.Add(this.btnListeTemizle);
            this.pnlDosyaButonlar.Controls.Add(this.btnMiktarKaydet);
            this.pnlDosyaButonlar.Controls.Add(this.btnDosyaEkle);
            this.pnlDosyaButonlar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDosyaButonlar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlDosyaButonlar.Size = new System.Drawing.Size(556, 58);
            this.pnlDosyaButonlar.TabIndex = 1;

            BtnSol(this.btnDosyaEkle, "Dosya Ekle", System.Drawing.Color.FromArgb(0, 122, 204));
            BtnSol(this.btnMiktarKaydet, "Miktar Kaydet", System.Drawing.Color.FromArgb(46, 139, 87));
            BtnSol(this.btnListeTemizle, "Listeyi Temizle", System.Drawing.Color.FromArgb(200, 60, 60));
            this.btnDosyaEkle.Location = new System.Drawing.Point(0, 8);
            this.btnMiktarKaydet.Location = new System.Drawing.Point(186, 8);
            this.btnListeTemizle.Location = new System.Drawing.Point(372, 8);
            this.btnDosyaEkle.Click += new System.EventHandler(this.btnDosyaEkle_Click);
            this.btnMiktarKaydet.Click += new System.EventHandler(this.btnMiktarKaydet_Click);
            this.btnListeTemizle.Click += new System.EventHandler(this.btnListeTemizle_Click);

            // ── splitter ─────────────────────────────────────────────────
            this.splitter.BackColor = System.Drawing.Color.FromArgb(210, 210, 215);
            this.splitter.Size = new System.Drawing.Size(6, 1016);
            this.splitter.TabIndex = 1;
            this.splitter.TabStop = false;

            // ── pnlSag ───────────────────────────────────────────────────
            this.pnlSag.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlSag.Controls.Add(this.grpSonuc);
            this.pnlSag.Controls.Add(this.pnlSonucButonlar);
            this.pnlSag.Controls.Add(this.pnlSonucButonlar2);
            this.pnlSag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSag.Padding = new System.Windows.Forms.Padding(6, 12, 12, 12);
            this.pnlSag.TabIndex = 0;

            // ── grpSonuc ─────────────────────────────────────────────────
            this.grpSonuc.Controls.Add(this.dgvSonuc);
            this.grpSonuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSonuc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSonuc.ForeColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.grpSonuc.Padding = new System.Windows.Forms.Padding(8);
            this.grpSonuc.TabIndex = 0;
            this.grpSonuc.TabStop = false;
            this.grpSonuc.Text = "Malzeme Özet Sonuçları";

            // ── dgvSonuc ─────────────────────────────────────────────────
            this.dgvSonuc.AllowUserToAddRows = false;
            this.dgvSonuc.AllowUserToDeleteRows = false;
            this.dgvSonuc.AllowUserToResizeRows = false;
            this.dgvSonuc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSonuc.BackgroundColor = System.Drawing.Color.White;
            this.dgvSonuc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSonuc.ColumnHeadersHeight = 38;
            this.dgvSonuc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSonuc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colProfil, this.colKalite, this.colToplamAdet,
                this.colToplamUzunluk, this.colToplamAgirlik });
            this.dgvSonuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSonuc.ReadOnly = true;
            this.dgvSonuc.RowHeadersVisible = false;
            this.dgvSonuc.RowTemplate.Height = 32;
            this.dgvSonuc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSonuc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvSonuc.TabIndex = 0;
            this.dgvSonuc.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSonuc_CellContentClick);

            this.colProfil.FillWeight = 25F; this.colProfil.HeaderText = "Profil"; this.colProfil.Name = "colProfil"; this.colProfil.ReadOnly = true;
            this.colKalite.FillWeight = 15F; this.colKalite.HeaderText = "Kalite"; this.colKalite.Name = "colKalite"; this.colKalite.ReadOnly = true;
            this.colToplamAdet.FillWeight = 20F; this.colToplamAdet.HeaderText = "Toplam Adet"; this.colToplamAdet.Name = "colToplamAdet"; this.colToplamAdet.ReadOnly = true;
            this.colToplamUzunluk.FillWeight = 20F; this.colToplamUzunluk.HeaderText = "Toplam Uzunluk (mm)"; this.colToplamUzunluk.Name = "colToplamUzunluk"; this.colToplamUzunluk.ReadOnly = true;
            this.colToplamAgirlik.FillWeight = 20F; this.colToplamAgirlik.HeaderText = "Toplam Ağırlık (kg)"; this.colToplamAgirlik.Name = "colToplamAgirlik"; this.colToplamAgirlik.ReadOnly = true;

            // ── pnlSonucButonlar ──────────────────────────────────────────
            this.pnlSonucButonlar.Controls.Add(this.btnErpAktarim);
            this.pnlSonucButonlar.Controls.Add(this.btnMalzemeExcel);
            this.pnlSonucButonlar.Controls.Add(this.btnHamDataExcel);
            this.pnlSonucButonlar.Controls.Add(this.btnHesapla);
            this.pnlSonucButonlar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSonucButonlar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlSonucButonlar.Size = new System.Drawing.Size(1300, 58);
            this.pnlSonucButonlar.TabIndex = 1;

            BtnSag(this.btnHesapla, "Hesapla", System.Drawing.Color.FromArgb(0, 122, 204));
            BtnSag(this.btnHamDataExcel, "Ham Data - Excel", System.Drawing.Color.FromArgb(34, 139, 34));
            BtnSag(this.btnMalzemeExcel, "Malzeme İhtiyacı", System.Drawing.Color.FromArgb(178, 34, 34));
            BtnSag(this.btnErpAktarim, "ERP Aktarım  →  Ağa Kaydet", System.Drawing.Color.FromArgb(0, 130, 120));
            this.btnErpAktarim.Size = new System.Drawing.Size(280, 48);
            this.btnHesapla.Location = new System.Drawing.Point(0, 8);
            this.btnHamDataExcel.Location = new System.Drawing.Point(210, 8); this.btnHamDataExcel.Visible = false;
            this.btnMalzemeExcel.Location = new System.Drawing.Point(210, 8);
            this.btnErpAktarim.Location = new System.Drawing.Point(420, 8);
            this.btnHesapla.Click += new System.EventHandler(this.btnHesapla_Click);
            this.btnHamDataExcel.Click += new System.EventHandler(this.btnHamDataExcel_Click);
            this.btnMalzemeExcel.Click += new System.EventHandler(this.btnMalzemeExcel_Click);
            this.btnErpAktarim.Click += new System.EventHandler(this.btnErpAktarim_Click);

            // ── pnlSonucButonlar2 ─────────────────────────────────────────
            this.pnlSonucButonlar2.Controls.Add(this.btnGalvanizEkran);
            this.pnlSonucButonlar2.Controls.Add(this.btnVeritabaniKaydet);
            this.pnlSonucButonlar2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSonucButonlar2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlSonucButonlar2.Size = new System.Drawing.Size(1300, 58);
            this.pnlSonucButonlar2.TabIndex = 2;

            BtnSag(this.btnVeritabaniKaydet, "Veritabanına Aktar", System.Drawing.Color.FromArgb(75, 0, 130));
            BtnSag(this.btnGalvanizEkran, "Galvaniz Ekranı", System.Drawing.Color.FromArgb(200, 120, 0));
            this.btnVeritabaniKaydet.Location = new System.Drawing.Point(0, 8); this.btnVeritabaniKaydet.Visible = false;
            this.btnGalvanizEkran.Location = new System.Drawing.Point(220, 8); this.btnGalvanizEkran.Visible = false;
            this.btnVeritabaniKaydet.Click += new System.EventHandler(this.btnVeritabaniKaydet_Click);
            this.btnGalvanizEkran.Click += new System.EventHandler(this.btnGalvanizEkran_Click);

            // ── Form ─────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(238, 238, 242);
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.pnlSag);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.pnlSol);
            this.Controls.Add(this.pnlUst);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1280, 800);
            this.Name = "Donusturucu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Baran Yardımcı";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Donusturucu_Load);

            this.pnlUst.ResumeLayout(false);
            this.pnlSol.ResumeLayout(false);
            this.grpDosyalar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDosyalar)).EndInit();
            this.pnlDosyaButonlar.ResumeLayout(false);
            this.pnlDurumBar.ResumeLayout(false);
            this.grpGecmis.ResumeLayout(false);
            this.pnlSag.ResumeLayout(false);
            this.grpSonuc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSonuc)).EndInit();
            this.pnlSonucButonlar.ResumeLayout(false);
            this.pnlSonucButonlar2.ResumeLayout(false);
            this.ctxDosya.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // ── Buton yardımcıları ────────────────────────────────────────────
        private void BtnSol(System.Windows.Forms.Button b, string text, System.Drawing.Color color)
        {
            b.BackColor = color; b.ForeColor = System.Drawing.Color.White;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            b.Cursor = System.Windows.Forms.Cursors.Hand;
            b.Size = new System.Drawing.Size(182, 48);
            b.Text = text;
            b.UseVisualStyleBackColor = false;
        }

        private void BtnSag(System.Windows.Forms.Button b, string text, System.Drawing.Color color)
        {
            b.BackColor = color; b.ForeColor = System.Drawing.Color.White;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            b.Cursor = System.Windows.Forms.Cursors.Hand;
            b.Size = new System.Drawing.Size(200, 48);
            b.Text = text;
            b.UseVisualStyleBackColor = false;
        }

        // ── Fields ───────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlUst;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Label lblSaat;
        private System.Windows.Forms.Panel pnlSol;
        private System.Windows.Forms.GroupBox grpDosyalar;
        private System.Windows.Forms.DataGridView dgvDosyalar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDosyaAdi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSiparisAdeti;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDurum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDosyaYolu;
        private System.Windows.Forms.Panel pnlDosyaButonlar;
        private System.Windows.Forms.Button btnDosyaEkle;
        private System.Windows.Forms.Button btnMiktarKaydet;
        private System.Windows.Forms.Button btnListeTemizle;
        private System.Windows.Forms.Panel pnlDurumBar;
        private System.Windows.Forms.Label lblDurumIkon;
        private System.Windows.Forms.Label lblDurumMetin;
        private System.Windows.Forms.GroupBox grpGecmis;
        private System.Windows.Forms.ListBox lbGecmis;
        private System.Windows.Forms.Button btnGecmisSil;
        private System.Windows.Forms.Splitter splitter;
        private System.Windows.Forms.Panel pnlSag;
        private System.Windows.Forms.GroupBox grpSonuc;
        private System.Windows.Forms.DataGridView dgvSonuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfil;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKalite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToplamAdet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToplamUzunluk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToplamAgirlik;
        private System.Windows.Forms.Panel pnlSonucButonlar;
        private System.Windows.Forms.Button btnHesapla;
        private System.Windows.Forms.Button btnHamDataExcel;
        private System.Windows.Forms.Button btnMalzemeExcel;
        private System.Windows.Forms.Button btnErpAktarim;
        private System.Windows.Forms.Panel pnlSonucButonlar2;
        private System.Windows.Forms.Button btnVeritabaniKaydet;
        private System.Windows.Forms.Button btnGalvanizEkran;
        private System.Windows.Forms.Timer timerSaat;
        private System.Windows.Forms.ContextMenuStrip ctxDosya;
        private System.Windows.Forms.ToolStripMenuItem mnuRotaGir;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuDosyaSil;
    }
}