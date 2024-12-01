using System;

namespace Webwatcher
{
    internal struct Download
    {
        internal double Progress;
        internal bool Done;
        internal string URL;
        internal string Path;
        internal DateTime Date;

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