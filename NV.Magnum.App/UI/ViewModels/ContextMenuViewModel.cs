using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace NV.Magnum.App.UI.ViewModels
{
    public class ContextMenuViewModel
    {
        private readonly string _screenshotsFolder;

        private ICommand _exitCommand;
        private ICommand _opensScreenshotsFolderCommand;

        public ContextMenuViewModel(string screenshotsFolder)
        {
            _screenshotsFolder = screenshotsFolder;
        }

        public KernelViewModel Kernel { get; set; }

        public ICommand Exit
        {
            get { return _exitCommand ?? (_exitCommand = new DelegateCommand(DoExit)); }
        }

        public ICommand OpenScreenshotsFolder
        {
            get { return _opensScreenshotsFolderCommand ?? (_opensScreenshotsFolderCommand = new DelegateCommand(DoOpenScreenshotsFolder));}
        }

        private void DoOpenScreenshotsFolder()
        {
            var p = new ProcessStartInfo("explorer") { Arguments = string.Format(@"""{0}""", _screenshotsFolder) };
            Process.Start(p);
        }

        private static void DoExit()
        {
            Application.Current.Shutdown();
        }
    }
}