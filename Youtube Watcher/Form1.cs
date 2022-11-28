using CefSharp;
using CefSharp.WinForms;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
//Main Form
namespace Youtube_Watcher
{
    public partial class Form1 : Form
    {
        public ChromiumWebBrowser chromeBrowser;
        public string version = "1.8"; //Version
        public bool prerelease = F; //Pre-release tag
        private const bool F = false;
        private const bool T = true;
        public string address;
        public string optionsFile;
        int x = 1024;
        int y = 768;
        string setting1;
        string setting2;
        string setting3;
        string setting4;
        readonly string path = Application.StartupPath + "\\options.txt";
        public static Form1 instance;
        public ChromiumWebBrowser browser1;
        string StatusLabelText = " Status: ?";
        string temp;
        public static bool darkmode;

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        public Form1()
        {
            CheckRegistry();
            CheckIfBetaVersion();
            InitializeComponent();
            InitializeOptions();
            InitializeChromium();
        }

        private void CheckRegistry()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize"))
            {
                if (key != null)
                {
                    object lightmode = key.GetValue("AppsUseLightTheme", RegistryValueOptions.None);
                    if (!Convert.ToBoolean(lightmode))
                    {
                        darkmode = true;
                        if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                            DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
                    }
                    else
                    {
                        darkmode = false;
                    }
                }
            }
        }

        private void CheckIfBetaVersion()
        {
            if (prerelease)
            {
                temp = version;
                version = "Development Build " + temp;
                ClearTemp();
            }
        }

