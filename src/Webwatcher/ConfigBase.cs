namespace Webwatcher
{
    public struct ConfigBase
    {
        internal ConfigBase(string homepage, bool useDefaultHomepage)
        {
            Homepage = homepage;
            UseDefaultHomepage = useDefaultHomepage;
        }

        internal string Homepage;
        internal bool UseDefaultHomepage;
    }
}