using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageMorpher
{
    public partial class MainWindow : Window
    {
        private List<BitmapSource> frames = new List<BitmapSource>();
        private int frameIndex = 0;
        
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
            dest.SetImage(source.BitmapSource.PixelWidth, source.BitmapSource.PixelHeight);
        }

        private void morphButton_Click(object sender, RoutedEventArgs e)
        {
            frames.Clear();
            frameIndex = 0;
            DirectBitmap resultBitmap = new DirectBitmap(source.DirectBitmap.Width, source.DirectBitmap.Height);

            double a = Convert.ToDouble(aSetting.Text);
            double b = Convert.ToDouble(bSetting.Text);
            double p = Convert.ToDouble(pSetting.Text);
            int numFrames = Convert.ToInt32(numFramesSetting.Text);

            Morpher morpher = new Morpher(source, dest, numFrames, a, b, p);

            var f = morpher.NextFrame();
            if (f != null) result.Source = f;
           
            while (f != null)
            {
                frames.Add(f);
                f = morpher.NextFrame();   
            }
        }

        private void nextFrameButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(frameIndex);
            if (frameIndex < frames.Count - 1)
            {
                result.Source = frames[++frameIndex];
            }
        }

        private void prevFrameButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(frameIndex);
            if (frameIndex > 0)
            {
                result.Source = frames[--frameIndex];
            }
        }

        private void animateButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
