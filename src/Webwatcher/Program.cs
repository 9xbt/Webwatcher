using System;
using System.Windows.Forms;
using EasyTabs;
using WindowsFormsAero.TaskDialog;

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
            try
            {
                ConfigManager.CheckForUpdates();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Browser testApp = new Browser();

                testApp.Tabs.Add(new TitleBarTab(testApp)
                {
                    Content = new TabWindow(ConfigManager.ShouldUpdate ? "https://github.com/9xbt/Webwatcher/releases/latest" : null)
                    {
                        Text = "New Tab"
                    }
                });
                testApp.SelectedTabIndex = 0;

                TitleBarTabsApplicationContext applicationContext = new TitleBarTabsApplicationContext();
                applicationContext.Start(testApp);

                Application.Run(applicationContext);
            }
            catch (Exception ex)
            {
                Crash = ex;

                TaskDialog dlg = new TaskDialog("Unable to start Webwatcher", "Webwatcher");
                dlg.CommonIcon = CommonIcon.Stop;
                dlg.Content = ex.Message;
                dlg.UseCommandLinks = true;
                dlg.CustomButtons = new CustomButton[] {
                    new CustomButton(9, "More information"),
                    new CustomButton(10, "Copy error to clipboard"),
                    new CustomButton(CommonButtonResult.Cancel, "Cancel")
                };

                dlg.ButtonClick += new EventHandler<ClickEventArgs>(CrashHandler_ButtonClick);

                TaskDialogResult results = dlg.Show();
            }
        }

        private static Exception Crash;

        private static void CrashHandler_ButtonClick(object sender, ClickEventArgs e)
        {
            switch (e.ButtonID)
            {
                case 9:
                    MessageBox.Show(Crash.ToString(), "Webwatcher - Stack Trace", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 10:
                    Clipboard.SetText(Crash.ToString());
                    break;
            }
        }
    }
}
