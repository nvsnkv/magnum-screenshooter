using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace NV.Magnum.App
{
    /// <summary>
    /// Interaction logic for InvisibleWindow.xaml
    /// </summary>
    public partial class InvisibleWindow : Window
    {
        private static class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr SetParent(IntPtr hwnd, IntPtr hwndNewParent);
        }

        private WindowInteropHelper _helper;
        private HwndSource _source;
        
        public InvisibleWindow()
        {
            InitializeComponent();
        }

        public IntPtr Handle
        {
            get { return _helper.Handle; }
        }

        public void AddHook(HwndSourceHook hook)
        {
            _source.AddHook(hook);
        }

        public void RemoveHook(HwndSourceHook hook)
        {
            _source.RemoveHook(hook);
        }

        public void Initialize()
        {
            Show();
            _helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(_helper.Handle);
        }

        private void InvisibleWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IntPtr.Zero == NativeMethods.SetParent(Handle, (IntPtr)(-3)))
            {
                var err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            };
        }
    }
}
