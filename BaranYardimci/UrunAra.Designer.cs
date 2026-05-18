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

        #region Designer code
        private void InitializeComponent()
        {
            this.pnlUst = new System.Windows.Forms.Panel();
            this.lblBaslik = new System.Windows.Forms.Label();
            this.lblAltBaslik = new System.Windows.Forms.Label();

            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.grpAgac = new System.Windows.Forms.GroupBox();
            this.pnlAgacUst = new System.Windows.Forms.Panel();
            this.txtAgacFiltre = new System.Windows.Forms.TextBox();
            this.btnAgacSekillendir = new System.Windows.Forms.Button();
            this.tvKategori = new System.Windows.Forms.TreeView();
            this.ctxAgac = new System.Windows.Forms.ContextMenuStrip();
            this.mnuYenidenAdlandir = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuYeniAlt = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuYenile = new System.Windows.Forms.ToolStripMenuItem();

            this.grpSag = new System.Windows.Forms.GroupBox();
            this.pnlArama = new System.Windows.Forms.Panel();
            this.lblArama = new System.Windows.Forms.Label();
            this.txtTasarimSearch = new System.Windows.Forms.TextBox();
            this.btnTasarimSearch = new System.Windows.Forms.Button();
            this.lblAciklama = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();

            this.pnlAlt = new System.Windows.Forms.Panel();
            this.btnYeniUrun = new System.Windows.Forms.Button();
            this.btnDetay = new System.Windows.Forms.Button();
            this.btnYenile = new System.Windows.Forms.Button();
            this.btnKapat = new System.Windows.Forms.Button();

            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblKayitSayisi = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlUst.SuspendLayout();
            this.grpAgac.SuspendLayout();
            this.pnlAgacUst.SuspendLayout();
            this.grpSag.SuspendLayout();
            this.pnlArama.SuspendLayout();
            this.pnlAlt.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.ctxAgac.SuspendLayout();
            this.SuspendLayout();

            // ── pnlUst ───────────────────────────────────────────────
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(30, 30, 32);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Height = 68;
            this.pnlUst.Controls.Add(this.lblAltBaslik);
            this.pnlUst.Controls.Add(this.lblBaslik);

            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Location = new System.Drawing.Point(20, 8);
            this.lblBaslik.AutoSize = true;
            this.lblBaslik.Text = "🌳  ÜRÜN KATALOĞU";

            this.lblAltBaslik.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblAltBaslik.ForeColor = System.Drawing.Color.FromArgb(180, 180, 200);
            this.lblAltBaslik.Location = new System.Drawing.Point(22, 40);
            this.lblAltBaslik.AutoSize = true;
            this.lblAltBaslik.Text = "Soldan kategori seçin · Sağdaki ana ürüne çift tıklayın → varyantlar açılır";

            // ── splitContainer ───────────────────────────────────────
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.SplitterDistance = 560;
            this.splitContainer.SplitterWidth = 6;
            this.splitContainer.Panel1MinSize = 460;
            this.splitContainer.BackColor = System.Drawing.Color.FromArgb(220, 220, 230);
            this.splitContainer.Panel1.Controls.Add(this.grpAgac);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(8, 6, 4, 6);
            this.splitContainer.Panel2.Controls.Add(this.grpSag);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(4, 6, 8, 6);

            // ── grpAgac ──────────────────────────────────────────────
            this.grpAgac.Text = "🌳  KATEGORİ AĞACI";
            this.grpAgac.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpAgac.ForeColor = System.Drawing.Color.FromArgb(0, 90, 160);
            this.grpAgac.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAgac.Padding = new System.Windows.Forms.Padding(6, 4, 6, 6);
            this.grpAgac.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            this.grpAgac.Controls.Add(this.tvKategori);
            this.grpAgac.Controls.Add(this.pnlAgacUst);

            // ── pnlAgacUst: filtre + şekillendir butonu ──────────────
            this.pnlAgacUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAgacUst.Height = 78;
            this.pnlAgacUst.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.pnlAgacUst.Controls.Add(this.btnAgacSekillendir);
            this.pnlAgacUst.Controls.Add(this.txtAgacFiltre);

            this.txtAgacFiltre.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtAgacFiltre.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtAgacFiltre.Height = 26;
            this.txtAgacFiltre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAgacFiltre.ForeColor = System.Drawing.Color.Gray;
            this.txtAgacFiltre.Text = "🔍 Ağaçta filtrele...";

            FlatBtn(this.btnAgacSekillendir, "🌲  AĞAÇ ŞEKİLLENDİR  (Düzenle Modu)",
                System.Drawing.Color.FromArgb(180, 100, 0));
            this.btnAgacSekillendir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAgacSekillendir.Height = 40;
            this.btnAgacSekillendir.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);

            // ── tvKategori ───────────────────────────────────────────
            this.tvKategori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvKategori.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tvKategori.BackColor = System.Drawing.Color.White;
            this.tvKategori.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvKategori.HideSelection = false;
            this.tvKategori.ShowLines = true;
            this.tvKategori.ShowPlusMinus = true;
            this.tvKategori.ShowRootLines = true;
            this.tvKategori.ItemHeight = 26;
            this.tvKategori.ContextMenuStrip = this.ctxAgac;

            // ── ctxAgac ──────────────────────────────────────────────
            this.ctxAgac.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ctxAgac.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuYenidenAdlandir, this.mnuYeniAlt, this.mnuSep1, this.mnuYenile });
            this.mnuYenidenAdlandir.Text = "✏  Yeniden Adlandır";
            this.mnuYenidenAdlandir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.mnuYeniAlt.Text = "➕  Bu Düğümün Altına Yeni Ekle";
            this.mnuYeniAlt.ForeColor = System.Drawing.Color.FromArgb(0, 120, 60);
            this.mnuYenile.Text = "🔄  Ağacı Yenile";

            // ── grpSag ───────────────────────────────────────────────
            this.grpSag.Text = "📋  SEÇİLEN KATEGORİ — ANA ÜRÜNLER";
            this.grpSag.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSag.ForeColor = System.Drawing.Color.FromArgb(40, 80, 40);
            this.grpSag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSag.Padding = new System.Windows.Forms.Padding(6, 4, 6, 6);
            this.grpSag.BackColor = System.Drawing.Color.FromArgb(248, 252, 248);
            this.grpSag.Controls.Add(this.dataGridView1);
            this.grpSag.Controls.Add(this.pnlArama);

            this.pnlArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlArama.Height = 70;
            this.pnlArama.BackColor = System.Drawing.Color.FromArgb(255, 250, 240);
            this.pnlArama.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.pnlArama.Controls.Add(this.lblAciklama);
            this.pnlArama.Controls.Add(this.btnTasarimSearch);
            this.pnlArama.Controls.Add(this.txtTasarimSearch);
            this.pnlArama.Controls.Add(this.lblArama);

            this.lblArama.Text = "⚡ Hızlı Ara:";
            this.lblArama.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblArama.ForeColor = System.Drawing.Color.FromArgb(200, 100, 0);
            this.lblArama.Location = new System.Drawing.Point(8, 12);
            this.lblArama.AutoSize = true;

            this.txtTasarimSearch.Location = new System.Drawing.Point(95, 9);
            this.txtTasarimSearch.Size = new System.Drawing.Size(320, 26);
            this.txtTasarimSearch.Font = new System.Drawing.Font("Consolas", 11F);
            this.txtTasarimSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTasarimSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            FlatBtn(this.btnTasarimSearch, "🔎  Ara", System.Drawing.Color.FromArgb(200, 100, 0));
            this.btnTasarimSearch.Size = new System.Drawing.Size(100, 30);
            this.btnTasarimSearch.Location = new System.Drawing.Point(420, 7);
            this.btnTasarimSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            this.lblAciklama.Text = "💡 Boşluk ve tire (-) önemsizdir.   örn:  YEH20-1   ≡   YE H20 1   ≡   YEH201";
            this.lblAciklama.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblAciklama.ForeColor = System.Drawing.Color.FromArgb(110, 110, 130);
            this.lblAciklama.Location = new System.Drawing.Point(95, 40);
            this.lblAciklama.AutoSize = true;

            // ── dataGridView ─────────────────────────────────────────
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
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
            this.dataGridView1.RowTemplate.Height = 32;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.dataGridView1.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(0, 120, 200);
            this.dataGridView1.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView1.Font = new System.Drawing.Font("Segoe UI", 9.5F);

            // ── pnlAlt ───────────────────────────────────────────────
            this.pnlAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlt.Height = 56;
            this.pnlAlt.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.pnlAlt.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlAlt.Controls.Add(this.btnYeniUrun);
            this.pnlAlt.Controls.Add(this.btnDetay);
            this.pnlAlt.Controls.Add(this.btnYenile);
            this.pnlAlt.Controls.Add(this.btnKapat);

            FlatBtn(this.btnYeniUrun, "➕  Yeni Ürün", System.Drawing.Color.FromArgb(46, 139, 87));
            this.btnYeniUrun.Size = new System.Drawing.Size(150, 40);
            this.btnYeniUrun.Location = new System.Drawing.Point(10, 8);

            FlatBtn(this.btnDetay, "📦  Varyantlara Git", System.Drawing.Color.FromArgb(0, 120, 200));
            this.btnDetay.Size = new System.Drawing.Size(180, 40);
            this.btnDetay.Location = new System.Drawing.Point(170, 8);

            FlatBtn(this.btnYenile, "🔄  Yenile", System.Drawing.Color.FromArgb(100, 100, 120));
            this.btnYenile.Size = new System.Drawing.Size(110, 40);
            this.btnYenile.Location = new System.Drawing.Point(360, 8);

            FlatBtn(this.btnKapat, "✖  Kapat", System.Drawing.Color.FromArgb(160, 60, 60));
            this.btnKapat.Size = new System.Drawing.Size(120, 40);
            this.btnKapat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // ── Status ───────────────────────────────────────────────
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
            this.lblStatus.Width = 500;
            this.lblStatus.Text = "Hazır.";

            this.lblKayitSayisi.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblKayitSayisi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKayitSayisi.ForeColor = System.Drawing.Color.FromArgb(255, 210, 80);
            this.lblKayitSayisi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblKayitSayisi.Width = 240;
            this.lblKayitSayisi.Text = "📊 0 ana ürün";

            // ── FORM ─────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(238, 238, 242);
            this.ClientSize = new System.Drawing.Size(1380, 800);
            this.MinimumSize = new System.Drawing.Size(1100, 680);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlUst);
            this.Controls.Add(this.pnlAlt);
            this.Controls.Add(this.pnlStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "UrunAra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ürün Kataloğu — Baran Çelik";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlUst.ResumeLayout(false);
            this.pnlUst.PerformLayout();
            this.grpAgac.ResumeLayout(false);
            this.pnlAgacUst.ResumeLayout(false);
            this.pnlAgacUst.PerformLayout();
            this.grpSag.ResumeLayout(false);
            this.pnlArama.ResumeLayout(false);
            this.pnlArama.PerformLayout();
            this.pnlAlt.ResumeLayout(false);
            this.pnlStatus.ResumeLayout(false);
            this.ctxAgac.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

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
        private System.Windows.Forms.Label lblBaslik, lblAltBaslik;

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox grpAgac;
        private System.Windows.Forms.Panel pnlAgacUst;
        private System.Windows.Forms.TextBox txtAgacFiltre;
        private System.Windows.Forms.Button btnAgacSekillendir;
        private System.Windows.Forms.TreeView tvKategori;
        private System.Windows.Forms.ContextMenuStrip ctxAgac;
        private System.Windows.Forms.ToolStripMenuItem mnuYenidenAdlandir, mnuYeniAlt, mnuYenile;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;

        private System.Windows.Forms.GroupBox grpSag;
        private System.Windows.Forms.Panel pnlArama;
        private System.Windows.Forms.Label lblArama, lblAciklama;
        private System.Windows.Forms.TextBox txtTasarimSearch;
        private System.Windows.Forms.Button btnTasarimSearch;
        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.Panel pnlAlt;
        private System.Windows.Forms.Button btnYeniUrun, btnDetay, btnYenile, btnKapat;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus, lblKayitSayisi;
    }
}