using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class UrunSecimFormu : Form
    {
        public int SecilenId { get; private set; }
        public string SecilenAd { get; private set; }
        public byte[] SecilenGorsel { get; private set; }
        public bool YeniEkleIstegi { get; private set; }

        private DataGridView dgv;
        private TextBox txtFiltre;
        private DataTable _dt;

        public UrunSecimFormu(DataTable dt)
        {
            _dt = dt;
            KurulumuYap();   // InitializeComponent() KALDIRILDI
        }

        private void KurulumuYap()
        {
            Text = "Ürün Seçimi";
            Size = new Size(860, 580);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(243, 244, 246);
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            var lblFiltre = new Label
            {
                Text = "Filtrele:",
                Location = new Point(12, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 10f)
            };

            txtFiltre = new TextBox
            {
                Location = new Point(76, 10),
                Width = 420,
                Height = 30,
                Font = new Font("Segoe UI", 11f)
            };
            txtFiltre.TextChanged += (s, e) => Filtrele();

            dgv = new DataGridView
            {
                Location = new Point(12, 50),
                Size = new Size(820, 420),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowTemplate = { Height = 42 }
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 41, 55);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 44;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) Sec(); };
            dgv.DataSource = _dt;
            GizliSutunlar();

            var btnSec = BtnYap("✔  SEÇ",
                Color.FromArgb(16, 185, 129), new Point(556, 490));
            btnSec.Click += (s, e) => Sec();

            var btnYeni = BtnYap("🆕  YENİ ÜRÜN",
                Color.FromArgb(245, 158, 11), new Point(688, 490));
            btnYeni.Click += (s, e) =>
            {
                YeniEkleIstegi = true;
                DialogResult = DialogResult.Cancel;
                Close();
            };

            var btnIptal = BtnYap("✖  İPTAL",
                Color.FromArgb(239, 68, 68), new Point(12, 490));
            btnIptal.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            Controls.AddRange(new Control[] {
                lblFiltre, txtFiltre, dgv, btnSec, btnYeni, btnIptal
            });
        }

        private void GizliSutunlar()
        {
            foreach (DataGridViewColumn c in dgv.Columns)
            {
                if (c.Name == "Id") c.Visible = false;
                if (c.Name == "UrunGorseli" && c is DataGridViewImageColumn ic)
                    ic.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
        }

        private void Filtrele()
        {
            if (_dt == null) return;
            string f = txtFiltre.Text.Trim();
            if (string.IsNullOrEmpty(f))
            {
                dgv.DataSource = _dt;
            }
            else
            {
                var satirlar = _dt.AsEnumerable().Where(r =>
                    r.ItemArray.Any(v =>
                        v?.ToString().IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0));
                dgv.DataSource = satirlar.Any() ? satirlar.CopyToDataTable() : _dt;
            }
            GizliSutunlar();
        }

        private void Sec()
        {
            if (dgv.SelectedRows.Count == 0) return;
            var row = dgv.SelectedRows[0];
            if (row.Cells["Id"].Value == null) return;

            SecilenId = Convert.ToInt32(row.Cells["Id"].Value);
            SecilenAd = row.Cells["UrunAdi"]?.Value?.ToString() ?? "";

            if (dgv.Columns.Contains("UrunGorseli"))
            {
                var v = row.Cells["UrunGorseli"].Value;
                if (v is byte[] b && b.Length > 0)
                    SecilenGorsel = b;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private Button BtnYap(string text, Color renk, Point konum)
        {
            var b = new Button
            {
                Text = text,
                Location = konum,
                Size = new Size(120, 42),
                FlatStyle = FlatStyle.Flat,
                BackColor = renk,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }
    }
}