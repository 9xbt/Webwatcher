using CefSharp;
using CefSharp.WinForms;
using EasyTabs;

namespace TestApp
{
	public partial class Browser : TitleBarTabs
    {
        public Browser()
        {
            InitializeComponent();

            AeroPeekEnabled = true;
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
