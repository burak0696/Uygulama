using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BaranYardimci
{
    public class FrmUrunDetay : Form
    {
        private readonly string[] _path;
        private string _height;
        private static readonly string[] Seviyeler =
            { "Design", "Sector", "ProductType", "StructureType", "VoltageAyak", "Foundation", "Tower" };

        private Panel pnlUst, pnlBilgi, pnlAlt;
        private Label lblBaslik, lblYol, lblBilgi;
        private DataGridView dgv;
        private Button btnYeni, btnSil, btnKaydet, btnKapat;

        public FrmUrunDetay(string[] path, string height = "")
        {
            _path = path;
            _height = height ?? "";
            Init();
            Yukle();
        }

        private void Init()
        {
            this.Text = "📦  Ürünler — " + _path[6];
            this.Size = new Size(1200, 700);
            this.MinimumSize = new Size(960, 560);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(238, 238, 242);
            this.Font = new Font("Segoe UI", 9.5f);

            pnlUst = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = Color.FromArgb(25, 35, 55) };
            lblBaslik = new Label
            {
                Text = string.IsNullOrEmpty(_height)
                    ? "📦  " + _path[6] + "  —  Tüm Ürünler"
                    : "📦  " + _path[6] + "   ·   Yükseklik: " + _height,
                Location = new Point(20, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White
            };
            lblYol = new Label
            {
                Text = "📁  " + string.Join("  →  ", _path.Where(x => !string.IsNullOrEmpty(x))),
                Location = new Point(22, 48),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(180, 200, 230)
            };
            pnlUst.Controls.Add(lblYol);
            pnlUst.Controls.Add(lblBaslik);

            pnlBilgi = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.FromArgb(255, 250, 230), Padding = new Padding(14, 14, 14, 8) };
            lblBilgi = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(110, 80, 0),
                Text = "💡 ➕ ile ürün ekleyin. Rev otomatik atanır. Yeşil = ana ürün."
            };
            pnlBilgi.Controls.Add(lblBilgi);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                RowTemplate = { Height = 34 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Font = new Font("Segoe UI", 10F)
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 80, 110);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 200);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            pnlAlt = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.FromArgb(240, 240, 245), Padding = new Padding(12, 12, 12, 12) };

            btnYeni = Btn("➕  Yeni Ürün Ekle", Color.FromArgb(46, 139, 87), 12, 200);
            btnYeni.Click += (s, e) => YeniUrun();
            btnSil = Btn("🗑  Seçileni Sil", Color.FromArgb(160, 60, 60), 222, 160);
            btnSil.Click += (s, e) => SeciliSil();
            btnKaydet = Btn("💾  Düzenlemeleri Kaydet", Color.FromArgb(0, 120, 200), 392, 200);
            btnKaydet.Click += (s, e) => DegisiklikleriKaydet();
            btnKapat = Btn("✖  Kapat", Color.FromArgb(100, 100, 120), 0, 120);
            btnKapat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnKapat.Click += (s, e) => this.DialogResult = DialogResult.OK;

            pnlAlt.Controls.AddRange(new Control[] { btnYeni, btnSil, btnKaydet, btnKapat });
            pnlAlt.Resize += (s, e) => btnKapat.Location = new Point(pnlAlt.ClientSize.Width - btnKapat.Width - 12, 12);

            this.Controls.Add(dgv);
            this.Controls.Add(pnlBilgi);
            this.Controls.Add(pnlUst);
            this.Controls.Add(pnlAlt);
            this.Controls.SetChildIndex(pnlAlt, 0);
        }

        private Button Btn(string text, Color color, int x, int w)
        {
            return new Button
            {
                Text = text,
                Size = new Size(w, 40),
                Location = new Point(x, 12),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        // ── Yükle: HEPSİNİ getir (height filtresi yok) ───────────────
        private void Yukle()
        {
            try
            {
                var pars = new List<SqlParameter>();
                var w = new List<string>();
                for (int i = 0; i < Seviyeler.Length; i++)
                {
                    w.Add("[" + Seviyeler[i] + "]=@p" + i);
                    pars.Add(new SqlParameter("@p" + i, _path[i] ?? ""));
                }
                // ⭐ Bu kulenin TÜM ürünleri (yükseklik filtresi yok)
                string sql = @"
SELECT ISNULL(Height,'')   AS Height,
       ISNULL(Rev,'')      AS Rev,
       ISNULL(Tasarim,'')  AS Aciklama,
       ISNULL(UrunKodu,'') AS UrunKodu
FROM dbo.Urun
WHERE " + string.Join(" AND ", w) + " ORDER BY Height, Rev";

                var dt = DB.GetTable(sql, pars.ToArray());
                dgv.DataSource = dt;

                if (dgv.Columns["Height"] != null) dgv.Columns["Height"].HeaderText = "Yükseklik";
                if (dgv.Columns["Rev"] != null) { dgv.Columns["Rev"].HeaderText = "Rev"; dgv.Columns["Rev"].ReadOnly = true; dgv.Columns["Rev"].Width = 80; }
                if (dgv.Columns["Aciklama"] != null) dgv.Columns["Aciklama"].HeaderText = "Açıklama  (proje / bilinen ad)";
                if (dgv.Columns["UrunKodu"] != null)
                {
                    dgv.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                    dgv.Columns["UrunKodu"].DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 220);
                    dgv.Columns["UrunKodu"].DefaultCellStyle.Font = new Font("Consolas", 10F, FontStyle.Bold);
                    dgv.Columns["UrunKodu"].ReadOnly = true;
                }

                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                foreach (DataGridViewColumn col in dgv.Columns)
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                if (dgv.Columns["Aciklama"] != null) dgv.Columns["Aciklama"].Width = Math.Max(300, dgv.Columns["Aciklama"].Width);

                dgv.ReadOnly = false;
                if (dgv.Columns["UrunKodu"] != null) dgv.Columns["UrunKodu"].ReadOnly = true;
                if (dgv.Columns["Rev"] != null) dgv.Columns["Rev"].ReadOnly = true;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    string rev = row.Cells["Rev"]?.Value?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(rev))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(220, 245, 220);
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                        row.Cells["Rev"].Value = "(ana)";
                    }
                }

                lblBilgi.Text = $"💡 {dt.Rows.Count} kayıt listeleniyor. Yeşil = ana ürün.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yükleme hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════
        private void YeniUrun()
        {
            string h = UrunAra.MetinDialog(
                "1/2  ·  YÜKSEKLİK girin\n\n(opsiyonel — boş bırakırsanız onay isteyeceğim)", "");
            if (h == null) return;
            h = (h ?? "").Trim();
            if (string.IsNullOrEmpty(h))
            {
                if (MessageBox.Show("⚠ Yükseklik girmediniz.\n\nDevam edilsin mi?",
                    "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            }

            string aciklama = UrunAra.MetinDialog(
                "2/2  ·  AÇIKLAMA girin\n\nHangi projede kullanıldı, bilinen adı, notlar...\n(zorunlu değil ama tavsiye edilir)", "");
            if (aciklama == null) return;
            aciklama = aciklama ?? "";

            string yeniRev = UrunAra.SonrakiRevUret(_path, h);
            string kod = UrunAra.UrunKoduUret(_path, h, yeniRev);

            try
            {
                var sayi = DB.GetValue("SELECT COUNT(1) FROM dbo.Urun WHERE UrunKodu=@k",
                    new SqlParameter("@k", kod));
                if (Convert.ToInt32(sayi) > 0)
                {
                    MessageBox.Show("⚠ Bu ürün kodu zaten var:\n\n" + kod,
                        "Mükerrer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var pars = new List<SqlParameter>();
                for (int i = 0; i < Seviyeler.Length; i++)
                    pars.Add(new SqlParameter("@p" + i, _path[i] ?? ""));
                pars.Add(new SqlParameter("@h", h ?? ""));
                pars.Add(new SqlParameter("@r", yeniRev ?? ""));
                pars.Add(new SqlParameter("@t", aciklama ?? ""));
                pars.Add(new SqlParameter("@k", kod));

                int n = DB.Execute(
                    "INSERT INTO dbo.Urun (Design,Sector,ProductType,StructureType,VoltageAyak,Foundation,Tower,Height,Rev,Tasarim,UrunKodu) " +
                    "VALUES (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@h,@r,@t,@k)", pars.ToArray());

                if (n > 0)
                {
                    string tip = string.IsNullOrEmpty(yeniRev) ? "🟢 ANA ÜRÜN" : "🔵 VARYANT (Rev: " + yeniRev + ")";
                    MessageBox.Show($"✅ {tip} eklendi.\n\nKod: {kod}", "Tamam",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Yukle();
                }
                else
                    MessageBox.Show("❌ INSERT başarısız.", "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SeciliSil()
        {
            if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Önce satır seçin."); return; }
            var row = dgv.SelectedRows[0];
            string kod = row.Cells["UrunKodu"]?.Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(kod)) return;
            if (MessageBox.Show($"'{kod}' silinsin mi?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                int n = DB.Execute("DELETE FROM dbo.Urun WHERE UrunKodu=@k", new SqlParameter("@k", kod));
                if (n > 0) Yukle();
            }
            catch (Exception ex) { MessageBox.Show("Silme hatası: " + ex.Message); }
        }

        private void DegisiklikleriKaydet()
        {
            try
            {
                dgv.EndEdit();
                var dt = (DataTable)dgv.DataSource;
                int guncellenen = 0;
                foreach (DataRow r in dt.Rows)
                {
                    if (r.RowState != DataRowState.Modified) continue;
                    string h = r["Height"]?.ToString() ?? "";
                    string ac = r["Aciklama"]?.ToString() ?? "";
                    string eskiKod = r["UrunKodu", DataRowVersion.Original]?.ToString() ?? "";
                    int n = DB.Execute(
                        "UPDATE dbo.Urun SET Height=@h, Tasarim=@t WHERE UrunKodu=@k",
                        new SqlParameter("@h", h),
                        new SqlParameter("@t", ac),
                        new SqlParameter("@k", eskiKod));
                    guncellenen += n;
                }
                if (guncellenen > 0)
                {
                    MessageBox.Show($"✅ {guncellenen} satır güncellendi.", "Kaydedildi");
                    Yukle();
                }
                else MessageBox.Show("Değişiklik yok.");
            }
            catch (Exception ex) { MessageBox.Show("Kayıt hatası: " + ex.Message); }
        }
    }
}