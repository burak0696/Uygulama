using System;
using System.IO;
using System.Text;

namespace BaranYardimci
{
    public static class LogHelper
    {
        public static void Yaz(string excelYolu, string islem, string detay)
        {
            try
            {
                string dosyaAdi = Path.GetFileNameWithoutExtension(excelYolu ?? "genel") + "_degisiklik.log";
                string logYol = null;

                if (!string.IsNullOrEmpty(excelYolu))
                {
                    string klasor = Path.GetDirectoryName(excelYolu);
                    if (!string.IsNullOrEmpty(klasor) && Directory.Exists(klasor))
                    {
                        try
                        {
                            string deneme = Path.Combine(klasor, dosyaAdi);
                            File.AppendAllText(deneme, "", Encoding.UTF8);
                            logYol = deneme;
                        }
                        catch { }
                    }
                }

                if (logYol == null)
                {
                    string fallback = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "BaranYardimci", "logs");
                    Directory.CreateDirectory(fallback);
                    logYol = Path.Combine(fallback, dosyaAdi);
                }

                string satir = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]  [{Environment.UserName}]  [{islem}]  {detay}";
                File.AppendAllText(logYol, satir + Environment.NewLine, Encoding.UTF8);
            }
            catch { }
        }

        public static string LogYoluGetir(string excelYolu)
        {
            if (string.IsNullOrEmpty(excelYolu)) return null;
            string dosyaAdi = Path.GetFileNameWithoutExtension(excelYolu) + "_degisiklik.log";

            string klasor = Path.GetDirectoryName(excelYolu);
            if (!string.IsNullOrEmpty(klasor))
            {
                string yol = Path.Combine(klasor, dosyaAdi);
                if (File.Exists(yol)) return yol;
            }

            string fallback = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "BaranYardimci", "logs", dosyaAdi);
            return File.Exists(fallback) ? fallback : null;
        }
    }
}