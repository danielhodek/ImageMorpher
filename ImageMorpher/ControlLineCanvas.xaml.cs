using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageMorpher
{
    /// <summary>
    /// Interaction logic for ControlLineCanvas.xaml
    /// </summary>
    public partial class ControlLineCanvas : UserControl
    {
        public List<ControlLine> ControlLines { get; private set; } = new List<ControlLine>();
        public bool IsDrawing { get; set; } = false;

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
            BitmapImage bitmap = ImageUtility.OpenImage();
            image.Source = bitmap;
        }

        public void SetImage(double width, double height)
        {
            BitmapImage bitmap = ImageUtility.OpenImage();
            TransformedBitmap transformedBitmap = ImageUtility.Resize(bitmap, width, height);
            image.Source = transformedBitmap;
        }

        public void AddControlLine(ControlLine controlLine)
        {
            var controlLineCopy = new ControlLine(this, controlLine);
            ControlLines.Add(controlLineCopy);
        }
    }
}
