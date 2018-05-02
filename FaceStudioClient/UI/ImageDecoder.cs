using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace FaceStudioClient.UI
{
    public class ImageDecoder
    {
        public static readonly DependencyProperty SourceProperty;
        public static string GetSource(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("Image");
            }

            return (String)image.GetValue(ImageDecoder.SourceProperty);
        }

        public static void SetSource(Image image, string value)
        {
            if (image == null)
            {
                throw new ArgumentNullException("Image");
            }
            image.SetValue(ImageDecoder.SourceProperty, value);
        }

        static ImageDecoder()
        {
            ImageDecoder.SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(string), typeof(ImageDecoder), new PropertyMetadata(new PropertyChangedCallback(ImageDecoder.OnSourceWithSourceChanged)));
            ImageQueue.OnComplete += ImageDecoder.OnImageDownloadCompleted;
        }

        private static void OnImageDownloadCompleted(Image i, string u, BitmapImage b)
        {
            string source = ImageDecoder.GetSource(i);
            if (source == u.ToString())
            {
                i.Source = b;
                //Storyboard storyboard = new Storyboard();
                //DoubleAnimation doubleAnimation = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromMilliseconds(500.0)));
                //Storyboard.SetTarget(doubleAnimation, i);
                //Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity", new object[0]));
                //storyboard.Children.Add(doubleAnimation);
                //storyboard.Begin();
                //if (i.Parent is Grid)
                //{
                //    Grid grid = i.Parent as Grid;
                //    foreach (var c in grid.Children)
                //    {
                //        if (c is WaitingProgressPane && c != null)
                //        {
                //            (c as WaitingProgressPane).Stop();
                //            break;
                //        }
                //    }
                //}
            }
        }

        private static void OnSourceWithSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ImageQueue.Queue((Image)o, (string)e.NewValue);
        }
    }
}
