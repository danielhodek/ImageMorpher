using System;
using System.Windows;

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
            dest.SetImage(source.image.Source.Width, source.image.Source.Height);
            morphButton.IsEnabled = true;
        }

        private void morphButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("source image width=" + source.PixelWidth);
            Console.WriteLine("source image height=" + source.PixelHeight);
        }
    }
}
