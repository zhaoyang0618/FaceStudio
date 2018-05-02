using FaceStudioClient.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FaceStudioClient.UI
{
    public class ImageQueue
    {
        #region 辅助类别
        private class ImageQueueInfo
        {
            public Image image { get; set; }
            public String url { get; set; }
        }
        #endregion

        public delegate void CompleteDelegate(Image i, string u, BitmapImage b);
        public static event CompleteDelegate OnComplete;
        private static AutoResetEvent autoEvent;
        private static Queue<ImageQueueInfo> Stacks;
        static ImageQueue()
        {
            ImageQueue.Stacks = new Queue<ImageQueueInfo>();
            autoEvent = new AutoResetEvent(true);
            Thread t = new Thread(new ThreadStart(ImageQueue.DownloadImage));
            t.Name = "下载图片";
            t.IsBackground = true;
            t.Start();
        }

        private static async void DownloadImage()
        {
            while (true)
            {
                ImageQueueInfo t = null;
                lock (ImageQueue.Stacks)
                {
                    if (ImageQueue.Stacks.Count > 0)
                    {
                        t = ImageQueue.Stacks.Dequeue();
                    }
                }

                if (t != null)
                {
                    Uri uri = new Uri(t.url);
                    BitmapImage image = null;
                    try
                    {
                        if ("http".Equals(uri.Scheme, StringComparison.CurrentCultureIgnoreCase))
                        {
                            //如果是HTTP下载文件
                            var service = new PhotoImageService();
                            Byte[] imgData = await service.Download(t.url);
                            if (null != imgData)
                            {
                                using (var ms = new MemoryStream(imgData))
                                {
                                    image = new BitmapImage();
                                    image.BeginInit();
                                    image.CacheOption = BitmapCacheOption.OnLoad;
                                    image.StreamSource = ms;
                                    image.EndInit();
                                }
                            }
                        }
                        else if ("file".Equals(uri.Scheme, StringComparison.CurrentCultureIgnoreCase))
                        {
                            using (var fs = new FileStream(t.url, FileMode.Open))
                            {
                                image = new BitmapImage();
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = fs;
                                image.EndInit();
                            }
                        }

                        if (image != null)
                        {
                            if (image.CanFreeze)
                                image.Freeze();

                            await t.image.Dispatcher.BeginInvoke(new Action<ImageQueueInfo, BitmapImage>((i, bmp) =>
                            {
                                if (ImageQueue.OnComplete != null)
                                {
                                    ImageQueue.OnComplete(i.image, i.url, bmp);
                                }
                            }), new Object[] { t, image });
                        }

                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Message);
                        continue;
                    }

                }

                if (ImageQueue.Stacks.Count > 0) continue;
                autoEvent.WaitOne();
            }
        }

        public static void Queue(Image img, String url)
        {
            if (String.IsNullOrEmpty(url)) return;
            lock (ImageQueue.Stacks)
            {
                ImageQueue.Stacks.Enqueue(new ImageQueueInfo { url = url, image = img });
                ImageQueue.autoEvent.Set();
            }
        }
    }
}
