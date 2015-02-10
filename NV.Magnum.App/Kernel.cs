using System;
using NV.Magnum.App.HotKey;
using NV.Magnum.App.Screen;

namespace NV.Magnum.App
{
    internal class Kernel : IDisposable
    {
        private readonly IHotKeyMonitor _hotKeyMonitor;
        private readonly IScreenCather _screenCather;

        public Kernel(IHotKeyMonitor hotKeyMonitor, IScreenCather screenCather)
        {
            if (hotKeyMonitor == null) throw new ArgumentNullException("hotKeyMonitor");
            if (screenCather == null) throw new ArgumentNullException("screenCather");

            _hotKeyMonitor = hotKeyMonitor;
            _screenCather = screenCather;
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (IsRunning)
                return;

            _hotKeyMonitor.HotKeyPressed += HotKeyMonitorOnHotKeyPressed;
            _hotKeyMonitor.Start();
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _hotKeyMonitor.Stop();
            _hotKeyMonitor.HotKeyPressed -= HotKeyMonitorOnHotKeyPressed;

            IsRunning = false;
        }

        private void HotKeyMonitorOnHotKeyPressed(object o, EventArgs eventArgs)
        {
            _screenCather.TakePicture();
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