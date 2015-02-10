using NV.Magnum.App.Screen;

namespace NV.Magnum.App.Storage
{
    public interface IStorage
    {
        void Store(IScreenshot screenshot);
    }
}