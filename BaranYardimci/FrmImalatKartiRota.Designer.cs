namespace BaranYardimci
{
    partial class FrmImalatKartiRota
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
            this.lblBaslik = new System.Windows.Forms.Label();
            this.lblErpDosya = new System.Windows.Forms.Label();
            this.btnKlasorSec = new System.Windows.Forms.Button();
            this.btnAcDosya = new System.Windows.Forms.Button();
            this.pnlKlasorYol = new System.Windows.Forms.Panel();
            this.lblKlasorBaslik = new System.Windows.Forms.Label();
            this.txtKlasorYol = new System.Windows.Forms.TextBox();
            this.btnKlasorGit = new System.Windows.Forms.Button();
            this.btnKlasorGozat = new System.Windows.Forms.Button();
            this.pnlAna = new System.Windows.Forms.Panel();
            this.pnlSol = new System.Windows.Forms.Panel();
            this.lbPozlar = new System.Windows.Forms.ListBox();
            this.lblKlasor = new System.Windows.Forms.Label();
            this.lblPozAdi = new System.Windows.Forms.Label();
            this.splitterSol = new System.Windows.Forms.Splitter();
            this.pnlMerkez = new System.Windows.Forms.Panel();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.pnlDosyaBilgi = new System.Windows.Forms.Panel();
            this.lblDosyaBilgi = new System.Windows.Forms.Label();
            this.splitterSag = new System.Windows.Forms.Splitter();
            this.pnlSag = new System.Windows.Forms.Panel();
            this.pnlRotaOzeti = new System.Windows.Forms.Panel();
            this.pnlAlt = new System.Windows.Forms.Panel();
            this.pnlRotaButonlar = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlNav = new System.Windows.Forms.Panel();
            this.lblAdet = new System.Windows.Forms.Label();
            this.lblRotaKodu = new System.Windows.Forms.Label();
            this.btnTumunuSec = new System.Windows.Forms.Button();
            this.btnHepsiniTemizle = new System.Windows.Forms.Button();
            this.btnGeri = new System.Windows.Forms.Button();
            this.lblIlerleme = new System.Windows.Forms.Label();
            this.btnIleri = new System.Windows.Forms.Button();
            this.btnYazdir = new System.Windows.Forms.Button();
            this.btnKaydetCikis = new System.Windows.Forms.Button();

            this.pnlUst.SuspendLayout();
            this.pnlKlasorYol.SuspendLayout();
            this.pnlAna.SuspendLayout();
            this.pnlSol.SuspendLayout();
            this.pnlMerkez.SuspendLayout();
            this.pnlDosyaBilgi.SuspendLayout();
            this.pnlSag.SuspendLayout();
            this.pnlAlt.SuspendLayout();
            this.pnlNav.SuspendLayout();
            this.SuspendLayout();

            // ── pnlUst ──────────────────────────────────────────────────
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(22, 22, 26);
            this.pnlUst.Controls.Add(this.btnAcDosya);
            this.pnlUst.Controls.Add(this.btnKlasorSec);
            this.pnlUst.Controls.Add(this.lblErpDosya);
            this.pnlUst.Controls.Add(this.lblBaslik);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Size = new System.Drawing.Size(1920, 56);

            this.lblBaslik.AutoSize = false;
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Size = new System.Drawing.Size(340, 56);
            this.lblBaslik.Text = "   İmalat Kartı & Rota";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblErpDosya.AutoSize = false;
            this.lblErpDosya.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblErpDosya.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblErpDosya.ForeColor = System.Drawing.Color.FromArgb(160, 210, 160);
            this.lblErpDosya.Size = new System.Drawing.Size(480, 56);
            this.lblErpDosya.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblErpDosya.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);

            BtnStyle(this.btnKlasorSec, "📁 Klasör Seç",
                System.Drawing.Color.FromArgb(52, 152, 219),
                System.Windows.Forms.DockStyle.Left, 155, 40);
            this.btnKlasorSec.Click += new System.EventHandler(this.btnKlasorSec_Click);

            BtnStyle(this.btnAcDosya, "↗ Dosyayı Aç",
                System.Drawing.Color.FromArgb(80, 80, 90),
                System.Windows.Forms.DockStyle.Left, 140, 40);
            this.btnAcDosya.Click += new System.EventHandler(this.btnAcDosya_Click);

            // ── pnlKlasorYol — klasör yolu giriş şeridi ─────────────────
            this.pnlKlasorYol.BackColor = System.Drawing.Color.FromArgb(18, 18, 22);
            this.pnlKlasorYol.Controls.Add(this.btnKlasorGit);
            this.pnlKlasorYol.Controls.Add(this.btnKlasorGozat);
            this.pnlKlasorYol.Controls.Add(this.txtKlasorYol);
            this.pnlKlasorYol.Controls.Add(this.lblKlasorBaslik);
            this.pnlKlasorYol.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKlasorYol.Size = new System.Drawing.Size(1920, 46);
            this.pnlKlasorYol.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);

            this.lblKlasorBaslik.AutoSize = false;
            this.lblKlasorBaslik.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblKlasorBaslik.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblKlasorBaslik.ForeColor = System.Drawing.Color.FromArgb(140, 160, 200);
            this.lblKlasorBaslik.Size = new System.Drawing.Size(160, 32);
            this.lblKlasorBaslik.Text = "  📂 İmalat Kartı Klasörü:";
            this.lblKlasorBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.txtKlasorYol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKlasorYol.Font = new System.Drawing.Font("Consolas", 11F);
            this.txtKlasorYol.BackColor = System.Drawing.Color.FromArgb(32, 32, 40);
            this.txtKlasorYol.ForeColor = System.Drawing.Color.FromArgb(200, 230, 255);
            this.txtKlasorYol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKlasorYol.TabIndex = 0;
            this.txtKlasorYol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKlasorYol_KeyDown);

            BtnStyle(this.btnKlasorGozat, "📂 Gözat",
                System.Drawing.Color.FromArgb(52, 100, 180),
                System.Windows.Forms.DockStyle.Right, 110, 32);
            this.btnKlasorGozat.Click += new System.EventHandler(this.btnKlasorGozat_Click);

            BtnStyle(this.btnKlasorGit, "▶ Git",
                System.Drawing.Color.FromArgb(0, 140, 100),
                System.Windows.Forms.DockStyle.Right, 90, 32);
            this.btnKlasorGit.Click += new System.EventHandler(this.btnKlasorGit_Click);

            // ── pnlAna ──────────────────────────────────────────────────
            this.pnlAna.Controls.Add(this.pnlMerkez);
            this.pnlAna.Controls.Add(this.splitterSag);
            this.pnlAna.Controls.Add(this.pnlSag);
            this.pnlAna.Controls.Add(this.splitterSol);
            this.pnlAna.Controls.Add(this.pnlSol);
            this.pnlAna.Dock = System.Windows.Forms.DockStyle.Fill;

            // ── pnlSol ──────────────────────────────────────────────────
            this.pnlSol.BackColor = System.Drawing.Color.FromArgb(30, 30, 36);
            this.pnlSol.Controls.Add(this.lbPozlar);
            this.pnlSol.Controls.Add(this.lblKlasor);
            this.pnlSol.Controls.Add(this.lblPozAdi);
            this.pnlSol.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSol.Padding = new System.Windows.Forms.Padding(6);
            this.pnlSol.Size = new System.Drawing.Size(300, 960);

            this.lblPozAdi.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPozAdi.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPozAdi.ForeColor = System.Drawing.Color.FromArgb(100, 180, 255);
            this.lblPozAdi.Size = new System.Drawing.Size(288, 36);
            this.lblPozAdi.Text = "Poz seçiniz...";
            this.lblPozAdi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblKlasor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblKlasor.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblKlasor.ForeColor = System.Drawing.Color.FromArgb(120, 120, 130);
            this.lblKlasor.Size = new System.Drawing.Size(288, 24);
            this.lblKlasor.Text = "Klasör seçilmedi";

            this.lbPozlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPozlar.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.lbPozlar.ItemHeight = 22;
            this.lbPozlar.BackColor = System.Drawing.Color.FromArgb(28, 28, 34);
            this.lbPozlar.ForeColor = System.Drawing.Color.FromArgb(200, 200, 210);
            this.lbPozlar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbPozlar.SelectedIndexChanged +=
                new System.EventHandler(this.lbPozlar_SelectedIndexChanged);

            // ── splitterSol ──────────────────────────────────────────────
            this.splitterSol.BackColor = System.Drawing.Color.FromArgb(50, 50, 60);
            this.splitterSol.Size = new System.Drawing.Size(4, 960);
            this.splitterSol.TabStop = false;

            // ── pnlSag ──────────────────────────────────────────────────
            this.pnlSag.BackColor = System.Drawing.Color.FromArgb(26, 26, 32);
            this.pnlSag.Controls.Add(this.pnlRotaOzeti);
            this.pnlSag.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSag.Padding = new System.Windows.Forms.Padding(6);
            this.pnlSag.Size = new System.Drawing.Size(180, 960);

            this.pnlRotaOzeti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRotaOzeti.BackColor = System.Drawing.Color.FromArgb(26, 26, 32);
            this.pnlRotaOzeti.AutoScroll = false;
            this.pnlRotaOzeti.TabIndex = 0;

            // ── splitterSag ──────────────────────────────────────────────
            this.splitterSag.BackColor = System.Drawing.Color.FromArgb(50, 50, 60);
            this.splitterSag.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterSag.Size = new System.Drawing.Size(4, 960);
            this.splitterSag.TabStop = false;

            // ── pnlMerkez ───────────────────────────────────────────────
            this.pnlMerkez.BackColor = System.Drawing.Color.Black;
            this.pnlMerkez.Controls.Add(this.webBrowser);
            this.pnlMerkez.Controls.Add(this.pnlDosyaBilgi);
            this.pnlMerkez.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlDosyaBilgi.BackColor = System.Drawing.Color.FromArgb(30, 30, 36);
            this.pnlDosyaBilgi.Controls.Add(this.lblDosyaBilgi);
            this.pnlDosyaBilgi.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDosyaBilgi.Size = new System.Drawing.Size(1000, 30);
            this.pnlDosyaBilgi.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);

            this.lblDosyaBilgi.AutoSize = false;
            this.lblDosyaBilgi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDosyaBilgi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDosyaBilgi.ForeColor = System.Drawing.Color.FromArgb(160, 200, 160);
            this.lblDosyaBilgi.Text = "";
            this.lblDosyaBilgi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.ScriptErrorsSuppressed = true;

            // ── pnlAlt ──────────────────────────────────────────────────
            this.pnlAlt.BackColor = System.Drawing.Color.FromArgb(28, 28, 34);
            this.pnlAlt.Controls.Add(this.pnlRotaButonlar);
            this.pnlAlt.Controls.Add(this.pnlNav);
            this.pnlAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlt.Size = new System.Drawing.Size(1920, 160);
            this.pnlAlt.Padding = new System.Windows.Forms.Padding(6, 4, 6, 0);

            this.pnlRotaButonlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRotaButonlar.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlRotaButonlar.AutoSize = false;
            this.pnlRotaButonlar.WrapContents = false;
            this.pnlRotaButonlar.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);

            // ── pnlNav ──────────────────────────────────────────────────
            this.pnlNav.BackColor = System.Drawing.Color.FromArgb(22, 22, 28);
            this.pnlNav.Controls.Add(this.btnKaydetCikis);
            this.pnlNav.Controls.Add(this.btnYazdir);
            this.pnlNav.Controls.Add(this.btnIleri);
            this.pnlNav.Controls.Add(this.lblIlerleme);
            this.pnlNav.Controls.Add(this.btnGeri);
            this.pnlNav.Controls.Add(this.btnHepsiniTemizle);
            this.pnlNav.Controls.Add(this.btnTumunuSec);
            this.pnlNav.Controls.Add(this.lblRotaKodu);
            this.pnlNav.Controls.Add(this.lblAdet);
            this.pnlNav.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlNav.Size = new System.Drawing.Size(1920, 54);
            this.pnlNav.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);

            this.lblAdet.AutoSize = false;
            this.lblAdet.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAdet.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAdet.ForeColor = System.Drawing.Color.FromArgb(255, 220, 80);
            this.lblAdet.Size = new System.Drawing.Size(300, 44);
            this.lblAdet.Text = "—";
            this.lblAdet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblRotaKodu.AutoSize = false;
            this.lblRotaKodu.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblRotaKodu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRotaKodu.ForeColor = System.Drawing.Color.FromArgb(252, 243, 207);
            this.lblRotaKodu.Size = new System.Drawing.Size(400, 44);
            this.lblRotaKodu.Text = "Rota: (seçilmedi)";
            this.lblRotaKodu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            BtnStyle(this.btnTumunuSec, "✓ Tümü",
                System.Drawing.Color.FromArgb(39, 174, 96),
                System.Windows.Forms.DockStyle.Left, 110, 40);
            this.btnTumunuSec.Click += new System.EventHandler(this.btnTumunuSec_Click);

            BtnStyle(this.btnHepsiniTemizle, "✗ Temizle",
                System.Drawing.Color.FromArgb(150, 60, 60),
                System.Windows.Forms.DockStyle.Left, 110, 40);
            this.btnHepsiniTemizle.Click += new System.EventHandler(this.btnHepsiniTemizle_Click);

            BtnStyle(this.btnGeri, "◄ Geri",
                System.Drawing.Color.FromArgb(70, 70, 80),
                System.Windows.Forms.DockStyle.Left, 100, 40);
            this.btnGeri.Click += new System.EventHandler(this.btnGeri_Click);

            this.lblIlerleme.AutoSize = false;
            this.lblIlerleme.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblIlerleme.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblIlerleme.ForeColor = System.Drawing.Color.White;
            this.lblIlerleme.Size = new System.Drawing.Size(130, 44);
            this.lblIlerleme.Text = "0 / 0 poz";
            this.lblIlerleme.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            BtnStyle(this.btnIleri, "İleri ►",
                System.Drawing.Color.FromArgb(70, 70, 80),
                System.Windows.Forms.DockStyle.Left, 100, 40);
            this.btnIleri.Click += new System.EventHandler(this.btnIleri_Click);

            BtnStyle(this.btnYazdir, "🖨  İmalat Kartlarını Yazdır",
                System.Drawing.Color.FromArgb(120, 60, 160),
                System.Windows.Forms.DockStyle.Right, 260, 40);
            this.btnYazdir.Click += new System.EventHandler(this.btnYazdir_Click);

            BtnStyle(this.btnKaydetCikis, "💾 Kaydet & Çıkış",
                System.Drawing.Color.FromArgb(0, 150, 136),
                System.Windows.Forms.DockStyle.Right, 210, 40);
            this.btnKaydetCikis.Click += new System.EventHandler(this.btnKaydetCikis_Click);

            // ── Form ────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 36);
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.pnlAna);
            this.Controls.Add(this.pnlAlt);
            this.Controls.Add(this.pnlKlasorYol);
            this.Controls.Add(this.pnlUst);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.Name = "FrmImalatKartiRota";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "İmalat Kartı & Rota Girişi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmImalatKartiRota_Load);

            this.pnlUst.ResumeLayout(false);
            this.pnlKlasorYol.ResumeLayout(false);
            this.pnlAna.ResumeLayout(false);
            this.pnlSol.ResumeLayout(false);
            this.pnlMerkez.ResumeLayout(false);
            this.pnlDosyaBilgi.ResumeLayout(false);
            this.pnlSag.ResumeLayout(false);
            this.pnlAlt.ResumeLayout(false);
            this.pnlNav.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void BtnStyle(System.Windows.Forms.Button b, string text,
            System.Drawing.Color color, System.Windows.Forms.DockStyle dock, int w, int h)
        {
            b.Text = text;
            b.BackColor = color;
            b.ForeColor = System.Drawing.Color.White;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            b.Cursor = System.Windows.Forms.Cursors.Hand;
            b.Dock = dock;
            b.Size = new System.Drawing.Size(w, h);
            b.UseVisualStyleBackColor = false;
        }

        // ── Fields ───────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlUst;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Label lblErpDosya;
        private System.Windows.Forms.Button btnKlasorSec;
        private System.Windows.Forms.Button btnAcDosya;
        private System.Windows.Forms.Panel pnlKlasorYol;
        private System.Windows.Forms.Label lblKlasorBaslik;
        private System.Windows.Forms.TextBox txtKlasorYol;
        private System.Windows.Forms.Button btnKlasorGozat;
        private System.Windows.Forms.Button btnKlasorGit;
        private System.Windows.Forms.Panel pnlAna;
        private System.Windows.Forms.Panel pnlSol;
        private System.Windows.Forms.ListBox lbPozlar;
        private System.Windows.Forms.Label lblKlasor;
        private System.Windows.Forms.Label lblPozAdi;
        private System.Windows.Forms.Splitter splitterSol;
        private System.Windows.Forms.Panel pnlMerkez;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Panel pnlDosyaBilgi;
        private System.Windows.Forms.Label lblDosyaBilgi;
        private System.Windows.Forms.Splitter splitterSag;
        private System.Windows.Forms.Panel pnlSag;
        private System.Windows.Forms.Panel pnlRotaOzeti;
        private System.Windows.Forms.Panel pnlAlt;
        private System.Windows.Forms.FlowLayoutPanel pnlRotaButonlar;
        private System.Windows.Forms.Panel pnlNav;
        private System.Windows.Forms.Label lblAdet;
        private System.Windows.Forms.Label lblRotaKodu;
        private System.Windows.Forms.Button btnTumunuSec;
        private System.Windows.Forms.Button btnHepsiniTemizle;
        private System.Windows.Forms.Button btnGeri;
        private System.Windows.Forms.Label lblIlerleme;
        private System.Windows.Forms.Button btnIleri;
        private System.Windows.Forms.Button btnYazdir;
        private System.Windows.Forms.Button btnKaydetCikis;
    }
}