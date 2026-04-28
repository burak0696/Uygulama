using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaranYardimci
{
    using System.Collections.Generic;

    public class BulunamadiItem
    {
        public string Profil { get; set; }
        public string Kalite { get; set; }
        public double ToplamAdet { get; set; }
        public double ToplamUzunluk { get; set; }
        public double ToplamAgirlik { get; set; }
        public List<string> Satirlar { get; set; } = new List<string>();
    }

    public class EslestirSonuc
    {
        public string No { get; set; }
        public string Ad { get; set; }
    }
}