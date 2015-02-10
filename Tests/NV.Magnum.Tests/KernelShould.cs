using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NV.Magnum.App;
using NV.Magnum.App.HotKey;
using NV.Magnum.App.Screen;

namespace NV.Magnum.Tests
{
    [TestFixture, Category("Kernel")]
    public class KernelShould
    {
        #region Setup

        private class Mocks
        {
            public readonly Mock<IHotKeyMonitor> HotKeyMonitor = new Mock<IHotKeyMonitor>();
            public readonly Mock<IScreenCather> ScreenCatcher = new Mock<IScreenCather>();
        }

        private Mocks _mocks;
        private Kernel _kernel;
        

        [SetUp]
        public void CreateKernel()
        {
            _mocks = new Mocks();
            _kernel = new Kernel(_mocks.HotKeyMonitor.Object, _mocks.ScreenCatcher.Object);
        }

        [TearDown]
        public void DisposeKernel()
        {
            _kernel.Dispose();
            _kernel = null;
            _mocks = null;
        } 
        #endregion

        [Test]
        public void SetIsRunningAfterSuccessfulStart()
        {
            _kernel.Start();

            Assert.That(_kernel.IsRunning, Is.True);
        }

        [Test]
        public void UnsetIsRunningAfterSuccessfulStop()
        {
            _kernel.Start();

            _kernel.Stop();
            Assert.That(_kernel.IsRunning, Is.False);
        }

        [Test]
        public void NotSetIsRunningAtCreation()
        {
            Assert.That(_kernel.IsRunning, Is.False);
        }

        
        [Test, Category("HotKeyMonitor")]
        public void StartHotKeyMonitorOnceWhenStarted()
        {
            var mock = _mocks.HotKeyMonitor;
            mock.Setup(m => m.Start()).Verifiable();
            
            _kernel.Start();
            _kernel.Start();

            mock.Verify(m => m.Start(), Times.Once);
        }

        [Test, Category("HotKeyMonitor")]
        public void StoptHotKeyMonitorOnceWhenStopped()
        {
            var mock = _mocks.HotKeyMonitor;
            mock.Setup(m => m.Stop()).Verifiable();
            
            _kernel.Start();
            
            _kernel.Stop();
            _kernel.Stop();

            mock.Verify(m => m.Stop(), Times.Once);
        }

        [Test, Category("ScreenCather")]
        public void CallScreenCatcherTakePictureAfterHotKeyMonitorHotKeyPressedWasRaised()
        {
            var monitor = _mocks.HotKeyMonitor;
            var catcher = _mocks.ScreenCatcher;
            catcher.Setup(c => c.TakePicture()).Verifiable();

            _kernel.Start();

            monitor.Raise(m => m.HotKeyPressed += null, EventArgs.Empty);
            catcher.Verify(c => c.TakePicture(), Times.Once);
        }
    }
}


