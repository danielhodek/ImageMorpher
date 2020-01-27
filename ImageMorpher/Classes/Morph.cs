using System;
using System.Windows;

namespace ImageMorpher
{
    public class Morph
    {
        public static double A = 0.01;
        public static double B = 1;
        public static double P = 0;

        public static void Apply(ControlLineCanvas sourceCanvas, ControlLineCanvas destCanvas, DirectBitmap resultBitmap)
        {
            DirectBitmap source = sourceCanvas.DirectBitmap;

            for (int i = 0; i < source.Height; i++)
            {
                for (int j = 0; j < source.Width; j++)
                {
                    double totalWeight = 0;
                    double totalDeltaX = 0;
                    double totalDeltaY = 0;

                    Point t1 = new Point(j, i);

                    for (int k = 0; k < destCanvas.ControlLines.Count; k++)
                    {
                        ControlLine c1 = destCanvas.ControlLines[k];
                        ControlLine c2 = sourceCanvas.ControlLines[k];

                        // Destination
                        Point p1 = c1.Start.GetPixelPos();
                        Point q1 = c1.End.GetPixelPos();

                        Vector p1q1 = q1 - p1;
                        Vector n1 = new Vector(-(p1q1.Y), p1q1.X);
                        Vector t1p1 = p1 - t1;
                        Vector p1t1 = t1 - p1;

                        double d = (t1p1 * n1) / n1.Length;
                        double f = ((p1t1 * p1q1) / p1q1.Length) / p1q1.Length;

                        // Source
                        Point p2 = c2.Start.GetPixelPos();
                        Point q2 = c2.End.GetPixelPos();

                        Vector p2q2 = q2 - p2;
                        Vector n2 = new Vector(p2q2.Y, -(p2q2.X));
                        n2 /= n2.Length;
                        Point fp = new Point(p2.X + f * p2q2.X, p2.Y + f * p2q2.Y);
                        Point t2 = new Point(fp.X + n2.X * d, fp.Y + n2.Y * d);

                        double w = Math.Pow(Math.Pow(p1q1.Length, P) / (A + Math.Abs(d)), B);
                        double deltaX = (t2.X - t1.X) * w;
                        double deltaY = (t2.Y - t1.Y) * w;

                        totalWeight += w;
                        totalDeltaX += deltaX;
                        totalDeltaY += deltaY;
                    }

                    Point sp = new Point(t1.X + (totalDeltaX / totalWeight), t1.Y + (totalDeltaY / totalWeight));

                    if (sp.X < 0) sp.X = 0;
                    else if (sp.X > source.Width) sp.X = source.Width - 1;
                    if (sp.Y < 0) sp.Y = 0;
                    else if (sp.Y > source.Height) sp.Y = source.Height - 1;

                    resultBitmap.SetPixel((int)t1.X, (int)t1.Y, sourceCanvas.GetPixel((int)sp.X, (int)sp.Y));
                }
            }
        }
    }
}
