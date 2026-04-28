using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace BaranYardimci
{
    public partial class Donusturucu : Form
    {
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(ref NETRESOURCE nr, string password, string username, int flags);
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);

        [StructLayout(LayoutKind.Sequential)]
        private struct NETRESOURCE
        {
            public int dwScope, dwType, dwDisplayType, dwUsage;
            public string lpLocalName, lpRemoteName, lpComment, lpProvider;
        }

        const string NET_PATH = @"\\192.168.2.10\erp\Hazir_Exceller";
        const string NET_USER = "admin";
        const string NET_PASS = "asd123asd";
        const string CACHE_FILE = "erp_aktarim_gecmisi.txt";

        class HamSatir
        {
            public string DosyaId, DosyaAdi, MontajNo, ParcaNo, ParcaProfil, Kalite;
            public double MontajAdeti, BirimAdet, Uzunluk, Agirlik;
        }
        class MalzemeOzet
        {
            public string Profil, Kalite;
            public double ToplamAdet, ToplamUzunluk, ToplamAgirlik;
        }
        class KesimParcasi
        {
            public string ParcaNo;
            public double Uzunluk;
            public int Adet;
        }
        class StokBar
        {
            public int BarNo;
            public double StokBoyu;
            public List<KesimParcasi> Dilimler = new List<KesimParcasi>();
            const double KERF = 3.0;
            public double Kullanilan
            {
                get
                {
                    double t = 0; int n = 0;
                    foreach (var d in Dilimler) { t += d.Uzunluk * d.Adet; n += d.Adet; }
                    if (n > 1) t += (n - 1) * KERF;
                    return t;
                }
            }
            public double Fire { get { return Math.Max(0, StokBoyu - Kullanilan); } }
            public double FirePct { get { return StokBoyu > 0 ? Fire / StokBoyu * 100.0 : 0; } }
        }
        class ProfilSonuc
        {
            public string Profil, Kalite;
            public double KgPerMetre, ToplamKg, ToplamNetMetre;
            public int Bar6Adet;
            public double Bar6ToplamMetre, Bar6ToplamKg, Bar6FirePct;
            public int Bar12Adet;
            public double Bar12ToplamMetre, Bar12ToplamKg, Bar12FirePct;
            public int OnerilenAdet;
            public double OnerilenBoy, OnerilenMetre, OnerilenKg, OnerilenFirePct;
            public string OneriNot;
            public List<StokBar> OnerilenBarlar = new List<StokBar>();
        }
        class HammaddeItem { public string No, Adi; }

        const double KERF_MM = 3.0;
        private static readonly double[] STOK_ADAYLARI =
            { 6000, 7000, 7500, 8000, 9000, 10000, 11000, 12000 };

        private static int RenkBaslik = Ole(31, 73, 125);
        private static int RenkMavi = Ole(68, 114, 196);
        private static int RenkAcik = Ole(222, 235, 247);
        private static int RenkYesil = Ole(198, 239, 206);
        private static int RenkSari = Ole(255, 235, 156);
        private static int RenkKirmizi = Ole(255, 199, 206);
        private static int RenkBeyaz = Ole(255, 255, 255);
        private static int RenkGri = Ole(242, 242, 242);
        private static int Ole(int r, int g, int b)
            => System.Drawing.ColorTranslator.ToOle(Color.FromArgb(r, g, b));

        private List<HamSatir> _tumVeriler = new List<HamSatir>();
        private string _imalatKartlariKlasoru = "";
        private HashSet<string> _erpAktarimYapilan =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> _erpExcelYollari =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private int _sagKlikSatir = -1;

        // ════════════════════════════════════════════════════════════════
        //  CONSTRUCTOR & LOAD
        // ════════════════════════════════════════════════════════════════

        public Donusturucu() { InitializeComponent(); }

        private void Donusturucu_Load(object sender, EventArgs e)
        {
            GecmisiYukle();
            DurumGuncelle();
        }

        // ══════════════════════════��═════════════════════════════════════
        //  TIMER
        // ════════════════════════════════════════════════════════════════

        private void timerSaat_Tick(object sender, EventArgs e)
        {
            try { lblSaat.Text = DateTime.Now.ToString("dd.MM.yyyy  HH:mm:ss"); } catch { }
        }

        // ════════════════════════════════════════════════════════════════
        //  GEÇMİŞ
        // ════════════════════════════════════════════════════════════════

        private void btnGecmisSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Geçmiş silinsin mi?", "Onay",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try { File.Delete(CACHE_FILE); } catch { }
                GecmisiYukle();
            }
        }

        private void GecmisiYukle()
        {
            try { lbGecmis.Items.Clear(); } catch { return; }
            if (!File.Exists(CACHE_FILE)) return;
            try
            {
                foreach (var s in File.ReadAllLines(CACHE_FILE, Encoding.UTF8)
                                      .Reverse().Take(50)
                                      .Where(x => !string.IsNullOrWhiteSpace(x)))
                    lbGecmis.Items.Add(s);
            }
            catch { }
        }

        private void GecmiseEkle(string dosyaAdi)
        {
            try
            {
                File.AppendAllText(CACHE_FILE,
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm") + "  |  " + dosyaAdi
                    + Environment.NewLine, Encoding.UTF8);
                GecmisiYukle();
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════════════
        //  DURUM BARI
        // ════════════════════════════════════════════════════════════════

        private void DurumGuncelle()
        {
            try
            {
                int toplam = dgvDosyalar.Rows.Count;
                int aktarildi = dgvDosyalar.Rows.Cast<DataGridViewRow>()
                    .Count(r => _erpAktarimYapilan.Contains(
                        r.Cells["colDosyaYolu"].Value?.ToString() ?? ""));

                if (toplam == 0)
                {
                    lblDurumIkon.Text = "📋";
                    lblDurumMetin.Text = "Dosya yüklenmedi";
                    lblDurumMetin.ForeColor = Color.FromArgb(100, 100, 120);
                    pnlDurumBar.BackColor = Color.FromArgb(230, 230, 235);
                }
                else if (aktarildi == 0)
                {
                    lblDurumIkon.Text = "📂";
                    lblDurumMetin.Text = $"{toplam} dosya yüklendi  —  Excel aktarımı bekleniyor";
                    lblDurumMetin.ForeColor = Color.FromArgb(140, 80, 0);
                    pnlDurumBar.BackColor = Color.FromArgb(255, 243, 205);
                }
                else if (aktarildi < toplam)
                {
                    lblDurumIkon.Text = "⚠";
                    lblDurumMetin.Text = $"{aktarildi}/{toplam} dosya aktarıldı  —  Kalan dosyalar bekliyor";
                    lblDurumMetin.ForeColor = Color.FromArgb(140, 80, 0);
                    pnlDurumBar.BackColor = Color.FromArgb(255, 243, 205);
                }
                else
                {
                    lblDurumIkon.Text = "✅";
                    lblDurumMetin.Text = "Tüm dosyalar aktarıldı  —  Sağ klik → Rota Gir";
                    lblDurumMetin.ForeColor = Color.FromArgb(0, 100, 0);
                    pnlDurumBar.BackColor = Color.FromArgb(210, 245, 215);
                }
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════════════
        //  DGV RENK & SAĞ KLİK
        // ════════════════════════════════════════════════════════════════

        private void dgvDosyalar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvDosyalar.Rows[e.RowIndex];
            string yol = row.Cells["colDosyaYolu"].Value?.ToString() ?? "";
            if (_erpAktarimYapilan.Contains(yol))
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(198, 239, 206);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 80, 0);
                row.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            }
        }

        private void dgvDosyalar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var hit = dgvDosyalar.HitTest(e.X, e.Y);
            if (hit.RowIndex < 0) return;

            dgvDosyalar.ClearSelection();
            dgvDosyalar.Rows[hit.RowIndex].Selected = true;
            _sagKlikSatir = hit.RowIndex;

            string yol = dgvDosyalar.Rows[hit.RowIndex].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            bool aktarildi = _erpAktarimYapilan.Contains(yol);

            mnuRotaGir.Enabled = aktarildi;
            mnuRotaGir.Text = aktarildi
                ? "✏  Rota Gir  —  İmalat Kartlarını Bas"
                : "✏  Rota Gir  (önce ERP aktarımı yapın)";
            mnuRotaGir.ForeColor = aktarildi ? Color.FromArgb(0, 100, 0) : Color.Gray;

            ctxDosya.Show(dgvDosyalar, e.Location);
        }

        private void mnuRotaGir_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0 || _sagKlikSatir >= dgvDosyalar.Rows.Count) return;
            string yol = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            RotaGir(yol);
        }

        private void mnuDosyaSil_Click(object sender, EventArgs e)
        {
            if (_sagKlikSatir < 0 || _sagKlikSatir >= dgvDosyalar.Rows.Count) return;
            string yol = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaYolu"].Value?.ToString() ?? "";
            string ad = dgvDosyalar.Rows[_sagKlikSatir].Cells["colDosyaAdi"].Value?.ToString() ?? "";

            if (MessageBox.Show($"'{ad}' listeden çıkarılsın mı?", "Onay",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            _tumVeriler.RemoveAll(v => v.DosyaId == yol);
            _erpAktarimYapilan.Remove(yol);
            _erpExcelYollari.Remove(yol);
            dgvDosyalar.Rows.RemoveAt(_sagKlikSatir);
            _sagKlikSatir = -1;
            DurumGuncelle();
        }

        private void RotaGir(string dosyaYolu)
        {
            string erpExcel = _erpExcelYollari.ContainsKey(dosyaYolu) ? _erpExcelYollari[dosyaYolu] : "";
            if (string.IsNullOrEmpty(erpExcel) || !File.Exists(erpExcel))
            {
                MessageBox.Show("ERP Excel dosyası bulunamadı:\n" + erpExcel,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (var frm = new FrmImalatKartiRota(erpExcel))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  DOSYA İŞLEMLERİ
        // ════════════════════════════════════════════════════════════════

        private void btnDosyaEkle_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            { Filter = "Rapor|*.DOC;*.TXT;*.RPT|Hepsi|*.*", Multiselect = true };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            int ok = 0, top = 0;
            foreach (string yol in ofd.FileNames)
            {
                if (DosyaVarMi(yol)) continue;
                string ad = Path.GetFileName(yol);
                int n = DosyaOku(yol, ad);
                if (n > 0) { dgvDosyalar.Rows.Add(ad, "1", "Yüklendi", yol); ok++; top += n; }
                else MessageBox.Show("Okunamadi: " + ad);
            }
            if (ok > 0) { MessageBox.Show(ok + " dosya, " + top + " parça."); DurumGuncelle(); }
        }

        private bool DosyaVarMi(string yol)
        {
            foreach (DataGridViewRow r in dgvDosyalar.Rows)
                if (r.Cells["colDosyaYolu"].Value?.ToString() == yol) return true;
            return false;
        }

        private int DosyaOku(string yol, string ad)
        {
            var list = new List<HamSatir>();
            try
            {
                string[] satirlar = File.ReadAllLines(yol, Encoding.Default);
                if (string.Join(" ", satirlar).Contains("+ACo-"))
                    satirlar = File.ReadAllLines(yol, Encoding.UTF7);
                string asm = ""; double madet = 1;
                foreach (string satir in satirlar)
                {
                    string t = satir.Trim();
                    if (string.IsNullOrEmpty(t) || t.StartsWith("-") ||
                        t.ToUpper().Contains("ASSEMBLY") || t.Contains("Total:")) continue;
                    string[] p = Regex.Split(t, @"\s{2,}");
                    if (p.Length < 2) continue;
                    int bo = 0;
                    for (int k = 0; k < satir.Length; k++) { if (satir[k] == ' ') bo++; else break; }
                    if (bo < 5 && IsNum(p[1]))
                    { asm = p[0]; madet = N(p[1]); if (madet <= 0) madet = 1; }
                    else if (bo >= 5)
                    {
                        var tp = p.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                        if (tp.Count >= 5 && IsNum(tp[1]))
                            list.Add(new HamSatir
                            {
                                DosyaId = yol,
                                DosyaAdi = ad,
                                MontajNo = asm,
                                MontajAdeti = madet,
                                ParcaNo = tp[0],
                                BirimAdet = N(tp[1]),
                                ParcaProfil = tp[2],
                                Kalite = tp.Count >= 6 ? tp[3] : "",
                                Uzunluk = N(tp.Count >= 6 ? tp[4] : tp[3]),
                                Agirlik = N(tp.Count >= 6 ? tp[5] : tp[4])
                            });
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); return 0; }
            _tumVeriler.AddRange(list);
            return list.Count;
        }

        // ════════════════════════════════════════════��═══════════════════
        //  NORMALİZASYON
        // ════════════════════════════════════════════════════════════════

        private string PG(string p)
        {
            if ((p.StartsWith("PL", StringComparison.OrdinalIgnoreCase) ||
                 p.StartsWith("FPL", StringComparison.OrdinalIgnoreCase)) && p.Contains("*"))
                return p.Split('*')[0];
            return p;
        }

        private string KD(string k)
        {
            if (string.IsNullOrWhiteSpace(k)) return k;
            switch (k.Trim().ToUpper())
            {
                case "ST37": return "S235JR";
                case "ST52": return "S355JR";
                case "ST44": return "S275JR";
                default: return k;
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  BUTONLAR
        // ═══════════════════════════════════════════════════���════════════

        private void btnMiktarKaydet_Click(object sender, EventArgs e)
        { dgvDosyalar.EndEdit(); MessageBox.Show("Kaydedildi."); }

        private void btnListeTemizle_Click(object sender, EventArgs e)
        {
            dgvDosyalar.Rows.Clear(); dgvSonuc.Rows.Clear();
            _tumVeriler.Clear(); _erpAktarimYapilan.Clear(); _erpExcelYollari.Clear();
            DurumGuncelle();
        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }
            dgvDosyalar.EndEdit();
            Cursor.Current = Cursors.WaitCursor;
            dgvSonuc.Rows.Clear();
            foreach (var item in Ozet(SM()).Values.OrderBy(x => x.Profil).ThenBy(x => x.Kalite))
                dgvSonuc.Rows.Add(item.Profil, item.Kalite,
                    item.ToplamAdet, item.ToplamUzunluk, Math.Round(item.ToplamAgirlik, 2));
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Tamam.");
        }

        // ════════════════════════════════════════════════════════════════
        //  VERİTABANI
        // ════════════════════════════════════════════════════════════════

        private void btnVeritabaniKaydet_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }
            dgvDosyalar.EndEdit();
            int musteriID = -1; string musteriAdi = "", durum = "Uretim Bekliyor";
            using (var frm = new MusteriSecForm())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog() != DialogResult.OK) return;
                musteriID = frm.SecilenMusteriID;
                musteriAdi = (frm.SecilenMusteriAd + " " + frm.SecilenMusteriSoyad).Trim();
                durum = frm.SecilenMalzemeDurum;
            }
            var tf = new FrmTerminSec();
            if (tf.ShowDialog() != DialogResult.OK) return;
            DateTime termin = tf.SecilenTarih;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var sm = SM(); int n = 0;
                foreach (var v in _tumVeriler)
                {
                    double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                    double urt = sip * v.MontajAdeti * v.BirimAdet;
                    DB.Execute(
                        @"INSERT INTO MalzemeKayit
                            (MusteriID,MusteriAdi,DosyaAdi,SiparisAdeti,AssemblyNo,AssemblyAdeti,
                             PozNo,Profil,Kalite,BirimAdet,UretilecekAdet,Uzunluk,ParcaAgirlik,
                             ToplamAgirlik,MalzemeDurum,KayitTarihi,TerminTarihi)
                          VALUES(@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m,@n,@o,GETDATE(),@p)",
                        new SqlParameter("@a", musteriID), new SqlParameter("@b", musteriAdi),
                        new SqlParameter("@c", v.DosyaAdi), new SqlParameter("@d", sip),
                        new SqlParameter("@e", v.MontajNo ?? ""), new SqlParameter("@f", v.MontajAdeti),
                        new SqlParameter("@g", v.ParcaNo ?? ""), new SqlParameter("@h", PG(v.ParcaProfil)),
                        new SqlParameter("@i", KD(v.Kalite)), new SqlParameter("@j", v.BirimAdet),
                        new SqlParameter("@k", urt), new SqlParameter("@l", v.Uzunluk),
                        new SqlParameter("@m", v.Agirlik),
                        new SqlParameter("@n", Math.Round(urt * v.Agirlik, 2)),
                        new SqlParameter("@o", durum), new SqlParameter("@p", termin));
                    n++;
                }
                MessageBox.Show(n + " kayit eklendi.");
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { Cursor.Current = Cursors.Default; }
        }

        // ════════════════════════════════════════════════════════════════
        //  ERP AKTARİM
        // ════════════════════════════════════════════════════════════════

        private void btnErpAktarim_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }
            dgvDosyalar.EndEdit();
            var ozet = Ozet(SM());
            if (ozet.Count == 0) return;

            var sm = SM();
            var profilSatirlar = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var v in _tumVeriler)
            {
                string k2 = PG(v.ParcaProfil) + "|" + KD(v.Kalite);
                double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                double urt = sip * v.MontajAdeti * v.BirimAdet;
                if (!profilSatirlar.ContainsKey(k2)) profilSatirlar[k2] = new List<string>();
                profilSatirlar[k2].Add(
                    $"► {v.ParcaNo,-14}  Montaj: {v.MontajNo,-12}  " +
                    $"Adet: {urt,6:0.##}  Boy: {v.Uzunluk,7:0} mm  " +
                    $"Agirlik: {Math.Round(urt * v.Agirlik, 2),8:0.##} kg  [{v.DosyaAdi}]");
            }

            var hm = new List<HammaddeItem>();
            try
            {
                using (var conn = new SqlConnection(DB.ConnStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT no,adi FROM hammadde", conn))
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                            hm.Add(new HammaddeItem
                            { No = dr["no"].ToString(), Adi = dr["adi"].ToString() });
                }
            }
            catch (Exception ex) { MessageBox.Show("DB: " + ex.Message); return; }

            var esles = new Dictionary<string, HammaddeItem>(StringComparer.OrdinalIgnoreCase);
            var bulunamadi = new List<BulunamadiItem>();

            foreach (var oz in ozet.Values)
            {
                string key = oz.Profil + "|" + oz.Kalite;
                bool b = false;
                string vUp = oz.Profil.ToUpper().Replace(" ", "");
                foreach (var h in hm)
                    if (h.Adi.ToUpper().Replace(" ", "").Contains(vUp))
                    { esles[key] = h; b = true; break; }

                if (!b)
                    bulunamadi.Add(new BulunamadiItem
                    {
                        Profil = oz.Profil,
                        Kalite = oz.Kalite,
                        ToplamAdet = oz.ToplamAdet,
                        ToplamUzunluk = oz.ToplamUzunluk,
                        ToplamAgirlik = Math.Round(oz.ToplamAgirlik, 2),
                        Satirlar = profilSatirlar.ContainsKey(key)
                                            ? profilSatirlar[key] : new List<string>()
                    });
            }

            if (bulunamadi.Count > 0)
            {
                using (var frm = new FrmErpManuelEslestir(bulunamadi, _imalatKartlariKlasoru))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    if (frm.ShowDialog(this) != DialogResult.OK) return;
                    foreach (var kv in frm.Sonuclar)
                        esles[kv.Key] = new HammaddeItem { No = kv.Value.No, Adi = kv.Value.Ad };
                }
            }

            string dosyaAdi = "ERP_Aktarim_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            string kaydedilenYol = AcExcelVeKaydet(dosyaAdi, xa =>
            {
                int r = 1;
                Yaz(xa, r, 1, "KOD"); Yaz(xa, r, 2, "ADI"); Yaz(xa, r, 3, "POZ NO");
                Yaz(xa, r, 4, "PROFIL"); Yaz(xa, r, 5, "ROTA"); Yaz(xa, r, 6, "KALITE");
                Yaz(xa, r, 7, "ADET"); Yaz(xa, r, 8, "UZUNLUK(mm)"); Yaz(xa, r, 9, "AGIRLIK(kg)");
                r = 2;
                foreach (var v in _tumVeriler.OrderBy(x => PG(x.ParcaProfil)).ThenBy(x => x.ParcaNo))
                {
                    string key = PG(v.ParcaProfil) + "|" + KD(v.Kalite);
                    bool b = esles.ContainsKey(key);
                    double sip2 = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                    double urt = sip2 * v.MontajAdeti * v.BirimAdet;
                    Yaz(xa, r, 1, b ? esles[key].No : ""); Yaz(xa, r, 2, b ? esles[key].Adi : "");
                    Yaz(xa, r, 3, (v.ParcaNo ?? "").Replace("/", "")); Yaz(xa, r, 4, PG(v.ParcaProfil));
                    Yaz(xa, r, 5, ""); Yaz(xa, r, 6, KD(v.Kalite));
                    Yaz(xa, r, 7, Math.Round(urt, 2)); Yaz(xa, r, 8, v.Uzunluk);
                    Yaz(xa, r, 9, Math.Round(urt * v.Agirlik, 2)); r++;
                }
            });

            if (!string.IsNullOrEmpty(kaydedilenYol))
            {
                foreach (DataGridViewRow row in dgvDosyalar.Rows)
                {
                    string y = row.Cells["colDosyaYolu"].Value?.ToString() ?? "";
                    if (string.IsNullOrEmpty(y)) continue;
                    _erpAktarimYapilan.Add(y);
                    _erpExcelYollari[y] = kaydedilenYol;
                    try { row.Cells["colDurum"].Value = "✅ ERP Exceli Aktarıldı"; } catch { }
                }
                dgvDosyalar.Refresh();
                DurumGuncelle();
            }
        }

        private void btnGalvanizEkran_Click(object sender, EventArgs e)
            => new GalvanizKontrol().Show();

        private void btnHamDataExcel_Click(object sender, EventArgs e)
        {
            if (_tumVeriler.Count == 0) return;
            dgvDosyalar.EndEdit();
            var sm = SM();
            AcExcel(xa =>
            {
                int r = 1;
                Yaz(xa, r, 1, "DOSYA"); Yaz(xa, r, 2, "SIPARIS"); Yaz(xa, r, 3, "MONTAJ");
                Yaz(xa, r, 4, "MADET"); Yaz(xa, r, 5, "PARCA"); Yaz(xa, r, 6, "PROFIL");
                Yaz(xa, r, 7, "KALITE"); Yaz(xa, r, 8, "BADET"); Yaz(xa, r, 9, "UADET");
                Yaz(xa, r, 10, "UZUNLUK"); Yaz(xa, r, 11, "PAG"); Yaz(xa, r, 12, "TAG");
                r = 2;
                foreach (var v in _tumVeriler)
                {
                    double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                    double urt = sip * v.MontajAdeti * v.BirimAdet;
                    Yaz(xa, r, 1, v.DosyaAdi); Yaz(xa, r, 2, sip); Yaz(xa, r, 3, v.MontajNo);
                    Yaz(xa, r, 4, v.MontajAdeti); Yaz(xa, r, 5, v.ParcaNo); Yaz(xa, r, 6, PG(v.ParcaProfil));
                    Yaz(xa, r, 7, KD(v.Kalite)); Yaz(xa, r, 8, v.BirimAdet); Yaz(xa, r, 9, urt);
                    Yaz(xa, r, 10, v.Uzunluk); Yaz(xa, r, 11, v.Agirlik);
                    Yaz(xa, r, 12, Math.Round(urt * v.Agirlik, 2)); r++;
                }
            });
        }

        // ════════════════════════════════════════════════════════════════
        //  MALZEME EXCEL
        // ════════════════════════════════════════════════════════════════

        private void btnMalzemeExcel_Click(object sender, EventArgs e)
        {
            if (dgvSonuc.Rows.Count == 0) { MessageBox.Show("Önce HESAPLA butonuna bas."); return; }
            dgvDosyalar.EndEdit();
            Cursor.Current = Cursors.WaitCursor;

            var sm = SM();
            var dict = new Dictionary<string, List<KesimParcasi>>(StringComparer.OrdinalIgnoreCase);
            var kgPerM = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            foreach (var v in _tumVeriler)
            {
                if (v.Uzunluk <= 0) continue;
                double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                int adet = (int)Math.Round(sip * v.MontajAdeti * v.BirimAdet);
                if (adet <= 0) continue;
                string key = PG(v.ParcaProfil) + "||" + KD(v.Kalite);
                if (!dict.ContainsKey(key)) dict[key] = new List<KesimParcasi>();
                var m = dict[key].FirstOrDefault(
                    x => x.ParcaNo == v.ParcaNo && Math.Abs(x.Uzunluk - v.Uzunluk) < 0.1);
                if (m != null) m.Adet += adet;
                else dict[key].Add(new KesimParcasi { ParcaNo = v.ParcaNo, Uzunluk = v.Uzunluk, Adet = adet });
                if (v.Uzunluk > 0 && v.Agirlik > 0 && !kgPerM.ContainsKey(key))
                    kgPerM[key] = v.Agirlik / (v.Uzunluk / 1000.0);
            }

            var sonuclar = new List<ProfilSonuc>();
            foreach (var kv in dict.OrderBy(x => x.Key))
            {
                string[] pts = kv.Key.Split(new[] { "||" }, StringSplitOptions.None);
                string profil = pts[0], kalite = pts.Length > 1 ? pts[1] : "";
                double maxUz = kv.Value.Max(x => x.Uzunluk);
                double kgM = kgPerM.ContainsKey(kv.Key) ? kgPerM[kv.Key] : 0;
                double netMm = kv.Value.Sum(x => x.Uzunluk * x.Adet);
                var bars6 = maxUz <= 6000 ? FFD(kv.Value, 6000) : new List<StokBar>();
                var bars12 = FFD(kv.Value, 12000);
                double ts6 = bars6.Count * 6000.0, ts12 = bars12.Count * 12000.0;
                double fp6 = ts6 > 0 ? bars6.Sum(b => b.Fire) / ts6 * 100 : 999;
                double fp12 = ts12 > 0 ? bars12.Sum(b => b.Fire) / ts12 * 100 : 999;
                List<StokBar> enIyi = bars12; double enIyiBoy = 12000, enIyiFire = fp12;
                foreach (double aday in STOK_ADAYLARI)
                {
                    if (aday < maxUz) continue;
                    var b = FFD(kv.Value, aday); if (b.Count == 0) continue;
                    double ts = b.Count * aday;
                    double fp = ts > 0 ? b.Sum(x => x.Fire) / ts * 100 : 999;
                    if (fp < enIyiFire - 0.5 || (Math.Abs(fp - enIyiFire) < 0.5 && b.Count < enIyi.Count))
                    { enIyiFire = fp; enIyiBoy = aday; enIyi = b; }
                }
                double oneriMetre = enIyi.Count * enIyiBoy / 1000.0;
                string not = Math.Abs(enIyiBoy - 6000) < 1 ? "6m yeterli" :
                             Math.Abs(enIyiBoy - 12000) < 1 ? "12m tercih edildi" :
                             (enIyiBoy / 1000.0).ToString("0.0") + "m alirsan daha az fire";
                sonuclar.Add(new ProfilSonuc
                {
                    Profil = profil,
                    Kalite = kalite,
                    KgPerMetre = Math.Round(kgM, 3),
                    ToplamNetMetre = Math.Round(netMm / 1000.0, 2),
                    ToplamKg = Math.Round(netMm / 1000.0 * kgM, 1),
                    Bar6Adet = bars6.Count,
                    Bar6ToplamMetre = Math.Round(ts6 / 1000.0, 1),
                    Bar6ToplamKg = Math.Round(ts6 / 1000.0 * kgM, 1),
                    Bar6FirePct = Math.Round(fp6, 1),
                    Bar12Adet = bars12.Count,
                    Bar12ToplamMetre = Math.Round(ts12 / 1000.0, 1),
                    Bar12ToplamKg = Math.Round(ts12 / 1000.0 * kgM, 1),
                    Bar12FirePct = Math.Round(fp12, 1),
                    OnerilenAdet = enIyi.Count,
                    OnerilenBoy = enIyiBoy,
                    OnerilenMetre = Math.Round(oneriMetre, 1),
                    OnerilenKg = Math.Round(oneriMetre * kgM, 1),
                    OnerilenFirePct = Math.Round(enIyiFire, 1),
                    OneriNot = not,
                    OnerilenBarlar = enIyi
                });
            }

            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Add(System.Type.Missing);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                int row = 1;
                S(ws, row, 1, "SATIN ALIM LİSTESİ  —  " + DateTime.Now.ToString("dd.MM.yyyy"),
                    bold: true, bg: RenkBaslik, fg: RenkBeyaz, size: 13); H(ws, row, 18); row++;
                string[] h1 = { "PROFİL","KALİTE","kg/m","GEREKEN NET METRE","GEREKEN NET KG",
                    "6m BAR — ADET","6m — TOPLAM METRE","6m — TOPLAM KG","6m FIRE %",
                    "12m BAR — ADET","12m — TOPLAM METRE","12m — TOPLAM KG","12m FIRE %","ÖNERİLEN","NEDEN" };
                for (int i = 0; i < h1.Length; i++)
                    S(ws, row, i + 1, h1[i], bold: true, bg: RenkMavi, fg: RenkBeyaz, center: true);
                H(ws, row, 20); row++;
                int satirNo = 0;
                foreach (var s in sonuclar)
                {
                    int bg6 = s.Bar6FirePct > 20 ? RenkKirmizi : s.Bar6FirePct > 10 ? RenkSari : RenkYesil;
                    int bg12 = s.Bar12FirePct > 20 ? RenkKirmizi : s.Bar12FirePct > 10 ? RenkSari : RenkYesil;
                    int bgRow = satirNo % 2 == 0 ? RenkAcik : RenkBeyaz;
                    string oneri = s.Bar6Adet == 0 ? s.OnerilenAdet + " adet  " + (s.OnerilenBoy / 1000.0).ToString("0.0") + "m"
                        : Math.Abs(s.OnerilenBoy - 6000) < 1 ? s.OnerilenAdet + " adet  6m"
                        : Math.Abs(s.OnerilenBoy - 12000) < 1 ? s.OnerilenAdet + " adet  12m"
                        : s.OnerilenAdet + " adet  " + (s.OnerilenBoy / 1000.0).ToString("0.0") + "m";
                    S(ws, row, 1, s.Profil, bg: bgRow); S(ws, row, 2, s.Kalite, bg: bgRow);
                    S(ws, row, 3, s.KgPerMetre, center: true, bg: bgRow);
                    S(ws, row, 4, s.ToplamNetMetre + " m", center: true, bg: bgRow);
                    S(ws, row, 5, s.ToplamKg + " kg", center: true, bg: bgRow, bold: true);
                    if (s.Bar6Adet > 0)
                    {
                        S(ws, row, 6, s.Bar6Adet, center: true, bg: bgRow, bold: true);
                        S(ws, row, 7, s.Bar6ToplamMetre + " m", center: true, bg: bgRow);
                        S(ws, row, 8, s.Bar6ToplamKg + " kg", center: true, bg: bgRow);
                        S(ws, row, 9, "%" + s.Bar6FirePct.ToString("0.0"), center: true, bg: bg6);
                    }
                    else
                    {
                        S(ws, row, 6, "SIĞMAZ", center: true, bg: RenkKirmizi);
                        S(ws, row, 7, "-", center: true, bg: RenkGri); S(ws, row, 8, "-", center: true, bg: RenkGri);
                        S(ws, row, 9, "-", center: true, bg: RenkGri);
                    }
                    S(ws, row, 10, s.Bar12Adet, center: true, bg: bgRow, bold: true);
                    S(ws, row, 11, s.Bar12ToplamMetre + " m", center: true, bg: bgRow);
                    S(ws, row, 12, s.Bar12ToplamKg + " kg", center: true, bg: bgRow);
                    S(ws, row, 13, "%" + s.Bar12FirePct.ToString("0.0"), center: true, bg: bg12);
                    int bgO = s.OnerilenFirePct > 20 ? RenkKirmizi : s.OnerilenFirePct > 10 ? RenkSari : RenkYesil;
                    S(ws, row, 14, oneri + "  (%" + s.OnerilenFirePct.ToString("0.0") + " fire)", bold: true, bg: bgO, center: true);
                    S(ws, row, 15, s.OneriNot, bg: bgRow); H(ws, row, 18); row++; satirNo++;
                }
                try
                {
                    int[] w = { 16, 10, 7, 16, 14, 14, 16, 14, 10, 14, 16, 14, 10, 28, 26 };
                    for (int i = 0; i < w.Length; i++) ((Excel.Range)ws.Columns[i + 1]).ColumnWidth = w[i];
                }
                catch { }
                row += 2;
                S(ws, row, 1, "KESİM PLANI  —  Önerilen bar boyuna göre her bardan ne kesileceği",
                    bold: true, bg: RenkBaslik, fg: RenkBeyaz, size: 12); H(ws, row, 22); row++;
                string[] h2 ={"PROFİL","KALİTE","BAR BOYU","BAR NO","KULLANILAN (mm)","FIRE (mm)","FIRE %",
                    "PARÇALAR  (parca_no × uzunluk × adet)"};
                for (int i = 0; i < h2.Length; i++)
                    S(ws, row, i + 1, h2[i], bold: true, bg: RenkMavi, fg: RenkBeyaz, center: true);
                try { ((Excel.Range)ws.Columns[8]).ColumnWidth = 70; } catch { }
                H(ws, row, 20); row++; satirNo = 0;
                foreach (var s in sonuclar)
                {
                    string boyStr = (s.OnerilenBoy / 1000.0).ToString("0.0") + " m";
                    foreach (var bar in s.OnerilenBarlar)
                    {
                        string pstr = string.Join("  |  ", bar.Dilimler.Select(
                            d => d.ParcaNo + " × " + d.Uzunluk.ToString("0") + "mm × " + d.Adet + " adet"));
                        if (pstr.Length > 3000) pstr = pstr.Substring(0, 3000);
                        int bgRow = satirNo % 2 == 0 ? RenkAcik : RenkBeyaz;
                        int fBg = bar.FirePct > 20 ? RenkKirmizi : bar.FirePct > 10 ? RenkSari : RenkYesil;
                        S(ws, row, 1, s.Profil, bg: bgRow); S(ws, row, 2, s.Kalite, bg: bgRow);
                        S(ws, row, 3, boyStr, center: true, bg: bgRow, bold: true);
                        S(ws, row, 4, "Bar " + bar.BarNo, center: true, bg: bgRow, bold: true);
                        S(ws, row, 5, Math.Round(bar.Kullanilan, 0), center: true, bg: bgRow);
                        S(ws, row, 6, Math.Round(bar.Fire, 0), center: true, bg: bgRow);
                        S(ws, row, 7, "%" + bar.FirePct.ToString("0.0"), center: true, bg: fBg, bold: bar.FirePct > 20);
                        S(ws, row, 8, pstr, bg: bgRow); H(ws, row, 17); row++; satirNo++;
                    }
                    try { ((Excel.Range)ws.Rows[row]).RowHeight = 6; } catch { }
                    row++;
                }
                app.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel Hatasi: " + ex.Message);
                try { if (wb != null) wb.Close(false); } catch { }
                try { if (app != null) app.Quit(); } catch { }
            }
            finally
            {
                try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { }
                try { if (app != null) Marshal.ReleaseComObject(app); } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  FFD
        // ════════════════════════════════════════════════════════════════

        private List<StokBar> FFD(List<KesimParcasi> parcalar, double stokBoyu)
        {
            var sira = new List<KesimParcasi>();
            foreach (var p in parcalar.OrderByDescending(x => x.Uzunluk))
                for (int i = 0; i < p.Adet; i++)
                    sira.Add(new KesimParcasi { ParcaNo = p.ParcaNo, Uzunluk = p.Uzunluk, Adet = 1 });
            var barlar = new List<StokBar>();
            foreach (var parca in sira)
            {
                if (parca.Uzunluk > stokBoyu) continue;
                StokBar hedef = null;
                foreach (var bar in barlar)
                {
                    int topN = bar.Dilimler.Sum(d => d.Adet);
                    double ekle = parca.Uzunluk + (topN > 0 ? KERF_MM : 0);
                    if (bar.Kullanilan + ekle <= stokBoyu) { hedef = bar; break; }
                }
                if (hedef == null) { hedef = new StokBar { BarNo = barlar.Count + 1, StokBoyu = stokBoyu }; barlar.Add(hedef); }
                var d2 = hedef.Dilimler.FirstOrDefault(
                    x => x.ParcaNo == parca.ParcaNo && Math.Abs(x.Uzunluk - parca.Uzunluk) < 0.1);
                if (d2 != null) d2.Adet++;
                else hedef.Dilimler.Add(new KesimParcasi { ParcaNo = parca.ParcaNo, Uzunluk = parca.Uzunluk, Adet = 1 });
            }
            return barlar;
        }

        // ════════════════════════════════════════════════════════════════
        //  AĞ + EXCEL
        // ════════════════════════════════════════════════════════════════

        private bool AgaBaglan()
        {
            var nr = new NETRESOURCE { dwType = 1, lpRemoteName = NET_PATH };
            int ret = WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0);
            return ret == 0 || ret == 1219;
        }

        private void AgaBaglantiyiKes()
        { try { WNetCancelConnection2(NET_PATH, 0, false); } catch { } }

        private string AcExcelVeKaydet(string dosyaAdi, Action<Excel.Worksheet> yaz)
        {
            Cursor.Current = Cursors.WaitCursor;
            string kaydedilenYol = "";
            Excel.Application app = null; Excel.Workbook wb = null; Excel.Worksheet ws = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Add(System.Type.Missing); ws = (Excel.Worksheet)wb.ActiveSheet;
                yaz(ws);
                if (AgaBaglan())
                {
                    kaydedilenYol = Path.Combine(NET_PATH, dosyaAdi);
                    wb.SaveAs(kaydedilenYol, Excel.XlFileFormat.xlOpenXMLWorkbook,
                        System.Type.Missing, System.Type.Missing, false, false,
                        Excel.XlSaveAsAccessMode.xlNoChange,
                        System.Type.Missing, System.Type.Missing,
                        System.Type.Missing, System.Type.Missing, System.Type.Missing);
                    GecmiseEkle(dosyaAdi);
                    MessageBox.Show("Excel kaydedildi:\n" + kaydedilenYol, "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Ağ bağlantısı kurulamadı!\n" + NET_PATH, "Ağ Hatası",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                app.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel/Ağ Hatası: " + ex.Message); kaydedilenYol = "";
                try { if (wb != null) wb.Close(false); } catch { }
                try { if (app != null) app.Quit(); } catch { }
            }
            finally
            {
                AgaBaglantiyiKes();
                try { if (ws != null) Marshal.ReleaseComObject(ws); } catch { }
                try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { }
                try { if (app != null) Marshal.ReleaseComObject(app); } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
            return kaydedilenYol;
        }

        private void AcExcel(Action<Excel.Worksheet> yaz)
        {
            Cursor.Current = Cursors.WaitCursor;
            Excel.Application app = null; Excel.Workbook wb = null; Excel.Worksheet ws = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Add(System.Type.Missing); ws = (Excel.Worksheet)wb.ActiveSheet;
                yaz(ws); app.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel: " + ex.Message);
                try { if (wb != null) wb.Close(false); } catch { }
                try { if (app != null) app.Quit(); } catch { }
            }
            finally
            {
                try { if (ws != null) Marshal.ReleaseComObject(ws); } catch { }
                try { if (wb != null) Marshal.ReleaseComObject(wb); } catch { }
                try { if (app != null) Marshal.ReleaseComObject(app); } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  STİL + YARDIMCILAR
        // ════════════════════════════════════════════════════════════════

        private void S(Excel.Worksheet ws, int r, int c, object v,
            bool bold = false, int bg = 0, int fg = 0, bool center = false, int size = 0)
        {
            try
            {
                var cell = (Excel.Range)ws.Cells[r, c];
                if (v != null) cell.Value2 = v;
                if (bold) cell.Font.Bold = true;
                if (bg != 0) cell.Interior.Color = bg;
                if (fg != 0) cell.Font.Color = fg;
                if (size > 0) cell.Font.Size = size;
                if (center) cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }
            catch { }
        }
        private void H(Excel.Worksheet ws, int r, double h)
        { try { ((Excel.Range)ws.Rows[r]).RowHeight = h; } catch { } }
        private void Yaz(Excel.Worksheet ws, int r, int c, object v)
        { try { ((Excel.Range)ws.Cells[r, c]).Value2 = v; } catch { } }

        private Dictionary<string, double> SM()
        {
            var map = new Dictionary<string, double>();
            foreach (DataGridViewRow r in dgvDosyalar.Rows)
            {
                string yol = r.Cells["colDosyaYolu"].Value?.ToString() ?? "";
                double s = N(r.Cells["colSiparisAdeti"].Value); if (s <= 0) s = 1;
                if (!string.IsNullOrEmpty(yol) && !map.ContainsKey(yol)) map[yol] = s;
            }
            return map;
        }

        private Dictionary<string, MalzemeOzet> Ozet(Dictionary<string, double> sm)
        {
            var ozet = new Dictionary<string, MalzemeOzet>(StringComparer.OrdinalIgnoreCase);
            foreach (var v in _tumVeriler)
            {
                double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                double g = sip * v.MontajAdeti * v.BirimAdet;
                string pro = PG(v.ParcaProfil), kal = KD(v.Kalite);
                if (string.IsNullOrWhiteSpace(pro)) continue;
                string key = pro + "|" + kal;
                if (!ozet.ContainsKey(key)) ozet[key] = new MalzemeOzet { Profil = pro, Kalite = kal };
                ozet[key].ToplamAdet += g; ozet[key].ToplamUzunluk += g * v.Uzunluk;
                ozet[key].ToplamAgirlik += g * v.Agirlik;
            }
            return ozet;
        }

        private bool IsNum(string v)
        {
            double d;
            return double.TryParse(v?.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out d)
                || double.TryParse(v?.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out d);
        }

        private double N(object o)
        {
            if (o == null) return 0;
            string s = o.ToString().Trim(); if (string.IsNullOrEmpty(s)) return 0;
            if (s.Contains(",") && s.Contains("."))
            {
                int a = s.LastIndexOf('.'), b = s.LastIndexOf(',');
                s = a > b ? s.Replace(",", "") : s.Replace(".", "").Replace(",", ".");
            }
            else if (s.Contains(",")) s = s.Replace(",", ".");
            double r;
            return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out r) ? r : 0;
        }

        private void dgvSonuc_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}