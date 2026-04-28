namespace BaranYardimci
{
    partial class FrmTerminSec
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
            this.pnlOrta = new System.Windows.Forms.Panel();
            this.dtpTermin = new System.Windows.Forms.DateTimePicker();
            this.lblTermin = new System.Windows.Forms.Label();
            this.pnlAlt = new System.Windows.Forms.Panel();
            this.btnIptal = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.pnlUst.SuspendLayout();
            this.pnlOrta.SuspendLayout();
            this.pnlAlt.SuspendLayout();
            this.SuspendLayout();
            // pnlUst
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(30, 36, 40);
            this.pnlUst.Controls.Add(this.lblBaslik);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Size = new System.Drawing.Size(400, 50);
            // lblBaslik
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.White;
            this.lblBaslik.Text = "  TERMIN TARIHI SEC";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // pnlOrta
            this.pnlOrta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOrta.Padding = new System.Windows.Forms.Padding(30, 20, 30, 10);
            this.pnlOrta.Controls.Add(this.dtpTermin);
            this.pnlOrta.Controls.Add(this.lblTermin);
            // lblTermin
            this.lblTermin.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTermin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTermin.Text = "Termin Tarihi:";
            this.lblTermin.Size = new System.Drawing.Size(340, 35);
            this.lblTermin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // dtpTermin
            this.dtpTermin.Dock = System.Windows.Forms.DockStyle.Top;
            this.dtpTermin.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtpTermin.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpTermin.Size = new System.Drawing.Size(340, 35);
            // pnlAlt
            this.pnlAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlt.Size = new System.Drawing.Size(400, 60);
            this.pnlAlt.Padding = new System.Windows.Forms.Padding(30, 8, 30, 8);
            this.pnlAlt.Controls.Add(this.btnIptal);
            this.pnlAlt.Controls.Add(this.btnKaydet);
            // btnKaydet
            this.btnKaydet.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnKaydet.Size = new System.Drawing.Size(140, 44);
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnKaydet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // btnIptal
            this.btnIptal.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnIptal.Size = new System.Drawing.Size(120, 44);
            this.btnIptal.Text = "Iptal";
            this.btnIptal.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnIptal.ForeColor = System.Drawing.Color.White;
            this.btnIptal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIptal.FlatAppearance.BorderSize = 0;
            this.btnIptal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnIptal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // FrmTerminSec
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 220);
            this.Controls.Add(this.pnlOrta);
            this.Controls.Add(this.pnlAlt);
            this.Controls.Add(this.pnlUst);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Termin Tarihi Sec";
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.Name = "FrmTerminSec";
            this.Load += new System.EventHandler(this.FrmTerminSec_Load);
            this.pnlUst.ResumeLayout(false);
            this.pnlOrta.ResumeLayout(false);
            this.pnlAlt.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel pnlUst;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Panel pnlOrta;
        private System.Windows.Forms.DateTimePicker dtpTermin;
        private System.Windows.Forms.Label lblTermin;
        private System.Windows.Forms.Panel pnlAlt;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;
    }
}