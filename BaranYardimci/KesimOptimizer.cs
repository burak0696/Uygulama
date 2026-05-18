using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaranYardimci
{
    // ═══════════════════════════════════════════════════════════════════
    //  KESİM OPTİMİZASYON MOTORU
    //  ───────────────────────────────────────────────────────────────────
    //  • Çoklu strateji dener (FFD + BFD + Karışık stok)
    //  • Tüm stok boylarını tarar (6m → 12m)
    //  • Hedef seçilebilir: EnAzKg / EnAzFire / EnAzBarSayisi
    //  • Kullanılabilir fire eşiği destekler (hurda vs. yeniden kullanılabilir)
    //  • kg/m bilgisi ile gerçek maliyet (kg) hesaplar
    // ═══════════════════════════════════════════════════════════════════

    public enum KesimHedefi
    {
        EnAzKg,         // En az toplam satın alınan kg  (genelde en mantıklı)
        EnAzFire,       // En düşük fire yüzdesi
        EnAzBarSayisi   // En az bar adedi  (logistik kolaylığı)
    }

    public class KesimAyarlari
    {
        public double KerfMm = 3.0;                                     // Testere kalınlığı
        public double MinKullanilabilirFireMm = 300;                    // Bu uzunluğun altı hurda
        public double[] StokBoylari = { 6000, 12000 }; public bool KarisikStokIzinli = true;                           // 6m + 12m karışım dene
        public KesimHedefi Hedef = KesimHedefi.EnAzKg;
        public int MaxKarisikKombinasyon = 3;                           // En fazla 3 farklı boy karıştır
    }

    public class KesilecekParca
    {
        public string ParcaNo;
        public double Uzunluk;   // mm
        public int Adet;
        public string DosyaAdi;  // hangi dosyadan geldi (raporlama)
        public string MontajNo;  // hangi montaja ait (raporlama)
    }

    public class Dilim
    {
        public string ParcaNo;
        public double Uzunluk;
        public string DosyaAdi;
        public string MontajNo;
    }

    public class StokBar
    {
        public int BarNo;
        public double StokBoyu;          // mm
        public double KerfMm = 3.0;
        public List<Dilim> Dilimler = new List<Dilim>();

        public double Kullanilan
        {
            get
            {
                if (Dilimler.Count == 0) return 0;
                return Dilimler.Sum(d => d.Uzunluk) + (Dilimler.Count - 1) * KerfMm;
            }
        }
        public double Fire => Math.Max(0, StokBoyu - Kullanilan);
        public double FirePct => StokBoyu > 0 ? Fire / StokBoyu * 100.0 : 0;

        /// <summary>Bu parça bu bara sığar mı? (kerf dahil)</summary>
        public bool Sigar(double parcaUzunluk)
        {
            double ekstraKerf = Dilimler.Count > 0 ? KerfMm : 0;
            return Kullanilan + ekstraKerf + parcaUzunluk <= StokBoyu + 0.001;
        }

        public void Ekle(Dilim d) => Dilimler.Add(d);

        /// <summary>Fire, kullanılabilir mi yoksa hurda mı?</summary>
        public bool FireKullanilabilir(double esikMm) => Fire >= esikMm;
    }

    public class ProfilKesimSonucu
    {
        public string Profil, Kalite;
        public double KgPerMetre;

        // İhtiyaç
        public double GerekenNetMetre;
        public double GerekenNetKg;
        public int ToplamParcaAdedi;

        // Satın alım
        public List<StokBar> SecilenBarlar = new List<StokBar>();
        public double SatinAlimToplamMetre;
        public double SatinAlimToplamKg;

        // Fire
        public double ToplamFireMm;
        public double ToplamFireKg;
        public double FireYuzdesi;
        public double KullanilabilirFireMm;   // Yeniden kullanılabilir uzun fireler
        public double HurdaFireMm;            // Çöp (eşiğin altı)
        public double VerimYuzdesi;           // NetKg / SatinAlimKg * 100

        // Senaryo bilgisi
        public string SecilenStrateji;        // örn. "BFD - 12000mm tek boy"
        public Dictionary<double, int> BarDagilimi = new Dictionary<double, int>();
        public List<SenaryoOzeti> TumSenaryolar = new List<SenaryoOzeti>();

        public string KisaOzet()
        {
            var parts = BarDagilimi.OrderByDescending(kv => kv.Key)
                .Select(kv => $"{kv.Key / 1000.0:0.#}m × {kv.Value} bar");
            return string.Join(" + ", parts);
        }
    }

    public class SenaryoOzeti
    {
        public string Ad;
        public double ToplamKg;
        public double FireYuzdesi;
        public int BarSayisi;
        public double KullanilabilirFireMm;
        public bool Secildi;
    }

    // ═══════════════════════════════════════════════════════════════════

    public static class KesimOptimizer
    {
        /// <summary>Tek profil için tüm stratejileri dener, en iyiyi döner.</summary>
        public static ProfilKesimSonucu Optimize(
            string profil, string kalite,
            List<KesilecekParca> parcalar,
            double kgPerMetre,
            KesimAyarlari ayar)
        {
            if (ayar == null) ayar = new KesimAyarlari();

            // Tek tek dilimlere aç (adet × parça)
            var dilimler = new List<Dilim>();
            foreach (var p in parcalar)
                for (int i = 0; i < p.Adet; i++)
                    dilimler.Add(new Dilim
                    {
                        ParcaNo = p.ParcaNo,
                        Uzunluk = p.Uzunluk,
                        DosyaAdi = p.DosyaAdi,
                        MontajNo = p.MontajNo
                    });

            double netMm = dilimler.Sum(d => d.Uzunluk);
            int parcaAdet = dilimler.Count;

            var sonuc = new ProfilKesimSonucu
            {
                Profil = profil,
                Kalite = kalite,
                KgPerMetre = kgPerMetre,
                GerekenNetMetre = netMm / 1000.0,
                GerekenNetKg = (netMm / 1000.0) * kgPerMetre,
                ToplamParcaAdedi = parcaAdet
            };

            if (parcaAdet == 0) return sonuc;

            // ── Tüm senaryoları üret ─────────────────────────────────────
            var adaylar = new List<(string ad, List<StokBar> barlar)>();

            // 1) Her stok boyu için FFD ve BFD ayrı ayrı
            foreach (double boy in ayar.StokBoylari)
            {
                // Bu boya sığmayan parça varsa atla
                if (dilimler.Any(d => d.Uzunluk > boy)) continue;

                var ffd = FFD(dilimler, boy, ayar.KerfMm);
                adaylar.Add(($"FFD {boy / 1000.0:0.#}m", ffd));

                var bfd = BFD(dilimler, boy, ayar.KerfMm);
                adaylar.Add(($"BFD {boy / 1000.0:0.#}m", bfd));
            }

            // 2) Karışık stok (büyük → küçük doğru sırayla)
            if (ayar.KarisikStokIzinli && ayar.StokBoylari.Length >= 2)
            {
                var buyukten = ayar.StokBoylari
                    .Where(b => dilimler.All(d => d.Uzunluk <= b) ||
                                ayar.StokBoylari.Max() >= dilimler.Max(d => d.Uzunluk))
                    .OrderByDescending(b => b)
                    .Take(ayar.MaxKarisikKombinasyon)
                    .ToArray();

                if (buyukten.Length >= 2)
                {
                    var karisik = KarisikStokBFD(dilimler, buyukten, ayar.KerfMm);
                    if (karisik != null && karisik.Count > 0)
                    {
                        string ad = "Karışık: " + string.Join("+",
                            karisik.GroupBy(b => b.StokBoyu)
                                   .OrderByDescending(g => g.Key)
                                   .Select(g => $"{g.Key / 1000.0:0.#}m×{g.Count()}"));
                        adaylar.Add((ad, karisik));
                    }
                }
            }

            // ── Her aday için skor hesapla ───────────────────────────────
            var skorlu = adaylar
                .Where(a => a.barlar != null && a.barlar.Count > 0)
                .Select(a => new
                {
                    a.ad,
                    a.barlar,
                    toplamMm = a.barlar.Sum(b => b.StokBoyu),
                    toplamKg = a.barlar.Sum(b => b.StokBoyu) / 1000.0 * kgPerMetre,
                    barSayisi = a.barlar.Count,
                    fireMm = a.barlar.Sum(b => b.Fire),
                    firePct = a.barlar.Sum(b => b.Fire) / Math.Max(1, a.barlar.Sum(b => b.StokBoyu)) * 100.0,
                    kullanilabilirFire = a.barlar.Where(b => b.FireKullanilabilir(ayar.MinKullanilabilirFireMm))
                                                  .Sum(b => b.Fire)
                })
                .ToList();

            if (skorlu.Count == 0) return sonuc;

            var enIyiSorgu = skorlu.OrderBy(x => x.toplamKg);
            switch (ayar.Hedef)
            {
                case KesimHedefi.EnAzFire:
                    enIyiSorgu = skorlu.OrderBy(x => x.firePct).ThenBy(x => x.toplamKg);
                    break;
                case KesimHedefi.EnAzBarSayisi:
                    enIyiSorgu = skorlu.OrderBy(x => x.barSayisi).ThenBy(x => x.toplamKg);
                    break;
                case KesimHedefi.EnAzKg:
                default:
                    enIyiSorgu = skorlu.OrderBy(x => x.toplamKg)
                                       .ThenBy(x => x.fireMm - x.kullanilabilirFire);
                    break;
            }
            var enIyi = enIyiSorgu.First();
            // ── Sonuca doldur ────────────────────────────────────────────
            int barNo = 1;
            foreach (var b in enIyi.barlar) { b.BarNo = barNo++; b.KerfMm = ayar.KerfMm; }

            sonuc.SecilenBarlar = enIyi.barlar;
            sonuc.SatinAlimToplamMetre = enIyi.toplamMm / 1000.0;
            sonuc.SatinAlimToplamKg = enIyi.toplamKg;
            sonuc.ToplamFireMm = enIyi.fireMm;
            sonuc.ToplamFireKg = enIyi.fireMm / 1000.0 * kgPerMetre;
            sonuc.FireYuzdesi = enIyi.firePct;
            sonuc.KullanilabilirFireMm = enIyi.kullanilabilirFire;
            sonuc.HurdaFireMm = enIyi.fireMm - enIyi.kullanilabilirFire;
            sonuc.VerimYuzdesi = enIyi.toplamKg > 0 ? sonuc.GerekenNetKg / enIyi.toplamKg * 100.0 : 0;
            sonuc.SecilenStrateji = enIyi.ad;
            sonuc.BarDagilimi = enIyi.barlar.GroupBy(b => b.StokBoyu)
                .ToDictionary(g => g.Key, g => g.Count());

            sonuc.TumSenaryolar = skorlu.Select(x => new SenaryoOzeti
            {
                Ad = x.ad,
                ToplamKg = x.toplamKg,
                FireYuzdesi = x.firePct,
                BarSayisi = x.barSayisi,
                KullanilabilirFireMm = x.kullanilabilirFire,
                Secildi = x.ad == enIyi.ad
            }).OrderBy(s => s.ToplamKg).ToList();

            return sonuc;
        }

        // ── ALGORİTMALAR ─────────────────────────────────────────────────

        /// <summary>First-Fit Decreasing: uzundan kısaya, sığan ilk bara.</summary>
        private static List<StokBar> FFD(List<Dilim> dilimler, double stokBoyu, double kerf)
        {
            var sira = dilimler.OrderByDescending(d => d.Uzunluk).ToList();
            var barlar = new List<StokBar>();
            foreach (var d in sira)
            {
                if (d.Uzunluk > stokBoyu) continue;
                var hedef = barlar.FirstOrDefault(b => b.Sigar(d.Uzunluk));
                if (hedef == null)
                {
                    hedef = new StokBar { StokBoyu = stokBoyu, KerfMm = kerf };
                    barlar.Add(hedef);
                }
                hedef.Ekle(d);
            }
            return barlar;
        }

        /// <summary>Best-Fit Decreasing: uzundan kısaya, en az fire kalan bara.</summary>
        private static List<StokBar> BFD(List<Dilim> dilimler, double stokBoyu, double kerf)
        {
            var sira = dilimler.OrderByDescending(d => d.Uzunluk).ToList();
            var barlar = new List<StokBar>();
            foreach (var d in sira)
            {
                if (d.Uzunluk > stokBoyu) continue;
                StokBar enIyi = null;
                double enAzKalan = double.MaxValue;
                foreach (var b in barlar)
                {
                    if (!b.Sigar(d.Uzunluk)) continue;
                    double kalan = b.StokBoyu - b.Kullanilan - (b.Dilimler.Count > 0 ? kerf : 0) - d.Uzunluk;
                    if (kalan < enAzKalan) { enAzKalan = kalan; enIyi = b; }
                }
                if (enIyi == null)
                {
                    enIyi = new StokBar { StokBoyu = stokBoyu, KerfMm = kerf };
                    barlar.Add(enIyi);
                }
                enIyi.Ekle(d);
            }
            return barlar;
        }

        /// <summary>
        /// Karışık stok BFD: parçaları büyükten küçüğe sırayla yerleştirirken
        /// her parça için TÜM mevcut barlardan en uygununu seç, sığmıyorsa
        /// büyükten küçüğe doğru yeni boyları dene.
        /// </summary>
        private static List<StokBar> KarisikStokBFD(List<Dilim> dilimler, double[] boylar, double kerf)
        {
            var sira = dilimler.OrderByDescending(d => d.Uzunluk).ToList();
            var barlar = new List<StokBar>();
            var boyDesc = boylar.OrderByDescending(b => b).ToArray();

            foreach (var d in sira)
            {
                // 1) Açık barlardan en az fire bırakanı bul (BFD)
                StokBar enIyi = null;
                double enAzKalan = double.MaxValue;
                foreach (var b in barlar)
                {
                    if (!b.Sigar(d.Uzunluk)) continue;
                    double kalan = b.StokBoyu - b.Kullanilan - (b.Dilimler.Count > 0 ? kerf : 0) - d.Uzunluk;
                    if (kalan < enAzKalan) { enAzKalan = kalan; enIyi = b; }
                }

                // 2) Açık barlara sığmıyor → bu parçaya en uygun (en küçük yeterli) yeni boy aç
                if (enIyi == null)
                {
                    double secilenBoy = boyDesc.Where(b => b >= d.Uzunluk).DefaultIfEmpty(boyDesc[0]).Min();
                    // Eğer parça çok kısaysa en küçük boy yerine ekonomik boya bak:
                    // tek parça çok kısaysa küçük bar daha az fire verir.
                    // Heuristic: parça uzunluğunun 2 katı kadar boy yeterli, yoksa orta boy
                    enIyi = new StokBar { StokBoyu = secilenBoy, KerfMm = kerf };
                    barlar.Add(enIyi);
                }
                enIyi.Ekle(d);
            }
            return barlar;
        }
    }
}