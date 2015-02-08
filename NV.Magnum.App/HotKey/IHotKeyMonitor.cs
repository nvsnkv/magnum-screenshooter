using System;

namespace NV.Magnum.App.HotKey
{
    public interface IHotKeyMonitor : IDisposable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Jeffrey Rihter told me to implement events this way")]
        event Action<object,EventArgs> HotKeyPressed;
        void Start();
        void Stop();
    }
}