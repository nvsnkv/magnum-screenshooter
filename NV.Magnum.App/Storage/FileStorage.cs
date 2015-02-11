﻿using System;
using System.IO;
using NV.Magnum.App.Screen;

namespace NV.Magnum.App.Storage
{
    internal class FileStorage : IStorage
    {
        private readonly string _path;

        public FileStorage(string path)
        {
            if (!Directory.Exists(path))
                throw new ArgumentException("Storage directory was not found!","path");

            _path = path;
        }

        public void Store(IScreenshot screenshot)
        {
            var name = Guid.NewGuid().ToString();

            File.WriteAllBytes(Path.Combine(_path, name),screenshot.Content);
        }
    }
}