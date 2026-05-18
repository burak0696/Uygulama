using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BaranYardimci
{
    /// <summary>
    /// Bir ana ürünün (path + Height) altındaki Rev varyantlarını yönetir.
    /// height boş geçilirse Tower'ın tüm Height + Rev'leri gelir.
    /// </summary>
    public class FrmUrunDetay : Form
    {
        private readonly string[] _path;
        private readonly string _height;
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
            this.Text = string.IsNullOrEmpty(_height)
                ? "📦  Varyantlar — " + _path[6]
                : $"📦  Varyantlar — {_path[6]}  ·  Yükseklik: {_height}";
            this.Size = new Size(1100, 640);
            this.MinimumSize = new Size(900, 540);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(238, 238, 242);
            this.Font = new Font("Segoe UI", 9.5f);

            pnlUst = new Panel { Dock = DockStyle.Top, Height = 78, BackColor = Color.FromArgb(30, 30, 32) };
            lblBaslik = new Label
            {
                Text = string.IsNullOrEmpty(_height)
                    ? "📦  " + _path[6] + "  —  Tüm Varyantlar"
                    : "📦  " + _path[6] + "  ·  " + _height + "  —  Rev Varyantları",
                Location = new Point(20, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White
            };
            lblYol = new Label
            {
                Text = "📁  " + string.Join("  →  ", _path.Where(x => !string.IsNullOrEmpty(x)))
                     + (string.IsNullOrEmpty(_height) ? "" : "   ⤷   Yükseklik: " + _height),
                Location = new Point(22, 44),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(180, 200, 230)
            };
            pnlUst.Controls.Add(lblYol);
            pnlUst.Controls.Add(lblBaslik);

            pnlBilgi = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(255, 250, 230), Padding = new Padding(14, 12, 14, 8) };
            lblBilgi = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(110, 80, 0),
                Text = "💡 Hücrelere çift tıklayarak düzenleyebilirsiniz. Yeni varyant Rev otomatik atanır."
            };
            pnlBilgi.Controls.Add(lblBilgi);

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

            pnlAlt = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.FromArgb(240, 240, 245), Padding = new Padding(10, 10, 10, 10) };

            btnYeni = Btn("➕  Yeni Varyant (otomatik Rev)", Color.FromArgb(46, 139, 87), 10, 240);
            btnYeni.Click += (s, e) => YeniVaryant();
            btnSil = Btn("🗑  Seçili Varyantı Sil", Color.FromArgb(160, 60, 60), 260, 180);
            btnSil.Click += (s, e) => SeciliSil();
            btnKaydet = Btn("💾  Değişiklikleri Kaydet", Color.FromArgb(0, 120, 200), 450, 200);
            btnKaydet.Click += (s, e) => DegisiklikleriKaydet();
            btnKapat = Btn("✖  Kapat", Color.FromArgb(100, 100, 120), 0, 120);
            btnKapat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnKapat.Click += (s, e) => this.DialogResult = DialogResult.OK;

            pnlAlt.Controls.AddRange(new Control[] { btnYeni, btnSil, btnKaydet, btnKapat });
            pnlAlt.Resize += (s, e) => btnKapat.Location = new Point(pnlAlt.ClientSize.Width - btnKapat.Width - 10, 10);

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
                var pars = new List<SqlParameter>();
                var w = new List<string>();
                for (int i = 0; i < Seviyeler.Length; i++)
                {
                    w.Add("[" + Seviyeler[i] + "]=@p" + i);
                    pars.Add(new SqlParameter("@p" + i, _path[i] ?? ""));
                }
                if (!string.IsNullOrEmpty(_height))
                {
                    w.Add("Height=@h");
                    pars.Add(new SqlParameter("@h", _height));
                }
                string sql = "SELECT Height, Rev, Tasarim, UrunKodu FROM dbo.Urun WHERE "
                           + string.Join(" AND ", w) + " ORDER BY Height, Rev";

                var dt = DB.GetTable(sql, pars.ToArray());
                dgv.DataSource = dt;

                if (dgv.Columns["Height"] != null) { dgv.Columns["Height"].HeaderText = "Yükseklik"; dgv.Columns["Height"].ReadOnly = !string.IsNullOrEmpty(_height); }
                if (dgv.Columns["Rev"] != null) dgv.Columns["Rev"].HeaderText = "Revizyon";
                if (dgv.Columns["Tasarim"] != null) dgv.Columns["Tasarim"].HeaderText = "Tasarım Kodu";
                if (dgv.Columns["UrunKodu"] != null)
                {
                    dgv.Columns["UrunKodu"].HeaderText = "Ürün Kodu";
                    dgv.Columns["UrunKodu"].DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 220);
                    dgv.Columns["UrunKodu"].DefaultCellStyle.Font = new Font("Consolas", 10F, FontStyle.Bold);
                    dgv.Columns["UrunKodu"].ReadOnly = true;
                }
                dgv.ReadOnly = false;
                if (dgv.Columns["UrunKodu"] != null) dgv.Columns["UrunKodu"].ReadOnly = true;

                // Boş Rev = "ANA ÜRÜN" olarak vurgula
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    string rev = row.Cells["Rev"]?.Value?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(rev))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(220, 245, 220);
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                        if (row.Cells["Rev"] != null) row.Cells["Rev"].Value = "(ana)";
                    }
                }

                lblBilgi.Text = $"💡 {dt.Rows.Count} kayıt. Yeşil satır = ana ürün (rev boş). Çift tıkla düzenle, 💾 kaydet.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yükleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ────────────────────────────────────────────────────────────
        private void YeniVaryant()
        {
            string h = _height;
            if (string.IsNullOrEmpty(h))
            {
                h = UrunAra.MetinDialog("Yükseklik (Height):");
                if (string.IsNullOrWhiteSpace(h)) return;
            }
            // Otomatik Rev
            string onerilenRev = UrunAra.SonrakiRevUret(_path, h);
            string r = UrunAra.MetinDialog($"Revizyon (otomatik atandı, değiştirebilirsiniz):", onerilenRev);
            if (r == null) return; // iptal
            // Boş kabul edilir (ana ürün için)

            string t = UrunAra.MetinDialog("Tasarım Kodu:");
            if (string.IsNullOrWhiteSpace(t)) return;

            string kod = UrunKoduUret(h, r);
            try
            {
                var pars = new List<SqlParameter>();
                for (int i = 0; i < Seviyeler.Length; i++) pars.Add(new SqlParameter("@p" + i, _path[i] ?? ""));
                pars.Add(new SqlParameter("@h", h));
                pars.Add(new SqlParameter("@r", r ?? ""));
                pars.Add(new SqlParameter("@t", t));
                pars.Add(new SqlParameter("@k", kod));

                string sql = "INSERT INTO dbo.Urun (Design,Sector,ProductType,StructureType,VoltageAyak,Foundation,Tower,Height,Rev,Tasarim,UrunKodu) "
                           + "VALUES (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@h,@r,@t,@k)";
                int n = DB.Execute(sql, pars.ToArray());
                if (n > 0)
                {
                    MessageBox.Show($"✅ Varyant eklendi:\n\nRev: {(string.IsNullOrEmpty(r) ? "(ana)" : r)}\nKod: {kod}",
                        "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string rv = r["Rev"]?.ToString() ?? "";
                    if (rv == "(ana)") rv = "";  // görsel etiketi geri çevir
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
            string baz = string.Join("-",
                Cln(_path[0]), Cln(_path[1]), Cln(_path[2]), Cln(_path[3]),
                Cln(_path[4]), Cln(_path[5]), Cln(_path[6]), Cln(h));
            // Rev boşsa ürün koduna eklenmesin → ana ürün
            return string.IsNullOrWhiteSpace(r) ? baz : baz + "-" + Cln(r);
        }
    }

    // ════════════════════════════════════════════════════════════════
    //  Yeni Ürün — Rev otomatik
    // ════════════════════════════════════════════════════════════════
    public class FrmYeniUrun : Form
    {
        private static readonly string[] Seviyeler =
            { "Design", "Sector", "ProductType", "StructureType", "VoltageAyak", "Foundation", "Tower" };
        private static readonly string[] Adlar =
            { "Tasarım", "Sektör", "Ürün Tipi", "Yapı Tipi", "Voltaj/Ayak", "Temel", "Kule" };

        private readonly TextBox[] _txtKat = new TextBox[7];
        private TextBox _txtH, _txtR, _txtT;
        private CheckBox _chkAnaUrun;
        private readonly string[] _varsayilan;

        public FrmYeniUrun(string[] varsayilan)
        {
            _varsayilan = varsayilan ?? new string[7];
            Init();
        }

        private void Init()
        {
            this.Text = "➕  Yeni Ürün Ekle";
            this.Size = new Size(580, 640);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Segoe UI", 9.5f);

            var lblBas = new Label
            {
                Text = "7 kategoriyi + Yükseklik / Rev / Tasarım'ı doldurun.\nAna ürün ekliyorsanız 'Ana Ürün' kutusunu işaretleyin (Rev boş kalır).",
                Dock = DockStyle.Top,
                Height = 56,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(30, 60, 110),
                Padding = new Padding(14, 10, 0, 0)
            };

            var pnl = new Panel { Location = new Point(14, 68), Size = new Size(550, 460) };

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
                    Size = new Size(400, 26),
                    Font = new Font("Segoe UI", 10F),
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = _varsayilan.Length > i ? (_varsayilan[i] ?? "") : ""
                };
                pnl.Controls.Add(_txtKat[i]);
                y += 34;
            }

            y += 10;
            pnl.Controls.Add(new Label { Text = "─────  VARYANT  ─────", Location = new Point(180, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(160, 90, 0) });
            y += 28;

            // Yükseklik
            pnl.Controls.Add(new Label { Text = "Yükseklik :", Location = new Point(0, y + 6), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });
            _txtH = new TextBox { Location = new Point(140, y), Size = new Size(400, 26), Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
            _txtH.TextChanged += (s, e) => RevOtomatikDoldur();
            pnl.Controls.Add(_txtH);
            y += 34;

            // Ana ürün checkbox
            _chkAnaUrun = new CheckBox
            {
                Text = "✓  Bu ANA ÜRÜN (Rev otomatik boş kalsın)",
                Location = new Point(140, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 100, 0),
                Checked = true
            };
            _chkAnaUrun.CheckedChanged += (s, e) =>
            {
                if (_chkAnaUrun.Checked) { _txtR.Text = ""; _txtR.Enabled = false; _txtR.BackColor = Color.FromArgb(230, 245, 230); }
                else { _txtR.Enabled = true; _txtR.BackColor = Color.White; RevOtomatikDoldur(); }
            };
            pnl.Controls.Add(_chkAnaUrun);
            y += 32;

            // Rev
            pnl.Controls.Add(new Label { Text = "Revizyon :", Location = new Point(0, y + 6), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });
            _txtR = new TextBox { Location = new Point(140, y), Size = new Size(400, 26), Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle, Enabled = false, BackColor = Color.FromArgb(230, 245, 230) };
            pnl.Controls.Add(_txtR);
            y += 34;

            // Tasarım
            pnl.Controls.Add(new Label { Text = "Tasarım :", Location = new Point(0, y + 6), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });
            _txtT = new TextBox { Location = new Point(140, y), Size = new Size(400, 26), Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
            pnl.Controls.Add(_txtT);

            var btnOk = new Button { Text = "✔ Ekle", Size = new Size(140, 40), Location = new Point(160, 550), BackColor = Color.FromArgb(46, 139, 87), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) => Ekle();
            var btnIp = new Button { Text = "✖ İptal", Size = new Size(140, 40), Location = new Point(310, 550), BackColor = Color.FromArgb(160, 60, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold), DialogResult = DialogResult.Cancel };
            btnIp.FlatAppearance.BorderSize = 0;

            this.Controls.Add(pnl);
            this.Controls.Add(lblBas);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnIp);
            this.CancelButton = btnIp;
        }

        private void RevOtomatikDoldur()
        {
            if (_chkAnaUrun.Checked) return;
            // Path tamamlanmış mı, height var mı?
            for (int i = 0; i < 7; i++)
                if (string.IsNullOrWhiteSpace(_txtKat[i].Text)) return;
            if (string.IsNullOrWhiteSpace(_txtH.Text)) return;

            var path = new string[7];
            for (int i = 0; i < 7; i++) path[i] = _txtKat[i].Text.Trim();
            string oneri = UrunAra.SonrakiRevUret(path, _txtH.Text.Trim());
            _txtR.Text = oneri;
        }

        private void Ekle()
        {
            for (int i = 0; i < 7; i++)
                if (string.IsNullOrWhiteSpace(_txtKat[i].Text))
                { MessageBox.Show(Adlar[i] + " boş olamaz."); _txtKat[i].Focus(); return; }
            if (string.IsNullOrWhiteSpace(_txtH.Text) || string.IsNullOrWhiteSpace(_txtT.Text))
            { MessageBox.Show("Yükseklik / Tasarım boş olamaz."); return; }

            string rev = _chkAnaUrun.Checked ? "" : (_txtR.Text ?? "").Trim();

            string Cln(string s) => s.Trim().Replace("-", "_");
            string baz = string.Join("-",
                Cln(_txtKat[0].Text), Cln(_txtKat[1].Text), Cln(_txtKat[2].Text), Cln(_txtKat[3].Text),
                Cln(_txtKat[4].Text), Cln(_txtKat[5].Text), Cln(_txtKat[6].Text),
                Cln(_txtH.Text));
            string kod = string.IsNullOrEmpty(rev) ? baz : baz + "-" + Cln(rev);

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
                    new SqlParameter("@r", rev),
                    new SqlParameter("@t", _txtT.Text.Trim()),
                    new SqlParameter("@k", kod));
                if (n > 0)
                {
                    MessageBox.Show($"✅ {(string.IsNullOrEmpty(rev) ? "Ana ürün" : "Varyant")} eklendi:\n\n{kod}",
                        "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
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