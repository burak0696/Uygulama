using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class UrunDetayFormu : Form
    {
        public int KaydedilenId { get; private set; }
        public string KaydedilenAd { get; private set; }

        private int? _urunId;
        private bool _duzenlemeModu;

        private TabControl tabMain;
        private TabPage tabGenel, tabStok, tabResim;

        // Genel tab
        private TextBox txtBarkod, txtUrunAdi, txtKisaAd, txtAciklama;
        private ComboBox cmbKategori, cmbBirim;
        private Label lblBarkodUyari;
        private Button btnBarkodUret;

        // Stok tab
        private NumericUpDown numMinStok, numMaxStok, numAlisFiyat, numSatisFiyat;
        private TextBox txtRafYeri, txtTedarikciKod;
        private ComboBox cmbTercihliTedarikci;
        private Label lblMevcutStok;
        private DataGridView dgvStokHareket;

        // Resim tab
        private PictureBox picUrun;
        private Button btnResimSec, btnResimKaldir;
        private Label lblResimBilgi;

        // Alt
        private Button btnKaydet, btnIptal;
        private Panel pnlAlt;

        // ── Constructor ───────────────────────────────────────────────────
        public UrunDetayFormu(int? urunId = null)
        {
            _urunId = urunId;
            _duzenlemeModu = urunId.HasValue;
            InitializeComponent();
            FormOlustur();
            if (_duzenlemeModu) VerileriYukle();
        }

        // ════════════════════════════════════════════════════════════════
        //  FORM OLUŞTUR
        // ════════════════════════════════════════════════════════════════

        private void FormOlustur()
        {
            Text = _duzenlemeModu ? "Ürün Düzenle" : "Yeni Ürün Ekle";
            Size = new Size(800, 660);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(243, 244, 246);
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Font = new Font("Segoe UI", 10f);

            tabMain = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f) };
            tabGenel = new TabPage("📋  Genel Bilgiler") { BackColor = Color.White, Padding = new Padding(12) };
            tabStok = new TabPage("📦  Stok & Fiyat") { BackColor = Color.White, Padding = new Padding(12) };
            tabResim = new TabPage("🖼  Ürün Resmi") { BackColor = Color.White, Padding = new Padding(12) };
            tabMain.Controls.AddRange(new Control[] { tabGenel, tabStok, tabResim });

            pnlAlt = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(249, 250, 251)
            };

            btnKaydet = BtnYap("💾  KAYDET", Color.FromArgb(16, 185, 129),
                new Point(560, 11), new Size(210, 40));
            btnKaydet.Click += BtnKaydet_Click;

            btnIptal = BtnYap("✖  İPTAL", Color.FromArgb(239, 68, 68),
                new Point(340, 11), new Size(210, 40));
            btnIptal.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            pnlAlt.Controls.AddRange(new Control[] { btnKaydet, btnIptal });

            Controls.Add(tabMain);
            Controls.Add(pnlAlt);

            GenelTabOlustur();
            StokTabOlustur();
            ResimTabOlustur();
        }

        // ════════════════════════════════════════════════════════════════
        //  TAB 1 — GENEL
        // ════════════════════════════════════════════════════════════════

        private void GenelTabOlustur()
        {
            int y = 10;

            EkleBaslik(tabGenel, "BARKOD BİLGİSİ", ref y);

            var pnlBarkod = new Panel { Location = new Point(14, y), Size = new Size(720, 36) };
            txtBarkod = new TextBox
            {
                Location = new Point(0, 0),
                Width = 420,
                Height = 32,
                Font = new Font("Consolas", 12f)
            };
            GriYazı(txtBarkod, "Barkod okutun veya girin...");
            txtBarkod.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BarkodKontrol(); };
            txtBarkod.Leave += (s, e) => BarkodKontrol();

            btnBarkodUret = BtnYap("🔄 Otomatik",
                Color.FromArgb(100, 116, 139), new Point(430, 0), new Size(140, 32));
            btnBarkodUret.Click += (s, e) =>
            {
                txtBarkod.Text = "BRN" + DateTime.Now.ToString("yyMMddHHmm");
                txtBarkod.ForeColor = Color.Black;
                lblBarkodUyari.Text = "✔  Otomatik oluşturuldu";
                lblBarkodUyari.ForeColor = Color.FromArgb(16, 185, 129);
            };

            lblBarkodUyari = new Label
            {
                Location = new Point(580, 7),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.Gray
            };

            pnlBarkod.Controls.AddRange(new Control[] { txtBarkod, btnBarkodUret, lblBarkodUyari });
            tabGenel.Controls.Add(pnlBarkod);
            y += 52;

            EkleBaslik(tabGenel, "ÜRÜN TANIMI", ref y);

            EkleLabel(tabGenel, "Ürün Adı *:", 14, y);
            txtUrunAdi = new TextBox
            {
                Location = new Point(140, y - 2),
                Width = 580,
                Font = new Font("Segoe UI", 11f)
            };
            tabGenel.Controls.Add(txtUrunAdi);
            y += 36;

            EkleLabel(tabGenel, "Kısa Ad:", 14, y);
            txtKisaAd = new TextBox { Location = new Point(140, y - 2), Width = 300 };
            GriYazı(txtKisaAd, "Listede görünecek kısa ad...");
            tabGenel.Controls.Add(txtKisaAd);
            y += 36;

            EkleLabel(tabGenel, "Kategori *:", 14, y);
            cmbKategori = new ComboBox
            {
                Location = new Point(140, y - 2),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            KategorileriDoldur();
            tabGenel.Controls.Add(cmbKategori);
            y += 36;

            EkleLabel(tabGenel, "Birim *:", 14, y);
            cmbBirim = new ComboBox
            {
                Location = new Point(140, y - 2),
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbBirim.Items.AddRange(new object[] {
                "Adet","Kg","Lt","Mt","Kutu","Paket","Rulo","Çift","Takım","Set","M²","M³","Ton"
            });
            cmbBirim.SelectedIndex = 0;
            tabGenel.Controls.Add(cmbBirim);
            y += 46;

            EkleBaslik(tabGenel, "AÇIKLAMA / NOTLAR", ref y);

            txtAciklama = new TextBox
            {
                Location = new Point(14, y),
                Size = new Size(716, 110),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10f)
            };
            GriYazı(txtAciklama, "Ürün özellikleri, kullanım alanı, teknik bilgiler, notlar...");
            tabGenel.Controls.Add(txtAciklama);
        }

        // ════════════════════════════════════════════════════════════════
        //  TAB 2 — STOK
        // ════════════════════════════════════════════════════════════════

        private void StokTabOlustur()
        {
            int y = 10;

            EkleBaslik(tabStok, "STOK SEVİYELERİ", ref y);

            EkleLabel(tabStok, "Min. Stok:", 14, y);
            numMinStok = new NumericUpDown
            {
                Location = new Point(140, y - 2),
                Width = 120,
                Minimum = 0,
                Maximum = 999999,
                Value = 5,
                Font = new Font("Segoe UI", 11f)
            };
            tabStok.Controls.Add(numMinStok);
            EkleLabel(tabStok, "← Bu seviyenin altında uyarı verir", 272, y + 3,
                Color.FromArgb(100, 116, 139), 9f);
            y += 36;

            EkleLabel(tabStok, "Max. Stok:", 14, y);
            numMaxStok = new NumericUpDown
            {
                Location = new Point(140, y - 2),
                Width = 120,
                Minimum = 0,
                Maximum = 999999,
                Value = 100,
                Font = new Font("Segoe UI", 11f)
            };
            tabStok.Controls.Add(numMaxStok);
            EkleLabel(tabStok, "← Maksimum depo kapasitesi", 272, y + 3,
                Color.FromArgb(100, 116, 139), 9f);
            y += 36;

            if (_duzenlemeModu)
            {
                lblMevcutStok = new Label
                {
                    Location = new Point(14, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold),
                    ForeColor = Color.FromArgb(37, 99, 235)
                };
                tabStok.Controls.Add(lblMevcutStok);
                y += 36;
            }
            y += 10;

            EkleBaslik(tabStok, "FİYAT BİLGİLERİ", ref y);

            EkleLabel(tabStok, "Alış Fiyatı:", 14, y);
            numAlisFiyat = new NumericUpDown
            {
                Location = new Point(140, y - 2),
                Width = 160,
                Minimum = 0,
                Maximum = 9999999,
                DecimalPlaces = 2,
                Value = 0,
                Font = new Font("Segoe UI", 11f)
            };
            tabStok.Controls.Add(numAlisFiyat);
            EkleLabel(tabStok, "₺", 308, y + 3);
            y += 36;

            EkleLabel(tabStok, "Satış Fiyatı:", 14, y);
            numSatisFiyat = new NumericUpDown
            {
                Location = new Point(140, y - 2),
                Width = 160,
                Minimum = 0,
                Maximum = 9999999,
                DecimalPlaces = 2,
                Value = 0,
                Font = new Font("Segoe UI", 11f)
            };
            tabStok.Controls.Add(numSatisFiyat);
            EkleLabel(tabStok, "₺", 308, y + 3);
            y += 46;

            EkleBaslik(tabStok, "DEPO / TEDARİKÇİ BİLGİLERİ", ref y);

            EkleLabel(tabStok, "Raf / Konum:", 14, y);
            txtRafYeri = new TextBox { Location = new Point(140, y - 2), Width = 200 };
            GriYazı(txtRafYeri, "Örn: A-01-03");
            tabStok.Controls.Add(txtRafYeri);
            y += 36;

            EkleLabel(tabStok, "Tedarikçi:", 14, y);
            cmbTercihliTedarikci = new ComboBox
            {
                Location = new Point(140, y - 2),
                Width = 320,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            TedarikciDoldur();
            tabStok.Controls.Add(cmbTercihliTedarikci);
            y += 36;

            EkleLabel(tabStok, "Tedarikçi Kodu:", 14, y);
            txtTedarikciKod = new TextBox { Location = new Point(140, y - 2), Width = 220 };
            GriYazı(txtTedarikciKod, "Tedarikçinin ürün kodu...");
            tabStok.Controls.Add(txtTedarikciKod);
            y += 46;

            if (_duzenlemeModu)
            {
                EkleBaslik(tabStok, "SON STOK HAREKETLERİ", ref y);
                dgvStokHareket = new DataGridView
                {
                    Location = new Point(14, y),
                    Size = new Size(716, 150),
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    RowHeadersVisible = false,
                    EnableHeadersVisualStyles = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    RowTemplate = { Height = 30 }
                };
                dgvStokHareket.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 41, 55);
                dgvStokHareket.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvStokHareket.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
                dgvStokHareket.ColumnHeadersHeight = 34;
                dgvStokHareket.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
                dgvStokHareket.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvStokHareket.CellFormatting += (s, ev) =>
                {
                    if (ev.RowIndex < 0) return;
                    if (dgvStokHareket.Columns[ev.ColumnIndex].Name == "IslemTuru" && ev.Value != null)
                    {
                        ev.CellStyle.ForeColor = ev.Value.ToString() == "Giriş"
                            ? Color.FromArgb(16, 185, 129)
                            : Color.FromArgb(239, 68, 68);
                        ev.CellStyle.Font = new Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
                    }
                };
                tabStok.Controls.Add(dgvStokHareket);
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  TAB 3 — RESİM
        // ════════════════════════════════════════════════════════════════

        private void ResimTabOlustur()
        {
            var lblIpucu = new Label
            {
                Location = new Point(14, 10),
                AutoSize = false,
                Width = 720,
                Height = 22,
                Font = new Font("Segoe UI", 9f, System.Drawing.FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Text = "💡  Net beyaz arka planlı, kare format (400×400 px) resimler önerilir.  PNG / JPG / BMP — Max 2 MB",
                TextAlign = ContentAlignment.MiddleCenter
            };
            tabResim.Controls.Add(lblIpucu);

            picUrun = new PictureBox
            {
                Location = new Point(200, 42),
                Size = new Size(340, 340),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            tabResim.Controls.Add(picUrun);

            btnResimSec = BtnYap("📷  Resim Seç",
                Color.FromArgb(59, 130, 246), new Point(200, 394), new Size(165, 42));
            btnResimSec.Click += (s, e) =>
            {
                using (var ofd = new OpenFileDialog
                { Filter = "Resim|*.jpg;*.jpeg;*.png;*.bmp;*.gif", Title = "Ürün Resmi" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        try
                        {
                            picUrun.Image = Image.FromFile(ofd.FileName);
                            var fi = new System.IO.FileInfo(ofd.FileName);
                            lblResimBilgi.Text = $"{fi.Name}  ({fi.Length / 1024} KB)";
                        }
                        catch (Exception ex) { MessageBox.Show("Resim yüklenemedi: " + ex.Message); }
                }
            };

            btnResimKaldir = BtnYap("🗑  Kaldır",
                Color.FromArgb(239, 68, 68), new Point(375, 394), new Size(165, 42));
            btnResimKaldir.Click += (s, e) =>
            {
                picUrun.Image = null;
                lblResimBilgi.Text = "Resim yok";
            };

            lblResimBilgi = new Label
            {
                Location = new Point(100, 448),
                AutoSize = false,
                Width = 540,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9f),
                Text = "Henüz resim seçilmedi"
            };

            tabResim.Controls.AddRange(new Control[] {
                lblIpucu, picUrun, btnResimSec, btnResimKaldir, lblResimBilgi
            });
        }

        // ════════════════════════════════════════════════════════════════
        //  VERİLERİ YÜKLE
        // ════════════════════════════════════════════════════════════════

        private void VerileriYukle()
        {
            if (!_urunId.HasValue) return;
            try
            {
                var dt = DB.GetTable("SELECT * FROM Urunler WHERE Id=@id",
                    new SqlParameter("@id", _urunId.Value));
                if (dt.Rows.Count == 0) return;
                var r = dt.Rows[0];

                SetText(txtBarkod, r["Barkod"]);
                SetText(txtUrunAdi, r["UrunAdi"]);
                SetText(txtKisaAd, r["KisaAd"]);
                SetText(txtRafYeri, r["RafYeri"]);
                SetText(txtTedarikciKod, r["TedarikciKod"]);
                SetAciklama(r["Aciklama"]);

                if (r["KategoriId"] != DBNull.Value)
                    cmbKategori.SelectedValue = r["KategoriId"];
                if (r["Birim"] != null)
                    cmbBirim.SelectedItem = r["Birim"].ToString();
                if (r["TercihliTedarikci"] != DBNull.Value)
                    cmbTercihliTedarikci.SelectedItem = r["TercihliTedarikci"].ToString();

                SetNum(numMinStok, r["MinStok"]);
                SetNum(numMaxStok, r["MaxStok"]);
                SetNum(numAlisFiyat, r["AlisFiyat"]);
                SetNum(numSatisFiyat, r["SatisFiyat"]);

                int stok = r["StokMiktari"] != DBNull.Value ? Convert.ToInt32(r["StokMiktari"]) : 0;
                if (lblMevcutStok != null)
                {
                    lblMevcutStok.Text = $"📦  Mevcut Stok: {stok} {r["Birim"]}";
                    lblMevcutStok.ForeColor = stok <= 5 ? Color.Red :
                                              stok <= 20 ? Color.Orange :
                                              Color.FromArgb(16, 185, 129);
                }

                if (r["UrunGorseli"] != DBNull.Value && r["UrunGorseli"] is byte[] bytes && bytes.Length > 0)
                    try
                    {
                        using (var ms = new MemoryStream(bytes))
                            picUrun.Image = Image.FromStream(ms);
                        lblResimBilgi.Text = "Kayıtlı resim";
                    }
                    catch { }

                // Hareket geçmişi
                if (dgvStokHareket != null)
                {
                    dgvStokHareket.DataSource = DB.GetTable(
                        @"SELECT TOP 20
                                 IslemTuru, Miktar,
                                 ISNULL(KisiAdi,'—') AS KisiAdi,
                                 ISNULL(Sebep,'—')   AS Sebep,
                                 CONVERT(VARCHAR,Tarih,103)+' '+CONVERT(VARCHAR,Tarih,108) AS Tarih
                          FROM   StokHareketleri
                          WHERE  UrunId=@id
                          ORDER  BY Tarih DESC",
                        new SqlParameter("@id", _urunId.Value));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yükleme hatası: " + ex.Message);
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  BARKOD KONTROL
        // ════════════════════════════════════════════════════════════════

        private void BarkodKontrol()
        {
            string barkod = txtBarkod.Text.Trim();
            if (string.IsNullOrEmpty(barkod) || txtBarkod.ForeColor == Color.Gray) return;

            int mevcut = Convert.ToInt32(DB.GetValue(
                "SELECT COUNT(*) FROM Urunler WHERE Barkod=@b AND (@id IS NULL OR Id<>@id)",
                new SqlParameter("@b", barkod),
                new SqlParameter("@id", (object)_urunId ?? DBNull.Value)) ?? 0);

            if (mevcut > 0)
            {
                lblBarkodUyari.Text = "⚠  Bu barkod zaten kayıtlı!";
                lblBarkodUyari.ForeColor = Color.Red;
            }
            else
            {
                lblBarkodUyari.Text = "✔  Kullanılabilir";
                lblBarkodUyari.ForeColor = Color.FromArgb(16, 185, 129);
            }
        }

        // ════════════════════════════════════════════════════════════════
        //  KAYDET
        // ════════════════════════════════════════════════════════════════

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrunAdi.Text) || txtUrunAdi.Text == "")
            { MessageBox.Show("Ürün adı zorunludur."); txtUrunAdi.Focus(); return; }
            if (cmbKategori.SelectedValue == null)
            { MessageBox.Show("Kategori seçin."); cmbKategori.Focus(); return; }
            if (lblBarkodUyari.ForeColor == Color.Red)
            { MessageBox.Show("Barkod başka ürüne ait!"); txtBarkod.Focus(); return; }

            byte[] img = null;
            if (picUrun.Image != null)
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        picUrun.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        img = ms.ToArray();
                    }
                }
                catch { }

            string barkod = MetinAl(txtBarkod);
            string urunAdi = txtUrunAdi.Text.Trim();
            string kisaAd = MetinAl(txtKisaAd);
            string aciklama = MetinAl(txtAciklama);
            string rafYeri = MetinAl(txtRafYeri);
            string tedKod = MetinAl(txtTedarikciKod);
            string ted = cmbTercihliTedarikci.SelectedIndex > 0
                ? cmbTercihliTedarikci.SelectedItem.ToString() : "";

            int res;
            if (_duzenlemeModu)
            {
                res = DB.Execute(
                    @"UPDATE Urunler SET
                        Barkod=@b, UrunAdi=@a, KisaAd=@ka, Aciklama=@acik,
                        KategoriId=@kat, Birim=@bir,
                        MinStok=@min, MaxStok=@max,
                        AlisFiyat=@alis, SatisFiyat=@satis,
                        RafYeri=@raf, TercihliTedarikci=@ted, TedarikciKod=@tedk,
                        UrunGorseli=@img
                      WHERE Id=@id",
                    P("@b", barkod),
                    P("@a", urunAdi),
                    P("@ka", kisaAd),
                    P("@acik", aciklama),
                    P("@kat", cmbKategori.SelectedValue),
                    P("@bir", cmbBirim.Text),
                    P("@min", numMinStok.Value),
                    P("@max", numMaxStok.Value),
                    P("@alis", numAlisFiyat.Value),
                    P("@satis", numSatisFiyat.Value),
                    P("@raf", rafYeri),
                    P("@ted", ted),
                    P("@tedk", tedKod),
                    new SqlParameter("@img", (object)img ?? DBNull.Value),
                    P("@id", _urunId.Value));
                KaydedilenId = _urunId.Value;
            }
            else
            {
                res = DB.Execute(
                    @"INSERT INTO Urunler
                        (Barkod,UrunAdi,KisaAd,Aciklama,KategoriId,Birim,
                         StokMiktari,MinStok,MaxStok,AlisFiyat,SatisFiyat,
                         RafYeri,TercihliTedarikci,TedarikciKod,UrunGorseli)
                      VALUES
                        (@b,@a,@ka,@acik,@kat,@bir,
                         0,@min,@max,@alis,@satis,
                         @raf,@ted,@tedk,@img)",
                    P("@b", barkod),
                    P("@a", urunAdi),
                    P("@ka", kisaAd),
                    P("@acik", aciklama),
                    P("@kat", cmbKategori.SelectedValue),
                    P("@bir", cmbBirim.Text),
                    P("@min", numMinStok.Value),
                    P("@max", numMaxStok.Value),
                    P("@alis", numAlisFiyat.Value),
                    P("@satis", numSatisFiyat.Value),
                    P("@raf", rafYeri),
                    P("@ted", ted),
                    P("@tedk", tedKod),
                    new SqlParameter("@img", (object)img ?? DBNull.Value));

                if (res > 0)
                {
                    var newId = DB.GetValue("SELECT SCOPE_IDENTITY()");
                    if (newId != null) KaydedilenId = Convert.ToInt32(newId);
                }
            }

            if (res > 0)
            {
                KaydedilenAd = urunAdi;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
                MessageBox.Show("Kayıt sırasında hata oluştu!", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ════════════════════════════════════════════════════════════════
        //  YARDIMCILAR
        // ════════════════════════════════════════════════════════════════

        private void KategorileriDoldur()
        {
            try
            {
                var dt = DB.GetTable("SELECT Id, KategoriAdi FROM Kategoriler ORDER BY KategoriAdi");
                cmbKategori.DataSource = dt;
                cmbKategori.DisplayMember = "KategoriAdi";
                cmbKategori.ValueMember = "Id";
            }
            catch { }
        }

        private void TedarikciDoldur()
        {
            try
            {
                cmbTercihliTedarikci.Items.Clear();
                cmbTercihliTedarikci.Items.Add("— Seçiniz —");
                var dt = DB.GetTable(
                    "SELECT FirmaAdi FROM Tedarikciler WHERE Aktif=1 ORDER BY FirmaAdi");
                foreach (DataRow r in dt.Rows)
                    cmbTercihliTedarikci.Items.Add(r["FirmaAdi"].ToString());
                cmbTercihliTedarikci.SelectedIndex = 0;
            }
            catch { }
        }

        // Placeholder efekti (.NET Framework desteklemediği için)
        private void GriYazı(TextBox tb, string ipucu)
        {
            tb.ForeColor = Color.Gray;
            tb.Text = ipucu;
            tb.GotFocus += (s, e) =>
            {
                if (tb.ForeColor == Color.Gray)
                { tb.Text = ""; tb.ForeColor = Color.Black; }
            };
            tb.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                { tb.Text = ipucu; tb.ForeColor = Color.Gray; }
            };
        }

        private string MetinAl(TextBox tb)
            => tb.ForeColor == Color.Gray ? "" : tb.Text.Trim();

        private void SetText(TextBox tb, object val)
        {
            if (val == null || val == DBNull.Value) return;
            tb.Text = val.ToString();
            tb.ForeColor = Color.Black;
        }

        private void SetAciklama(object val)
        {
            if (val == null || val == DBNull.Value) return;
            txtAciklama.Text = val.ToString();
            txtAciklama.ForeColor = Color.Black;
        }

        private void SetNum(NumericUpDown num, object val)
        {
            if (val == null || val == DBNull.Value) return;
            try { num.Value = Convert.ToDecimal(val); } catch { }
        }

        private Button BtnYap(string text, Color renk, Point konum, Size boyut)
        {
            var b = new Button
            {
                Text = text,
                Location = konum,
                Size = boyut,
                FlatStyle = FlatStyle.Flat,
                BackColor = renk,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void EkleBaslik(Control parent, string text, ref int y)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Location = new Point(14, y),
                AutoSize = false,
                Width = 720,
                Height = 22,
                Font = new Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 116, 139),
                BackColor = Color.FromArgb(241, 245, 249)
            });
            y += 28;
        }

        private void EkleLabel(Control parent, string text, int x, int y,
            Color? renk = null, float size = 10f)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", size),
                ForeColor = renk ?? Color.FromArgb(55, 65, 81)
            });
        }

        private SqlParameter P(string name, object value)
            => new SqlParameter(name, value ?? DBNull.Value);
    }
}