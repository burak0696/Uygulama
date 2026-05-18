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
        // ── Kategori hiyerarşisi (DB kolon adları, sırayla) ──────────
        private static readonly string[] Seviyeler = new[]
        {
            "Design", "Sector", "ProductType", "StructureType",
            "VoltageAyak", "Foundation", "Tower"
        };
        private static readonly string[] SeviyeAdlari = new[]
        {
            "Tasarım", "Sektör", "Ürün Tipi", "Yapı Tipi",
            "Voltaj/Ayak", "Temel", "Kule"
        };

        private const string PLACEHOLDER = "🔍 Ağaçta filtrele...";

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
            tvKategori.NodeMouseDoubleClick += (s, e) => YapragiAc(e.Node);
            tvKategori.MouseDown += (s, e) =>
            {
                // Sağ tıkta o node'u seç
                if (e.Button == MouseButtons.Right)
                {
                    var n = tvKategori.GetNodeAt(e.X, e.Y);
                    if (n != null) tvKategori.SelectedNode = n;
                }
            };

            ctxAgac.Opening += (s, e) =>
            {
                bool aktif = chkDuzenle.Checked && tvKategori.SelectedNode != null;
                mnuYenidenAdlandir.Enabled = aktif;
                mnuSil.Enabled = aktif;
                if (!chkDuzenle.Checked)
                {
                    e.Cancel = true; // Düzenle modu kapalıysa menü açma
                    DurumYaz("⚠ Önce 'Düzenle Modu'nu açın.", Color.FromArgb(255, 200, 100));
                }
            };

            mnuYenidenAdlandir.Click += (s, e) => YenidenAdlandir(tvKategori.SelectedNode);
            mnuSil.Click += (s, e) => DugumSil(tvKategori.SelectedNode);
            mnuYenile.Click += (s, e) => AgacYukle();

            chkDuzenle.CheckedChanged += (s, e) =>
            {
                if (chkDuzenle.Checked)
                    DurumYaz("🔧 Düzenle modu AÇIK — sağ tık menü aktif, dikkatli olun.", Color.FromArgb(255, 210, 80));
                else
                    DurumYaz("👁 Görüntüleme modu.", Color.FromArgb(180, 200, 180));
            };

            // Ağaç filtresi (placeholder mantığı)
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
            btnDetay.Click += (s, e) => YapragiAc(tvKategori.SelectedNode);
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
                var row = dataGridView1.Rows[e.RowIndex];
                // Bu satırdaki Tower'a karşılık gelen ağaç düğümünü bul ve aç
                var path = Seviyeler.Select(k => row.Cells[k]?.Value?.ToString() ?? "").ToArray();
                var node = DugumeGit(path);
                if (node != null) { tvKategori.SelectedNode = node; YapragiAc(node); }
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
                    DugumEkleVeyaBul(path, adet);
                }

                // Sayıları derle (her node'da alt yaprak sayısı)
                foreach (TreeNode root in tvKategori.Nodes) ToplamGuncelle(root);

                tvKategori.EndUpdate();

                int toplamUrun = dt.AsEnumerable().Sum(x => Convert.ToInt32(x["Adet"]));
                int kategoriSayisi = ToplamDugumSay(tvKategori.Nodes);
                lblKayitSayisi.Text = $"📊 {toplamUrun} ürün · {kategoriSayisi} kategori düğümü";

                DurumYaz($"✅ Ağaç yüklendi — {dt.Rows.Count} yaprak.", Color.FromArgb(140, 240, 160));
            }
            catch (Exception ex)
            {
                DurumYaz("❌ Ağaç yüklenemedi: " + ex.Message, Color.FromArgb(255, 140, 140));
                MessageBox.Show("Ağaç yüklenemedi:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { Cursor.Current = Cursors.Default; }
        }

        private int ToplamDugumSay(TreeNodeCollection nodes)
        {
            int n = 0;
            foreach (TreeNode tn in nodes) { n++; n += ToplamDugumSay(tn.Nodes); }
            return n;
        }

        /// <summary>Verilen path'i ağaca ekler veya mevcut yaprağı günceller.</summary>
        private TreeNode DugumEkleVeyaBul(string[] path, int yaprakAdet)
        {
            TreeNodeCollection current = tvKategori.Nodes;
            TreeNode son = null;
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
                    DugumStil(bulundu);
                    current.Add(bulundu);
                }
                son = bulundu;
                current = bulundu.Nodes;
            }
            return son;
        }

        private int ToplamGuncelle(TreeNode node)
        {
            var bilgi = (NodeBilgi)node.Tag;
            int toplam = bilgi.Adet;
            foreach (TreeNode c in node.Nodes) toplam += ToplamGuncelle(c);
            bilgi.AltToplam = toplam;
            // Görsel başlık
            string ikon = bilgi.YaprakMi ? "📦" : "📁";
            string sayi = bilgi.YaprakMi ? $" · {bilgi.Adet} vrn" : $" · {toplam}";
            node.Text = $"{ikon}  {(node.Name == "(boş)" ? "(boş)" : node.Name)}{sayi}";
            return toplam;
        }

        private void DugumStil(TreeNode n)
        {
            var b = (NodeBilgi)n.Tag;
            if (b.YaprakMi)
            {
                n.ForeColor = Color.FromArgb(140, 70, 0);
                n.NodeFont = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            }
            else
            {
                n.ForeColor = Color.FromArgb(30, 60, 110);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  AĞAÇ FİLTRESİ — basit, ad eşleşeni göster
        // ════════════════════════════════════════════════════════════
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
        //  AĞAÇ SEÇİMİ → GRİD'E ÜRÜNLERİ DOLDUR
        // ════════════════════════════════════════════════════════════
        private void TvKategori_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;
            var b = (NodeBilgi)e.Node.Tag;
            var whereParts = new List<string>();
            var pars = new List<SqlParameter>();
            for (int i = 0; i < b.Path.Length; i++)
            {
                whereParts.Add("[" + Seviyeler[i] + "]=@p" + i);
                pars.Add(new SqlParameter("@p" + i, b.Path[i]));
            }

            string sql = "SELECT " + string.Join(",", Seviyeler.Select(k => "[" + k + "]"))
                        + ", Height, Rev, Tasarim, UrunKodu FROM dbo.Urun WHERE "
                        + string.Join(" AND ", whereParts)
                        + " ORDER BY " + string.Join(",", Seviyeler.Select(k => "[" + k + "]"))
                        + ", Height, Rev";

            try
            {
                var dt = DB.GetTable(sql, pars.ToArray());
                BindGrid(dt);
                grpSag.Text = "📋  " + string.Join("  →  ", b.Path.Where(x => !string.IsNullOrEmpty(x)))
                            + $"  ({dt.Rows.Count} ürün)";
            }
            catch (Exception ex)
            {
                DurumYaz("❌ " + ex.Message, Color.FromArgb(255, 140, 140));
            }
        }

        // ════════════════════════════════════════════════════════════
        //  YAPRAK AÇ → FrmUrunDetay
        // ════════════════════════════════════════════════════════════
        private void YapragiAc(TreeNode node)
        {
            if (node == null) { MessageBox.Show("Önce ağaçtan bir düğüm seçin."); return; }
            var b = (NodeBilgi)node.Tag;
            if (!b.YaprakMi)
            {
                MessageBox.Show("Bu bir kategori dalı (📁).\nVaryantları görmek için yaprak (📦) düğüm seçin.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var frm = new FrmUrunDetay(b.Path))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK)
                    AgacYukle(); // Değişiklik olduysa ağacı yenile
            }
        }

        private TreeNode DugumeGit(string[] path)
        {
            TreeNodeCollection cur = tvKategori.Nodes;
            TreeNode son = null;
            foreach (var ad in path)
            {
                TreeNode bulundu = null;
                foreach (TreeNode t in cur)
                    if (string.Equals(t.Name, ad, StringComparison.OrdinalIgnoreCase))
                    { bulundu = t; break; }
                if (bulundu == null) return son;
                son = bulundu; cur = bulundu.Nodes;
            }
            return son;
        }

        // ════════════════════════════════════════════════════════════
        //  DÜZENLEME: YENİDEN ADLANDIR
        // ════════════════════════════════════════════════════════════
        private void YenidenAdlandir(TreeNode node)
        {
            if (node == null) return;
            var b = (NodeBilgi)node.Tag;
            string eski = b.Path[b.Seviye];
            string kolon = Seviyeler[b.Seviye];

            string yeni = MetinDialog($"'{SeviyeAdlari[b.Seviye]}' düğümünü yeniden adlandır:", eski);
            if (string.IsNullOrWhiteSpace(yeni) || yeni == eski) return;

            // WHERE: parent path tam eşleşmeli + bu seviye eski
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
        //  DÜZENLEME: SİL
        // ════════════════════════════════════════════════════════════
        private void DugumSil(TreeNode node)
        {
            if (node == null) return;
            var b = (NodeBilgi)node.Tag;
            var whereParts = new List<string>();
            var pars = new List<SqlParameter>();
            for (int i = 0; i <= b.Seviye; i++)
            {
                whereParts.Add("[" + Seviyeler[i] + "]=@p" + i);
                pars.Add(new SqlParameter("@p" + i, b.Path[i]));
            }
            string sql = "DELETE FROM dbo.Urun WHERE " + string.Join(" AND ", whereParts);

            if (MessageBox.Show(
                $"⚠ Bu düğüm ve TÜM alt ürünleri SİLİNECEK!\n\n" +
                $"Yol: {string.Join(" → ", b.Path)}\n" +
                $"Tahmini etkilenen kayıt: {b.AltToplam}\n\n" +
                "Devam edilsin mi?",
                "Tehlikeli İşlem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                int n = DB.Execute(sql, pars.ToArray());
                DurumYaz($"🗑 {n} kayıt silindi.", Color.FromArgb(255, 180, 100));
                AgacYukle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  YENİ ÜRÜN — 7 kategori + Height/Rev/Tasarim
        // ════════════════════════════════════════════════════════════
        private void BtnYeniUrun_Click(object sender, EventArgs e)
        {
            // Seçili düğümün path'ini varsayılan olarak kullan
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
        //  HIZLI ARAMA
        // ════════════════════════════════════════════════════════════
        private void BtnTasarimSearch_Click(object sender, EventArgs e)
        {
            string raw = txtTasarimSearch.Text ?? "";
            string withHyphen = Regex.Replace(raw.Trim(), @"\s+", "");
            if (string.IsNullOrWhiteSpace(withHyphen))
            { MessageBox.Show("Arama metni giriniz."); return; }
            string noHyphen = withHyphen.Replace("-", "");

            const string sql = @"
SELECT Design, Sector, ProductType, StructureType, VoltageAyak, Foundation, Tower, Height, Rev, Tasarim, UrunKodu
FROM dbo.Urun
WHERE
    REPLACE(REPLACE(ISNULL(Tasarim , ''), ' ', ''), '-', '') = @noHyphen
    OR REPLACE(ISNULL(Tasarim , ''), ' ', '')               = @withHyphen
    OR ISNULL(Tasarim , '')                                  = @withHyphen
    OR REPLACE(REPLACE(ISNULL(UrunKodu, ''), ' ', ''), '-', '') = @noHyphen
    OR ISNULL(UrunKodu, '')                                  = @withHyphen
ORDER BY Design, Sector, ProductType";

            try
            {
                var dt = DB.GetTable(sql,
                    new SqlParameter("@withHyphen", SqlDbType.NVarChar, 500) { Value = withHyphen },
                    new SqlParameter("@noHyphen", SqlDbType.NVarChar, 500) { Value = noHyphen });
                BindGrid(dt);
                if (dt.Rows.Count == 0)
                    DurumYaz($"⚠ '{raw}' bulunamadı.", Color.FromArgb(255, 200, 100));
                else
                    DurumYaz($"✅ {dt.Rows.Count} eşleşme.", Color.FromArgb(140, 240, 160));
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  GRID
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
                ["UrunKodu"] = "Ürün Kodu"
            };
            foreach (var kv in headers)
                if (dataGridView1.Columns[kv.Key] != null)
                    dataGridView1.Columns[kv.Key].HeaderText = kv.Value;

            if (dataGridView1.Columns["UrunKodu"] != null)
            {
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 220);
                dataGridView1.Columns["UrunKodu"].DefaultCellStyle.Font = new Font("Consolas", 9.5f, FontStyle.Bold);
            }
            dataGridView1.AutoResizeColumns();
            lblKayitSayisi.Text = $"📊 {dt.Rows.Count} ürün";
        }

        // ════════════════════════════════════════════════════════════
        //  YARDIMCI: input dialog
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

        private void DurumYaz(string m, Color c)
        { try { lblStatus.Text = m; lblStatus.ForeColor = c; } catch { } }

        // ── Tag class ────────────────────────────────────────────────
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