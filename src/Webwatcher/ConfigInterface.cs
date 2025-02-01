using CefSharp;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

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

        public void ShowDownloadsFolder()
            => Process.Start("explorer", ConfigManager.DownloadPath);

        public void ShowDownloadedFile(string file)
            => Process.Start("explorer", $"/select,\"{ConfigManager.DownloadPath + @"\" + file}\"");

        public void UpdateDownloads()
        {
            foreach (var download in DownloadHandler.Downloads)
            {
                Parent.UpdateDownload(download);
            }
        }

        public void RemoveDownload(string file)
        {
            for (int i = 0; i < DownloadHandler.Downloads.Count; i++)
            {
                var download = DownloadHandler.Downloads[i];
                var filename = Path.GetFileName(download.Path);

                if (filename == file)
                {
                    download.Cancel = true;
                    DownloadHandler.Downloads[i] = download;
                    return;
                }
            }
        }
    }
}