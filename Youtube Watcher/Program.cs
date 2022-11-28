using System;
using System.Windows.Forms;
//Program
namespace Youtube_Watcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}