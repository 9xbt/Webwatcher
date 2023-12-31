using System;
using System.Windows.Forms;

namespace Webwatcher
{
    internal static class Program
    {
        #region Methods

        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">Program arguments</param>
        [STAThread]
        static void Main(string[] args)
        {
            bool isChild = false;
            foreach (string arg in args) isChild = arg == "--child";

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Browser(isChild));
        }

        #endregion
    }
}
