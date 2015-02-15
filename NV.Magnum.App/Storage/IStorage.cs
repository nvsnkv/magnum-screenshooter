using System;
using NV.Magnum.App.Screen;

namespace NV.Magnum.App.Storage
{
    public interface IStorage
    {
        void Store(IScreenshot screenshot);

        event EventHandler<ScreenshotStoredEventArgs> ScreenshotStored;
    }
}