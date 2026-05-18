using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BaranYardimci;

namespace BARAN
{
    public partial class MainMenu : Form
    {
        private System.Windows.Forms.Timer _tmrDbKontrol;
        private System.Windows.Forms.Timer _tmrSaat;
        private bool _sonDurumOk = false;
        private ToolTip _ttDbDurum;

        public MainMenu()
        {
            InitializeComponent();

            // DB ışığı için paint
            pnlDbIsik.Paint += PnlDbIsik_Paint;

            // Tarih saat
            GuncelleTarihSaat();
            _tmrSaat = new System.Windows.Forms.Timer { Interval = 1000 };
            _tmrSaat.Tick += (s, e) => GuncelleTarihSaat();
            _tmrSaat.Start();

            // ToolTip
            _ttDbDurum = new ToolTip();
            _ttDbDurum.SetToolTip(pnlDbDurum, "Veritabanı bağlantı durumu");
            _ttDbDurum.SetToolTip(lblDbDurum, "Veritabanı bağlantı durumu");
            _ttDbDurum.SetToolTip(pnlDbIsik, "Veritabanı bağlantı durumu");

            // Tıklayınca elle test
            pnlDbDurum.Click += (s, e) => DbBaglantiKontrolEt();
            lblDbDurum.Click += (s, e) => DbBaglantiKontrolEt();
            lblDbBaslik.Click += (s, e) => DbBaglantiKontrolEt();
            pnlDbIsik.Click += (s, e) => DbBaglantiKontrolEt();

            // İlk kontrol + periyodik
            DbBaglantiKontrolEt();
            _tmrDbKontrol = new System.Windows.Forms.Timer { Interval = 10000 }; // 10 sn
            _tmrDbKontrol.Tick += (s, e) => DbBaglantiKontrolEt();
            _tmrDbKontrol.Start();
        }

        public MainMenu(string user) : this()
        {
            lblBaslik.Text = $"Hoşgeldiniz, Sayın {user}";
        }

        // ── Tarih/Saat ─────────────────────────────────────────────────
        private void GuncelleTarihSaat()
        {
            try
            {
                var tr = new System.Globalization.CultureInfo("tr-TR");
                lblTarih.Text = DateTime.Now.ToString("dd MMMM yyyy, dddd  •  HH:mm:ss", tr);
            }
            catch { lblTarih.Text = DateTime.Now.ToString(); }
        }

        // ── DB Bağlantı Kontrol ────────────────────────────────────────
        private void DbBaglantiKontrolEt()
        {
            bool ok = false;
            string detay = "Bağlantı sağlıklı";
            try
            {
                using (var conn = new SqlConnection(BaranYardimci.DB.ConnStr))
                {
                    conn.Open();
                    ok = (conn.State == ConnectionState.Open);
                    detay = $"Sunucu: {conn.DataSource}\nVeritabanı: {conn.Database}\nDurum: AKTİF";
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                ok = false;
                detay = "Hata: " + ex.Message;
            }

            _sonDurumOk = ok;

            if (ok)
            {
                lblDbDurum.Text = "BAĞLANTI AKTİF";
                lblDbDurum.ForeColor = Color.FromArgb(34, 139, 58);
                pnlDbDurum.BackColor = Color.FromArgb(232, 248, 236);
                lblDbBaslik.ForeColor = Color.FromArgb(120, 150, 130);
            }
            else
            {
                lblDbDurum.Text = "BAĞLANTI YOK";
                lblDbDurum.ForeColor = Color.FromArgb(192, 40, 40);
                pnlDbDurum.BackColor = Color.FromArgb(253, 232, 232);
                lblDbBaslik.ForeColor = Color.FromArgb(160, 110, 110);
            }

            try { _ttDbDurum.SetToolTip(pnlDbDurum, detay); } catch { }
            try { _ttDbDurum.SetToolTip(lblDbDurum, detay); } catch { }
            try { _ttDbDurum.SetToolTip(pnlDbIsik, detay); } catch { }

            pnlDbIsik.Invalidate();
        }

        private void PnlDbIsik_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color koyu = _sonDurumOk ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60);
            Color acik = _sonDurumOk ? Color.FromArgb(170, 240, 195) : Color.FromArgb(255, 180, 180);
            Color koyuKenar = _sonDurumOk ? Color.FromArgb(30, 130, 76) : Color.FromArgb(160, 30, 30);

            var rect = new Rectangle(1, 1, pnlDbIsik.Width - 3, pnlDbIsik.Height - 3);

            // Dış halo (parlaklık)
            using (var halo = new SolidBrush(Color.FromArgb(60, koyu.R, koyu.G, koyu.B)))
                e.Graphics.FillEllipse(halo, -2, -2, pnlDbIsik.Width + 2, pnlDbIsik.Height + 2);

            // Asıl daire (gradient)
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                using (var br = new PathGradientBrush(path))
                {
                    br.CenterColor = acik;
                    br.SurroundColors = new[] { koyu };
                    br.CenterPoint = new PointF(rect.Width * 0.35f, rect.Height * 0.30f);
                    e.Graphics.FillEllipse(br, rect);
                }
            }

            // Kenar
            using (var pen = new Pen(koyuKenar, 1.2f))
                e.Graphics.DrawEllipse(pen, rect);
        }

        // ── Buton tıklamaları ──────────────────────────────────────────
        private void btnUrunAra_Click(object sender, EventArgs e)
            => Bilgi("Ürün Arama modülü yakında aktif olacak.");

        private void btnMusteri_Click(object sender, EventArgs e)
            => Bilgi("Müşteri Paneli modülü yakında aktif olacak.");

        private void btnGalvaniz_Click(object sender, EventArgs e)
            => Bilgi("Galvaniz Teklif modülü yakında aktif olacak.");

        private void btnSatinalma_Click(object sender, EventArgs e)
            => Ac(() => new Sarf(), "Satın Alma");

        private void btnTeklif_Click(object sender, EventArgs e)
            => Bilgi("Teklif modülü yakında aktif olacak.");

        private void btnBakimOnarim_Click(object sender, EventArgs e)
            => Bilgi("Bakım / Onarım modülü yakında aktif olacak.");

        private void btnUretim_Click(object sender, EventArgs e)
            => Bilgi("Üretim modülü yakında aktif olacak.");

        private void btnIK_Click(object sender, EventArgs e)
            => Bilgi("İnsan Kaynakları modülü yakında aktif olacak.");

        private void btnDonusturucu_Click(object sender, EventArgs e)
            => Ac(() => new Donusturucu(), "Dönüştürücü");

        private void btnImalatPlanlama_Click(object sender, EventArgs e)
            => Bilgi("İmalat Planlama modülü yakında aktif olacak.");

        private void btnGalvanizKontrol_Click(object sender, EventArgs e)
            => Ac(() => new GalvanizKontrol(), "Galvaniz Kontrol");

        private void button1_Click(object sender, EventArgs e)
            => Ac(() => new galvaniz(), "Galvaniz");

        private void btnCikis_Click(object sender, EventArgs e)
            => Application.Exit();

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                try { if (_tmrDbKontrol != null) { _tmrDbKontrol.Stop(); _tmrDbKontrol.Dispose(); } } catch { }
                try { if (_tmrSaat != null) { _tmrSaat.Stop(); _tmrSaat.Dispose(); } } catch { }
                Application.Exit();
            }
        }

        // ── Yardımcılar ────────────────────────────────────────────────
        private void Bilgi(string mesaj)
            => MessageBox.Show(mesaj, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void Ac(Func<Form> uret, string ad)
        {
            try
            {
                using (var frm = uret())
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ad} formu hatası: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}