using System;

namespace NV.Magnum.App
{
    internal class Kernel : IDisposable
    {
        public void Run()
        {
            
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            
        }

        public bool IsRunning { get; private set; }
    }
}