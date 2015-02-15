using System;
using System.IO;
using NV.Magnum.App.Screen;

namespace NV.Magnum.App.Storage
{
    internal class FileStorage : IStorage
    {
        private readonly string _path;

        public FileStorage(string path)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException("Storage directory was not found!","path");

            _path = path;
        }

        public void Store(IScreenshot screenshot)
        {
            var name = Guid.NewGuid() + ".jpg";

            File.WriteAllBytes(Path.Combine(_path, name),screenshot.Content);

            RaiseScreenshotStored(new ScreenshotStoredEventArgs(name));
        }

        public event EventHandler<ScreenshotStoredEventArgs> ScreenshotStored;

        protected virtual void RaiseScreenshotStored(ScreenshotStoredEventArgs e)
        {
            var handler = ScreenshotStored;
            if (handler != null) handler(this, e);
        }
    }
}