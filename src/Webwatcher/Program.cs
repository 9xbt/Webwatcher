using System;
using System.Windows.Forms;

namespace Webwatcher
{
    internal static class Program
    {
        static bool isChild;

        [STAThread]
        static void Main(string[] args)
        {
            foreach (string arg in args) isChild = arg.Contains("-child");
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Browser(isChild));
        }
    }
}
