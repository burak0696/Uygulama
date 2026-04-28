namespace BaranYardimci
{
    partial class GalvanizKontrol
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabBekleyen = new System.Windows.Forms.TabPage();
            this.dgvBekleyen = new System.Windows.Forms.DataGridView();
            this.pnlBekleyenAlt = new System.Windows.Forms.Panel();
            this.btnBekleyenTemizle = new System.Windows.Forms.Button();
            this.txtBekleyenAra = new System.Windows.Forms.TextBox();
            this.lblBekleyenAra = new System.Windows.Forms.Label();
            this.btnBekleyenGeciken = new System.Windows.Forms.Button();
            this.btnBekleyenAcil = new System.Windows.Forms.Button();
            this.btnBekleyenTumu = new System.Windows.Forms.Button();
            this.pnlBekleyenButon = new System.Windows.Forms.Panel();
            this.btnGalvanizeAl = new System.Windows.Forms.Button();
            this.btnTumunuSec = new System.Windows.Forms.Button();
            this.tabGalvanizde = new System.Windows.Forms.TabPage();
            this.dgvGalvanizde = new System.Windows.Forms.DataGridView();
            this.pnlGalvanizdeAlt = new System.Windows.Forms.Panel();
            this.btnGalvanizdeTemizle = new System.Windows.Forms.Button();
            this.txtGalvanizdeAra = new System.Windows.Forms.TextBox();
            this.lblGalvanizdeAra = new System.Windows.Forms.Label();
            this.btnGalvanizdeGeciken = new System.Windows.Forms.Button();
            this.btnGalvanizdeAcil = new System.Windows.Forms.Button();
            this.btnGalvanizdeTumu = new System.Windows.Forms.Button();
            this.pnlGalvanizdeButon = new System.Windows.Forms.Panel();
            this.btnKontrolsuzGecis = new System.Windows.Forms.Button();
            this.btnGalvanizdenCikart = new System.Windows.Forms.Button();
            this.tabBiten = new System.Windows.Forms.TabPage();
            this.dgvBiten = new System.Windows.Forms.DataGridView();
            this.pnlBitenAlt = new System.Windows.Forms.Panel();
            this.btnBitenTemizle = new System.Windows.Forms.Button();
            this.txtBitenAra = new System.Windows.Forms.TextBox();
            this.lblBitenAra = new System.Windows.Forms.Label();
            this.pnlBitenButon = new System.Windows.Forms.Panel();
            this.btnPdfCikti = new System.Windows.Forms.Button();
            this.pnlAlt = new System.Windows.Forms.Panel();
            this.btnYenile = new System.Windows.Forms.Button();
            this.pnlUst.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabBekleyen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBekleyen)).BeginInit();
            this.pnlBekleyenAlt.SuspendLayout();
            this.pnlBekleyenButon.SuspendLayout();
            this.tabGalvanizde.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGalvanizde)).BeginInit();
            this.pnlGalvanizdeAlt.SuspendLayout();
            this.pnlGalvanizdeButon.SuspendLayout();
            this.tabBiten.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBiten)).BeginInit();
            this.pnlBitenAlt.SuspendLayout();
            this.pnlBitenButon.SuspendLayout();
            this.pnlAlt.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUst
            // 
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(36)))), ((int)(((byte)(40)))));
            this.pnlUst.Controls.Add(this.lblBaslik);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Location = new System.Drawing.Point(0, 0);
            this.pnlUst.Name = "pnlUst";
            this.pnlUst.Size = new System.Drawing.Size(1100, 60);
            this.pnlUst.TabIndex = 2;
            // 
            // lblBaslik
            // 
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Location = new System.Drawing.Point(0, 0);
            this.lblBaslik.Name = "lblBaslik";
            this.lblBaslik.Size = new System.Drawing.Size(1100, 60);
            this.lblBaslik.TabIndex = 0;
            this.lblBaslik.Text = "  GALVANIZ KONTROL PANELI";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabBekleyen);
            this.tabControl.Controls.Add(this.tabGalvanizde);
            this.tabControl.Controls.Add(this.tabBiten);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.tabControl.Location = new System.Drawing.Point(0, 60);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1100, 540);
            this.tabControl.TabIndex = 0;
            // 
            // tabBekleyen
            // 
            this.tabBekleyen.BackColor = System.Drawing.Color.White;
            this.tabBekleyen.Controls.Add(this.dgvBekleyen);
            this.tabBekleyen.Controls.Add(this.pnlBekleyenAlt);
            this.tabBekleyen.Controls.Add(this.pnlBekleyenButon);
            this.tabBekleyen.Location = new System.Drawing.Point(4, 29);
            this.tabBekleyen.Name = "tabBekleyen";
            this.tabBekleyen.Padding = new System.Windows.Forms.Padding(10);
            this.tabBekleyen.Size = new System.Drawing.Size(1092, 507);
            this.tabBekleyen.TabIndex = 0;
            this.tabBekleyen.Text = "Galvaniz Bekleyen";
            // 
            // dgvBekleyen
            // 
            this.dgvBekleyen.AllowUserToAddRows = false;
            this.dgvBekleyen.AllowUserToDeleteRows = false;
            this.dgvBekleyen.AllowUserToResizeColumns = false;
            this.dgvBekleyen.AllowUserToResizeRows = false;
            this.dgvBekleyen.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBekleyen.BackgroundColor = System.Drawing.Color.White;
            this.dgvBekleyen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvBekleyen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBekleyen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvBekleyen.Location = new System.Drawing.Point(10, 10);
            this.dgvBekleyen.Name = "dgvBekleyen";
            this.dgvBekleyen.ReadOnly = true;
            this.dgvBekleyen.RowHeadersVisible = false;
            this.dgvBekleyen.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBekleyen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBekleyen.Size = new System.Drawing.Size(1072, 390);
            this.dgvBekleyen.TabIndex = 0;
            // 
            // pnlBekleyenAlt
            // 
            this.pnlBekleyenAlt.Controls.Add(this.btnBekleyenTemizle);
            this.pnlBekleyenAlt.Controls.Add(this.txtBekleyenAra);
            this.pnlBekleyenAlt.Controls.Add(this.lblBekleyenAra);
            this.pnlBekleyenAlt.Controls.Add(this.btnBekleyenGeciken);
            this.pnlBekleyenAlt.Controls.Add(this.btnBekleyenAcil);
            this.pnlBekleyenAlt.Controls.Add(this.btnBekleyenTumu);
            this.pnlBekleyenAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBekleyenAlt.Location = new System.Drawing.Point(10, 400);
            this.pnlBekleyenAlt.Name = "pnlBekleyenAlt";
            this.pnlBekleyenAlt.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlBekleyenAlt.Size = new System.Drawing.Size(1072, 42);
            this.pnlBekleyenAlt.TabIndex = 1;
            // 
            // btnBekleyenTemizle
            // 
            this.btnBekleyenTemizle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnBekleyenTemizle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBekleyenTemizle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBekleyenTemizle.FlatAppearance.BorderSize = 0;
            this.btnBekleyenTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBekleyenTemizle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBekleyenTemizle.ForeColor = System.Drawing.Color.White;
            this.btnBekleyenTemizle.Location = new System.Drawing.Point(255, 5);
            this.btnBekleyenTemizle.Name = "btnBekleyenTemizle";
            this.btnBekleyenTemizle.Size = new System.Drawing.Size(70, 37);
            this.btnBekleyenTemizle.TabIndex = 0;
            this.btnBekleyenTemizle.Text = "X";
            this.btnBekleyenTemizle.UseVisualStyleBackColor = false;
            this.btnBekleyenTemizle.Click += new System.EventHandler(this.btnBekleyenTemizle_Click);
            // 
            // txtBekleyenAra
            // 
            this.txtBekleyenAra.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtBekleyenAra.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBekleyenAra.Location = new System.Drawing.Point(75, 5);
            this.txtBekleyenAra.Name = "txtBekleyenAra";
            this.txtBekleyenAra.Size = new System.Drawing.Size(180, 25);
            this.txtBekleyenAra.TabIndex = 1;
            this.txtBekleyenAra.TextChanged += new System.EventHandler(this.txtBekleyenAra_TextChanged);
            // 
            // lblBekleyenAra
            // 
            this.lblBekleyenAra.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblBekleyenAra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBekleyenAra.Location = new System.Drawing.Point(0, 5);
            this.lblBekleyenAra.Name = "lblBekleyenAra";
            this.lblBekleyenAra.Size = new System.Drawing.Size(75, 37);
            this.lblBekleyenAra.TabIndex = 2;
            this.lblBekleyenAra.Text = "Poz Ara:";
            this.lblBekleyenAra.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBekleyenGeciken
            // 
            this.btnBekleyenGeciken.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnBekleyenGeciken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBekleyenGeciken.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBekleyenGeciken.FlatAppearance.BorderSize = 0;
            this.btnBekleyenGeciken.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBekleyenGeciken.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBekleyenGeciken.ForeColor = System.Drawing.Color.White;
            this.btnBekleyenGeciken.Location = new System.Drawing.Point(732, 5);
            this.btnBekleyenGeciken.Name = "btnBekleyenGeciken";
            this.btnBekleyenGeciken.Size = new System.Drawing.Size(120, 37);
            this.btnBekleyenGeciken.TabIndex = 3;
            this.btnBekleyenGeciken.Text = "Geciken";
            this.btnBekleyenGeciken.UseVisualStyleBackColor = false;
            this.btnBekleyenGeciken.Click += new System.EventHandler(this.btnBekleyenGeciken_Click);
            // 
            // btnBekleyenAcil
            // 
            this.btnBekleyenAcil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnBekleyenAcil.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBekleyenAcil.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBekleyenAcil.FlatAppearance.BorderSize = 0;
            this.btnBekleyenAcil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBekleyenAcil.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBekleyenAcil.ForeColor = System.Drawing.Color.White;
            this.btnBekleyenAcil.Location = new System.Drawing.Point(852, 5);
            this.btnBekleyenAcil.Name = "btnBekleyenAcil";
            this.btnBekleyenAcil.Size = new System.Drawing.Size(120, 37);
            this.btnBekleyenAcil.TabIndex = 4;
            this.btnBekleyenAcil.Text = "Acil (0-7 gun)";
            this.btnBekleyenAcil.UseVisualStyleBackColor = false;
            this.btnBekleyenAcil.Click += new System.EventHandler(this.btnBekleyenAcil_Click);
            // 
            // btnBekleyenTumu
            // 
            this.btnBekleyenTumu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnBekleyenTumu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBekleyenTumu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBekleyenTumu.FlatAppearance.BorderSize = 0;
            this.btnBekleyenTumu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBekleyenTumu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBekleyenTumu.ForeColor = System.Drawing.Color.White;
            this.btnBekleyenTumu.Location = new System.Drawing.Point(972, 5);
            this.btnBekleyenTumu.Name = "btnBekleyenTumu";
            this.btnBekleyenTumu.Size = new System.Drawing.Size(100, 37);
            this.btnBekleyenTumu.TabIndex = 5;
            this.btnBekleyenTumu.Text = "Tumu";
            this.btnBekleyenTumu.UseVisualStyleBackColor = false;
            this.btnBekleyenTumu.Click += new System.EventHandler(this.btnBekleyenTumu_Click);
            // 
            // pnlBekleyenButon
            // 
            this.pnlBekleyenButon.Controls.Add(this.btnGalvanizeAl);
            this.pnlBekleyenButon.Controls.Add(this.btnTumunuSec);
            this.pnlBekleyenButon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBekleyenButon.Location = new System.Drawing.Point(10, 442);
            this.pnlBekleyenButon.Name = "pnlBekleyenButon";
            this.pnlBekleyenButon.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlBekleyenButon.Size = new System.Drawing.Size(1072, 55);
            this.pnlBekleyenButon.TabIndex = 2;
            // 
            // btnGalvanizeAl
            // 
            this.btnGalvanizeAl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.btnGalvanizeAl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGalvanizeAl.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnGalvanizeAl.FlatAppearance.BorderSize = 0;
            this.btnGalvanizeAl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGalvanizeAl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGalvanizeAl.ForeColor = System.Drawing.Color.White;
            this.btnGalvanizeAl.Location = new System.Drawing.Point(180, 8);
            this.btnGalvanizeAl.Name = "btnGalvanizeAl";
            this.btnGalvanizeAl.Size = new System.Drawing.Size(220, 47);
            this.btnGalvanizeAl.TabIndex = 0;
            this.btnGalvanizeAl.Text = "Galvanize Al";
            this.btnGalvanizeAl.UseVisualStyleBackColor = false;
            this.btnGalvanizeAl.Click += new System.EventHandler(this.btnGalvanizeAl_Click);
            // 
            // btnTumunuSec
            // 
            this.btnTumunuSec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnTumunuSec.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTumunuSec.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTumunuSec.FlatAppearance.BorderSize = 0;
            this.btnTumunuSec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTumunuSec.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTumunuSec.ForeColor = System.Drawing.Color.White;
            this.btnTumunuSec.Location = new System.Drawing.Point(0, 8);
            this.btnTumunuSec.Name = "btnTumunuSec";
            this.btnTumunuSec.Size = new System.Drawing.Size(180, 47);
            this.btnTumunuSec.TabIndex = 1;
            this.btnTumunuSec.Text = "Tumunu Sec";
            this.btnTumunuSec.UseVisualStyleBackColor = false;
            this.btnTumunuSec.Click += new System.EventHandler(this.btnTumunuSec_Click);
            // 
            // tabGalvanizde
            // 
            this.tabGalvanizde.BackColor = System.Drawing.Color.White;
            this.tabGalvanizde.Controls.Add(this.dgvGalvanizde);
            this.tabGalvanizde.Controls.Add(this.pnlGalvanizdeAlt);
            this.tabGalvanizde.Controls.Add(this.pnlGalvanizdeButon);
            this.tabGalvanizde.Location = new System.Drawing.Point(4, 29);
            this.tabGalvanizde.Name = "tabGalvanizde";
            this.tabGalvanizde.Padding = new System.Windows.Forms.Padding(10);
            this.tabGalvanizde.Size = new System.Drawing.Size(1092, 507);
            this.tabGalvanizde.TabIndex = 1;
            this.tabGalvanizde.Text = "Galvanizde";
            // 
            // dgvGalvanizde
            // 
            this.dgvGalvanizde.AllowUserToAddRows = false;
            this.dgvGalvanizde.AllowUserToDeleteRows = false;
            this.dgvGalvanizde.AllowUserToResizeColumns = false;
            this.dgvGalvanizde.AllowUserToResizeRows = false;
            this.dgvGalvanizde.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGalvanizde.BackgroundColor = System.Drawing.Color.White;
            this.dgvGalvanizde.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGalvanizde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGalvanizde.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvGalvanizde.Location = new System.Drawing.Point(10, 10);
            this.dgvGalvanizde.Name = "dgvGalvanizde";
            this.dgvGalvanizde.ReadOnly = true;
            this.dgvGalvanizde.RowHeadersVisible = false;
            this.dgvGalvanizde.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGalvanizde.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGalvanizde.Size = new System.Drawing.Size(1072, 390);
            this.dgvGalvanizde.TabIndex = 0;
            // 
            // pnlGalvanizdeAlt
            // 
            this.pnlGalvanizdeAlt.Controls.Add(this.btnGalvanizdeTemizle);
            this.pnlGalvanizdeAlt.Controls.Add(this.txtGalvanizdeAra);
            this.pnlGalvanizdeAlt.Controls.Add(this.lblGalvanizdeAra);
            this.pnlGalvanizdeAlt.Controls.Add(this.btnGalvanizdeGeciken);
            this.pnlGalvanizdeAlt.Controls.Add(this.btnGalvanizdeAcil);
            this.pnlGalvanizdeAlt.Controls.Add(this.btnGalvanizdeTumu);
            this.pnlGalvanizdeAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlGalvanizdeAlt.Location = new System.Drawing.Point(10, 400);
            this.pnlGalvanizdeAlt.Name = "pnlGalvanizdeAlt";
            this.pnlGalvanizdeAlt.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlGalvanizdeAlt.Size = new System.Drawing.Size(1072, 42);
            this.pnlGalvanizdeAlt.TabIndex = 1;
            // 
            // btnGalvanizdeTemizle
            // 
            this.btnGalvanizdeTemizle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnGalvanizdeTemizle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGalvanizdeTemizle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnGalvanizdeTemizle.FlatAppearance.BorderSize = 0;
            this.btnGalvanizdeTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGalvanizdeTemizle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGalvanizdeTemizle.ForeColor = System.Drawing.Color.White;
            this.btnGalvanizdeTemizle.Location = new System.Drawing.Point(255, 5);
            this.btnGalvanizdeTemizle.Name = "btnGalvanizdeTemizle";
            this.btnGalvanizdeTemizle.Size = new System.Drawing.Size(70, 37);
            this.btnGalvanizdeTemizle.TabIndex = 0;
            this.btnGalvanizdeTemizle.Text = "X";
            this.btnGalvanizdeTemizle.UseVisualStyleBackColor = false;
            this.btnGalvanizdeTemizle.Click += new System.EventHandler(this.btnGalvanizdeTemizle_Click);
            // 
            // txtGalvanizdeAra
            // 
            this.txtGalvanizdeAra.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtGalvanizdeAra.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtGalvanizdeAra.Location = new System.Drawing.Point(75, 5);
            this.txtGalvanizdeAra.Name = "txtGalvanizdeAra";
            this.txtGalvanizdeAra.Size = new System.Drawing.Size(180, 25);
            this.txtGalvanizdeAra.TabIndex = 1;
            this.txtGalvanizdeAra.TextChanged += new System.EventHandler(this.txtGalvanizdeAra_TextChanged);
            // 
            // lblGalvanizdeAra
            // 
            this.lblGalvanizdeAra.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblGalvanizdeAra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGalvanizdeAra.Location = new System.Drawing.Point(0, 5);
            this.lblGalvanizdeAra.Name = "lblGalvanizdeAra";
            this.lblGalvanizdeAra.Size = new System.Drawing.Size(75, 37);
            this.lblGalvanizdeAra.TabIndex = 2;
            this.lblGalvanizdeAra.Text = "Poz Ara:";
            this.lblGalvanizdeAra.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnGalvanizdeGeciken
            // 
            this.btnGalvanizdeGeciken.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnGalvanizdeGeciken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGalvanizdeGeciken.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnGalvanizdeGeciken.FlatAppearance.BorderSize = 0;
            this.btnGalvanizdeGeciken.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGalvanizdeGeciken.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGalvanizdeGeciken.ForeColor = System.Drawing.Color.White;
            this.btnGalvanizdeGeciken.Location = new System.Drawing.Point(732, 5);
            this.btnGalvanizdeGeciken.Name = "btnGalvanizdeGeciken";
            this.btnGalvanizdeGeciken.Size = new System.Drawing.Size(120, 37);
            this.btnGalvanizdeGeciken.TabIndex = 3;
            this.btnGalvanizdeGeciken.Text = "Geciken";
            this.btnGalvanizdeGeciken.UseVisualStyleBackColor = false;
            this.btnGalvanizdeGeciken.Click += new System.EventHandler(this.btnGalvanizdeGeciken_Click);
            // 
            // btnGalvanizdeAcil
            // 
            this.btnGalvanizdeAcil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnGalvanizdeAcil.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGalvanizdeAcil.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnGalvanizdeAcil.FlatAppearance.BorderSize = 0;
            this.btnGalvanizdeAcil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGalvanizdeAcil.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGalvanizdeAcil.ForeColor = System.Drawing.Color.White;
            this.btnGalvanizdeAcil.Location = new System.Drawing.Point(852, 5);
            this.btnGalvanizdeAcil.Name = "btnGalvanizdeAcil";
            this.btnGalvanizdeAcil.Size = new System.Drawing.Size(120, 37);
            this.btnGalvanizdeAcil.TabIndex = 4;
            this.btnGalvanizdeAcil.Text = "Acil (0-7 gun)";
            this.btnGalvanizdeAcil.UseVisualStyleBackColor = false;
            this.btnGalvanizdeAcil.Click += new System.EventHandler(this.btnGalvanizdeAcil_Click);
            // 
            // btnGalvanizdeTumu
            // 
            this.btnGalvanizdeTumu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnGalvanizdeTumu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGalvanizdeTumu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnGalvanizdeTumu.FlatAppearance.BorderSize = 0;
            this.btnGalvanizdeTumu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGalvanizdeTumu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGalvanizdeTumu.ForeColor = System.Drawing.Color.White;
            this.btnGalvanizdeTumu.Location = new System.Drawing.Point(972, 5);
            this.btnGalvanizdeTumu.Name = "btnGalvanizdeTumu";
            this.btnGalvanizdeTumu.Size = new System.Drawing.Size(100, 37);
            this.btnGalvanizdeTumu.TabIndex = 5;
            this.btnGalvanizdeTumu.Text = "Tumu";
            this.btnGalvanizdeTumu.UseVisualStyleBackColor = false;
            this.btnGalvanizdeTumu.Click += new System.EventHandler(this.btnGalvanizdeTumu_Click);
            // 
            // pnlGalvanizdeButon
            // 
            this.pnlGalvanizdeButon.Controls.Add(this.btnKontrolsuzGecis);
            this.pnlGalvanizdeButon.Controls.Add(this.btnGalvanizdenCikart);
            this.pnlGalvanizdeButon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlGalvanizdeButon.Location = new System.Drawing.Point(10, 442);
            this.pnlGalvanizdeButon.Name = "pnlGalvanizdeButon";
            this.pnlGalvanizdeButon.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlGalvanizdeButon.Size = new System.Drawing.Size(1072, 55);
            this.pnlGalvanizdeButon.TabIndex = 2;
            // 
            // btnKontrolsuzGecis
            // 
            this.btnKontrolsuzGecis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.btnKontrolsuzGecis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKontrolsuzGecis.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnKontrolsuzGecis.FlatAppearance.BorderSize = 0;
            this.btnKontrolsuzGecis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKontrolsuzGecis.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnKontrolsuzGecis.ForeColor = System.Drawing.Color.White;
            this.btnKontrolsuzGecis.Location = new System.Drawing.Point(300, 8);
            this.btnKontrolsuzGecis.Name = "btnKontrolsuzGecis";
            this.btnKontrolsuzGecis.Size = new System.Drawing.Size(300, 47);
            this.btnKontrolsuzGecis.TabIndex = 0;
            this.btnKontrolsuzGecis.Text = "Kontrolsuz Gecis - Paketleme";
            this.btnKontrolsuzGecis.UseVisualStyleBackColor = false;
            this.btnKontrolsuzGecis.Click += new System.EventHandler(this.btnKontrolsuzGecis_Click);
            // 
            // btnGalvanizdenCikart
            // 
            this.btnGalvanizdenCikart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnGalvanizdenCikart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGalvanizdenCikart.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnGalvanizdenCikart.FlatAppearance.BorderSize = 0;
            this.btnGalvanizdenCikart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGalvanizdenCikart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGalvanizdenCikart.ForeColor = System.Drawing.Color.White;
            this.btnGalvanizdenCikart.Location = new System.Drawing.Point(0, 8);
            this.btnGalvanizdenCikart.Name = "btnGalvanizdenCikart";
            this.btnGalvanizdenCikart.Size = new System.Drawing.Size(300, 47);
            this.btnGalvanizdenCikart.TabIndex = 1;
            this.btnGalvanizdenCikart.Text = "Galvanizden Cikart - Kontrol Formu";
            this.btnGalvanizdenCikart.UseVisualStyleBackColor = false;
            this.btnGalvanizdenCikart.Click += new System.EventHandler(this.btnGalvanizdenCikart_Click);
            // 
            // tabBiten
            // 
            this.tabBiten.BackColor = System.Drawing.Color.White;
            this.tabBiten.Controls.Add(this.dgvBiten);
            this.tabBiten.Controls.Add(this.pnlBitenAlt);
            this.tabBiten.Controls.Add(this.pnlBitenButon);
            this.tabBiten.Location = new System.Drawing.Point(4, 29);
            this.tabBiten.Name = "tabBiten";
            this.tabBiten.Padding = new System.Windows.Forms.Padding(10);
            this.tabBiten.Size = new System.Drawing.Size(1092, 507);
            this.tabBiten.TabIndex = 2;
            this.tabBiten.Text = "Galvaniz Biten";
            // 
            // dgvBiten
            // 
            this.dgvBiten.AllowUserToAddRows = false;
            this.dgvBiten.AllowUserToDeleteRows = false;
            this.dgvBiten.AllowUserToResizeColumns = false;
            this.dgvBiten.AllowUserToResizeRows = false;
            this.dgvBiten.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBiten.BackgroundColor = System.Drawing.Color.White;
            this.dgvBiten.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvBiten.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBiten.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvBiten.Location = new System.Drawing.Point(10, 10);
            this.dgvBiten.Name = "dgvBiten";
            this.dgvBiten.ReadOnly = true;
            this.dgvBiten.RowHeadersVisible = false;
            this.dgvBiten.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvBiten.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBiten.Size = new System.Drawing.Size(1072, 390);
            this.dgvBiten.TabIndex = 0;
            // 
            // pnlBitenAlt
            // 
            this.pnlBitenAlt.Controls.Add(this.btnBitenTemizle);
            this.pnlBitenAlt.Controls.Add(this.txtBitenAra);
            this.pnlBitenAlt.Controls.Add(this.lblBitenAra);
            this.pnlBitenAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBitenAlt.Location = new System.Drawing.Point(10, 400);
            this.pnlBitenAlt.Name = "pnlBitenAlt";
            this.pnlBitenAlt.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlBitenAlt.Size = new System.Drawing.Size(1072, 42);
            this.pnlBitenAlt.TabIndex = 1;
            // 
            // btnBitenTemizle
            // 
            this.btnBitenTemizle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnBitenTemizle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBitenTemizle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBitenTemizle.FlatAppearance.BorderSize = 0;
            this.btnBitenTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBitenTemizle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBitenTemizle.ForeColor = System.Drawing.Color.White;
            this.btnBitenTemizle.Location = new System.Drawing.Point(255, 5);
            this.btnBitenTemizle.Name = "btnBitenTemizle";
            this.btnBitenTemizle.Size = new System.Drawing.Size(70, 37);
            this.btnBitenTemizle.TabIndex = 0;
            this.btnBitenTemizle.Text = "X";
            this.btnBitenTemizle.UseVisualStyleBackColor = false;
            this.btnBitenTemizle.Click += new System.EventHandler(this.btnBitenTemizle_Click);
            // 
            // txtBitenAra
            // 
            this.txtBitenAra.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtBitenAra.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBitenAra.Location = new System.Drawing.Point(75, 5);
            this.txtBitenAra.Name = "txtBitenAra";
            this.txtBitenAra.Size = new System.Drawing.Size(180, 25);
            this.txtBitenAra.TabIndex = 1;
            this.txtBitenAra.TextChanged += new System.EventHandler(this.txtBitenAra_TextChanged);
            // 
            // lblBitenAra
            // 
            this.lblBitenAra.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblBitenAra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBitenAra.Location = new System.Drawing.Point(0, 5);
            this.lblBitenAra.Name = "lblBitenAra";
            this.lblBitenAra.Size = new System.Drawing.Size(75, 37);
            this.lblBitenAra.TabIndex = 2;
            this.lblBitenAra.Text = "Poz Ara:";
            this.lblBitenAra.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBitenButon
            // 
            this.pnlBitenButon.Controls.Add(this.btnPdfCikti);
            this.pnlBitenButon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBitenButon.Location = new System.Drawing.Point(10, 442);
            this.pnlBitenButon.Name = "pnlBitenButon";
            this.pnlBitenButon.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlBitenButon.Size = new System.Drawing.Size(1072, 55);
            this.pnlBitenButon.TabIndex = 2;
            // 
            // btnPdfCikti
            // 
            this.btnPdfCikti.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnPdfCikti.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPdfCikti.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPdfCikti.FlatAppearance.BorderSize = 0;
            this.btnPdfCikti.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPdfCikti.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPdfCikti.ForeColor = System.Drawing.Color.White;
            this.btnPdfCikti.Location = new System.Drawing.Point(0, 8);
            this.btnPdfCikti.Name = "btnPdfCikti";
            this.btnPdfCikti.Size = new System.Drawing.Size(250, 47);
            this.btnPdfCikti.TabIndex = 0;
            this.btnPdfCikti.Text = "Kontrol Formu Yazdir";
            this.btnPdfCikti.UseVisualStyleBackColor = false;
            this.btnPdfCikti.Click += new System.EventHandler(this.btnPdfCikti_Click);
            // 
            // pnlAlt
            // 
            this.pnlAlt.Controls.Add(this.btnYenile);
            this.pnlAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlt.Location = new System.Drawing.Point(0, 600);
            this.pnlAlt.Name = "pnlAlt";
            this.pnlAlt.Padding = new System.Windows.Forms.Padding(10, 8, 10, 0);
            this.pnlAlt.Size = new System.Drawing.Size(1100, 50);
            this.pnlAlt.TabIndex = 1;
            // 
            // btnYenile
            // 
            this.btnYenile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnYenile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYenile.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnYenile.FlatAppearance.BorderSize = 0;
            this.btnYenile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYenile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnYenile.ForeColor = System.Drawing.Color.White;
            this.btnYenile.Location = new System.Drawing.Point(940, 8);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(150, 42);
            this.btnYenile.TabIndex = 0;
            this.btnYenile.Text = "Yenile";
            this.btnYenile.UseVisualStyleBackColor = false;
            this.btnYenile.Click += new System.EventHandler(this.btnYenile_Click);
            // 
            // GalvanizKontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlAlt);
            this.Controls.Add(this.pnlUst);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "GalvanizKontrol";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Galvaniz Kontrol Paneli";
            this.Load += new System.EventHandler(this.GalvanizKontrol_Load);
            this.pnlUst.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabBekleyen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBekleyen)).EndInit();
            this.pnlBekleyenAlt.ResumeLayout(false);
            this.pnlBekleyenAlt.PerformLayout();
            this.pnlBekleyenButon.ResumeLayout(false);
            this.tabGalvanizde.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGalvanizde)).EndInit();
            this.pnlGalvanizdeAlt.ResumeLayout(false);
            this.pnlGalvanizdeAlt.PerformLayout();
            this.pnlGalvanizdeButon.ResumeLayout(false);
            this.tabBiten.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBiten)).EndInit();
            this.pnlBitenAlt.ResumeLayout(false);
            this.pnlBitenAlt.PerformLayout();
            this.pnlBitenButon.ResumeLayout(false);
            this.pnlAlt.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Panel pnlUst;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBekleyen;
        private System.Windows.Forms.DataGridView dgvBekleyen;
        private System.Windows.Forms.Panel pnlBekleyenAlt;
        private System.Windows.Forms.Button btnBekleyenTemizle;
        private System.Windows.Forms.TextBox txtBekleyenAra;
        private System.Windows.Forms.Label lblBekleyenAra;
        private System.Windows.Forms.Button btnBekleyenGeciken;
        private System.Windows.Forms.Button btnBekleyenAcil;
        private System.Windows.Forms.Button btnBekleyenTumu;
        private System.Windows.Forms.Panel pnlBekleyenButon;
        private System.Windows.Forms.Button btnGalvanizeAl;
        private System.Windows.Forms.Button btnTumunuSec;
        private System.Windows.Forms.TabPage tabGalvanizde;
        private System.Windows.Forms.DataGridView dgvGalvanizde;
        private System.Windows.Forms.Panel pnlGalvanizdeAlt;
        private System.Windows.Forms.Button btnGalvanizdeTemizle;
        private System.Windows.Forms.TextBox txtGalvanizdeAra;
        private System.Windows.Forms.Label lblGalvanizdeAra;
        private System.Windows.Forms.Button btnGalvanizdeGeciken;
        private System.Windows.Forms.Button btnGalvanizdeAcil;
        private System.Windows.Forms.Button btnGalvanizdeTumu;
        private System.Windows.Forms.Panel pnlGalvanizdeButon;
        private System.Windows.Forms.Button btnGalvanizdenCikart;
        private System.Windows.Forms.Button btnKontrolsuzGecis;
        private System.Windows.Forms.TabPage tabBiten;
        private System.Windows.Forms.DataGridView dgvBiten;
        private System.Windows.Forms.Panel pnlBitenAlt;
        private System.Windows.Forms.Button btnBitenTemizle;
        private System.Windows.Forms.TextBox txtBitenAra;
        private System.Windows.Forms.Label lblBitenAra;
        private System.Windows.Forms.Panel pnlBitenButon;
        private System.Windows.Forms.Button btnPdfCikti;
        private System.Windows.Forms.Panel pnlAlt;
        private System.Windows.Forms.Button btnYenile;
    }
}