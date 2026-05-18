using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BaranYardimci
{
    /// <summary>
    /// Bir Tower (yaprak) düğümünün altındaki Height/Rev varyantlarını gösterir.
    /// Yeni varyant ekleme, silme, düzenleme yapılır.
    /// </summary>
    public class FrmUrunDetay : Form
    {
        private readonly string[] _path;  // 7 elemanlı: Design..Tower
        private static readonly string[] Seviyeler =
            { "Design", "Sector", "ProductType", "StructureType", "VoltageAyak", "Foundation", "Tower" };

        private Panel pnlUst, pnlBilgi, pnlAlt;
        private Label lblBaslik, lblYol;
        private DataGridView dgv;
        private Button btnYeni, btnSil, btnKaydet, btnKapat;
        private Label lblBilgi;

        public FrmUrunDetay(string[] kategoriPath)
        {
            _path = kategoriPath;
            Init();
            Yukle();
        }

        private void Init()
        {
            this.Text = "📦  Ürün Varyantları — " + _path[6];
            this.Size = new Size(1100, 640);
            this.MinimumSize = new Size(900, 540);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(238, 238, 242);
            this.Font = new Font("Segoe UI", 9.5f);

            // ── Üst ──
            pnlUst = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(30, 30, 32) };
            lblBaslik = new Label
            {
                Text = "📦  " + _path[6] + "  —  Varyantlar",
                Location = new Point(20, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White
            };
            lblYol = new Label
            {
                Text = "📁  " + string.Join("  →  ", _path.Where(x => !string.IsNullOrEmpty(x))),
                Location = new Point(22, 42),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(180, 200, 230)
            };
            pnlUst.Controls.Add(lblYol);
            pnlUst.Controls.Add(lblBaslik);

            // ── Bilgi paneli ──
            pnlBilgi = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(255, 250, 230), Padding = new Padding(14, 12, 14, 8) };
            lblBilgi = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(110, 80, 0),
                Text = "💡 Bu kule altındaki tüm varyantlar listeleniyor. Height, Rev, Tasarım ve Ürün Kodu hücrelerine çift tıklayıp düzenleyebilirsiniz."
            };
            pnlBilgi.Controls.Add(lblBilgi);

            // ── Grid ──
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 38,
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

            // ── Alt buton paneli ──
            pnlAlt = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.FromArgb(240, 240, 245), Padding = new Padding(10, 10, 10, 10) };

            btnYeni = Btn("➕  Yeni Varyant", Color.FromArgb(46, 139, 87), 10);
            btnYeni.Click += (s, e) => YeniVaryant();

            btnSil = Btn("🗑  Seçili Varyantı Sil", Color.FromArgb(160, 60, 60), 180);
            btnSil.Click += (s, e) => SeciliSil();

            btnKaydet = Btn("💾  Değişiklikleri Kaydet", Color.FromArgb(0, 120, 200), 360);
            btnKaydet.Click += (s, e) => DegisiklikleriKaydet();

            btnKapat = Btn("✖  Kapat", Color.FromArgb(100, 100, 120), 0);
            btnKapat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnKapat.Click += (s, e) => this.DialogResult = DialogResult.OK;

            pnlAlt.Controls.AddRange(new Control[] { btnYeni, btnSil, btnKaydet, btnKapat });
            pnlAlt.Resize += (s, e) => btnKapat.Location = new Point(pnlAlt.ClientSize.Width - btnKapat.Width - 10, 10);

            // Form'a ekle
            this.Controls.Add(dgv);
            this.Controls.Add(pnlBilgi);
            this.Controls.Add(pnlUst);
            this.Controls.Add(pnlAlt);

            // Pnl alt'ı en alta yerleştir
            this.Controls.SetChildIndex(pnlAlt, 0);
        }

        private Button Btn(string text, Color color, int x)
        {
            return new Button
            {
                Text = text,
                Size = new Size(170, 40),
                Location = new Point(x, 10),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        // ────────────────────────────────────────────────────────────
        private void Yukle()
        {
            try
            {
                var pars = new SqlParameter[Seviyeler.Length];
                var w = new System.Text.StringBuilder();
                for (int i = 0; i < Seviyeler.Length; i++)
                {
                    if (i > 0) w.Append(" AND ");
                    w.Append("[" + Seviyeler[i] + "]=@p" + i);
                    pars[i] = new SqlParameter("@p" + i, _path[i]);
                }
                string sql = "SELECT Height, Rev, Tasarim, UrunKodu FROM dbo.Urun WHERE " + w.ToString()
                           + " ORDER BY Height, Rev";
                var dt = DB.GetTable(sql, pars);
                dgv.DataSource = dt;

                if (dgv.Columns["Height"] != null) dgv.Columns["Height"].HeaderText = "Yükseklik";
                if (dgv.Columns["Rev"] != null) dgv.Columns["Rev"].HeaderText = "Revizyon";
                if (dgv.Columns["Tasarim"] != null) dgv.Columns["Tasarim"].HeaderText = "Tasarım Kodu";
                if (dgv.Columns["UrunKodu"] != null)
                {
                    dgv.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                    dgv.Columns["UrunKodu"].DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 220);
                    dgv.Columns["UrunKodu"].DefaultCellStyle.Font = new Font("Consolas", 10F, FontStyle.Bold);
                }
                // Düzenlenebilir
                dgv.ReadOnly = false;
                if (dgv.Columns["UrunKodu"] != null) dgv.Columns["UrunKodu"].ReadOnly = true; // ürün kodu hesaplanır

                lblBilgi.Text = $"💡 {dt.Rows.Count} varyant listeleniyor. Hücrelere çift tıklayıp düzenleyin, sonra 💾 Kaydet'e basın.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yükleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ────────────────────────────────────────────────────────────
        private void YeniVaryant()
        {
            string h = UrunAra.MetinDialog("Yükseklik (Height):");
            if (string.IsNullOrWhiteSpace(h)) return;
            string r = UrunAra.MetinDialog("Revizyon (Rev):");
            if (string.IsNullOrWhiteSpace(r)) return;
            string t = UrunAra.MetinDialog("Tasarım Kodu:");
            if (string.IsNullOrWhiteSpace(t)) return;

            string kod = UrunKoduUret(h, r);

            try
            {
                var pars = new System.Collections.Generic.List<SqlParameter>();
                for (int i = 0; i < Seviyeler.Length; i++) pars.Add(new SqlParameter("@p" + i, _path[i]));
                pars.Add(new SqlParameter("@h", h));
                pars.Add(new SqlParameter("@r", r));
                pars.Add(new SqlParameter("@t", t));
                pars.Add(new SqlParameter("@k", kod));

                string sql = "INSERT INTO dbo.Urun (Design,Sector,ProductType,StructureType,VoltageAyak,Foundation,Tower,Height,Rev,Tasarim,UrunKodu) "
                           + "VALUES (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@h,@r,@t,@k)";
                int n = DB.Execute(sql, pars.ToArray());
                if (n > 0)
                {
                    MessageBox.Show("✅ Varyant eklendi:\n" + kod, "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Yukle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SeciliSil()
        {
            if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Önce varyant seçin."); return; }
            var row = dgv.SelectedRows[0];
            string kod = row.Cells["UrunKodu"]?.Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(kod)) return;
            if (MessageBox.Show($"'{kod}' kalıcı olarak silinsin mi?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                int n = DB.Execute("DELETE FROM dbo.Urun WHERE UrunKodu=@k",
                    new SqlParameter("@k", kod));
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
                    string rv = r["Rev"]?.ToString() ?? "";
                    string ta = r["Tasarim"]?.ToString() ?? "";
                    string eskiKod = r["UrunKodu", DataRowVersion.Original]?.ToString() ?? "";

                    int n = DB.Execute(
                        "UPDATE dbo.Urun SET Height=@h, Rev=@r, Tasarim=@t WHERE UrunKodu=@k",
                        new SqlParameter("@h", h),
                        new SqlParameter("@r", rv),
                        new SqlParameter("@t", ta),
                        new SqlParameter("@k", eskiKod));
                    guncellenen += n;
                }
                if (guncellenen > 0)
                {
                    MessageBox.Show($"✅ {guncellenen} varyant güncellendi.", "Kaydedildi",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Yukle();
                }
                else MessageBox.Show("Değişiklik yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string UrunKoduUret(string h, string r)
        {
            string Cln(string s) => string.IsNullOrWhiteSpace(s) ? "NA" : s.Trim().Replace("-", "_");
            return string.Join("-",
                Cln(_path[0]), Cln(_path[1]), Cln(_path[2]), Cln(_path[3]),
                Cln(_path[4]), Cln(_path[5]), Cln(_path[6]), Cln(h), Cln(r));
        }
    }

    // ════════════════════════════════════════════════════════════════
    //  Mini "Yeni Ürün" formu (UrunAra'dan çağrılıyor)
    // ════════════════════════════════════════════════════════════════
    public class FrmYeniUrun : Form
    {
        private static readonly string[] Seviyeler =
            { "Design", "Sector", "ProductType", "StructureType", "VoltageAyak", "Foundation", "Tower" };
        private static readonly string[] Adlar =
            { "Tasarım", "Sektör", "Ürün Tipi", "Yapı Tipi", "Voltaj/Ayak", "Temel", "Kule" };

        private readonly TextBox[] _txtKat = new TextBox[7];
        private TextBox _txtH, _txtR, _txtT;
        private readonly string[] _varsayilan;

        public FrmYeniUrun(string[] varsayilan)
        {
            _varsayilan = varsayilan ?? new string[7];
            Init();
        }

        private void Init()
        {
            this.Text = "➕  Yeni Ürün Ekle";
            this.Size = new Size(560, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Segoe UI", 9.5f);

            var lblBas = new Label
            {
                Text = "Tüm 7 kategoriyi + Yükseklik/Revizyon/Tasarım'ı girin:",
                Dock = DockStyle.Top,
                Height = 38,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 110),
                Padding = new Padding(14, 10, 0, 0)
            };

            var pnl = new Panel { Location = new Point(14, 50), Size = new Size(530, 440) };

            int y = 0;
            for (int i = 0; i < 7; i++)
            {
                pnl.Controls.Add(new Label
                {
                    Text = (i + 1) + ". " + Adlar[i] + " :",
                    Location = new Point(0, y + 6),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                });
                _txtKat[i] = new TextBox
                {
                    Location = new Point(140, y),
                    Size = new Size(380, 26),
                    Font = new Font("Segoe UI", 10F),
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = _varsayilan.Length > i ? (_varsayilan[i] ?? "") : ""
                };
                pnl.Controls.Add(_txtKat[i]);
                y += 36;
            }

            y += 12;
            pnl.Controls.Add(new Label { Text = "─────  VARYANT  ─────", Location = new Point(170, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(160, 90, 0) });
            y += 28;

            string[] son = { "Yükseklik", "Revizyon", "Tasarım Kodu" };
            for (int i = 0; i < 3; i++)
            {
                pnl.Controls.Add(new Label { Text = son[i] + " :", Location = new Point(0, y + 6), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });
                var t = new TextBox { Location = new Point(140, y), Size = new Size(380, 26), Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
                pnl.Controls.Add(t);
                if (i == 0) _txtH = t; else if (i == 1) _txtR = t; else _txtT = t;
                y += 36;
            }

            var btnOk = new Button { Text = "✔ Ekle", Size = new Size(130, 38), Location = new Point(160, 510), BackColor = Color.FromArgb(46, 139, 87), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) => Ekle();
            var btnIp = new Button { Text = "✖ İptal", Size = new Size(130, 38), Location = new Point(300, 510), BackColor = Color.FromArgb(160, 60, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold), DialogResult = DialogResult.Cancel };
            btnIp.FlatAppearance.BorderSize = 0;

            this.Controls.Add(pnl);
            this.Controls.Add(lblBas);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnIp);
            this.CancelButton = btnIp;
        }

        private void Ekle()
        {
            for (int i = 0; i < 7; i++)
                if (string.IsNullOrWhiteSpace(_txtKat[i].Text))
                { MessageBox.Show(Adlar[i] + " boş olamaz."); _txtKat[i].Focus(); return; }
            if (string.IsNullOrWhiteSpace(_txtH.Text) || string.IsNullOrWhiteSpace(_txtR.Text) || string.IsNullOrWhiteSpace(_txtT.Text))
            { MessageBox.Show("Yükseklik / Revizyon / Tasarım boş olamaz."); return; }

            string Cln(string s) => s.Trim().Replace("-", "_");
            string kod = string.Join("-",
                Cln(_txtKat[0].Text), Cln(_txtKat[1].Text), Cln(_txtKat[2].Text), Cln(_txtKat[3].Text),
                Cln(_txtKat[4].Text), Cln(_txtKat[5].Text), Cln(_txtKat[6].Text),
                Cln(_txtH.Text), Cln(_txtR.Text));

            try
            {
                var sayi = DB.GetValue("SELECT COUNT(1) FROM dbo.Urun WHERE UrunKodu=@k",
                    new SqlParameter("@k", kod));
                if (Convert.ToInt32(sayi) > 0)
                { MessageBox.Show("Bu ürün kodu zaten var:\n" + kod, "Mükerrer", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                int n = DB.Execute(
                    "INSERT INTO dbo.Urun (Design,Sector,ProductType,StructureType,VoltageAyak,Foundation,Tower,Height,Rev,Tasarim,UrunKodu) " +
                    "VALUES (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@h,@r,@t,@k)",
                    new SqlParameter("@p0", _txtKat[0].Text.Trim()),
                    new SqlParameter("@p1", _txtKat[1].Text.Trim()),
                    new SqlParameter("@p2", _txtKat[2].Text.Trim()),
                    new SqlParameter("@p3", _txtKat[3].Text.Trim()),
                    new SqlParameter("@p4", _txtKat[4].Text.Trim()),
                    new SqlParameter("@p5", _txtKat[5].Text.Trim()),
                    new SqlParameter("@p6", _txtKat[6].Text.Trim()),
                    new SqlParameter("@h", _txtH.Text.Trim()),
                    new SqlParameter("@r", _txtR.Text.Trim()),
                    new SqlParameter("@t", _txtT.Text.Trim()),
                    new SqlParameter("@k", kod));
                if (n > 0)
                {
                    MessageBox.Show("✅ Eklendi:\n" + kod, "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}