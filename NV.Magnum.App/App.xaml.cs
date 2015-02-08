using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace NV.Magnum.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _icon;
        private Kernel _kernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _kernel = new Kernel();
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
