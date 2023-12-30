using System;
using System.Windows.Forms;

namespace Webwatcher
{
    internal static class Program
    {
        /// <summary>
        /// Webwatcher entry point
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            bool isChild = false;
            foreach (string arg in args) isChild = arg.Contains("-child");

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Browser(isChild));
        }
    }
}
