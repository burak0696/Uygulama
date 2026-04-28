using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaranYardimci
{
    // NOT: static class — partial değil, designer dosyası yok
    public static class InputBox
    {
        public static string Goster(string prompt, string title = "Giriş")
        {
            using (var frm = new Form
            {
                Text = title,
                Size = new Size(420, 170),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                BackColor = Color.White
            })
            {
                var lbl = new Label
                {
                    Text = prompt,
                    Location = new Point(16, 18),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10f)
                };
                var txt = new TextBox
                {
                    Location = new Point(16, 44),
                    Width = 372,
                    Height = 32,
                    Font = new Font("Segoe UI", 11f)
                };
                var btnOk = new Button
                {
                    Text = "✔  Tamam",
                    Location = new Point(186, 88),
                    Size = new Size(100, 38),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(37, 99, 235),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    DialogResult = DialogResult.OK
                };
                btnOk.FlatAppearance.BorderSize = 0;

                var btnIptal = new Button
                {
                    Text = "✖  İptal",
                    Location = new Point(296, 88),
                    Size = new Size(92, 38),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(239, 68, 68),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    DialogResult = DialogResult.Cancel
                };
                btnIptal.FlatAppearance.BorderSize = 0;

                frm.AcceptButton = btnOk;
                frm.CancelButton = btnIptal;
                frm.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnIptal });

                return frm.ShowDialog() == DialogResult.OK
                    ? txt.Text
                    : string.Empty;
            }
        }
    }
}