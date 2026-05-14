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
            this.pnlAna = new System.Windows.Forms.Panel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.lvSonuclar = new System.Windows.Forms.ListView();
            this.colProfil2 = new System.Windows.Forms.ColumnHeader();
            this.colKalite2 = new System.Windows.Forms.ColumnHeader();
            this.colAdet2 = new System.Windows.Forms.ColumnHeader();
            this.colUzunluk2 = new System.Windows.Forms.ColumnHeader();
            this.colAgirlik2 = new System.Windows.Forms.ColumnHeader();
            this.colKod2 = new System.Windows.Forms.ColumnHeader();
            this.colAdi2 = new System.Windows.Forms.ColumnHeader();
            this.pnlSagIc = new System.Windows.Forms.Panel();
            this.splitSag = new System.Windows.Forms.SplitContainer();
            this.pnlDetay = new System.Windows.Forms.Panel();
            this.rtbDetay = new System.Windows.Forms.RichTextBox();
            this.lblBilgi = new System.Windows.Forms.Label();
            this.lblPozNo = new System.Windows.Forms.Label();
            this.pnlArama = new System.Windows.Forms.Panel();
            this.pnlAramaUst = new System.Windows.Forms.Panel();
            this.lblAramaBaslik = new System.Windows.Forms.Label();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.lvHammadde = new System.Windows.Forms.ListView();
            this.colHNo = new System.Windows.Forms.ColumnHeader();
            this.colHAdi = new System.Windows.Forms.ColumnHeader();
            this.pnlSecilenBar = new System.Windows.Forms.Panel();
            this.lblSecilenKod = new System.Windows.Forms.Label();
            this.lblSecilenBaslik = new System.Windows.Forms.Label();
            this.pnlAlt = new System.Windows.Forms.Panel();
            this.pnlAltIc = new System.Windows.Forms.Panel();
            this.btnIptal = new System.Windows.Forms.Button();
            this.btnTamam = new System.Windows.Forms.Button();
            this.btnAtla = new System.Windows.Forms.Button();
            this.btnEsle = new System.Windows.Forms.Button();
            this.pnlSolIc = new System.Windows.Forms.Panel();

            this.pnlUst.SuspendLayout();
            this.pnlAna.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitMain).BeginInit();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitSag).BeginInit();
            this.splitSag.SuspendLayout();
            this.pnlDetay.SuspendLayout();
            this.pnlArama.SuspendLayout();
            this.pnlAramaUst.SuspendLayout();
            this.pnlSecilenBar.SuspendLayout();
            this.pnlAlt.SuspendLayout();
            this.pnlAltIc.SuspendLayout();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════════════
            //  pnlUst
            // ════════════════════════════════════════════════════════════
            this.pnlUst.BackColor = System.Drawing.Color.FromArgb(22, 22, 26);
            this.pnlUst.Controls.Add(this.lblBaslik);
            this.pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUst.Size = new System.Drawing.Size(1920, 54);

            this.lblBaslik.AutoSize = false;
            this.lblBaslik.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBaslik.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblBaslik.ForeColor = System.Drawing.Color.FromArgb(200, 220, 255);
            this.lblBaslik.Text = "ERP Manuel Eşleştirme";
            this.lblBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ════════════════════════════════════════════════════════════
            //  pnlAlt
            // ════════════════════════════════════════════════════════════
            this.pnlAlt.BackColor = System.Drawing.Color.FromArgb(22, 22, 26);
            this.pnlAlt.Controls.Add(this.pnlAltIc);
            this.pnlAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAlt.Size = new System.Drawing.Size(1920, 76);
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

            // ════════════════════════════════════════════════════════════
            //  pnlAna
            // ════════════════════════════════════════════════════════════
            this.pnlAna.Controls.Add(this.splitMain);
            this.pnlAna.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAna.Padding = new System.Windows.Forms.Padding(10, 8, 10, 6);
            this.pnlAna.BackColor = System.Drawing.Color.FromArgb(235, 237, 242);

            // ════════════════════════════════════════════════════════════
            //  splitMain — Üst: sonuç listesi  |  Alt: detay + arama
            //  Üst bölge kısa tutuldu (240px), asıl çalışma alanı altta
            // ════════════════════════════════════════════════════════════
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitMain.SplitterDistance = 240;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.BackColor = System.Drawing.Color.FromArgb(180, 190, 210);
            this.splitMain.Panel1.Controls.Add(this.lvSonuclar);
            this.splitMain.Panel2.Controls.Add(this.pnlSagIc);
            this.splitMain.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);

            // ════════════════════════════════════════════════════════════
            //  lvSonuclar
            // ════════════════════════════════════════════════════════════
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
            this.colProfil2.Text = "Profil"; this.colProfil2.Width = 220;
            this.colKalite2.Text = "Kalite"; this.colKalite2.Width = 100;
            this.colAdet2.Text = "Adet"; this.colAdet2.Width = 90;
            this.colUzunluk2.Text = "Uzunluk (mm)"; this.colUzunluk2.Width = 140;
            this.colAgirlik2.Text = "Ağırlık (kg)"; this.colAgirlik2.Width = 140;
            this.colKod2.Text = "ERP Kodu"; this.colKod2.Width = 130;
            this.colAdi2.Text = "ERP Adı"; this.colAdi2.Width = 500;
            this.lvSonuclar.SelectedIndexChanged +=
                new System.EventHandler(this.lvSonuclar_SelectedIndexChanged);

            // ════════════════════════════════════════════════════════════
            //  pnlSagIc
            // ════════════════════════════════════════════════════════════
            this.pnlSagIc.Controls.Add(this.splitSag);
            this.pnlSagIc.Dock = System.Windows.Forms.DockStyle.Fill;

            // ════════════════════════════════════════════════════════════
            //  splitSag
            //  Sol (detay): 300px — dar, sadece poz bilgisi
            //  Sağ (arama): kalan her şey — ÇOK GENİŞ
            // ════════════════════════════════════════════════════════════
            this.splitSag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSag.SplitterDistance = 300;
            this.splitSag.SplitterWidth = 6;
            this.splitSag.BackColor = System.Drawing.Color.FromArgb(180, 190, 210);
            this.splitSag.Panel1.Controls.Add(this.pnlDetay);
            this.splitSag.Panel2.Controls.Add(this.pnlArama);

            // ════════════════════════════════════════════════════════════
            //  pnlDetay — sol kısım (poz bilgisi)
            // ════════════════════════════════════════════════════════════
            this.pnlDetay.Controls.Add(this.rtbDetay);
            this.pnlDetay.Controls.Add(this.lblBilgi);
            this.pnlDetay.Controls.Add(this.lblPozNo);
            this.pnlDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetay.Padding = new System.Windows.Forms.Padding(10, 8, 6, 6);
            this.pnlDetay.BackColor = System.Drawing.Color.White;

            this.lblPozNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPozNo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblPozNo.ForeColor = System.Drawing.Color.FromArgb(0, 72, 148);
            this.lblPozNo.Size = new System.Drawing.Size(280, 40);
            this.lblPozNo.Text = "—";
            this.lblPozNo.Padding = new System.Windows.Forms.Padding(2, 4, 0, 0);

            this.lblBilgi.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBilgi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBilgi.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            this.lblBilgi.Size = new System.Drawing.Size(280, 28);
            this.lblBilgi.Text = "";
            this.lblBilgi.Padding = new System.Windows.Forms.Padding(2, 0, 0, 4);

            this.rtbDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDetay.Font = new System.Drawing.Font("Consolas", 10F);
            this.rtbDetay.BackColor = System.Drawing.Color.FromArgb(248, 250, 255);
            this.rtbDetay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDetay.ReadOnly = true;
            this.rtbDetay.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;

            // ════════════════════════════════════════════════════════════
            //  pnlArama — sağ kısım (hammadde arama — TAM GENİŞ)
            // ════════════════════════════════════════════════════════════
            this.pnlArama.Controls.Add(this.lvHammadde);
            this.pnlArama.Controls.Add(this.pnlSecilenBar);
            this.pnlArama.Controls.Add(this.pnlAramaUst);
            this.pnlArama.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlArama.Padding = new System.Windows.Forms.Padding(8, 8, 10, 6);
            this.pnlArama.BackColor = System.Drawing.Color.FromArgb(240, 244, 252);

            // ── Başlık + arama kutusu ─────────────────────────────────────
            this.pnlAramaUst.Controls.Add(this.txtArama);
            this.pnlAramaUst.Controls.Add(this.lblAramaBaslik);
            this.pnlAramaUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAramaUst.Size = new System.Drawing.Size(1550, 84);
            this.pnlAramaUst.BackColor = System.Drawing.Color.Transparent;

            this.lblAramaBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAramaBaslik.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAramaBaslik.ForeColor = System.Drawing.Color.FromArgb(0, 72, 148);
            this.lblAramaBaslik.Size = new System.Drawing.Size(1550, 34);
            this.lblAramaBaslik.Text = "🔍  Hammadde Ara";
            this.lblAramaBaslik.Padding = new System.Windows.Forms.Padding(2, 4, 0, 0);

            // ── ÇOK BÜYÜK ARAMA KUTUSU ───────────────────────────────────
            this.txtArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtArama.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.txtArama.Size = new System.Drawing.Size(1550, 46);
            this.txtArama.BackColor = System.Drawing.Color.White;
            this.txtArama.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArama.TextChanged += new System.EventHandler(this.txtArama_TextChanged);

            // ── Seçilen hammadde göstergesi (alt) ─────────────────────────
            this.pnlSecilenBar.Controls.Add(this.lblSecilenKod);
            this.pnlSecilenBar.Controls.Add(this.lblSecilenBaslik);
            this.pnlSecilenBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSecilenBar.Size = new System.Drawing.Size(1550, 74);
            this.pnlSecilenBar.BackColor = System.Drawing.Color.FromArgb(220, 245, 225);
            this.pnlSecilenBar.Padding = new System.Windows.Forms.Padding(14, 6, 14, 6);

            this.lblSecilenBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSecilenBaslik.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSecilenBaslik.ForeColor = System.Drawing.Color.FromArgb(0, 90, 30);
            this.lblSecilenBaslik.Size = new System.Drawing.Size(1522, 22);
            this.lblSecilenBaslik.Text = "SEÇİLEN HAMMADDE:";

            this.lblSecilenKod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSecilenKod.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSecilenKod.ForeColor = System.Drawing.Color.FromArgb(0, 100, 20);
            this.lblSecilenKod.Text = "(seçilmedi)";
            this.lblSecilenKod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── Hammadde listesi — tam genişlik, büyük font ───────────────
            this.lvHammadde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHammadde.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lvHammadde.FullRowSelect = true;
            this.lvHammadde.GridLines = true;
            this.lvHammadde.HideSelection = false;
            this.lvHammadde.MultiSelect = false;
            this.lvHammadde.View = System.Windows.Forms.View.Details;
            this.lvHammadde.BackColor = System.Drawing.Color.White;
            this.lvHammadde.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvHammadde.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colHNo, this.colHAdi });
            this.colHNo.Text = "Kod"; this.colHNo.Width = 140;
            this.colHAdi.Text = "Hammadde Adı"; this.colHAdi.Width = 900;
            this.lvHammadde.SelectedIndexChanged +=
                new System.EventHandler(this.lvHammadde_SelectedIndexChanged);
            this.lvHammadde.DoubleClick +=
                new System.EventHandler(this.lvHammadde_DoubleClick);

            // ════════════════════════════════════════════════════════════
            //  FORM
            // ════════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(235, 237, 242);
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.pnlAna);
            this.Controls.Add(this.pnlAlt);
            this.Controls.Add(this.pnlUst);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1280, 800);
            this.Name = "FrmErpManuelEslestir";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ERP Manuel Eşleştirme";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmErpManuelEslestir_Load);

            this.pnlUst.ResumeLayout(false);
            this.pnlAna.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitMain).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.splitSag).EndInit();
            this.splitSag.ResumeLayout(false);
            this.pnlDetay.ResumeLayout(false);
            this.pnlArama.ResumeLayout(false);
            this.pnlAramaUst.ResumeLayout(false);
            this.pnlAramaUst.PerformLayout();
            this.pnlSecilenBar.ResumeLayout(false);
            this.pnlAlt.ResumeLayout(false);
            this.pnlAltIc.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void BtnS(System.Windows.Forms.Button b, string text,
            System.Drawing.Color color, System.Windows.Forms.DockStyle dock, int w, int h)
        {
            b.Text = text;
            b.BackColor = color;
            b.ForeColor = System.Drawing.Color.White;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            b.Cursor = System.Windows.Forms.Cursors.Hand;
            b.Dock = dock;
            b.Size = new System.Drawing.Size(w, h);
            b.UseVisualStyleBackColor = false;
        }

        // ── Fields ───────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlUst;
        private System.Windows.Forms.Label lblBaslik;
        private System.Windows.Forms.Panel pnlAna;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ListView lvSonuclar;
        private System.Windows.Forms.ColumnHeader colProfil2;
        private System.Windows.Forms.ColumnHeader colKalite2;
        private System.Windows.Forms.ColumnHeader colAdet2;
        private System.Windows.Forms.ColumnHeader colUzunluk2;
        private System.Windows.Forms.ColumnHeader colAgirlik2;
        private System.Windows.Forms.ColumnHeader colKod2;
        private System.Windows.Forms.ColumnHeader colAdi2;
        private System.Windows.Forms.Panel pnlSolIc;
        private System.Windows.Forms.Panel pnlSagIc;
        private System.Windows.Forms.SplitContainer splitSag;
        private System.Windows.Forms.Panel pnlDetay;
        private System.Windows.Forms.RichTextBox rtbDetay;
        private System.Windows.Forms.Label lblBilgi;
        private System.Windows.Forms.Label lblPozNo;
        private System.Windows.Forms.Panel pnlArama;
        private System.Windows.Forms.Panel pnlAramaUst;
        private System.Windows.Forms.Label lblAramaBaslik;
        private System.Windows.Forms.TextBox txtArama;
        private System.Windows.Forms.ListView lvHammadde;
        private System.Windows.Forms.ColumnHeader colHNo;
        private System.Windows.Forms.ColumnHeader colHAdi;
        private System.Windows.Forms.Panel pnlSecilenBar;
        private System.Windows.Forms.Label lblSecilenBaslik;
        private System.Windows.Forms.Label lblSecilenKod;
        private System.Windows.Forms.Panel pnlAlt;
        private System.Windows.Forms.Panel pnlAltIc;
        private System.Windows.Forms.Button btnEsle;
        private System.Windows.Forms.Button btnAtla;
        private System.Windows.Forms.Button btnTamam;
        private System.Windows.Forms.Button btnIptal;
    }
}