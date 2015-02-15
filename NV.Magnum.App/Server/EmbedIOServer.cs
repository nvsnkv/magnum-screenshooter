using System;
using System.IO;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;

namespace NV.Magnum.App.Server
{
    internal class EmbedIOServer : IServer, IDisposable
    {
        private WebServer _server;
        private readonly string _prefix;
        private readonly string _contentDir;

        public EmbedIOServer(int port, string contentDir)
        {
            if (port < 1)
                throw new ArgumentOutOfRangeException("port","Port number should be positive!");

            if (!Directory.Exists(contentDir))
                throw new ArgumentException("COntent directory doesn't exist!","contentDir");

            _prefix = string.Format("http://+:{0}/", port);
            _contentDir = contentDir;
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (IsRunning)
                return;

            _server = new WebServer(_prefix);
            _server.RegisterModule(new StaticFilesModule(_contentDir) { UseRamCache = true, MaxRamCacheFileSize = 20 * 1024 * 1024 });
            _server.RunAsync();

            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            if (_server != null)
            {
                _server.Dispose();
                _server = null;
            }
            
            IsRunning = false;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_server != null)
            {
                _server.Dispose();
                _server = null;
            }
        }
    }
}