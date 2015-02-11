using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace NV.Magnum.App.UI.ViewModels
{
    public class KernelViewModel:BindableBase
    {
        private readonly Kernel _kernel;

        private bool _isRunning;
        private ICommand _startCommand;
        private ICommand _stopCommand;

        internal KernelViewModel(Kernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException("kernel");
            _kernel = kernel;
            _isRunning = _kernel.IsRunning;
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            private set { SetProperty(ref _isRunning, value); }
        }

        public ICommand Run
        {
            get { return _startCommand ?? (_startCommand = new DelegateCommand(DoStart)); }
        }

        public ICommand Stop
        {
            get { return _stopCommand ?? (_stopCommand = new DelegateCommand(DoStop)); }
        }

        private void DoStop()
        {
            _kernel.Stop();
            IsRunning = false;
        }

        private void DoStart()
        {
            _kernel.Start();
            IsRunning = true;
        }
    }
}