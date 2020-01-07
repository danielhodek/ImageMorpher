using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ExtensionMethods
{
    public static class CanvasExtensions
    {
        public static void DrawLine(this Canvas canvas, Brush color, double thickness, double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            line.Stroke = color;
            line.StrokeThickness = thickness;
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            canvas.Children.Add(line);
        }
    }
}
