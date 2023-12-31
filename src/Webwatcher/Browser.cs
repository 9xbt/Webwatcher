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
    /// <summary>
    /// <see cref="Browser"/> form
    /// </summary>
    internal partial class Browser : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="Browser"/>
        /// </summary>
        /// <param name="isChild">Is browser a child window</param>
        public Browser(bool isChild)
        {
            InitializeComponent();
            LoadSettings(isChild);
            InitUI();
            DarkTitleBar();
            InitializeChromium();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize Chromium instance
        /// </summary>
        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings
            {
                PersistSessionCookies = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.134 Safari/537.36 Webwatcher/" + Version
            };

            if (!Cef.Initialize(settings))
            {
                MessageBox.Show("CEF failed to initialize! Expect to not be able to log into Google accounts", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Log("CEF failed to initialize! Expect to not be able to log into Google accounts");
            }
            else Logger.Log("Chromium initialized successfully");

            Invoke(new Action(() =>
            {
                Logger.Log("Loading homepage...");
                Chromium.LoadUrl(browserSettings.homepage);
                AddressBar.Text = browserSettings.homepage;
                AddressBar.ForeColor = Color.Black;
                Logger.Log("Done loading homepage!");
            }));
            
            Logger.Log("Everything initialized successfully, welcome back to Webwatcher!");
        }

        [DllImport("DwmApi")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        /// <summary>
        /// Makes the title bar dark depending on the system settings
        /// </summary>
        private void DarkTitleBar()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize"))
            {
                if (!Convert.ToBoolean(key.GetValue("AppsUseLightTheme", RegistryValueOptions.None)) && key != null && DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                {
                    DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
                }
            }
        }

        /// <summary>
        /// Loads the browser settings
        /// </summary>
        /// <param name="isChild">Is browser a child window</param>
        private void LoadSettings(bool isChild)
        {
            if (!File.Exists(settingsPath)) File.WriteAllText(settingsPath,
                                                             "showpagestatus=false\n" +
                                                             "enableanimations=true\n" +
                                                             "enableupdates=true\n" +
                                                             "homepage=https://google.com");

            foreach (string line in File.ReadAllLines(settingsPath))
            {
                switch (line.Trim().ToLower())
                {
                    // replace a with line if it doesn't work
                    
                    case { } a when a.StartsWith("showpagestatus="):
                        browserSettings.updateStatus = a.EndsWith("true");
                        break;

                    case { } a when a.StartsWith("enableanimations="):
                        browserSettings.enableVisualStyles = a.EndsWith("true") && !isChild;
                        break;

                    case { } a when a.StartsWith("enableupdates="):
                        browserSettings.enableUpdates = a.EndsWith("true");
                        break;

                    case { } a when a.StartsWith("homepage="):
                        browserSettings.homepage = a.Split('=')[1];
                        break;
                }
            }
        }

        private void InitUI()
        {
            VersionLabel.Text = "Webwatcher " + Version;
            Text = "Webwatcher " + Version;
            if (browserSettings.enableVisualStyles)
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
                Text = e.Title + " - Webwatcher " + Version;
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
                // TODO: make it download the file
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
                        Chromium.LoadUrl(Links.SettingsPage + "?showpagestatus=" + browserSettings.updateStatus.ToString().ToLower() + "&enableanimations=" + browserSettings.enableVisualStyles.ToString().ToLower() + "&enableupdates=" + browserSettings.enableUpdates.ToString().ToLower() + "&homepage=" + browserSettings.homepage.ToLower());
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

        private void NewTab_Click(object sender, EventArgs e) => Process.Start(Application.StartupPath + @"\Webwatcher.exe", "--child");

        private void Settings_Click(object sender, EventArgs e) => Chromium.LoadUrl(Links.SettingsPage + "?showpagestatus=" + browserSettings.updateStatus.ToString().ToLower() + "&enableanimations=" + browserSettings.enableVisualStyles.ToString().ToLower() + "&enableupdates=" + browserSettings.enableUpdates.ToString().ToLower() + "&homepage=" + browserSettings.homepage.ToLower());

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

        #endregion

        #region Fields

        /// <summary>
        /// <see cref="Browser"/> version
        /// </summary>
        internal const string Version = "1.8.2";

        /// <summary>
        /// Settings file path
        /// </summary>
        private string settingsPath = Application.StartupPath + @"\options.txt";

        /// <summary>
        /// Browser settings
        /// </summary>
        private (bool updateStatus, bool enableVisualStyles, bool enableUpdates, string homepage) browserSettings;

        #endregion
    }
}
