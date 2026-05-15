namespace BaranYardimci
{
    partial class FrmErpManuelEslestir
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
            this.pnlAlt = new System.Windows.Forms.Panel();
            this.pnlAltIc = new System.Windows.Forms.Panel();
            this.btnEsle = new System.Windows.Forms.Button();
            this.btnAtla = new System.Windows.Forms.Button();
            this.btnTamam = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.pnlIcerik = new System.Windows.Forms.Panel();
            this.pnlSag = new System.Windows.Forms.Panel();
            this.pnlAramaUst = new System.Windows.Forms.Panel();
            this.lblAramaBaslik = new System.Windows.Forms.Label();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.lvHammadde = new System.Windows.Forms.ListView();
            this.colHNo = new System.Windows.Forms.ColumnHeader();
            this.colHAdi = new System.Windows.Forms.ColumnHeader();
            this.pnlSecilenBar = new System.Windows.Forms.Panel();
            this.lblSecilenBaslik = new System.Windows.Forms.Label();
            this.lblSecilenKod = new System.Windows.Forms.Label();
            this.pnlSol = new System.Windows.Forms.Panel();
            this.pnlDetay = new System.Windows.Forms.Panel();
            this.lblPozNo = new System.Windows.Forms.Label();
            this.lblBilgi = new System.Windows.Forms.Label();
            this.rtbDetay = new System.Windows.Forms.RichTextBox();
            this.lvSonuclar = new System.Windows.Forms.ListView();
            this.colProfil2 = new System.Windows.Forms.ColumnHeader();
            this.colKalite2 = new System.Windows.Forms.ColumnHeader();
            this.colAdet2 = new System.Windows.Forms.ColumnHeader();
            this.colUzunluk2 = new System.Windows.Forms.ColumnHeader();
            this.colAgirlik2 = new System.Windows.Forms.ColumnHeader();
            this.colKod2 = new System.Windows.Forms.ColumnHeader();
            this.colAdi2 = new System.Windows.Forms.ColumnHeader();

            this.pnlUst.SuspendLayout();
            this.pnlAlt.SuspendLayout();
            this.pnlAltIc.SuspendLayout();
            this.pnlIcerik.SuspendLayout();
            this.pnlSag.SuspendLayout();
            this.pnlAramaUst.SuspendLayout();
            this.pnlSecilenBar.SuspendLayout();
            this.pnlSol.SuspendLayout();
            this.pnlDetay.SuspendLayout();
            this.SuspendLayout();

            // ── pnlUst ──────────────────────────────────────────────────
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(22, 22, 26);
            this.pnlUst.Controls.Add(this.lblBaslik);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Size = new System.Drawing.Size(1280, 54);
            this.lblBaslik.AutoSize = false;
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.FromArgb(200, 220, 255);
            this.lblBaslik.Text = "ERP Manuel Eşleştirme";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── pnlAlt ──────────────────────────────────────────────────
            this.pnlAlt.BackColor = System.Drawing.Color.FromArgb(22, 22, 26);
            this.pnlAlt.Controls.Add(this.pnlAltIc);
            this.pnlAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlt.Size = new System.Drawing.Size(1280, 76);
            this.pnlAlt.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.pnlAltIc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAltIc.BackColor = System.Drawing.Color.Transparent;
            this.pnlAltIc.Controls.Add(this.btnIptal);
            this.pnlAltIc.Controls.Add(this.btnTamam);
            this.pnlAltIc.Controls.Add(this.btnAtla);
            this.pnlAltIc.Controls.Add(this.btnEsle);
            BtnS(this.btnEsle, "✔  Eşleştir & İleri", System.Drawing.Color.FromArgb(39, 174, 96), System.Windows.Forms.DockStyle.Left, 250, 56);
            BtnS(this.btnAtla, "⏭  Atla", System.Drawing.Color.FromArgb(90, 90, 105), System.Windows.Forms.DockStyle.Left, 150, 56);
            BtnS(this.btnTamam, "💾  Tamam & Kapat", System.Drawing.Color.FromArgb(0, 122, 204), System.Windows.Forms.DockStyle.Right, 220, 56);
            BtnS(this.btnIptal, "✗  İptal", System.Drawing.Color.FromArgb(180, 55, 55), System.Windows.Forms.DockStyle.Right, 150, 56);
            this.btnEsle.Click += new System.EventHandler(this.btnEsle_Click);
            this.btnAtla.Click += new System.EventHandler(this.btnAtla_Click);
            this.btnTamam.Click += new System.EventHandler(this.btnTamam_Click);
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);

            // ── pnlIcerik (Fill) ─────────────────────────────────────────
            this.pnlIcerik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlIcerik.BackColor = System.Drawing.Color.FromArgb(28, 28, 34);
            this.pnlIcerik.Controls.Add(this.pnlSol);
            this.pnlIcerik.Controls.Add(this.pnlSag);  // sağ önce eklenmeli

            // ── pnlSag (Dock=Right, sabit 560px) ─────────────────────────
            this.pnlSag.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSag.Width = 560;
            this.pnlSag.BackColor = System.Drawing.Color.FromArgb(240, 244, 252);
            this.pnlSag.Padding = new System.Windows.Forms.Padding(8, 8, 10, 6);

            // aramaUst
            this.pnlAramaUst.Controls.Add(this.txtArama);
            this.pnlAramaUst.Controls.Add(this.lblAramaBaslik);
            this.pnlAramaUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAramaUst.Height = 90;
            this.pnlAramaUst.BackColor = System.Drawing.Color.Transparent;
            this.lblAramaBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAramaBaslik.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAramaBaslik.ForeColor = System.Drawing.Color.FromArgb(0, 72, 148);
            this.lblAramaBaslik.Height = 36;
            this.lblAramaBaslik.Text = "🔍  Hammadde Ara";
            this.lblAramaBaslik.Padding = new System.Windows.Forms.Padding(2, 6, 0, 0);
            this.txtArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtArama.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.txtArama.Height = 50;
            this.txtArama.BackColor = System.Drawing.Color.White;
            this.txtArama.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArama.TextChanged += new System.EventHandler(this.txtArama_TextChanged);

            // seçilen bar
            this.pnlSecilenBar.Controls.Add(this.lblSecilenKod);
            this.pnlSecilenBar.Controls.Add(this.lblSecilenBaslik);
            this.pnlSecilenBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSecilenBar.Height = 90;
            this.pnlSecilenBar.BackColor = System.Drawing.Color.FromArgb(220, 245, 225);
            this.pnlSecilenBar.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
            this.lblSecilenBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSecilenBaslik.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSecilenBaslik.ForeColor = System.Drawing.Color.FromArgb(0, 90, 30);
            this.lblSecilenBaslik.Height = 24;
            this.lblSecilenBaslik.Text = "SEÇİLEN HAMMADDE:";
            this.lblSecilenKod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSecilenKod.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSecilenKod.ForeColor = System.Drawing.Color.FromArgb(0, 100, 20);
            this.lblSecilenKod.Text = "(seçilmedi)";
            this.lblSecilenKod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // hammadde listesi
            this.lvHammadde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHammadde.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lvHammadde.FullRowSelect = true;
            this.lvHammadde.GridLines = true;
            this.lvHammadde.HideSelection = false;
            this.lvHammadde.MultiSelect = false;
            this.lvHammadde.View = System.Windows.Forms.View.Details;
            this.lvHammadde.BackColor = System.Drawing.Color.White;
            this.lvHammadde.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvHammadde.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.colHNo, this.colHAdi });
            this.colHNo.Text = "Kod"; this.colHNo.Width = 140;
            this.colHAdi.Text = "Hammadde Adı"; this.colHAdi.Width = 800;
            this.lvHammadde.SelectedIndexChanged += new System.EventHandler(this.lvHammadde_SelectedIndexChanged);
            this.lvHammadde.DoubleClick += new System.EventHandler(this.lvHammadde_DoubleClick);

            // Dock sırası: Fill en son
            this.pnlSag.Controls.Add(this.lvHammadde);
            this.pnlSag.Controls.Add(this.pnlSecilenBar);
            this.pnlSag.Controls.Add(this.pnlAramaUst);

            // ── pnlSol (Fill) ─────────────────────────────────────────────
            this.pnlSol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSol.BackColor = System.Drawing.Color.FromArgb(235, 237, 242);
            this.pnlSol.Padding = new System.Windows.Forms.Padding(10, 8, 6, 6);

            // pnlDetay (Dock=Bottom, 220px) — poz bilgisi
            this.pnlDetay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetay.Height = 220;
            this.pnlDetay.BackColor = System.Drawing.Color.White;
            this.pnlDetay.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.lblPozNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPozNo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblPozNo.ForeColor = System.Drawing.Color.FromArgb(0, 72, 148);
            this.lblPozNo.Height = 40;
            this.lblPozNo.Text = "—";
            this.lblBilgi.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBilgi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBilgi.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblBilgi.Height = 28;
            this.lblBilgi.Text = "";
            this.rtbDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDetay.Font = new System.Drawing.Font("Consolas", 10F);
            this.rtbDetay.BackColor = System.Drawing.Color.FromArgb(248, 250, 255);
            this.rtbDetay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDetay.ReadOnly = true;
            this.rtbDetay.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.pnlDetay.Controls.Add(this.rtbDetay);
            this.pnlDetay.Controls.Add(this.lblBilgi);
            this.pnlDetay.Controls.Add(this.lblPozNo);

            // lvSonuclar (Fill) — üst liste
            this.lvSonuclar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSonuclar.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lvSonuclar.FullRowSelect = true;
            this.lvSonuclar.GridLines = true;
            this.lvSonuclar.HideSelection = false;
            this.lvSonuclar.MultiSelect = false;
            this.lvSonuclar.View = System.Windows.Forms.View.Details;
            this.lvSonuclar.BackColor = System.Drawing.Color.White;
            this.lvSonuclar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvSonuclar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colProfil2, this.colKalite2, this.colAdet2,
                this.colUzunluk2, this.colAgirlik2, this.colKod2, this.colAdi2 });
            this.colProfil2.Text = "Profil"; this.colProfil2.Width = 200;
            this.colKalite2.Text = "Kalite"; this.colKalite2.Width = 90;
            this.colAdet2.Text = "Adet"; this.colAdet2.Width = 80;
            this.colUzunluk2.Text = "Uzunluk (mm)"; this.colUzunluk2.Width = 120;
            this.colAgirlik2.Text = "Ağırlık (kg)"; this.colAgirlik2.Width = 110;
            this.colKod2.Text = "ERP Kodu"; this.colKod2.Width = 120;
            this.colAdi2.Text = "ERP Adı"; this.colAdi2.Width = 400;
            this.lvSonuclar.SelectedIndexChanged += new System.EventHandler(this.lvSonuclar_SelectedIndexChanged);

            // pnlDetay önce (Bottom), sonra lvSonuclar (Fill)
            this.pnlSol.Controls.Add(this.lvSonuclar);
            this.pnlSol.Controls.Add(this.pnlDetay);

            // ── FORM ────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(235, 237, 242);
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.pnlIcerik);
            this.Controls.Add(this.pnlAlt);
            this.Controls.Add(this.pnlUst);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "FrmErpManuelEslestir";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ERP Manuel Eşleştirme";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmErpManuelEslestir_Load);

            this.pnlUst.ResumeLayout(false);
            this.pnlAlt.ResumeLayout(false);
            this.pnlAltIc.ResumeLayout(false);
            this.pnlIcerik.ResumeLayout(false);
            this.pnlSag.ResumeLayout(false);
            this.pnlAramaUst.ResumeLayout(false);
            this.pnlAramaUst.PerformLayout();
            this.pnlSecilenBar.ResumeLayout(false);
            this.pnlSol.ResumeLayout(false);
            this.pnlDetay.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void BtnS(System.Windows.Forms.Button b, string text, System.Drawing.Color color,
            System.Windows.Forms.DockStyle dock, int w, int h)
        {
            b.Text = text; b.BackColor = color; b.ForeColor = System.Drawing.Color.White;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat; b.FlatAppearance.BorderSize = 0;
            b.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            b.Cursor = System.Windows.Forms.Cursors.Hand; b.Dock = dock;
            b.Size = new System.Drawing.Size(w, h); b.UseVisualStyleBackColor = false;
        }

        private System.Windows.Forms.Panel pnlUst, pnlAlt, pnlAltIc, pnlIcerik;
        private System.Windows.Forms.Panel pnlSag, pnlAramaUst, pnlSecilenBar;
        private System.Windows.Forms.Panel pnlSol, pnlDetay;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Label lblAramaBaslik, lblSecilenBaslik, lblSecilenKod;
        private System.Windows.Forms.Label lblPozNo, lblBilgi;
        private System.Windows.Forms.TextBox txtArama;
        private System.Windows.Forms.ListView lvHammadde, lvSonuclar;
        private System.Windows.Forms.ColumnHeader colHNo, colHAdi;
        private System.Windows.Forms.ColumnHeader colProfil2, colKalite2, colAdet2, colUzunluk2, colAgirlik2, colKod2, colAdi2;
        private System.Windows.Forms.RichTextBox rtbDetay;
        private System.Windows.Forms.Button btnEsle, btnAtla, btnTamam, btnIptal;
    }
}