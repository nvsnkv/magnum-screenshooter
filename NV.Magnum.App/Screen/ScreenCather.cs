using System;

namespace NV.Magnum.App.Screen
{
    internal class ScreenCather : IScreenCather
    {
        public void TakePicture()
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<ScreenshotCreatedEventArgs> ScreenshotCreated;
    }
}