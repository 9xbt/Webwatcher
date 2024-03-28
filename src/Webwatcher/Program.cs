using System;
using EasyTabs;
using System.Windows.Forms;

namespace Webwatcher
{
    public static class Program
    {
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
