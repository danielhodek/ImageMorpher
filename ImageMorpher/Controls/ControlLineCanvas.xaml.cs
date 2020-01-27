using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;

namespace ImageMorpher
{
    public partial class ControlLineCanvas : UserControl
    {
        public List<ControlLine> ControlLines { get; private set; } = new List<ControlLine>();
        public bool IsDrawing { get; set; } = false;
        public DirectBitmap DirectBitmap { get; private set; }

        public ControlLineCanvas()
        {
            InitializeComponent();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(canvas);
            var controlLine = new ControlLine(this, mousePos.X, mousePos.Y);
            IsDrawing = true;
            ControlLines.Add(controlLine);
            controlLine.End.CaptureMouse();
            controlLine.End.RaiseEvent(e);
        }

        public void Add(UIElement element)
        {
            canvas.Children.Add(element);
        }

        public void SetImage()
        {
            BitmapImage bitmapImage = ImageUtility.OpenImage();
            if (bitmapImage == null)
                return;
            InitBitmap(bitmapImage);
        }

        public void SetImage(int width, int height)
        {
            BitmapImage bitmapImage = ImageUtility.OpenImage();
            if (bitmapImage == null)
                return;
            Bitmap bitmap = ImageUtility.SourceToBitmap(bitmapImage);
            Bitmap resizedBitmap = ImageUtility.ResizeImage(bitmap, width, height);
            BitmapSource resizedBitmapSource = ImageUtility.BitmapToSource(resizedBitmap);
            InitBitmap(resizedBitmapSource);
        }

        public void SetPixel(int x, int y, Color color)
        {
            DirectBitmap.SetPixel(x, y, color);
        }

        public Color GetPixel(int x, int y)
        {
            return DirectBitmap.GetPixel(x, y);
        }

        public void AddControlLine(ControlLine controlLine)
        {
            var controlLineCopy = new ControlLine(this, controlLine);
            ControlLines.Add(controlLineCopy);
        }

        private void InitBitmap(BitmapSource bitmapSource)
        {
            image.Source = bitmapSource;
            Bitmap bitmap = ImageUtility.SourceToBitmap((BitmapSource)image.Source);
            DirectBitmap = new DirectBitmap(bitmap);
        }
    }
}
