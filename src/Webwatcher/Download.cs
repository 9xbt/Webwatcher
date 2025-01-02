using System;

namespace Webwatcher
{
    public struct Download
    {
        public double Progress;
        public bool Done;
        public string URL;
        public string Path;
        public DateTime Date;

        public Download(double progress, bool done, string url, string path, DateTime date)
        {
            Progress = progress;
            Done = done;
            URL = url;
            Path = path;
            Date = date;
        }
    }
}