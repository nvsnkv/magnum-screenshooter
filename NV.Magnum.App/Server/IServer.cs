namespace NV.Magnum.App.Server
{
    public interface IServer
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}