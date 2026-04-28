using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Drawing2D;
using DC = System.Drawing.Color;
using DBM = System.Drawing.Bitmap;
using DG = System.Drawing.Graphics;
using DPF = System.Drawing.PointF;
using DP = System.Drawing.Pen;
using DSB = System.Drawing.SolidBrush;
using DF = System.Drawing.Font;
using ACadSharp;
using ACadSharp.IO;
using ACadSharp.Entities;

namespace BaranYardimci
{
    public static class DwgJpegHelper
    {
        // ── İç Sınıflar ──────────────────────────────────────────────────────

        public class CadTransform
        {
            public double X { get; set; } = 0;
            public double Y { get; set; } = 0;
            public double ScaleX { get; set; } = 1;
            public double ScaleY { get; set; } = 1;
            public double Rotation { get; set; } = 0;

            public DPF Apply(double x, double y)
            {
                double sx = x * ScaleX;
                double sy = y * ScaleY;
                double cos = Math.Cos(Rotation);
                double sin = Math.Sin(Rotation);
                return new DPF(
                    (float)((sx * cos) - (sy * sin) + X),
                    (float)((sx * sin) + (sy * cos) + Y));
            }

            public static CadTransform Combine(CadTransform parent, CadTransform child)
            {
                DPF p = parent.Apply(child.X, child.Y);
                return new CadTransform
                {
                    X = p.X,
                    Y = p.Y,
                    ScaleX = parent.ScaleX * child.ScaleX,
                    ScaleY = parent.ScaleY * child.ScaleY,
                    Rotation = parent.Rotation + child.Rotation
                };
            }
        }

        public class RenderItem
        {
            public string Type { get; set; }
            public DPF P1 { get; set; }
            public DPF P2 { get; set; }
            public float Radius { get; set; }
            public List<DPF> Points { get; set; }
            public string Text { get; set; }
            public float Size { get; set; }
            public float StartAngle { get; set; }
            public float EndAngle { get; set; }
        }

        // ── Ana Metod ────────────────────────────────────────────────────────

        /// <summary>
        /// DWG veya DXF dosyasını JPEG'e çevirir.
        /// Zaten dönüştürülmüşse tekrar işlem yapmaz.
        /// Çıktı dosyasının tam yolunu döndürür; hata olursa null.
        /// </summary>
        public static string Convert(string inputPath, string outputDir = null)
        {
            if (!File.Exists(inputPath)) return null;

            string dir = outputDir ?? Path.GetDirectoryName(inputPath);
            string outPath = Path.Combine(dir,
                Path.GetFileNameWithoutExtension(inputPath) + ".jpg");

            if (File.Exists(outPath)) return outPath; // zaten var

            CadDocument doc;
            string ext = Path.GetExtension(inputPath).ToLower();
            try
            {
                if (ext == ".dxf")
                    using (var r = new DxfReader(inputPath)) doc = r.Read();
                else
                    using (var r = new DwgReader(inputPath)) doc = r.Read();
            }
            catch { return null; }

            var items = new List<RenderItem>();
            var baseT = new CadTransform();

            foreach (var entity in doc.Entities)
                CollectEntity(entity, items, baseT);

            if (items.Count == 0) return null;

            DrawItems(items, outPath);
            return File.Exists(outPath) ? outPath : null;
        }

        // ── Entity Toplama ───────────────────────────────────────────────────

        private static void CollectEntity(Entity entity,
                                          List<RenderItem> list,
                                          CadTransform t)
        {
            if (entity is Insert insert)
            {
                var bt = new CadTransform
                {
                    X = insert.InsertPoint.X,
                    Y = insert.InsertPoint.Y,
                    ScaleX = insert.XScale,
                    ScaleY = insert.YScale,
                    Rotation = insert.Rotation
                };
                var nt = CadTransform.Combine(t, bt);
                foreach (var sub in insert.Block.Entities)
                    CollectEntity(sub, list, nt);
            }
            else if (entity is ACadSharp.Entities.Line line)
            {
                list.Add(new RenderItem
                {
                    Type = "Line",
                    P1 = t.Apply(line.StartPoint.X, line.StartPoint.Y),
                    P2 = t.Apply(line.EndPoint.X, line.EndPoint.Y)
                });
            }
            else if (entity is ACadSharp.Entities.Circle circle)
            {
                list.Add(new RenderItem
                {
                    Type = "Circle",
                    P1 = t.Apply(circle.Center.X, circle.Center.Y),
                    Radius = (float)(circle.Radius * t.ScaleX)
                });
            }
            else if (entity is LwPolyline poly)
            {
                var pts = new List<DPF>();
                foreach (var v in poly.Vertices)
                    pts.Add(t.Apply(v.Location.X, v.Location.Y));
                if (poly.IsClosed && pts.Count > 0) pts.Add(pts[0]);
                list.Add(new RenderItem { Type = "Polyline", Points = pts });
            }
            else if (entity is ACadSharp.Entities.Arc arc)
            {
                list.Add(new RenderItem
                {
                    Type = "Arc",
                    P1 = t.Apply(arc.Center.X, arc.Center.Y),
                    Radius = (float)(arc.Radius * t.ScaleX),
                    StartAngle = (float)(arc.StartAngle + t.Rotation),
                    EndAngle = (float)(arc.EndAngle + t.Rotation)
                });
            }
            else if (entity is TextEntity text)
            {
                list.Add(new RenderItem
                {
                    Type = "Text",
                    P1 = t.Apply(text.InsertPoint.X, text.InsertPoint.Y),
                    Text = text.Value,
                    Size = (float)(text.Height * t.ScaleX)
                });
            }
            else if (entity is MText mtext)
            {
                list.Add(new RenderItem
                {
                    Type = "Text",
                    P1 = t.Apply(mtext.InsertPoint.X, mtext.InsertPoint.Y),
                    Text = mtext.Value,
                    Size = (float)(mtext.Height * t.ScaleX)
                });
            }
        }

