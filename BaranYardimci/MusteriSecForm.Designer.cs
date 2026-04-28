namespace BaranYardimci
{
    partial class MusteriSecForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblAra = new System.Windows.Forms.Label();
            this.txtAra = new System.Windows.Forms.TextBox();
            this.btnAra = new System.Windows.Forms.Button();
            this.dgvMusteriler = new System.Windows.Forms.DataGridView();
            this.grpYeniMusteri = new System.Windows.Forms.GroupBox();
            this.lblAd = new System.Windows.Forms.Label();
            this.txtYeniAd = new System.Windows.Forms.TextBox();
            this.lblVN = new System.Windows.Forms.Label();
            this.txtYeniVN = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtYeniEmail = new System.Windows.Forms.TextBox();
            this.lblTelefon = new System.Windows.Forms.Label();
            this.txtYeniTelefon = new System.Windows.Forms.TextBox();
            this.btnYeniMusteri = new System.Windows.Forms.Button();
            this.grpAktarimBilgi = new System.Windows.Forms.GroupBox();
            this.lblDurum = new System.Windows.Forms.Label();
            this.cmbMalzemeDurum = new System.Windows.Forms.ComboBox();
            this.btnOnayla = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvMusteriler)).BeginInit();
            this.grpYeniMusteri.SuspendLayout();
            this.grpAktarimBilgi.SuspendLayout();
            this.SuspendLayout();

            // === ARAMA ===
            this.lblAra.Text = "Müşteri Ara:";
            this.lblAra.Location = new System.Drawing.Point(12, 15);
            this.lblAra.AutoSize = true;
            this.lblAra.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            this.txtAra.Location = new System.Drawing.Point(115, 12);
            this.txtAra.Size = new System.Drawing.Size(350, 25);
            this.txtAra.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAra.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAra_KeyDown);

            this.btnAra.Text = "Ara";
            this.btnAra.Location = new System.Drawing.Point(475, 10);
            this.btnAra.Size = new System.Drawing.Size(80, 30);
            this.btnAra.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnAra.ForeColor = System.Drawing.Color.White;
            this.btnAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra.FlatAppearance.BorderSize = 0;
            this.btnAra.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAra.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);

            // === MÜŞTERİ LİSTESİ ===
            this.dgvMusteriler.Location = new System.Drawing.Point(12, 48);
            this.dgvMusteriler.Size = new System.Drawing.Size(620, 200);
            this.dgvMusteriler.AllowUserToAddRows = false;
            this.dgvMusteriler.AllowUserToDeleteRows = false;
            this.dgvMusteriler.ReadOnly = true;
            this.dgvMusteriler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMusteriler.MultiSelect = false;
            this.dgvMusteriler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMusteriler.BackgroundColor = System.Drawing.Color.White;
            this.dgvMusteriler.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMusteriler.RowHeadersVisible = false;
            this.dgvMusteriler.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvMusteriler.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMusteriler_CellDoubleClick);

            // === YENİ MÜŞTERİ ===
            this.grpYeniMusteri.Text = "Yeni Müşteri Ekle";
            this.grpYeniMusteri.Location = new System.Drawing.Point(12, 258);
            this.grpYeniMusteri.Size = new System.Drawing.Size(620, 85);
            this.grpYeniMusteri.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);

            this.lblAd.Text = "Ad:";
            this.lblAd.Location = new System.Drawing.Point(10, 28);
            this.lblAd.AutoSize = true;
            this.lblAd.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.txtYeniAd.Location = new System.Drawing.Point(45, 25);
            this.txtYeniAd.Size = new System.Drawing.Size(110, 22);
            this.txtYeniAd.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.lblVN.Text = "VN:";
            this.lblVN.Location = new System.Drawing.Point(165, 28);
            this.lblVN.AutoSize = true;
            this.lblVN.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.txtYeniVN.Location = new System.Drawing.Point(195, 25);
            this.txtYeniVN.Size = new System.Drawing.Size(130, 22);
            this.txtYeniVN.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(10, 55);
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.txtYeniEmail.Location = new System.Drawing.Point(55, 52);
            this.txtYeniEmail.Size = new System.Drawing.Size(160, 22);
            this.txtYeniEmail.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.lblTelefon.Text = "Tel:";
            this.lblTelefon.Location = new System.Drawing.Point(225, 55);
            this.lblTelefon.AutoSize = true;
            this.lblTelefon.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.txtYeniTelefon.Location = new System.Drawing.Point(255, 52);
            this.txtYeniTelefon.Size = new System.Drawing.Size(120, 22);
            this.txtYeniTelefon.Font = new System.Drawing.Font("Segoe UI", 9F);

            this.btnYeniMusteri.Text = "Müşteri Ekle";
            this.btnYeniMusteri.Location = new System.Drawing.Point(400, 25);
            this.btnYeniMusteri.Size = new System.Drawing.Size(200, 50);
            this.btnYeniMusteri.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
            this.btnYeniMusteri.ForeColor = System.Drawing.Color.White;
            this.btnYeniMusteri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYeniMusteri.FlatAppearance.BorderSize = 0;
            this.btnYeniMusteri.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnYeniMusteri.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYeniMusteri.Click += new System.EventHandler(this.btnYeniMusteri_Click);

            this.grpYeniMusteri.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblAd, txtYeniAd, lblVN, txtYeniVN,
                lblEmail, txtYeniEmail, lblTelefon, txtYeniTelefon, btnYeniMusteri
            });

            // === AKTARIM BİLGİLERİ (Sadece Malzeme Durumu) ===
            this.grpAktarimBilgi.Text = "Aktarım Bilgileri";
            this.grpAktarimBilgi.Location = new System.Drawing.Point(12, 350);
            this.grpAktarimBilgi.Size = new System.Drawing.Size(620, 65);
            this.grpAktarimBilgi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);

            this.lblDurum.Text = "Malzeme Durumu:";
            this.lblDurum.Location = new System.Drawing.Point(10, 28);
            this.lblDurum.AutoSize = true;
            this.lblDurum.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.cmbMalzemeDurum.Location = new System.Drawing.Point(145, 25);
            this.cmbMalzemeDurum.Size = new System.Drawing.Size(250, 25);
            this.cmbMalzemeDurum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMalzemeDurum.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.grpAktarimBilgi.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblDurum, cmbMalzemeDurum
            });

            // === ALT BUTONLAR ===
            this.btnOnayla.Text = "✔ Onayla ve Aktar";
            this.btnOnayla.Location = new System.Drawing.Point(350, 425);
            this.btnOnayla.Size = new System.Drawing.Size(170, 40);
            this.btnOnayla.BackColor = System.Drawing.Color.FromArgb(255, 140, 0);
            this.btnOnayla.ForeColor = System.Drawing.Color.White;
            this.btnOnayla.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnayla.FlatAppearance.BorderSize = 0;
            this.btnOnayla.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnOnayla.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOnayla.Click += new System.EventHandler(this.btnOnayla_Click);

            this.btnIptal.Text = "İptal";
            this.btnIptal.Location = new System.Drawing.Point(530, 425);
            this.btnIptal.Size = new System.Drawing.Size(100, 40);
            this.btnIptal.BackColor = System.Drawing.Color.FromArgb(200, 60, 60);
            this.btnIptal.ForeColor = System.Drawing.Color.White;
            this.btnIptal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIptal.FlatAppearance.BorderSize = 0;
            this.btnIptal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnIptal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);

            // === FORM ===
            this.ClientSize = new System.Drawing.Size(645, 478);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblAra, txtAra, btnAra,
                dgvMusteriler, grpYeniMusteri, grpAktarimBilgi,
                btnOnayla, btnIptal
            });
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Müşteri Seçimi ve Aktarım Bilgileri";
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.Load += new System.EventHandler(this.MusteriSecForm_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvMusteriler)).EndInit();
            this.grpYeniMusteri.ResumeLayout(false);
            this.grpYeniMusteri.PerformLayout();
            this.grpAktarimBilgi.ResumeLayout(false);
            this.grpAktarimBilgi.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblAra;
        private System.Windows.Forms.TextBox txtAra;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.DataGridView dgvMusteriler;
        private System.Windows.Forms.GroupBox grpYeniMusteri;
        private System.Windows.Forms.Label lblAd;
        private System.Windows.Forms.TextBox txtYeniAd;
        private System.Windows.Forms.Label lblVN;
        private System.Windows.Forms.TextBox txtYeniVN;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtYeniEmail;
        private System.Windows.Forms.Label lblTelefon;
        private System.Windows.Forms.TextBox txtYeniTelefon;
        private System.Windows.Forms.Button btnYeniMusteri;
        private System.Windows.Forms.GroupBox grpAktarimBilgi;
        private System.Windows.Forms.Label lblDurum;
        private System.Windows.Forms.ComboBox cmbMalzemeDurum;
        private System.Windows.Forms.Button btnOnayla;
        private System.Windows.Forms.Button btnIptal;
    }
}