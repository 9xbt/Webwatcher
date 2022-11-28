using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//Form3
namespace Youtube_Watcher
{
    public partial class Form3 : Form
    {
        string setting1;
        string defaultUrl = "https://google.com/webhp";
        string path = Application.StartupPath + "\\options.txt";
        string optionsFile;
        bool canSave = false;
        public Form3() => Startup();

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        private void Startup()
        {
            if (Form1.darkmode)
            {
                if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                    DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
            }

            InitializeComponent();
            CheckFile();
            canSave = false;
        }
        private void CheckFile()
        {
            if (!File.Exists(Application.StartupPath + "\\options.txt"))
            {
                File.Create(Application.StartupPath + "\\options.txt");
                ReadOptions();
            }
            else
                ReadOptions();
        }
        private void ReadOptions()
        {
            if (new FileInfo(Application.StartupPath + "\\options.txt").Length > 0)
            {
                optionsFile = File.ReadLines(Application.StartupPath + "\\options.txt").Skip(0).Take(1).First();
                textBox1.Text = optionsFile;
            }
        }
        private void button1_Click(object sender, EventArgs e) => SaveSettings();
        private void SaveSettings()
        {
            if (canSave == true)
            {
                setting1 = textBox1.Text;
                if (setting1.Length == 0)
                {
                    DialogResult result;
                    result = MessageBox.Show("You did not enter a home page, use default one?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        setting1 = defaultUrl;
                        File.WriteAllText(path, setting1 + "\n");
                    }
                }
                else
                {
                    File.WriteAllText(path, setting1 + "\n");
                }
                canSave = false;
            }
        }
        private void button2_Click(object sender, EventArgs e) => Close();
        private void timer1_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.01;
            if (Opacity < 0.7)
                timer1.Stop();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            Opacity += 0.01;
            if (Opacity == 1)
                timer2.Stop();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            canSave = true;
            button1.Enabled = true;
            if (e.KeyChar == (char)Keys.Enter && canSave == true)
            {
                SaveSettings();
                MessageBox.Show("Setting saved.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}