        // ── Çizim ────────────────────────────────────────────────────────────

        private static void DrawItems(List<RenderItem> items, string outputPath)
        {
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            void Check(DPF p)
            {
                if (p.X < minX) minX = p.X;
                if (p.X > maxX) maxX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.Y > maxY) maxY = p.Y;
            }

            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case "Line":
                        Check(item.P1);
                        Check(item.P2);
                        break;
                    case "Circle":
                    case "Arc":
                        Check(new DPF(item.P1.X - item.Radius, item.P1.Y - item.Radius));
                        Check(new DPF(item.P1.X + item.Radius, item.P1.Y + item.Radius));
                        break;
                    case "Polyline":
                        if (item.Points != null)
                            foreach (var p in item.Points) Check(p);
                        break;
                    case "Text":
                        Check(item.P1);
                        break;
                }
            }

            const int cw = 3000;
            const int ch = 2000;
            const int pad = 80;

            float dw = maxX - minX; if (dw <= 0) dw = 1;
            float dh = maxY - minY; if (dh <= 0) dh = 1;

            float scale = Math.Min(
                (cw - pad * 2f) / dw,
                (ch - pad * 2f) / dh);

            float ox = (cw - dw * scale) / 2f;
            float oy = (ch - dh * scale) / 2f;

            DPF ToScreen(DPF p) => new DPF(
                (p.X - minX) * scale + ox,
                ch - ((p.Y - minY) * scale + oy));

            using (var bmp = new DBM(cw, ch))
            using (var g = DG.FromImage(bmp))
            {
                g.Clear(DC.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                using (var pen = new DP(DC.Black, 1.5f))
                using (var brush = new DSB(DC.Black))
                {
                    foreach (var item in items)
                    {
                        try
                        {
                            switch (item.Type)
                            {
                                case "Line":
                                    g.DrawLine(pen, ToScreen(item.P1), ToScreen(item.P2));
                                    break;

                                case "Circle":
                                    {
                                        var c = ToScreen(item.P1);
                                        float r = item.Radius * scale;
                                        g.DrawEllipse(pen, c.X - r, c.Y - r, r * 2, r * 2);
                                        break;
                                    }

                                case "Polyline":
                                    if (item.Points != null && item.Points.Count > 1)
                                        for (int i = 0; i < item.Points.Count - 1; i++)
                                            g.DrawLine(pen,
                                                ToScreen(item.Points[i]),
                                                ToScreen(item.Points[i + 1]));
                                    break;

                                case "Arc":
                                    {
                                        var c = ToScreen(item.P1);
                                        float r = item.Radius * scale;
                                        float s = (float)(item.StartAngle * 180.0 / Math.PI);
                                        float e = (float)(item.EndAngle * 180.0 / Math.PI);
                                        float sw = e - s;
                                        if (sw < 0) sw += 360;
                                        g.DrawArc(pen,
                                            c.X - r, c.Y - r, r * 2, r * 2,
                                            -s, -sw);
                                        break;
                                    }

                                case "Text":
                                    if (!string.IsNullOrEmpty(item.Text))
                                    {
                                        float fs = Math.Max(8, Math.Min(150, item.Size * scale));
                                        var pos = ToScreen(item.P1);
                                        pos = new DPF(pos.X, pos.Y - fs);
                                        using (var font = new DF("Arial", fs))
                                            g.DrawString(item.Text, font, brush, pos);
                                    }
                                    break;
                            }
                        }
                        catch { /* tek item hata verirse devam et */ }
                    }
                }

                bmp.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}