using System;
using System.IO;
using System.Diagnostics;
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
                Form form = new CrashForm();

                dlg.SetMarqueeProgressBar(true, 30);
                dlg.Show(form);

                dlg = new TaskDialog("webwatcher.exe has stopped working", "webwatcher.exe", "A problem caused the program to stop working correctly. Please close the program.")
                {
                    UseCommandLinks = true,
                    CustomButtons = new CustomButton[]
                    {
                        new CustomButton(CommonButtonResult.Cancel, "Close the program")
                    }
                };

                dlg.Show(form);
            }
        }
    }
}
