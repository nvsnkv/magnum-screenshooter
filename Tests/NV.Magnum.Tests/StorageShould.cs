using System;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using NV.Magnum.App.Screen;
using NV.Magnum.App.Storage;

namespace NV.Magnum.Tests
{
    [TestFixture, Category("Storage")]
    public class FileStorageShould
    {
        private string _storagePath;

        [SetUp]
        public void PrepareFileSystem()
        {
            _storagePath = Path.Combine(Environment.CurrentDirectory, "TMP");
            if (Directory.Exists(_storagePath))
                Directory.Delete(_storagePath,true);

            Directory.CreateDirectory(_storagePath);
        }

        [Test, Category("Integation")]
        public void StoreScreenshotOnDisk()
        {
            var content = new byte[2*1024*1024];
            var rnd = new Random();
            rnd.NextBytes(content);

            var screenshot = new Mock<IScreenshot>();
            screenshot.Setup(s => s.Content).Returns(content);

            var storage = new FileStorage(_storagePath);

            var beforeSave = DateTime.Now;
            storage.Store(screenshot.Object);

            var files = Directory.GetFiles(_storagePath);
            Assert.That(files.Count(), Is.EqualTo(1));

            var file = new FileInfo(files.First());
            Assert.That(file.CreationTime, Is.GreaterThan(beforeSave));
            Assert.That(file.Length, Is.EqualTo(content.Length));
        }
    }
}