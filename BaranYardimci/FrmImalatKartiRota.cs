using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ACadSharp;
using ACadSharp.IO;
using ACadSharp.Entities;
using Button = System.Windows.Forms.Button;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using FontStyle = System.Drawing.FontStyle;
using Excel = Microsoft.Office.Interop.Excel;

namespace BaranYardimci
{
    public partial class FrmImalatKartiRota : Form
    {
        // ── Rota ─────────────────────────────���───────────────────────────
        private static readonly string[] ROTA_KOD = { "K", "D", "M", "O", "S", "B", "W", "G" };
        private static readonly string[] ROTA_ISIM = { "KESME", "DELME", "MARKALAMA", "OYMA", "SIRT ALMA", "BÜKME", "KAYNAK", "GALVANİZ" };

        // ── WNet ─────────────────────────────────────────────────────────
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(ref NETRESOURCE nr, string password, string username, int flags);
        [StructLayout(LayoutKind.Sequential)]
        private struct NETRESOURCE
        {
            public int dwScope, dwType, dwDisplayType, dwUsage;
            public string lpLocalName, lpRemoteName, lpComment, lpProvider;
        }
        const string NET_PATH = @"\\192.168.2.10\










\Hazir_Exceller";
        const string NET_USER = "admin";
        const string NET_PASS = "asd123asd";

        // ── Model ─────────────────────────────────────────────────────────
        private class PozBilgi
        {
            public string PozNo;
            public string PozNoOrijinal;
            public string DwgYolu;
            public string PdfYolu;
            public string GoruntuYolu;
            public int ExcelSatir;
            public double UretilecekAdet;
        }

        // ── ACadSharp nesneleri ───────────────────────────────────────────
        private class CadTransform
        {
            public double X, Y, ScaleX = 1, ScaleY = 1, Rotation;
            public PointF Apply(double x, double y)
            {
                double sx = x * ScaleX, sy = y * ScaleY;
                double cos = Math.Cos(Rotation), sin = Math.Sin(Rotation);
                return new PointF((float)(sx * cos - sy * sin + X),
                                  (float)(sx * sin + sy * cos + Y));
            }
            public static CadTransform Combine(CadTransform p, CadTransform c)
            {
                var pos = p.Apply(c.X, c.Y);
                return new CadTransform
                {
                    X = pos.X,
                    Y = pos.Y,
                    ScaleX = p.ScaleX * c.ScaleX,
                    ScaleY = p.ScaleY * c.ScaleY,
                    Rotation = p.Rotation + c.Rotation
                };
            }
        }
        private class RenderItem
        {
            public string Type;
            public PointF P1, P2;
            public float Radius, StartAngle, EndAngle, Size;
            public List<PointF> Points;
            public string Text;
        }

        // ── State ─────────────────────────────────────────────────────────
        private readonly string _erpExcelYolu;
        private string _imalatKlasor = "";
        private List<PozBilgi> _pozList = new List<PozBilgi>();
        private Dictionary<string, bool[]> _rotaDurumlar = new Dictionary<string, bool[]>(StringComparer.OrdinalIgnoreCase);
        private int _mevcutIndex = -1;
        private Button[] _rotaButonlar = new Button[8];
        private List<string> _geciciDosyalar = new List<string>();

        // ── Constructor ───────────────────────────────────────────────────
        public FrmImalatKartiRota(string erpExcelYolu)
        {
            InitializeComponent();
            _erpExcelYolu = erpExcelYolu;
        }

        private void FrmImalatKartiRota_Load(object sender, EventArgs e)
        {
            var nr = new NETRESOURCE { dwType = 1, lpRemoteName = NET_PATH };
            WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0);

