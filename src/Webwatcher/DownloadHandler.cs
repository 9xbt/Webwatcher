﻿using CefSharp;
using System;
using System.IO;
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
                    Uri uri = new Uri(downloadItem.Url);
                    string filename = Path.GetFileName(uri.AbsolutePath).Replace("%20", " ");
                    Console.WriteLine("Filename: " + filename);

                    Download download = new Download(0, false, downloadItem.Url, ConfigManager.DownloadPath + $@"\{filename}", DateTime.Now);
                    Downloads.Add(download);

                    callback.Continue(download.Path, false);
                    Parent.Invoke(new Action(() => Parent.OpenPage("webwatcher://downloads", ConfigManager.DownloadsURL)));
                }
            }

            return true;
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            Console.WriteLine(downloadItem.FullPath);

            int downloadIndex = GetDownloadIndex(downloadItem.FullPath);
            if (downloadIndex >= 0)
            {
                var download = Downloads[downloadIndex];
                download.Progress = downloadItem.PercentComplete;
                Downloads[downloadIndex] = download;
            }

            Console.WriteLine("Download progress " + downloadItem.PercentComplete);

            if (downloadItem.IsComplete)
            {
                Downloads.Remove(GetDownloadByPath(downloadItem.FullPath));
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