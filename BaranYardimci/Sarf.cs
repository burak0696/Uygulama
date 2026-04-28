using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace BaranYardimci
{
    public partial class Sarf : Form
    {
        // ── Timerlar ───────────────────────────────────────────────────
        private readonly Timer _dovizTmr = new Timer();
        private readonly Timer _autoRefreshTmr = new Timer();

        // ── Sepet ──────────────────────────────────────────────────────
        private DataTable _sepet = new DataTable();

        // ── Barkod zamanlama ───────────────────────────────────────────
        private DateTime _sonBarkodZaman = DateTime.MinValue;

        // ── Teklif paneli ──────────────────────────────────────────────
        private Panel _pnlTeklif;
        private Label _lblTeklifBilgi;

        // ── Tedarikçi Seçim Overlay ────────────────────────────────────
        private Panel _pnlTedOverlay;
        private DataGridView _dgvTedSecim;
        private TextBox _txtTedAra, _txtTedFiyat;
        private TextBox _txtYeniTedFirma, _txtYeniTedTel;
        private Label _lblTedSecimBaslik;
        private string _aktifTalepNo;
        private int _aktifUrunId;
        private string _aktifUrunAdi;
        private decimal _aktifMiktar;
        private string _aktifBirim;

        // ── Depo giriş paneli ──────────────────────────────────────────
        private Panel _pnlDepoGiris;
        private TextBox _txtDepoAra, _txtDepoFatura, _txtDepoIrsaliye;
        private Label _lblDepoSonuc;

        // ── Renkler ────────────────────────────────────────────────────
        private readonly Color _clrAktif = Color.FromArgb(37, 99, 235);
        private readonly Color _clrYesil = Color.FromArgb(16, 185, 129);
        private readonly Color _clrSari = Color.FromArgb(245, 158, 11);
        private readonly Color _clrKirmizi = Color.FromArgb(239, 68, 68);
        private readonly Color _clrDusukStok = Color.FromArgb(254, 226, 226);
        private readonly Color _clrOrtaStok = Color.FromArgb(254, 243, 199);
        private readonly Color _clrTamStok = Color.FromArgb(209, 250, 229);

        private const int KRITIK = 5;
        private const int DUSUK = 20;
        private const decimal LIM_MD = 5_000m;
        private const decimal LIM_GM = 25_000m;

        // ════════════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ════════════════════════════════════════════════════════════════

        public Sarf()
        {
            InitializeComponent();
        }

        // ════════════════════════════════════════════════════════════════
        //  FORM LOAD
        // ════════════════════════════════════════════════════════════════

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                TlsAyarla();
                UiAyarla();
                TeklifPaneliOlustur();
                TedarikciSecimOverlayOlustur();
                DepoGirisPaneliOlustur();
                SepetOlustur();
                ArayuzHazirla();
                DovizGetir();
                DashboardGuncelle();
                SayfaGit(0, "📊  GENEL BAKIŞ");
                MenuAktif(btnMenuDash);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yükleme hatası: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _dovizTmr.Stop();
            _autoRefreshTmr.Stop();
        }

        private static void TlsAyarla()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (s, c, ch, er) => true;
        }

        // ════════════════════════════════════════════════════════════════
        //  UI AYARLA
        // ════════════════════════════════════════════════════════════════

        private void UiAyarla()
        {
            DataGridView[] grids = {
                dgvTalepSepet, dgvTeklifListesi, dgvOnayListesi,
                dgvStokListesi, dgvStokDurumu,   dgvLogListesi,
                dgvTedarikci,   dgvRapor,         dgvKategoriler
            };
            foreach (var g in grids)
            {
                if (g == null) continue;
                g.DefaultCellStyle.SelectionBackColor = _clrAktif;
                g.DefaultCellStyle.SelectionForeColor = Color.White;
                g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
                g.AllowUserToResizeRows = false;
                g.RowHeadersVisible = false;
                g.CellFormatting += Grid_Formatla;
                g.RowPostPaint += Grid_RowBorder;
            }

            if (cmbPopBirim.Items.Count == 0)
                cmbPopBirim.Items.AddRange(new object[] {
                    "Adet","Kg","Lt","Mt","Kutu","Paket","Rulo","Çift","Takım","Set","M²","M³","Ton"
                });

            dgvTalepSepet.KeyDown += SepetKeyDown;
            dgvStokDurumu.CellDoubleClick += (s, ev) =>
            { if (ev.RowIndex >= 0) StokDusumSatirSec(ev.RowIndex); };

            tabControlAna.SelectedIndexChanged += (s, ev) =>
            {
                switch (tabControlAna.SelectedIndex)
                {
                    case 2: TeklifYukle(); break;
                    case 3: OnayYukle(); break;
                    case 4: GirisYukle(); break;
                    case 5: StokDurumYukle(); break;
                    case 6: TedarikciYukle(); break;
                    case 7: KategoriYukle(); break;
                    case 8: LogYukle(); break;
                }
            };

            _dovizTmr.Interval = 300_000;
            _dovizTmr.Tick += (s, ev) => DovizGetir();
            _dovizTmr.Start();

            _autoRefreshTmr.Interval = 60_000;
            _autoRefreshTmr.Tick += (s, ev) =>
            { if (tabControlAna.SelectedIndex == 0) DashboardGuncelle(); };
            _autoRefreshTmr.Start();
        }

        // ════════════════════════════════════════════════════════════════
        //  TEKLİF PANELİ (sadece info + buton)
        // ════════════════════════════════════════════════════════════════

        private void TeklifPaneliOlustur()
        {
            var tab = tabControlAna.TabPages[2];
            dgvTeklifListesi.Dock = DockStyle.Fill;
            dgvTeklifListesi.ContextMenuStrip = null; // sağ tık menüsü yok

            _pnlTeklif = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(8, 8, 8, 8)
            };

            _lblTeklifBilgi = new Label
            {
                Location = new Point(8, 14),
                AutoSize = false,
                Width = 720,
                Height = 32,
                Font = new Font("Segoe UI", 10f, FontStyle.Italic),
                ForeColor = Color.FromArgb(148, 163, 184),
                Text = "Listeden bir kalem seçin, ardından 'Teklif Al' butonuna tıklayın."
            };

            var btnTeklifAl = BtnYap("💰  Teklif Al / Tedarikçi Seç",
                Color.FromArgb(16, 185, 129), new Point(740, 10), new Size(260, 40));
            btnTeklifAl.Click += BtnTeklifAl_Click;

            _pnlTeklif.Controls.AddRange(new Control[] { _lblTeklifBilgi, btnTeklifAl });
            tab.Controls.Add(_pnlTeklif);

            dgvTeklifListesi.SelectionChanged += (s, ev) =>
            {
                if (dgvTeklifListesi.SelectedRows.Count == 0) return;
                var row = dgvTeklifListesi.SelectedRows[0];
                string urun = row.Cells["UrunAdi"]?.Value?.ToString() ?? "";
                string mik = row.Cells["Miktar"]?.Value?.ToString() ?? "";
                string birim = row.Cells["Birim"]?.Value?.ToString() ?? "";
                string durum = row.Cells["Durum"]?.Value?.ToString() ?? "";
                _lblTeklifBilgi.Text = $"Seçili: {urun}  |  {mik} {birim}  |  {durum}";
            };
        }

        private void BtnTeklifAl_Click(object sender, EventArgs e)
        {
            if (dgvTeklifListesi.SelectedRows.Count == 0)
            { MessageBox.Show("Teklif almak için bir kalem se��in."); return; }

            var row = dgvTeklifListesi.SelectedRows[0];

            _aktifTalepNo = row.Cells["_TalepNo"]?.Value?.ToString() ?? "";
            _aktifUrunId = I(row.Cells["_UrunId"]?.Value);
            _aktifUrunAdi = row.Cells["UrunAdi"]?.Value?.ToString() ?? "";
            _aktifMiktar = Convert.ToDecimal(row.Cells["Miktar"]?.Value ?? 0);
            _aktifBirim = row.Cells["Birim"]?.Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(_aktifTalepNo) || _aktifUrunId == 0)
            { MessageBox.Show("Satır bilgisi okunamadı."); return; }

            // Başlık güncelle
            _lblTedSecimBaslik.Text =
                $"Tedarikçi Seç  —  {_aktifUrunAdi}  ({_aktifMiktar} {_aktifBirim})";

            // Mevcut teklifler listele
            TedSecimGridYukle();

            // Overlay göster
            _pnlTedOverlay.Visible = true;
            _pnlTedOverlay.BringToFront();
            _txtTedAra.Focus();
        }

        // ════════════════════════════════════════════════════════════════
        //  TEDARİKÇİ SEÇİM OVERLAY
        // ════════════════════════════════════════════��═══════════════════

        private void TedarikciSecimOverlayOlustur()
        {
            _pnlTedOverlay = new Panel
            {
                Size = new Size(780, 520),
                BackColor = Color.FromArgb(15, 23, 42),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            _pnlTedOverlay.Paint += (s, ev) =>
            {
                using (var p = new System.Drawing.Pen(Color.FromArgb(59, 130, 246), 2))
                    ev.Graphics.DrawRectangle(p, 0, 0,
                        _pnlTedOverlay.Width - 1, _pnlTedOverlay.Height - 1);
            };

            // Başlık
            _lblTedSecimBaslik = new Label
            {
                Location = new Point(12, 12),
                AutoSize = false,
                Width = 650,
                Height = 28,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Text = "Tedarikçi Seç"
            };

            var btnKapat = BtnYap("✖", Color.FromArgb(239, 68, 68),
                new Point(734, 8), new Size(36, 36));
            btnKapat.Click += (s, ev) => _pnlTedOverlay.Visible = false;

            // ── Ara ────────────────────────────────────────────────────
            var lblAra = LblYap("Tedarikçi Ara:", Color.FromArgb(148, 163, 184));
            lblAra.Location = new Point(12, 52);

            _txtTedAra = new TextBox
            {
                Location = new Point(110, 48),
                Width = 380,
                Font = new Font("Segoe UI", 11f),
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White
            };
            _txtTedAra.TextChanged += (s, ev) => TedSecimGridYukle(_txtTedAra.Text.Trim());

            // ── Tedarikçi grid ─────────────────────────────────────────
            _dgvTedSecim = new DataGridView
            {
                Location = new Point(12, 82),
                Size = new Size(756, 180),
                BackgroundColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 32 }
            };
            _dgvTedSecim.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            _dgvTedSecim.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _dgvTedSecim.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            _dgvTedSecim.ColumnHeadersHeight = 34;
            _dgvTedSecim.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            _dgvTedSecim.DefaultCellStyle.SelectionForeColor = Color.White;
            _dgvTedSecim.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            _dgvTedSecim.DefaultCellStyle.ForeColor = Color.White;
            _dgvTedSecim.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(51, 65, 85);

            // ── Yeni tedarikçi ekle ────────────────────────────────────
            var pnlYeni = new Panel
            {
                Location = new Point(12, 272),
                Size = new Size(756, 60),
                BackColor = Color.FromArgb(22, 33, 62)
            };

            var lblYeni = LblYap("+ Yeni Tedarikçi:", Color.FromArgb(245, 158, 11));
            lblYeni.Location = new Point(6, 20);
            lblYeni.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            _txtYeniTedFirma = new TextBox
            {
                Location = new Point(110, 16),
                Width = 280,
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White
            };
            GriYazi(_txtYeniTedFirma, "Firma adı...");

            _txtYeniTedTel = new TextBox
            {
                Location = new Point(398, 16),
                Width = 160,
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White
            };
            GriYazi(_txtYeniTedTel, "Telefon...");

            var btnYeniTedEkle = BtnYap("➕ Ekle",
                Color.FromArgb(245, 158, 11), new Point(566, 14), new Size(90, 34));
            btnYeniTedEkle.Click += BtnYeniTedEkle_Click;

            pnlYeni.Controls.AddRange(new Control[] {
                lblYeni, _txtYeniTedFirma, _txtYeniTedTel, btnYeniTedEkle
            });

            // ── Mevcut teklifler (bu kalem için) ───────────────────────
            var lblMev = LblYap("Bu kalem için mevcut teklifler:", Color.FromArgb(148, 163, 184));
            lblMev.Location = new Point(12, 344);

            var dgvMevcut = new DataGridView
            {
                Name = "dgvMevcutTeklifler",
                Location = new Point(12, 368),
                Size = new Size(756, 80),
                BackgroundColor = Color.FromArgb(22, 33, 62),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 30 }
            };
            dgvMevcut.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 65, 85);
            dgvMevcut.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMevcut.ColumnHeadersHeight = 30;
            dgvMevcut.DefaultCellStyle.BackColor = Color.FromArgb(22, 33, 62);
            dgvMevcut.DefaultCellStyle.ForeColor = Color.White;

            // ── Fiyat + Onayla ─────────────────────────────────────────
            var lblFiy = LblYap("Birim Fiyat (₺):*", Color.FromArgb(148, 163, 184));
            lblFiy.Location = new Point(12, 460);
            lblFiy.Font = new Font("Segoe UI", 10f, FontStyle.Bold);

            _txtTedFiyat = new TextBox
            {
                Location = new Point(140, 456),
                Width = 180,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.FromArgb(16, 185, 129),
                Text = "0"
            };

            var btnOnayla = BtnYap("✅  Onayla & Gönder",
                Color.FromArgb(16, 185, 129), new Point(340, 454), new Size(220, 42));
            btnOnayla.Click += BtnTedSecimOnayla_Click;

            var btnIptal = BtnYap("✖  İptal",
                Color.FromArgb(100, 116, 139), new Point(572, 454), new Size(120, 42));
            btnIptal.Click += (s, ev) => _pnlTedOverlay.Visible = false;

            _pnlTedOverlay.Controls.AddRange(new Control[] {
                _lblTedSecimBaslik, btnKapat,
                lblAra, _txtTedAra,
                _dgvTedSecim,
                pnlYeni,
                lblMev, dgvMevcut,
                lblFiy, _txtTedFiyat,
                btnOnayla, btnIptal
            });

            // Form'a ekle
            Controls.Add(_pnlTedOverlay);
            _pnlTedOverlay.BringToFront();

            // Boyutlandırma / konumlandırma
            SizeChanged += (s, ev) => TedOverlayKonumla();
            TedOverlayKonumla();
        }

        private void TedOverlayKonumla()
        {
            if (_pnlTedOverlay == null) return;
            _pnlTedOverlay.Location = new Point(
                (Width - _pnlTedOverlay.Width) / 2,
                (Height - _pnlTedOverlay.Height) / 2);
        }

        private void TedSecimGridYukle(string filtre = "")
        {
            try
            {
                string sql = string.IsNullOrWhiteSpace(filtre)
                    ? "SELECT Id, FirmaAdi, YetkiliKisi, Telefon FROM Tedarikciler WHERE Aktif=1 ORDER BY FirmaAdi"
                    : "SELECT Id, FirmaAdi, YetkiliKisi, Telefon FROM Tedarikciler WHERE Aktif=1 AND (FirmaAdi LIKE @f OR YetkiliKisi LIKE @f) ORDER BY FirmaAdi";

                DataTable dt = string.IsNullOrWhiteSpace(filtre)
                    ? DB.GetTable(sql)
                    : DB.GetTable(sql, P("@f", "%" + filtre + "%"));

                _dgvTedSecim.DataSource = null;
                _dgvTedSecim.DataSource = dt;
                if (_dgvTedSecim.Columns["Id"] != null)
                    _dgvTedSecim.Columns["Id"].Visible = false;

                // Mevcut teklifler
                var dgvMev = _pnlTedOverlay.Controls.OfType<DataGridView>()
                    .FirstOrDefault(g => g.Name == "dgvMevcutTeklifler");
                if (dgvMev != null && _aktifUrunId > 0 && !string.IsNullOrEmpty(_aktifTalepNo))
                {
                    dgvMev.DataSource = null;
                    dgvMev.DataSource = DB.GetTable(
                        @"SELECT ISNULL(Tedarikci,'—') AS Tedarikci,
                                 ISNULL(BirimFiyat,0)  AS [Birim Fiyat ₺],
                                 CASE OnayDurumu WHEN 3 THEN '⌛ Onay Bekleniyor'
                                                WHEN 1 THEN '✅ Onaylandı'
                                                ELSE '⏳ Fiyat Girildi' END AS Durum
                          FROM   TalepDetay
                          WHERE  TalepNo=@no AND UrunId=@uid AND ISNULL(BirimFiyat,0) > 0",
                        P("@no", _aktifTalepNo), P("@uid", _aktifUrunId));
                    dgvMev.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 65, 85);
                    dgvMev.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgvMev.DefaultCellStyle.BackColor = Color.FromArgb(22, 33, 62);
                    dgvMev.DefaultCellStyle.ForeColor = Color.White;
                    dgvMev.ColumnHeadersHeight = 30;
                }
            }
            catch { }
        }

        private void BtnYeniTedEkle_Click(object sender, EventArgs e)
        {
            string firma = _txtYeniTedFirma.ForeColor == Color.Gray ? "" : _txtYeniTedFirma.Text.Trim();
            if (string.IsNullOrWhiteSpace(firma))
            { MessageBox.Show("Firma adı girin."); _txtYeniTedFirma.Focus(); return; }

            if (I(DB.GetValue("SELECT COUNT(*) FROM Tedarikciler WHERE FirmaAdi=@f",
                P("@f", firma))) > 0)
            { MessageBox.Show("Bu tedarikçi zaten kayıtlı."); return; }

            string tel = _txtYeniTedTel.ForeColor == Color.Gray ? "" : _txtYeniTedTel.Text.Trim();
            DB.Execute("INSERT INTO Tedarikciler (FirmaAdi,Telefon,Aktif) VALUES (@f,@t,1)",
                P("@f", firma), P("@t", tel));

            _txtYeniTedFirma.ForeColor = Color.Gray;
            _txtYeniTedFirma.Text = "Firma adı...";
            _txtYeniTedTel.ForeColor = Color.Gray;
            _txtYeniTedTel.Text = "Telefon...";

            TedSecimGridYukle(_txtTedAra.Text.Trim());
            MessageBox.Show($"✔  '{firma}' eklendi. Listeden seçebilirsiniz.", "Başarılı",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnTedSecimOnayla_Click(object sender, EventArgs e)
        {
            if (_dgvTedSecim.SelectedRows.Count == 0)
            { MessageBox.Show("Listeden tedarikçi seçin."); _dgvTedSecim.Focus(); return; }

            if (!decimal.TryParse(
                    _txtTedFiyat.Text.Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal fiy) || fiy <= 0)
            { MessageBox.Show("Geçerli birim fiyat girin."); _txtTedFiyat.Focus(); return; }

            string ted = _dgvTedSecim.SelectedRows[0].Cells["FirmaAdi"]?.Value?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(ted)) return;

            decimal top = fiy * _aktifMiktar;
            string sev = top >= LIM_GM ? "  (🔴 Genel Müdür onayı gerekli)" :
                           top >= LIM_MD ? "  (🟡 Müdür onayı gerekli)" : "";

            // Aynı tedarikçi zaten var mı?
            int varMi = I(DB.GetValue(
                "SELECT COUNT(*) FROM TalepDetay WHERE TalepNo=@no AND UrunId=@uid AND Tedarikci=@ted",
                P("@no", _aktifTalepNo), P("@uid", _aktifUrunId), P("@ted", ted)));

            if (varMi > 0)
            {
                DB.Execute(
                    "UPDATE TalepDetay SET BirimFiyat=@fiy, OnayDurumu=3 WHERE TalepNo=@no AND UrunId=@uid AND Tedarikci=@ted",
                    P("@fiy", fiy), P("@no", _aktifTalepNo),
                    P("@uid", _aktifUrunId), P("@ted", ted));
            }
            else
            {
                DB.Execute(
                    @"INSERT INTO TalepDetay
                        (TalepNo,UrunId,Miktar,BirimFiyat,Tedarikci,OnayDurumu,Oncelik,Tarih)
                      SELECT TalepNo,UrunId,Miktar,@fiy,@ted,3,ISNULL(Oncelik,2),GETDATE()
                      FROM   TalepDetay
                      WHERE  TalepNo=@no AND UrunId=@uid
                      ORDER  BY Id OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY",
                    P("@fiy", fiy), P("@ted", ted),
                    P("@no", _aktifTalepNo), P("@uid", _aktifUrunId));
            }

            _pnlTedOverlay.Visible = false;
            TeklifYukle();
            DashboardGuncelle();
            MessageBox.Show($"✔  {ted} — {fiy:N2} ₺ — onaya gönderildi.{sev}",
                "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ═══════════════════════════════════════════════════��════════════
        //  DEPO GİRİŞ PANELİ
        // ════════════════════════════════════════════════════════════════

        private void DepoGirisPaneliOlustur()
        {
            var tab = tabControlAna.TabPages[4];
            dgvStokListesi.Dock = DockStyle.Fill;
            dgvStokListesi.MultiSelect = true;
            dgvStokListesi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // txtFaturaNo / txtIrsaliyeNo — Designer'daki fazladan alanları gizle
            try { if (txtFaturaNo != null) txtFaturaNo.Visible = false; } catch { }
            try { if (txtIrsaliyeNo != null) txtIrsaliyeNo.Visible = false; } catch { }

            _pnlDepoGiris = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(10, 8, 10, 6)
            };

            // ── Satır 1: Fatura / İrsaliye ─────────────────────────────
            var lblFat = LblYap("Fatura No:", Color.FromArgb(148, 163, 184));
            lblFat.Location = new Point(10, 12);

            _txtDepoFatura = new TextBox
            {
                Location = new Point(82, 8),
                Width = 180,
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White
            };

            var lblIrs = LblYap("İrsaliye No: *", Color.FromArgb(255, 220, 80));
            lblIrs.Location = new Point(278, 12);
            lblIrs.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            _txtDepoIrsaliye = new TextBox
            {
                Location = new Point(376, 8),
                Width = 180,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.FromArgb(255, 220, 80)
            };

            // ── Satır 2: Arama + Butonlar ──────────────────────────────
            var lblAra = LblYap("Ürün / Barkod:", Color.FromArgb(148, 163, 184));
            lblAra.Location = new Point(10, 54);

            _txtDepoAra = new TextBox
            {
                Location = new Point(100, 50),
                Width = 260,
                Font = new Font("Segoe UI", 10f),
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White
            };
            _txtDepoAra.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Enter) DepoUrunAra(); };

            _lblDepoSonuc = new Label
            {
                Location = new Point(374, 54),
                AutoSize = false,
                Width = 260,
                Height = 28,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(148, 163, 184),
                Text = "Bekleyen tüm girişler listeleniyor"
            };

            var btnAra = BtnYap("🔍 Ara",
                Color.FromArgb(59, 130, 246), new Point(646, 48), new Size(100, 34));
            btnAra.Click += (s, ev) => DepoUrunAra();

            var btnTumunu = BtnYap("📋 Tümü",
                Color.FromArgb(100, 116, 139), new Point(756, 48), new Size(100, 34));
            btnTumunu.Click += (s, ev) => { _txtDepoAra.Clear(); GirisYukle(); };

            var btnIsle = BtnYap("📦  İşaretlenenleri Depoya Al",
                Color.FromArgb(16, 185, 129), new Point(866, 48), new Size(260, 34));
            btnIsle.Click += BtnDepoIsle_Click;

            _pnlDepoGiris.Controls.AddRange(new Control[] {
                lblFat, _txtDepoFatura,
                lblIrs, _txtDepoIrsaliye,
                lblAra, _txtDepoAra, _lblDepoSonuc,
                btnAra, btnTumunu, btnIsle
            });

            tab.Controls.Add(_pnlDepoGiris);
        }

        private void DepoUrunAra()
        {
            string aranan = _txtDepoAra.Text.Trim();
            if (string.IsNullOrWhiteSpace(aranan)) { GirisYukle(); return; }

            DepoGridYukle(
                @"SELECT TD.Id AS KayitId, U.UrunAdi,
                         ISNULL(U.Barkod,'—')              AS Barkod,
                         TD.Miktar, U.Birim,
                         ISNULL(TD.BirimFiyat,0)           AS BirimFiyat,
                         TD.Miktar * ISNULL(TD.BirimFiyat,0) AS ToplamTutar,
                         ISNULL(TD.Tedarikci,'—')          AS Tedarikci,
                         ISNULL(TD.SiparisNo,'—')          AS SiparisNo,
                         ISNULL(U.RafYeri,'—')             AS RafYeri,
                         T.TalepEden,
                         CONVERT(VARCHAR,T.Tarih,103)      AS TalepTarihi
                  FROM   TalepDetay TD
                  JOIN   Urunler    U ON U.Id      = TD.UrunId
                  JOIN   Talepler   T ON T.TalepNo = TD.TalepNo
                  WHERE  TD.OnayDurumu = 1
                    AND  (TD.IrsaliyeNo IS NULL OR TD.IrsaliyeNo = '')
                    AND  (U.UrunAdi LIKE @ara OR U.Barkod LIKE @ara)
                  ORDER  BY T.Tarih DESC",
                P("@ara", "%" + aranan + "%"));

            _lblDepoSonuc.Text = $"{dgvStokListesi.Rows.Count} sonuç bulundu";
            _lblDepoSonuc.ForeColor = Color.FromArgb(16, 185, 129);
        }

        private void DepoGridYukle(string sql, params SqlParameter[] prms)
        {
            try
            {
                dgvStokListesi.DataSource = null;
                dgvStokListesi.DataSource = DB.GetTable(sql, prms);

                foreach (DataGridViewColumn c in dgvStokListesi.Columns)
                    if (c.Name == "KayitId") c.Visible = false;

                // Checkbox sütunu yoksa ekle
                if (!dgvStokListesi.Columns.Contains("_Sec"))
                {
                    var chk = new DataGridViewCheckBoxColumn
                    {
                        Name = "_Sec",
                        HeaderText = "✓",
                        Width = 38,
                        DisplayIndex = 0,
                        TrueValue = true,
                        FalseValue = false
                    };
                    dgvStokListesi.Columns.Add(chk);
                }
                dgvStokListesi.Columns["_Sec"].DisplayIndex = 0;
                dgvStokListesi.AllowUserToResizeRows = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[DepoGridYukle] " + ex.Message);
            }
        }

        private void BtnDepoIsle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtDepoIrsaliye.Text))
            {
                MessageBox.Show("İrsaliye No zorunludur.", "Uyarı",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtDepoIrsaliye.Focus(); return;
            }

            // Checkbox'ı işaretli satırları bul
            var seciliSatirlar = dgvStokListesi.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["_Sec"].Value != null &&
                            Convert.ToBoolean(r.Cells["_Sec"].Value))
                .ToList();

            if (seciliSatirlar.Count == 0)
            {
                MessageBox.Show("Depoya alınacak satırları işaretleyin (✓ sütunu).", "Uyarı",
                MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            int islem = 0;
            foreach (var r in seciliSatirlar)
            {
                if (r.Cells["KayitId"].Value == null) continue;
                int id = Convert.ToInt32(r.Cells["KayitId"].Value);
                decimal mik = Convert.ToDecimal(r.Cells["Miktar"].Value ?? 0);

                DB.Execute(
                    "UPDATE TalepDetay SET FaturaNo=@fat, IrsaliyeNo=@irs, TeslimTarihi=GETDATE() WHERE Id=@id",
                    P("@fat", _txtDepoFatura.Text.Trim()),
                    P("@irs", _txtDepoIrsaliye.Text.Trim()),
                    P("@id", id));

                DB.Execute(
                    "UPDATE Urunler SET StokMiktari = StokMiktari + @mik WHERE Id = (SELECT UrunId FROM TalepDetay WHERE Id=@id)",
                    P("@mik", mik), P("@id", id));

                DB.Execute(
                    @"INSERT INTO StokHareketleri (UrunId,Miktar,IslemTuru,KisiAdi,Sebep,Tarih)
                      SELECT UrunId,@mik,'Giriş',@fat,@irs,GETDATE() FROM TalepDetay WHERE Id=@id",
                    P("@mik", mik),
                    P("@fat", _txtDepoFatura.Text.Trim()),
                    P("@irs", _txtDepoIrsaliye.Text.Trim()),
                    P("@id", id));
                islem++;
            }

            MessageBox.Show($"✔  {islem} kalem depoya alındı.", "Başarılı",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            _txtDepoFatura.Clear();
            _txtDepoIrsaliye.Clear();
            _txtDepoAra.Clear();
            GirisYukle();
            DashboardGuncelle();
        }

        private void btnStogaIsle_Click(object sender, EventArgs e)
            => BtnDepoIsle_Click(sender, e);

        // ════════════════════════════════════════════════════════════════
        //  SEPET
        // ════════════════════════════════════════════════════════════════

        private void SepetOlustur()
        {
            _sepet.Columns.Add("UrunId", typeof(int));
            _sepet.Columns.Add("Barkod");
            _sepet.Columns.Add("UrunAdi");
            _sepet.Columns.Add("Miktar", typeof(decimal));
            _sepet.Columns.Add("Birim");
            _sepet.Columns.Add("Aciklama");
            _sepet.Columns.Add("Oncelik");

            dgvTalepSepet.DataSource = _sepet;
            if (dgvTalepSepet.Columns["UrunId"] != null)
                dgvTalepSepet.Columns["UrunId"].Visible = false;
        }

        private void SepetKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || dgvTalepSepet.SelectedRows.Count == 0) return;
            dgvTalepSepet.SelectedRows.Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow)
                .Select(r => r.Index)
                .OrderByDescending(i => i)
                .ToList()
                .ForEach(i => _sepet.Rows.RemoveAt(i));
        }

        private void ArayuzHazirla()
        {
            cmbIslemTuru.Items.Clear();
            cmbIslemTuru.Items.AddRange(new object[] { "Tümü", "Giriş", "Çıkış" });
            cmbIslemTuru.SelectedIndex = 0;
            dtpBaslangic.Value = DateTime.Now.AddDays(-30);
            dtpBitis.Value = DateTime.Now;
            dtpRaporBas.Value = DateTime.Now.AddMonths(-1);
            dtpRaporBit.Value = DateTime.Now;
            KategoriComboDoldur();
            MenuBadgeGuncelle();
        }

        // ════════════════════════════════════════════════════════════════
        //  GRID FORMATLAMA
        // ════════════════════════════════════════════════════════════════

        private void Grid_Formatla(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!(sender is DataGridView dgv) || e.RowIndex < 0) return;
            var row = dgv.Rows[e.RowIndex];
            string col = dgv.Columns[e.ColumnIndex].Name;

            if (col == "RenkKodu" && e.Value != null)
            {
                try
                {
                    var c = ColorTranslator.FromHtml(e.Value.ToString());
                    e.CellStyle.BackColor = c; e.CellStyle.ForeColor = c;
                }
                catch { }
                return;
            }

            if (dgv == dgvStokDurumu && col == "StokMiktari")
            {
                var v = row.Cells["StokMiktari"].Value;
                if (v != null && v != DBNull.Value)
                {
                    int s = Convert.ToInt32(v);
                    var bg = s <= KRITIK ? _clrDusukStok : s <= DUSUK ? _clrOrtaStok : Color.White;
                    if (bg != Color.White)
                    { e.CellStyle.BackColor = bg; e.CellStyle.ForeColor = Color.FromArgb(55, 65, 81); }
                }
            }

            if (col == "Durum" && e.Value != null)
            {
                string d = e.Value.ToString();
                if (d.Contains("onayda") || d.Contains("Onay"))
                {
                    e.CellStyle.BackColor = Color.FromArgb(254, 243, 199);
                    e.CellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                }
                else if (d.Contains("bekleniyor") || d.Contains("Fiyat"))
                {
                    e.CellStyle.BackColor = Color.FromArgb(219, 234, 254);
                    e.CellStyle.ForeColor = Color.FromArgb(30, 64, 175);
                }
                else if (d.Contains("teklif verdi"))
                {
                    e.CellStyle.BackColor = _clrTamStok;
                    e.CellStyle.ForeColor = Color.FromArgb(6, 95, 70);
                }
            }

            if (col == "Oncelik" && e.Value != null)
            {
                int o = Convert.ToInt32(e.Value);
                e.Value = o == 1 ? "🔴 ACİL" : o == 2 ? "🟡 NORMAL" : "🟢 DÜŞÜK";
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                e.FormattingApplied = true;
            }

            if (col == "IslemTuru" && e.Value != null)
            {
                e.CellStyle.ForeColor = e.Value.ToString() == "Giriş" ? _clrYesil : _clrKirmizi;
                e.CellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            }
        }

        private void Grid_RowBorder(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (var p = new System.Drawing.Pen(Color.FromArgb(229, 231, 235)))
                e.Graphics.DrawLine(p,
                    e.RowBounds.Left, e.RowBounds.Bottom - 1,
                    e.RowBounds.Right, e.RowBounds.Bottom - 1);
        }

        private void GridYukle(DataGridView dgv, string sql, params SqlParameter[] prms)
        {
            try
            {
                int sel = dgv.SelectedRows.Count > 0 ? dgv.SelectedRows[0].Index : -1;
                dgv.DataSource = null;
                dgv.DataSource = DB.GetTable(sql, prms);
                foreach (DataGridViewColumn c in dgv.Columns)
                {
                    if (c.Name == "KayitId" || c.Name.StartsWith("_")) c.Visible = false;
                    if (c.Name == "UrunGorseli" && c is DataGridViewImageColumn ic)
                        ic.ImageLayout = DataGridViewImageCellLayout.Zoom;
                }
                dgv.AllowUserToResizeRows = false;
                if (sel >= 0 && sel < dgv.Rows.Count) dgv.Rows[sel].Selected = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[GridYukle] " + ex.Message);
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  SAYFA / MENÜ
        // ════════════════════════════════════════════════════════════════

        private void SayfaGit(int index, string baslik)
        {
            tabControlAna.SelectedIndex = index;
            lblBaslik.Text = baslik;
        }

        private void MenuAktif(Button aktifBtn)
        {
            foreach (Control c in layoutMenu.Controls)
                if (c is Button b)
                { b.BackColor = Color.Transparent; b.ForeColor = Color.FromArgb(209, 213, 219); }
            if (aktifBtn != null)
            { aktifBtn.BackColor = _clrAktif; aktifBtn.ForeColor = Color.White; }
        }

        private void MenuBadgeGuncelle()
        {
            try
            {
                int teklif = I(DB.GetValue("SELECT COUNT(*) FROM TalepDetay WHERE OnayDurumu=0"));
                int onay = I(DB.GetValue("SELECT COUNT(*) FROM TalepDetay WHERE OnayDurumu=3"));
                int giris = I(DB.GetValue(
                    "SELECT COUNT(*) FROM TalepDetay WHERE OnayDurumu=1 AND (IrsaliyeNo IS NULL OR IrsaliyeNo='')"));
                int kritik = I(DB.GetValue($"SELECT COUNT(*) FROM Urunler WHERE StokMiktari<={KRITIK}"));

                btnMenuTeklif.Text = teklif > 0 ? $"💰  TEKLİF AL  [{teklif}]" : "💰  TEKLİF AL";
                btnMenuOnay.Text = onay > 0 ? $"✅  ONAY  [{onay}]" : "✅  ONAY EKRANI";
                btnMenuStok.Text = giris > 0 ? $"📦  DEPO GİRİŞ  [{giris}]" : "📦  DEPO GİRİŞ";
                btnMenuDusum.Text = kritik > 0 ? $"🔻  STOK DÜŞÜM  ⚠{kritik}" : "🔻  STOK DÜŞÜM";
            }
            catch { }
        }

        private void btnMenuDash_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuDash); SayfaGit(0, "📊  GENEL BAKIŞ"); DashboardGuncelle(); }
        private void btnMenuTalep_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuTalep); SayfaGit(1, "📝  TALEP OLUŞTURMA"); }
        private void btnMenuTeklif_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuTeklif); SayfaGit(2, "💰  TEKLİF ALMA"); TeklifYukle(); }
        private void btnMenuOnay_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuOnay); SayfaGit(3, "✅  YÖNETİCİ ONAYI"); OnayYukle(); }
        private void btnMenuStok_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuStok); SayfaGit(4, "📦  DEPO GİRİŞ"); GirisYukle(); }
        private void btnMenuDusum_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuDusum); SayfaGit(5, "🔻  STOK DÜŞÜM"); StokDurumYukle(); }
        private void btnMenuTedarikci_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuTedarikci); SayfaGit(6, "🏭  TEDARİKÇİ YÖNETİMİ"); TedarikciYukle(); }
        private void btnMenuKategori_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuKategori); SayfaGit(7, "🏷  KATEGORİ YÖNETİMİ"); KategoriYukle(); }
        private void btnMenuLog_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuLog); SayfaGit(8, "📋  LOG KAYITLARI"); LogYukle(); }
        private void btnMenuRapor_Click(object sender, EventArgs e)
        { MenuAktif(btnMenuRapor); SayfaGit(9, "📈  RAPORLAMA"); KategoriComboDoldur(); }
        private void btnMenuCikis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkmak istiyor musunuz?", "Çıkış",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        // ════════════════════════════════════════════════════════════════
        //  DASHBOARD
        // ════════════════════════════════════════════════════════════════

        private void DashboardGuncelle()
        {
            try
            {
                lblDash1Val.Text = I(DB.GetValue("SELECT COUNT(*) FROM Talepler WHERE AsamaKodu=1")).ToString();
                lblDash2Val.Text = I(DB.GetValue("SELECT COUNT(*) FROM TalepDetay WHERE OnayDurumu=3")).ToString();
                lblDash3Val.Text = I(DB.GetValue(
                    "SELECT COUNT(*) FROM TalepDetay WHERE OnayDurumu=1 AND (IrsaliyeNo IS NULL OR IrsaliyeNo='')")).ToString();
                lblDash4Val.Text = I(DB.GetValue("SELECT COUNT(*) FROM Urunler")).ToString();

                int kritik = I(DB.GetValue($"SELECT COUNT(*) FROM Urunler WHERE StokMiktari<={KRITIK}"));
                KritikPanelGuncelle(kritik);

                chartHarcama.Series[0].Points.Clear();
                chartHarcama.Series[0].ChartType = SeriesChartType.Pie;
                chartHarcama.Series[0]["PieLabelStyle"] = "Outside";
                chartHarcama.Series[0]["PieLineColor"] = "Black";

                var dtK = DB.GetTable(@"
                    SELECT K.KategoriAdi, K.RenkKodu,
                           ISNULL(SUM(TD.Miktar*TD.BirimFiyat),0) AS Tutar
                    FROM   Kategoriler K
                    LEFT JOIN Urunler    U  ON U.KategoriId=K.Id
                    LEFT JOIN TalepDetay TD ON TD.UrunId=U.Id AND TD.OnayDurumu=1
                    GROUP BY K.KategoriAdi, K.RenkKodu
                    HAVING ISNULL(SUM(TD.Miktar*TD.BirimFiyat),0) > 0");
                foreach (DataRow r in dtK.Rows)
                {
                    int ix = chartHarcama.Series[0].Points.AddXY(
                        r["KategoriAdi"], Convert.ToDouble(r["Tutar"]));
                    try
                    {
                        chartHarcama.Series[0].Points[ix].Color =
                            ColorTranslator.FromHtml(r["RenkKodu"].ToString());
                    }
                    catch { }
                }

                chartStok.Series[0].Points.Clear();
                chartStok.Series[0].ChartType = SeriesChartType.Bar;
                var dtS = DB.GetTable("SELECT TOP 10 UrunAdi, StokMiktari FROM Urunler ORDER BY StokMiktari ASC");
                foreach (DataRow r in dtS.Rows)
                {
                    int stok = Convert.ToInt32(r["StokMiktari"]);
                    int ix = chartStok.Series[0].Points.AddXY(r["UrunAdi"], stok);
                    chartStok.Series[0].Points[ix].Color =
                        stok <= KRITIK ? _clrKirmizi : stok <= DUSUK ? _clrSari : _clrYesil;
                    chartStok.Series[0].Points[ix].Label = stok.ToString();
                }

                MenuBadgeGuncelle();
            }
            catch { }
        }

        private void KritikPanelGuncelle(int sayi)
        {
            const string ad = "pnlKritikUyari";
            var eski = tabDash.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == ad);
            if (eski != null) tabDash.Controls.Remove(eski);
            if (sayi <= 0) return;

            var pnl = new Panel
            {
                Name = ad,
                Dock = DockStyle.Bottom,
                Height = 38,
                BackColor = Color.FromArgb(254, 226, 226)
            };
            var lbl = new Label
            {
                Text = $"⚠  {sayi} üründe KRİTİK STOK seviyesi! — Tıklayarak görüntüleyin",
                Dock = DockStyle.Fill,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 28, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lbl.Click += (s, e) =>
            { MenuAktif(btnMenuDusum); SayfaGit(5, "🔻  STOK DÜŞÜM"); StokDurumYukle(); };
            pnl.Controls.Add(lbl);
            tabDash.Controls.Add(pnl);
            pnl.BringToFront();
        }

        // ════════════════════════════════════════════════════════════════
        //  DÖVİZ
        // ════════════════════════════════════════════════════════════════

        private void DovizGetir()
        {
            try
            {
                string usd = "—", eur = "—", gbp = "—";
                string[] urls = {
                    "https://www.tcmb.gov.tr/kurlar/today.xml",
                    $"https://www.tcmb.gov.tr/kurlar/{DateTime.Now:yyyy}{DateTime.Now:MM}/{DateTime.Now:yyyyMMdd}.xml"
                };
                foreach (string url in urls)
                {
                    try
                    {
                        var req = (HttpWebRequest)WebRequest.Create(url);
                        req.Timeout = 7000;
                        req.UserAgent = "Mozilla/5.0";
                        using (var resp = (HttpWebResponse)req.GetResponse())
                        using (var sr = new StreamReader(resp.GetResponseStream()))
                        {
                            var xml = new XmlDocument();
                            xml.LoadXml(sr.ReadToEnd());
                            usd = xml.SelectSingleNode("//Currency[@Kod='USD']/BanknoteSelling")?.InnerText ?? "—";
                            eur = xml.SelectSingleNode("//Currency[@Kod='EUR']/BanknoteSelling")?.InnerText ?? "—";
                            gbp = xml.SelectSingleNode("//Currency[@Kod='GBP']/BanknoteSelling")?.InnerText ?? "—";
                        }
                        if (usd != "—") break;
                    }
                    catch { }
                }
                string m = $"💵 USD: {usd} ₺  |  💶 EUR: {eur} ₺  |  💷 GBP: {gbp} ₺  |  🕐 {DateTime.Now:HH:mm}";
                if (statusStrip1.InvokeRequired)
                    statusStrip1.Invoke(new Action(() => lblKurBilgi.Text = m));
                else
                    lblKurBilgi.Text = m;
            }
            catch { lblKurBilgi.Text = $"⚠ Kur alınamadı  |  🕐 {DateTime.Now:HH:mm}"; }
        }

        // ════════════════════════════════════════════════════════════════
        //  KATEGORİ COMBO
        // ════════════════════════════════════════════════════════════════

        private void KategoriComboDoldur()
        {
            try
            {
                var dt = DB.GetTable("SELECT Id, KategoriAdi FROM Kategoriler ORDER BY KategoriAdi");
                cmbPopKategori.DataSource = null;
                cmbPopKategori.DataSource = dt;
                cmbPopKategori.DisplayMember = "KategoriAdi";
                cmbPopKategori.ValueMember = "Id";

                cmbRaporKategori.Items.Clear();
                cmbRaporKategori.Items.Add("Tüm Kategoriler");
                foreach (DataRow r in dt.Rows)
                    cmbRaporKategori.Items.Add(r["KategoriAdi"].ToString());
                if (cmbRaporKategori.Items.Count > 0) cmbRaporKategori.SelectedIndex = 0;
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════════════
        //  LİSTE YÜKLEME
        // ════════════════════════════════════════════════════════════════

        private void TeklifYukle()
        {
            // Her kalem için grupla — kaç tedarikçiden teklif alındığını göster
            GridYukle(dgvTeklifListesi,
                @"SELECT
                       MIN(TD.Id)                           AS KayitId,
                       TD.TalepNo                           AS _TalepNo,
                       TD.UrunId                            AS _UrunId,
                       U.UrunAdi,
                       MAX(TD.Miktar)                       AS Miktar,
                       U.Birim,
                       ISNULL(MAX(TD.Oncelik),2)            AS Oncelik,
                       T.TalepEden,
                       CONVERT(VARCHAR,T.Tarih,103)         AS TalepTarihi,
                       CASE
                           WHEN SUM(CASE WHEN ISNULL(TD.BirimFiyat,0)>0 AND TD.OnayDurumu=3 THEN 1 ELSE 0 END) > 0
                                THEN CAST(SUM(CASE WHEN ISNULL(TD.BirimFiyat,0)>0 AND TD.OnayDurumu=3 THEN 1 ELSE 0 END) AS NVARCHAR(5))
                                     + ' tedarikçiden teklif alındı, onayda'
                           WHEN SUM(CASE WHEN ISNULL(TD.BirimFiyat,0)>0 THEN 1 ELSE 0 END) > 0
                                THEN CAST(SUM(CASE WHEN ISNULL(TD.BirimFiyat,0)>0 THEN 1 ELSE 0 END) AS NVARCHAR(5))
                                     + ' tedarikçi teklif verdi'
                           ELSE '⏳ Fiyat bekleniyor'
                       END AS Durum
                  FROM TalepDetay TD
                  JOIN Urunler    U ON U.Id      = TD.UrunId
                  JOIN Talepler   T ON T.TalepNo = TD.TalepNo
                  WHERE TD.OnayDurumu IN (0,3)
                  GROUP BY TD.TalepNo, TD.UrunId, U.UrunAdi, U.Birim, T.TalepEden, T.Tarih
                  ORDER BY ISNULL(MAX(TD.Oncelik),2) ASC, T.Tarih DESC");
        }

        private void OnayYukle()
        {
            GridYukle(dgvOnayListesi,
                @"SELECT TD.Id AS KayitId,
                         T.TalepNo,
                         U.UrunAdi,
                         TD.Miktar, U.Birim,
                         ISNULL(TD.BirimFiyat,0)             AS BirimFiyat,
                         TD.Miktar*ISNULL(TD.BirimFiyat,0)   AS ToplamTutar,
                         ISNULL(TD.Tedarikci,'—')            AS Tedarikci,
                         ISNULL(TD.Oncelik,2)                AS Oncelik,
                         ISNULL(TD.OnayNotu,'')              AS OnayNotu,
                         T.TalepEden,
                         CONVERT(VARCHAR,T.Tarih,103)        AS TalepTarihi
                  FROM   TalepDetay TD
                  JOIN   Urunler    U ON U.Id      = TD.UrunId
                  JOIN   Talepler   T ON T.TalepNo = TD.TalepNo
                  WHERE  TD.OnayDurumu = 3
                  ORDER  BY ISNULL(TD.Oncelik,2) ASC, T.Tarih DESC");
        }

        private void GirisYukle()
        {
            DepoGridYukle(
                @"SELECT TD.Id AS KayitId,
                         U.UrunAdi,
                         ISNULL(U.Barkod,'—')               AS Barkod,
                         TD.Miktar, U.Birim,
                         ISNULL(TD.BirimFiyat,0)            AS BirimFiyat,
                         TD.Miktar*ISNULL(TD.BirimFiyat,0)  AS ToplamTutar,
                         ISNULL(TD.Tedarikci,'—')           AS Tedarikci,
                         ISNULL(TD.SiparisNo,'—')           AS SiparisNo,
                         ISNULL(U.RafYeri,'—')              AS RafYeri,
                         T.TalepEden,
                         CONVERT(VARCHAR,T.Tarih,103)       AS TalepTarihi
                  FROM   TalepDetay TD
                  JOIN   Urunler    U ON U.Id      = TD.UrunId
                  JOIN   Talepler   T ON T.TalepNo = TD.TalepNo
                  WHERE  TD.OnayDurumu = 1
                    AND  (TD.IrsaliyeNo IS NULL OR TD.IrsaliyeNo='')
                  ORDER  BY T.Tarih DESC");

            if (_lblDepoSonuc != null)
            {
                _lblDepoSonuc.Text = $"{dgvStokListesi.Rows.Count} bekleyen giriş";
                _lblDepoSonuc.ForeColor = Color.FromArgb(148, 163, 184);
            }
        }

        private void StokDurumYukle()
        {
            GridYukle(dgvStokDurumu,
                @"SELECT U.Id,
                         ISNULL(U.Barkod,'—')               AS Barkod,
                         U.UrunAdi,
                         ISNULL(K.KategoriAdi,'—')          AS Kategori,
                         U.StokMiktari, U.Birim,
                         ISNULL(U.MinStok,5)                AS MinStok,
                         ISNULL(U.RafYeri,'—')              AS RafYeri,
                         ISNULL(U.TercihliTedarikci,'—')    AS Tedarikci,
                         CASE WHEN U.StokMiktari<=5  THEN '🔴 KRİTİK'
                              WHEN U.StokMiktari<=20 THEN '🟡 DÜŞÜK'
                              ELSE '🟢 YETERLİ' END AS StokDurumu
                  FROM   Urunler U
                  LEFT JOIN Kategoriler K ON K.Id=U.KategoriId
                  ORDER  BY U.StokMiktari ASC, U.UrunAdi");
        }

        private void TedarikciYukle()
        {
            GridYukle(dgvTedarikci,
                @"SELECT T.Id, T.FirmaAdi, T.YetkiliKisi, T.Telefon,
                         ISNULL(T.VergiNo,'—') AS VergiNo,
                         ISNULL(T.Adres,'—')   AS Adres,
                         (SELECT COUNT(*) FROM TalepDetay TD
                          WHERE TD.Tedarikci=T.FirmaAdi)   AS ToplamSiparis,
                         (SELECT ISNULL(SUM(TD.Miktar*TD.BirimFiyat),0) FROM TalepDetay TD
                          WHERE TD.Tedarikci=T.FirmaAdi AND TD.OnayDurumu=1) AS ToplamTutar
                  FROM   Tedarikciler T WHERE T.Aktif=1 ORDER BY T.FirmaAdi");
        }

        private void KategoriYukle()
        {
            GridYukle(dgvKategoriler,
                @"SELECT K.Id, K.KategoriAdi, K.RenkKodu,
                         COUNT(U.Id) AS UrunSayisi,
                         ISNULL(SUM(U.StokMiktari),0) AS ToplamStok
                  FROM   Kategoriler K LEFT JOIN Urunler U ON U.KategoriId=K.Id
                  GROUP  BY K.Id,K.KategoriAdi,K.RenkKodu ORDER BY K.KategoriAdi");
        }

        private void LogYukle()
        {
            var sb = new StringBuilder(@"
                SELECT SH.Id,
                       CONVERT(VARCHAR,SH.Tarih,103)+' '+CONVERT(VARCHAR,SH.Tarih,108) AS Tarih,
                       ISNULL(U.UrunAdi,'—') AS UrunAdi, ISNULL(U.Barkod,'—') AS Barkod,
                       SH.Miktar, ISNULL(U.Birim,'—') AS Birim,
                       SH.IslemTuru, ISNULL(SH.KisiAdi,'—') AS KisiAdi, ISNULL(SH.Sebep,'—') AS Sebep
                FROM   StokHareketleri SH LEFT JOIN Urunler U ON U.Id=SH.UrunId
                WHERE  1=1");
            var prm = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(txtLogArama.Text))
            {
                sb.Append(" AND (U.UrunAdi LIKE @ara OR SH.KisiAdi LIKE @ara OR U.Barkod LIKE @ara)");
                prm.Add(P("@ara", "%" + txtLogArama.Text.Trim() + "%"));
            }
            if (cmbIslemTuru.SelectedIndex > 0)
            { sb.Append(" AND SH.IslemTuru=@tur"); prm.Add(P("@tur", cmbIslemTuru.Text)); }

            sb.Append(" AND SH.Tarih BETWEEN @bas AND @bit ORDER BY SH.Tarih DESC");
            prm.Add(P("@bas", dtpBaslangic.Value.Date));
            prm.Add(P("@bit", dtpBitis.Value.Date.AddDays(1).AddSeconds(-1)));
            GridYukle(dgvLogListesi, sb.ToString(), prm.ToArray());
        }

        private void btnLogListele_Click(object sender, EventArgs e) => LogYukle();

        // ════════════════════════════════════════════════════════════════
        //  TALEP
        // ════════════════════════════════════════════════════════════════

        private void btnUrunBul_Click(object sender, EventArgs e) => UrunAra();

        private void txtArama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            TimeSpan s = DateTime.Now - _sonBarkodZaman;
            _sonBarkodZaman = DateTime.Now;
            UrunAra(s.TotalMilliseconds < 200);
        }

        private void UrunAra(bool barkodModu = false)
        {
            string aranan = txtArama.Text.Trim();
            if (string.IsNullOrWhiteSpace(aranan)) return;

            DataTable dt = barkodModu
                ? DB.GetTable("SELECT * FROM Urunler WHERE Barkod=@a", P("@a", aranan))
                : DB.GetTable(
                    "SELECT * FROM Urunler WHERE UrunAdi LIKE @a OR Barkod LIKE @a OR ISNULL(KisaAd,'') LIKE @a ORDER BY UrunAdi",
                    P("@a", "%" + aranan + "%"));

            if (dt.Rows.Count == 0)
            {
                if (MessageBox.Show($"'{aranan}' bulunamadı.\nYeni ürün eklensin mi?",
                    "Bulunamadı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    YeniUrunFormuAc();
                return;
            }
            if (dt.Rows.Count == 1)
            {
                UrunSepeteHazirla(Convert.ToInt32(dt.Rows[0]["Id"]),
                    dt.Rows[0]["UrunAdi"].ToString(), dt.Rows[0]["UrunGorseli"] as byte[]);
                if (barkodModu) System.Media.SystemSounds.Beep.Play();
                return;
            }
            using (var frm = new UrunSecimFormu(dt))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    UrunSepeteHazirla(frm.SecilenId, frm.SecilenAd, frm.SecilenGorsel);
                else if (frm.YeniEkleIstegi)
                    YeniUrunFormuAc();
            }
        }

        private void UrunSepeteHazirla(int id, string ad, byte[] gorsel)
        {
            lblGizliId.Text = id.ToString();
            var dt = DB.GetTable("SELECT StokMiktari,Birim FROM Urunler WHERE Id=@id", P("@id", id));
            int stok = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["StokMiktari"]) : 0;
            string birim = dt.Rows.Count > 0 ? dt.Rows[0]["Birim"].ToString() : "Adet";

            lblUrunBilgi.Text = $"{ad}  (Mevcut: {stok} {birim})";
            lblUrunBilgi.ForeColor = stok <= KRITIK ? _clrKirmizi : stok <= DUSUK ? _clrSari : _clrYesil;

            if (gorsel != null && gorsel.Length > 0)
                try { using (var ms = new MemoryStream(gorsel)) pbSecilenResim.Image = Image.FromStream(ms); }
                catch { pbSecilenResim.Image = null; }
            else pbSecilenResim.Image = null;

            txtArama.Clear();
            numMiktar.Focus();
        }

        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lblGizliId.Text) || lblGizliId.Text == "0")
            { MessageBox.Show("Lütfen önce ürün seçin."); return; }

            int id = int.Parse(lblGizliId.Text);
            decimal mik = numMiktar.Value;

            foreach (DataRow r in _sepet.Rows)
                if (Convert.ToInt32(r["UrunId"]) == id)
                { r["Miktar"] = Convert.ToDecimal(r["Miktar"]) + mik; return; }

            var dt = DB.GetTable("SELECT Birim,Barkod FROM Urunler WHERE Id=@id", P("@id", id));
            string birim = dt.Rows.Count > 0 ? dt.Rows[0]["Birim"].ToString() : "Adet";
            string barkod = dt.Rows.Count > 0 ? dt.Rows[0]["Barkod"].ToString() : "";
            string urunAd = lblUrunBilgi.Text.Split('(')[0].Trim();

            _sepet.Rows.Add(id, barkod, urunAd, mik, birim, "", 2);
            lblGizliId.Text = "";
            lblUrunBilgi.Text = "Ürün seçilmedi...";
            lblUrunBilgi.ForeColor = Color.FromArgb(37, 99, 235);
            pbSecilenResim.Image = null;
            numMiktar.Value = 1;
            txtArama.Focus();
        }

        private void btnTalepGonder_Click(object sender, EventArgs e)
        {
            if (_sepet.Rows.Count == 0) { MessageBox.Show("Sepet boş!"); return; }
            if (string.IsNullOrWhiteSpace(txtTalepEden.Text))
            { MessageBox.Show("Talep eden adı girin."); txtTalepEden.Focus(); return; }

            if (MessageBox.Show($"{_sepet.Rows.Count} kalem talep gönderilecek. Devam?",
                "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string no = "TLP-" + DateTime.Now.ToString("yyMMddHHmmss");
            DB.Execute(
                "INSERT INTO Talepler (TalepNo,TalepEden,AsamaKodu,Durum,Aciklama,Tarih) VALUES (@no,@eden,1,'Talep',@acik,GETDATE())",
                P("@no", no), P("@eden", txtTalepEden.Text.Trim()), P("@acik", txtAciklama.Text.Trim()));

            foreach (DataRow r in _sepet.Rows)
                DB.Execute(
                    "INSERT INTO TalepDetay (TalepNo,UrunId,Miktar,OnayDurumu,Aciklama,Oncelik,Tarih) VALUES (@no,@uid,@mik,0,@acik,@once,GETDATE())",
                    P("@no", no), P("@uid", r["UrunId"]), P("@mik", r["Miktar"]),
                    P("@acik", r["Aciklama"]), P("@once", r["Oncelik"]));

            if (MessageBox.Show($"✔  Talep '{no}' oluşturuldu!\nFiş yazdırılsın mı?",
                "Başarılı", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                TalepFisiYazdir(no, txtTalepEden.Text.Trim());

            _sepet.Rows.Clear();
            lblGizliId.Text = ""; lblUrunBilgi.Text = "Ürün seçilmedi...";
            pbSecilenResim.Image = null;
            txtTalepEden.Clear(); txtAciklama.Clear();
            DashboardGuncelle();
        }

        private void TalepFisiYazdir(string talepNo, string talepEden)
        {
            var pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += (s, ev) =>
            {
                var g = ev.Graphics; int y = 20, w = ev.PageBounds.Width;
                using (var fB = new Font("Segoe UI", 14f, FontStyle.Bold))
                using (var fN = new Font("Segoe UI", 10f))
                using (var fS = new Font("Segoe UI", 10f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.FromArgb(17, 24, 39)))
                using (var gr = new SolidBrush(Color.Gray))
                {
                    g.DrawString("SARF MALZEME TALEP FİŞİ", fB, br, w / 2 - 140, y); y += 32;
                    g.DrawLine(Pens.Black, 20, y, w - 20, y); y += 8;
                    g.DrawString($"Talep No  : {talepNo}", fN, br, 20, y); y += 20;
                    g.DrawString($"Tarih     : {DateTime.Now:dd.MM.yyyy HH:mm}", fN, br, 20, y); y += 20;
                    g.DrawString($"Talep Eden: {talepEden}", fN, br, 20, y); y += 20;
                    g.DrawLine(Pens.Black, 20, y, w - 20, y); y += 8;
                    g.DrawString("Ürün Adı", fS, br, 20, y); g.DrawString("Miktar", fS, br, 320, y);
                    g.DrawString("Birim", fS, br, 420, y); g.DrawString("Öncelik", fS, br, 500, y); y += 22;
                    g.DrawLine(Pens.Gray, 20, y, w - 20, y); y += 6;
                    foreach (DataRow r in _sepet.Rows)
                    {
                        string o = Convert.ToInt32(r["Oncelik"]) == 1 ? "ACİL" : Convert.ToInt32(r["Oncelik"]) == 2 ? "Normal" : "Düşük";
                        g.DrawString(r["UrunAdi"].ToString(), fN, br, 20, y);
                        g.DrawString(r["Miktar"].ToString(), fN, br, 320, y);
                        g.DrawString(r["Birim"].ToString(), fN, br, 420, y);
                        g.DrawString(o, fN, br, 500, y); y += 20;
                    }
                    g.DrawLine(Pens.Black, 20, y + 6, w - 20, y + 6); y += 22;
                    g.DrawString($"Toplam: {_sepet.Rows.Count} kalem", fS, br, 20, y); y += 32;
                    g.DrawString("Hazırlayan: ___________________", fN, gr, 20, y);
                    g.DrawString("Onaylayan:  ___________________", fN, gr, 300, y);
                }
            };
            using (var dlg = new PrintDialog { Document = pd, UseEXDialog = true })
                if (dlg.ShowDialog() == DialogResult.OK) pd.Print();
        }

        // Designer uyumluluk
        private void btnFiyatKaydet_Click(object sender, EventArgs e)
            => BtnTeklifAl_Click(sender, e);
        private void mnuSatirCogalt_Click(object sender, EventArgs e) { }

        // ════════════════════════════════════════════════════════════════
        //  ONAY
        // ════════════════════════════════════════════════════════════════

        private void btnOnayla_Click(object sender, EventArgs e)
        {
            if (dgvOnayListesi.SelectedRows.Count == 0)
            { MessageBox.Show("Onaylanacak satırı seçin."); return; }
            string not = InputBox.Goster("Onay notu (isteğe bağlı):", "Onayla");
            int kayit = 0;
            foreach (DataGridViewRow r in dgvOnayListesi.SelectedRows)
            {
                if (r.Cells["KayitId"].Value == null) continue;
                string sip = "SIP-" + DateTime.Now.ToString("yyMMddHHmmss") + kayit;
                DB.Execute("UPDATE TalepDetay SET OnayDurumu=1,OnayNotu=@not,SiparisNo=@sip WHERE Id=@id",
                    P("@not", not), P("@sip", sip), P("@id", r.Cells["KayitId"].Value));
                kayit++;
            }
            MessageBox.Show($"✔  {kayit} sipariş onaylandı.", "Onaylandı",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnayYukle(); DashboardGuncelle();
        }

        private void btnReddet_Click(object sender, EventArgs e)
        {
            if (dgvOnayListesi.SelectedRows.Count == 0) return;
            string not = InputBox.Goster("Red gerekçesi (zorunlu):", "Reddet");
            if (string.IsNullOrWhiteSpace(not)) { MessageBox.Show("Gerekçe zorunludur."); return; }
            foreach (DataGridViewRow r in dgvOnayListesi.SelectedRows)
            {
                if (r.Cells["KayitId"].Value == null) continue;
                DB.Execute("UPDATE TalepDetay SET OnayDurumu=2,OnayNotu=@not WHERE Id=@id",
                    P("@not", not), P("@id", r.Cells["KayitId"].Value));
            }
            MessageBox.Show("✖  Reddedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnayYukle(); DashboardGuncelle();
        }

        private void btnBeklemeyeAl_Click(object sender, EventArgs e)
        {
            if (dgvOnayListesi.SelectedRows.Count == 0) return;
            foreach (DataGridViewRow r in dgvOnayListesi.SelectedRows)
            {
                if (r.Cells["KayitId"].Value == null) continue;
                DB.Execute("UPDATE TalepDetay SET OnayDurumu=3 WHERE Id=@id",
                    P("@id", r.Cells["KayitId"].Value));
            }
            OnayYukle(); DashboardGuncelle();
        }

        // ════════════════════════════════════════════════════════════════
        //  STOK DÜŞÜM
        // ════════════════════════════════════════════════════════════════

        private void btnCikisBul_Click(object sender, EventArgs e) => StokAra();

        private void txtDusumBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            TimeSpan s = DateTime.Now - _sonBarkodZaman;
            _sonBarkodZaman = DateTime.Now;
            if (s.TotalMilliseconds < 200) BarkodIleUrunSec(txtDusumBarkod.Text.Trim());
            else StokAra();
        }

        private void StokAra()
        {
            if (string.IsNullOrWhiteSpace(txtDusumBarkod.Text)) { StokDurumYukle(); return; }
            GridYukle(dgvStokDurumu,
                @"SELECT U.Id, ISNULL(U.Barkod,'—') AS Barkod, U.UrunAdi,
                         ISNULL(K.KategoriAdi,'—') AS Kategori,
                         U.StokMiktari, U.Birim,
                         ISNULL(U.MinStok,5) AS MinStok, ISNULL(U.RafYeri,'—') AS RafYeri,
                         CASE WHEN U.StokMiktari<=5  THEN '🔴 KRİTİK'
                              WHEN U.StokMiktari<=20 THEN '🟡 DÜŞÜK'
                              ELSE '🟢 YETERLİ' END AS StokDurumu
                  FROM   Urunler U LEFT JOIN Kategoriler K ON K.Id=U.KategoriId
                  WHERE  U.UrunAdi LIKE @ara OR U.Barkod LIKE @ara OR ISNULL(U.KisaAd,'') LIKE @ara
                  ORDER  BY U.StokMiktari ASC",
                P("@ara", "%" + txtDusumBarkod.Text.Trim() + "%"));
            if (dgvStokDurumu.Rows.Count == 1) StokDusumSatirSec(0);
        }

        private void BarkodIleUrunSec(string barkod)
        {
            var dt = DB.GetTable(
                @"SELECT U.Id, ISNULL(U.Barkod,'—') AS Barkod, U.UrunAdi,
                         ISNULL(K.KategoriAdi,'—') AS Kategori,
                         U.StokMiktari, U.Birim,
                         ISNULL(U.MinStok,5) AS MinStok, ISNULL(U.RafYeri,'—') AS RafYeri,
                         CASE WHEN U.StokMiktari<=5  THEN '🔴 KRİTİK'
                              WHEN U.StokMiktari<=20 THEN '🟡 DÜŞÜK'
                              ELSE '🟢 YETERLİ' END AS StokDurumu
                  FROM   Urunler U LEFT JOIN Kategoriler K ON K.Id=U.KategoriId
                  WHERE  U.Barkod=@b", P("@b", barkod));

            if (dt.Rows.Count == 0) { MessageBox.Show($"Barkod bulunamadı: {barkod}"); return; }
            dgvStokDurumu.DataSource = dt;
            foreach (DataGridViewColumn c in dgvStokDurumu.Columns)
                if (c.Name == "KayitId") c.Visible = false;
            dgvStokDurumu.AllowUserToResizeRows = false;
            StokDusumSatirSec(0);
            System.Media.SystemSounds.Beep.Play();
        }

        private void btnTumunuGoster_Click(object sender, EventArgs e)
        { txtDusumBarkod.Text = ""; StokDurumYukle(); }

        private void dgvStokDurumu_CellClick(object sender, DataGridViewCellEventArgs e)
        { if (e.RowIndex >= 0) StokDusumSatirSec(e.RowIndex); }

        private void StokDusumSatirSec(int idx)
        {
            if (idx >= dgvStokDurumu.Rows.Count) return;
            var row = dgvStokDurumu.Rows[idx];
            if (row.Cells["Id"].Value == null) return;

            lblDusumGizliId.Text = row.Cells["Id"].Value.ToString();
            int stok = Convert.ToInt32(row.Cells["StokMiktari"].Value);
            string birim = row.Cells["Birim"]?.Value?.ToString() ?? "";
            string raf = row.Cells["RafYeri"]?.Value?.ToString() ?? "—";

            lblDusumUrun.Text = $"✔  {row.Cells["UrunAdi"].Value}  (Mevcut: {stok} {birim}  |  Raf: {raf})";
            lblDusumUrun.ForeColor = stok <= KRITIK ? _clrKirmizi : stok <= DUSUK ? _clrSari : Color.FromArgb(6, 95, 70);
            numDusumMiktar.Maximum = stok > 0 ? stok : 9999;
            numDusumMiktar.Value = 1;
            numDusumMiktar.Focus();
        }
        private void btnDusumYap_ClickTam(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lblDusumGizliId.Text))
            { MessageBox.Show("Listeden ürün seçin ya da barkod okutun."); return; }
            if (string.IsNullOrWhiteSpace(txtDusumKime.Text))
            { MessageBox.Show("Alan kişi zorunludur."); txtDusumKime.Focus(); return; }
            if (string.IsNullOrWhiteSpace(txtDusumNeden.Text))
            { MessageBox.Show("Kullanım sebebi zorunludur."); txtDusumNeden.Focus(); return; }

            int uid = int.Parse(lblDusumGizliId.Text);
            int mik = (int)numDusumMiktar.Value;
            int mev = I(DB.GetValue("SELECT StokMiktari FROM Urunler WHERE Id=@id", P("@id", uid)));

            if (mev < mik) { MessageBox.Show($"Yetersiz stok! Mevcut: {mev}"); return; }
            if (MessageBox.Show($"{mik} adet düşülecek. Onaylıyor musunuz?",
                "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            DB.Execute("UPDATE Urunler SET StokMiktari=StokMiktari-@mik WHERE Id=@id",
                P("@mik", mik), P("@id", uid));
            DB.Execute(
                "INSERT INTO StokHareketleri (UrunId,Miktar,IslemTuru,KisiAdi,Sebep,Tarih) VALUES (@id,@mik,'Çıkış',@kisi,@seb,GETDATE())",
                P("@id", uid),
                P("@mik", mik),
                P("@kisi", txtDusumKime.Text.Trim()),
                P("@seb", txtDusumNeden.Text.Trim()));

            int yeni = mev - mik;
            string uyari = yeni <= KRITIK ? $"\n\n⚠  Stok kritik seviyeye düştü! ({yeni} adet)" : "";
            MessageBox.Show($"✔  {mik} adet düşüldü.{uyari}", "Başarılı",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            lblDusumGizliId.Text = "";
            lblDusumUrun.Text = "Seçilen Ürün: —";
            txtDusumKime.Clear();
            txtDusumNeden.Clear();
            txtDusumBarkod.Clear();
            txtDusumBarkod.Focus();
            StokDurumYukle();
            DashboardGuncelle();
        }

        // ════════════════════════════════════════════════════════════════
        //  TEDARİKÇİ
        // ════════════════════════════════════════════════════════════════

        private void btnTedEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTedFirma.Text))
            { MessageBox.Show("Firma adı zorunludur."); return; }
            if (I(DB.GetValue("SELECT COUNT(*) FROM Tedarikciler WHERE FirmaAdi=@f AND Aktif=1",
                P("@f", txtTedFirma.Text.Trim()))) > 0)
            { MessageBox.Show("Bu firma zaten kayıtlı!"); return; }

            DB.Execute(
                "INSERT INTO Tedarikciler (FirmaAdi,YetkiliKisi,Telefon,VergiNo,Adres,Aktif) VALUES (@f,@y,@t,@v,@a,1)",
                P("@f", txtTedFirma.Text.Trim()),
                P("@y", txtTedYetkili.Text.Trim()),
                P("@t", txtTedTel.Text.Trim()),
                P("@v", txtTedVergi.Text.Trim()),
                P("@a", txtTedAdres.Text.Trim()));

            txtTedFirma.Clear(); txtTedYetkili.Clear();
            txtTedTel.Clear(); txtTedVergi.Clear(); txtTedAdres.Clear();
            TedarikciYukle();
            MessageBox.Show("✔  Tedarikçi eklendi.", "Başarılı",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnTedSil_Click(object sender, EventArgs e)
        {
            if (dgvTedarikci.SelectedRows.Count == 0) return;
            var row = dgvTedarikci.SelectedRows[0];
            if (row.Cells["Id"].Value == null) return;
            if (MessageBox.Show($"'{row.Cells["FirmaAdi"].Value}' silinsin mi?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            DB.Execute("UPDATE Tedarikciler SET Aktif=0 WHERE Id=@id",
                P("@id", row.Cells["Id"].Value));
            TedarikciYukle();
        }

        // ════════════════════════════════════════════════════════════════
        //  KATEGORİ
        // ════════════════════════════════════════════════════════════════

        private void btnKatEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKatAdi.Text))
            { MessageBox.Show("Kategori adı girin."); return; }
            using (var cd = new ColorDialog())
            {
                if (cd.ShowDialog() != DialogResult.OK) return;
                string renk = $"#{cd.Color.R:X2}{cd.Color.G:X2}{cd.Color.B:X2}";
                DB.Execute("INSERT INTO Kategoriler (KategoriAdi,RenkKodu) VALUES (@ad,@renk)",
                    P("@ad", txtKatAdi.Text.Trim()), P("@renk", renk));
                txtKatAdi.Clear();
                KategoriYukle();
                KategoriComboDoldur();
            }
        }

        private void btnKatSil_Click(object sender, EventArgs e)
        {
            if (dgvKategoriler.SelectedRows.Count == 0) return;
            var row = dgvKategoriler.SelectedRows[0];
            if (row.Cells["Id"].Value == null) return;
            int urunSayisi = I(row.Cells["UrunSayisi"]?.Value ?? 0);
            if (urunSayisi > 0)
            {
                MessageBox.Show($"{urunSayisi} ürün bu kategoride. Önce taşıyın.",
                    "Silinemez", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Kategori silinsin mi?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            DB.Execute("DELETE FROM Kategoriler WHERE Id=@id", P("@id", row.Cells["Id"].Value));
            KategoriYukle();
            KategoriComboDoldur();
        }

        // ════════════════════════════════════════════════════════════════
        //  YENİ ÜRÜN
        // ════════════════════════════════════════════════════════════════

        private void YeniUrunFormuAc()
        {
            using (var frm = new UrunDetayFormu())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                { KategoriComboDoldur(); DashboardGuncelle(); }
            }
        }

        private void btnYeniUrun_Click(object sender, EventArgs e) => YeniUrunFormuAc();

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            { Filter = "Resim|*.jpg;*.jpeg;*.png;*.bmp;*.gif", Title = "Ürün Resmi" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    try { picPopUrun.Image = Image.FromFile(ofd.FileName); }
                    catch (Exception ex) { MessageBox.Show("Resim yüklenemedi: " + ex.Message); }
            }
        }

        private void btnPopKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPopAd.Text))
            { MessageBox.Show("Ürün adı zorunludur."); txtPopAd.Focus(); return; }
            if (cmbPopKategori.SelectedValue == null)
            { MessageBox.Show("Kategori seçin."); return; }

            byte[] img = null;
            if (picPopUrun.Image != null)
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        picPopUrun.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        img = ms.ToArray();
                    }
                }
                catch { }

            int res = DB.Execute(
                "INSERT INTO Urunler (Barkod,UrunAdi,StokMiktari,Birim,UrunGorseli,KategoriId) VALUES (@b,@a,0,@bir,@img,@kat)",
                P("@b", txtPopBarkod.Text.Trim()),
                P("@a", txtPopAd.Text.Trim()),
                P("@bir", cmbPopBirim.Text),
                new SqlParameter("@img", (object)img ?? DBNull.Value),
                P("@kat", cmbPopKategori.SelectedValue));

            if (res > 0)
            {
                MessageBox.Show($"✔  '{txtPopAd.Text}' eklendi.", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                pnlPopup.Visible = false;
                KategoriComboDoldur();
                DashboardGuncelle();
            }
        }

        private void btnPopKapat_Click(object sender, EventArgs e)
            => pnlPopup.Visible = false;

        // ════════════════════════════════════════════════════════════════
        //  RAPOR
        // ════════════════════════════════════════════════════════════════

        private void btnRaporGetir_Click(object sender, EventArgs e)
        {
            var prm = new List<SqlParameter>();
            var filtre = new StringBuilder(" AND T.Tarih BETWEEN @bas AND @bit");
            prm.Add(P("@bas", dtpRaporBas.Value.Date));
            prm.Add(P("@bit", dtpRaporBit.Value.Date.AddDays(1).AddSeconds(-1)));

            if (cmbRaporKategori.SelectedIndex > 0)
            {
                filtre.Append(" AND K.KategoriAdi=@kat");
                prm.Add(P("@kat", cmbRaporKategori.Text));
            }
            if (!string.IsNullOrWhiteSpace(txtRaporUrun.Text))
            {
                filtre.Append(" AND (U.UrunAdi LIKE @urun OR TD.Tedarikci LIKE @urun)");
                prm.Add(P("@urun", "%" + txtRaporUrun.Text.Trim() + "%"));
            }

            string sql = cmbRaporTuru.Text == "Genel Satın Alma Özeti"
                ? $@"SELECT U.UrunAdi, K.KategoriAdi,
                            SUM(TD.Miktar)               AS [Top. Adet],
                            MIN(TD.BirimFiyat)           AS [Min. Fiyat],
                            MAX(TD.BirimFiyat)           AS [Max. Fiyat],
                            AVG(TD.BirimFiyat)           AS [Ort. Fiyat],
                            SUM(TD.Miktar*TD.BirimFiyat) AS [Top. Tutar]
                     FROM   TalepDetay TD
                     JOIN   Talepler T ON T.TalepNo=TD.TalepNo
                     JOIN   Urunler  U ON U.Id=TD.UrunId
                     LEFT JOIN Kategoriler K ON K.Id=U.KategoriId
                     WHERE  TD.OnayDurumu=1 {filtre}
                     GROUP  BY U.UrunAdi,K.KategoriAdi
                     ORDER  BY [Top. Tutar] DESC"
                : $@"SELECT CONVERT(VARCHAR,T.Tarih,103) AS Tarih,
                            U.UrunAdi, K.KategoriAdi,
                            TD.Miktar, U.Birim, TD.BirimFiyat,
                            TD.Miktar*TD.BirimFiyat      AS ToplamTutar,
                            TD.Tedarikci, T.TalepEden,
                            ISNULL(TD.SiparisNo,'—')     AS SiparisNo
                     FROM   TalepDetay TD
                     JOIN   Talepler T ON T.TalepNo=TD.TalepNo
                     JOIN   Urunler  U ON U.Id=TD.UrunId
                     LEFT JOIN Kategoriler K ON K.Id=U.KategoriId
                     WHERE  1=1 {filtre}
                     ORDER  BY T.Tarih DESC";

            GridYukle(dgvRapor, sql, prm.ToArray());

            if (dgvRapor.Rows.Count > 0)
            {
                decimal toplam = 0;
                string tc = dgvRapor.Columns.Contains("Top. Tutar") ? "Top. Tutar" :
                                 dgvRapor.Columns.Contains("ToplamTutar") ? "ToplamTutar" : "";
                if (!string.IsNullOrEmpty(tc))
                    foreach (DataGridViewRow r in dgvRapor.Rows)
                        try { toplam += Convert.ToDecimal(r.Cells[tc].Value); } catch { }

                lblBaslik.Text = $"📈  RAPORLAMA  —  {dgvRapor.Rows.Count} kayıt" +
                                 (toplam > 0 ? $"  |  Toplam: {toplam:N2} ₺" : "");
            }
        }

        private void btnRaporExcel_Click(object sender, EventArgs e)
        {
            if (dgvRapor.Rows.Count == 0)
            { MessageBox.Show("Önce raporu getirin."); return; }
            try
            {
                var sfd = new SaveFileDialog
                {
                    Filter = "CSV Dosyası|*.csv",
                    FileName = "Rapor_" + DateTime.Now.ToString("yyyyMMdd_HHmm")
                };
                if (sfd.ShowDialog() != DialogResult.OK) return;

                var sb = new StringBuilder();
                sb.Append('\uFEFF');
                sb.AppendLine(string.Join(";",
                    dgvRapor.Columns.Cast<DataGridViewColumn>()
                        .Where(c => c.Visible)
                        .Select(c => "\"" + c.HeaderText + "\"")));

                foreach (DataGridViewRow r in dgvRapor.Rows)
                    sb.AppendLine(string.Join(";",
                        dgvRapor.Columns.Cast<DataGridViewColumn>()
                            .Where(c => c.Visible)
                            .Select(c => "\"" + (r.Cells[c.Index].Value?.ToString() ?? "") + "\"")));

                File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8);
                MessageBox.Show("✔  Kaydedildi:\n" + sfd.FileName, "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(sfd.FileName);
            }
            catch (Exception ex) { MessageBox.Show("Excel hatası: " + ex.Message); }
        }

        // ════════════════════════════════════════════════════════════════
        //  YARDIMCI METODLAR
        // ════════════════════════════════════════════════════════════════

        private static SqlParameter P(string name, object value)
            => new SqlParameter(name, value ?? DBNull.Value);

        private static int I(object val)
        {
            if (val == null || val == DBNull.Value) return 0;
            try { return Convert.ToInt32(val); } catch { return 0; }
        }

        private static Label LblYap(string text, Color renk)
            => new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = renk
            };

        private static Button BtnYap(string text, Color renk, Point konum, Size boyut)
        {
            var b = new Button
            {
                Text = text,
                Location = konum,
                Size = boyut,
                FlatStyle = FlatStyle.Flat,
                BackColor = renk,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private static void GriYazi(TextBox tb, string ipucu)
        {
            tb.ForeColor = Color.Gray;
            tb.Text = ipucu;
            tb.GotFocus += (s, e) =>
            {
                if (tb.ForeColor == Color.Gray)
                { tb.Text = ""; tb.ForeColor = Color.White; }
            };
            tb.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                { tb.Text = ipucu; tb.ForeColor = Color.Gray; }
            };
        }
    }
}