using System;

namespace NV.Magnum.App.HotKey
{
    public interface IHotKeyMonitor : IDisposable
    {
        event EventHandler HotKeyPressed;
        void Start();
        void Stop();
    }
}