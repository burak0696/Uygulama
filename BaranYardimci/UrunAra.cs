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
        public static readonly string[] Seviyeler =
            { "Design", "Sector", "ProductType", "StructureType", "VoltageAyak", "Foundation", "Tower" };
        public static readonly string[] SeviyeAdlari =
            { "Tasarım", "Sektör", "Ürün Tipi", "Yapı Tipi", "Voltaj/Ayak", "Temel", "Kule" };

        private const string PLACEHOLDER = "🔍 Ağaçta filtrele...";
        private const string ADMIN_SIFRE = "12345";
        private bool _duzenleModu = false;
        private string[] _sonSecilenPath = null;   // ⭐ yenileme sonrası geri seçim için

        public UrunAra()
        {
            InitializeComponent();
            AttachEvents();
            AgacYukle();
            ModRozetiGuncelle();
            DurumYaz("Hazır. Soldan kategori seçin.", Color.FromArgb(180, 200, 180));
        }

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
                if (!_duzenleModu) { e.Cancel = true; DurumYaz("🔒 Yönetici modu kapalı.", Color.FromArgb(255, 200, 100)); return; }
                bool aktif = tvKategori.SelectedNode != null;
                mnuYenidenAdlandir.Enabled = aktif;
                mnuYeniAlt.Enabled = aktif;
                mnuSil.Enabled = aktif;
            };

            mnuYenidenAdlandir.Click += (s, e) => YenidenAdlandir(tvKategori.SelectedNode);
            mnuYeniAlt.Click += (s, e) => DugumeAltEkle(tvKategori.SelectedNode);
            mnuSil.Click += (s, e) => DugumSil(tvKategori.SelectedNode);
            mnuYenile.Click += (s, e) => AgacYenile();

            btnAgacSekillendir.Click += BtnAgacSekillendir_Click;

            txtAgacFiltre.GotFocus += (s, e) =>
            { if (txtAgacFiltre.Text == PLACEHOLDER) { txtAgacFiltre.Text = ""; txtAgacFiltre.ForeColor = Color.Black; } };
            txtAgacFiltre.LostFocus += (s, e) =>
            { if (string.IsNullOrWhiteSpace(txtAgacFiltre.Text)) { txtAgacFiltre.Text = PLACEHOLDER; txtAgacFiltre.ForeColor = Color.Gray; } };
            txtAgacFiltre.TextChanged += (s, e) =>
            { if (txtAgacFiltre.Text == PLACEHOLDER) return; AgacFiltrele(txtAgacFiltre.Text); };

            btnDetay.Click += (s, e) => SeciliyiVaryantOlarakAc();
            btnYenile.Click += (s, e) => AgacYenile();
            btnKapat.Click += (s, e) => this.Close();
            btnTasarimSearch.Click += BtnTasarimSearch_Click;
            txtTasarimSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                { BtnTasarimSearch_Click(s, EventArgs.Empty); e.Handled = true; e.SuppressKeyPress = true; }
            };
            dataGridView1.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) SeciliyiVaryantOlarakAc(); };
        }

        // ════════════════════════════════════════════════════════════
        //  YÖNETİCİ MODU
        // ════════════════════════════════════════════════════════════
        private void BtnAgacSekillendir_Click(object sender, EventArgs e)
        {
            if (_duzenleModu)
            {
                _duzenleModu = false;
                ModRozetiGuncelle();
                tvKategori.BackColor = Color.White;
                DurumYaz("👁 Görüntüleme moduna dönüldü.", Color.FromArgb(180, 200, 180));
                return;
            }
            string s = SifreDialog();
            if (s == null) return;
            if (s != ADMIN_SIFRE)
            {
                MessageBox.Show("❌  Yanlış şifre!", "Erişim Reddedildi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _duzenleModu = true;
            ModRozetiGuncelle();
            tvKategori.BackColor = Color.FromArgb(255, 248, 230);
            DurumYaz("🔧 Yönetici modu AÇIK.", Color.FromArgb(255, 210, 80));
            MessageBox.Show("🔧 YÖNETİCİ MODU AÇIK\n\nSAĞ TIK ile:\n• ✏ Yeniden Adlandır\n• ➕ Altına Yeni Ekle\n• 🗑 Sil",
                "Yönetici", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ModRozetiGuncelle()
        {
            if (_duzenleModu)
            {
                lblModRozet.Text = "🔧  YÖNETİCİ MODU";
                lblModRozet.BackColor = Color.FromArgb(200, 60, 40);
                btnAgacSekillendir.Text = "🔓  KİLİTLE";
                btnAgacSekillendir.BackColor = Color.FromArgb(200, 60, 40);
            }
            else
            {
                lblModRozet.Text = "👁  GÖRÜNTÜLEME MODU";
                lblModRozet.BackColor = Color.FromArgb(60, 100, 140);
                btnAgacSekillendir.Text = "🔒  AĞAÇ ŞEKİLLENDİR  (Yönetici)";
                btnAgacSekillendir.BackColor = Color.FromArgb(180, 100, 0);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  AĞAÇ — ⭐ AgacYenile = seçimi koruyarak yenile
        // ════════════════════════════════════════════════════════════
        public void AgacYenile()
        {
            // Mevcut seçimi sakla
            if (tvKategori.SelectedNode != null)
                _sonSecilenPath = ((NodeBilgi)tvKategori.SelectedNode.Tag).Path;

            AgacYukle();

            // Geri seç
            if (_sonSecilenPath != null)
            {
                var n = NodeYoluyleBul(_sonSecilenPath);
                if (n != null)
                {
                    tvKategori.SelectedNode = n;
                    n.EnsureVisible();
                    // Grid'i de güncelle
                    TvKategori_AfterSelect(tvKategori, new TreeViewEventArgs(n));
                }
            }
        }

        private TreeNode NodeYoluyleBul(string[] path)
        {
            TreeNodeCollection col = tvKategori.Nodes;
            TreeNode son = null;
            foreach (var ad in path)
            {
                string aranan = string.IsNullOrEmpty(ad) ? "(boş)" : ad;
                TreeNode bulundu = null;
                foreach (TreeNode t in col)
                    if (string.Equals(t.Name, aranan, StringComparison.OrdinalIgnoreCase))
                    { bulundu = t; break; }
                if (bulundu == null) return son;
                son = bulundu;
                col = bulundu.Nodes;
            }
            return son;
        }

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
                lblIstatistik.Text = $"📊  {toplamUrun} ürün  ·  {dt.Rows.Count} kule  ·  {tvKategori.Nodes.Count} ana tasarım";
                DurumYaz($"✅ Ağaç yüklendi.", Color.FromArgb(140, 240, 160));
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
                    if (string.Equals(t.Name, ad, StringComparison.OrdinalIgnoreCase)) { bulundu = t; break; }
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
        //  GRİD — ANA ÜRÜNLER + VARYANT SAYISI
        // ════════════════════════════════════════════════════════════
        private int _aktifPathSeviye = 0;   // ⭐ BindGrid hangi seviyeleri gizleyecek bilsin

        private void TvKategori_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;
            var b = (NodeBilgi)e.Node.Tag;
            _sonSecilenPath = b.Path;
            _aktifPathSeviye = b.Path.Length;

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
       ISNULL(u.Height,'')   AS Height,
       ISNULL(u.Rev,'')      AS Rev,
       ISNULL(u.Tasarim,'')  AS Aciklama,
       ISNULL(u.UrunKodu,'') AS UrunKodu,
       (SELECT COUNT(*) FROM dbo.Urun v
          WHERE v.Design=u.Design AND v.Sector=u.Sector AND v.ProductType=u.ProductType
            AND v.StructureType=u.StructureType AND v.VoltageAyak=u.VoltageAyak
            AND v.Foundation=u.Foundation AND v.Tower=u.Tower
            AND ISNULL(v.Height,'')=ISNULL(u.Height,'')
            AND v.Rev IS NOT NULL AND LTRIM(RTRIM(v.Rev))<>'') AS Varyant
FROM dbo.Urun u
WHERE {string.Join(" AND ", whereParts)}
  AND (u.Rev IS NULL OR LTRIM(RTRIM(u.Rev))='')
ORDER BY u.Design, u.Sector, u.ProductType, u.StructureType, u.VoltageAyak, u.Foundation, u.Tower, u.Height";

            try
            {
                var dt = DB.GetTable(sql, pars.ToArray());
                BindGrid(dt);
                grpSag.Text = "📋  " + string.Join("  →  ", b.Path.Where(x => !string.IsNullOrEmpty(x)))
                            + $"   ·   {dt.Rows.Count} ana ürün";
            }
            catch (Exception ex) { DurumYaz("❌ " + ex.Message, Color.FromArgb(255, 140, 140)); }
        }

        // ════════════════════════════════════════════════════════════
        //  VARYANT EKRANI
        // ════════════════════════════════════════════════════════════
        private void SeciliyiVaryantOlarakAc()
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Index >= 0)
            {
                var row = dataGridView1.SelectedRows[0];
                if (row.Cells[Seviyeler[0]]?.Value != null)
                {
                    var path = new string[Seviyeler.Length];
                    for (int i = 0; i < Seviyeler.Length; i++)
                        path[i] = row.Cells[Seviyeler[i]]?.Value?.ToString() ?? "";
                    string height = row.Cells["Height"]?.Value?.ToString() ?? "";
                    using (var frm = new FrmUrunDetay(path, height))
                    {
                        frm.ShowDialog(this);
                        AgacYenile();  // ⭐ Seçimi koruyarak yenile
                    }
                    return;
                }
            }
            if (tvKategori.SelectedNode != null)
            {
                var b = (NodeBilgi)tvKategori.SelectedNode.Tag;
                if (b.YaprakMi)
                {
                    using (var frm = new FrmUrunDetay(b.Path, ""))
                    {
                        frm.ShowDialog(this);
                        AgacYenile();
                    }
                    return;
                }
            }
            MessageBox.Show("Önce bir ana ürün veya Kule (📦) seçin.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ════════════════════════════════════════════════════════════
        //  YÖNETİCİ — Adlandır / Sil / Ekle
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

            if (MessageBox.Show($"'{eski}' → '{yeni}'\n\nDevam?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                int n = DB.Execute($"UPDATE dbo.Urun SET [{kolon}]=@new WHERE " + string.Join(" AND ", whereParts), pars.ToArray());
                DurumYaz($"✅ {n} kayıt güncellendi.", Color.FromArgb(140, 240, 160));
                // Yenilemede path da değişti
                if (_sonSecilenPath != null && b.Seviye < _sonSecilenPath.Length)
                    _sonSecilenPath[b.Seviye] = yeni;
                AgacYenile();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        private void DugumSil(TreeNode node)
        {
            if (node == null) return;
            var b = (NodeBilgi)node.Tag;
            if (MessageBox.Show($"⚠ TEHLİKE!\n\nYol: {string.Join(" → ", b.Path)}\nEtkilenen: {b.AltToplam}\n\nDevam?",
                "İlk Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            string teyit = MetinDialog($"Onay için '{b.Path[b.Seviye]}' yazın:", "");
            if (teyit == null || teyit != b.Path[b.Seviye])
            { MessageBox.Show("İptal."); return; }

            var whereParts = new List<string>();
            var pars = new List<SqlParameter>();
            for (int i = 0; i <= b.Seviye; i++)
            {
                whereParts.Add("[" + Seviyeler[i] + "]=@p" + i);
                pars.Add(new SqlParameter("@p" + i, b.Path[i]));
            }
            try
            {
                int n = DB.Execute("DELETE FROM dbo.Urun WHERE " + string.Join(" AND ", whereParts), pars.ToArray());
                MessageBox.Show($"✅ {n} kayıt silindi.", "Tamam");
                _sonSecilenPath = null; // seçim kayboldu
                AgacYukle();
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
        }

        private void DugumeAltEkle(TreeNode node)
        {
            if (node == null) return;
            var b = (NodeBilgi)node.Tag;
            if (b.YaprakMi)
            {
                using (var frm = new FrmUrunDetay(b.Path, ""))
                {
                    frm.ShowDialog(this);
                    AgacYenile();
                }
                return;
            }
            var yeniPath = new string[7];
            for (int i = 0; i < b.Path.Length; i++) yeniPath[i] = b.Path[i];
            for (int s = b.Seviye + 1; s < Seviyeler.Length; s++)
            {
                string deger = MetinDialog($"'{SeviyeAdlari[s]}' için yeni değer girin:", "");
                if (string.IsNullOrWhiteSpace(deger)) return;
                yeniPath[s] = deger.Trim();
            }
            using (var frm = new FrmUrunDetay(yeniPath, ""))
            {
                frm.ShowDialog(this);
                _sonSecilenPath = yeniPath;
                AgacYenile();
            }
        }

        // ════════════════════════════════════════════════════════════
        //  HIZLI ARAMA
        // ════════════════════════════════════════════════════════════
        private void BtnTasarimSearch_Click(object sender, EventArgs e)
        {
            string raw = txtTasarimSearch.Text ?? "";
            string withHyphen = Regex.Replace(raw.Trim(), @"\s+", "");
            if (string.IsNullOrWhiteSpace(withHyphen)) { MessageBox.Show("Arama metni girin."); return; }
            string noHyphen = withHyphen.Replace("-", "");

            string secimler = string.Join(",", Seviyeler.Select(k => "[" + k + "]"));
            string sql = $@"
SELECT {secimler},
       ISNULL(Height,'')   AS Height,
       ISNULL(Rev,'')      AS Rev,
       ISNULL(Tasarim,'')  AS Aciklama,
       ISNULL(UrunKodu,'') AS UrunKodu,
       0 AS Varyant
FROM dbo.Urun
WHERE (Rev IS NULL OR LTRIM(RTRIM(Rev))='')
  AND (
        REPLACE(REPLACE(ISNULL(Tasarim , ''), ' ', ''), '-', '') LIKE '%' + @noHyphen + '%'
        OR ISNULL(Tasarim , '') LIKE '%' + @withHyphen + '%'
        OR REPLACE(REPLACE(ISNULL(UrunKodu, ''), ' ', ''), '-', '') LIKE '%' + @noHyphen + '%'
        OR ISNULL(UrunKodu, '') LIKE '%' + @withHyphen + '%'
  )
ORDER BY {secimler}, Height";
            try
            {
                _aktifPathSeviye = 0; // arama → tüm kategorileri göster
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
        //  GRID — ⭐ kategori kolonlarını gizle, Varyant öne al
        // ════════════════════════════════════════════════════════════
        private void BindGrid(DataTable dt)
        {
            dataGridView1.DataSource = dt;

            // Veri kolonları (her zaman gizli)
            string[] hepGizli = { "Height", "Rev", "Aciklama" };
            foreach (var g in hepGizli)
                if (dataGridView1.Columns[g] != null)
                    dataGridView1.Columns[g].Visible = false;

            // Kategori kolonları — path zaten breadcrumb'da → seçilen seviyeye kadar gizle
            // Sadece path'in DERIN olmadığı seviyeler gözüksün
            for (int i = 0; i < Seviyeler.Length; i++)
            {
                if (dataGridView1.Columns[Seviyeler[i]] == null) continue;
                // Path o seviyeyi içeriyorsa kolonu gizle
                dataGridView1.Columns[Seviyeler[i]].Visible = (i >= _aktifPathSeviye);
            }

            var headers = new Dictionary<string, string>
            {
                ["Design"] = "Tasarım",
                ["Sector"] = "Sektör",
                ["ProductType"] = "Ürün Tipi",
                ["StructureType"] = "Yapı Tipi",
                ["VoltageAyak"] = "Voltaj/Ayak",
                ["Foundation"] = "Temel",
                ["Tower"] = "Kule",
                ["UrunKodu"] = "Ürün Kodu",
                ["Varyant"] = "Varyant"
            };
            foreach (var kv in headers)
                if (dataGridView1.Columns[kv.Key] != null)
                    dataGridView1.Columns[kv.Key].HeaderText = kv.Value;

            // ⭐ Kolon sıralaması: Varyant - UrunKodu - Tower ... (önce sağdakiler)
            int idx = 0;
            if (dataGridView1.Columns["Varyant"] != null) dataGridView1.Columns["Varyant"].DisplayIndex = idx++;
            if (dataGridView1.Columns["UrunKodu"] != null) dataGridView1.Columns["UrunKodu"].DisplayIndex = idx++;
            // Sonra kategoriler ters sıralı (Tower önce)
            for (int i = Seviyeler.Length - 1; i >= 0; i--)
                if (dataGridView1.Columns[Seviyeler[i]] != null && dataGridView1.Columns[Seviyeler[i]].Visible)
                    dataGridView1.Columns[Seviyeler[i]].DisplayIndex = idx++;

            // Stil
            if (dataGridView1.Columns["UrunKodu"] != null)
            {
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 220);
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.Font = new Font("Consolas", 10f, FontStyle.Bold);
            }
            if (dataGridView1.Columns["Varyant"] != null)
            {
                dataGridView1.Columns["Varyant"].DefaultCellStyle.BackColor = Color.FromArgb(220, 240, 255);
                dataGridView1.Columns["Varyant"].DefaultCellStyle.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                dataGridView1.Columns["Varyant"].DefaultCellStyle.ForeColor = Color.FromArgb(0, 80, 160);
                dataGridView1.Columns["Varyant"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["Varyant"].Frozen = true; // ⭐ kaydırınca sabit kalsın
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                if (col.Visible) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (!col.Visible) continue;
                if (col.Width > 300) col.Width = 300;
                if (col.Width < 80) col.Width = 80;
            }
            // Varyant ve UrunKodu için minimum
            if (dataGridView1.Columns["Varyant"] != null) dataGridView1.Columns["Varyant"].Width = 90;
            if (dataGridView1.Columns["UrunKodu"] != null && dataGridView1.Columns["UrunKodu"].Width < 200)
                dataGridView1.Columns["UrunKodu"].Width = 200;

            lblKayitSayisi.Text = $"📊 {dt.Rows.Count} ana ürün";
        }

        // ════════════════════════════════════════════════════════════
        //  Yardımcı dialoglar
        // ════════════════════════════════════════════════════════════
        public static string MetinDialog(string prompt, string defaultText = "")
        {
            using (var dlg = new Form())
            {
                dlg.Text = "Giriş";
                dlg.Size = new Size(560, 280);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false; dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(245, 245, 250);

                var lbl = new Label
                {
                    Text = prompt,
                    Location = new Point(16, 16),
                    Size = new Size(520, 110),
                    Font = new Font("Segoe UI", 10.5F),
                    ForeColor = Color.FromArgb(30, 50, 80)
                };
                var tx = new TextBox
                {
                    Location = new Point(16, 140),
                    Size = new Size(520, 32),
                    Font = new Font("Segoe UI", 12F),
                    Text = defaultText ?? "",
                    BorderStyle = BorderStyle.FixedSingle
                };
                var ok = new Button { Text = "✔ Tamam", Size = new Size(140, 40), Location = new Point(244, 190), BackColor = Color.FromArgb(46, 139, 87), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.OK, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
                var ip = new Button { Text = "✖ İptal", Size = new Size(140, 40), Location = new Point(394, 190), BackColor = Color.FromArgb(160, 60, 60), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
                ok.FlatAppearance.BorderSize = 0; ip.FlatAppearance.BorderSize = 0;
                dlg.Controls.AddRange(new Control[] { lbl, tx, ok, ip });
                dlg.AcceptButton = ok; dlg.CancelButton = ip;
                tx.SelectAll();
                return dlg.ShowDialog() == DialogResult.OK ? tx.Text?.Trim() : null;
            }
        }

        public static string SifreDialog()
        {
            using (var dlg = new Form())
            {
                dlg.Text = "🔒 Yönetici"; dlg.Size = new Size(420, 220);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false; dlg.MinimizeBox = false;
                dlg.BackColor = Color.FromArgb(30, 35, 50);
                var lbl = new Label { Text = "🔒  Şifre:", Location = new Point(14, 18), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(255, 210, 80) };
                var tx = new TextBox { Location = new Point(14, 60), Size = new Size(380, 32), Font = new Font("Segoe UI", 14F, FontStyle.Bold), BorderStyle = BorderStyle.FixedSingle, PasswordChar = '●', TextAlign = HorizontalAlignment.Center };
                var ok = new Button { Text = "🔓 Giriş", Size = new Size(130, 40), Location = new Point(140, 120), BackColor = Color.FromArgb(46, 139, 87), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.OK, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
                var ip = new Button { Text = "İptal", Size = new Size(130, 40), Location = new Point(274, 120), BackColor = Color.FromArgb(100, 100, 110), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
                ok.FlatAppearance.BorderSize = 0; ip.FlatAppearance.BorderSize = 0;
                dlg.Controls.AddRange(new Control[] { lbl, tx, ok, ip });
                dlg.AcceptButton = ok; dlg.CancelButton = ip;
                tx.Focus();
                return dlg.ShowDialog() == DialogResult.OK ? tx.Text : null;
            }
        }

        // ════════════════════════════════════════════════════════════
        //  REV ATAMA
        // ════════════════════════════════════════════════════════════
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
                w.Add("ISNULL(Height,'')=@h");
                pars.Add(new SqlParameter("@h", height ?? ""));

                string sql = "SELECT COUNT(*) AS Toplam, " +
                             "SUM(CASE WHEN Rev IS NOT NULL AND LTRIM(RTRIM(Rev))<>'' THEN 1 ELSE 0 END) AS Varyantli " +
                             "FROM dbo.Urun WHERE " + string.Join(" AND ", w);
                var dt = DB.GetTable(sql, pars.ToArray());
                int toplam = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Rows[0]["Toplam"]);
                int varyantli = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Rows[0]["Varyantli"] ?? 0);

                if (toplam == 0) return "";          // ilk ana
                if (varyantli == 0) return "1";      // ana var, ilk varyant

                var revSql = "SELECT Rev FROM dbo.Urun WHERE " + string.Join(" AND ", w)
                           + " AND Rev IS NOT NULL AND LTRIM(RTRIM(Rev))<>''";
                var revDt = DB.GetTable(revSql, pars.ToArray());
                var mevcut = revDt.AsEnumerable()
                    .Select(r => (r["Rev"]?.ToString() ?? "").Trim().ToUpper()).ToList();

                if (mevcut.All(s => int.TryParse(s, out _)))
                    return (mevcut.Max(s => int.Parse(s)) + 1).ToString();

                var harfler = mevcut.Where(s => s.Length == 1 && s[0] >= 'A' && s[0] <= 'Z').ToList();
                if (harfler.Count > 0)
                {
                    char yeni = (char)(harfler.Max(s => s[0]) + 1);
                    return yeni > 'Z' ? "AA" : yeni.ToString();
                }
                return (mevcut.Count + 1).ToString();
            }
            catch { return "1"; }
        }

        // ════════════════════════════════════════════════════════════
        //  ⭐ ÜRÜN KODU — TEMİZ, KISA
        //  Format:  XXX-YYY-...-H{ht}-R{rev}
        //  Her parça max 6 char, sadece A-Z/0-9
        // ════════════════════════════════════════════════════════════
        public static string UrunKoduUret(string[] path, string height, string rev)
        {
            string Kisalt(string s, int max)
            {
                if (string.IsNullOrWhiteSpace(s)) return "X";
                var temiz = Regex.Replace(s.Trim().ToUpper(), @"[^A-Z0-9]", "");
                if (temiz.Length == 0) return "X";
                return temiz.Length > max ? temiz.Substring(0, max) : temiz;
            }

            var parts = new List<string>();
            foreach (var p in path) parts.Add(Kisalt(p, 6));
            if (!string.IsNullOrWhiteSpace(height)) parts.Add("H" + Kisalt(height, 5));
            if (!string.IsNullOrWhiteSpace(rev)) parts.Add("R" + Kisalt(rev, 3));
            return string.Join("-", parts);
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