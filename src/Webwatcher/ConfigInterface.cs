using CefSharp;
using System;
using System.Diagnostics;

namespace Webwatcher
{
    public class ConfigInterface
    {
        public TabWindow Parent;

        public ConfigInterface(TabWindow parent)
            => Parent = parent;

        public void SaveBasicConfig(string homepage, bool useDefaultHomepage, string searchEngine)
        {
            var config = new ConfigBase(homepage, useDefaultHomepage, searchEngine, ConfigManager.Config.UseHardwareAccel);

            ConfigManager.Save(config);
        }

        public void SaveAdvancedConfig(bool useHardwareAccel)
        {
            var config = new ConfigBase(ConfigManager.Config.Homepage, ConfigManager.Config.UseDefaultHomepage, ConfigManager.Config.SearchEngine, useHardwareAccel);

            ConfigManager.Save(config);
        }

        public void ShowDevTools()
            => Parent.WebBrowser.ShowDevTools();
    }
}