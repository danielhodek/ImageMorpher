using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;

namespace ImageMorpher
{
    /// <summary>
    /// Interaction logic for ControlLineCanvas.xaml
    /// </summary>
    public partial class ControlLineCanvas : UserControl
    {
        public List<ControlLine> ControlLines { get; private set; } = new List<ControlLine>();
        public bool IsDrawing { get; set; } = false;
        public BitmapImage BitmapImage { get; set; }

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
            BitmapImage bitmapImage = ImageUtil.OpenImage();
            if (bitmapImage == null)
                return;
            image.Source = bitmapImage;
        }

        public void SetImage(int width, int height)
        {
            BitmapImage bitmapImage = ImageUtil.OpenImage();
            if (bitmapImage == null)
                return;
            Bitmap bitmap = ImageUtil.BitmapSourceToBitmap(bitmapImage);
            Bitmap resizedBitmap = ImageUtil.ResizeImage(bitmap, width, height);
            BitmapSource resizedBitmapSource = ImageUtil.BitmapToBitmapSource(resizedBitmap);
            image.Source = resizedBitmapSource;
        }

        public void AddControlLine(ControlLine controlLine)
        {
            var controlLineCopy = new ControlLine(this, controlLine);
            ControlLines.Add(controlLineCopy);
        }
    }
}
