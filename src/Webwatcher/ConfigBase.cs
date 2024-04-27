namespace Webwatcher
{
    public struct ConfigBase
    {
        internal ConfigBase(string homepage, bool useDefaultHomepage, string searchEngine, bool useHardwareAccel)
        {
            Homepage = homepage;
            UseDefaultHomepage = useDefaultHomepage;
            SearchEngine = searchEngine;
            UseHardwareAccel = useHardwareAccel;
        }

        internal string Homepage;
        internal bool UseDefaultHomepage;
        internal string SearchEngine;
        internal bool UseHardwareAccel;
    }
}