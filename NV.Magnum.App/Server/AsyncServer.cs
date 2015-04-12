using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;

namespace NV.Magnum.App.Server
{
    internal class AsyncServer : IServer
    {
        private readonly HttpListener _listener;
        private readonly Semaphore _workerSemaphore;
        private readonly string _wwwRoot;

        public AsyncServer(int port, string wwwRoot, string hostname, int workerLimit)
        {
            if (port < 1) throw new ArgumentOutOfRangeException("port", "Invalid port number!");
            if (!Directory.Exists(wwwRoot)) throw new ArgumentException("Content directory doesn't exist!","wwwRoot");
            if (string.IsNullOrEmpty(hostname)) throw new ArgumentException("Invalid hostname", hostname);

            _wwwRoot = wwwRoot;
            _listener = new HttpListener();
            _listener.Prefixes.Add(string.Format("http://{0}:{1}/",hostname,port));
            _workerSemaphore = new Semaphore(workerLimit, workerLimit);
        }

        public bool IsRunning { get { return _listener.IsListening; }}

        public void Start()
        {
            if (_listener.IsListening)
                return;

            _listener.Start();
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var context = await _listener.GetContextAsync();

                    _workerSemaphore.WaitOne();

                    #pragma warning disable 4014
                    //Here I want to start new Task
                    Task.Factory.StartNew(() =>
                    #pragma warning restore 4014
                    {
                        try
                        {
                            ProcessRequest(context);
                        }
                        catch (HttpListenerException e)
                        {
                            // it usually appears after _listener.Stop();
                            // I have no idea how to cancel _listener.BeginGetContext()
                        }
                        finally
                        {
                           _workerSemaphore.Release();
                        }
                    });

                }
            });

        }

        [LogException]
        public void Stop()
        {
            if (!_listener.IsListening)
                return;

            _listener.Stop();
        }

        [Log]
        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            if (!CheckRequest(context.Request))
            {
                RespondError(context.Response);
            }
            else
            {
                RespondFile(context.Request.Url.AbsolutePath.Trim('/'), context.Response);
            }
        }

        [Log]
        private void RespondFile(string absolutePath, HttpListenerResponse response)
        {
            var file = new FileInfo(Path.Combine(_wwwRoot, absolutePath));
            
            using (var responseStream = response.OutputStream)
            using (var contentStream = file.OpenRead())
            {
                contentStream.CopyTo(responseStream, 1*1024*1024);
            }
        }

        [Log]
        private void RespondError(HttpListenerResponse response)
        {
            response.StatusCode = 404;
            response.Close();
        }

        [LogException]
        private bool CheckRequest(HttpListenerRequest request)
        {
            return request.HttpMethod.ToUpperInvariant() == "GET" 
                   && File.Exists(Path.Combine(_wwwRoot, request.Url.AbsolutePath.Trim('/')));
        }
    }
}