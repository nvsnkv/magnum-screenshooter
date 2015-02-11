using System;
using System.Configuration;
using System.IO;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using NV.Magnum.App.HotKey;
using NV.Magnum.App.Screen;
using NV.Magnum.App.Storage;
using NV.Magnum.App.UI.ViewModels;

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

            var screenshotsFolder = ConfigurationManager.AppSettings["ScrenshotsFolder"] ?? "Screenshots";
            
            RunKernel(screenshotsFolder);

            _icon = new TaskbarIcon
            {
                DataContext = new ContextMenuViewModel(screenshotsFolder) {Kernel = new KernelViewModel(_kernel)},
                Style = (Style) FindResource("TaryIconStyle")
            };

        }

        private void RunKernel(string screenshotsFolder)
        {
            var hotkeyWindow = (InvisibleWindow) FindResource("HotKeyMonitorWindow");
            hotkeyWindow.Initialize();

            var monitor = new HotKeyMonitor(hotkeyWindow);
            monitor.HotKeyPressed += (o, args) => MessageBox.Show("HotKey was pressed!");

            if (!Directory.Exists(screenshotsFolder))
                Directory.CreateDirectory(screenshotsFolder);

            var fileStorage = new FileStorage(screenshotsFolder);

            _kernel = new Kernel(monitor, new ScreenCather(), fileStorage);
            _kernel.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _icon.Dispose();
            base.OnExit(e);
        }
    }
}
