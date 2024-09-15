using System;
using System.Windows.Forms;
using EasyTabs;

namespace Webwatcher
{
    public static class Program
    {
        public const string Version = "1.9.3";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			Browser testApp = new Browser();
	        
			testApp.Tabs.Add(new TitleBarTab(testApp)
			{
				Content = new TabWindow
			    {
				    Text = "New Tab"
				}
			});
			testApp.SelectedTabIndex = 0;

			TitleBarTabsApplicationContext applicationContext = new TitleBarTabsApplicationContext();
			applicationContext.Start(testApp);

            Application.Run(applicationContext);
        }
    }
}
