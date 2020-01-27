using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageMorpher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            source.PreviewMouseLeftButtonUp += Source_PreviewMouseLeftButtonUp;
            dest.PreviewMouseLeftButtonUp += Dest_PreviewMouseLeftButtonUp;
        }

        private void Source_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (source.IsDrawing)
            {
                source.IsDrawing = false;
                dest.AddControlLine(source.ControlLines[source.ControlLines.Count - 1]);
            }
        }

        private void Dest_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dest.IsDrawing)
            {
                dest.IsDrawing = false;
                source.AddControlLine(dest.ControlLines[dest.ControlLines.Count - 1]);
            }
        }

        private void sourceButton_Click(object sender, RoutedEventArgs e)
        {
            source.SetImage();
            destButton.IsEnabled = true;
        }

        private void destButton_Click(object sender, RoutedEventArgs e)
        {
            dest.SetImage();
            //dest.SetImage((int)source.image.Source.Width, (int)source.image.Source.Height);
            morphButton.IsEnabled = true;
        }

        private void morphButton_Click(object sender, RoutedEventArgs e)
        {
            DirectBitmap resultBitmap = new DirectBitmap(source.DirectBitmap.Width, source.DirectBitmap.Height);

            Morph.Apply(source, dest, resultBitmap);

            BitmapSource resultBitmapSource = ImageUtility.BitmapToSource(resultBitmap.Bitmap);
            result.Source = resultBitmapSource;
        }
    }
}
