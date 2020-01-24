using ImageMorpher.Classes;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ImageMorpher
{
    /// <summary>4
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            dest.SetImage((int)source.image.Source.Width, (int)source.image.Source.Height);
            morphButton.IsEnabled = true;
        }

        private void morphButton_Click(object sender, RoutedEventArgs e)
        {
            Bitmap sourceBitmap = ImageUtil.BitmapSourceToBitmap((BitmapSource)source.image.Source);
            Bitmap destBitmap = ImageUtil.BitmapSourceToBitmap((BitmapSource)dest.image.Source);
            DirectBitmap resultBitmap = 

            

            result.Source = ImageUtil.BitmapToBitmapSource(resultBitmap);
        }
    }
}
