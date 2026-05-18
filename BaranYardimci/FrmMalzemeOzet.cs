using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BaranYardimci
{
    public partial class FrmMalzemeOzet : Form
    {
        public class OzetVeri
        {
            public string DosyaId, DosyaAdi, MontajNo, ParcaNo, Profil, Kalite;
            public double MontajAdeti, BirimAdet, Uzunluk, Agirlik, SiparisAdeti;
        }

        class MalzemeOzet { public string Profil, Kalite; public double ToplamAdet, ToplamUzunluk, ToplamAgirlik; }
        class KesimParcasi { public string ParcaNo; public double Uzunluk; public int Adet; }
        class StokBar
        {
            public int BarNo; public double StokBoyu;
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
            public double Fire => Math.Max(0, StokBoyu - Kullanilan);
            public double FirePct => StokBoyu > 0 ? Fire / StokBoyu * 100.0 : 0;
        }
        class ProfilSonuc
        {
            public string Profil, Kalite, OneriNot;
            public double KgPerMetre, ToplamKg, ToplamNetMetre;
            public int Bar6Adet, Bar12Adet, OnerilenAdet;
            public double Bar6ToplamMetre, Bar6ToplamKg, Bar6FirePct;
            public double Bar12ToplamMetre, Bar12ToplamKg, Bar12FirePct;
            public double OnerilenBoy, OnerilenMetre, OnerilenKg, OnerilenFirePct;
            public List<StokBar> OnerilenBarlar = new List<StokBar>();
        }

        static readonly double[] STOK_ADAYLARI = { 6000, 7000, 7500, 8000, 9000, 10000, 11000, 12000 };
        const double KERF_MM = 3.0;

        static int Ole(int r, int g, int b) => ColorTranslator.ToOle(Color.FromArgb(r, g, b));
        static readonly int RenkBaslik = Ole(31, 73, 125);
        static readonly int RenkMavi = Ole(68, 114, 196);
        static readonly int RenkAcik = Ole(222, 235, 247);
        static readonly int RenkYesil = Ole(198, 239, 206);
        static readonly int RenkSari = Ole(255, 235, 156);
        static readonly int RenkKirmizi = Ole(255, 199, 206);
        static readonly int RenkBeyaz = Ole(255, 255, 255);
        static readonly int RenkGri = Ole(242, 242, 242);

        private List<OzetVeri> _veriler;
        private DataGridView dgv;
        private Label lblToplam;

        public FrmMalzemeOzet(List<OzetVeri> veriler)
        {
            _veriler = veriler ?? new List<OzetVeri>();
            ArayuzKur();
            Hesapla();
        }

        private void ArayuzKur()
        {
            this.Text = "Malzeme Özet & İhtiyaç";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Segoe UI", 9.5f);

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.FromArgb(31, 73, 125) };
            var lblTitle = new Label
            {
                Text = "  📊  MALZEME ÖZET & SATIN ALIM",
                Dock = DockStyle.Left,
                Width = 400,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlTop.Controls.Add(lblTitle);
            this.Controls.Add(pnlTop);

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.FromArgb(230, 230, 235), Padding = new Padding(8) };

            var btnYenile = MkBtn("🔄  Yenile", 130, Color.FromArgb(90, 90, 110));
            btnYenile.Click += (s, e) => Hesapla();

            var btnMalzemeExcel = MkBtn("📋  Satın Alım Excel", 180, Color.FromArgb(100, 60, 160));
            btnMalzemeExcel.Click += BtnMalzemeExcel_Click;

            var btnDbKaydet = MkBtn("💾  Veritabanına Kaydet", 200, Color.FromArgb(0, 120, 100));
            btnDbKaydet.Click += BtnDbKaydet_Click;

            var btnKapat = MkBtn("✖  Kapat", 110, Color.FromArgb(160, 40, 40));
            btnKapat.Click += (s, e) => this.Close();

            // Sağdan sola yerleşim
            int x = pnlBottom.ClientSize.Width - 8;
            void Yerlestir(Button b)
            {
                x -= b.Width;
                b.Location = new Point(x, 8);
                b.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                pnlBottom.Controls.Add(b);
                x -= 6;
            }
            Yerlestir(btnKapat);
            Yerlestir(btnDbKaydet);
            Yerlestir(btnMalzemeExcel);

            btnYenile.Location = new Point(8, 8);
            btnYenile.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlBottom.Controls.Add(btnYenile);

            lblToplam = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                BackColor = Color.FromArgb(31, 73, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(68, 114, 196),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 32,
                RowTemplate = { Height = 26 }
            };
            dgv.Columns.Add("colProfil", "Profil");
            dgv.Columns.Add("colKalite", "Kalite");
            dgv.Columns.Add("colAdet", "Toplam Adet");
            dgv.Columns.Add("colUzunluk", "Toplam Uzunluk (mm)");
            dgv.Columns.Add("colAgirlik", "Toplam Ağırlık (kg)");

            this.Controls.Add(dgv);
            this.Controls.Add(lblToplam);
            this.Controls.Add(pnlBottom);
        }

        private Button MkBtn(string text, int w, Color bg)
        {
            var b = new Button
            {
                Text = text,
                Size = new Size(w, 44),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = bg,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        // ── HESAPLA ──────────────────────────────────────────────────────
        private void Hesapla()
        {
            dgv.Rows.Clear();
            var sm = _veriler.GroupBy(v => v.DosyaId).ToDictionary(g => g.Key, g => g.First().SiparisAdeti);
            var ozet = Ozet(sm);

            double topKg = 0;
            foreach (var item in ozet.Values.OrderBy(x => x.Profil).ThenBy(x => x.Kalite))
            {
                dgv.Rows.Add(item.Profil, item.Kalite,
                    Math.Round(item.ToplamAdet, 2),
                    Math.Round(item.ToplamUzunluk, 2),
                    Math.Round(item.ToplamAgirlik, 2));
                topKg += item.ToplamAgirlik;
            }
            lblToplam.Text = $"  TOPLAM:  {ozet.Count} profil türü   —   {Math.Round(topKg, 2):N2} kg";
        }

        private Dictionary<string, MalzemeOzet> Ozet(Dictionary<string, double> sm)
        {
            var ozet = new Dictionary<string, MalzemeOzet>(StringComparer.OrdinalIgnoreCase);
            foreach (var v in _veriler)
            {
                string key = PG(v.Profil) + "|" + KD(v.Kalite);
                double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                double urt = sip * v.MontajAdeti * v.BirimAdet;
                if (!ozet.ContainsKey(key))
                    ozet[key] = new MalzemeOzet { Profil = PG(v.Profil), Kalite = KD(v.Kalite) };
                ozet[key].ToplamAdet += urt;
                ozet[key].ToplamUzunluk += urt * v.Uzunluk;
                ozet[key].ToplamAgirlik += urt * v.Agirlik;
            }
            return ozet;
        }

        private static string PG(string p)
        {
            if (string.IsNullOrEmpty(p)) return p;
            if ((p.StartsWith("PL", StringComparison.OrdinalIgnoreCase) || p.StartsWith("FPL", StringComparison.OrdinalIgnoreCase)) && p.Contains("*"))
                return p.Split('*')[0];
            return p;
        }
        private static string KD(string k)
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

        // ── DB KAYDET ────────────────────────────────────────────────────
        private void BtnDbKaydet_Click(object sender, EventArgs e)
        {
            if (_veriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }

            int musteriID = -1;
            using (var frm = new MusteriSecForm())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog() != DialogResult.OK) return;
                musteriID = frm.SecilenMusteriID;
            }
            var tf = new FrmTerminSec();
            if (tf.ShowDialog() != DialogResult.OK) return;
            DateTime termin = tf.SecilenTarih;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int n = 0;
                using (var conn = new SqlConnection(DB.ConnStr))
                {
                    conn.Open();
                    const string sql = @"INSERT INTO MalzemeKayit (MusteriID,DosyaAdi,MontajNo,ParcaNo,Profil,Kalite,Uzunluk,Agirlik,UretimAdeti,Durum,TerminTarihi) VALUES(@mid,@da,@mn,@pn,@pr,@ka,@uz,@ag,@ua,@du,@tt)";
                    foreach (var v in _veriler)
                    {
                        double urt = v.SiparisAdeti * v.MontajAdeti * v.BirimAdet;
                        using (var cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@mid", musteriID);
                            cmd.Parameters.AddWithValue("@da", v.DosyaAdi ?? "");
                            cmd.Parameters.AddWithValue("@mn", v.MontajNo ?? "");
                            cmd.Parameters.AddWithValue("@pn", v.ParcaNo ?? "");
                            cmd.Parameters.AddWithValue("@pr", PG(v.Profil));
                            cmd.Parameters.AddWithValue("@ka", KD(v.Kalite));
                            cmd.Parameters.AddWithValue("@uz", v.Uzunluk);
                            cmd.Parameters.AddWithValue("@ag", Math.Round(urt * v.Agirlik, 3));
                            cmd.Parameters.AddWithValue("@ua", urt);
                            cmd.Parameters.AddWithValue("@du", "Uretim Bekliyor");
                            cmd.Parameters.AddWithValue("@tt", termin);
                            cmd.ExecuteNonQuery();
                        }
                        n++;
                    }
                }
                MessageBox.Show(n + " kayıt eklendi.", "Tamam", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            finally { Cursor.Current = Cursors.Default; }
        }

        // ── MALZEME / SATIN ALIM EXCEL ───────────────────────────────────
        private void BtnMalzemeExcel_Click(object sender, EventArgs e)
        {
            if (_veriler.Count == 0) { MessageBox.Show("Veri yok!"); return; }

            Cursor.Current = Cursors.WaitCursor;
            var sm = _veriler.GroupBy(v => v.DosyaId).ToDictionary(g => g.Key, g => g.First().SiparisAdeti);
            var dict = new Dictionary<string, List<KesimParcasi>>(StringComparer.OrdinalIgnoreCase);
            var kgPerM = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            foreach (var v in _veriler)
            {
                if (v.Uzunluk <= 0) continue;
                double sip = sm.ContainsKey(v.DosyaId) ? sm[v.DosyaId] : 1;
                int adet = (int)Math.Round(sip * v.MontajAdeti * v.BirimAdet);
                string key = PG(v.Profil) + "||" + KD(v.Kalite);
                if (!dict.ContainsKey(key)) dict[key] = new List<KesimParcasi>();
                var mevcut = dict[key].FirstOrDefault(x => Math.Abs(x.Uzunluk - v.Uzunluk) < 1);
                if (mevcut != null) mevcut.Adet += adet;
                else dict[key].Add(new KesimParcasi { ParcaNo = v.ParcaNo, Uzunluk = v.Uzunluk, Adet = adet });
                if (v.Uzunluk > 0 && v.Agirlik > 0 && !kgPerM.ContainsKey(key))
                    kgPerM[key] = v.Agirlik / (v.Uzunluk / 1000.0);
            }

            var sonuclar = new List<ProfilSonuc>();
            foreach (var kv in dict.OrderBy(x => x.Key))
            {
                string[] pts = kv.Key.Split(new[] { "||" }, StringSplitOptions.None);
                string profil = pts[0], kalite = pts.Length > 1 ? pts[1] : "";
                double kgM = kgPerM.ContainsKey(kv.Key) ? kgPerM[kv.Key] : 0;
                double netMetre = kv.Value.Sum(p => p.Uzunluk * p.Adet) / 1000.0;
                var b6 = FFD(kv.Value, 6000); var b12 = FFD(kv.Value, 12000);
                double fp6 = b6.Count > 0 ? b6.Average(x => x.FirePct) : 0;
                double fp12 = b12.Count > 0 ? b12.Average(x => x.FirePct) : 0;
                double enIyiFire = double.MaxValue, enIyiBoy = 6000;
                List<StokBar> enIyiBarlar = b6;
                foreach (double boy in STOK_ADAYLARI)
                {
                    var bb = FFD(kv.Value, boy);
                    double fp = bb.Count > 0 ? bb.Average(x => x.FirePct) : 0;
                    if (fp < enIyiFire) { enIyiFire = fp; enIyiBoy = boy; enIyiBarlar = bb; }
                }
                sonuclar.Add(new ProfilSonuc
                {
                    Profil = profil,
                    Kalite = kalite,
                    KgPerMetre = kgM,
                    ToplamNetMetre = netMetre,
                    ToplamKg = netMetre * kgM,
                    Bar6Adet = b6.Count,
                    Bar6ToplamMetre = b6.Count * 6,
                    Bar6ToplamKg = b6.Count * 6 * kgM,
                    Bar6FirePct = fp6,
                    Bar12Adet = b12.Count,
                    Bar12ToplamMetre = b12.Count * 12,
                    Bar12ToplamKg = b12.Count * 12 * kgM,
                    Bar12FirePct = fp12,
                    OnerilenBoy = enIyiBoy,
                    OnerilenAdet = enIyiBarlar.Count,
                    OnerilenMetre = enIyiBarlar.Count * enIyiBoy / 1000.0,
                    OnerilenKg = enIyiBarlar.Count * enIyiBoy / 1000.0 * kgM,
                    OnerilenFirePct = enIyiFire,
                    OnerilenBarlar = enIyiBarlar,
                    OneriNot = ""
                });
            }

            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                app = new Excel.Application(); app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Add(Type.Missing);
                var ws = (Excel.Worksheet)wb.Worksheets[1];
                int row = 1;
                S(ws, row, 1, "SATIN ALIM LİSTESİ  —  " + DateTime.Now.ToString("dd.MM.yyyy"),
                    bold: true, bg: RenkBaslik, fg: RenkBeyaz, size: 13); H(ws, row, 18); row++;
                string[] h1 = { "PROFİL","KALİTE","kg/m","GEREKEN NET METRE","GEREKEN NET KG",
                    "6m BAR — ADET","6m — TOPLAM METRE","6m — TOPLAM KG","6m FIRE %",
                    "12m BAR — ADET","12m — TOPLAM METRE","12m — TOPLAM KG","12m FIRE %",
                    "ÖNERİLEN","NOT" };
                for (int i = 0; i < h1.Length; i++) S(ws, row, i + 1, h1[i], bold: true, bg: RenkMavi, fg: RenkBeyaz, center: true);
                H(ws, row, 20); row++;
                int sNo = 0;
                foreach (var s in sonuclar)
                {
                    int bg6 = s.Bar6FirePct > 20 ? RenkKirmizi : s.Bar6FirePct > 10 ? RenkSari : RenkYesil;
                    int bg12 = s.Bar12FirePct > 20 ? RenkKirmizi : s.Bar12FirePct > 10 ? RenkSari : RenkYesil;
                    int rowBg = sNo % 2 == 0 ? RenkBeyaz : RenkAcik;
                    S(ws, row, 1, s.Profil, bg: rowBg);
                    S(ws, row, 2, s.Kalite, bg: rowBg);
                    S(ws, row, 3, s.KgPerMetre, bg: rowBg);
                    S(ws, row, 4, Math.Round(s.ToplamNetMetre, 2), bg: rowBg);
                    S(ws, row, 5, Math.Round(s.ToplamKg, 2), bg: rowBg);
                    S(ws, row, 6, s.Bar6Adet, bg: rowBg);
                    S(ws, row, 7, s.Bar6ToplamMetre, bg: rowBg);
                    S(ws, row, 8, Math.Round(s.Bar6ToplamKg, 2), bg: rowBg);
                    S(ws, row, 9, Math.Round(s.Bar6FirePct, 1), bg: bg6);
                    S(ws, row, 10, s.Bar12Adet, bg: rowBg);
                    S(ws, row, 11, s.Bar12ToplamMetre, bg: rowBg);
                    S(ws, row, 12, Math.Round(s.Bar12ToplamKg, 2), bg: rowBg);
                    S(ws, row, 13, Math.Round(s.Bar12FirePct, 1), bg: bg12);
                    S(ws, row, 14, (s.OnerilenBoy / 1000.0) + " m  x  " + s.OnerilenAdet + " adet  =  " + Math.Round(s.OnerilenMetre, 1) + " m", bg: rowBg);
                    S(ws, row, 15, s.OneriNot, bg: rowBg);
                    row++; sNo++;
                }
                try { int[] w = { 16, 10, 7, 16, 14, 14, 16, 14, 10, 14, 16, 14, 10, 28, 26 }; for (int i = 0; i < w.Length; i++) ((Excel.Range)ws.Columns[i + 1]).ColumnWidth = w[i]; } catch { }
                row += 2;
                S(ws, row, 1, "KESİM PLANI", bold: true, bg: RenkBaslik, fg: RenkBeyaz, size: 12); H(ws, row, 22); row++;
                string[] h2 = { "PROFİL", "KALİTE", "BAR BOYU", "BAR NO", "KULLANILAN (mm)", "FIRE (mm)", "FIRE %", "PARÇALAR" };
                for (int i = 0; i < h2.Length; i++) S(ws, row, i + 1, h2[i], bold: true, bg: RenkMavi, fg: RenkBeyaz, center: true);
                try { ((Excel.Range)ws.Columns[8]).ColumnWidth = 70; } catch { }
                H(ws, row, 20); row++; sNo = 0;
                foreach (var s in sonuclar)
                {
                    string boyStr = (s.OnerilenBoy / 1000.0).ToString("0.0") + " m";
                    foreach (var bar in s.OnerilenBarlar)
                    {
                        string pstr = string.Join("  |  ",
                            bar.Dilimler.Select(d => d.ParcaNo + " (" + d.Uzunluk + "mm x " + d.Adet + ")"));
                        int bgF = bar.FirePct > 20 ? RenkKirmizi : bar.FirePct > 10 ? RenkSari : RenkYesil;
                        int rowBg = sNo % 2 == 0 ? RenkBeyaz : RenkAcik;
                        S(ws, row, 1, s.Profil, bg: rowBg);
                        S(ws, row, 2, s.Kalite, bg: rowBg);
                        S(ws, row, 3, boyStr, bg: rowBg);
                        S(ws, row, 4, bar.BarNo, bg: rowBg);
                        S(ws, row, 5, Math.Round(bar.Kullanilan, 1), bg: rowBg);
                        S(ws, row, 6, Math.Round(bar.Fire, 1), bg: rowBg);
                        S(ws, row, 7, Math.Round(bar.FirePct, 1), bg: bgF);
                        S(ws, row, 8, pstr, bg: rowBg);
                        row++; sNo++;
                    }
                }
                app.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel Hatası: " + ex.Message);
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
                var ayni = hedef.Dilimler.FirstOrDefault(d => d.ParcaNo == parca.ParcaNo && Math.Abs(d.Uzunluk - parca.Uzunluk) < 1);
                if (ayni != null) ayni.Adet++;
                else hedef.Dilimler.Add(new KesimParcasi { ParcaNo = parca.ParcaNo, Uzunluk = parca.Uzunluk, Adet = 1 });
            }
            return barlar;
        }

        private void S(Excel.Worksheet ws, int r, int c, object v, bool bold = false, int bg = 0, int fg = 0, bool center = false, int size = 0)
        {
            try
            {
                var cell = (Excel.Range)ws.Cells[r, c];
                if (v != null) cell.Value2 = v;
                if (bold) cell.Font.Bold = true;
                if (bg != 0) cell.Interior.Color = bg;
                if (fg != 0) cell.Font.Color = fg;
                if (center) cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                if (size > 0) cell.Font.Size = size;
            }
            catch { }
        }
        private void H(Excel.Worksheet ws, int r, double h)
        {
            try { ((Excel.Range)ws.Rows[r]).RowHeight = h; } catch { }
        }
    }
}