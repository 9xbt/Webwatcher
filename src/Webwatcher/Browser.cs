using EasyTabs;
using CefSharp;
using CefSharp.WinForms;

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
            CefSettings cefSettings = new CefSettings
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/" + Cef.ChromiumVersion + " Safari/537.36 Webwatcher/1.9",
                PersistSessionCookies = true,
                CachePath = ConfigManager.ConfigPath
            };
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
