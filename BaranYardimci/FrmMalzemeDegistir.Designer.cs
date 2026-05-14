namespace BaranYardimci
{
    partial class FrmMalzemeDegistir
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
            this.txtFiltre = new System.Windows.Forms.TextBox();
            this.lblFiltre = new System.Windows.Forms.Label();
            this.btnExceliAc = new System.Windows.Forms.Button();
            this.txtExcelYolu = new System.Windows.Forms.TextBox();
            this.lblExcel = new System.Windows.Forms.Label();
            this.dgvMalzemeler = new System.Windows.Forms.DataGridView();
            this.colSatir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPozNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPozAciklama = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAnaPozNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBilesenTuru = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMevcutBilesenNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMevcutAciklama = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYeniBilesenNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYeniAciklama = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDurum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAra = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlAlt = new System.Windows.Forms.Panel();
            this.btnIptal = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.lblOzet = new System.Windows.Forms.Label();
            this.pnlUst.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMalzemeler)).BeginInit();
            this.pnlAlt.SuspendLayout();
            this.SuspendLayout();
            // pnlUst
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlUst.Controls.Add(this.txtFiltre);
            this.pnlUst.Controls.Add(this.lblFiltre);
            this.pnlUst.Controls.Add(this.btnExceliAc);
            this.pnlUst.Controls.Add(this.txtExcelYolu);
            this.pnlUst.Controls.Add(this.lblExcel);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Padding = new System.Windows.Forms.Padding(12, 12, 12, 10);
            this.pnlUst.Size = new System.Drawing.Size(1100, 96);
            // lblExcel
            this.lblExcel.AutoSize = true;
            this.lblExcel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblExcel.Location = new System.Drawing.Point(12, 12);
            this.lblExcel.Text = "Excel Dosyası";
            // txtExcelYolu
            this.txtExcelYolu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExcelYolu.Location = new System.Drawing.Point(15, 35);
            this.txtExcelYolu.ReadOnly = true;
            this.txtExcelYolu.Size = new System.Drawing.Size(920, 23);
            // btnExceliAc
            this.btnExceliAc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExceliAc.BackColor = System.Drawing.Color.FromArgb(0, 120, 180);
            this.btnExceliAc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExceliAc.FlatAppearance.BorderSize = 0;
            this.btnExceliAc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExceliAc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExceliAc.ForeColor = System.Drawing.Color.White;
            this.btnExceliAc.Location = new System.Drawing.Point(946, 31);
            this.btnExceliAc.Size = new System.Drawing.Size(139, 30);
            this.btnExceliAc.Text = "📂 Excel'i Aç";
            this.btnExceliAc.UseVisualStyleBackColor = false;
            this.btnExceliAc.Click += new System.EventHandler(this.btnExceliAc_Click);
            // lblFiltre
            this.lblFiltre.AutoSize = true;
            this.lblFiltre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFiltre.Location = new System.Drawing.Point(12, 68);
            this.lblFiltre.Text = "Tüm satırlar için filtre";
            // txtFiltre
            this.txtFiltre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFiltre.Location = new System.Drawing.Point(158, 65);
            this.txtFiltre.Size = new System.Drawing.Size(927, 23);
            this.txtFiltre.TextChanged += new System.EventHandler(this.txtFiltre_TextChanged);
            // dgvMalzemeler
            this.dgvMalzemeler.AllowUserToAddRows = false;
            this.dgvMalzemeler.AllowUserToDeleteRows = false;
            this.dgvMalzemeler.AllowUserToResizeRows = false;
            this.dgvMalzemeler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMalzemeler.BackgroundColor = System.Drawing.Color.White;
            this.dgvMalzemeler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMalzemeler.ColumnHeadersHeight = 34;
            this.dgvMalzemeler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMalzemeler.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSatir,
            this.colPozNo,
            this.colPozAciklama,
            this.colAnaPozNo,
            this.colBilesenTuru,
            this.colMevcutBilesenNo,
            this.colMevcutAciklama,
            this.colYeniBilesenNo,
            this.colYeniAciklama,
            this.colDurum,
            this.colAra});
            this.dgvMalzemeler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMalzemeler.MultiSelect = false;
            this.dgvMalzemeler.RowHeadersVisible = false;
            this.dgvMalzemeler.RowTemplate.Height = 30;
            this.dgvMalzemeler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMalzemeler.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMalzemeler_CellContentClick);
            this.dgvMalzemeler.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMalzemeler_CellDoubleClick);
            // columns
            this.colSatir.Name = "colSatir";
            this.colSatir.HeaderText = "Satır";
            this.colSatir.FillWeight = 20F;
            this.colSatir.ReadOnly = true;
            this.colPozNo.Name = "colPozNo";
            this.colPozNo.HeaderText = "Poz No";
            this.colPozNo.FillWeight = 38F;
            this.colPozNo.ReadOnly = true;
            this.colPozAciklama.Name = "colPozAciklama";
            this.colPozAciklama.HeaderText = "Poz Açıklaması";
            this.colPozAciklama.FillWeight = 75F;
            this.colPozAciklama.ReadOnly = true;
            this.colAnaPozNo.Name = "colAnaPozNo";
            this.colAnaPozNo.HeaderText = "Ana Poz No";
            this.colAnaPozNo.FillWeight = 42F;
            this.colAnaPozNo.ReadOnly = true;
            this.colBilesenTuru.Name = "colBilesenTuru";
            this.colBilesenTuru.HeaderText = "Bileşen Türü";
            this.colBilesenTuru.FillWeight = 36F;
            this.colBilesenTuru.ReadOnly = true;
            this.colMevcutBilesenNo.Name = "colMevcutBilesenNo";
            this.colMevcutBilesenNo.HeaderText = "Mevcut Bileşen No";
            this.colMevcutBilesenNo.FillWeight = 48F;
            this.colMevcutBilesenNo.ReadOnly = true;
            this.colMevcutAciklama.Name = "colMevcutAciklama";
            this.colMevcutAciklama.HeaderText = "Mevcut Açıklama";
            this.colMevcutAciklama.FillWeight = 72F;
            this.colMevcutAciklama.ReadOnly = true;
            this.colYeniBilesenNo.Name = "colYeniBilesenNo";
            this.colYeniBilesenNo.HeaderText = "▶ Yeni Bileşen No";
            this.colYeniBilesenNo.FillWeight = 48F;
            this.colYeniBilesenNo.ReadOnly = true;
            this.colYeniAciklama.Name = "colYeniAciklama";
            this.colYeniAciklama.HeaderText = "▶ Yeni Açıklama";
            this.colYeniAciklama.FillWeight = 72F;
            this.colYeniAciklama.ReadOnly = true;
            this.colDurum.Name = "colDurum";
            this.colDurum.HeaderText = "Durum";
            this.colDurum.FillWeight = 36F;
            this.colDurum.ReadOnly = true;
            this.colAra.Name = "colAra";
            this.colAra.HeaderText = "";
            this.colAra.FillWeight = 16F;
            this.colAra.UseColumnTextForButtonValue = true;
            this.colAra.Text = "🔍";
            // pnlAlt
            this.pnlAlt.BackColor = System.Drawing.Color.FromArgb(245, 245, 248);
            this.pnlAlt.Controls.Add(this.btnIptal);
            this.pnlAlt.Controls.Add(this.btnKaydet);
            this.pnlAlt.Controls.Add(this.lblOzet);
            this.pnlAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlt.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.pnlAlt.Size = new System.Drawing.Size(1100, 60);
            // lblOzet
            this.lblOzet.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblOzet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOzet.ForeColor = System.Drawing.Color.FromArgb(120, 85, 0);
            this.lblOzet.Size = new System.Drawing.Size(280, 40);
            this.lblOzet.Text = "0 satır değiştirildi";
            this.lblOzet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // btnKaydet
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnKaydet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.Size = new System.Drawing.Size(210, 40);
            this.btnKaydet.Text = "💾 Kaydet (Revizyon Olarak)";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // btnIptal
            this.btnIptal.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnIptal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIptal.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnIptal.FlatAppearance.BorderSize = 0;
            this.btnIptal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIptal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnIptal.ForeColor = System.Drawing.Color.White;
            this.btnIptal.Size = new System.Drawing.Size(120, 40);
            this.btnIptal.Text = "❌ İptal";
            this.btnIptal.UseVisualStyleBackColor = false;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // FrmMalzemeDegistir
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.dgvMalzemeler);
            this.Controls.Add(this.pnlAlt);
            this.Controls.Add(this.pnlUst);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FrmMalzemeDegistir";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "🔧 ERP Excel — Malzeme Değiştir";
            this.Load += new System.EventHandler(this.FrmMalzemeDegistir_Load);
            this.pnlUst.ResumeLayout(false);
            this.pnlUst.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMalzemeler)).EndInit();
            this.pnlAlt.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlUst;
        private System.Windows.Forms.Label lblExcel;
        private System.Windows.Forms.TextBox txtExcelYolu;
        private System.Windows.Forms.Button btnExceliAc;
        private System.Windows.Forms.Label lblFiltre;
        private System.Windows.Forms.TextBox txtFiltre;
        private System.Windows.Forms.DataGridView dgvMalzemeler;
        private System.Windows.Forms.Panel pnlAlt;
        private System.Windows.Forms.Label lblOzet;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSatir;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPozNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPozAciklama;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAnaPozNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBilesenTuru;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMevcutBilesenNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMevcutAciklama;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYeniBilesenNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYeniAciklama;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDurum;
        private System.Windows.Forms.DataGridViewButtonColumn colAra;
    }
}
