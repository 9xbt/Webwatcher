namespace Webwatcher
{
    public struct ConfigBase
    {
        internal ConfigBase(string homepage, bool useDefaultHomepage, string searchEngine)
        {
            Homepage = homepage;
            UseDefaultHomepage = useDefaultHomepage;
            SearchEngine = searchEngine;
        }

        internal string Homepage;
        internal bool UseDefaultHomepage;
        internal string SearchEngine;
    }
}