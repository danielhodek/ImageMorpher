﻿using ExtensionMethods;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageMorpher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Line> controlLines;
        private Point startPoint;
        private Point endPoint;
        private bool isStartPoint;

        public MainWindow()
        {
            InitializeComponent();

            isStartPoint = true;
        }

        private void SelectSourceButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                try
                {
                    string fileName = openFileDialog.FileName;
                    sourceImage.Source = new BitmapImage(new Uri(fileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void SourceCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("Clicked");
            if (isStartPoint)
            {
                startPoint = Mouse.GetPosition(sourceCanvas);
                isStartPoint = false;
            }
            else
            {
                endPoint = Mouse.GetPosition(sourceCanvas);
                sourceCanvas.DrawLine(Brushes.Red, 2, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
            }
        }

        private void sourceButton_Click(object sender, RoutedEventArgs e)
        {
            sourceImage.SetImage()
        }

        private void destButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}