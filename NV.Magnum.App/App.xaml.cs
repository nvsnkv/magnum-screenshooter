using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using NV.Magnum.App.HotKey;
using NV.Magnum.App.Screen;
using NV.Magnum.App.Storage;

namespace NV.Magnum.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "All disposables will be disposed OnExit")]
    public partial class App : Application
    {
        private TaskbarIcon _icon;
        private Kernel _kernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var hotkeyWindow = (InvisibleWindow)FindResource("HotKeyMonitorWindow");
            hotkeyWindow.Initialize();

            var monitor = new HotKeyMonitor(hotkeyWindow);
            monitor.HotKeyPressed += (o,args) => MessageBox.Show("HotKey was pressed!");

            _kernel = new Kernel(monitor,new ScreenCather(), new FileStorage());
            _kernel.Start();

            _icon = (TaskbarIcon) FindResource("TaskbarIcon");

        }

        protected override void OnExit(ExitEventArgs e)
        {
            _icon.Dispose();
            base.OnExit(e);
        }
    }
}