        private void InitializeOptions()
        {
            if (!File.Exists(Application.StartupPath + "\\options.txt"))
            {
                MessageBox.Show("Settings file not existing, settings will be reset.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                WriteDefaultOptions();
            }

            else if (File.Exists(Application.StartupPath + "\\options.txt"))
            {
                if (new FileInfo(Application.StartupPath + "\\options.txt").Length > 0)
                {
                    //Options reading
                    optionsFile = File.ReadLines(Application.StartupPath + "\\options.txt").Skip(0).Take(1).First();
                    string defaultUrl = optionsFile;
                    chromiumWebBrowser1.LoadUrl(defaultUrl);
                }
                else
                {
                    MessageBox.Show("Settings file not existing, settings will be reset.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WriteDefaultOptions();
                }
            }
        }

        private void InitializeChromium()
        {
            //Folder management
            Directory.CreateDirectory(Application.StartupPath + "\\Cache");
            Directory.CreateDirectory(Application.StartupPath + "\\Cache\\RootCache");

            //Browser Settings
            CefSettings settings = new CefSettings
            {
                PersistSessionCookies = true,
                CachePath = Application.StartupPath + "\\Cache",
                RootCachePath = Application.StartupPath + "\\Cache\\RootCache",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion
            };
            Environment.SetEnvironmentVariable("GOOGLE_API_KEY", "AIzaSyATornPGCHzlrg6QpocFceLkZQVvap0WOM");
            Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_ID", "580109901678-obrsaugnt1n15811rggr6a4rsg0ojrbh.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_SECRET", "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");
            Cef.Initialize(settings);
        }

        private void ClearTemp() => temp = null;

        private void button2_Click(object sender, EventArgs e) => chromiumWebBrowser1.Forward();

        private void button1_Click(object sender, EventArgs e) => chromiumWebBrowser1.Back();

        private void timer1_Tick(object sender, EventArgs e) => chromiumWebBrowser1.LoadUrl(textBox1.Text);

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => Cef.Shutdown();

        private void button4_Click(object sender, EventArgs e) => Process.Start(Application.StartupPath + "\\Webwatcher.exe");

        private void button6_Click(object sender, EventArgs e) => chromiumWebBrowser1.ShowDevTools();

        private async void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            timer9.Start();
            Form2 frm2 = new Form2();
            frm2.ShowDialog();
            timer9.Stop();
            timer10.Start();
            await Task.Delay(1000);
            timer10.Stop();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                chromiumWebBrowser1.LoadUrl(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panel1.Height == 44)
            {
                panel1.Height = 23;
                chromiumWebBrowser1.Height += 21;
                chromiumWebBrowser1.Top -= 21;
            }
            else if (panel1.Height != 44)
            {
                panel1.Height = 44;
                chromiumWebBrowser1.Height -= 21;
                chromiumWebBrowser1.Top += 21;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Startup
            panel1.Height = 23;
            instance = this;
            label1.Text = $"Webwatcher {version}";
            Text = label1.Text;
            Width = 1;
            Height = 1;
            timer4.Start();
            timer3.Start();
            timer1.Start();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            { //Enable
                StoreSize();
                checkBox1.Enabled = F;
                timer5.Start();
            }
            else
            { //Disable
                checkBox1.Enabled = F;
                timer7.Start();
            }
        }

        private void chromiumWebBrowser1_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                textBox1.Text = chromiumWebBrowser1.Address;
                StatusLabelText = " Status: Loading...";
                address = chromiumWebBrowser1.Address;
            }));
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            Height += 20;
            if (Height > 768)
            {
                Height = 768;
                timer2.Stop();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            Opacity += 0.01;
            if (Opacity == 1)
                timer4.Stop();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Width += 20;
            if (Width > 250 && this.Width < 270)
            {
                timer2.Start();
            }
            if (Width > 1024)
            {
                Width = 1024;
                MinimumSize = new System.Drawing.Size(500, 400);
                timer3.Stop();
                chromiumWebBrowser1.Visible = true;
            }
        }

        private void WriteDefaultOptions()
        {
            setting1 = "https://google.com/webhp";
            setting2 = "false";
            setting3 = null;
            setting4 = null;
            File.WriteAllText(path, setting1 + "\n" + setting2 + "\n" + setting3 + "\n" + setting4);
        }

        private void timer1_Tick_2(object sender, EventArgs e)
        {
            StatusLabel.Text = StatusLabelText;
            if (textBox1.TextLength > 0)
                Text = $"Webwatcher {version} ({address})";
            if (chromiumWebBrowser1.CanGoBack)
                button1.Enabled = T;
            else
                button1.Enabled = F;

            if (chromiumWebBrowser1.CanGoForward)
                button2.Enabled = T;
            else
                button2.Enabled = F;
            if (prerelease)
            {
                infoLabel.Visible = true;
                infoLabel.Left += 10;
                if (infoLabel.Left > border1.Left)
                    infoLabel.Left = -316;
            }
            else if (!prerelease)
            {
                infoLabel.Visible = false;
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.05;
            if (Opacity < 0.01)
            {
                timer6.Start();
                timer5.Stop();
            }
        }

        private void StoreSize()
        {
            x = Width;
            y = Height;
        }

        private async void timer6_Tick(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            checkBox1.Enabled = T;
            await Task.Delay(100);
            Opacity += 0.05;
            if (Opacity == 1)
                timer6.Stop();
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.05;
            if (Opacity < 0.01)
            {
                timer8.Start();
                timer7.Stop();
            }
        }

        private async void timer8_Tick(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;
            Width = x;
            Height = y;
            checkBox1.Enabled = T;
            await Task.Delay(100);
            Opacity += 0.05;
            if (Opacity == 1)
                timer8.Stop();
        }

        private void timer9_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.01;
            if (Opacity < 0.7)
                timer9.Stop();
        }

        private void timer10_Tick(object sender, EventArgs e)
        {
            Opacity += 0.01;
            if (Opacity > 0.99)
                timer10.Stop();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            timer9.Start();
            Form3 frm3 = new Form3();
            frm3.ShowDialog();
            timer9.Stop();
            timer10.Start();
            await Task.Delay(1000);
            timer10.Stop();
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            timer9.Start();
            Form4 frm4 = new Form4();
            frm4.ShowDialog();
            timer9.Stop();
            timer10.Start();
            await Task.Delay(1000);
            timer10.Stop();
        }

        private void chromiumWebBrowser1_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (StatusLabel.Text == " Status: Loading...")
                StatusLabelText = " Status: Loaded!";
        }
    }
}