namespace BaranYardimci
{
    partial class Sarf
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Stil tanımları ────────────────────────────────────────────
            var headerStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(31, 41, 55),
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold),
                SelectionBackColor = System.Drawing.Color.FromArgb(31, 41, 55),
                SelectionForeColor = System.Drawing.Color.White,
                Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            };
            var rowStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                SelectionBackColor = System.Drawing.Color.FromArgb(229, 231, 235),
                SelectionForeColor = System.Drawing.Color.Black,
                Padding = new System.Windows.Forms.Padding(5)
            };

            // ── Chart alanları ────────────────────────────────────────────
            var chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            var legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            var legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            var series2 = new System.Windows.Forms.DataVisualization.Charting.Series();

            // ── Ana yapılar ──────────────────────────────────────���────────
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.layoutMenu = new System.Windows.Forms.TableLayoutPanel();
            this.pnlUst = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tabControlAna = new System.Windows.Forms.TabControl();
            this.pnlPopup = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblKurBilgi = new System.Windows.Forms.ToolStripStatusLabel();

            // ── Menü bileşenleri ──────────────────────────────────────────
            this.lblLogo = new System.Windows.Forms.Label();
            this.btnMenuDash = new System.Windows.Forms.Button();
            this.btnMenuTalep = new System.Windows.Forms.Button();
            this.btnMenuTeklif = new System.Windows.Forms.Button();
            this.btnMenuOnay = new System.Windows.Forms.Button();
            this.btnMenuStok = new System.Windows.Forms.Button();
            this.btnMenuDusum = new System.Windows.Forms.Button();
            this.btnMenuTedarikci = new System.Windows.Forms.Button();
            this.btnMenuKategori = new System.Windows.Forms.Button();
            this.btnMenuLog = new System.Windows.Forms.Button();
            this.btnMenuRapor = new System.Windows.Forms.Button();
            this.btnMenuCikis = new System.Windows.Forms.Button();

            // ── Üst panel ────────────────────────────────────────────────
            this.lblBaslik = new System.Windows.Forms.Label();

            // ── Tablar ───────────────────────────────────────────────────
            this.tabDash = new System.Windows.Forms.TabPage();
            this.tabTalep = new System.Windows.Forms.TabPage();
            this.tabTeklif = new System.Windows.Forms.TabPage();
            this.tabOnay = new System.Windows.Forms.TabPage();
            this.tabStok = new System.Windows.Forms.TabPage();
            this.tabDusum = new System.Windows.Forms.TabPage();
            this.tabTedarikci = new System.Windows.Forms.TabPage();
            this.tabKategori = new System.Windows.Forms.TabPage();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.tabRapor = new System.Windows.Forms.TabPage();

            // ── Dashboard ────────────────────────────────────────────────
            this.pnlDashUst = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDashCard1 = new System.Windows.Forms.Panel();
            this.lblDash1Title = new System.Windows.Forms.Label();
            this.lblDash1Val = new System.Windows.Forms.Label();
            this.pnlDashCard2 = new System.Windows.Forms.Panel();
            this.lblDash2Title = new System.Windows.Forms.Label();
            this.lblDash2Val = new System.Windows.Forms.Label();
            this.pnlDashCard3 = new System.Windows.Forms.Panel();
            this.lblDash3Title = new System.Windows.Forms.Label();
            this.lblDash3Val = new System.Windows.Forms.Label();
            this.pnlDashCard4 = new System.Windows.Forms.Panel();
            this.lblDash4Title = new System.Windows.Forms.Label();
            this.lblDash4Val = new System.Windows.Forms.Label();
            this.chartStok = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartHarcama = new System.Windows.Forms.DataVisualization.Charting.Chart();

            // ── Talep ─────────────────────────────────────────────────────
            this.grpUrunSecim = new System.Windows.Forms.GroupBox();
            this.lblAraBaslik = new System.Windows.Forms.Label();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.btnUrunBul = new System.Windows.Forms.Button();
            this.lblUrunBilgi = new System.Windows.Forms.Label();
            this.pbSecilenResim = new System.Windows.Forms.PictureBox();
            this.numMiktar = new System.Windows.Forms.NumericUpDown();
            this.btnSepeteEkle = new System.Windows.Forms.Button();
            this.btnYeniUrun = new System.Windows.Forms.Button();
            this.lblGizliId = new System.Windows.Forms.Label();
            this.grpTalepListe = new System.Windows.Forms.GroupBox();
            this.dgvTalepSepet = new System.Windows.Forms.DataGridView();
            this.pnlTalepAlt = new System.Windows.Forms.Panel();
            this.lblTalepEden = new System.Windows.Forms.Label();
            this.txtTalepEden = new System.Windows.Forms.TextBox();
            this.lblAciklama = new System.Windows.Forms.Label();
            this.txtAciklama = new System.Windows.Forms.TextBox();
            this.btnTalepGonder = new System.Windows.Forms.Button();

            // ── Teklif ────────────────────────────────────────────────────
            this.ctxTeklif = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSatirCogalt = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvTeklifListesi = new System.Windows.Forms.DataGridView();
            this.pnlTeklifAlt = new System.Windows.Forms.Panel();
            this.btnFiyatKaydet = new System.Windows.Forms.Button();

            // ── Onay ──────────────────────────────────────────────────────
            this.dgvOnayListesi = new System.Windows.Forms.DataGridView();
            this.pnlOnayAlt = new System.Windows.Forms.Panel();
            this.btnOnayla = new System.Windows.Forms.Button();
            this.btnReddet = new System.Windows.Forms.Button();
            this.btnBeklemeyeAl = new System.Windows.Forms.Button();

            // ── Stok giriş ────────────────────────────────────────────────
            this.dgvStokListesi = new System.Windows.Forms.DataGridView();
            this.pnlStokAlt = new System.Windows.Forms.Panel();
            this.lblFatura = new System.Windows.Forms.Label();
            this.txtFaturaNo = new System.Windows.Forms.TextBox();
            this.lblIrsaliye = new System.Windows.Forms.Label();
            this.txtIrsaliyeNo = new System.Windows.Forms.TextBox();
            this.btnStogaIsle = new System.Windows.Forms.Button();

            // ── Stok düşüm ────────────────────────────────────────────────
            this.dgvStokDurumu = new System.Windows.Forms.DataGridView();
            this.pnlDusumAlt = new System.Windows.Forms.Panel();
            this.txtDusumBarkod = new System.Windows.Forms.TextBox();
            this.btnCikisBul = new System.Windows.Forms.Button();
            this.btnTumunuGoster = new System.Windows.Forms.Button();
            this.lblDusumUrun = new System.Windows.Forms.Label();
            this.lblDusumKime = new System.Windows.Forms.Label();
            this.txtDusumKime = new System.Windows.Forms.TextBox();
            this.lblDusumNeden = new System.Windows.Forms.Label();
            this.txtDusumNeden = new System.Windows.Forms.TextBox();
            this.numDusumMiktar = new System.Windows.Forms.NumericUpDown();
            this.btnDusumYap = new System.Windows.Forms.Button();
            this.lblDusumGizliId = new System.Windows.Forms.Label();

            // ── Tedarikçi ─────────────────────────────────────────────────
            this.dgvTedarikci = new System.Windows.Forms.DataGridView();
            this.pnlTedAlt = new System.Windows.Forms.Panel();
            this.lblTedFirma = new System.Windows.Forms.Label();
            this.txtTedFirma = new System.Windows.Forms.TextBox();
            this.lblTedYetkili = new System.Windows.Forms.Label();
            this.txtTedYetkili = new System.Windows.Forms.TextBox();
            this.lblTedTel = new System.Windows.Forms.Label();
            this.txtTedTel = new System.Windows.Forms.TextBox();
            this.lblTedVergi = new System.Windows.Forms.Label();
            this.txtTedVergi = new System.Windows.Forms.TextBox();
            this.lblTedAdres = new System.Windows.Forms.Label();
            this.txtTedAdres = new System.Windows.Forms.TextBox();
            this.btnTedEkle = new System.Windows.Forms.Button();
            this.btnTedSil = new System.Windows.Forms.Button();

            // ── Kategori ──────────────────────────────────────────────────
            this.dgvKategoriler = new System.Windows.Forms.DataGridView();
            this.pnlKatAlt = new System.Windows.Forms.Panel();
            this.lblKatAdi = new System.Windows.Forms.Label();
            this.txtKatAdi = new System.Windows.Forms.TextBox();
            this.btnKatEkle = new System.Windows.Forms.Button();
            this.btnKatSil = new System.Windows.Forms.Button();

            // ── Log ───────────────────────────────────────────────────────
            this.dgvLogListesi = new System.Windows.Forms.DataGridView();
            this.pnlLogUst = new System.Windows.Forms.Panel();
            this.lblLogBas = new System.Windows.Forms.Label();
            this.txtLogArama = new System.Windows.Forms.TextBox();
            this.dtpBaslangic = new System.Windows.Forms.DateTimePicker();
            this.dtpBitis = new System.Windows.Forms.DateTimePicker();
            this.cmbIslemTuru = new System.Windows.Forms.ComboBox();
            this.btnLogListele = new System.Windows.Forms.Button();

            // ── Rapor ─────────────────────────────────────────────────────
            this.dgvRapor = new System.Windows.Forms.DataGridView();
            this.pnlRaporUst = new System.Windows.Forms.Panel();
            this.lblRaporTuru = new System.Windows.Forms.Label();
            this.cmbRaporTuru = new System.Windows.Forms.ComboBox();
            this.lblRaporKat = new System.Windows.Forms.Label();
            this.cmbRaporKategori = new System.Windows.Forms.ComboBox();
            this.lblRaporUrun = new System.Windows.Forms.Label();
            this.txtRaporUrun = new System.Windows.Forms.TextBox();
            this.dtpRaporBas = new System.Windows.Forms.DateTimePicker();
            this.dtpRaporBit = new System.Windows.Forms.DateTimePicker();
            this.btnRaporGetir = new System.Windows.Forms.Button();
            this.btnRaporExcel = new System.Windows.Forms.Button();

            // ── Popup ─────────────────────────────────────────────────────
            this.lblPopBaslik = new System.Windows.Forms.Label();
            this.lblPopKat = new System.Windows.Forms.Label();
            this.cmbPopKategori = new System.Windows.Forms.ComboBox();
            this.lblPopBarkod = new System.Windows.Forms.Label();
            this.txtPopBarkod = new System.Windows.Forms.TextBox();
            this.lblPopAd = new System.Windows.Forms.Label();
            this.txtPopAd = new System.Windows.Forms.TextBox();
            this.lblPopBirim = new System.Windows.Forms.Label();
            this.cmbPopBirim = new System.Windows.Forms.ComboBox();
            this.picPopUrun = new System.Windows.Forms.PictureBox();
            this.btnResimSec = new System.Windows.Forms.Button();
            this.btnPopKaydet = new System.Windows.Forms.Button();
            this.btnPopKapat = new System.Windows.Forms.Button();

            // ── SuspendLayout ─────────────────────────────────────────────
            this.pnlMenu.SuspendLayout();
            this.layoutMenu.SuspendLayout();
            this.pnlUst.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tabControlAna.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabDash.SuspendLayout();
            this.pnlDashUst.SuspendLayout();
            this.pnlDashCard1.SuspendLayout();
            this.pnlDashCard2.SuspendLayout();
            this.pnlDashCard3.SuspendLayout();
            this.pnlDashCard4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartStok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartHarcama)).BeginInit();
            this.tabTalep.SuspendLayout();
            this.grpUrunSecim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSecilenResim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMiktar)).BeginInit();
            this.grpTalepListe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTalepSepet)).BeginInit();
            this.pnlTalepAlt.SuspendLayout();
            this.tabTeklif.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeklifListesi)).BeginInit();
            this.ctxTeklif.SuspendLayout();
            this.pnlTeklifAlt.SuspendLayout();
            this.tabOnay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOnayListesi)).BeginInit();
            this.pnlOnayAlt.SuspendLayout();
            this.tabStok.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStokListesi)).BeginInit();
            this.pnlStokAlt.SuspendLayout();
            this.tabDusum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStokDurumu)).BeginInit();
            this.pnlDusumAlt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDusumMiktar)).BeginInit();
            this.tabTedarikci.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTedarikci)).BeginInit();
            this.pnlTedAlt.SuspendLayout();
            this.tabKategori.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKategoriler)).BeginInit();
            this.pnlKatAlt.SuspendLayout();
            this.tabLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogListesi)).BeginInit();
            this.pnlLogUst.SuspendLayout();
            this.tabRapor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRapor)).BeginInit();
            this.pnlRaporUst.SuspendLayout();
            this.pnlPopup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPopUrun)).BeginInit();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════════════
            //  FORM
            // ════════════════════════════════════════════════════════════
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "Sarf";
            this.Text = "BARAN ÇELİK — Sarf Malzeme Yönetimi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);

            // ════════════════════════════════════════════════════════════
            //  POPUP
            // ════════════════════════════════════════════════════════════
            this.pnlPopup.Size = new System.Drawing.Size(480, 560);
            this.pnlPopup.BackColor = System.Drawing.Color.White;
            this.pnlPopup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPopup.Visible = false;

            this.lblPopBaslik.Text = "YENİ ÜRÜN EKLE";
            this.lblPopBaslik.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPopBaslik.Location = new System.Drawing.Point(20, 18);
            this.lblPopBaslik.AutoSize = true;
            this.lblPopBaslik.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);

            SatirYap(this.lblPopKat, "Kategori:", this.cmbPopKategori, 60);
            SatirYap(this.lblPopBarkod, "Barkod:", this.txtPopBarkod, 100);
            SatirYap(this.lblPopAd, "Ürün Adı:", this.txtPopAd, 140);
            SatirYap(this.lblPopBirim, "Birim:", this.cmbPopBirim, 180);

            this.picPopUrun.Location = new System.Drawing.Point(120, 225);
            this.picPopUrun.Size = new System.Drawing.Size(160, 160);
            this.picPopUrun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPopUrun.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            this.btnResimSec.Text = "📷 Resim Seç";
            this.btnResimSec.Location = new System.Drawing.Point(295, 225);
            this.btnResimSec.Size = new System.Drawing.Size(140, 40);
            Btn(this.btnResimSec, System.Drawing.Color.FromArgb(100, 116, 139));
            this.btnResimSec.Click += new System.EventHandler(this.btnResimSec_Click);

            this.btnPopKaydet.Text = "✔  KAYDET";
            this.btnPopKaydet.Location = new System.Drawing.Point(120, 420);
            this.btnPopKaydet.Size = new System.Drawing.Size(150, 50);
            Btn(this.btnPopKaydet, System.Drawing.Color.FromArgb(16, 185, 129));
            this.btnPopKaydet.Click += new System.EventHandler(this.btnPopKaydet_Click);

            this.btnPopKapat.Text = "✖  İPTAL";
            this.btnPopKapat.Location = new System.Drawing.Point(290, 420);
            this.btnPopKapat.Size = new System.Drawing.Size(150, 50);
            Btn(this.btnPopKapat, System.Drawing.Color.FromArgb(239, 68, 68));
            this.btnPopKapat.Click += new System.EventHandler(this.btnPopKapat_Click);

            this.pnlPopup.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblPopBaslik, lblPopKat, cmbPopKategori, lblPopBarkod, txtPopBarkod,
                lblPopAd, txtPopAd, lblPopBirim, cmbPopBirim,
                picPopUrun, btnResimSec, btnPopKaydet, btnPopKapat });
            this.Controls.Add(this.pnlPopup);

            // ════════════════════════════════════════════════════════════
            //  ANA YERLEŞİM
            // ════════════════════════════════════════════════════════════
            // Sol menü paneli
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Width = 260;
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(17, 24, 39);

            // TableLayoutPanel — menü grid'i
            this.layoutMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMenu.ColumnCount = 1;
            this.layoutMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMenu.RowCount = 13; // Logo + 10 buton + Çıkış + StatusStrip
            this.layoutMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F)); // Logo
            for (int i = 0; i < 10; i++)
                this.layoutMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.5F));
            this.layoutMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9F));  // Çıkış
            this.layoutMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F)); // StatusStrip
            this.pnlMenu.Controls.Add(this.layoutMenu);

            // Üst başlık
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Height = 65;
            this.pnlUst.BackColor = System.Drawing.Color.White;

            // Ana içerik
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Padding = new System.Windows.Forms.Padding(20);

            // TabControl — gizli sekmeler
            this.tabControlAna.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlAna.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlAna.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControlAna.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;

            this.pnlMain.Controls.Add(this.tabControlAna);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlUst);
            this.Controls.Add(this.pnlMenu);

            // ════════════════════════════════════════════════════════════
            //  MENÜ — Logo + Butonlar
            // ════════════════════════════════════════════════════════════
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Text = "⚙  BARAN ÇELİK";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.layoutMenu.Controls.Add(this.lblLogo, 0, 0);

            MenuBtn(this.btnMenuDash, "📊  GENEL BAKIŞ", 0, 1, this.btnMenuDash_Click);
            MenuBtn(this.btnMenuTalep, "📝  TALEP OLUŞTUR", 0, 2, this.btnMenuTalep_Click);
            MenuBtn(this.btnMenuTeklif, "💰  TEKLİF AL", 0, 3, this.btnMenuTeklif_Click);
            MenuBtn(this.btnMenuOnay, "✅  ONAY EKRANI", 0, 4, this.btnMenuOnay_Click);
            MenuBtn(this.btnMenuStok, "📦  DEPO GİRİŞ", 0, 5, this.btnMenuStok_Click);
            MenuBtn(this.btnMenuDusum, "🔻  STOK DÜŞÜM", 0, 6, this.btnMenuDusum_Click);
            MenuBtn(this.btnMenuTedarikci, "🏭  TEDARİKÇİLER", 0, 7, this.btnMenuTedarikci_Click);
            MenuBtn(this.btnMenuKategori, "🏷  KATEGORİLER", 0, 8, this.btnMenuKategori_Click);
            MenuBtn(this.btnMenuLog, "📋  LOGLAR", 0, 9, this.btnMenuLog_Click);
            MenuBtn(this.btnMenuRapor, "📈  RAPORLAMA", 0, 10, this.btnMenuRapor_Click);

            this.btnMenuCikis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenuCikis.Text = "🚪  ÇIKIŞ YAP";
            this.btnMenuCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuCikis.FlatAppearance.BorderSize = 0;
            this.btnMenuCikis.ForeColor = System.Drawing.Color.FromArgb(252, 165, 165);
            this.btnMenuCikis.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnMenuCikis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnMenuCikis.Click += new System.EventHandler(this.btnMenuCikis_Click);
            this.layoutMenu.Controls.Add(this.btnMenuCikis, 0, 11);

            // StatusStrip — kur bilgisi
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.statusStrip1.Items.Add(this.lblKurBilgi);
            this.lblKurBilgi.ForeColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.pnlMenu.Controls.Add(this.statusStrip1);

            // ÜST — Başlık
            this.lblBaslik.AutoSize = false;
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.lblBaslik.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pnlUst.Controls.Add(this.lblBaslik);

            // ════════════════════════════════════════════════════════════
            //  TAB 0: DASHBOARD
            // ════════════════════════════════════════════════════════════
            this.tabDash.Text = "Dash";
            this.tabDash.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);

            this.pnlDashUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDashUst.Height = 180;
            this.pnlDashUst.ColumnCount = 4;
            this.pnlDashUst.RowCount = 1;
            this.pnlDashUst.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlDashUst.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlDashUst.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlDashUst.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlDashUst.Padding = new System.Windows.Forms.Padding(10, 16, 10, 10);

            DashKart(this.pnlDashCard1, this.lblDash1Title, this.lblDash1Val,
                "Bekleyen Talepler", System.Drawing.Color.FromArgb(59, 130, 246), 0);
            DashKart(this.pnlDashCard2, this.lblDash2Title, this.lblDash2Val,
                "Onay Bekleyenler", System.Drawing.Color.FromArgb(245, 158, 11), 1);
            DashKart(this.pnlDashCard3, this.lblDash3Title, this.lblDash3Val,
                "Giriş Bekleyenler", System.Drawing.Color.FromArgb(16, 185, 129), 2);
            DashKart(this.pnlDashCard4, this.lblDash4Title, this.lblDash4Val,
                "Toplam Ürün", System.Drawing.Color.FromArgb(139, 92, 246), 3);

            chartArea1.Name = "CA1"; chartArea1.BackColor = System.Drawing.Color.Transparent;
            legend1.Name = "L1";
            series1.Name = "Stok"; series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            this.chartStok.BackColor = System.Drawing.Color.White;
            this.chartStok.Dock = System.Windows.Forms.DockStyle.Left;
            this.chartStok.Width = 640;
            this.chartStok.Padding = new System.Windows.Forms.Padding(10);
            this.chartStok.ChartAreas.Add(chartArea1);
            this.chartStok.Legends.Add(legend1);
            this.chartStok.Series.Add(series1);

            chartArea2.Name = "CA2"; chartArea2.BackColor = System.Drawing.Color.Transparent;
            legend2.Name = "L2";
            series2.Name = "Harcama"; series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            this.chartHarcama.BackColor = System.Drawing.Color.White;
            this.chartHarcama.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartHarcama.Padding = new System.Windows.Forms.Padding(10);
            this.chartHarcama.ChartAreas.Add(chartArea2);
            this.chartHarcama.Legends.Add(legend2);
            this.chartHarcama.Series.Add(series2);

            this.tabDash.Controls.Add(this.chartHarcama);
            this.tabDash.Controls.Add(this.chartStok);
            this.tabDash.Controls.Add(this.pnlDashUst);

            // ════════════════════════════════════════════════════════════
            //  TAB 1: TALEP
            // ════════════════════════════════════════════════════════════
            this.tabTalep.Text = "Talep";
            this.tabTalep.BackColor = System.Drawing.Color.White;

            this.grpUrunSecim.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpUrunSecim.Width = 420;
            this.grpUrunSecim.Text = "Ürün Seçimi";
            this.grpUrunSecim.Padding = new System.Windows.Forms.Padding(12);
            this.grpUrunSecim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.lblAraBaslik.Text = "Ürün Adı ile Ara:";
            this.lblAraBaslik.Location = new System.Drawing.Point(14, 38);
            this.lblAraBaslik.AutoSize = true;

            this.txtArama.Location = new System.Drawing.Point(14, 64);
            this.txtArama.Width = 240;
            this.txtArama.Height = 34;
            this.txtArama.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtArama.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArama_KeyDown);

            this.btnUrunBul.Text = "ARA";
            this.btnUrunBul.Location = new System.Drawing.Point(264, 62);
            this.btnUrunBul.Size = new System.Drawing.Size(110, 36);
            Btn(this.btnUrunBul, System.Drawing.Color.FromArgb(59, 130, 246));
            this.btnUrunBul.Click += new System.EventHandler(this.btnUrunBul_Click);

            this.lblUrunBilgi.Location = new System.Drawing.Point(14, 112);
            this.lblUrunBilgi.AutoSize = true;
            this.lblUrunBilgi.Text = "Ürün seçilmedi...";
            this.lblUrunBilgi.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.lblUrunBilgi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            this.lblGizliId.Visible = false;
            this.lblGizliId.Location = new System.Drawing.Point(0, 0);

            this.pbSecilenResim.Location = new System.Drawing.Point(14, 148);
            this.pbSecilenResim.Size = new System.Drawing.Size(374, 280);
            this.pbSecilenResim.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSecilenResim.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSecilenResim.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);

            this.numMiktar.Location = new System.Drawing.Point(14, 446);
            this.numMiktar.Width = 110;
            this.numMiktar.Minimum = 1;
            this.numMiktar.Maximum = 99999;
            this.numMiktar.Value = 1;
            this.numMiktar.Font = new System.Drawing.Font("Segoe UI", 14F);

            this.btnSepeteEkle.Text = "➕  SEPETE EKLE";
            this.btnSepeteEkle.Location = new System.Drawing.Point(136, 442);
            this.btnSepeteEkle.Size = new System.Drawing.Size(252, 44);
            Btn(this.btnSepeteEkle, System.Drawing.Color.FromArgb(16, 185, 129));
            this.btnSepeteEkle.Click += new System.EventHandler(this.btnSepeteEkle_Click);

            this.btnYeniUrun.Text = "🆕  YENİ ÜRÜN OLUŞTUR";
            this.btnYeniUrun.Location = new System.Drawing.Point(14, 502);
            this.btnYeniUrun.Size = new System.Drawing.Size(374, 50);
            Btn(this.btnYeniUrun, System.Drawing.Color.FromArgb(245, 158, 11));
            this.btnYeniUrun.Click += new System.EventHandler(this.btnYeniUrun_Click);

            this.grpUrunSecim.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblAraBaslik, txtArama, btnUrunBul, lblUrunBilgi, lblGizliId,
                pbSecilenResim, numMiktar, btnSepeteEkle, btnYeniUrun });

            this.grpTalepListe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTalepListe.Text = "Talep Sepeti";
            this.grpTalepListe.Padding = new System.Windows.Forms.Padding(10);

            DgvAyarla(this.dgvTalepSepet, headerStyle, rowStyle);
            this.dgvTalepSepet.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlTalepAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTalepAlt.Height = 90;
            this.pnlTalepAlt.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            InlineLabel(this.lblTalepEden, "Talep Eden:", 16, 32);
            InlineText(this.txtTalepEden, 120, 28, 220);
            InlineLabel(this.lblAciklama, "Açıklama:", 360, 32);
            InlineText(this.txtAciklama, 450, 28, 420);

            this.btnTalepGonder.Text = "📨  TALEBİ GÖNDER";
            this.btnTalepGonder.Location = new System.Drawing.Point(900, 18);
            this.btnTalepGonder.Size = new System.Drawing.Size(220, 56);
            Btn(this.btnTalepGonder, System.Drawing.Color.FromArgb(31, 41, 55));
            this.btnTalepGonder.Click += new System.EventHandler(this.btnTalepGonder_Click);

            this.pnlTalepAlt.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTalepEden, txtTalepEden, lblAciklama, txtAciklama, btnTalepGonder });

            this.grpTalepListe.Controls.Add(this.dgvTalepSepet);
            this.grpTalepListe.Controls.Add(this.pnlTalepAlt);
            this.tabTalep.Controls.Add(this.grpTalepListe);
            this.tabTalep.Controls.Add(this.grpUrunSecim);

            // ════════════════════════════════════════════════════════════
            //  TAB 2: TEKLİF
            // ════════════════════════════════════════════════════════════
            this.tabTeklif.Text = "Teklif";
            this.tabTeklif.BackColor = System.Drawing.Color.White;

            DgvAyarla(this.dgvTeklifListesi, headerStyle, rowStyle);
            this.dgvTeklifListesi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTeklifListesi.ContextMenuStrip = this.ctxTeklif;

            this.mnuSatirCogalt.Text = "➕  Bu Kalem İçin Yeni Teklif Al";
            this.mnuSatirCogalt.Click += new System.EventHandler(this.mnuSatirCogalt_Click);
            this.ctxTeklif.Items.Add(this.mnuSatirCogalt);

            this.pnlTeklifAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTeklifAlt.Height = 72;
            this.pnlTeklifAlt.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            this.btnFiyatKaydet.Text = "📤  SEÇİLİ TEKLİFLERİ ONAYA GÖNDER";
            this.btnFiyatKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFiyatKaydet.Width = 340;
            Btn(this.btnFiyatKaydet, System.Drawing.Color.FromArgb(59, 130, 246));
            this.btnFiyatKaydet.Click += new System.EventHandler(this.btnFiyatKaydet_Click);

            this.pnlTeklifAlt.Controls.Add(this.btnFiyatKaydet);
            this.tabTeklif.Controls.Add(this.dgvTeklifListesi);
            this.tabTeklif.Controls.Add(this.pnlTeklifAlt);

            // ════════════════════════════════════════════════════════════
            //  TAB 3: ONAY
            // ════════════════════════════════════════════════════════════
            this.tabOnay.Text = "Onay";
            this.tabOnay.BackColor = System.Drawing.Color.White;

            DgvAyarla(this.dgvOnayListesi, headerStyle, rowStyle);
            this.dgvOnayListesi.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlOnayAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlOnayAlt.Height = 80;
            this.pnlOnayAlt.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            this.btnOnayla.Text = "✔  ONAYLA";
            this.btnOnayla.Location = new System.Drawing.Point(16, 14);
            this.btnOnayla.Size = new System.Drawing.Size(160, 52);
            Btn(this.btnOnayla, System.Drawing.Color.FromArgb(16, 185, 129));
            this.btnOnayla.Click += new System.EventHandler(this.btnOnayla_Click);

            this.btnReddet.Text = "✖  REDDET";
            this.btnReddet.Location = new System.Drawing.Point(190, 14);
            this.btnReddet.Size = new System.Drawing.Size(160, 52);
            Btn(this.btnReddet, System.Drawing.Color.FromArgb(239, 68, 68));
            this.btnReddet.Click += new System.EventHandler(this.btnReddet_Click);

            this.btnBeklemeyeAl.Text = "⏸  BEKLEMEYE AL";
            this.btnBeklemeyeAl.Location = new System.Drawing.Point(364, 14);
            this.btnBeklemeyeAl.Size = new System.Drawing.Size(180, 52);
            Btn(this.btnBeklemeyeAl, System.Drawing.Color.FromArgb(245, 158, 11));
            this.btnBeklemeyeAl.Click += new System.EventHandler(this.btnBeklemeyeAl_Click);

            this.pnlOnayAlt.Controls.AddRange(new System.Windows.Forms.Control[] {
                btnOnayla, btnReddet, btnBeklemeyeAl });
            this.tabOnay.Controls.Add(this.dgvOnayListesi);
            this.tabOnay.Controls.Add(this.pnlOnayAlt);

            // ════════════════════════════════════════════════════════════
            //  TAB 4: DEPO GİRİŞ
            // ════════════════════════════════════════════════════════════
            this.tabStok.Text = "Stok";
            this.tabStok.BackColor = System.Drawing.Color.White;

            DgvAyarla(this.dgvStokListesi, headerStyle, rowStyle);
            this.dgvStokListesi.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlStokAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStokAlt.Height = 80;
            this.pnlStokAlt.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            InlineLabel(this.lblFatura, "Fatura No:", 16, 30);
            InlineText(this.txtFaturaNo, 110, 26, 210);
            InlineLabel(this.lblIrsaliye, "İrsaliye No:", 340, 30);
            InlineText(this.txtIrsaliyeNo, 450, 26, 210);

            this.btnStogaIsle.Text = "📥  DEPO GİRİŞİ YAP";
            this.btnStogaIsle.Location = new System.Drawing.Point(690, 14);
            this.btnStogaIsle.Size = new System.Drawing.Size(220, 52);
            Btn(this.btnStogaIsle, System.Drawing.Color.FromArgb(16, 185, 129));
            this.btnStogaIsle.Click += new System.EventHandler(this.btnStogaIsle_Click);

            this.pnlStokAlt.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblFatura, txtFaturaNo, lblIrsaliye, txtIrsaliyeNo, btnStogaIsle });
            this.tabStok.Controls.Add(this.dgvStokListesi);
            this.tabStok.Controls.Add(this.pnlStokAlt);

            // ════════════════════════════════════════════════════════════
            //  TAB 5: STOK DÜŞÜM
            // ════════════════════════════════════════════════════════════
            this.tabDusum.Text = "Dusum";
            this.tabDusum.BackColor = System.Drawing.Color.White;

            DgvAyarla(this.dgvStokDurumu, headerStyle, rowStyle);
            this.dgvStokDurumu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStokDurumu.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStokDurumu_CellClick);

            this.pnlDusumAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDusumAlt.Height = 120;
            this.pnlDusumAlt.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            // Satır 1: Arama
            this.txtDusumBarkod.Location = new System.Drawing.Point(16, 16);
            this.txtDusumBarkod.Width = 220;
            this.txtDusumBarkod.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtDusumBarkod.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDusumBarkod_KeyDown);

            this.btnCikisBul.Text = "ARA";
            this.btnCikisBul.Location = new System.Drawing.Point(246, 14);
            this.btnCikisBul.Size = new System.Drawing.Size(80, 34);
            Btn(this.btnCikisBul, System.Drawing.Color.FromArgb(100, 116, 139));
            this.btnCikisBul.Click += new System.EventHandler(this.btnCikisBul_Click);

            this.btnTumunuGoster.Text = "🔄  LİSTEYİ YENİLE";
            this.btnTumunuGoster.Location = new System.Drawing.Point(336, 14);
            this.btnTumunuGoster.Size = new System.Drawing.Size(170, 34);
            Btn(this.btnTumunuGoster, System.Drawing.Color.FromArgb(100, 116, 139));
            this.btnTumunuGoster.Click += new System.EventHandler(this.btnTumunuGoster_Click);

            this.lblDusumUrun.Text = "Seçilen Ürün: —";
            this.lblDusumUrun.Location = new System.Drawing.Point(530, 16);
            this.lblDusumUrun.AutoSize = true;
            this.lblDusumUrun.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDusumUrun.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);

            this.lblDusumGizliId.Visible = false;
            this.lblDusumGizliId.Location = new System.Drawing.Point(0, 0);

            // Satır 2: Düşüm bilgileri
            InlineLabel(this.lblDusumKime, "Alan Kişi:", 16, 70);
            InlineText(this.txtDusumKime, 100, 66, 210);
            InlineLabel(this.lblDusumNeden, "Sebep:", 326, 70);
            InlineText(this.txtDusumNeden, 390, 66, 290);

            this.numDusumMiktar.Location = new System.Drawing.Point(700, 66);
            this.numDusumMiktar.Width = 110;
            this.numDusumMiktar.Minimum = 1;
            this.numDusumMiktar.Maximum = 999999;
            this.numDusumMiktar.Value = 1;
            this.numDusumMiktar.Font = new System.Drawing.Font("Segoe UI", 12F);

            this.btnDusumYap.Text = "🔻  STOKTAN DÜŞ";
            this.btnDusumYap.Location = new System.Drawing.Point(826, 60);
            this.btnDusumYap.Size = new System.Drawing.Size(190, 44);
            Btn(this.btnDusumYap, System.Drawing.Color.FromArgb(239, 68, 68));
            this.btnDusumYap.Click += new System.EventHandler(this.btnDusumYap_ClickTam);
            this.pnlDusumAlt.Controls.AddRange(new System.Windows.Forms.Control[] {
                txtDusumBarkod, btnCikisBul, btnTumunuGoster, lblDusumUrun, lblDusumGizliId,
                lblDusumKime, txtDusumKime, lblDusumNeden, txtDusumNeden,
                numDusumMiktar, btnDusumYap });
            this.tabDusum.Controls.Add(this.dgvStokDurumu);
            this.tabDusum.Controls.Add(this.pnlDusumAlt);

            // ════════════════════════════════════════════════════════════
            //  TAB 6: TEDARİKÇİ
            // ════════════════════════════════════════════════════════════
            this.tabTedarikci.Text = "Tedarikci";
            this.tabTedarikci.BackColor = System.Drawing.Color.White;

            DgvAyarla(this.dgvTedarikci, headerStyle, rowStyle);
            this.dgvTedarikci.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlTedAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTedAlt.Height = 140;
            this.pnlTedAlt.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            InlineLabel(this.lblTedFirma, "Firma Adı:", 16, 28); InlineText(this.txtTedFirma, 110, 24, 260);
            InlineLabel(this.lblTedYetkili, "Yetkili:", 396, 28); InlineText(this.txtTedYetkili, 460, 24, 200);
            InlineLabel(this.lblTedTel, "Telefon:", 680, 28); InlineText(this.txtTedTel, 756, 24, 200);
            InlineLabel(this.lblTedVergi, "Vergi No:", 16, 82); InlineText(this.txtTedVergi, 110, 78, 260);
            InlineLabel(this.lblTedAdres, "Adres:", 396, 82); InlineText(this.txtTedAdres, 460, 78, 500);

            this.btnTedEkle.Text = "💾  KAYDET";
            this.btnTedEkle.Location = new System.Drawing.Point(980, 20);
            this.btnTedEkle.Size = new System.Drawing.Size(140, 100);
            Btn(this.btnTedEkle, System.Drawing.Color.FromArgb(16, 185, 129));
            this.btnTedEkle.Click += new System.EventHandler(this.btnTedEkle_Click);

            this.btnTedSil.Text = "🗑  SİL";
            this.btnTedSil.Location = new System.Drawing.Point(1130, 20);
            this.btnTedSil.Size = new System.Drawing.Size(110, 100);
            Btn(this.btnTedSil, System.Drawing.Color.FromArgb(239, 68, 68));
            this.btnTedSil.Click += new System.EventHandler(this.btnTedSil_Click);

            this.pnlTedAlt.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTedFirma, txtTedFirma, lblTedYetkili, txtTedYetkili,
                lblTedTel, txtTedTel, lblTedVergi, txtTedVergi, lblTedAdres, txtTedAdres,
                btnTedEkle, btnTedSil });
            this.tabTedarikci.Controls.Add(this.dgvTedarikci);
            this.tabTedarikci.Controls.Add(this.pnlTedAlt);

            // ════════════════════════════════════════════════════════════
            //  TAB 7: KATEGORİ
            // ════════════════════════════════════════════════════════════
            this.tabKategori.Text = "Kategori";
            this.tabKategori.BackColor = System.Drawing.Color.White;

            DgvAyarla(this.dgvKategoriler, headerStyle, rowStyle);
            this.dgvKategoriler.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlKatAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlKatAlt.Height = 80;
            this.pnlKatAlt.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            InlineLabel(this.lblKatAdi, "Kategori Adı:", 20, 30);
            InlineText(this.txtKatAdi, 136, 26, 300);

            this.btnKatEkle.Text = "🎨  RENK SEÇ VE EKLE";
            this.btnKatEkle.Location = new System.Drawing.Point(454, 18);
            this.btnKatEkle.Size = new System.Drawing.Size(220, 44);
            Btn(this.btnKatEkle, System.Drawing.Color.FromArgb(100, 116, 139));
            this.btnKatEkle.Click += new System.EventHandler(this.btnKatEkle_Click);

            this.btnKatSil.Text = "🗑  SİL";
            this.btnKatSil.Location = new System.Drawing.Point(684, 18);
            this.btnKatSil.Size = new System.Drawing.Size(120, 44);
            Btn(this.btnKatSil, System.Drawing.Color.FromArgb(239, 68, 68));
            this.btnKatSil.Click += new System.EventHandler(this.btnKatSil_Click);

            this.pnlKatAlt.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblKatAdi, txtKatAdi, btnKatEkle, btnKatSil });
            this.tabKategori.Controls.Add(this.dgvKategoriler);
            this.tabKategori.Controls.Add(this.pnlKatAlt);

            // ════════════════════════════════════════════════════════════
            //  TAB 8: LOG
            // ═══════════════════════════════════════════════════════��════
            this.tabLog.Text = "Log";
            this.tabLog.BackColor = System.Drawing.Color.White;

            DgvAyarla(this.dgvLogListesi, headerStyle, rowStyle);
            this.dgvLogListesi.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlLogUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogUst.Height = 80;
            this.pnlLogUst.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            InlineLabel(this.lblLogBas, "Ürün / Kişi Ara:", 16, 30);
            this.txtLogArama.Location = new System.Drawing.Point(148, 26);
            this.txtLogArama.Width = 220;
            this.txtLogArama.Font = new System.Drawing.Font("Segoe UI", 11F);

            this.dtpBaslangic.Location = new System.Drawing.Point(390, 26);
            this.dtpBaslangic.Width = 160;
            this.dtpBitis.Location = new System.Drawing.Point(564, 26);
            this.dtpBitis.Width = 160;

            this.cmbIslemTuru.Location = new System.Drawing.Point(738, 26);
            this.cmbIslemTuru.Width = 140;
            this.cmbIslemTuru.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.btnLogListele.Text = "🔍  SORGULA";
            this.btnLogListele.Location = new System.Drawing.Point(894, 20);
            this.btnLogListele.Size = new System.Drawing.Size(160, 40);
            Btn(this.btnLogListele, System.Drawing.Color.FromArgb(59, 130, 246));
            this.btnLogListele.Click += new System.EventHandler(this.btnLogListele_Click);

            this.pnlLogUst.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblLogBas, txtLogArama, dtpBaslangic, dtpBitis, cmbIslemTuru, btnLogListele });
            this.tabLog.Controls.Add(this.dgvLogListesi);
            this.tabLog.Controls.Add(this.pnlLogUst);

            // ════════════════════════════════════════════════════════════
            //  TAB 9: RAPOR
            // ════════════════════════════════════════════════════════════
            this.tabRapor.Text = "Rapor";
            this.tabRapor.BackColor = System.Drawing.Color.White;

            DgvAyarla(this.dgvRapor, headerStyle, rowStyle);
            this.dgvRapor.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlRaporUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRaporUst.Height = 82;
            this.pnlRaporUst.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);

            InlineLabel(this.lblRaporTuru, "Rapor Türü:", 16, 28);
            this.cmbRaporTuru.Location = new System.Drawing.Point(16, 50);
            this.cmbRaporTuru.Width = 240;
            this.cmbRaporTuru.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRaporTuru.Items.AddRange(new object[] {
                "Genel Satın Alma Özeti", "Detaylı Hareket Raporu" });
            this.cmbRaporTuru.SelectedIndex = 0;

            InlineLabel(this.lblRaporKat, "Kategori:", 276, 28);
            this.cmbRaporKategori.Location = new System.Drawing.Point(276, 50);
            this.cmbRaporKategori.Width = 200;
            this.cmbRaporKategori.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            InlineLabel(this.lblRaporUrun, "Ürün / Tedarikçi:", 496, 28);
            this.txtRaporUrun.Location = new System.Drawing.Point(496, 50);
            this.txtRaporUrun.Width = 220;

            this.dtpRaporBas.Location = new System.Drawing.Point(736, 50);
            this.dtpRaporBas.Width = 140;
            this.dtpRaporBit.Location = new System.Drawing.Point(888, 50);
            this.dtpRaporBit.Width = 140;

            this.btnRaporGetir.Text = "📊  RAPORLA";
            this.btnRaporGetir.Location = new System.Drawing.Point(1046, 44);
            this.btnRaporGetir.Size = new System.Drawing.Size(140, 38);
            Btn(this.btnRaporGetir, System.Drawing.Color.FromArgb(59, 130, 246));
            this.btnRaporGetir.Click += new System.EventHandler(this.btnRaporGetir_Click);

            this.btnRaporExcel.Text = "📥  EXCEL";
            this.btnRaporExcel.Location = new System.Drawing.Point(1200, 44);
            this.btnRaporExcel.Size = new System.Drawing.Size(120, 38);
            Btn(this.btnRaporExcel, System.Drawing.Color.FromArgb(16, 185, 129));
            this.btnRaporExcel.Click += new System.EventHandler(this.btnRaporExcel_Click);

            this.pnlRaporUst.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblRaporTuru, cmbRaporTuru, lblRaporKat, cmbRaporKategori,
                lblRaporUrun, txtRaporUrun, dtpRaporBas, dtpRaporBit,
                btnRaporGetir, btnRaporExcel });
            this.tabRapor.Controls.Add(this.dgvRapor);
            this.tabRapor.Controls.Add(this.pnlRaporUst);

            // ── Tüm tabları ekle ─────────────────────────────────────────
            this.tabControlAna.Controls.AddRange(new System.Windows.Forms.TabPage[] {
                tabDash, tabTalep, tabTeklif, tabOnay, tabStok,
                tabDusum, tabTedarikci, tabKategori, tabLog, tabRapor });

            // ── ResumeLayout ─────────────────────────────────────────────
            this.pnlDashCard1.ResumeLayout(false); this.pnlDashCard1.PerformLayout();
            this.pnlDashCard2.ResumeLayout(false); this.pnlDashCard2.PerformLayout();
            this.pnlDashCard3.ResumeLayout(false); this.pnlDashCard3.PerformLayout();
            this.pnlDashCard4.ResumeLayout(false); this.pnlDashCard4.PerformLayout();
            this.pnlDashUst.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartStok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartHarcama)).EndInit();
            this.tabDash.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTalepSepet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSecilenResim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMiktar)).EndInit();
            this.pnlTalepAlt.ResumeLayout(false); this.pnlTalepAlt.PerformLayout();
            this.grpTalepListe.ResumeLayout(false);
            this.grpUrunSecim.ResumeLayout(false); this.grpUrunSecim.PerformLayout();
            this.tabTalep.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeklifListesi)).EndInit();
            this.ctxTeklif.ResumeLayout(false);
            this.pnlTeklifAlt.ResumeLayout(false);
            this.tabTeklif.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOnayListesi)).EndInit();
            this.pnlOnayAlt.ResumeLayout(false);
            this.tabOnay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStokListesi)).EndInit();
            this.pnlStokAlt.ResumeLayout(false); this.pnlStokAlt.PerformLayout();
            this.tabStok.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStokDurumu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDusumMiktar)).EndInit();
            this.pnlDusumAlt.ResumeLayout(false); this.pnlDusumAlt.PerformLayout();
            this.tabDusum.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTedarikci)).EndInit();
            this.pnlTedAlt.ResumeLayout(false); this.pnlTedAlt.PerformLayout();
            this.tabTedarikci.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKategoriler)).EndInit();
            this.pnlKatAlt.ResumeLayout(false); this.pnlKatAlt.PerformLayout();
            this.tabKategori.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogListesi)).EndInit();
            this.pnlLogUst.ResumeLayout(false); this.pnlLogUst.PerformLayout();
            this.tabLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRapor)).EndInit();
            this.pnlRaporUst.ResumeLayout(false); this.pnlRaporUst.PerformLayout();
            this.tabRapor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPopUrun)).EndInit();
            this.pnlPopup.ResumeLayout(false); this.pnlPopup.PerformLayout();
            this.statusStrip1.ResumeLayout(false); this.statusStrip1.PerformLayout();
            this.layoutMenu.ResumeLayout(false);
            this.pnlMenu.ResumeLayout(false); this.pnlMenu.PerformLayout();
            this.pnlUst.ResumeLayout(false); this.pnlUst.PerformLayout();
            this.tabControlAna.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ── Yardımcı metodlar ─────────────────────────────────────────────

        private void MenuBtn(System.Windows.Forms.Button b, string text, int col, int row,
            System.EventHandler click)
        {
            b.Dock = System.Windows.Forms.DockStyle.Fill;
            b.Text = text;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.ForeColor = System.Drawing.Color.FromArgb(209, 213, 219);
            b.Font = new System.Drawing.Font("Segoe UI", 11F);
            b.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            b.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            b.Click += click;
            this.layoutMenu.Controls.Add(b, col, row);
        }

        private void DashKart(System.Windows.Forms.Panel pnl,
            System.Windows.Forms.Label lblTitle, System.Windows.Forms.Label lblVal,
            string title, System.Drawing.Color color, int col)
        {
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            pnl.Margin = new System.Windows.Forms.Padding(10);
            pnl.BackColor = System.Drawing.Color.White;

            lblTitle.Text = title;
            lblTitle.Location = new System.Drawing.Point(20, 22);
            lblTitle.AutoSize = true;
            lblTitle.ForeColor = System.Drawing.Color.Gray;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F);

            lblVal.Text = "—";
            lblVal.Location = new System.Drawing.Point(20, 58);
            lblVal.AutoSize = true;
            lblVal.ForeColor = color;
            lblVal.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblVal);
            this.pnlDashUst.Controls.Add(pnl, col, 0);
        }

        private void Btn(System.Windows.Forms.Button b, System.Drawing.Color color)
        {
            b.BackColor = color;
            b.ForeColor = System.Drawing.Color.White;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            b.Cursor = System.Windows.Forms.Cursors.Hand;
            b.UseVisualStyleBackColor = false;
        }

        private void DgvAyarla(System.Windows.Forms.DataGridView dgv,
            System.Windows.Forms.DataGridViewCellStyle header,
            System.Windows.Forms.DataGridViewCellStyle row)
        {
            dgv.ColumnHeadersDefaultCellStyle = header;
            dgv.DefaultCellStyle = row;
            dgv.BackgroundColor = System.Drawing.Color.White;
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 44;
            dgv.RowTemplate.Height = 40;
            dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
        }

        private void InlineLabel(System.Windows.Forms.Label lbl, string text, int x, int y)
        {
            lbl.Text = text;
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Segoe UI", 10F);
            lbl.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
        }

        private void InlineText(System.Windows.Forms.TextBox tb, int x, int y, int w)
        {
            tb.Location = new System.Drawing.Point(x, y);
            tb.Width = w;
            tb.Height = 32;
            tb.Font = new System.Drawing.Font("Segoe UI", 10F);
        }

        private void SatirYap(System.Windows.Forms.Label lbl, string text,
            System.Windows.Forms.Control ctrl, int y)
        {
            lbl.Text = text;
            lbl.Location = new System.Drawing.Point(20, y + 3);
            lbl.AutoSize = true;
            ctrl.Location = new System.Drawing.Point(120, y);
            ctrl.Width = 330;
            if (ctrl is System.Windows.Forms.TextBox tb)
                tb.Font = new System.Drawing.Font("Segoe UI", 10F);
        }

        // ── Field bildirimleri ────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlMenu, pnlUst, pnlMain, pnlPopup;
        private System.Windows.Forms.Panel pnlTalepAlt, pnlTeklifAlt, pnlOnayAlt, pnlStokAlt;
        private System.Windows.Forms.Panel pnlDusumAlt, pnlTedAlt, pnlKatAlt, pnlLogUst, pnlRaporUst;
        private System.Windows.Forms.Panel pnlDashCard1, pnlDashCard2, pnlDashCard3, pnlDashCard4;
        private System.Windows.Forms.TableLayoutPanel layoutMenu, pnlDashUst;
        private System.Windows.Forms.TabControl tabControlAna;
        private System.Windows.Forms.TabPage tabDash, tabTalep, tabTeklif, tabOnay, tabStok;
        private System.Windows.Forms.TabPage tabDusum, tabTedarikci, tabKategori, tabLog, tabRapor;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblKurBilgi;
        private System.Windows.Forms.Label lblLogo, lblBaslik;
        private System.Windows.Forms.Label lblDash1Title, lblDash1Val, lblDash2Title, lblDash2Val;
        private System.Windows.Forms.Label lblDash3Title, lblDash3Val, lblDash4Title, lblDash4Val;
        private System.Windows.Forms.Button btnMenuDash, btnMenuTalep, btnMenuTeklif, btnMenuOnay;
        private System.Windows.Forms.Button btnMenuStok, btnMenuDusum, btnMenuTedarikci;
        private System.Windows.Forms.Button btnMenuKategori, btnMenuLog, btnMenuRapor, btnMenuCikis;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStok, chartHarcama;
        private System.Windows.Forms.GroupBox grpUrunSecim, grpTalepListe;
        private System.Windows.Forms.Label lblAraBaslik, lblUrunBilgi, lblGizliId;
        private System.Windows.Forms.Label lblTalepEden, lblAciklama;
        private System.Windows.Forms.TextBox txtArama, txtTalepEden, txtAciklama;
        private System.Windows.Forms.PictureBox pbSecilenResim;
        private System.Windows.Forms.NumericUpDown numMiktar;
        private System.Windows.Forms.Button btnUrunBul, btnSepeteEkle, btnYeniUrun, btnTalepGonder;
        private System.Windows.Forms.DataGridView dgvTalepSepet, dgvTeklifListesi, dgvOnayListesi;
        private System.Windows.Forms.DataGridView dgvStokListesi, dgvStokDurumu, dgvTedarikci;
        private System.Windows.Forms.DataGridView dgvKategoriler, dgvLogListesi, dgvRapor;
        private System.Windows.Forms.ContextMenuStrip ctxTeklif;
        private System.Windows.Forms.ToolStripMenuItem mnuSatirCogalt;
        private System.Windows.Forms.Button btnFiyatKaydet;
        private System.Windows.Forms.Button btnOnayla, btnReddet, btnBeklemeyeAl;
        private System.Windows.Forms.Label lblFatura, lblIrsaliye;
        private System.Windows.Forms.TextBox txtFaturaNo, txtIrsaliyeNo;
        private System.Windows.Forms.Button btnStogaIsle;
        private System.Windows.Forms.TextBox txtDusumBarkod, txtDusumKime, txtDusumNeden;
        private System.Windows.Forms.Label lblDusumUrun, lblDusumGizliId, lblDusumKime, lblDusumNeden;
        private System.Windows.Forms.NumericUpDown numDusumMiktar;
        private System.Windows.Forms.Button btnCikisBul, btnTumunuGoster, btnDusumYap;
        private System.Windows.Forms.Label lblTedFirma, lblTedYetkili, lblTedTel, lblTedVergi, lblTedAdres;
        private System.Windows.Forms.TextBox txtTedFirma, txtTedYetkili, txtTedTel, txtTedVergi, txtTedAdres;
        private System.Windows.Forms.Button btnTedEkle, btnTedSil;
        private System.Windows.Forms.Label lblKatAdi;
        private System.Windows.Forms.TextBox txtKatAdi;
        private System.Windows.Forms.Button btnKatEkle, btnKatSil;
        private System.Windows.Forms.Label lblLogBas;
        private System.Windows.Forms.TextBox txtLogArama;
        private System.Windows.Forms.DateTimePicker dtpBaslangic, dtpBitis;
        private System.Windows.Forms.ComboBox cmbIslemTuru;
        private System.Windows.Forms.Button btnLogListele;
        private System.Windows.Forms.Label lblRaporTuru, lblRaporKat, lblRaporUrun;
        private System.Windows.Forms.ComboBox cmbRaporTuru, cmbRaporKategori;
        private System.Windows.Forms.TextBox txtRaporUrun;
        private System.Windows.Forms.DateTimePicker dtpRaporBas, dtpRaporBit;
        private System.Windows.Forms.Button btnRaporGetir, btnRaporExcel;
        private System.Windows.Forms.Label lblPopBaslik, lblPopKat, lblPopBarkod, lblPopAd, lblPopBirim;
        private System.Windows.Forms.ComboBox cmbPopKategori, cmbPopBirim;
        private System.Windows.Forms.TextBox txtPopBarkod, txtPopAd;
        private System.Windows.Forms.PictureBox picPopUrun;
        private System.Windows.Forms.Button btnResimSec, btnPopKaydet, btnPopKapat;
    }
}