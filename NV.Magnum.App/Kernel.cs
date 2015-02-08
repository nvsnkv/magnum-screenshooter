using System;

namespace NV.Magnum.App
{
    internal class Kernel : IDisposable
    {
        public bool IsRunning { get; private set; }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
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