            if (string.IsNullOrEmpty(_erpExcelYolu) || !File.Exists(_erpExcelYolu))
            {
                MessageBox.Show("ERP Excel dosyası bulunamadı:\n" + _erpExcelYolu,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            lblErpDosya.Text = "📄 " + Path.GetFileName(_erpExcelYolu);
            OlusturRotaButonlar();
            ExceldenPozOku();

            if (_pozList.Count == 0)
            {
                MessageBox.Show("Excel'de poz bulunamadı.", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Klasör yolu textbox'ına odaklan
            txtKlasorYol.Focus();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            foreach (var f in _geciciDosyalar)
                try { if (File.Exists(f)) File.Delete(f); } catch { }
        }

        // ════════════════════════════════════════════════════════════════
        //  KLASÖR SEÇİMİ
        // ════════════════════════════════════════════════════════════════

        private void btnKlasorSec_Click(object sender, EventArgs e)
        {
            txtKlasorYol.Focus();
            txtKlasorYol.SelectAll();
        }

        private void btnKlasorGozat_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog
            {
                Description = "İmalat kartlarının bulunduğu klasörü seçiniz  (DWG / DXF / PDF)",
                ShowNewFolderButton = false,
                SelectedPath = txtKlasorYol.Text.Trim()
            };
            if (fbd.ShowDialog() != DialogResult.OK) return;
            txtKlasorYol.Text = fbd.SelectedPath;
            KlasorUygula(fbd.SelectedPath);
        }

        private void btnKlasorGit_Click(object sender, EventArgs e)
        {
            string yol = txtKlasorYol.Text.Trim();
            if (string.IsNullOrEmpty(yol))
            {
                MessageBox.Show("Lütfen bir klasör yolu girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKlasorYol.Focus();
                return;
            }

            if (!Directory.Exists(yol))
            {
                // Ağ yoluysa bağlanmayı dene
                if (yol.StartsWith(@"\\"))
                {
                    var nr = new NETRESOURCE { dwType = 1, lpRemoteName = yol };
                    WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0);
                }

                if (!Directory.Exists(yol))
                {
                    txtKlasorYol.BackColor = Color.FromArgb(80, 30, 30);
                    MessageBox.Show("Klasör bulunamadı:\n" + yol, "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            txtKlasorYol.BackColor = Color.FromArgb(20, 50, 30);
            KlasorUygula(yol);
        }

        private void txtKlasorYol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnKlasorGit_Click(sender, EventArgs.Empty);
            }
        }

        private void KlasorUygula(string yol)
        {
            _imalatKlasor = yol;
            lblKlasor.Text = "📁 " + yol;
            txtKlasorYol.Text = yol;
            txtKlasorYol.BackColor = Color.FromArgb(20, 50, 30);
            DosyalariEsle();
            if (_pozList.Count > 0)
                GosterPoz(_mevcutIndex >= 0 ? _mevcutIndex : 0);
        }

        // ════════════════════════════════════════════════════════════════
        //  ROTA BUTONLARI
        // ════════════════════════════════════════════════════════════════

        private void OlusturRotaButonlar()
        {
            pnlRotaButonlar.Controls.Clear();
            for (int i = 0; i < 8; i++)
            {
                int idx = i;
                var btn = new Button
                {
                    Size = new Size(142, 68),
                    Text = ROTA_KOD[i] + "\r\n" + ROTA_ISIM[i],
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(3),
                    Tag = idx
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, ev) => RotaToggle(idx);
                ButonKirmizi(btn);
                _rotaButonlar[i] = btn;
                pnlRotaButonlar.Controls.Add(btn);
            }
        }

        private void ButonYesil(Button b)
        {
            b.BackColor = Color.FromArgb(39, 174, 96);
            b.ForeColor = Color.White;
            b.FlatAppearance.BorderColor = Color.FromArgb(30, 130, 76);
            b.FlatAppearance.BorderSize = 2;
        }

        private void ButonKirmizi(Button b)
        {
            b.BackColor = Color.FromArgb(192, 57, 43);
            b.ForeColor = Color.White;
            b.FlatAppearance.BorderColor = Color.FromArgb(150, 40, 30);
            b.FlatAppearance.BorderSize = 2;
        }

        private void RotaToggle(int idx)
        {
            if (_mevcutIndex < 0 || _mevcutIndex >= _pozList.Count) return;
            string poz = _pozList[_mevcutIndex].PozNo;
            if (!_rotaDurumlar.ContainsKey(poz)) _rotaDurumlar[poz] = new bool[8];
            _rotaDurumlar[poz][idx] = !_rotaDurumlar[poz][idx];
            if (_rotaDurumlar[poz][idx]) ButonYesil(_rotaButonlar[idx]);
            else ButonKirmizi(_rotaButonlar[idx]);
            GuncelleRotaLabel(poz);
            GuncelleRotaOzeti(poz);
            ListeyiGuncelle();
        }

        private string RotaKoduOlustur(string pozNo)
        {
            if (!_rotaDurumlar.ContainsKey(pozNo)) return "";
            var sb = new StringBuilder();
            bool[] d = _rotaDurumlar[pozNo];
            for (int i = 0; i < 8; i++) if (d[i]) sb.Append(ROTA_KOD[i]);
            return sb.ToString();
        }

        private void GuncelleRotaLabel(string pozNo)
        {
            string kod = RotaKoduOlustur(pozNo);
            if (string.IsNullOrEmpty(kod)) { lblRotaKodu.Text = "Rota: (seçilmedi)"; return; }
            var isimler = kod.ToCharArray().Select(c =>
            {
                int i = Array.IndexOf(ROTA_KOD, c.ToString());
                return i >= 0 ? ROTA_ISIM[i] : "?";
            });
            lblRotaKodu.Text = "Rota: " + kod + "  →  " + string.Join(" + ", isimler);
        }

        private void GuncelleRotaOzeti(string pozNo)
        {
            pnlRotaOzeti.Controls.Clear();
            bool[] dur = _rotaDurumlar.ContainsKey(pozNo) ? _rotaDurumlar[pozNo] : new bool[8];

            var baslik = new Label
            {
                Text = "SEÇİLEN ROTALAR",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(160, 160, 180),
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(30, 30, 38)
            };
            pnlRotaOzeti.Controls.Add(baslik);

            for (int i = 7; i >= 0; i--)
            {
                int idx = i;
                var lbl = new Label
                {
                    Text = ROTA_KOD[i] + "  " + ROTA_ISIM[i],
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    Height = 38,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(2),
                    BackColor = dur[i] ? Color.FromArgb(39, 174, 96) : Color.FromArgb(60, 40, 40),
                    ForeColor = dur[i] ? Color.White : Color.FromArgb(140, 100, 100)
                };
                lbl.Click += (s, ev) => RotaToggle(idx);
                pnlRotaOzeti.Controls.Add(lbl);
            }
            baslik.BringToFront();
        }

        // ════════════════════════════════════════════════════════════════
        //  DWG / DXF → PNG  (ACadSharp)
        // ════════════════════════════════════════════════════════════════

        private string DwgdenPngYap(string kaynak)
        {
            try
            {
                string gecici = Path.Combine(Path.GetTempPath(),
                    "IMK_" + Path.GetFileNameWithoutExtension(kaynak) +
                    "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + ".png");

                CadDocument doc;
                string ext = Path.GetExtension(kaynak).ToLower();
                if (ext == ".dxf")
                    using (var r = new DxfReader(kaynak)) doc = r.Read();
                else
                    using (var r = new DwgReader(kaynak)) doc = r.Read();

                var items = new List<RenderItem>();
                var baseTr = new CadTransform();
                foreach (var entity in doc.Entities)
                    CollectEntity(entity, items, baseTr);

                if (items.Count == 0) return null;

                RenderToPng(items, gecici);
                _geciciDosyalar.Add(gecici);
                return gecici;
            }
            catch (Exception ex)
            {
                lblDosyaBilgi.Text = "⚠ DWG okuma hatası: " + ex.Message;
                lblDosyaBilgi.ForeColor = Color.FromArgb(220, 100, 50);
                return null;
            }
        }

        private void CollectEntity(Entity entity, List<RenderItem> list, CadTransform tr)
        {
            if (entity is Insert ins)
            {
                var bt = new CadTransform
                {
                    X = ins.InsertPoint.X,
                    Y = ins.InsertPoint.Y,
                    ScaleX = ins.XScale,
                    ScaleY = ins.YScale,
                    Rotation = ins.Rotation
                };
                var newTr = CadTransform.Combine(tr, bt);
                foreach (var sub in ins.Block.Entities)
                    CollectEntity(sub, list, newTr);
            }
            else if (entity is ACadSharp.Entities.Line ln)
            {
                list.Add(new RenderItem
                {
                    Type = "Line",
                    P1 = tr.Apply(ln.StartPoint.X, ln.StartPoint.Y),
                    P2 = tr.Apply(ln.EndPoint.X, ln.EndPoint.Y)
                });
            }
            else if (entity is ACadSharp.Entities.Circle ci)
            {
                list.Add(new RenderItem
                {
                    Type = "Circle",
                    P1 = tr.Apply(ci.Center.X, ci.Center.Y),
                    Radius = (float)(ci.Radius * tr.ScaleX)
                });
            }
            else if (entity is LwPolyline pl)
            {
                var pts = pl.Vertices
                    .Select(v => tr.Apply(v.Location.X, v.Location.Y))
                    .ToList();
                if (pl.IsClosed && pts.Count > 0) pts.Add(pts[0]);
                list.Add(new RenderItem { Type = "Polyline", Points = pts });
            }
            else if (entity is ACadSharp.Entities.Arc arc)
            {
                list.Add(new RenderItem
                {
                    Type = "Arc",
                    P1 = tr.Apply(arc.Center.X, arc.Center.Y),
                    Radius = (float)(arc.Radius * tr.ScaleX),
                    StartAngle = (float)(arc.StartAngle + tr.Rotation),
                    EndAngle = (float)(arc.EndAngle + tr.Rotation)
                });
            }
            else if (entity is TextEntity txt)
            {
                list.Add(new RenderItem
                {
                    Type = "Text",
                    P1 = tr.Apply(txt.InsertPoint.X, txt.InsertPoint.Y),
                    Text = txt.Value,
                    Size = (float)(txt.Height * tr.ScaleX)
                });
            }
            else if (entity is MText mt)
            {
                list.Add(new RenderItem
                {
                    Type = "Text",
                    P1 = tr.Apply(mt.InsertPoint.X, mt.InsertPoint.Y),
                    Text = mt.Value,
                    Size = (float)(mt.Height * tr.ScaleX)
                });
            }
        }

        private void RenderToPng(List<RenderItem> items, string cikisYolu)
        {
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            void Check(PointF p)
            {
                if (p.X < minX) minX = p.X; if (p.X > maxX) maxX = p.X;
                if (p.Y < minY) minY = p.Y; if (p.Y > maxY) maxY = p.Y;
            }

            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case "Line": Check(item.P1); Check(item.P2); break;
                    case "Circle":
                    case "Arc":
                        Check(new PointF(item.P1.X - item.Radius, item.P1.Y - item.Radius));
                        Check(new PointF(item.P1.X + item.Radius, item.P1.Y + item.Radius));
                        break;
                    case "Polyline":
                        if (item.Points != null)
                            foreach (var p in item.Points) Check(p);
                        break;
                    case "Text": Check(item.P1); break;
                }
            }

            int W = 3508, H = 2480, PAD = 80; // A4 300dpi landscape
            float dW = maxX - minX; if (dW <= 0) dW = 1;
            float dH = maxY - minY; if (dH <= 0) dH = 1;
            float sc = Math.Min((W - PAD * 2) / dW, (H - PAD * 2) / dH);
            float offX = (W - dW * sc) / 2f;
            float offY = (H - dH * sc) / 2f;

            PointF ToScreen(PointF p) => new PointF(
                (p.X - minX) * sc + offX,
                H - ((p.Y - minY) * sc + offY));

            using (var bmp = new Bitmap(W, H))
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                using (var pen = new Pen(Color.Black, 1.5f))
                using (var br = new SolidBrush(Color.Black))
                {
                    foreach (var item in items)
                    {
                        switch (item.Type)
                        {
                            case "Line":
                                g.DrawLine(pen, ToScreen(item.P1), ToScreen(item.P2));
                                break;

                            case "Circle":
                                {
                                    var c = ToScreen(item.P1);
                                    float r = item.Radius * sc;
                                    g.DrawEllipse(pen, c.X - r, c.Y - r, r * 2, r * 2);
                                    break;
                                }
                            case "Polyline":
                                if (item.Points != null && item.Points.Count > 1)
                                    for (int i = 0; i < item.Points.Count - 1; i++)
                                        g.DrawLine(pen,
                                            ToScreen(item.Points[i]),
                                            ToScreen(item.Points[i + 1]));
                                break;

                            case "Arc":
                                {
                                    var c = ToScreen(item.P1);
                                    float r = item.Radius * sc;
                                    float s = (float)(item.StartAngle * 180 / Math.PI);
                                    float en = (float)(item.EndAngle * 180 / Math.PI);
                                    float sw = en - s;
                                    if (sw < 0) sw += 360;
                                    try { g.DrawArc(pen, c.X - r, c.Y - r, r * 2, r * 2, -s, -sw); } catch { }
                                    break;
                                }
                            case "Text":
                                {
                                    float fs = Math.Max(8f, Math.Min(item.Size * sc, 180f));
                                    var pos = ToScreen(item.P1);
                                    pos.Y -= fs;
                                    g.DrawString(item.Text, new Font("Arial", fs), br, pos);
                                    break;
                                }
                        }
                    }
                }
                bmp.Save(cikisYolu, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  EXCEL OKU
        // ════════════════════════════════════════════════════════════════

        private void ExceldenPozOku()
        {
            _pozList.Clear();
            lbPozlar.Items.Clear();

            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                app = new Excel.Application();
                app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Open(_erpExcelYolu, false, true,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);

                var ws = (Excel.Worksheet)wb.Worksheets[1];
                int last = ws.UsedRange.Rows.Count;

                // Poz bazında adetleri topla (G=7. sütun)
                var pozAdetler = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
                for (int r = 2; r <= last; r++)
                {
                    object pv = ((Excel.Range)ws.Cells[r, 3]).Value2;
                    if (pv == null) continue;
                    string p = pv.ToString().Replace("/", "").Trim();
                    object av = ((Excel.Range)ws.Cells[r, 7]).Value2;
                    double a = 0;
                    if (av != null) double.TryParse(av.ToString(),
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out a);
                    if (pozAdetler.ContainsKey(p)) pozAdetler[p] += a;
                    else pozAdetler[p] = a;
                }

                var islenen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                for (int r = 2; r <= last; r++)
                {
                    object pv = ((Excel.Range)ws.Cells[r, 3]).Value2;
                    if (pv == null) continue;
                    string orijinal = pv.ToString().Trim();
                    if (string.IsNullOrEmpty(orijinal)) continue;
                    string temiz = orijinal.Replace("/", "").Replace("\\", "").Trim();
                    if (islenen.Contains(temiz)) continue;
                    islenen.Add(temiz);

                    object rv = ((Excel.Range)ws.Cells[r, 5]).Value2;
                    string mevcutRota = rv?.ToString().Trim() ?? "";

                    var poz = new PozBilgi
                    {
                        PozNo = temiz,
                        PozNoOrijinal = orijinal,
                        ExcelSatir = r,
                        UretilecekAdet = pozAdetler.ContainsKey(temiz) ? pozAdetler[temiz] : 0
                    };

                    if (!string.IsNullOrEmpty(mevcutRota) && !_rotaDurumlar.ContainsKey(temiz))
                    {
                        bool[] dur = new bool[8];
                        for (int i = 0; i < 8; i++)
                            dur[i] = mevcutRota.Contains(ROTA_KOD[i]);
                        _rotaDurumlar[temiz] = dur;
                    }
                    _pozList.Add(poz);
                }
            }
            catch (Exception ex) { MessageBox.Show("Excel okuma hatası: " + ex.Message); }
            finally
            {
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
            }

            lblIlerleme.Text = $"0 / {_pozList.Count} poz";
            ListeyiGuncelle();
        }

        // ════════════════════════════════════════════════════════════════
        //  DOSYA EŞLEŞTİRME
        // ════════════════════════════════════════════════════════════════

        private void DosyalariEsle()
        {
            if (string.IsNullOrEmpty(_imalatKlasor)) return;

            var tumDosyalar = new List<string>();
            try
            {
                foreach (string ext in new[] { "*.dwg", "*.dxf", "*.pdf" })
                    tumDosyalar.AddRange(
                        Directory.GetFiles(_imalatKlasor, ext, SearchOption.AllDirectories));
            }
            catch { }

            foreach (var poz in _pozList)
            {
                string aranan = Temizle(poz.PozNo);
                poz.DwgYolu = null; poz.PdfYolu = null; poz.GoruntuYolu = null;

                foreach (var dosya in tumDosyalar)
                {
                    string isim = Temizle(Path.GetFileNameWithoutExtension(dosya));
                    string ext = Path.GetExtension(dosya).ToLower();

                    if ((ext == ".dwg" || ext == ".dxf") && isim == aranan && poz.DwgYolu == null)
                        poz.DwgYolu = dosya;
                    if (ext == ".pdf" && isim == aranan && poz.PdfYolu == null)
                        poz.PdfYolu = dosya;
                }

                // DWG ile aynı dizinde PDF var mı?
                if (poz.PdfYolu == null && poz.DwgYolu != null)
                {
                    string yan = Path.ChangeExtension(poz.DwgYolu, ".pdf");
                    if (File.Exists(yan)) poz.PdfYolu = yan;
                }
            }
            ListeyiGuncelle();
        }

        private static string Temizle(string s) =>
            s.Replace("/", "").Replace("\\", "").Replace("-", "")
             .Replace("_", "").Replace(" ", "").ToUpper();

        // ════════════════════════════════════════════════════════════════
        //  POZ GÖSTER
        // ════════════════════════════════════════════════════════════════

        private void GosterPoz(int index)
        {
            if (index < 0 || index >= _pozList.Count) return;
            _mevcutIndex = index;
            var poz = _pozList[index];

            lbPozlar.SelectedIndex = index;
            lblPozAdi.Text = $"[{index + 1}/{_pozList.Count}]   POZ: {poz.PozNo}";
            lblIlerleme.Text = $"{index + 1} / {_pozList.Count} poz";
            lblAdet.Text = poz.UretilecekAdet > 0
                ? $"ÜRETİLECEK: {poz.UretilecekAdet:0.##} ADET"
                : "Adet bilgisi yok";

            bool[] dur = _rotaDurumlar.ContainsKey(poz.PozNo)
                ? _rotaDurumlar[poz.PozNo] : new bool[8];
            for (int i = 0; i < 8; i++)
            {
                if (dur[i]) ButonYesil(_rotaButonlar[i]);
                else ButonKirmizi(_rotaButonlar[i]);
            }
            GuncelleRotaLabel(poz.PozNo);
            GuncelleRotaOzeti(poz.PozNo);

            // ── Gösterim önceliği: PDF > Mevcut PNG > DWG/DXF'i çevir ──
            if (poz.PdfYolu != null && File.Exists(poz.PdfYolu))
            {
                lblDosyaBilgi.Text = "📄 PDF  →  " + Path.GetFileName(poz.PdfYolu);
                lblDosyaBilgi.ForeColor = Color.FromArgb(160, 220, 160);
                try { webBrowser.Navigate(new Uri(poz.PdfYolu)); }
                catch { GosterHtml(poz, "PDF yüklenemedi."); }
            }
            else if (poz.GoruntuYolu != null && File.Exists(poz.GoruntuYolu))
            {
                GosterGoruntu(poz, poz.GoruntuYolu);
            }
            else if (poz.DwgYolu != null)
            {
                string uzanti = Path.GetExtension(poz.DwgYolu).ToUpper().TrimStart('.');
                lblDosyaBilgi.Text = $"⏳ {uzanti} → PNG dönüştürülüyor...";
                lblDosyaBilgi.ForeColor = Color.FromArgb(200, 160, 0);
                Application.DoEvents();

                string png = DwgdenPngYap(poz.DwgYolu);
                if (png != null)
                {
                    poz.GoruntuYolu = png;
                    GosterGoruntu(poz, png);
                }
                else
                {
                    GosterHtml(poz,
                        $"{uzanti} dosyası bulundu fakat görüntüye çevrilemedi.<br>" +
                        "ACadSharp kütüphanesi yüklü mü kontrol edin.");
                }
            }
            else
            {
                GosterHtml(poz, "Eşleşen dosya bulunamadı.<br>Klasör yolunu kontrol edin.");
            }

            ListeyiGuncelle();
        }

        private void GosterGoruntu(PozBilgi poz, string pngYolu)
        {
            string uzanti = poz.DwgYolu != null
                ? Path.GetExtension(poz.DwgYolu).ToUpper().TrimStart('.') : "DWG";
            lblDosyaBilgi.Text = $"🔧 {uzanti} → PNG  |  {Path.GetFileName(pngYolu)}";
            lblDosyaBilgi.ForeColor = Color.FromArgb(160, 200, 255);

            string base64;
            try { base64 = Convert.ToBase64String(File.ReadAllBytes(pngYolu)); }
            catch { GosterHtml(poz, "Görüntü yüklenemedi."); return; }

            webBrowser.DocumentText = $@"<!DOCTYPE html>
<html>
<head>
<style>
  * {{ margin:0; padding:0; box-sizing:border-box; }}
  body {{ background:#1a1a20; display:flex; align-items:center;
          justify-content:center; width:100%; height:100vh; overflow:hidden; }}
  img  {{ max-width:100%; max-height:100vh; object-fit:contain;
          box-shadow:0 0 40px #000; border:1px solid #333; }}
</style>
</head>
<body>
  <img src='data:image/png;base64,{base64}' alt='POZ: {poz.PozNo}' />
</body>
</html>";
        }

        private void GosterHtml(PozBilgi poz, string mesaj)
        {
            string dosyaBilgi = poz.DwgYolu != null
                ? $"<code style='color:#7fb3d3'>{poz.DwgYolu}</code>"
                : "<span style='color:#e74c3c'>Klasörde eşleşen dosya bulunamadı</span>";

            lblDosyaBilgi.Text = "⚠  " + mesaj.Replace("<br>", " ");
            lblDosyaBilgi.ForeColor = Color.FromArgb(220, 150, 60);

            webBrowser.DocumentText = $@"<!DOCTYPE html>
<html>
<body style='font-family:Segoe UI;background:#1a1a20;color:#ccc;padding:60px;'>
  <h2 style='color:#e67e22;margin-bottom:10px'>⚠  POZ: {poz.PozNo}</h2>
  <p style='font-size:15px;margin-bottom:16px'>{mesaj}</p>
  <p style='font-size:13px'>{dosyaBilgi}</p>
</body>
</html>";
        }

        private void ListeyiGuncelle()
        {
            int sel = lbPozlar.SelectedIndex;
            lbPozlar.BeginUpdate();
            lbPozlar.Items.Clear();
            foreach (var p in _pozList)
            {
                string ikon = p.PdfYolu != null ? "📄"
                            : p.GoruntuYolu != null ? "🖼"
                            : p.DwgYolu != null ? "🔧" : "❌";
                string rota = RotaKoduOlustur(p.PozNo);
                string adet = p.UretilecekAdet > 0 ? $"×{p.UretilecekAdet:0.##}" : "";
                lbPozlar.Items.Add(
                    $"{ikon} {p.PozNo,-16} {adet,-6} [{(string.IsNullOrEmpty(rota) ? "—" : rota)}]");
            }
            lbPozlar.EndUpdate();
            if (sel >= 0 && sel < lbPozlar.Items.Count)
                lbPozlar.SelectedIndex = sel;
        }

        // ════════════════════════════════════════════════════════════════
        //  İMALAT KARTLARINI YAZDIR
        // ════════════════════════════════════════════════════════════════

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            if (_pozList.Count == 0) { MessageBox.Show("Poz listesi boş."); return; }

            // Henüz PNG'ye çevrilmemiş DWG'leri çevir
            lblDosyaBilgi.Text = "⏳ Tüm pozlar hazırlanıyor...";
            lblDosyaBilgi.ForeColor = Color.FromArgb(200, 200, 100);
            Application.DoEvents();

            for (int i = 0; i < _pozList.Count; i++)
            {
                var poz = _pozList[i];
                if (poz.GoruntuYolu != null && File.Exists(poz.GoruntuYolu)) continue;
                if (poz.DwgYolu == null) continue;
                lblDosyaBilgi.Text = $"⏳ [{i + 1}/{_pozList.Count}] {poz.PozNo} dönüştürülüyor...";
                Application.DoEvents();
                string png = DwgdenPngYap(poz.DwgYolu);
                if (png != null) poz.GoruntuYolu = png;
            }

            var dr = MessageBox.Show(
                $"Yazdırma kapsamını seçin:\n\n" +
                $"[Evet]   →  Yalnızca mevcut poz  ({(_mevcutIndex >= 0 ? _pozList[_mevcutIndex].PozNo : "—")})\n" +
                $"[Hayır]  →  Tüm {_pozList.Count} poz",
                "İmalat Kartlarını Yazdır",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (dr == DialogResult.Cancel) return;

            var liste = dr == DialogResult.Yes
                ? _pozList.Where((p, i) => i == _mevcutIndex).ToList()
                : _pozList.ToList();

            YazdirPozlar(liste);
        }

        private void YazdirPozlar(List<PozBilgi> liste)
        {
            using (var pd = new PrintDialog())
            {
                pd.UseEXDialog = true;
                if (pd.ShowDialog() != DialogResult.OK) return;

                using (var doc = new System.Drawing.Printing.PrintDocument())
                {
                    doc.PrinterSettings = pd.PrinterSettings;
                    doc.DefaultPageSettings.Landscape = true;

                    int idx = 0;
                    doc.PrintPage += (s, ev) =>
                    {
                        var poz = liste[idx];
                        var g = ev.Graphics;
                        var sayfa = ev.PageBounds;
                        g.Clear(Color.White);

                        // ── Başlık bandı ──────────────────────────────────
                        using (var hBr = new SolidBrush(Color.FromArgb(31, 73, 125)))
                            g.FillRectangle(hBr, sayfa.X, sayfa.Y, sayfa.Width, 58);

                        string rotaKod = RotaKoduOlustur(poz.PozNo);
                        string adetStr = poz.UretilecekAdet > 0
                            ? $"ÜRETİLECEK: {poz.UretilecekAdet:0.##} ADET" : "";

                        using (var fn = new Font("Segoe UI", 16f, FontStyle.Bold))
                        using (var wb2 = new SolidBrush(Color.White))
                            g.DrawString($"POZ: {poz.PozNo}", fn, wb2,
                                new PointF(sayfa.X + 18, sayfa.Y + 10));

                        if (!string.IsNullOrEmpty(adetStr))
                            using (var fn2 = new Font("Segoe UI", 13f, FontStyle.Bold))
                            using (var yBr = new SolidBrush(Color.FromArgb(255, 230, 80)))
                                g.DrawString(adetStr, fn2, yBr,
                                    new PointF(sayfa.X + sayfa.Width / 2f - 130, sayfa.Y + 14));

                        if (!string.IsNullOrEmpty(rotaKod))
                            using (var fn3 = new Font("Segoe UI", 11f, FontStyle.Bold))
                            using (var gBr = new SolidBrush(Color.FromArgb(160, 255, 160)))
                                g.DrawString($"ROTA: {rotaKod}", fn3, gBr,
                                    new PointF(sayfa.X + sayfa.Width - 230, sayfa.Y + 18));

                        // ── Görüntü alanı ─────────────────────────────────
                        var imgRect = new Rectangle(
                            sayfa.X + 8, sayfa.Y + 66,
                            sayfa.Width - 16, sayfa.Height - 90);

                        string goruntYolu = poz.PdfYolu ?? poz.GoruntuYolu;

                        if (goruntYolu != null && File.Exists(goruntYolu) &&
                            !goruntYolu.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                using (var img = Image.FromFile(goruntYolu))
                                {
                                    float rW = (float)imgRect.Width / img.Width;
                                    float rH = (float)imgRect.Height / img.Height;
                                    float rt = Math.Min(rW, rH);
                                    int iW = (int)(img.Width * rt);
                                    int iH = (int)(img.Height * rt);
                                    int iX = imgRect.X + (imgRect.Width - iW) / 2;
                                    int iY = imgRect.Y + (imgRect.Height - iH) / 2;
                                    g.DrawImage(img, iX, iY, iW, iH);
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            using (var fn4 = new Font("Segoe UI", 18f))
                            using (var grBr = new SolidBrush(Color.Gray))
                                g.DrawString("Görüntü mevcut değil",
                                    fn4, grBr,
                                    new PointF(imgRect.X + 40, imgRect.Y + 40));
                        }

                        // ── Alt bant: rota açıklamaları ───────────────────
                        if (!string.IsNullOrEmpty(rotaKod))
                        {
                            var isimler = rotaKod.ToCharArray().Select(c =>
                            {
                                int ii = Array.IndexOf(ROTA_KOD, c.ToString());
                                return ii >= 0 ? ROTA_ISIM[ii] : c.ToString();
                            });
                            using (var fns = new Font("Segoe UI", 9f))
                            using (var dbr = new SolidBrush(Color.FromArgb(50, 50, 50)))
                                g.DrawString("Rotalar: " + string.Join(" → ", isimler),
                                    fns, dbr,
                                    new PointF(sayfa.X + 18,
                                               sayfa.Y + sayfa.Height - 20));
                        }

                        idx++;
                        ev.HasMorePages = idx < liste.Count;
                    };

                    try { doc.Print(); }
                    catch (Exception ex) { MessageBox.Show("Yazdırma hatası: " + ex.Message); }
                }
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  OLAYLAR
        // ════════════════════════════════════════════════════════════════

        private void btnGeri_Click(object sender, EventArgs e)
        { if (_mevcutIndex > 0) GosterPoz(_mevcutIndex - 1); }

        private void btnIleri_Click(object sender, EventArgs e)
        { if (_mevcutIndex < _pozList.Count - 1) GosterPoz(_mevcutIndex + 1); }

        private void lbPozlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPozlar.SelectedIndex >= 0 && lbPozlar.SelectedIndex != _mevcutIndex)
                GosterPoz(lbPozlar.SelectedIndex);
        }

        private void btnTumunuSec_Click(object sender, EventArgs e)
        {
            if (_mevcutIndex < 0) return;
            string poz = _pozList[_mevcutIndex].PozNo;
            _rotaDurumlar[poz] = new bool[] { true, true, true, true, true, true, true, true };
            for (int i = 0; i < 8; i++) ButonYesil(_rotaButonlar[i]);
            GuncelleRotaLabel(poz); GuncelleRotaOzeti(poz); ListeyiGuncelle();
        }

        private void btnHepsiniTemizle_Click(object sender, EventArgs e)
        {
            if (_mevcutIndex < 0) return;
            string poz = _pozList[_mevcutIndex].PozNo;
            _rotaDurumlar[poz] = new bool[8];
            for (int i = 0; i < 8; i++) ButonKirmizi(_rotaButonlar[i]);
            GuncelleRotaLabel(poz); GuncelleRotaOzeti(poz); ListeyiGuncelle();
        }

        private void btnAcDosya_Click(object sender, EventArgs e)
        {
            if (_mevcutIndex < 0 || _mevcutIndex >= _pozList.Count) return;
            var poz = _pozList[_mevcutIndex];
            string d = poz.PdfYolu ?? poz.DwgYolu;
            if (d != null && File.Exists(d))
                try { System.Diagnostics.Process.Start(d); }
                catch (Exception ex) { MessageBox.Show("Açılamadı: " + ex.Message); }
            else
                MessageBox.Show("Dosya bulunamadı.");
        }

        // ════════════════════════════════════════════════════════════════
        //  KAYDET & ÇIKIŞ
        // ════════════════════════════════════════════════════════════════

        private void btnKaydetCikis_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Excel.Application app = null; Excel.Workbook wb = null;
            try
            {
                var nr = new NETRESOURCE { dwType = 1, lpRemoteName = NET_PATH };
                WNetAddConnection2(ref nr, NET_PASS, NET_USER, 0);

                app = new Excel.Application();
                app.Visible = false; app.DisplayAlerts = false;
                wb = app.Workbooks.Open(_erpExcelYolu, false, false,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);

                var ws = (Excel.Worksheet)wb.Worksheets[1];

                object bas5 = ((Excel.Range)ws.Cells[1, 5]).Value2;
                if (bas5 == null || bas5.ToString().Trim().ToUpper() != "ROTA")
                {
                    ((Excel.Range)ws.Columns[5]).Insert(
                        Excel.XlInsertShiftDirection.xlShiftToRight);
                    ((Excel.Range)ws.Cells[1, 5]).Value2 = "ROTA";
                }

                foreach (var poz in _pozList)
                    ((Excel.Range)ws.Cells[poz.ExcelSatir, 5]).Value2 =
                        RotaKoduOlustur(poz.PozNo);

                wb.Save();
                MessageBox.Show(
                    $"{_pozList.Count} poz için rota kaydedildi.\n\n{_erpExcelYolu}",
                    "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { MessageBox.Show("Excel yazma hatası: " + ex.Message); }
            finally
            {
                try { if (wb != null) { wb.Close(false); Marshal.ReleaseComObject(wb); } } catch { }
                try { if (app != null) { app.Quit(); Marshal.ReleaseComObject(app); } } catch { }
                GC.Collect(); GC.WaitForPendingFinalizers();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}