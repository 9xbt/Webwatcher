using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//Form4
namespace Youtube_Watcher
{
    public partial class Form4 : Form
    {
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        public Form4()
        {
            if (Form1.darkmode)
            {
                if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                    DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
            }
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) => Close();
    }
}