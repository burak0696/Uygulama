namespace BARAN
{
    partial class MainMenu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnCikis = new System.Windows.Forms.Button();
            this.lblSidebarInfo = new System.Windows.Forms.Label();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTarih = new System.Windows.Forms.Label();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCardUrun = new System.Windows.Forms.Button();
            this.btnCardMusteri = new System.Windows.Forms.Button();
            this.btnCardGalvaniz = new System.Windows.Forms.Button();
            this.btnCardSarf = new System.Windows.Forms.Button();
            this.btnCardTeklif = new System.Windows.Forms.Button();
            this.btnCardBakimOnarim = new System.Windows.Forms.Button();
            this.btnCardUretim = new System.Windows.Forms.Button();
            this.btnCardIK = new System.Windows.Forms.Button();
            this.btnCardDonusturucu = new System.Windows.Forms.Button();
            this.btnCardImalatPlanlama = new System.Windows.Forms.Button();
            this.btnCardGalvanizKontrol = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblFooter = new System.Windows.Forms.Label();
            this.pnlSidebar.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(36)))), ((int)(((byte)(40)))));
            this.pnlSidebar.Controls.Add(this.btnCikis);
            this.pnlSidebar.Controls.Add(this.lblSidebarInfo);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(230, 700);
            this.pnlSidebar.TabIndex = 0;
            // 
            // btnCikis
            // 
            this.btnCikis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnCikis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCikis.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCikis.FlatAppearance.BorderSize = 0;
            this.btnCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCikis.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCikis.ForeColor = System.Drawing.Color.White;
            this.btnCikis.Location = new System.Drawing.Point(0, 595);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(230, 55);
            this.btnCikis.TabIndex = 5;
            this.btnCikis.Text = "🚪  ÇIKIŞ";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // lblSidebarInfo
            // 
            this.lblSidebarInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSidebarInfo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSidebarInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lblSidebarInfo.Location = new System.Drawing.Point(0, 650);
            this.lblSidebarInfo.Name = "lblSidebarInfo";
            this.lblSidebarInfo.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblSidebarInfo.Size = new System.Drawing.Size(230, 50);
            this.lblSidebarInfo.TabIndex = 6;
            this.lblSidebarInfo.Text = "BARAN ÇELİK\nSürüm 0.0.2";
            this.lblSidebarInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlLogo.Controls.Add(this.lblLogo);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(230, 110);
            this.pnlLogo.TabIndex = 0;
            // 
            // lblLogo
            // 
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(230, 110);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "BARAN\nÇELİK & GALVANİZ";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblTarih);
            this.pnlHeader.Controls.Add(this.lblBaslik);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(230, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(920, 70);
            this.pnlHeader.TabIndex = 1;
            // 
            // lblTarih
            // 
            this.lblTarih.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTarih.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTarih.ForeColor = System.Drawing.Color.Gray;
            this.lblTarih.Location = new System.Drawing.Point(670, 22);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(240, 25);
            this.lblTarih.TabIndex = 1;
            this.lblTarih.Text = "24 Şubat 2026, Salı";
            this.lblTarih.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBaslik
            // 
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(36)))), ((int)(((byte)(40)))));
            this.lblBaslik.Location = new System.Drawing.Point(25, 17);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(177, 32);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "Kontrol Paneli";
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.pnlContainer.Controls.Add(this.flowLayoutPanel1);
            this.pnlContainer.Controls.Add(this.pnlFooter);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(230, 70);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(25, 25, 25, 10);
            this.pnlContainer.Size = new System.Drawing.Size(920, 630);
            this.pnlContainer.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnCardUrun);
            this.flowLayoutPanel1.Controls.Add(this.btnCardMusteri);
            this.flowLayoutPanel1.Controls.Add(this.btnCardGalvaniz);
            this.flowLayoutPanel1.Controls.Add(this.btnCardSarf);
            this.flowLayoutPanel1.Controls.Add(this.btnCardTeklif);
            this.flowLayoutPanel1.Controls.Add(this.btnCardBakimOnarim);
            this.flowLayoutPanel1.Controls.Add(this.btnCardUretim);
            this.flowLayoutPanel1.Controls.Add(this.btnCardIK);
            this.flowLayoutPanel1.Controls.Add(this.btnCardDonusturucu);
            this.flowLayoutPanel1.Controls.Add(this.btnCardImalatPlanlama);
            this.flowLayoutPanel1.Controls.Add(this.btnCardGalvanizKontrol);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(25, 25);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(870, 555);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnCardUrun
            // 
            this.btnCardUrun.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardUrun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardUrun.FlatAppearance.BorderSize = 0;
            this.btnCardUrun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardUrun.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardUrun.ForeColor = System.Drawing.Color.White;
            this.btnCardUrun.Location = new System.Drawing.Point(15, 15);
            this.btnCardUrun.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardUrun.Name = "btnCardUrun";
            this.btnCardUrun.Size = new System.Drawing.Size(265, 85);
            this.btnCardUrun.TabIndex = 0;
            this.btnCardUrun.Text = "🔍\r\nÜRÜN ARAMA";
            this.btnCardUrun.UseVisualStyleBackColor = false;
            this.btnCardUrun.Click += new System.EventHandler(this.btnUrunAra_Click);
            // 
            // btnCardMusteri
            // 
            this.btnCardMusteri.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardMusteri.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardMusteri.FlatAppearance.BorderSize = 0;
            this.btnCardMusteri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardMusteri.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardMusteri.ForeColor = System.Drawing.Color.White;
            this.btnCardMusteri.Location = new System.Drawing.Point(300, 15);
            this.btnCardMusteri.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardMusteri.Name = "btnCardMusteri";
            this.btnCardMusteri.Size = new System.Drawing.Size(265, 85);
            this.btnCardMusteri.TabIndex = 1;
            this.btnCardMusteri.Text = "👥\r\nMÜŞTERİ PANELİ";
            this.btnCardMusteri.UseVisualStyleBackColor = false;
            this.btnCardMusteri.Click += new System.EventHandler(this.btnMusteri_Click);
            // 
            // btnCardGalvaniz
            // 
            this.btnCardGalvaniz.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardGalvaniz.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardGalvaniz.FlatAppearance.BorderSize = 0;
            this.btnCardGalvaniz.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardGalvaniz.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardGalvaniz.ForeColor = System.Drawing.Color.White;
            this.btnCardGalvaniz.Location = new System.Drawing.Point(585, 15);
            this.btnCardGalvaniz.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardGalvaniz.Name = "btnCardGalvaniz";
            this.btnCardGalvaniz.Size = new System.Drawing.Size(265, 85);
            this.btnCardGalvaniz.TabIndex = 2;
            this.btnCardGalvaniz.Text = "🏭\r\nGALVANİZ TEKLİF";
            this.btnCardGalvaniz.UseVisualStyleBackColor = false;
            this.btnCardGalvaniz.Click += new System.EventHandler(this.btnGalvaniz_Click);
            // 
            // btnCardSarf
            // 
            this.btnCardSarf.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardSarf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardSarf.FlatAppearance.BorderSize = 0;
            this.btnCardSarf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardSarf.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardSarf.ForeColor = System.Drawing.Color.White;
            this.btnCardSarf.Location = new System.Drawing.Point(15, 120);
            this.btnCardSarf.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardSarf.Name = "btnCardSarf";
            this.btnCardSarf.Size = new System.Drawing.Size(265, 85);
            this.btnCardSarf.TabIndex = 3;
            this.btnCardSarf.Text = "📦\r\nSATIN ALMA";
            this.btnCardSarf.UseVisualStyleBackColor = false;
            this.btnCardSarf.Click += new System.EventHandler(this.btnSatinalma_Click);
            // 
            // btnCardTeklif
            // 
            this.btnCardTeklif.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardTeklif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardTeklif.FlatAppearance.BorderSize = 0;
            this.btnCardTeklif.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardTeklif.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardTeklif.ForeColor = System.Drawing.Color.White;
            this.btnCardTeklif.Location = new System.Drawing.Point(300, 120);
            this.btnCardTeklif.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardTeklif.Name = "btnCardTeklif";
            this.btnCardTeklif.Size = new System.Drawing.Size(265, 85);
            this.btnCardTeklif.TabIndex = 4;
            this.btnCardTeklif.Text = "✉\r\nTEKLİF";
            this.btnCardTeklif.UseVisualStyleBackColor = false;
            this.btnCardTeklif.Click += new System.EventHandler(this.btnTeklif_Click);
            // 
            // btnCardBakimOnarim
            // 
            this.btnCardBakimOnarim.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardBakimOnarim.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardBakimOnarim.FlatAppearance.BorderSize = 0;
            this.btnCardBakimOnarim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardBakimOnarim.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardBakimOnarim.ForeColor = System.Drawing.Color.White;
            this.btnCardBakimOnarim.Location = new System.Drawing.Point(585, 120);
            this.btnCardBakimOnarim.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardBakimOnarim.Name = "btnCardBakimOnarim";
            this.btnCardBakimOnarim.Size = new System.Drawing.Size(265, 85);
            this.btnCardBakimOnarim.TabIndex = 5;
            this.btnCardBakimOnarim.Text = "🔧\r\nBAKIM / ONARIM";
            this.btnCardBakimOnarim.UseVisualStyleBackColor = false;
            this.btnCardBakimOnarim.Click += new System.EventHandler(this.btnBakimOnarim_Click);
            // 
            // btnCardUretim
            // 
            this.btnCardUretim.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardUretim.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardUretim.FlatAppearance.BorderSize = 0;
            this.btnCardUretim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardUretim.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardUretim.ForeColor = System.Drawing.Color.White;
            this.btnCardUretim.Location = new System.Drawing.Point(15, 225);
            this.btnCardUretim.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardUretim.Name = "btnCardUretim";
            this.btnCardUretim.Size = new System.Drawing.Size(265, 85);
            this.btnCardUretim.TabIndex = 6;
            this.btnCardUretim.Text = "⚙\r\nÜRETİM";
            this.btnCardUretim.UseVisualStyleBackColor = false;
            this.btnCardUretim.Click += new System.EventHandler(this.btnUretim_Click);
            // 
            // btnCardIK
            // 
            this.btnCardIK.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardIK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardIK.FlatAppearance.BorderSize = 0;
            this.btnCardIK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardIK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardIK.ForeColor = System.Drawing.Color.White;
            this.btnCardIK.Location = new System.Drawing.Point(300, 225);
            this.btnCardIK.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardIK.Name = "btnCardIK";
            this.btnCardIK.Size = new System.Drawing.Size(265, 85);
            this.btnCardIK.TabIndex = 7;
            this.btnCardIK.Text = "👤\r\nİNSAN KAYNAKLARI";
            this.btnCardIK.UseVisualStyleBackColor = false;
            this.btnCardIK.Click += new System.EventHandler(this.btnIK_Click);
            // 
            // btnCardDonusturucu
            // 
            this.btnCardDonusturucu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnCardDonusturucu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardDonusturucu.FlatAppearance.BorderSize = 0;
            this.btnCardDonusturucu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardDonusturucu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardDonusturucu.ForeColor = System.Drawing.Color.White;
            this.btnCardDonusturucu.Location = new System.Drawing.Point(585, 225);
            this.btnCardDonusturucu.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardDonusturucu.Name = "btnCardDonusturucu";
            this.btnCardDonusturucu.Size = new System.Drawing.Size(265, 85);
            this.btnCardDonusturucu.TabIndex = 8;
            this.btnCardDonusturucu.Text = "📄\r\nDÖNÜŞTÜRÜCÜ";
            this.btnCardDonusturucu.UseVisualStyleBackColor = false;
            this.btnCardDonusturucu.Click += new System.EventHandler(this.btnDonusturucu_Click);
            // 
            // btnCardImalatPlanlama
            // 
            this.btnCardImalatPlanlama.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCardImalatPlanlama.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardImalatPlanlama.FlatAppearance.BorderSize = 0;
            this.btnCardImalatPlanlama.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardImalatPlanlama.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardImalatPlanlama.ForeColor = System.Drawing.Color.White;
            this.btnCardImalatPlanlama.Location = new System.Drawing.Point(15, 330);
            this.btnCardImalatPlanlama.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardImalatPlanlama.Name = "btnCardImalatPlanlama";
            this.btnCardImalatPlanlama.Size = new System.Drawing.Size(265, 85);
            this.btnCardImalatPlanlama.TabIndex = 9;
            this.btnCardImalatPlanlama.Text = "📋\r\nİMALAT PLANLAMA";
            this.btnCardImalatPlanlama.UseVisualStyleBackColor = false;
            this.btnCardImalatPlanlama.Click += new System.EventHandler(this.btnImalatPlanlama_Click);
            // 
            // btnCardGalvanizKontrol
            // 
            this.btnCardGalvanizKontrol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.btnCardGalvanizKontrol.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCardGalvanizKontrol.FlatAppearance.BorderSize = 0;
            this.btnCardGalvanizKontrol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCardGalvanizKontrol.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCardGalvanizKontrol.ForeColor = System.Drawing.Color.White;
            this.btnCardGalvanizKontrol.Location = new System.Drawing.Point(300, 330);
            this.btnCardGalvanizKontrol.Margin = new System.Windows.Forms.Padding(10);
            this.btnCardGalvanizKontrol.Name = "btnCardGalvanizKontrol";
            this.btnCardGalvanizKontrol.Size = new System.Drawing.Size(265, 85);
            this.btnCardGalvanizKontrol.TabIndex = 10;
            this.btnCardGalvanizKontrol.Text = "🧪\r\nGALVANİZ KONTROL";
            this.btnCardGalvanizKontrol.UseVisualStyleBackColor = false;
            this.btnCardGalvanizKontrol.Visible = false;
            this.btnCardGalvanizKontrol.Click += new System.EventHandler(this.btnGalvanizKontrol_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Yellow;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(585, 330);
            this.button1.Margin = new System.Windows.Forms.Padding(10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(265, 85);
            this.button1.TabIndex = 11;
            this.button1.Text = "🧪\r\nGALVANİZ TEKLİF";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.lblFooter);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(25, 580);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(870, 40);
            this.pnlFooter.TabIndex = 1;
            // 
            // lblFooter
            // 
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFooter.ForeColor = System.Drawing.Color.Gray;
            this.lblFooter.Location = new System.Drawing.Point(0, 0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(870, 40);
            this.lblFooter.TabIndex = 0;
            this.lblFooter.Text = "© 2025 Baran Çelik ve Galvaniz San. Tic. Ltd. Şti. - Tüm Hakları Saklıdır.";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1150, 700);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.MinimumSize = new System.Drawing.Size(1050, 650);
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BARAN ÇELİK VE GALVANİZ SANAYİ LİMİTED ŞİRKETİ";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlContainer.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.Label lblSidebarInfo;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCardUrun;
        private System.Windows.Forms.Button btnCardMusteri;
        private System.Windows.Forms.Button btnCardGalvaniz;
        private System.Windows.Forms.Button btnCardSarf;
        private System.Windows.Forms.Button btnCardTeklif;
        private System.Windows.Forms.Button btnCardBakimOnarim;
        private System.Windows.Forms.Button btnCardUretim;
        private System.Windows.Forms.Button btnCardIK;
        private System.Windows.Forms.Button btnCardDonusturucu;
        private System.Windows.Forms.Button btnCardImalatPlanlama;
        private System.Windows.Forms.Button btnCardGalvanizKontrol;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.Button button1;
    }
}