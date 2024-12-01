using CefSharp;
using System;
using System.Collections.Generic;

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
                    Download download = new Download(0, false, downloadItem.Url, ConfigManager.DownloadPath, DateTime.Now);
                    Downloads.Add(download);
                    
                    Parent.Invoke(new Action(() => Parent.OpenDownloads()));
                }
            }

            return true;
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {

        }
    }
}
