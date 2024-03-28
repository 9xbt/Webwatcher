using System.Windows.Forms;

namespace Webwatcher
{
    public class JavascriptInterface
    {
        internal TabWindow Parent;

        public JavascriptInterface(TabWindow parent)
            => Parent = parent;

        public void SaveConfig(string homepage, bool useDefaultHomepage)
        {
            var config = new ConfigBase(homepage, useDefaultHomepage);

            ConfigLoader.Save(config);
        }

        public void ShowDevTools()
            => Parent.ShowDevTools();
    }
}