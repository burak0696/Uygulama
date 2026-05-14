namespace BaranYardimci
{
    partial class Donusturucu
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
            this.lblBaslik = new System.Windows.Forms.Label();
            this.lblSaat = new System.Windows.Forms.Label();
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
            // Favoriler
            this.pnlFavoriler = new System.Windows.Forms.Panel();
            this.lblFavBaslik = new System.Windows.Forms.Label();
            this.lbFavoriler = new System.Windows.Forms.ListBox();
            this.pnlFavButonlar = new System.Windows.Forms.TableLayoutPanel();
            this.btnFavEkle = new System.Windows.Forms.Button();
            this.btnFavSil = new System.Windows.Forms.Button();
            this.lblFavAcikla = new System.Windows.Forms.Label();
            // Timer & ContextMenu
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
            this.pnlFavoriler.SuspendLayout();
            this.pnlFavButonlar.SuspendLayout();
            this.ctxDosya.SuspendLayout();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════════════
            //  timerSaat
            // ════════════════════════════════════════════════════════════
            this.timerSaat.Interval = 1000;
            this.timerSaat.Tick += new System.EventHandler(this.timerSaat_Tick);
            this.timerSaat.Start();

            // ════════════════════════════════════════════════════════════
            //  ctxDosya
            // ════════════════════════════════════════════════════════════
            this.ctxDosya.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ctxDosya.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuRotaGir, this.mnuSep1, this.mnuDosyaSil });

            this.mnuRotaGir.Text = "✏  Rota Gir  —  İmalat Kartlarını Bas";
            this.mnuRotaGir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.mnuRotaGir.Click += new System.EventHandler(this.mnuRotaGir_Click);

            this.mnuDosyaSil.Text = "🗑  Listeden Çıkar";
            this.mnuDosyaSil.ForeColor = System.Drawing.Color.FromArgb(180, 40, 40);
            this.mnuDosyaSil.Click += new System.EventHandler(this.mnuDosyaSil_Click);

            // ════════════════════════════════════════════════════════════
            //  pnlUst
            // ════════════════════════════════════════════════════════════
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(30, 30, 32);
            this.pnlUst.Controls.Add(this.lblSaat);
            this.pnlUst.Controls.Add(this.lblBaslik);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Size = new System.Drawing.Size(1920, 58);

            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Size = new System.Drawing.Size(620, 58);
            this.lblBaslik.Text = "    BARAN ÇELİK  —  Dönüştürücü";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblSaat.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSaat.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSaat.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.lblSaat.Size = new System.Drawing.Size(280, 58);
            this.lblSaat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSaat.Padding = new System.Windows.Forms.Padding(0, 0, 16, 0);

            // ════════════════════════════════════════════════════════════
            //  pnlFavoriler  — EN SAĞ
            // ════════════════════════════════════════════════════════════
            this.pnlFavoriler.BackColor = System.Drawing.Color.FromArgb(18, 18, 26);
            this.pnlFavoriler.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFavoriler.Size = new System.Drawing.Size(260, 1022);
            this.pnlFavoriler.Controls.Add(this.lbFavoriler);
            this.pnlFavoriler.Controls.Add(this.pnlFavButonlar);
            this.pnlFavoriler.Controls.Add(this.lblFavAcikla);
            this.pnlFavoriler.Controls.Add(this.lblFavBaslik);

            this.lblFavBaslik.Text = "⭐  SIKÇA KULLANILANLAR";
            this.lblFavBaslik.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFavBaslik.ForeColor = System.Drawing.Color.FromArgb(255, 210, 80);
            this.lblFavBaslik.BackColor = System.Drawing.Color.FromArgb(28, 28, 42);
            this.lblFavBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFavBaslik.Height = 34;
            this.lblFavBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lbFavoriler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFavoriler.BackColor = System.Drawing.Color.FromArgb(24, 24, 36);
            this.lbFavoriler.ForeColor = System.Drawing.Color.FromArgb(200, 215, 240);
            this.lbFavoriler.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lbFavoriler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbFavoriler.ItemHeight = 26;
            this.lbFavoriler.IntegralHeight = false;
            this.lbFavoriler.DoubleClick += new System.EventHandler(this.lbFavoriler_DoubleClick);

            this.pnlFavButonlar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFavButonlar.Height = 46;
            this.pnlFavButonlar.ColumnCount = 2;
            this.pnlFavButonlar.RowCount = 1;
            this.pnlFavButonlar.BackColor = System.Drawing.Color.FromArgb(18, 18, 26);
            this.pnlFavButonlar.Padding = new System.Windows.Forms.Padding(2);
            this.pnlFavButonlar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlFavButonlar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlFavButonlar.Controls.Add(this.btnFavEkle, 0, 0);
            this.pnlFavButonlar.Controls.Add(this.btnFavSil, 1, 0);

            BtnStyle(this.btnFavEkle, "➕ Ekle", System.Drawing.Color.FromArgb(34, 100, 55));
            this.btnFavEkle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFavEkle.Margin = new System.Windows.Forms.Padding(2);
            this.btnFavEkle.Click += new System.EventHandler(this.btnFavEkle_Click);

            BtnStyle(this.btnFavSil, "🗑 Sil", System.Drawing.Color.FromArgb(100, 28, 28));
            this.btnFavSil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFavSil.Margin = new System.Windows.Forms.Padding(2);
            this.btnFavSil.Click += new System.EventHandler(this.btnFavSil_Click);

            this.lblFavAcikla.Text = "2× tıkla → Direkt Excel'i aç";
            this.lblFavAcikla.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblFavAcikla.ForeColor = System.Drawing.Color.FromArgb(110, 120, 140);
            this.lblFavAcikla.BackColor = System.Drawing.Color.FromArgb(18, 18, 26);
            this.lblFavAcikla.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblFavAcikla.Height = 22;
            this.lblFavAcikla.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ════════════════════════════════════════════════════════════
            //  pnlSol
            // ════════════════════════════════════════════════════════════
            this.pnlSol.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlSol.Controls.Add(this.grpDosyalar);
            this.pnlSol.Controls.Add(this.pnlDosyaButonlar);
            this.pnlSol.Controls.Add(this.pnlDurumBar);
            this.pnlSol.Controls.Add(this.grpGecmis);
            this.pnlSol.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSol.Padding = new System.Windows.Forms.Padding(10, 10, 6, 10);
            this.pnlSol.Size = new System.Drawing.Size(560, 1022);

            // ── grpGecmis ────────────────────────────────────────────────
            this.grpGecmis.Controls.Add(this.lbGecmis);
            this.grpGecmis.Controls.Add(this.btnGecmisSil);
            this.grpGecmis.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpGecmis.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpGecmis.ForeColor = System.Drawing.Color.FromArgb(0, 100, 160);
            this.grpGecmis.Padding = new System.Windows.Forms.Padding(6, 4, 6, 6);
            this.grpGecmis.Size = new System.Drawing.Size(540, 210);
            this.grpGecmis.TabStop = false;
            this.grpGecmis.Text = "Son ERP Aktarımları  (2× tıkla → Excel'i aç)";

            this.lbGecmis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbGecmis.Font = new System.Drawing.Font("Consolas", 9F);
            this.lbGecmis.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbGecmis.BackColor = System.Drawing.Color.FromArgb(250, 252, 255);
            this.lbGecmis.ItemHeight = 20;
            this.lbGecmis.DoubleClick += new System.EventHandler(this.lbGecmis_DoubleClick);

            this.btnGecmisSil.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGecmisSil.Text = "Geçmişi Temizle";
            this.btnGecmisSil.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGecmisSil.BackColor = System.Drawing.Color.FromArgb(180, 60, 60);
            this.btnGecmisSil.ForeColor = System.Drawing.Color.White;
            this.btnGecmisSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGecmisSil.FlatAppearance.BorderSize = 0;
            this.btnGecmisSil.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGecmisSil.Size = new System.Drawing.Size(528, 30);
            this.btnGecmisSil.Click += new System.EventHandler(this.btnGecmisSil_Click);

            // ── pnlDurumBar ────────────��─────────────────────────────────
            this.pnlDurumBar.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
            this.pnlDurumBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDurumBar.Controls.Add(this.lblDurumMetin);
            this.pnlDurumBar.Controls.Add(this.lblDurumIkon);
            this.pnlDurumBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDurumBar.Size = new System.Drawing.Size(540, 44);
            this.pnlDurumBar.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);

            this.lblDurumIkon.AutoSize = false;
            this.lblDurumIkon.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDurumIkon.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblDurumIkon.Size = new System.Drawing.Size(36, 34);
            this.lblDurumIkon.Text = "📋";
            this.lblDurumIkon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblDurumMetin.AutoSize = false;
            this.lblDurumMetin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDurumMetin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDurumMetin.ForeColor = System.Drawing.Color.FromArgb(40, 80, 140);
            this.lblDurumMetin.Text = "Dosya yüklenmedi";
            this.lblDurumMetin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── grpDosyalar ───────────────────────────────────────────────
            this.grpDosyalar.Controls.Add(this.dgvDosyalar);
            this.grpDosyalar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDosyalar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpDosyalar.ForeColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.grpDosyalar.Padding = new System.Windows.Forms.Padding(6);
            this.grpDosyalar.TabStop = false;
            this.grpDosyalar.Text = "Yüklenen Dosyalar  (sağ klik → işlemler)";

            // ── dgvDosyalar ───────────────────────────────────────────────
            this.dgvDosyalar.AllowUserToAddRows = false;
            this.dgvDosyalar.AllowUserToDeleteRows = false;
            this.dgvDosyalar.AllowUserToResizeRows = false;
            this.dgvDosyalar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDosyalar.BackgroundColor = System.Drawing.Color.White;
            this.dgvDosyalar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDosyalar.ColumnHeadersHeight = 36;
            this.dgvDosyalar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDosyalar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colDosyaAdi, this.colSiparisAdeti, this.colDurum, this.colDosyaYolu });
            this.dgvDosyalar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDosyalar.RowHeadersVisible = false;
            this.dgvDosyalar.RowTemplate.Height = 32;
            this.dgvDosyalar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDosyalar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvDosyalar.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDosyalar_CellFormatting);
            this.dgvDosyalar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvDosyalar_MouseUp);

            this.colDosyaAdi.FillWeight = 45F; this.colDosyaAdi.HeaderText = "Dosya Adı"; this.colDosyaAdi.Name = "colDosyaAdi"; this.colDosyaAdi.ReadOnly = true;
            this.colSiparisAdeti.FillWeight = 13F; this.colSiparisAdeti.HeaderText = "Sipariş"; this.colSiparisAdeti.Name = "colSiparisAdeti";
            this.colDurum.FillWeight = 42F; this.colDurum.HeaderText = "Durum"; this.colDurum.Name = "colDurum"; this.colDurum.ReadOnly = true;
            this.colDosyaYolu.HeaderText = "Yol"; this.colDosyaYolu.Name = "colDosyaYolu"; this.colDosyaYolu.Visible = false;

            // ── pnlDosyaButonlar ──────────────────────────────────────────
            this.pnlDosyaButonlar.Controls.Add(this.btnListeTemizle);
            this.pnlDosyaButonlar.Controls.Add(this.btnMiktarKaydet);
            this.pnlDosyaButonlar.Controls.Add(this.btnDosyaEkle);
            this.pnlDosyaButonlar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDosyaButonlar.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlDosyaButonlar.Size = new System.Drawing.Size(540, 56);

            BtnStyle(this.btnDosyaEkle, "Dosya Ekle", System.Drawing.Color.FromArgb(0, 122, 204));
            BtnStyle(this.btnMiktarKaydet, "Miktar Kaydet", System.Drawing.Color.FromArgb(46, 139, 87));
            BtnStyle(this.btnListeTemizle, "Listeyi Temizle", System.Drawing.Color.FromArgb(200, 60, 60));
            this.btnDosyaEkle.Location = new System.Drawing.Point(0, 6);
            this.btnMiktarKaydet.Location = new System.Drawing.Point(184, 6);
            this.btnListeTemizle.Location = new System.Drawing.Point(368, 6);
            this.btnDosyaEkle.Click += new System.EventHandler(this.btnDosyaEkle_Click);
            this.btnMiktarKaydet.Click += new System.EventHandler(this.btnMiktarKaydet_Click);
            this.btnListeTemizle.Click += new System.EventHandler(this.btnListeTemizle_Click);

            // ════════════════════════════════════════════════════════════
            //  splitter
            // ════════════════════════════════════════════════════════════
            this.splitter.BackColor = System.Drawing.Color.FromArgb(210, 210, 215);
            this.splitter.Size = new System.Drawing.Size(5, 1022);
            this.splitter.TabStop = false;

            // ════════════════════════════════════════════════════════════
            //  pnlSag  (Fill)
            // ════════════════════════════════════════════════════════════
            this.pnlSag.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlSag.Controls.Add(this.grpSonuc);
            this.pnlSag.Controls.Add(this.pnlSonucButonlar);
            this.pnlSag.Controls.Add(this.pnlSonucButonlar2);
            this.pnlSag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSag.Padding = new System.Windows.Forms.Padding(6, 10, 10, 10);

            // ── grpSonuc ─────────────────────────────────────────────────
            this.grpSonuc.Controls.Add(this.dgvSonuc);
            this.grpSonuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSonuc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSonuc.ForeColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.grpSonuc.Padding = new System.Windows.Forms.Padding(6);
            this.grpSonuc.TabStop = false;
            this.grpSonuc.Text = "Malzeme Özet Sonuçları";

            // ── dgvSonuc ─────────────────────────────────────────────────
            this.dgvSonuc.AllowUserToAddRows = false;
            this.dgvSonuc.AllowUserToDeleteRows = false;
            this.dgvSonuc.AllowUserToResizeRows = false;
            this.dgvSonuc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSonuc.BackgroundColor = System.Drawing.Color.White;
            this.dgvSonuc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSonuc.ColumnHeadersHeight = 36;
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
            this.pnlSonucButonlar.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlSonucButonlar.Size = new System.Drawing.Size(1095, 56);

            BtnStyle(this.btnHesapla, "Hesapla", System.Drawing.Color.FromArgb(0, 122, 204));
            BtnStyle(this.btnHamDataExcel, "Ham Data - Excel", System.Drawing.Color.FromArgb(34, 139, 34));
            BtnStyle(this.btnMalzemeExcel, "Malzeme İhtiyacı", System.Drawing.Color.FromArgb(178, 34, 34));
            BtnStyle(this.btnErpAktarim, "ERP Aktarım  →  Ağa Kaydet", System.Drawing.Color.FromArgb(0, 130, 120));
            this.btnErpAktarim.Size = new System.Drawing.Size(280, 48);
            this.btnHesapla.Location = new System.Drawing.Point(0, 6);
            this.btnHamDataExcel.Location = new System.Drawing.Point(210, 6); this.btnHamDataExcel.Visible = false;
            this.btnMalzemeExcel.Location = new System.Drawing.Point(210, 6);
            this.btnErpAktarim.Location = new System.Drawing.Point(420, 6);
            this.btnHesapla.Click += new System.EventHandler(this.btnHesapla_Click);
            this.btnHamDataExcel.Click += new System.EventHandler(this.btnHamDataExcel_Click);
            this.btnMalzemeExcel.Click += new System.EventHandler(this.btnMalzemeExcel_Click);
            this.btnErpAktarim.Click += new System.EventHandler(this.btnErpAktarim_Click);

            // ── pnlSonucButonlar2 ─────────────────────────────────────────
            this.pnlSonucButonlar2.Controls.Add(this.btnGalvanizEkran);
            this.pnlSonucButonlar2.Controls.Add(this.btnVeritabaniKaydet);
            this.pnlSonucButonlar2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSonucButonlar2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pnlSonucButonlar2.Size = new System.Drawing.Size(1095, 56);

            BtnStyle(this.btnVeritabaniKaydet, "Veritabanına Aktar", System.Drawing.Color.FromArgb(75, 0, 130));
            BtnStyle(this.btnGalvanizEkran, "Galvaniz Ekranı", System.Drawing.Color.FromArgb(200, 120, 0));
            this.btnVeritabaniKaydet.Location = new System.Drawing.Point(0, 6); this.btnVeritabaniKaydet.Visible = false;
            this.btnGalvanizEkran.Location = new System.Drawing.Point(220, 6); this.btnGalvanizEkran.Visible = false;
            this.btnVeritabaniKaydet.Click += new System.EventHandler(this.btnVeritabaniKaydet_Click);
            this.btnGalvanizEkran.Click += new System.EventHandler(this.btnGalvanizEkran_Click);

            // ════════════════════════════════════════════════════════════
            //  FORM
            //  Controls.Add sırası dock önceliğini belirler:
            //  Fill en önce, sonra Right'lar (en son eklenen en sağa gider),
            //  sonra Left, en son Top
            // ════════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(238, 238, 242);
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.pnlSag);        // Fill
            this.Controls.Add(this.splitter);       // Left splitter
            this.Controls.Add(this.pnlSol);         // Left
            this.Controls.Add(this.pnlFavoriler);   // Right (en sağ)
            this.Controls.Add(this.pnlUst);         // Top
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
            this.pnlFavoriler.ResumeLayout(false);
            this.pnlFavButonlar.ResumeLayout(false);
            this.ctxDosya.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void BtnStyle(System.Windows.Forms.Button b, string text, System.Drawing.Color color)
        {
            b.Text = text;
            b.BackColor = color;
            b.ForeColor = System.Drawing.Color.White;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            b.Cursor = System.Windows.Forms.Cursors.Hand;
            b.Size = new System.Drawing.Size(205, 48);
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
        private System.Windows.Forms.Panel pnlFavoriler;
        private System.Windows.Forms.Label lblFavBaslik;
        private System.Windows.Forms.ListBox lbFavoriler;
        private System.Windows.Forms.TableLayoutPanel pnlFavButonlar;
        private System.Windows.Forms.Button btnFavEkle;
        private System.Windows.Forms.Button btnFavSil;
        private System.Windows.Forms.Label lblFavAcikla;
        private System.Windows.Forms.Timer timerSaat;
        private System.Windows.Forms.ContextMenuStrip ctxDosya;
        private System.Windows.Forms.ToolStripMenuItem mnuRotaGir;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuDosyaSil;
    }
}