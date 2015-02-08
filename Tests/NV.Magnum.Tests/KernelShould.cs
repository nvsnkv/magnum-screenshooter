using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NV.Magnum.App;

namespace NV.Magnum.Tests
{
    [TestFixture, Category("Kernel")]
    public class KernelShould
    {
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

        [Test]
        public void SetIsRunningAfterSuccessfulStart()
        {
            _kernel.Start();

            Assert.That(_kernel.IsRunning, Is.True);
        }

        [Test]
        public void UnSetIsRunningAfterSuccessfulStop()
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
    }
}
