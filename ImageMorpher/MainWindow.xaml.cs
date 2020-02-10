using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ImageMorpher
{
    public partial class MainWindow : Window
    {
        private BitmapSource[] frames;
        private int frameIndex = 0;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            source.PreviewMouseLeftButtonUp += Source_PreviewMouseLeftButtonUp;
            dest.PreviewMouseLeftButtonUp += Dest_PreviewMouseLeftButtonUp;
            ToggleMorphSettings(false);
        }

        private void AddLine(ControlLineCanvas canvas1, ControlLineCanvas canvas2)
        {
            if (canvas1.IsDrawing)
            {
                canvas1.IsDrawing = false;
                var cl1 = canvas1.ControlLines[canvas1.ControlLines.Count - 1];
                var cl2 = new ControlLine(canvas2, cl1);
                canvas2.ControlLines.Add(cl2);
                cl1.Mid.PreviewMouseRightButtonDown += DeleteLine;
                cl2.Mid.PreviewMouseRightButtonDown += DeleteLine;
            }
        }

        private void DeleteLine(object sender, MouseButtonEventArgs e)
        {
            var m = sender as ControlLineMidThumb;
            int index = m.ControlLine.Index;
            source.RemoveAt(index);
            dest.RemoveAt(index);
            Console.WriteLine(source.ControlLines.Count);
            Console.WriteLine(dest.ControlLines.Count);
        }

        private void Reset()
        {
            frameIndex = 0;
            result.Source = frames[0];
        }

        private void ToggleMorphSettings(bool isEnabled)
        {
            morphButton.IsEnabled = isEnabled;
            aSetting.IsEnabled = isEnabled;
            bSetting.IsEnabled = isEnabled;
            pSetting.IsEnabled = isEnabled;
            nextFrameButton.IsEnabled = isEnabled;
            prevFrameButton.IsEnabled = isEnabled;
            animateButton.IsEnabled = isEnabled;
            numFramesSetting.IsEnabled = isEnabled;
            numThreadsSetting.IsEnabled = isEnabled;
        }

        private void Source_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddLine(source, dest);
        }

        private void Dest_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddLine(dest, source);
        }

        private void sourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (source.SetImage() == true)
            {
                sourceButton.IsEnabled = false;
                destButton.IsEnabled = true;
            }
        }

        private void destButton_Click(object sender, RoutedEventArgs e)
        {
            if (dest.SetImage(source.BitmapSource.PixelWidth, source.BitmapSource.PixelHeight) == true)
            {
                destButton.IsEnabled = false;
                source.CanDraw = true;
                dest.CanDraw = true;
                ToggleMorphSettings(true);
            }
        }

        private async void morphButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMorphSettings(false);

            frameIndex = 0;
            DirectBitmap resultBitmap = new DirectBitmap(source.DirectBitmap.Width, source.DirectBitmap.Height);

            double a = Convert.ToDouble(aSetting.Text);
            double b = Convert.ToDouble(bSetting.Text);
            double p = Convert.ToDouble(pSetting.Text);
            int numFrames = Convert.ToInt32(numFramesSetting.Text);
            int numFramesLeft = numFrames;
            int numThreads = Convert.ToInt32(numThreadsSetting.Text);

            frames = new BitmapSource[numFrames];
            Task[] tasks = new Task[numThreads];

            Morpher morpher = new Morpher(source, dest, numFrames, a, b, p);

            if (numFrames < 1)
                return;

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < numThreads; i++)
            {
                int numThreadFrames = (i < numThreads - 1) ? (numFramesLeft / numThreads) : numFramesLeft;
                int begin = numFrames - numFramesLeft;
                tasks[i] = Task.Run(() =>
                {
                    for (int j = begin; j < begin + numThreadFrames; j++)
                    {
                        BitmapSource frame = morpher.GenerateFrame(j);
                        frame.Freeze();
                        frames[j] = frame;
                    }
                });
                numFramesLeft -= numThreadFrames;
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();
            benchmark.Content = "Benchmark: " + stopwatch.ElapsedMilliseconds + "ms";

            Console.WriteLine("done");
            result.Source = frames[0];
            ToggleMorphSettings(true);
        }

        private void nextFrameButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(frameIndex);
            if (frameIndex < frames.Length - 1)
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
            Reset();
            ToggleMorphSettings(false);
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (++frameIndex < frames.Length)
            {
                result.Source = frames[frameIndex];    
            } 
            else
            {
                timer.Stop();
                ToggleMorphSettings(true);
            }         
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            source.Clear();
            dest.Clear();
            source.CanDraw = false;
            dest.CanDraw = false;
            source.image.Source = null;
            dest.image.Source = null;
            result.Source = null;
            sourceButton.IsEnabled = true;
            destButton.IsEnabled = false;
            frameIndex = 0;

            ToggleMorphSettings(false);
        }
    }
}
