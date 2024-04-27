using System.Windows.Forms;
using CefSharp;

namespace Webwatcher
{
    public class ConfigInterface
    {
        public TabWindow Parent;

        public ConfigInterface(TabWindow parent)
            => Parent = parent;

        public void SaveBasicConfig(string homepage, bool useDefaultHomepage)
        {
            var config = new ConfigBase(homepage, useDefaultHomepage);

            ConfigManager.Save(config);
        }

        public void SaveSearchEngineConfig(string searchEngine)
        {
            MessageBox.Show(searchEngine);
        }

        public void ShowDevTools()
            => Parent.WebBrowser.ShowDevTools();
    }
}