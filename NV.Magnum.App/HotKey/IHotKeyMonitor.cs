using System;

namespace NV.Magnum.App.HotKey
{
    public interface IHotKeyMonitor : IDisposable
    {
        event Action HotKeyPressed;
        void Start();
        void Stop();
    }
}