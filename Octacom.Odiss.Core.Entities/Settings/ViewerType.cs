namespace Octacom.Odiss.Core.Entities.Settings
{
    public enum ViewerType
    {
        None = 0,
        Default = 1, // Default is the custom js viewer
        BrowserNative = 2, // Use browser native viewer (some browsers are not compatible with Adobe PDF)
        ByBrowser = 3, // If the browser is IE, than use Browser Native viewer, otherwise, use the custom js viewer
        Custom = 4
    }
}
