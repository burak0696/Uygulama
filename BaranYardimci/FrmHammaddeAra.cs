using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BaranYardimci
{
    public class FrmHammaddeAra : Form
    {
        private readonly string _ilkArama;
        private readonly List<HammaddeItem> _tumHammaddeler = new List<HammaddeItem>();
        private TextBox txtAra;
        private DataGridView dgvHammadde;
        private Button btnSec;
        private Button btnIptal;

        public HammaddeItem Secilen { get; private set; }

        public FrmHammaddeAra(string ilkArama = "")
        {
            _ilkArama = ilkArama ?? "";
            KontrolleriOlustur();
            Load += FrmHammaddeAra_Load;
        }

        private void KontrolleriOlustur()
        {
            Text = "Hammadde Ara";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(700, 500);
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.FromArgb(243, 244, 246);

            var pnlUst = new Panel { Dock = DockStyle.Top, Height = 72, Padding = new Padding(12, 12, 12, 8) };
            var lblAra = new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Text = "Hammadde Ara (No / Adı)",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold)
            };
            txtAra = new TextBox { Dock = DockStyle.Top, Height = 32, Font = new Font("Segoe UI", 10f) };
            txtAra.TextChanged += txtAra_TextChanged;
            pnlUst.Controls.Add(txtAra);
            pnlUst.Controls.Add(lblAra);

            dgvHammadde = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            dgvHammadde.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNo", HeaderText = "No", FillWeight = 28f, ReadOnly = true });
            dgvHammadde.Columns.Add(new DataGridViewTextBoxColumn { Name = "colAdi", HeaderText = "Adı", FillWeight = 72f, ReadOnly = true });
            dgvHammadde.CellDoubleClick += dgvHammadde_CellDoubleClick;

            var pnlAlt = new Panel { Dock = DockStyle.Bottom, Height = 58, Padding = new Padding(12, 10, 12, 10) };
            btnIptal = new Button
            {
                Text = "İptal",
                Dock = DockStyle.Right,
                Width = 120,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnIptal.FlatAppearance.BorderSize = 0;
            btnIptal.Click += btnIptal_Click;

            btnSec = new Button
            {
                Text = "Seç",
                Dock = DockStyle.Right,
                Width = 140,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnSec.FlatAppearance.BorderSize = 0;
            btnSec.Click += btnSec_Click;

            pnlAlt.Controls.Add(btnSec);
            pnlAlt.Controls.Add(btnIptal);

            Controls.Add(dgvHammadde);
            Controls.Add(pnlAlt);
            Controls.Add(pnlUst);

            AcceptButton = btnSec;
            CancelButton = btnIptal;
        }

        private void FrmHammaddeAra_Load(object sender, EventArgs e)
        {
            HammaddeleriYukle();
            ListeyiDoldur(_ilkArama);
            if (!string.IsNullOrWhiteSpace(_ilkArama)) txtAra.Text = _ilkArama;
        }

        private void HammaddeleriYukle()
        {
            _tumHammaddeler.Clear();
            try
            {
                using (var conn = new SqlConnection(DB.ConnStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT no, adi FROM hammadde ORDER BY no", conn))
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                            _tumHammaddeler.Add(new HammaddeItem
                            {
                                No = dr["no"].ToString(),
                                Adi = dr["adi"].ToString()
                            });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hammadde listesi yüklenemedi:\n" + ex.Message,
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
            => ListeyiDoldur(txtAra.Text);

        private void ListeyiDoldur(string filtre)
        {
            dgvHammadde.Rows.Clear();
            string q = (filtre ?? "").Trim();
            IEnumerable<HammaddeItem> liste = _tumHammaddeler;
            if (!string.IsNullOrWhiteSpace(q))
                liste = liste.Where(h =>
                    (!string.IsNullOrEmpty(h.No) && h.No.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(h.Adi) && h.Adi.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0));

            foreach (var hm in liste)
            {
                int idx = dgvHammadde.Rows.Add(hm.No, hm.Adi);
                dgvHammadde.Rows[idx].Tag = hm;
            }

            if (dgvHammadde.Rows.Count > 0)
                dgvHammadde.Rows[0].Selected = true;
        }

        private void dgvHammadde_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            SecVeKapat();
        }

        private void btnSec_Click(object sender, EventArgs e)
            => SecVeKapat();

        private void SecVeKapat()
        {
            if (dgvHammadde.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir hammadde seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Secilen = dgvHammadde.SelectedRows[0].Tag as HammaddeItem;
            if (Secilen == null) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
