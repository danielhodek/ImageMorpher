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
        public BitmapSource BitmapSource { get { return (BitmapSource)image.Source; } }

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

        public void SetImage()
        {
            BitmapImage bitmapImage = ImageUtility.OpenImage();
            if (bitmapImage == null)
                return;
            image.Source = bitmapImage;
            Bitmap bitmap = ImageUtility.SourceToBitmap(bitmapImage);
            DirectBitmap = new DirectBitmap(bitmap);
        }

        public void SetImage(int width, int height)
        {
            BitmapImage bitmapImage = ImageUtility.OpenImage();
            if (bitmapImage == null)
                return;
            Bitmap bitmap = ImageUtility.SourceToBitmap(bitmapImage);
            if (bitmap.Width != width || bitmap.Height != height)
            {
                bitmap = ImageUtility.ResizeImage(bitmap, width, height);
            }
            DirectBitmap = new DirectBitmap(bitmap);
            image.Source = DirectBitmap.BitmapSource;
        }

        public void Add(UIElement element)
        {
            canvas.Children.Add(element);
        }

        public void RemoveAt(int index)
        {
            var cl = ControlLines[index];
            canvas.Children.Remove(cl.Path);
            canvas.Children.Remove(cl.Start);
            canvas.Children.Remove(cl.End);
            canvas.Children.Remove(cl.Mid);
            ControlLines.Remove(cl);
        }

        public void Clear()
        {
            for (int i = 0; i < ControlLines.Count; i++)
            {
                RemoveAt(0);
            }
        }
    }
}
