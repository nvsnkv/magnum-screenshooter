using System;
using NV.Magnum.App.HotKey;

namespace NV.Magnum.App
{
    internal class Kernel : IDisposable
    {
        private readonly IHotKeyMonitor _hotKeyMonitor;

        public Kernel(IHotKeyMonitor hotKeyMonitor)
        {
            if (hotKeyMonitor == null) throw new ArgumentNullException("hotKeyMonitor");

            _hotKeyMonitor = hotKeyMonitor;
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (IsRunning)
                return;

            _hotKeyMonitor.Start();
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _hotKeyMonitor.Stop();

            IsRunning = false;
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