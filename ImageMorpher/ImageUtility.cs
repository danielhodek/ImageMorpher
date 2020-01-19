using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageMorpher
{
    class ImageUtility
    {
        public static BitmapImage OpenImage()
        {
            var openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                try
                {
                    string fileName = openFileDialog.FileName;
                    var bitmap = new BitmapImage(new Uri(fileName));
                    return bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return null;
        }

        public static TransformedBitmap Resize(BitmapImage source, double width, double height)
        {
            return new TransformedBitmap(source, new ScaleTransform(width / source.PixelWidth, height / source.PixelHeight));
        }
    }
}
