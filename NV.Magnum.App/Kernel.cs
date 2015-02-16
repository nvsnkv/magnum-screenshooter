using System;
using NV.Magnum.App.Clipboard;
using NV.Magnum.App.HotKey;
using NV.Magnum.App.Screen;
using NV.Magnum.App.Server;
using NV.Magnum.App.Storage;

namespace NV.Magnum.App
{
    internal class Kernel : IDisposable
    {
        private readonly IHotKeyMonitor _hotKeyMonitor;
        private readonly IScreenCather _screenCather;
        private readonly IStorage _storage;
        private readonly IServer _server;
        private readonly IClipboardManager _clipboardManager;
        private readonly string _webRoot;

        public Kernel(IHotKeyMonitor hotKeyMonitor, IScreenCather screenCather, IStorage storage, IServer server, IClipboardManager clipboardManager, string webRoot)
        {
            if (hotKeyMonitor == null) throw new ArgumentNullException("hotKeyMonitor");
            if (screenCather == null) throw new ArgumentNullException("screenCather");
            if (storage == null) throw new ArgumentNullException("storage");
            if (server == null) throw new ArgumentNullException("server");
            if (clipboardManager == null) throw new ArgumentNullException("clipboardManager");
            if (string.IsNullOrEmpty(webRoot)) throw new ArgumentNullException("webRoot");

            _hotKeyMonitor = hotKeyMonitor;
            _screenCather = screenCather;
            _storage = storage;
            _server = server;
            _clipboardManager = clipboardManager;
            _webRoot = webRoot;
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (IsRunning)
                return;

            _hotKeyMonitor.HotKeyPressed += HotKeyMonitorOnHotKeyPressed;
            _screenCather.ScreenshotCreated += ScreenCatherOnScreenshotCreated;
            _storage.ScreenshotStored += StorageOnScreenshotStored;
            _server.Start();
            _hotKeyMonitor.Start();
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _hotKeyMonitor.Stop();
            _server.Stop();
            _hotKeyMonitor.HotKeyPressed -= HotKeyMonitorOnHotKeyPressed;
            _screenCather.ScreenshotCreated -= ScreenCatherOnScreenshotCreated;
            _storage.ScreenshotStored -= StorageOnScreenshotStored;

            IsRunning = false;
        }

        private void HotKeyMonitorOnHotKeyPressed(object o, EventArgs e)
        {
            _screenCather.TakePicture();
        }

        private void ScreenCatherOnScreenshotCreated(object sender, ScreenshotCreatedEventArgs e)
        {
            _storage.Store(e.Screenshot);
        }

        private void StorageOnScreenshotStored(object sender, ScreenshotStoredEventArgs e)
        {
            _clipboardManager.Set(_webRoot + e.Name);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {

        }
        #endregion
    }
}