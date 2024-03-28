using EasyTabs;
using CefSharp;
using CefSharp.WinForms;
using System.Windows.Forms;

namespace Webwatcher
{
	public partial class Browser : TitleBarTabs
    {
        public Browser()
        {
            InitializeComponent();

            TabRenderer = new ChromeTabRenderer(this);
            Icon = Resources.Webwatcher;
        }

        static Browser()
        {
            CefSettings cefSettings = new CefSettings();
            Cef.Initialize(cefSettings);
        }

        public override TitleBarTab CreateTab()
        {
            return new TitleBarTab(this)
            {
                Content = new TabWindow
                {
                    Text = "New Tab"
                }
            };
        }
    }
}
