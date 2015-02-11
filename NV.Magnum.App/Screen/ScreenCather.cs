using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace NV.Magnum.App.Screen
{
    internal class ScreenCather : IScreenCather
    {
        public void TakePicture()
        {
            using (var screenBmp = new Bitmap(
                (int) SystemParameters.PrimaryScreenWidth,
                (int) SystemParameters.PrimaryScreenHeight,
                PixelFormat.Format32bppArgb))
            {
                using (var bmpGraphics = Graphics.FromImage(screenBmp))
                {
                    bmpGraphics.CopyFromScreen(0, 0, 0, 0, screenBmp.Size);
                    var source = Imaging.CreateBitmapSourceFromHBitmap(
                        screenBmp.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());

                    var encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(source));
                    using (var mstream = new MemoryStream())
                    {
                        encoder.Save(mstream);

                        var content = mstream.ToArray();
                        var screenshot = new Screenshot(content);
                        RaiseScreenshotCreated(new ScreenshotCreatedEventArgs(screenshot));
                    }

                }
            }
        }

        public event EventHandler<ScreenshotCreatedEventArgs> ScreenshotCreated;

        protected virtual void RaiseScreenshotCreated(ScreenshotCreatedEventArgs e)
        {
            var handler = ScreenshotCreated;
            if (handler != null) handler(this, e);
        }
    }
}