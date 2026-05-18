namespace BaranYardimci
{
    partial class UrunAra
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            // ── Üst başlık ─────────────────────────────────────────
            this.pnlUst = new System.Windows.Forms.Panel();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.lblAltBaslik = new System.Windows.Forms.Label();

            // ── Filtre grupları ────────────────────────────────────
            this.grpKategori = new System.Windows.Forms.GroupBox();
            this.tlpKategori = new System.Windows.Forms.TableLayoutPanel();

            this.lblDesign = new System.Windows.Forms.Label();
            this.cmbDesign = new System.Windows.Forms.ComboBox();
            this.lblSector = new System.Windows.Forms.Label();
            this.cmbSector = new System.Windows.Forms.ComboBox();
            this.lblProductType = new System.Windows.Forms.Label();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.lblStructureType = new System.Windows.Forms.Label();
            this.cmbStructureType = new System.Windows.Forms.ComboBox();
            this.lblVoltageAyak = new System.Windows.Forms.Label();
            this.cmbVoltageAyak = new System.Windows.Forms.ComboBox();
            this.lblFoundation = new System.Windows.Forms.Label();
            this.cmbFoundation = new System.Windows.Forms.ComboBox();
            this.lblTower = new System.Windows.Forms.Label();
            this.cmbTower = new System.Windows.Forms.ComboBox();

            this.pnlFiltreButon = new System.Windows.Forms.Panel();
            this.btnUrunGetir = new System.Windows.Forms.Button();
            this.btnTemizle = new System.Windows.Forms.Button();

            // ── Hızlı arama ────────────────────────────────────────
            this.grpHizliArama = new System.Windows.Forms.GroupBox();
            this.lblTasarimSearch = new System.Windows.Forms.Label();
            this.txtTasarimSearch = new System.Windows.Forms.TextBox();
            this.btnTasarimSearch = new System.Windows.Forms.Button();
            this.lblAciklama = new System.Windows.Forms.Label();

            // ── Sonuç grid ─────────────────────────────────────────
            this.grpSonuc = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();

            // ── Alt buton + status ─────────────────────────────────
            this.pnlAltButon = new System.Windows.Forms.Panel();
            this.btnYeniUrun = new System.Windows.Forms.Button();
            this.btnKapat = new System.Windows.Forms.Button();

            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblKayitSayisi = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.pnlUst.SuspendLayout();
            this.grpKategori.SuspendLayout();
            this.tlpKategori.SuspendLayout();
            this.pnlFiltreButon.SuspendLayout();
            this.grpHizliArama.SuspendLayout();
            this.grpSonuc.SuspendLayout();
            this.pnlAltButon.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════════
            //  pnlUst — Koyu başlık
            // ════════════════════════════════════════════════════════
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(30, 30, 32);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Height = 64;
            this.pnlUst.Controls.Add(this.lblAltBaslik);
            this.pnlUst.Controls.Add(this.lblBaslik);

            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Location = new System.Drawing.Point(20, 8);
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Text = "🔍  ÜRÜN ARAMA";

            this.lblAltBaslik.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblAltBaslik.ForeColor = System.Drawing.Color.FromArgb(180, 180, 200);
            this.lblAltBaslik.Location = new System.Drawing.Point(22, 38);
            this.lblAltBaslik.AutoSize = true;
            this.lblAltBaslik.Text = "Kategori filtreleriyle veya tasarım kodu ile ürün arayın";

            // ════════════════════════════════════════════════════════
            //  grpKategori — Kategori filtreleri
            // ════════════════════════════════════════════════════════
            this.grpKategori.Text = "🔎  KATEGORİ FİLTRELERİ  (kademeli)";
            this.grpKategori.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpKategori.ForeColor = System.Drawing.Color.FromArgb(0, 90, 160);
            this.grpKategori.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpKategori.Height = 170;
            this.grpKategori.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.grpKategori.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            this.grpKategori.Controls.Add(this.tlpKategori);
            this.grpKategori.Controls.Add(this.pnlFiltreButon);

            // TableLayout: 7 sütun × 2 satır (label/combo)
            this.tlpKategori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKategori.ColumnCount = 7;
            this.tlpKategori.RowCount = 2;
            for (int i = 0; i < 7; i++)
                this.tlpKategori.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / 7));
            this.tlpKategori.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpKategori.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpKategori.Padding = new System.Windows.Forms.Padding(4);

            FilterLabel(this.lblDesign, "1️⃣ Tasarım");
            FilterLabel(this.lblSector, "2️⃣ Sektör");
            FilterLabel(this.lblProductType, "3️⃣ Ürün Tipi");
            FilterLabel(this.lblStructureType, "4️⃣ Yapı Tipi");
            FilterLabel(this.lblVoltageAyak, "5️⃣ Voltaj/Ayak");
            FilterLabel(this.lblFoundation, "6️⃣ Temel");
            FilterLabel(this.lblTower, "7️⃣ Kule");

            FilterCombo(this.cmbDesign);
            FilterCombo(this.cmbSector);
            FilterCombo(this.cmbProductType);
            FilterCombo(this.cmbStructureType);
            FilterCombo(this.cmbVoltageAyak);
            FilterCombo(this.cmbFoundation);
            FilterCombo(this.cmbTower);

            this.tlpKategori.Controls.Add(this.lblDesign, 0, 0);
            this.tlpKategori.Controls.Add(this.lblSector, 1, 0);
            this.tlpKategori.Controls.Add(this.lblProductType, 2, 0);
            this.tlpKategori.Controls.Add(this.lblStructureType, 3, 0);
            this.tlpKategori.Controls.Add(this.lblVoltageAyak, 4, 0);
            this.tlpKategori.Controls.Add(this.lblFoundation, 5, 0);
            this.tlpKategori.Controls.Add(this.lblTower, 6, 0);

            this.tlpKategori.Controls.Add(this.cmbDesign, 0, 1);
            this.tlpKategori.Controls.Add(this.cmbSector, 1, 1);
            this.tlpKategori.Controls.Add(this.cmbProductType, 2, 1);
            this.tlpKategori.Controls.Add(this.cmbStructureType, 3, 1);
            this.tlpKategori.Controls.Add(this.cmbVoltageAyak, 4, 1);
            this.tlpKategori.Controls.Add(this.cmbFoundation, 5, 1);
            this.tlpKategori.Controls.Add(this.cmbTower, 6, 1);

            // Filtre buton paneli (sağ alt)
            this.pnlFiltreButon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFiltreButon.Height = 50;
            this.pnlFiltreButon.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.pnlFiltreButon.Controls.Add(this.btnUrunGetir);
            this.pnlFiltreButon.Controls.Add(this.btnTemizle);

            FlatBtn(this.btnUrunGetir, "🔍  Ürünleri Getir", System.Drawing.Color.FromArgb(0, 120, 200));
            this.btnUrunGetir.Size = new System.Drawing.Size(170, 38);
            this.btnUrunGetir.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            FlatBtn(this.btnTemizle, "🗑  Filtreleri Temizle", System.Drawing.Color.FromArgb(140, 140, 150));
            this.btnTemizle.Size = new System.Drawing.Size(170, 38);
            this.btnTemizle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // ════════════════════════════════════════════════════════
            //  grpHizliArama
            // ════════════════════════════════════════════════════════
            this.grpHizliArama.Text = "⚡  HIZLI ARAMA  —  Tasarım kodu veya ürün kodu";
            this.grpHizliArama.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpHizliArama.ForeColor = System.Drawing.Color.FromArgb(200, 100, 0);
            this.grpHizliArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpHizliArama.Height = 90;
            this.grpHizliArama.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.grpHizliArama.BackColor = System.Drawing.Color.FromArgb(255, 250, 240);

            this.lblTasarimSearch.Text = "Tasarım/Ürün Kodu:";
            this.lblTasarimSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTasarimSearch.ForeColor = System.Drawing.Color.FromArgb(60, 60, 70);
            this.lblTasarimSearch.Location = new System.Drawing.Point(14, 32);
            this.lblTasarimSearch.AutoSize = true;

            this.txtTasarimSearch.Location = new System.Drawing.Point(150, 30);
            this.txtTasarimSearch.Size = new System.Drawing.Size(360, 26);
            this.txtTasarimSearch.Font = new System.Drawing.Font("Consolas", 11F);
            this.txtTasarimSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTasarimSearch.Anchor = System.Windows.Forms.AnchorStyles.Top
                                         | System.Windows.Forms.AnchorStyles.Left
                                         | System.Windows.Forms.AnchorStyles.Right;

            FlatBtn(this.btnTasarimSearch, "🔎  Ara", System.Drawing.Color.FromArgb(200, 100, 0));
            this.btnTasarimSearch.Size = new System.Drawing.Size(120, 30);
            this.btnTasarimSearch.Location = new System.Drawing.Point(524, 28);

            this.lblAciklama.Text = "💡 Boşluk ve tire (-) önemsizdir.   örn:  YEH20-1   ≡   YE H20 1   ≡   YEH201";
            this.lblAciklama.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblAciklama.ForeColor = System.Drawing.Color.FromArgb(110, 110, 130);
            this.lblAciklama.Location = new System.Drawing.Point(150, 60);
            this.lblAciklama.AutoSize = true;

            this.grpHizliArama.Controls.Add(this.lblAciklama);
            this.grpHizliArama.Controls.Add(this.btnTasarimSearch);
            this.grpHizliArama.Controls.Add(this.txtTasarimSearch);
            this.grpHizliArama.Controls.Add(this.lblTasarimSearch);

            // ════════════════════════════════════════════════════════
            //  grpSonuc + dataGridView
            // ════════════════════════════════════════════════════════
            this.grpSonuc.Text = "📋  ARAMA SONUÇLARI";
            this.grpSonuc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSonuc.ForeColor = System.Drawing.Color.FromArgb(40, 80, 40);
            this.grpSonuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSonuc.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.grpSonuc.BackColor = System.Drawing.Color.FromArgb(248, 252, 248);
            this.grpSonuc.Controls.Add(this.dataGridView1);

            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeight = 38;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(60, 80, 110);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.dataGridView1.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(0, 120, 200);
            this.dataGridView1.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView1.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            // ════════════════════════════════════════════════════════
            //  pnlAltButon + Status
            // ════════════════════════════════════════════════════════
            this.pnlAltButon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAltButon.Height = 56;
            this.pnlAltButon.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.pnlAltButon.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlAltButon.Controls.Add(this.btnYeniUrun);
            this.pnlAltButon.Controls.Add(this.btnKapat);

            FlatBtn(this.btnYeniUrun, "➕  Yeni Ürün Ekle", System.Drawing.Color.FromArgb(46, 139, 87));
            this.btnYeniUrun.Size = new System.Drawing.Size(180, 40);
            this.btnYeniUrun.Location = new System.Drawing.Point(10, 8);

            FlatBtn(this.btnKapat, "✖  Kapat", System.Drawing.Color.FromArgb(160, 60, 60));
            this.btnKapat.Size = new System.Drawing.Size(140, 40);
            this.btnKapat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Height = 26;
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(30, 30, 32);
            this.pnlStatus.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.pnlStatus.Controls.Add(this.lblKayitSayisi);
            this.pnlStatus.Controls.Add(this.lblStatus);

            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(180, 200, 180);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Width = 400;
            this.lblStatus.Text = "Hazır.";

            this.lblKayitSayisi.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblKayitSayisi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKayitSayisi.ForeColor = System.Drawing.Color.FromArgb(255, 210, 80);
            this.lblKayitSayisi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblKayitSayisi.Width = 200;
            this.lblKayitSayisi.Text = "📊 0 kayıt";

            // ════════════════════════════════════════════════════════
            //  FORM
            //  Dock sırası: Fill önce, sonra Top'lar, sonra Bottom
            // ════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(238, 238, 242);
            this.ClientSize = new System.Drawing.Size(1280, 760);
            this.MinimumSize = new System.Drawing.Size(1080, 640);
            this.Controls.Add(this.grpSonuc);        // Fill
            this.Controls.Add(this.grpHizliArama);   // Top
            this.Controls.Add(this.grpKategori);     // Top
            this.Controls.Add(this.pnlUst);          // Top (en üst)
            this.Controls.Add(this.pnlAltButon);     // Bottom
            this.Controls.Add(this.pnlStatus);       // Bottom (en alt)
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UrunAra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ürün Ara — Baran Çelik";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.pnlUst.ResumeLayout(false);
            this.pnlUst.PerformLayout();
            this.grpKategori.ResumeLayout(false);
            this.tlpKategori.ResumeLayout(false);
            this.tlpKategori.PerformLayout();
            this.pnlFiltreButon.ResumeLayout(false);
            this.grpHizliArama.ResumeLayout(false);
            this.grpHizliArama.PerformLayout();
            this.grpSonuc.ResumeLayout(false);
            this.pnlAltButon.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

        // ── Stil yardımcıları ────────────────────────────────────────
        private void FilterLabel(System.Windows.Forms.Label l, string text)
        {
            l.Text = text;
            l.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            l.ForeColor = System.Drawing.Color.FromArgb(60, 70, 90);
            l.Dock = System.Windows.Forms.DockStyle.Fill;
            l.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            l.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        }
        private void FilterCombo(System.Windows.Forms.ComboBox c)
        {
            c.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            c.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            c.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            c.Dock = System.Windows.Forms.DockStyle.Fill;
            c.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
        }
        private void FlatBtn(System.Windows.Forms.Button b, string text, System.Drawing.Color color)
        {
            b.Text = text;
            b.BackColor = color;
            b.ForeColor = System.Drawing.Color.White;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            b.Cursor = System.Windows.Forms.Cursors.Hand;
            b.UseVisualStyleBackColor = false;
        }

        // ── Fields ───────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlUst;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Label lblAltBaslik;

        private System.Windows.Forms.GroupBox grpKategori;
        private System.Windows.Forms.TableLayoutPanel tlpKategori;
        private System.Windows.Forms.Label lblDesign, lblSector, lblProductType, lblStructureType, lblVoltageAyak, lblFoundation, lblTower;
        private System.Windows.Forms.ComboBox cmbDesign, cmbSector, cmbProductType, cmbStructureType, cmbVoltageAyak, cmbFoundation, cmbTower;
        private System.Windows.Forms.Panel pnlFiltreButon;
        private System.Windows.Forms.Button btnUrunGetir, btnTemizle;

        private System.Windows.Forms.GroupBox grpHizliArama;
        private System.Windows.Forms.Label lblTasarimSearch, lblAciklama;
        private System.Windows.Forms.TextBox txtTasarimSearch;
        private System.Windows.Forms.Button btnTasarimSearch;

        private System.Windows.Forms.GroupBox grpSonuc;
        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.Panel pnlAltButon;
        private System.Windows.Forms.Button btnYeniUrun, btnKapat;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus, lblKayitSayisi;
    }
}