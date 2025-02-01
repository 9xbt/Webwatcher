using CefSharp;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Webwatcher
{
    internal class DownloadHandler : IDownloadHandler
    {
        public static List<Download> Downloads = new List<Download>();
        private TabWindow Parent;

        internal DownloadHandler(TabWindow parent)
            => Parent = parent;

        public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
        {
            return true;
        }

        public bool OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    Uri uri = new Uri(downloadItem.Url);
                    string filename = Path.GetFileName(uri.AbsolutePath).Replace("%20", " ");

                    Download download = new Download(0, false, downloadItem.Url, ConfigManager.DownloadPath + $@"\{filename}", DateTime.Now);
                    Downloads.Add(download);

                    callback.Continue(download.Path, false);
                    Parent.Invoke(new Action(async () =>
                    {
                        await Task.Delay(50);
                        Parent.OpenPage("webwatcher://downloads", ConfigManager.DownloadsURL);
                        Parent.WebBrowser.Back();
                    }));
                }
            }

            return true;
        }

        public async void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            int downloadIndex = GetDownloadIndex(downloadItem.FullPath);
            if (downloadIndex >= 0)
            {
                var download = Downloads[downloadIndex];

                if (download.Cancel)
                {
                    callback.Cancel();
                }

                if (download.Progress == 100)
                {
                    await Task.Delay(1000);
                    Downloads.Remove(download);
                    return;
                }

                download.Progress = downloadItem.PercentComplete;
                Downloads[downloadIndex] = download;
            }
        }

        public static int GetDownloadIndex(string path)
        {
            for (int i = 0; i < Downloads.Count; i++)
            {
                if (Downloads[i].Path == path)
                {
                    return i;
                }
            }
            return -1;
        }

        public static Download GetDownloadByPath(string path)
        {
            foreach (var download in Downloads)
            {
                if (download.Path == path)
                {
                    return download;
                }
            }
            return new Download(-1, false, "", "", DateTime.Now);
        }
    }
}