using System;
using System.Configuration;

namespace NV.Magnum.App.Storage
{
    public class ScreenshotStoredEventArgs:EventArgs
    {
        public string Name;

        public ScreenshotStoredEventArgs(string name)
        {
            Name = name;
        }
    }
}