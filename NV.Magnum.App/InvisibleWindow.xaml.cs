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
        private WindowInteropHelper _helper;
        private HwndSource _source;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        
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
            var exStyle = (int)GetWindowLong(Handle, GWL_EXSTYLE);

            exStyle |= WS_EX_TOOLWINDOW;
            SetWindowLong(Handle, GWL_EXSTYLE, (IntPtr)exStyle);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        private static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            var error = 0;
            IntPtr result;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                var tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
    }
}
