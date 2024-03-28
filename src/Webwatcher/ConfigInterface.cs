using CefSharp;

namespace Webwatcher
{
    public class ConfigInterface
    {
        public TabWindow Parent;

        public ConfigInterface(TabWindow parent)
            => Parent = parent;

        public void SaveConfig(string homepage, bool useDefaultHomepage)
        {
            var config = new ConfigBase(homepage, useDefaultHomepage);

            ConfigLoader.Save(config);
        }

        public void ShowDevTools()
            => Parent.WebBrowser.ShowDevTools();
    }
}