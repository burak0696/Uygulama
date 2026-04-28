using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class GalvanizKontrol : Form
    {
        private DataTable _bitenDetay;

        public GalvanizKontrol()
        {
            InitializeComponent();
        }

        private void GalvanizKontrol_Load(object sender, EventArgs e)
        {
            GalvanizBekleyenleriYukle();
            GalvanizdeOlanlariYukle();
            GalvanizBitenleriYukle();
        }

        private void GalvanizBekleyenleriYukle()
        {
            dgvBekleyen.DataSource = DB.GetTable(@"SELECT Id, MusteriAdi, DosyaAdi, AssemblyNo, PozNo, Profil, Kalite, UretilecekAdet, Uzunluk, ToplamAgirlik, TerminTarihi, DATEDIFF(DAY,GETDATE(),TerminTarihi) AS KalanGun FROM MalzemeKayit WHERE MalzemeDurum='Galvaniz Bekliyor' AND (GalvanizDurum IS NULL OR GalvanizDurum='Bekliyor') ORDER BY TerminTarihi ASC,MusteriAdi,PozNo");
            DgvFormatla(dgvBekleyen);
            SatirRenklendir(dgvBekleyen);
        }

        private void GalvanizdeOlanlariYukle()
        {
            dgvGalvanizde.DataSource = DB.GetTable(@"SELECT Id, MusteriAdi, DosyaAdi, AssemblyNo, PozNo, Profil, Kalite, UretilecekAdet, Uzunluk, ToplamAgirlik, TerminTarihi, DATEDIFF(DAY,GETDATE(),TerminTarihi) AS KalanGun FROM MalzemeKayit WHERE GalvanizDurum='Galvanizde' ORDER BY TerminTarihi ASC,MusteriAdi,PozNo");
            DgvFormatla(dgvGalvanizde);
            SatirRenklendir(dgvGalvanizde);
        }

        private void GalvanizBitenleriYukle()
        {
            dgvBiten.DataSource = DB.GetTable(@"SELECT gk.Id AS KayitId,gk.MusteriAdi,gk.ParcaPozu,gk.MalzemeCinsi,gk.AskiAdeti,gk.AskiSekli,gk.Sicaklik,gk.Sure,gk.VardiyaNo,gk.OrtalamaKalinlik,CASE WHEN gk.ProsesOnay=1 THEN 'Evet' ELSE 'Hayir' END AS ProsesOnayText,CASE WHEN gk.SartliOnay=1 THEN 'Evet' ELSE 'Hayir' END AS SartliOnayText,gk.Tarih,'Kontrol Edildi' AS GecisYontemi,mk.GalvanizDurum FROM GalvanizKontrol gk INNER JOIN MalzemeKayit mk ON mk.Id=gk.MalzemeKayitId WHERE mk.GalvanizDurum='Galvaniz Bitti' UNION ALL SELECT mk.Id AS KayitId,mk.MusteriAdi,mk.PozNo AS ParcaPozu,mk.Profil AS MalzemeCinsi,0 AS AskiAdeti,'-' AS AskiSekli,'-' AS Sicaklik,'-' AS Sure,'-' AS VardiyaNo,0 AS OrtalamaKalinlik,'-' AS ProsesOnayText,'-' AS SartliOnayText,mk.KayitTarihi AS Tarih,'Kontrolsuz Gecis' AS GecisYontemi,mk.GalvanizDurum FROM MalzemeKayit mk WHERE mk.GalvanizDurum='Kontrolsuz Gecis' ORDER BY Tarih DESC");
            DgvFormatla(dgvBiten);
            BitenRenklendir();
        }

        private void SatirRenklendir(DataGridView dgv)
        {
            if (!dgv.Columns.Contains("KalanGun")) return;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["KalanGun"].Value == null || row.Cells["KalanGun"].Value == DBNull.Value)
                { row.DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230); continue; }
                int k = Convert.ToInt32(row.Cells["KalanGun"].Value);
                if (k < 0) { row.DefaultCellStyle.BackColor = Color.FromArgb(255, 150, 150); row.DefaultCellStyle.ForeColor = Color.FromArgb(139, 0, 0); }
                else if (k <= 3) { row.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200); row.DefaultCellStyle.ForeColor = Color.FromArgb(180, 0, 0); }
                else if (k <= 7) { row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 180); row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 100, 0); }
                else if (k <= 14) { row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200); row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 100, 0); }
                else { row.DefaultCellStyle.BackColor = Color.FromArgb(170, 240, 170); row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 80, 0); }
            }
        }

        private void BitenRenklendir()
        {
            if (!dgvBiten.Columns.Contains("GecisYontemi")) return;
            foreach (DataGridViewRow row in dgvBiten.Rows)
            {
                string yon = row.Cells["GecisYontemi"].Value?.ToString() ?? "";
                if (yon == "Kontrolsuz Gecis") { row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 200); row.DefaultCellStyle.ForeColor = Color.FromArgb(180, 80, 0); }
                else { row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200); row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 100, 0); }
            }
        }

        private void btnGalvanizeAl_Click(object sender, EventArgs e)
        {
            int c = 0;
            foreach (DataGridViewRow row in dgvBekleyen.SelectedRows)
            { DB.Execute("UPDATE MalzemeKayit SET GalvanizDurum='Galvanizde' WHERE Id=@id", new SqlParameter("@id", Convert.ToInt32(row.Cells["Id"].Value))); c++; }
            if (c == 0) { MessageBox.Show("Lutfen galvanize alinacak pozlari secin!"); return; }
            MessageBox.Show(c + " poz galvanize alindi.");
            GalvanizBekleyenleriYukle();
            GalvanizdeOlanlariYukle();
        }

        private void btnGalvanizdenCikart_Click(object sender, EventArgs e)
        {
            if (dgvGalvanizde.SelectedRows.Count == 0) { MessageBox.Show("Lutfen pozlari secin!"); return; }
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("MusteriAdi", typeof(string));
            dt.Columns.Add("PozNo", typeof(string));
            dt.Columns.Add("Profil", typeof(string));
            dt.Columns.Add("Kalite", typeof(string));
            dt.Columns.Add("ToplamAgirlik", typeof(double));
            foreach (DataGridViewRow row in dgvGalvanizde.SelectedRows)
            {
                dt.Rows.Add(Convert.ToInt32(row.Cells["Id"].Value), row.Cells["MusteriAdi"].Value?.ToString() ?? "",
                    row.Cells["PozNo"].Value?.ToString() ?? "", row.Cells["Profil"].Value?.ToString() ?? "",
                    row.Cells["Kalite"].Value?.ToString() ?? "", Convert.ToDouble(row.Cells["ToplamAgirlik"].Value ?? 0));
            }
            using (GalvanizKontrolFormu frm = new GalvanizKontrolFormu(dt))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog() == DialogResult.OK) { GalvanizdeOlanlariYukle(); GalvanizBitenleriYukle(); }
            }
        }

        private void btnKontrolsuzGecis_Click(object sender, EventArgs e)
        {
            if (dgvGalvanizde.SelectedRows.Count == 0) { MessageBox.Show("Lutfen pozlari secin!"); return; }
            if (MessageBox.Show(dgvGalvanizde.SelectedRows.Count + " poz kontrolsuz aktarilacak. Emin misiniz?", "Onay", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            int c = 0;
            foreach (DataGridViewRow row in dgvGalvanizde.SelectedRows)
            { DB.Execute("UPDATE MalzemeKayit SET MalzemeDurum='Kontrolsuz Paketleme Bekliyor',GalvanizDurum='Kontrolsuz Gecis' WHERE Id=@id", new SqlParameter("@id", Convert.ToInt32(row.Cells["Id"].Value))); c++; }
            MessageBox.Show(c + " poz aktarildi!");
            GalvanizdeOlanlariYukle();
            GalvanizBitenleriYukle();
        }

        private void btnTumunuSec_Click(object sender, EventArgs e) { dgvBekleyen.SelectAll(); }
        private void btnYenile_Click(object sender, EventArgs e) { GalvanizBekleyenleriYukle(); GalvanizdeOlanlariYukle(); GalvanizBitenleriYukle(); }

        private void txtBekleyenAra_TextChanged(object sender, EventArgs e) { Filtrele(dgvBekleyen, txtBekleyenAra.Text.Trim(), "PozNo"); }
        private void txtGalvanizdeAra_TextChanged(object sender, EventArgs e) { Filtrele(dgvGalvanizde, txtGalvanizdeAra.Text.Trim(), "PozNo"); }
        private void txtBitenAra_TextChanged(object sender, EventArgs e) { Filtrele(dgvBiten, txtBitenAra.Text.Trim(), "ParcaPozu"); }
        private void btnBekleyenTemizle_Click(object sender, EventArgs e) { txtBekleyenAra.Clear(); TerminFiltrele(dgvBekleyen, ""); }
        private void btnGalvanizdeTemizle_Click(object sender, EventArgs e) { txtGalvanizdeAra.Clear(); TerminFiltrele(dgvGalvanizde, ""); }
        private void btnBitenTemizle_Click(object sender, EventArgs e) { txtBitenAra.Clear(); DataTable d = dgvBiten.DataSource as DataTable; if (d != null) d.DefaultView.RowFilter = ""; BitenRenklendir(); }

        private void btnBekleyenGeciken_Click(object sender, EventArgs e) { TerminFiltrele(dgvBekleyen, "KalanGun < 0"); }
        private void btnBekleyenAcil_Click(object sender, EventArgs e) { TerminFiltrele(dgvBekleyen, "KalanGun >= 0 AND KalanGun <= 7"); }
        private void btnBekleyenTumu_Click(object sender, EventArgs e) { TerminFiltrele(dgvBekleyen, ""); txtBekleyenAra.Clear(); }
        private void btnGalvanizdeGeciken_Click(object sender, EventArgs e) { TerminFiltrele(dgvGalvanizde, "KalanGun < 0"); }
        private void btnGalvanizdeAcil_Click(object sender, EventArgs e) { TerminFiltrele(dgvGalvanizde, "KalanGun >= 0 AND KalanGun <= 7"); }
        private void btnGalvanizdeTumu_Click(object sender, EventArgs e) { TerminFiltrele(dgvGalvanizde, ""); txtGalvanizdeAra.Clear(); }

        private void Filtrele(DataGridView dgv, string ara, string sutun)
        {
            DataTable d = dgv.DataSource as DataTable;
            if (d == null) return;
            if (string.IsNullOrEmpty(ara)) d.DefaultView.RowFilter = "";
            else d.DefaultView.RowFilter = sutun + " LIKE '%" + ara.Replace("'", "''") + "%'";
            if (dgv == dgvBekleyen || dgv == dgvGalvanizde) SatirRenklendir(dgv);
            else if (dgv == dgvBiten) BitenRenklendir();
        }

        private void TerminFiltrele(DataGridView dgv, string filtre)
        {
            DataTable d = dgv.DataSource as DataTable;
            if (d == null) return;
            d.DefaultView.RowFilter = filtre;
            SatirRenklendir(dgv);
        }

        private void btnPdfCikti_Click(object sender, EventArgs e)
        {
            if (dgvBiten.CurrentRow == null) { MessageBox.Show("Kayit secin!"); return; }
            if ((dgvBiten.CurrentRow.Cells["GecisYontemi"].Value?.ToString() ?? "") == "Kontrolsuz Gecis") { MessageBox.Show("Kontrolsuz gecis icin form yok."); return; }
            _bitenDetay = DB.GetTable(@"SELECT gk.Id,gk.MalzemeKayitId,gk.MusteriAdi,gk.ParcaPozu,gk.SozlesmeNo,gk.MalzemeCinsi,gk.AskiAdeti,gk.AskiSekli,gk.Sicaklik,gk.Sure,gk.VardiyaNo,gk.Tarih,gk.NumuneMiktari,gk.OlcumBasi,gk.OlcumOrta,gk.OlcumSon,gk.OrtalamaKalinlik,gk.KaliteKontrolci,gk.Onaylayan,gk.ProsesOnay,gk.SartliOnay,mk.PozNo,mk.Profil,mk.Kalite,mk.ToplamAgirlik FROM GalvanizKontrol gk INNER JOIN MalzemeKayit mk ON gk.MalzemeKayitId=mk.Id WHERE gk.Id=@id", new SqlParameter("@id", Convert.ToInt32(dgvBiten.CurrentRow.Cells["KayitId"].Value)));
            if (_bitenDetay == null || _bitenDetay.Rows.Count == 0) { MessageBox.Show("Kayit bulunamadi!"); return; }
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += PdfSayfaCiz;
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd; ppd.Width = 1100; ppd.Height = 700;
            ppd.ShowDialog();
        }

        private void PdfSayfaCiz(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            DataRow r = _bitenDetay.Rows[0];
            float sm = 35, um = 25, w = e.PageBounds.Width - 70;
            Font f1 = new Font("Segoe UI", 15, FontStyle.Bold);
            Font f2 = new Font("Segoe UI", 8, FontStyle.Bold);
            Font f3 = new Font("Segoe UI", 14, FontStyle.Bold);
            Font f4 = new Font("Segoe UI", 10, FontStyle.Bold);
            Font f5 = new Font("Segoe UI", 9, FontStyle.Bold);
            Font f6 = new Font("Segoe UI", 11, FontStyle.Bold);
            Font f7 = new Font("Segoe UI", 7.5f);
            Font f8 = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            Font f9 = new Font("Segoe UI", 8, FontStyle.Bold);
            Font f10 = new Font("Segoe UI", 16, FontStyle.Bold);
            Font f11 = new Font("Segoe UI", 13, FontStyle.Bold);
            Font f12 = new Font("Segoe UI", 7f);
            Pen p1 = new Pen(Color.FromArgb(30, 36, 40), 1.8f);
            Pen p2 = new Pen(Color.FromArgb(30, 36, 40), 1.0f);
            Pen p3 = new Pen(Color.FromArgb(150, 150, 150), 0.6f);
            Color ct = Color.FromArgb(255, 140, 0);
            Color ck = Color.FromArgb(30, 36, 40);
            Brush bt = new SolidBrush(ct);
            Brush bk = new SolidBrush(ck);
            Brush bh = new SolidBrush(Color.FromArgb(230, 232, 235));
            Brush ba = new SolidBrush(Color.FromArgb(245, 245, 245));
            StringFormat sc = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            StringFormat sl = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            float y = um;
            float bH = 65;
            float fW = 170;
            g.FillRectangle(new SolidBrush(ck), sm, y, fW, bH);
            g.DrawRectangle(p1, sm, y, fW, bH);
            g.DrawString("BARAN", f1, Brushes.White, new RectangleF(sm, y + 5, fW, bH / 2), sc);
            g.DrawString("CELIK & GALVANIZ", f2, new SolidBrush(ct), new RectangleF(sm, y + bH / 2 - 2, fW, bH / 2 - 3), sc);
            float ow = w - fW - 160;
            g.FillRectangle(Brushes.White, sm + fW, y, ow, bH);
            g.DrawRectangle(p1, sm + fW, y, ow, bH);
            g.DrawString("GALVANIZED CONTROL FORM", f3, bk, new RectangleF(sm + fW, y + 8, ow, bH / 2), sc);
            g.DrawString("GALVANIZ KONTROL FORMU", f4, bt, new RectangleF(sm + fW, y + bH / 2 - 2, ow, bH / 2 - 5), sc);
            float dw = 160;
            float dx = sm + w - dw;
            g.FillRectangle(ba, dx, y, dw, bH);
            g.DrawRectangle(p1, dx, y, dw, bH);
            g.DrawLine(p3, dx, y + bH / 3, dx + dw, y + bH / 3);
            g.DrawLine(p3, dx, y + bH * 2 / 3, dx + dw, y + bH * 2 / 3);
            g.DrawString("Doc No: FR-KL-05", f12, bk, new RectangleF(dx + 5, y, dw - 10, bH / 3), sl);
            g.DrawString("Rev: 02", f12, bk, new RectangleF(dx + 5, y + bH / 3, dw - 10, bH / 3), sl);
            g.DrawString("Tarih: 01.01.2024", f12, bk, new RectangleF(dx + 5, y + bH * 2 / 3, dw - 10, bH / 3), sl);
            y += bH + 8;
            float sh = 48;
            float cw1 = w * 0.6f;
            float cw2 = w * 0.4f;
            g.FillRectangle(bh, sm, y, cw1, 18);
            g.DrawRectangle(p2, sm, y, cw1, sh);
            g.DrawLine(p3, sm, y + 18, sm + cw1, y + 18);
            g.DrawString("Marking of Part / Parca Pozu", f7, bk, new RectangleF(sm + 8, y, cw1 - 16, 18), sl);
            g.DrawString(Oku(r, "MusteriAdi") + " - " + Oku(r, "ParcaPozu"), f6, bk, new RectangleF(sm + 8, y + 18, cw1 - 16, sh - 18), sl);
            g.FillRectangle(bh, sm + cw1, y, cw2, 18);
            g.DrawRectangle(p2, sm + cw1, y, cw2, sh);
            g.DrawLine(p3, sm + cw1, y + 18, sm + cw1 + cw2, y + 18);
            g.DrawString("DATE / TARIH", f7, bk, new RectangleF(sm + cw1 + 8, y, cw2 - 16, 18), sl);
            string tarih = r["Tarih"] != DBNull.Value ? Convert.ToDateTime(r["Tarih"]).ToString("dd.MM.yyyy HH:mm") : "-";
            g.DrawString(tarih, f6, bt, new RectangleF(sm + cw1 + 8, y + 18, cw2 - 16, sh - 18), sl);
            y += sh + 4;
            float s2 = 55;
            float[] dc = { w * .17f, w * .20f, w * .11f, w * .13f, w * .13f, w * .13f, w * .13f };
            string[] en = { "Contract No", "Type of Material", "Hang Qty", "Type of Hang", "Heat (C)", "Time (min)", "Shift No" };
            string[] tr = { "Sozlesme No", "Malzemenin Cinsi", "Aski Adeti", "Aski Sekli", "Sicaklik", "Sure (dk)", "Vardiya No" };
            string[] vl = { Oku(r, "SozlesmeNo"), Oku(r, "MalzemeCinsi"), Oku(r, "AskiAdeti"), Oku(r, "AskiSekli"), Oku(r, "Sicaklik"), Oku(r, "Sure"), Oku(r, "VardiyaNo") };
            float xp = sm;
            for (int i = 0; i < dc.Length; i++)
            {
                g.FillRectangle(bh, xp, y, dc[i], 28);
                g.DrawRectangle(p2, xp, y, dc[i], s2);
                g.DrawLine(p3, xp, y + 28, xp + dc[i], y + 28);
                g.DrawString(en[i], f8, bk, new RectangleF(xp + 4, y + 1, dc[i] - 8, 14), sc);
                g.DrawString(tr[i], f7, bt, new RectangleF(xp + 4, y + 14, dc[i] - 8, 14), sc);
                g.DrawString(vl[i], f5, bk, new RectangleF(xp + 4, y + 28, dc[i] - 8, s2 - 28), sc);
                xp += dc[i];
            }
            y += s2 + 4;
            float oh = 110;
            float sw2 = w * .58f;
            float sg = w * .42f;
            g.DrawRectangle(p2, sm, y, sw2, oh);
            float nw = sw2 * .16f;
            g.FillRectangle(bh, sm, y, nw, oh);
            g.DrawRectangle(p2, sm, y, nw, oh);
            g.DrawString("SAMPLE QTY\nNUMUNE\nMIKTARI", f8, bk, new RectangleF(sm + 2, y + 5, nw - 4, 50), sc);
            g.DrawString(Oku(r, "NumuneMiktari"), f6, bt, new RectangleF(sm + 2, y + 60, nw - 4, 45), sc);
            float ox = sm + nw;
            float otw = sw2 * .54f;
            float ocw = otw / 3f;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 248, 230)), ox, y, otw, 28);
            g.DrawRectangle(p2, ox, y, otw, 28);
            g.DrawString("THICKNESS OF COATING (um)", f8, bk, new RectangleF(ox + 2, y + 1, otw - 4, 14), sc);
            g.DrawString("KAPLAMA KALINLIGI (um)", f7, bt, new RectangleF(ox + 2, y + 13, otw - 4, 14), sc);
            string[] oen = { "Start Point", "Middle Point", "End Point" };
            string[] otr = { "Malzeme Basi", "Malzeme Ortasi", "Malzeme Sonu" };
            string[] ovl = { Oku(r, "OlcumBasi"), Oku(r, "OlcumOrta"), Oku(r, "OlcumSon") };
            for (int i = 0; i < 3; i++)
            {
                float cx = ox + ocw * i;
                g.FillRectangle(bh, cx, y + 28, ocw, 30);
                g.DrawRectangle(p2, cx, y + 28, ocw, oh - 28);
                g.DrawLine(p3, cx, y + 58, cx + ocw, y + 58);
                g.DrawString(oen[i], f7, bk, new RectangleF(cx + 2, y + 29, ocw - 4, 16), sc);
                g.DrawString(otr[i], f7, bt, new RectangleF(cx + 2, y + 43, ocw - 4, 14), sc);
                g.DrawString(ovl[i] + " um", f5, bk, new RectangleF(cx + 2, y + 60, ocw - 4, oh - 62), sc);
            }
            float orx = ox + otw;
            float orw = sw2 - nw - otw;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 243, 220)), orx, y, orw, oh);
            g.DrawRectangle(p2, orx, y, orw, oh);
            g.DrawString("AVERAGE\nTHICKNESS", f8, bk, new RectangleF(orx + 2, y + 5, orw - 4, 22), sc);
            g.DrawString("ORTALAMA\nKALINLIK", f7, bt, new RectangleF(orx + 2, y + 28, orw - 4, 22), sc);
            g.DrawString(Oku(r, "OrtalamaKalinlik"), f10, bt, new RectangleF(orx + 2, y + 55, orw - 4, 30), sc);
            g.DrawString("um", f8, bk, new RectangleF(orx + 2, y + 85, orw - 4, 20), sc);
            float ix = sm + sw2 + 4;
            float iw = sg - 4;
            g.FillRectangle(ba, ix, y, iw, oh);
            g.DrawRectangle(p2, ix, y, iw, oh);
            g.FillRectangle(new SolidBrush(ck), ix, y, iw, 22);
            g.DrawString("EN ISO 1461 - Referans Degerler", f8, Brushes.White, new RectangleF(ix + 5, y, iw - 10, 22), sc);
            string[,] iso = { { "Steel > 6mm", ">= 85 um" }, { "3mm < Steel <= 6mm", ">= 70 um" }, { "1.5mm < Steel <= 3mm", ">= 55 um" }, { "Steel < 1.5mm", ">= 45 um" }, { "Castings >= 6mm", ">= 80 um" } };
            float iy = y + 24;
            float irh = (oh - 26) / 5f;
            float ic1 = iw * .65f;
            float ic2 = iw * .35f;
            for (int i = 0; i < 5; i++)
            {
                if (i % 2 == 0) g.FillRectangle(Brushes.White, ix, iy, iw, irh);
                g.DrawRectangle(p3, ix, iy, ic1, irh);
                g.DrawRectangle(p3, ix + ic1, iy, ic2, irh);
                g.DrawString(iso[i, 0], f7, bk, new RectangleF(ix + 8, iy, ic1 - 16, irh), sl);
                g.DrawString(iso[i, 1], f9, bt, new RectangleF(ix + ic1 + 4, iy, ic2 - 8, irh), sc);
                iy += irh;
            }
            y += oh + 4;
            float onh = 65;
            float onc = w / 4f;
            g.FillRectangle(bh, sm, y, onc, 28); g.DrawRectangle(p2, sm, y, onc, onh); g.DrawLine(p3, sm, y + 28, sm + onc, y + 28);
            g.DrawString("Final Quality Control", f8, bk, new RectangleF(sm + 5, y + 1, onc - 10, 14), sc);
            g.DrawString("Kalite Kontrol Elemani", f7, bt, new RectangleF(sm + 5, y + 14, onc - 10, 14), sc);
            g.DrawString(Oku(r, "KaliteKontrolci"), f5, bk, new RectangleF(sm + 5, y + 28, onc - 10, onh - 28), sc);
            g.FillRectangle(bh, sm + onc, y, onc, 28); g.DrawRectangle(p2, sm + onc, y, onc, onh); g.DrawLine(p3, sm + onc, y + 28, sm + onc * 2, y + 28);
            g.DrawString("Approved by / Onay", f8, bk, new RectangleF(sm + onc + 5, y + 1, onc - 10, 14), sc);
            g.DrawString("KK ve Lab. Sorumlusu", f7, bt, new RectangleF(sm + onc + 5, y + 14, onc - 10, 14), sc);
            g.DrawString(Oku(r, "Onaylayan"), f5, bk, new RectangleF(sm + onc + 5, y + 28, onc - 10, onh - 28), sc);
            bool po = r["ProsesOnay"] != DBNull.Value && Convert.ToBoolean(r["ProsesOnay"]);
            Color pr = po ? Color.FromArgb(39, 174, 96) : Color.FromArgb(192, 57, 43);
            g.FillRectangle(bh, sm + onc * 2, y, onc, 28); g.DrawRectangle(p2, sm + onc * 2, y, onc, onh); g.DrawLine(p3, sm + onc * 2, y + 28, sm + onc * 3, y + 28);
            g.DrawString("Proses Onay", f8, bk, new RectangleF(sm + onc * 2 + 5, y + 1, onc - 10, 14), sc);
            g.DrawString(po ? "ONAYLANDI" : "RED", f7, new SolidBrush(pr), new RectangleF(sm + onc * 2 + 5, y + 14, onc - 10, 14), sc);
            g.DrawString(po ? "EVET" : "HAYIR", f11, new SolidBrush(pr), new RectangleF(sm + onc * 2 + 5, y + 30, onc - 10, onh - 32), sc);
            bool so = r["SartliOnay"] != DBNull.Value && Convert.ToBoolean(r["SartliOnay"]);
            Color sr = so ? Color.FromArgb(243, 156, 18) : Color.FromArgb(149, 165, 166);
            g.FillRectangle(bh, sm + onc * 3, y, onc, 28); g.DrawRectangle(p2, sm + onc * 3, y, onc, onh); g.DrawLine(p3, sm + onc * 3, y + 28, sm + onc * 4, y + 28);
            g.DrawString("Sartli Onay", f8, bk, new RectangleF(sm + onc * 3 + 5, y + 1, onc - 10, 14), sc);
            g.DrawString(so ? "SARTLI" : "YOK", f7, new SolidBrush(sr), new RectangleF(sm + onc * 3 + 5, y + 14, onc - 10, 14), sc);
            g.DrawString(so ? "EVET" : "HAYIR", f11, new SolidBrush(sr), new RectangleF(sm + onc * 3 + 5, y + 30, onc - 10, onh - 32), sc);
            y += onh + 12;
            g.DrawLine(p3, sm, y, sm + w, y);
            g.DrawString("BARAN ÇELİK & GALVANIZ | Galvaniz Kontrol Formu", f7, new SolidBrush(Color.FromArgb(140, 140, 140)), new RectangleF(sm, y + 3, w, 16), sc);
            e.HasMorePages = false;
        }

        private string Oku(DataRow r, string s)
        {
            if (!r.Table.Columns.Contains(s)) return "-";
            if (r[s] == null || r[s] == DBNull.Value) return "-";
            string v = r[s].ToString().Trim();
            return string.IsNullOrEmpty(v) ? "-" : v;
        }

        private void DgvFormatla(DataGridView dgv)
        {
            if (dgv.Columns.Contains("Id")) dgv.Columns["Id"].Visible = false;
            if (dgv.Columns.Contains("GalvanizDurum")) dgv.Columns["GalvanizDurum"].Visible = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.Resizable = DataGridViewTriState.False;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
    }
}