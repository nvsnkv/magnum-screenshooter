using System;

namespace NV.Magnum.App.Screen
{
    public class ScreenshotCreatedEventArgs:EventArgs
    {
        public readonly IScreenshot Screenshot;

        public ScreenshotCreatedEventArgs(IScreenshot screenshot)
        {
            if (screenshot == null) throw new ArgumentNullException("screenshot");
            Screenshot = screenshot;
        }
    }
}