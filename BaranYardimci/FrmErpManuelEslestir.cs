using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class FrmErpManuelEslestir : Form
    {
        public Dictionary<string, EslestirSonuc> Sonuclar { get; private set; }
            = new Dictionary<string, EslestirSonuc>(StringComparer.OrdinalIgnoreCase);

        private readonly List<BulunamadiItem> _liste;
        private readonly string _imalatKlasor;
        private int _secilenSatir = -1;
        private List<HammaddeRow> _hammaddeler = new List<HammaddeRow>();

        class HammaddeRow { public string No, Adi; }

        public FrmErpManuelEslestir(List<BulunamadiItem> liste, string imalatKlasor)
        {
            InitializeComponent();
            _liste = liste;
            _imalatKlasor = imalatKlasor;
        }

        private void FrmErpManuelEslestir_Load(object sender, EventArgs e)
        {
            HammaddeleriYukle();
            ListeyiDoldur();
            if (lvSonuclar.Items.Count > 0) { lvSonuclar.Items[0].Selected = true; lvSonuclar.Select(); }
        }

        private void HammaddeleriYukle()
        {
            _hammaddeler.Clear();
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(DB.ConnStr))
                {
                    conn.Open();
                    using (var cmd = new System.Data.SqlClient.SqlCommand("SELECT no, adi FROM hammadde ORDER BY adi", conn))
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                            _hammaddeler.Add(new HammaddeRow { No = dr["no"].ToString(), Adi = dr["adi"].ToString() });
                }
            }
            catch (Exception ex) { MessageBox.Show("Hammadde yüklenemedi: " + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void ListeyiDoldur()
        {
            lvSonuclar.Items.Clear();
            foreach (var item in _liste)
            {
                string key = item.Profil + "|" + item.Kalite;
                bool eslendi = Sonuclar.ContainsKey(key);
                var lvi = new ListViewItem(item.Profil);
                lvi.SubItems.Add(item.Kalite);
                lvi.SubItems.Add(item.ToplamAdet.ToString("0.##"));
                lvi.SubItems.Add(item.ToplamUzunluk.ToString("0"));
                lvi.SubItems.Add(item.ToplamAgirlik.ToString("0.##"));
                lvi.SubItems.Add(eslendi ? Sonuclar[key].No : "");
                lvi.SubItems.Add(eslendi ? Sonuclar[key].Ad : "EŞLEŞTİRİLMEDİ");
                lvi.Tag = item;
                lvi.BackColor = eslendi ? Color.FromArgb(198, 239, 206) : Color.FromArgb(255, 199, 206);
                lvi.ForeColor = eslendi ? Color.FromArgb(0, 80, 0) : Color.FromArgb(130, 0, 0);
                lvSonuclar.Items.Add(lvi);
            }
            GuncelleBaslik();
        }

        private void lvSonuclar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSonuclar.SelectedItems.Count == 0) return;
            _secilenSatir = lvSonuclar.SelectedIndices[0];
            var item = (BulunamadiItem)lvSonuclar.SelectedItems[0].Tag;

            lblPozNo.Text = item.Profil + "  |  " + item.Kalite;
            lblBilgi.Text = $"{item.ToplamAdet:0.##} adet   —   {item.ToplamUzunluk:0} mm   —   {item.ToplamAgirlik:0.##} kg";

            rtbDetay.Clear();
            rtbDetay.SelectionFont = new Font("Consolas", 10f, FontStyle.Bold);
            rtbDetay.SelectionColor = Color.FromArgb(0, 72, 148);
            rtbDetay.AppendText("POZ DETAYLARI:\r\n");
            rtbDetay.SelectionFont = new Font("Consolas", 9.5f);
            rtbDetay.SelectionColor = Color.Black;
            foreach (var s in item.Satirlar) rtbDetay.AppendText(s + "\r\n");

            string vUp = item.Profil.ToUpper().Replace(" ", "");
            var oto = _hammaddeler.FirstOrDefault(h => h.Adi.ToUpper().Replace(" ", "").Contains(vUp));
            if (oto != null)
            { lblSecilenKod.Text = "✔  " + oto.No + "   —   " + oto.Adi; lblSecilenKod.ForeColor = Color.FromArgb(0, 110, 20); pnlSecilenBar.BackColor = Color.FromArgb(210, 245, 220); }
            else
            { lblSecilenKod.Text = "(seçilmedi)"; lblSecilenKod.ForeColor = Color.FromArgb(150, 0, 0); pnlSecilenBar.BackColor = Color.FromArgb(255, 235, 235); }

            txtArama.Text = item.Profil;
            AramaYap(item.Profil);
        }

        private void txtArama_TextChanged(object sender, EventArgs e) => AramaYap(txtArama.Text);

        private void AramaYap(string q)
        {
            lvHammadde.Items.Clear();
            if (string.IsNullOrWhiteSpace(q)) return;
            string up = q.ToUpper().Replace(" ", "");
            foreach (var h in _hammaddeler)
            {
                if (h.Adi.ToUpper().Replace(" ", "").Contains(up) || h.No.ToUpper().Contains(up))
                {
                    var lvi = new ListViewItem(h.No) { Tag = h };
                    lvi.SubItems.Add(h.Adi);
                    lvHammadde.Items.Add(lvi);
                }
            }
        }

        private void lvHammadde_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvHammadde.SelectedItems.Count == 0) return;
            var h = (HammaddeRow)lvHammadde.SelectedItems[0].Tag;
            lblSecilenKod.Text = "✔  " + h.No + "   —   " + h.Adi;
            lblSecilenKod.ForeColor = Color.FromArgb(0, 110, 20);
            pnlSecilenBar.BackColor = Color.FromArgb(210, 245, 220);
        }

        private void lvHammadde_DoubleClick(object sender, EventArgs e) => EsleVeIleri();

        private void btnEsle_Click(object sender, EventArgs e) => EsleVeIleri();

        private void EsleVeIleri()
        {
            if (_secilenSatir < 0 || _secilenSatir >= _liste.Count) return;
            if (lvHammadde.SelectedItems.Count == 0)
            { MessageBox.Show("Listeden bir hammadde seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var item = _liste[_secilenSatir];
            string key = item.Profil + "|" + item.Kalite;
            var h = (HammaddeRow)lvHammadde.SelectedItems[0].Tag;
            Sonuclar[key] = new EslestirSonuc { No = h.No, Ad = h.Adi };

            var lvi = lvSonuclar.Items[_secilenSatir];
            lvi.SubItems[5].Text = h.No;
            lvi.SubItems[6].Text = h.Adi;
            lvi.BackColor = Color.FromArgb(198, 239, 206);
            lvi.ForeColor = Color.FromArgb(0, 80, 0);

            int sonraki = _secilenSatir + 1;
            if (sonraki < lvSonuclar.Items.Count) { lvSonuclar.Items[sonraki].Selected = true; lvSonuclar.EnsureVisible(sonraki); }
            GuncelleBaslik();
        }

        private void btnAtla_Click(object sender, EventArgs e)
        {
            int sonraki = _secilenSatir + 1;
            if (sonraki < lvSonuclar.Items.Count) { lvSonuclar.Items[sonraki].Selected = true; lvSonuclar.EnsureVisible(sonraki); }
        }

        private void GuncelleBaslik()
        {
            int eslenen = Sonuclar.Count, toplam = _liste.Count;
            lblBaslik.Text = $"ERP Manuel Eşleştirme  —  {eslenen} / {toplam} eşleştirildi";
            lblBaslik.ForeColor = eslenen == toplam ? Color.FromArgb(120, 230, 140) : Color.FromArgb(255, 210, 80);
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            int eslenmemis = _liste.Count(item => !Sonuclar.ContainsKey(item.Profil + "|" + item.Kalite));
            if (eslenmemis > 0)
            {
                if (MessageBox.Show($"{eslenmemis} profil eşleştirilmedi. Yine de devam edilsin mi?\n(Eşleştirilmeyenler boş kod ile aktarılır)",
                    "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnIptal_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
    }
}