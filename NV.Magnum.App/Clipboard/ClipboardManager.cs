namespace NV.Magnum.App.Clipboard
{
    internal class ClipboardManager : IClipboardManager
    {
        public void Set(string text)
        {
            System.Windows.Clipboard.SetText(text);
        }
    }
}