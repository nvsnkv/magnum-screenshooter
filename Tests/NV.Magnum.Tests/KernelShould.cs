using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NV.Magnum.App;
using NV.Magnum.App.HotKey;

namespace NV.Magnum.Tests
{
    [TestFixture, Category("Kernel")]
    public class KernelShould
    {
        #region Setup
        private Kernel _kernel;

        [SetUp]
        public void CreateKernel()
        {
            _kernel = new Kernel();
        }

        [TearDown]
        public void DisposeKernel()
        {
            _kernel.Dispose();
            _kernel = null;
        } 
        #endregion

        [Test]
        public void SetIsRunningAfterSuccessfulStart()
        {
            _kernel.HotKeyMonitor = new Mock<IHotKeyMonitor>().Object;
            _kernel.Start();

            Assert.That(_kernel.IsRunning, Is.True);
        }

        [Test]
        public void UnsetIsRunningAfterSuccessfulStop()
        {
            _kernel.HotKeyMonitor = new Mock<IHotKeyMonitor>().Object;
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
        public void NotStartWhenHotKeyMonitorIsMissing()
        {
            Assert.Throws<InvalidOperationException>(() => _kernel.Start());
        }

        [Test, Category("HotKeyMonitor")]
        public void StartHotKeyMonitorOnceWhenStarted()
        {
            var mock = new Mock<IHotKeyMonitor>();
            mock.Setup(m => m.Start()).Verifiable();
            _kernel.HotKeyMonitor = mock.Object;

            _kernel.Start();
            _kernel.Start();

            mock.Verify(m => m.Start(), Times.Once);
        }

        [Test, Category("HotKeyMonitor")]
        public void StoptHotKeyMonitorOnceWhenStopped()
        {
            var mock = new Mock<IHotKeyMonitor>();
            mock.Setup(m => m.Stop()).Verifiable();
            _kernel.HotKeyMonitor = mock.Object;

            _kernel.Start();
            
            _kernel.Stop();
            _kernel.Stop();

            mock.Verify(m => m.Stop(), Times.Once);
        }

        [Test, Category("HotKeyMonitor")]
        public void ForbidHotKeyMonitorModificationWhenRunning()
        {
            _kernel.HotKeyMonitor = new Mock<IHotKeyMonitor>().Object;
            _kernel.Start();

            Assert.Throws<InvalidOperationException>(() => _kernel.HotKeyMonitor = new Mock<IHotKeyMonitor>().Object);
        }
    }
}


