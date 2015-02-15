using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NV.Magnum.App;
using NV.Magnum.App.Clipboard;
using NV.Magnum.App.HotKey;
using NV.Magnum.App.Screen;
using NV.Magnum.App.Server;
using NV.Magnum.App.Storage;

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
            public readonly Mock<IStorage> Storage = new Mock<IStorage>();
            public readonly Mock<IServer> Server = new Mock<IServer>();
            public readonly Mock<IClipboardManager> ClipboardManager = new Mock<IClipboardManager>();
        }

        private Mocks _mocks;
        private Kernel _kernel;
        

        [SetUp]
        public void CreateKernel()
        {
            _mocks = new Mocks();
            _kernel = new Kernel(_mocks.HotKeyMonitor.Object, _mocks.ScreenCatcher.Object, _mocks.Storage.Object, _mocks.Server.Object, _mocks.ClipboardManager.Object);
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

        [Test, Category("Server")]
        public void StartServerOnceWhenStarted()
        {
            var mock = _mocks.Server;
            mock.Setup(m => m.Start()).Verifiable();

            _kernel.Start();
            _kernel.Start();

            mock.Verify(m => m.Start(), Times.Once);
        }

        [Test, Category("Server")]
        public void StoptServerOnceWhenStopped()
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

        [Test, Category("ScreenCather")]
        public void NotCallScreenCatcherTakePictureWhenStopped()
        {
            var monitor = _mocks.HotKeyMonitor;
            var catcher = _mocks.ScreenCatcher;
            catcher.Setup(c => c.TakePicture()).Verifiable();

            _kernel.Start();
            _kernel.Stop();

            monitor.Raise(m => m.HotKeyPressed += null, EventArgs.Empty);
            catcher.Verify(c => c.TakePicture(), Times.Never);
        }

        [Test, Category("Storage")]
        public void CallStorageStoreWhenScreenshotCatherCathesAScreenshot()
        {
            var expectedScreenshot = new Mock<IScreenshot>().Object;

            var storage = _mocks.Storage;
            var catcher = _mocks.ScreenCatcher;
            storage.Setup(s => s.Store(expectedScreenshot));

            _kernel.Start();
            catcher.Raise(c => c.ScreenshotCreated += null, new ScreenshotCreatedEventArgs(expectedScreenshot));

            storage.Verify(s => s.Store(expectedScreenshot), Times.Once);
        }

        [Test, Category("Storage")]
        public void NotCallStorageStoreWhenStopped()
        {
            var storage = _mocks.Storage;
            var catcher = _mocks.ScreenCatcher;
            storage.Setup(s => s.Store(It.IsAny<IScreenshot>()));

            _kernel.Start();
            _kernel.Stop();

            catcher.Raise(c => c.ScreenshotCreated += null, new ScreenshotCreatedEventArgs(new Mock<IScreenshot>().Object));

            storage.Verify(s => s.Store(It.IsAny<IScreenshot>()), Times.Never);
        }

        [Test, Category("Clipboard")]
        public void CallCLipboardManagerSetWhenStorageStoresAScreenshot()
        {
            const string NAME = "Screenshot1";

            var manager = _mocks.ClipboardManager;
            manager.Setup(m => m.Set(NAME)).Verifiable();

            var storage = _mocks.Storage;
            _kernel.Start();

            storage.Raise(s => s.ScreenshotStored += null, new ScreenshotStoredEventArgs(NAME));

            manager.Verify(m => m.Set(NAME), Times.Once);
        }

        [Test, Category("Clipboard")]
        public void NotCallCLipboardManagerSetWhenStopped()
        {
            const string NAME = "Screenshot1";

            var manager = _mocks.ClipboardManager;
            manager.Setup(m => m.Set(NAME)).Verifiable();

            var storage = _mocks.Storage;
            
            storage.Raise(s => s.ScreenshotStored += null, new ScreenshotStoredEventArgs(NAME));

            manager.Verify(m => m.Set(NAME), Times.Never);
        }
    }
}


