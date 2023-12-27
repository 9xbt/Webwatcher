using CefSharp;
using CefSharp.WinForms;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Webwatcher
{
    public partial class Browser : Form
    {
        private string version = "1.8.1";

        private string settingsPath = Application.StartupPath + @"\options.txt", homepage = "";
        public static string pageTitle;
        private bool updateStatus, enableVisualStyles, enableUpdates;

        public static ChromiumWebBrowser ChromiumPublic;

        // Startup

        public Browser(bool isChild)
        {
            InitializeComponent();
            LoadSettings(isChild);
            Looks();
            CheckRegistry();
            InitializeChromium();
            ChromiumPublic = Chromium;
        }

        private void InitializeChromium()
        {
            //Browser Settings
            CefSettings settings = new CefSettings
            {
                PersistSessionCookies = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.134 Safari/537.36 Webwatcher/1.8.1"
            };
            Environment.SetEnvironmentVariable("GOOGLE_API_KEY", "AIzaSyATornPGCHzlrg6QpocFceLkZQVvap0WOM");
            Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_ID", "580109901678-obrsaugnt1n15811rggr6a4rsg0ojrbh.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_SECRET", "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");
            Cef.Initialize(settings);
            
            Invoke(new Action(() =>
            {
                Chromium.LoadUrl(homepage);
                AddressBar.Text = homepage;
                AddressBar.ForeColor = Color.Black;
            }));
        }

        [DllImport("DwmApi")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        private void CheckRegistry()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize"))
                if (!Convert.ToBoolean(key.GetValue("AppsUseLightTheme", RegistryValueOptions.None)) && key != null && DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                    DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        private void LoadSettings(bool isChild)
        {
            if (!File.Exists(settingsPath))
                File.WriteAllText(settingsPath, "showpagestatus=false\nenableanimations=true\nenableupdates=true\nhomepage=https://google.com");

            foreach (string line in File.ReadAllLines(settingsPath))
            {
                if (line.Trim().ToLower().StartsWith("showpagestatus=") && line.Contains("true"))
                    updateStatus = true;

                if (!isChild && line.Trim().ToLower().StartsWith("enableanimations=") && line.Contains("true"))
                    enableVisualStyles = true;

                if (line.Trim().ToLower().StartsWith("enableupdates=") && line.Contains("true"))
                    enableUpdates = true;

                if (line.Trim().ToLower().StartsWith("homepage="))
                    homepage = line.Split('=')[1];
            }
        }

        private void Looks()
        {
            VersionLabel.Text = "Webwatcher " + version;
            Text = "Webwatcher " + version;
            if (enableVisualStyles)
            {
                Opacity = 0;
                ClientSize = new Size(0, 0);
                OpacityTimer.Start();
                WidthTimer.Start();
            }
            else ClientSize = new Size(1024, 768);
        }

        // Animations

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (Fullscreen.Checked)
            {
                if (Cursor.Position.Y <= 35 && MenuBar.Top < 0)
                {
                    MenuBar.Top += 2;
                    Chromium.Top += 2;
                    Chromium.Height -= 2;
                }
                if (Cursor.Position.Y >= 35 && MenuBar.Top >= -23)
                {
                    MenuBar.Top--;
                    Chromium.Top--;
                    Chromium.Height++;
                }
            }
        }

        private void OpacityTimer_Tick(object sender, EventArgs e)
        {
            Opacity += 0.01;
            if (Opacity == 1)
                OpacityTimer.Stop();
        }

        private void WidthTimer_Tick(object sender, EventArgs e)
        {
            Width += 20;
            if (Width > 250 && Width < 270)
                HeightTimer.Start();

            if (Width > 1019)
            {
                Width = 1024;
                MinimumSize = new Size(500, 400);
                Chromium.Visible = true;
                WidthTimer.Stop();
            }
        }

        private void HeightTimer_Tick(object sender, EventArgs e)
        {
            Height += 20;
            if (Height > 768)
            {
                Height = 768;
                HeightTimer.Stop();
            }
        }

        // Chromium

        private void Chromium_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (Chromium.CanGoBack)
                {
                    Return.Enabled = true;
                    Forward.Enabled = false;
                }
                else
                {
                    Return.Enabled = false;
                    Forward.Enabled = true;
                }
            }));
        }

        private void Chromium_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                Text = e.Title + " - Webwatcher " + version;
                pageTitle = e.Title;
            }));
        }

        private void Chromium_LoadError(object sender, LoadErrorEventArgs e) => Chromium.LoadUrl(Links.ErrorPage);

        private void Chromium_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            var URL = e.Address;
            if (URL.EndsWith("saved=true"))
            {
                URL = URL.Split('?')[1];
                var contents = URL.Split('&');
                contents[4] = "";
                File.WriteAllLines(settingsPath, contents);
            }
            else if (!URL.EndsWith(".htm") || !URL.EndsWith(".html") || !URL.EndsWith(".php") || !URL.EndsWith(".asp"))
            {

            }
        }

        // Form elements

        private void URL_Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                var URL = AddressBar.Text.Trim().ToLower();

                if (URL.StartsWith("webwatcher://"))
                {
                    if (URL == ("webwatcher://about"))
                        Chromium.LoadUrl(Links.AboutPage);
                    else if (URL == ("webwatcher://changelog"))
                        Chromium.LoadUrl(Links.ChangelogPage);
                    else if (URL.StartsWith("webwatcher://settings"))
                        Chromium.LoadUrl(Links.SettingsPage + "?showpagestatus=" + updateStatus.ToString().ToLower() + "&enableanimations=" + enableVisualStyles.ToString().ToLower() + "&enableupdates=" + enableUpdates.ToString().ToLower() + "&homepage=" + homepage.ToLower());
                    else if (URL ==("webwatcher://error"))
                        Chromium.LoadUrl(Links.ErrorPage);
                }
                else Chromium.LoadUrl(AddressBar.Text.Trim());
            }
        }

        private void Google_Search_Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                Chromium.LoadUrl("https://google.com/search?q=" + GoogleBar.Text.Trim());
        }

        private void Google_Search_Box_Enter(object sender, EventArgs e)
        {
            if (GoogleBar.Text == "Google search")
            {
                GoogleBar.Text = "";
                GoogleBar.ForeColor = Color.Black;
            }
        }

        private void Google_Search_Box_Leave(object sender, EventArgs e)
        {
            if (GoogleBar.Text == "")
            {
                GoogleBar.Text = "Google search";
                GoogleBar.ForeColor = Color.Silver;
            }
        }

        private void URL_Box_Leave(object sender, EventArgs e)
        {
            if (AddressBar.Text == "")
            {
                AddressBar.Text = "Address";
                AddressBar.ForeColor = Color.Silver;
            }
        }

        private void URL_Box_Enter(object sender, EventArgs e)
        {
            if (AddressBar.Text == "Address")
            {
                AddressBar.Text = "";
                AddressBar.ForeColor = Color.Black;
            }
        }

        private void NewTab_Click(object sender, EventArgs e) => Process.Start(Application.StartupPath + @"\Webwatcher.exe", "-child");

        private void Settings_Click(object sender, EventArgs e) => Chromium.LoadUrl(Links.SettingsPage + "?showpagestatus=" + updateStatus.ToString().ToLower() + "&enableanimations=" + enableVisualStyles.ToString().ToLower() + "&enableupdates=" + enableUpdates.ToString().ToLower() + "&homepage=" + homepage.ToLower());

        private void Button_Forward_Click(object sender, EventArgs e) => Chromium.Forward();

        private void Button_Back_Click(object sender, EventArgs e) => Chromium.Back();

        private void FullscreenCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (Fullscreen.Checked)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                Location = new Point(0, 0);
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                ClientSize = new Size(1024, 768);
            }
        }

        private void Link_Changelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Chromium.LoadUrl(Links.ChangelogPage);

        private void DevToolsButton_Click(object sender, EventArgs e) => Chromium.ShowDevTools();
    }
}
