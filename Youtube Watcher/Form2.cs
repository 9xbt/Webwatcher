using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
//Form2
namespace Youtube_Watcher
{
    public partial class Form2 : Form
    {
        #region Startup

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        public Form2() => InitializeComponent();
        private void Form2_Load(object sender, EventArgs e)
        {
            if (Form1.darkmode)
            {
                if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                    DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
            }

            Height = 0;
            Thread.Sleep(1);
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Height += 20;
            Opacity += 0.05;
            if (Height > 480)
            {
                timer1.Stop();
                Height = 480;
            }
        }
        #endregion
    }
}