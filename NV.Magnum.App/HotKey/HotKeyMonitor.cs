using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace NV.Magnum.App.HotKey
{
    internal class HotKeyMonitor : IHotKeyMonitor
    {
        private const int HOTKEY_ID = 33011;
        private const int MOD_ALT = 0x1;
        private const int VK_SNAPSHOT = 0x2c;

        [DllImport("User32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);

        private readonly InvisibleWindow _window;

        public HotKeyMonitor(InvisibleWindow window)
        {
            if (window == null) throw new ArgumentNullException("window");
            _window = window;
        }

        public event Action HotKeyPressed;

        public void Start()
        {
            if (IsRunning) return;

            _window.AddHook(HotKeyHook);

            if (!RegisterHotKey(_window.Handle, HOTKEY_ID, MOD_ALT, VK_SNAPSHOT))
            {
                var err = Marshal.GetLastWin32Error();
                throw new InvalidOperationException(string.Format("Unable to register hotkey (Error code 0x{0:X})", err), new Win32Exception(err));
            }

            IsRunning = true;
        }

        private IntPtr HotKeyHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wparam.ToInt32())
                    {
                        case HOTKEY_ID:
                            RaiseHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        public bool IsRunning { get; private set; }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _window.RemoveHook(HotKeyHook);
            if (!UnregisterHotKey(_window.Handle, HOTKEY_ID))
            {
                var err = Marshal.GetLastWin32Error();
                throw new InvalidOperationException(string.Format("Unable to unregister hotkey (Error code 0x{0:X})", err), new Win32Exception(err));
            }

            IsRunning = false;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();
        }

        protected virtual void RaiseHotKeyPressed()
        {
            var handler = HotKeyPressed;
            if (handler != null) handler();
        }
    }
}