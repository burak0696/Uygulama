using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class UrunAra : Form
    {
        private static readonly string[] Seviyeler =
            { "Design", "Sector", "ProductType", "StructureType", "VoltageAyak", "Foundation", "Tower" };
        private static readonly string[] SeviyeAdlari =
            { "Tasarım", "Sektör", "Ürün Tipi", "Yapı Tipi", "Voltaj/Ayak", "Temel", "Kule" };

        private const string PLACEHOLDER = "🔍 Ağaçta filtrele...";
        private bool _duzenleModu = false;

        public UrunAra()
        {
            InitializeComponent();
            AttachEvents();
            AgacYukle();
            DurumYaz("Hazır. Soldan kategori seçin.", Color.FromArgb(180, 200, 180));
        }

        // ════════════════════════════════════════════════════════════
        //  EVENTS
        // ════════════════════════════════════════════════════════════
        private void AttachEvents()
        {
            tvKategori.AfterSelect += TvKategori_AfterSelect;
            tvKategori.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var n = tvKategori.GetNodeAt(e.X, e.Y);
                    if (n != null) tvKategori.SelectedNode = n;
                }
            };

            ctxAgac.Opening += (s, e) =>
            {
                if (!_duzenleModu)
                {
                    e.Cancel = true;
                    DurumYaz("⚠ Önce 'Ağaç Şekillendir'i açın.", Color.FromArgb(255, 200, 100));
                    return;
                }
                bool aktif = tvKategori.SelectedNode != null;
                mnuYenidenAdlandir.Enabled = aktif;
                mnuYeniAlt.Enabled = aktif;
            };

            mnuYenidenAdlandir.Click += (s, e) => YenidenAdlandir(tvKategori.SelectedNode);
            mnuYeniAlt.Click += (s, e) => DugumeAltEkle(tvKategori.SelectedNode);
            mnuYenile.Click += (s, e) => AgacYukle();

            btnAgacSekillendir.Click += (s, e) =>
            {
                _duzenleModu = !_duzenleModu;
                if (_duzenleModu)
                {
                    btnAgacSekillendir.BackColor = Color.FromArgb(220, 60, 40);
                    btnAgacSekillendir.Text = "🔒  ŞEKİLLENDİRMEYİ KAPAT";
                    tvKategori.BackColor = Color.FromArgb(255, 252, 240);
                    DurumYaz("🔧 Şekillendirme AÇIK — sağ tık menüsü aktif.", Color.FromArgb(255, 210, 80));
                    MessageBox.Show(
                        "🌲 AĞAÇ ŞEKİLLENDİR\n\n" +
                        "Ağaçtaki herhangi bir düğüme SAĞ TIK ile:\n" +
                        "• ✏ Yeniden adlandır\n" +
                        "• ➕ Altına yeni düğüm/ürün ekle\n\n" +
                        "Silme işlemi bu modda yapılmaz.",
                        "Düzenleme Modu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    btnAgacSekillendir.BackColor = Color.FromArgb(180, 100, 0);
                    btnAgacSekillendir.Text = "🌲  AĞAÇ ŞEKİLLENDİR  (Düzenle Modu)";
                    tvKategori.BackColor = Color.White;
                    DurumYaz("👁 Görüntüleme modu.", Color.FromArgb(180, 200, 180));
                }
            };

            // Ağaç filtre placeholder
            txtAgacFiltre.GotFocus += (s, e) =>
            {
                if (txtAgacFiltre.Text == PLACEHOLDER)
                { txtAgacFiltre.Text = ""; txtAgacFiltre.ForeColor = Color.Black; }
            };
            txtAgacFiltre.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtAgacFiltre.Text))
                { txtAgacFiltre.Text = PLACEHOLDER; txtAgacFiltre.ForeColor = Color.Gray; }
            };
            txtAgacFiltre.TextChanged += (s, e) =>
            {
                if (txtAgacFiltre.Text == PLACEHOLDER) return;
                AgacFiltrele(txtAgacFiltre.Text);
            };

            btnYeniUrun.Click += BtnYeniUrun_Click;
            btnDetay.Click += (s, e) => SeciliyiVaryantOlarakAc();
            btnYenile.Click += (s, e) => AgacYukle();
            btnKapat.Click += (s, e) => this.Close();
            btnTasarimSearch.Click += BtnTasarimSearch_Click;

            txtTasarimSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BtnTasarimSearch_Click(s, EventArgs.Empty);
                    e.Handled = true; e.SuppressKeyPress = true;
                }
            };

            dataGridView1.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                SeciliyiVaryantOlarakAc();
            };
        }

        // ════════════════════════════════════════════════════════════
        //  AĞAÇ YÜKLEME
        // ════════════════════════════════════════════════════════════
        private void AgacYukle()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                tvKategori.Nodes.Clear();
                tvKategori.BeginUpdate();

                string sql = "SELECT " + string.Join(",", Seviyeler.Select(k => "[" + k + "]"))
                           + ", COUNT(*) AS Adet FROM dbo.Urun "
                           + "GROUP BY " + string.Join(",", Seviyeler.Select(k => "[" + k + "]"))
                           + " ORDER BY " + string.Join(",", Seviyeler.Select(k => "[" + k + "]"));

                var dt = DB.GetTable(sql);
                foreach (DataRow r in dt.Rows)
                {
                    var path = Seviyeler.Select(k => (r[k]?.ToString() ?? "").Trim()).ToArray();
                    int adet = Convert.ToInt32(r["Adet"]);
                    DugumEkle(path, adet);
                }
                foreach (TreeNode root in tvKategori.Nodes) ToplamGuncelle(root);
                tvKategori.EndUpdate();

                int toplamUrun = dt.AsEnumerable().Sum(x => Convert.ToInt32(x["Adet"]));
                DurumYaz($"✅ Ağaç yüklendi — {toplamUrun} ürün, {dt.Rows.Count} kule.", Color.FromArgb(140, 240, 160));
            }
            catch (Exception ex)
            {
                DurumYaz("❌ " + ex.Message, Color.FromArgb(255, 140, 140));
                MessageBox.Show("Ağaç yüklenemedi:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor.Current = Cursors.Default; }
        }

        private void DugumEkle(string[] path, int yaprakAdet)
        {
            TreeNodeCollection current = tvKategori.Nodes;
            for (int i = 0; i < path.Length; i++)
            {
                string ad = string.IsNullOrEmpty(path[i]) ? "(boş)" : path[i];
                TreeNode bulundu = null;
                foreach (TreeNode t in current)
                    if (string.Equals(t.Name, ad, StringComparison.OrdinalIgnoreCase))
                    { bulundu = t; break; }

                if (bulundu == null)
                {
                    bulundu = new TreeNode(ad) { Name = ad };
                    bulundu.Tag = new NodeBilgi
                    {
                        Seviye = i,
                        Path = path.Take(i + 1).ToArray(),
                        YaprakMi = (i == path.Length - 1),
                        Adet = (i == path.Length - 1) ? yaprakAdet : 0
                    };
                    if (bulundu.Tag is NodeBilgi nb)
                    {
                        bulundu.ForeColor = nb.YaprakMi ? Color.FromArgb(140, 70, 0) : Color.FromArgb(30, 60, 110);
                        if (nb.YaprakMi) bulundu.NodeFont = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                    }
                    current.Add(bulundu);
                }
                current = bulundu.Nodes;
            }
        }

        private int ToplamGuncelle(TreeNode node)
        {
            var bilgi = (NodeBilgi)node.Tag;
            int toplam = bilgi.Adet;
            foreach (TreeNode c in node.Nodes) toplam += ToplamGuncelle(c);
            bilgi.AltToplam = toplam;
            string ikon = bilgi.YaprakMi ? "📦" : "📁";
            string sayi = bilgi.YaprakMi ? $"  ·  {bilgi.Adet} ürün" : $"  ·  {toplam}";
            node.Text = $"{ikon}  {(node.Name == "(boş)" ? "(boş)" : node.Name)}{sayi}";
            return toplam;
        }

        private void AgacFiltrele(string aranan)
        {
            if (string.IsNullOrWhiteSpace(aranan)) { AgacYukle(); return; }
            tvKategori.BeginUpdate();
            tvKategori.CollapseAll();
            foreach (TreeNode n in tvKategori.Nodes) FiltreUygula(n, aranan.ToLower());
            tvKategori.EndUpdate();
        }
        private bool FiltreUygula(TreeNode n, string aranan)
        {
            bool benim = n.Name.ToLower().Contains(aranan);
            bool altta = false;
            foreach (TreeNode c in n.Nodes) altta |= FiltreUygula(c, aranan);
            if (benim || altta) { n.Expand(); n.BackColor = benim ? Color.FromArgb(255, 250, 200) : Color.Empty; return true; }
            n.BackColor = Color.FromArgb(245, 245, 245);
            return false;
        }

        // ════════════════════════════════════════════════════════════
        //  AĞAÇ SEÇİMİ → GRİD (SADECE ANA ÜRÜNLER — Rev BOŞ olanlar)
        // ════════════════════════════════════════════════════════════
        private void TvKategori_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;
            var b = (NodeBilgi)e.Node.Tag;

            var whereParts = new List<string>();
            var pars = new List<SqlParameter>();
            for (int i = 0; i < b.Path.Length; i++)
            {
                whereParts.Add("u.[" + Seviyeler[i] + "]=@p" + i);
                pars.Add(new SqlParameter("@p" + i, b.Path[i]));
            }
            string secimler = string.Join(",", Seviyeler.Select(k => "u.[" + k + "]"));

            string sql = $@"
SELECT {secimler},
       u.Height,
       ISNULL(u.Rev,'')      AS Rev,
       ISNULL(u.Tasarim,'')  AS Tasarim,
       ISNULL(u.UrunKodu,'') AS UrunKodu,
       (SELECT COUNT(*) FROM dbo.Urun u2
          WHERE u2.Design=u.Design AND u2.Sector=u.Sector AND u2.ProductType=u.ProductType
            AND u2.StructureType=u.StructureType AND u2.VoltageAyak=u.VoltageAyak
            AND u2.Foundation=u.Foundation AND u2.Tower=u.Tower
            AND u2.Height=u.Height
            AND u2.Rev IS NOT NULL AND LTRIM(RTRIM(u2.Rev))<>'') AS Varyant
FROM dbo.Urun u
WHERE {string.Join(" AND ", whereParts)}
  AND (u.Rev IS NULL OR LTRIM(RTRIM(u.Rev))='')
ORDER BY u.Design, u.Sector, u.ProductType, u.StructureType, u.VoltageAyak, u.Foundation, u.Tower, u.Height";

            try
            {
                var dt = DB.GetTable(sql, pars.ToArray());
                BindGrid(dt);
                grpSag.Text = "📋  " + string.Join("  →  ", b.Path.Where(x => !string.IsNullOrEmpty(x)))
                            + $"  ({dt.Rows.Count} ana ürün)";
            }
            catch (Exception ex)
            {
                DurumYaz("❌ " + ex.Message, Color.FromArgb(255, 140, 140));
            }
        }

        // ════════════════════════════════════════════════════════════
        //  ÇİFT TIK → VARYANTLAR
        // ════════════════════════════════════════════════════════════
        private void SeciliyiVaryantOlarakAc()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Önce sağdaki listeden bir ana ürün seçin.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var row = dataGridView1.SelectedRows[0];
            var path = new string[Seviyeler.Length];
            for (int i = 0; i < Seviyeler.Length; i++)
                path[i] = row.Cells[Seviyeler[i]]?.Value?.ToString() ?? "";
            string height = row.Cells["Height"]?.Value?.ToString() ?? "";

            using (var frm = new FrmUrunDetay(path, height))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    AgacYukle();
                    if (tvKategori.SelectedNode != null)
                        TvKategori_AfterSelect(tvKategori, new TreeViewEventArgs(tvKategori.SelectedNode));
                }
            }
        }

        // ════════════════════════════════════════════════════════════
        //  YENİDEN ADLANDIR
        // ════════════════════════════════════════════════════════════
        private void YenidenAdlandir(TreeNode node)
        {
            if (node == null) return;
            var b = (NodeBilgi)node.Tag;
            string eski = b.Path[b.Seviye];
            string kolon = Seviyeler[b.Seviye];

            string yeni = MetinDialog($"'{SeviyeAdlari[b.Seviye]}' düğümünü yeniden adlandır:", eski);
            if (string.IsNullOrWhiteSpace(yeni) || yeni == eski) return;

            var whereParts = new List<string>();
            var pars = new List<SqlParameter>();
            for (int i = 0; i < b.Seviye; i++)
            {
                whereParts.Add("[" + Seviyeler[i] + "]=@p" + i);
                pars.Add(new SqlParameter("@p" + i, b.Path[i]));
            }
            whereParts.Add("[" + kolon + "]=@old");
            pars.Add(new SqlParameter("@old", eski));
            pars.Add(new SqlParameter("@new", yeni));

            string sql = $"UPDATE dbo.Urun SET [{kolon}]=@new WHERE " + string.Join(" AND ", whereParts);

            if (MessageBox.Show($"'{eski}' → '{yeni}'\n\nBu düğüme bağlı tüm ürünler güncellenecek.\nDevam edilsin mi?",
                "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                int n = DB.Execute(sql, pars.ToArray());
                DurumYaz($"✅ {n} kayıt güncellendi.", Color.FromArgb(140, 240, 160));
                AgacYukle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  DÜĞÜMÜN ALTINA YENİ EKLE
        // ════════════════════════════════════════════════════════════
        private void DugumeAltEkle(TreeNode node)
        {
            if (node == null) return;
            var b = (NodeBilgi)node.Tag;

            // Bu düğümün path'iyle yeni ürün formunu doldur,
            // kalan kategorileri kullanıcı dolduracak
            var v = new string[7];
            for (int i = 0; i < b.Path.Length; i++) v[i] = b.Path[i];

            using (var frm = new FrmYeniUrun(v))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK) AgacYukle();
            }
        }

        // ════════════════════════════════════════════════════════════
        //  YENİ ÜRÜN (alttaki büyük buton)
        // ════════════════════════════════════════════════════════════
        private void BtnYeniUrun_Click(object sender, EventArgs e)
        {
            string[] varsayilan = new string[7];
            if (tvKategori.SelectedNode != null)
            {
                var b = (NodeBilgi)tvKategori.SelectedNode.Tag;
                for (int i = 0; i < b.Path.Length; i++) varsayilan[i] = b.Path[i];
            }

            using (var frm = new FrmYeniUrun(varsayilan))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK) AgacYukle();
            }
        }

        // ════════════════════════════════════════════════════════════
        //  HIZLI ARAMA — sadece ana ürünler (Rev boş)
        // ════════════════════════════════════════════════════════════
        private void BtnTasarimSearch_Click(object sender, EventArgs e)
        {
            string raw = txtTasarimSearch.Text ?? "";
            string withHyphen = Regex.Replace(raw.Trim(), @"\s+", "");
            if (string.IsNullOrWhiteSpace(withHyphen)) { MessageBox.Show("Arama metni girin."); return; }
            string noHyphen = withHyphen.Replace("-", "");

            string secimler = string.Join(",", Seviyeler.Select(k => "[" + k + "]"));
            string sql = $@"
SELECT {secimler}, Height,
       ISNULL(Rev,'')      AS Rev,
       ISNULL(Tasarim,'')  AS Tasarim,
       ISNULL(UrunKodu,'') AS UrunKodu
FROM dbo.Urun
WHERE
    (Rev IS NULL OR LTRIM(RTRIM(Rev))='')
    AND (
        REPLACE(REPLACE(ISNULL(Tasarim , ''), ' ', ''), '-', '') = @noHyphen
        OR REPLACE(ISNULL(Tasarim , ''), ' ', '')               = @withHyphen
        OR ISNULL(Tasarim , '')                                  = @withHyphen
        OR REPLACE(REPLACE(ISNULL(UrunKodu, ''), ' ', ''), '-', '') = @noHyphen
        OR ISNULL(UrunKodu, '')                                  = @withHyphen
    )
ORDER BY {secimler}, Height";

            try
            {
                var dt = DB.GetTable(sql,
                    new SqlParameter("@withHyphen", SqlDbType.NVarChar, 500) { Value = withHyphen },
                    new SqlParameter("@noHyphen", SqlDbType.NVarChar, 500) { Value = noHyphen });
                BindGrid(dt);
                DurumYaz(dt.Rows.Count == 0 ? $"⚠ '{raw}' bulunamadı." : $"✅ {dt.Rows.Count} eşleşme.",
                    dt.Rows.Count == 0 ? Color.FromArgb(255, 200, 100) : Color.FromArgb(140, 240, 160));
            }
            catch (Exception ex) { MessageBox.Show("SQL hatası: " + ex.Message); }
        }

        // ════════════════════════════════════════════════════════════
        //  GRID — içeriğe göre kolon genişliği
        // ════════════════════════════════════════════════════════════
        private void BindGrid(DataTable dt)
        {
            dataGridView1.DataSource = dt;
            var headers = new Dictionary<string, string>
            {
                ["Design"] = "Tasarım",
                ["Sector"] = "Sektör",
                ["ProductType"] = "Ürün Tipi",
                ["StructureType"] = "Yapı Tipi",
                ["VoltageAyak"] = "Voltaj/Ayak",
                ["Foundation"] = "Temel",
                ["Tower"] = "Kule",
                ["Height"] = "Yükseklik",
                ["Rev"] = "Rev",
                ["Tasarim"] = "Tasarım Kodu",
                ["UrunKodu"] = "Ürün Kodu",
                ["Varyant"] = "Varyant"
            };
            foreach (var kv in headers)
                if (dataGridView1.Columns[kv.Key] != null)
                    dataGridView1.Columns[kv.Key].HeaderText = kv.Value;

            if (dataGridView1.Columns["UrunKodu"] != null)
            {
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 220);
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.Font = new Font("Consolas", 9.5f, FontStyle.Bold);
            }
            if (dataGridView1.Columns["Varyant"] != null)
            {
                dataGridView1.Columns["Varyant"].DefaultCellStyle.BackColor = Color.FromArgb(220, 240, 255);
                dataGridView1.Columns["Varyant"].DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                dataGridView1.Columns["Varyant"].DefaultCellStyle.ForeColor = Color.FromArgb(0, 80, 160);
                dataGridView1.Columns["Varyant"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dataGridView1.Columns["Height"] != null)
            {
                dataGridView1.Columns["Height"].DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                dataGridView1.Columns["Height"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // İçeriğe göre otomatik sığdır
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.AutoSizeMode = DataGridViewAutoSizeMode.AllCells;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Width > 280) col.Width = 280;
                if (col.Width < 70) col.Width = 70;
            }

            lblKayitSayisi.Text = $"📊 {dt.Rows.Count} ana ürün";
        }

        // ════════════════════════════════════════════════════════════
        //  Yardımcılar
        // ════════════════════════════════════════════════════════════
        public static string MetinDialog(string prompt, string defaultText = "")
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Giriş"; dlg.Size = new Size(440, 180);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false; dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(245, 245, 250);
                var lbl = new Label { Text = prompt, Location = new Point(14, 14), AutoSize = true, Font = new Font("Segoe UI", 10F) };
                var tx = new TextBox { Location = new Point(14, 50), Size = new Size(390, 28), Font = new Font("Segoe UI", 11F), Text = defaultText, BorderStyle = BorderStyle.FixedSingle };
                var ok = new Button { Text = "✔ Tamam", Size = new Size(110, 36), Location = new Point(184, 90), BackColor = Color.FromArgb(46, 139, 87), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.OK, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
                var ip = new Button { Text = "✖ İptal", Size = new Size(110, 36), Location = new Point(298, 90), BackColor = Color.FromArgb(160, 60, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
                ok.FlatAppearance.BorderSize = 0; ip.FlatAppearance.BorderSize = 0;
                dlg.Controls.AddRange(new Control[] { lbl, tx, ok, ip });
                dlg.AcceptButton = ok; dlg.CancelButton = ip;
                tx.SelectAll();
                return dlg.ShowDialog() == DialogResult.OK ? tx.Text?.Trim() : null;
            }
        }

        public static string SonrakiRevUret(string[] path, string height)
        {
            try
            {
                var pars = new List<SqlParameter>();
                var w = new List<string>();
                for (int i = 0; i < Seviyeler.Length; i++)
                {
                    w.Add("[" + Seviyeler[i] + "]=@p" + i);
                    pars.Add(new SqlParameter("@p" + i, path[i] ?? ""));
                }
                w.Add("Height=@h"); pars.Add(new SqlParameter("@h", height ?? ""));

                string sql = "SELECT Rev FROM dbo.Urun WHERE " + string.Join(" AND ", w);
                var dt = DB.GetTable(sql, pars.ToArray());
                var mevcut = dt.AsEnumerable()
                    .Select(r => (r["Rev"]?.ToString() ?? "").Trim().ToUpper())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();

                if (mevcut.Count == 0) return "";
                bool hepsiSayi = mevcut.All(s => int.TryParse(s, out _));
                if (hepsiSayi)
                {
                    int max = mevcut.Max(s => int.Parse(s));
                    return (max + 1).ToString();
                }
                var harfler = mevcut.Where(s => s.Length == 1 && s[0] >= 'A' && s[0] <= 'Z').ToList();
                if (harfler.Count > 0)
                {
                    char max = harfler.Max(s => s[0]);
                    char yeni = (char)(max + 1);
                    if (yeni > 'Z') return "A1";
                    return yeni.ToString();
                }
                return (mevcut.Count + 1).ToString();
            }
            catch { return ""; }
        }

        private void DurumYaz(string m, Color c)
        { try { lblStatus.Text = m; lblStatus.ForeColor = c; } catch { } }

        private class NodeBilgi
        {
            public int Seviye;
            public string[] Path;
            public bool YaprakMi;
            public int Adet;
            public int AltToplam;
        }
    }
}