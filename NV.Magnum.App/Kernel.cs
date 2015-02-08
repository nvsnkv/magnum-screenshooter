using System;
using NV.Magnum.App.HotKey;

namespace NV.Magnum.App
{
    internal class Kernel : IDisposable
    {
        private IHotKeyMonitor _hotKeyMonitor;
        public bool IsRunning { get; private set; }

        public IHotKeyMonitor HotKeyMonitor
        {
            get { return _hotKeyMonitor; }
            set
            {
                if (IsRunning)
                    throw new InvalidOperationException("Unable to change HotKeyMonitor while running!");
                _hotKeyMonitor = value;
            }
        }

        public void Start()
        {
            if (IsRunning)
                return;

            if (HotKeyMonitor == null)
                throw new InvalidOperationException("Unable to start: HotKeyMonitor missing!");
            
            HotKeyMonitor.Start();
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            HotKeyMonitor.Stop();

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