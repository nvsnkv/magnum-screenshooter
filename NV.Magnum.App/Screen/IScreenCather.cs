using System;

namespace NV.Magnum.App.Screen
{
    public interface IScreenCather
    {
        void TakePicture();
        event EventHandler<ScreenshotCreatedEventArgs> ScreenshotCreated;
    }
}