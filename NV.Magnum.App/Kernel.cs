using System;
using NV.Magnum.App.HotKey;
using NV.Magnum.App.Screen;
using NV.Magnum.App.Storage;

namespace NV.Magnum.App
{
    internal class Kernel : IDisposable
    {
        private readonly IHotKeyMonitor _hotKeyMonitor;
        private readonly IScreenCather _screenCather;
        private readonly IStorage _storage;

        public Kernel(IHotKeyMonitor hotKeyMonitor, IScreenCather screenCather, IStorage storage)
        {
            if (hotKeyMonitor == null) throw new ArgumentNullException("hotKeyMonitor");
            if (screenCather == null) throw new ArgumentNullException("screenCather");
            if (storage == null) throw new ArgumentNullException("storage");

            _hotKeyMonitor = hotKeyMonitor;
            _screenCather = screenCather;
            _storage = storage;
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (IsRunning)
                return;

            _hotKeyMonitor.HotKeyPressed += HotKeyMonitorOnHotKeyPressed;
            _screenCather.ScreenshotCreated += ScreenCatherOnScreenshotCreated;
            _hotKeyMonitor.Start();
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _hotKeyMonitor.Stop();
            _hotKeyMonitor.HotKeyPressed -= HotKeyMonitorOnHotKeyPressed;
            _screenCather.ScreenshotCreated -= ScreenCatherOnScreenshotCreated;

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