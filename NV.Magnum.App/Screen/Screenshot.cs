namespace NV.Magnum.App.Screen
{
    public class Screenshot:IScreenshot
    {
        public Screenshot(byte[] content)
        {
            Content = content;
        }

        public byte[] Content { get; private set; }
    }
}