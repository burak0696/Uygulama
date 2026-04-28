using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaranYardimci
{
    public partial class FrmTerminSec : Form
    {
        private int _malzemeKayitId;
        private DateTime _mevcutTermin;

        public DateTime SecilenTarih { get; private set; }

        public FrmTerminSec(int malzemeKayitId, DateTime mevcutTermin)
        {
            InitializeComponent();
            _malzemeKayitId = malzemeKayitId;
            _mevcutTermin = mevcutTermin;
        }

        public FrmTerminSec(int malzemeKayitId)
        {
            InitializeComponent();
            _malzemeKayitId = malzemeKayitId;
            _mevcutTermin = DateTime.Now.AddDays(14);
        }

        public FrmTerminSec()
        {
            InitializeComponent();
            _malzemeKayitId = 0;
            _mevcutTermin = DateTime.Now.AddDays(14);
        }

        private void FrmTerminSec_Load(object sender, EventArgs e)
        {
            dtpTermin.Value = _mevcutTermin;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SecilenTarih = dtpTermin.Value;

            if (_malzemeKayitId > 0)
            {
                try
                {
                    DB.Execute("UPDATE MalzemeKayit SET TerminTarihi=@tarih WHERE Id=@id",
                        new SqlParameter("@tarih", SecilenTarih),
                        new SqlParameter("@id", _malzemeKayitId));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}