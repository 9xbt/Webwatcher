using System;

namespace Webwatcher
{
    public struct Download
    {
        public double Progress;
        public bool Done;
        public bool Cancel;
        public string URL;
        public string Path;
        public DateTime Date;

        public Download(double progress, bool done, string url, string path, DateTime date)
        {
            Progress = progress;
            Done = done;
            Cancel = false;
            URL = url;
            Path = path;
            Date = date;
        }
    }
}