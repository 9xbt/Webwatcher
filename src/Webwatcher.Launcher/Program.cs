using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using WindowsFormsAero.TaskDialog;

namespace Webwatcher.Launcher
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

        loop:
            Directory.SetCurrentDirectory(@"..\..\..\Webwatcher\bin\Debug");
            var browser = Process.Start("webwatcher.exe");

            browser.WaitForExit();

            if (browser.ExitCode == 0xFFFF)
            {
                goto loop;
            }
            else if (browser.ExitCode != 0)
            {
                TaskDialog dlg = new TaskDialog("webwatcher.exe has stopped working", "webwatcher.exe", "Windows is checking for a solution to the problem...", CommonButton.Cancel);
                dlg.SetMarqueeProgressBar(true, 30);

                dlg.Show(new CrashForm());
            }
        }
    }
}
