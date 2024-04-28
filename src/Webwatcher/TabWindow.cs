using CefSharp;
using CefSharp.DevTools.Debugger;
using CefSharp.WinForms;
using EasyTabs;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webwatcher
{
    public partial class TabWindow : Form
    {
        private class NewTabLifespanHandler : ILifeSpanHandler
        {
            private readonly TabWindow _tab;

            public NewTabLifespanHandler(TabWindow tab)
            {
                _tab = tab;
            }

            public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
                if (browser.IsPopup)
                {
                    return false;
                }

                return true;
            }

            public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

            public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

            public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                TabWindow newTab = null;

                _tab.ParentTabs.Invoke(new Action(() =>
                {
                    _tab.ParentTabs.AddNewTab();
                    newTab = _tab.ParentTabs.SelectedTab.Content as TabWindow;

                    bool newTabLoaded = false;

                    if (!newTab.WebBrowser.IsBrowserInitialized)
                    {
                        newTab.WebBrowser.IsBrowserInitializedChanged += (_, __) =>
                        {
                            newTab.WebBrowser.LoadingStateChanged += (___, e) =>
                            {
                                if (!newTabLoaded)
                                {
                                    if (!e.IsLoading)
                                    {
                                        newTabLoaded = true;
                                        newTab.WebBrowser.Load(targetUrl);
                                    }
                                }
                            };
                        };
                    }
                    else
                    {
                        newTab.WebBrowser.LoadingStateChanged += (_, e) =>
                        {
                            if (!newTabLoaded)
                            {
                                if (!e.IsLoading)
                                {
                                    newTabLoaded = true;
                                    newTab.WebBrowser.Load(targetUrl);
                                }
                            }
                        };
                    }                    
                }));

                newBrowser = null;
                return true;
            }
        }

        private string _urlTextBoxDefaultText = "Search Google or type a URL";
        private string _lastAddress = null, _actualHomepage = null;
        private bool _faviconLoaded = false;
        private bool _firstLoad = true, _firstActualLoad = true;

        public readonly ChromiumWebBrowser WebBrowser;

        protected TitleBarTabs ParentTabs => ParentForm as TitleBarTabs;

        public TabWindow(string url = null)
        {
            InitializeComponent();

            _lastAddress = ConfigManager.Config.UseDefaultHomepage ? "http://google.com/" : ConfigManager.Config.Homepage;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebBrowser = new ChromiumWebBrowser(url ?? (ConfigManager.Config.UseDefaultHomepage ? (ConfigManager.Config.SearchEngine == "google" ? "https://google.com/" : (ConfigManager.Config.SearchEngine == "duckduckgo" ? "https://start.duckduckgo.com/" : "about:blank")) : ConfigManager.Config.Homepage))
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(0, 38),
                MinimumSize = new Size(20, 20),
                Name = "webBrowser",
                Size = new Size(326, 251),
                TabIndex = 6,
                LifeSpanHandler = new NewTabLifespanHandler(this)
            };
            WebBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            WebBrowser.JavascriptObjectRepository.Register("webwatcher", new ConfigInterface(this), true, BindingOptions.DefaultBinder);
            WebBrowser.TitleChanged += WebBrowser_TitleChanged;
            WebBrowser.AddressChanged += WebBrowser_AddressChanged;
            WebBrowser.LoadingStateChanged += WebBrowser_DocumentCompleted;
            WebBrowser.LoadError += WebBrowser_LoadError;

            Controls.Add(WebBrowser);

            switch (ConfigManager.Config.SearchEngine)
            {
                case "google":
                    _urlTextBoxDefaultText = "Search Google or type a URL";
                    break;

                case "duckduckgo":
                    _urlTextBoxDefaultText = "Search DuckDuckGo or type a URL";
                    break;
            }

            UrlTextBox.Text = _urlTextBoxDefaultText;
        }

        private void WebBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (_firstActualLoad)
            {
                _actualHomepage = e.Address;
                _firstActualLoad = false;
            }

            _lastAddress = e.Address;

            if (!_firstLoad && e.Address != ConfigManager.ErrorURL)
            {
                Invoke(new Action(() =>
                {
                    UrlTextBox.Text = e.Address.Replace(
                        ConfigManager.ConfigURL, "webwatcher://settings").Replace(
                        ConfigManager.AdvancedConfigURL, "webwatcher://settings/advanced").Replace(
                        ConfigManager.AboutURL, "webwatcher://about").Replace(
                        ConfigManager.ChangelogURL, "webwatcher://changelog");
                    UrlTextBox.ForeColor = Color.Black;
                    WebBrowser.Focus();
                }));
            }

            if (_firstLoad && _lastAddress != _actualHomepage)
                _firstLoad = false;

            if (e.Address != "about.blank" && !_faviconLoaded)
            {
                var uri = new Uri(e.Address);

                if (uri.Scheme == "http" || uri.Scheme == "https")
                {
                    try
                    {
                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri.Scheme + "://" + uri.Host + "/favicon.ico");
                        webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36";
                        webRequest.KeepAlive = false;
                        webRequest.AllowAutoRedirect = true;

                        WebResponse response = webRequest.GetResponse();
                        Stream stream = response.GetResponseStream();

                        if (stream != null)
                        {
                            byte[] buffer = new byte[1024];

                            using (MemoryStream ms = new MemoryStream())
                            {
                                int read;

                                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                    ms.Write(buffer, 0, read);

                                ms.Seek(0, SeekOrigin.Begin);

                                Invoke(new Action(() =>
                                {
                                    Icon = new Icon(ms);

                                    ParentTabs.UpdateThumbnailPreviewIcon(ParentTabs.Tabs.Single(t => t.Content == this));
                                    ParentTabs.RedrawTabs();
                                }));
                            }
                        }
                    }
                    catch { Invoke(new Action(() => Icon = Resources.GenericGlobe)); }
                }
                else Invoke(new Action(() => Icon = Resources.GenericGlobe));

                Invoke(new Action(() => Parent.Refresh()));
                _faviconLoaded = true;
            }
        }

        private void WebBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
            => Invoke(new Action(() => Text = e.Title));

        private void WebBrowser_DocumentCompleted(object sender, LoadingStateChangedEventArgs e)
        {
            var cleanAddress = WebBrowser.Address.Contains("?") ? WebBrowser.Address.Remove(WebBrowser.Address.IndexOf("?")) : WebBrowser.Address;

            if (UrlTextBox.Text == "about:blank")
            {
                Invoke(new Action(() => Icon = Resources.GenericGlobe));
            }
            else if (cleanAddress == ConfigManager.ConfigURL)
            {
                WebBrowser.ExecuteScriptAsync(
                    "document.querySelector(\"#homepage_url\").value = \"" + ConfigManager.Config.Homepage + "\";" +
                    "document.querySelector(\"#homepage_url\").disabled = " + (ConfigManager.Config.UseDefaultHomepage ? "true" : "false") + ";" +
                    "document.querySelector(\"#homepage_type_def\").checked = " + (ConfigManager.Config.UseDefaultHomepage ? "true" : "false") + ";" +
                    "document.querySelector(\"#homepage_type_man\").checked = " + (ConfigManager.Config.UseDefaultHomepage ? "false" : "true") + ";" +
                    "document.querySelector(\"#search_engine_google\").checked = " + (ConfigManager.Config.SearchEngine == "google" ? "true" : "false") + ";" +
                    "document.querySelector(\"#search_engine_duckduckgo\").checked = " + (ConfigManager.Config.SearchEngine == "duckduckgo" ? "true" : "false") + ";" +
                    "search_engine = \"" + ConfigManager.Config.SearchEngine + "\";"
                );
            }
            else if (cleanAddress == ConfigManager.AdvancedConfigURL)
            {
                WebBrowser.ExecuteScriptAsync("document.querySelector(\"#use_hardware_accel\").checked = " + (ConfigManager.Config.UseHardwareAccel ? "true" : "false"));
            }
            else if (cleanAddress == ConfigManager.AboutURL)
            {
                WebBrowser.ExecuteScriptAsync("const webwatcher_ver = \"1.9\"");
            }
            else if (cleanAddress == ConfigManager.ErrorURL)
            {
                WebBrowser.ExecuteScriptAsync(
                    "const server_span = document.querySelector(\"#server_span\");" +
                    "server_span.textContent = \"We can’t connect to the server at " + UrlTextBox.Text + ".\";");
            }

            if (!WebBrowser.JavascriptObjectRepository.IsBound("webwatcher"))
            {
                WebBrowser.JavascriptObjectRepository.Register("webwatcher", new ConfigInterface(this), true, BindingOptions.DefaultBinder);
            }
        }

        private void WebBrowser_LoadError(object sender, LoadErrorEventArgs e)
            => WebBrowser.LoadUrl(ConfigManager.ErrorURL);

        private void BackButton_MouseEnter(object sender, EventArgs e)
            => BackButton.BackgroundImage = Resources.ButtonHoverBackground;

        private void BackButton_MouseLeave(object sender, EventArgs e)
            => BackButton.BackgroundImage = null;

        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            _firstLoad = false;

            var url = UrlTextBox.Text;

            if (url.StartsWith("webwatcher://"))
            {
                switch (url)
                {
                    case "webwatcher://settings":
                        url = ConfigManager.ConfigURL;
                        break;

                    case "webwatcher://settings/advanced":
                        url = ConfigManager.AdvancedConfigURL;
                        break;

                    case "webwatcher://about":
                        url = ConfigManager.AboutURL;
                        break;

                    case "webwatcher://changelog":
                        url = ConfigManager.ChangelogURL;
                        break;

                    default:
                        WebBrowser.LoadUrl(ConfigManager.ErrorURL);
                        return;
                }
            }
            else if (!Regex.IsMatch(url,
                @"^(http[s]?://)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,63}(/\S*)?$",
                RegexOptions.IgnoreCase) &&
                !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                switch (ConfigManager.Config.SearchEngine)
                {
                    case "google":
                        url = url.Insert(0, "https://google.com/search?q=");
                        break;
                        
                    case "duckduckgo":
                        url = url.Insert(0, "https://duckduckgo.com/?q=");
                        break;
                }
            }

            _faviconLoaded = false;
            WebBrowser.Load(url);
        }

        private void forwardButton_MouseEnter(object sender, EventArgs e)
            => ForwardButton.BackgroundImage = Resources.ButtonHoverBackground;

        private void forwardButton_MouseLeave(object sender, EventArgs e)
            => ForwardButton.BackgroundImage = null;

        private void BackButton_Click(object sender, EventArgs e)
            => WebBrowser.Back();

        private void ForwardButton_Click(object sender, EventArgs e)
            => WebBrowser.Forward();

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            ParentTabs.Tabs.Add(new TitleBarTab(ParentTabs)
            {
                Content = new TabWindow(ConfigManager.ConfigURL)
            });
            ParentTabs.SelectedTabIndex++;

            var selectedTab = (TabWindow)ParentTabs.Tabs[ParentTabs.SelectedTabIndex].Content;
            selectedTab.UrlTextBox.Text = "webwatcher://settings";
            selectedTab.UrlTextBox.ForeColor = Color.Black;
        }

        private void SettingsButton_MouseEnter(object sender, EventArgs e)
            => SettingsButton.BackgroundImage = Resources.ButtonHoverBackground;

        private void SettingsButton_MouseLeave(object sender, EventArgs e)
            => SettingsButton.BackgroundImage = null;

        private async void UrlTextBox_Enter(object sender, EventArgs e)
        {
            if (UrlTextBox.Text != _urlTextBoxDefaultText)
            {
                await Task.Delay(1); // Wait for the base to finish doing stuff

                UrlTextBox.SelectAll();
                UrlTextBox.Focus();
                return;
            }

            UrlTextBox.Text = string.Empty;
            UrlTextBox.ForeColor = Color.Black;
        }

        private void UrlTextBox_Leave(object sender, EventArgs e)
        {
            if (UrlTextBox.Text != string.Empty) return;

            UrlTextBox.Text = _urlTextBoxDefaultText;
            unchecked { UrlTextBox.ForeColor = Color.FromArgb((int)0xFFB4B6B7); }
        }
    }
}