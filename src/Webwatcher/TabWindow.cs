using CefSharp;
using CefSharp.WinForms;
using EasyTabs;
using System;
using System.Drawing;
using System.Globalization;
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
                                if (!newTabLoaded && !e.IsLoading)
                                {
                                    newTabLoaded = true;
                                    newTab.WebBrowser.Load(targetUrl);
                                }
                            };
                        };
                    }
                    else
                    {
                        newTab.WebBrowser.LoadingStateChanged += (_, e) =>
                        {
                            if (!newTabLoaded && !e.IsLoading)
                            {
                                newTabLoaded = true;
                                newTab.WebBrowser.Load(targetUrl);
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

            if (url == null)
            {
                if (ConfigManager.Config.UseDefaultHomepage)
                {
                    switch (ConfigManager.Config.SearchEngine)
                    {
                        case "google":
                            _lastAddress = "https://google.com/";
                            break;
                        case "duckduckgo":
                            _lastAddress = "https://start.duckduckgo.com/";
                            break;
                        case "yahoo":
                            _lastAddress = "https://search.yahoo.com/";
                            break;
                        default:
                            _lastAddress = "about:blank";
                            break;
                    }
                }
                else _lastAddress = ConfigManager.Config.Homepage;
            }
            else _lastAddress = url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebBrowser = new ChromiumWebBrowser(_lastAddress)
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
            WebBrowser.DownloadHandler = new DownloadHandler(this);

            Controls.Add(WebBrowser);

            switch (ConfigManager.Config.SearchEngine)
            {
                case "google":
                    _urlTextBoxDefaultText = "Search Google or type a URL";
                    break;
                case "duckduckgo":
                    _urlTextBoxDefaultText = "Search DuckDuckGo or type a URL";
                    break;
                case "yahoo":
                    _urlTextBoxDefaultText = "Search Yahoo! or type a URL";
                    break;
            }

            UrlTextBox.Text = _urlTextBoxDefaultText;
        }

        public void OpenPage(string url, string realPath)
        {
            ParentTabs.Tabs.Add(new TitleBarTab(ParentTabs)
            {
                Content = new TabWindow(realPath)
            });
            ParentTabs.SelectedTabIndex++;

            var selectedTab = (TabWindow)ParentTabs.Tabs[ParentTabs.SelectedTabIndex].Content;
            selectedTab.UrlTextBox.Text = url;
            selectedTab.UrlTextBox.ForeColor = Color.Black;
        }

        public void UpdateDownloads(Download download)
        {
            var filename = Path.GetFileName(download.Path);

            WebBrowser.ExecuteScriptAsync(
                "const spans = document.querySelectorAll(\"span\");" +
                "const current = Array.from(spans).find(span => span.textContent.trim().startsWith(\"" + filename + "\"));" +
                "current.textContent = \"" + filename + " — " + download.Progress + "% done\";"
            );
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
                    UrlTextBox.Text = e.Address.Replace("%20", " ").Replace(
                        ConfigManager.ConfigURL, "webwatcher://settings").Replace(
                        ConfigManager.AdvancedConfigURL, "webwatcher://settings/advanced").Replace(
                        ConfigManager.AboutURL, "webwatcher://about").Replace(
                        ConfigManager.ChangelogURL, "webwatcher://changelog".Replace(
                        ConfigManager.DownloadsURL, "webwatcher://downloads"));
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
                                    try
                                    {
                                        Icon = new Icon(ms);

                                        ParentTabs.UpdateThumbnailPreviewIcon(ParentTabs.Tabs.Single(t => t.Content == this));
                                        ParentTabs.RedrawTabs();
                                    }
                                    catch { SetDefaultIcon(); }
                                }));
                            }
                        }
                    }
                    catch { SetDefaultIcon(); }
                }
                else SetDefaultIcon();

                Invoke(new Action(() => Parent.Refresh()));
                _faviconLoaded = true;
            }
        }

        private void SetDefaultIcon()
            => Invoke(new Action(() => Icon = Resources.GenericGlobe));

        private void WebBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
            => Invoke(new Action(() => Text = e.Title));

        private void WebBrowser_DocumentCompleted(object sender, LoadingStateChangedEventArgs e)
        {
            var cleanAddress = (WebBrowser.Address.Contains("?") ?
                WebBrowser.Address.Remove(WebBrowser.Address.IndexOf("?")) :
                WebBrowser.Address).Replace("%20", " ");

            if (cleanAddress == "about:blank")
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
                    "document.querySelector(\"#search_engine_yahoo\").checked = " + (ConfigManager.Config.SearchEngine == "yahoo" ? "true" : "false") + ";" +
                    "search_engine = \"" + ConfigManager.Config.SearchEngine + "\";"
                );
            }
            else if (cleanAddress == ConfigManager.AdvancedConfigURL)
            {
                WebBrowser.ExecuteScriptAsync("document.querySelector(\"#use_hardware_accel\").checked = " + (ConfigManager.Config.UseHardwareAccel ? "true" : "false"));
            }
            else if (cleanAddress == ConfigManager.AboutURL)
            {
                WebBrowser.ExecuteScriptAsync("const webwatcher_ver = \"" + Program.Version + "\"");
            }
            else if (cleanAddress == ConfigManager.ErrorURL)
            {
                WebBrowser.ExecuteScriptAsync(
                    "const server_span = document.querySelector(\"#server_span\");" +
                    "server_span.textContent = \"We can’t connect to the server at " + UrlTextBox.Text + ".\";"
                );
            }
            else if (cleanAddress.StartsWith(ConfigManager.DownloadsURL))
            {
                var files = Directory.GetFiles(ConfigManager.DownloadPath)
                    .Select(file => new
                    {
                        Name = Path.GetFileName(file),
                        Path = file,
                        Time = File.GetCreationTime(file)
                    })
                    .OrderBy(file => file.Time)
                    .ToList();
                files.Reverse();

                foreach (var file in files)
                {
                    var download = DownloadHandler.GetDownloadByPath(file.Path);
                    var fmtDateLong = file.Time.ToString("MMMM d", CultureInfo.InvariantCulture) + GetDaySuffix(file.Time.Day) + ", " + file.Time.Year;
                    var fmtDateShort = file.Time.ToString("dd-MM-yyyy");

                    WebBrowser.ExecuteScriptAsync(
                        "function addItem() {" +
                        "    const div = document.querySelector('.changelog');" +
                        "    const date = document.createElement('h2');" +
                        "    const file = document.createElement('a');" +
                        "    date.textContent = \"" + fmtDateLong + "\";" +
                        "    file.textContent = \"" + file.Name + (download.Progress >= 0 ? " — " + download.Progress + "% done" : "") + "\";" +
                        "    file.href = \"javascript:webwatcher.showDownloadedFile('" + file.Name + "')\";" +
                        "    file.classList.add(\"download_item\");" +
                        "    const headers = document.querySelectorAll('h2');" +
                        "    const duplicate = Array.from(headers).some(h2 => h2.textContent.trim() === \"" + fmtDateLong + "\");" +
                        "    if (!duplicate) {" +
                        "        const headings = div.querySelectorAll('h2');" +
                        "        if (headings.length > 0) {" +
                        "            div.appendChild(document.createElement('br'));" +
                        "        }" +
                        "        div.appendChild(date);" +
                        "    }" +
                        "    div.appendChild(file);" +
                        "    div.appendChild(document.createElement('br'));" +
                        "}" +
                        "addItem();"
                    );
                }
                WebBrowser.ExecuteScriptAsync(
                    "function addPadding() {" +
                    "    const mainDiv = document.querySelector('.changelog');" +
                    "    mainDiv.appendChild(document.createElement('br'));" +
                    "}" +
                    "addPadding();"
                );
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
                    case "webwatcher://downloads":
                        url = ConfigManager.DownloadsURL;
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
                    case "yahoo":
                        url = url.Insert(0, "https://search.yahoo.com/search?p=");
                        break;
                }
            }

            _faviconLoaded = false;
            WebBrowser.Load(url);
        }

        private void ForwardButton_MouseEnter(object sender, EventArgs e)
            => ForwardButton.BackgroundImage = Resources.ButtonHoverBackground;

        private void ForwardButton_MouseLeave(object sender, EventArgs e)
            => ForwardButton.BackgroundImage = null;

        private void BackButton_Click(object sender, EventArgs e)
            => WebBrowser.Back();

        private void ForwardButton_Click(object sender, EventArgs e)
            => WebBrowser.Forward();

        private void SettingsButton_Click(object sender, EventArgs e)
            => ContextMenu.Show(Cursor.Position.X, Cursor.Position.Y + 10);

        private void SettingsButton_MouseEnter(object sender, EventArgs e)
            => SettingsButton.BackgroundImage = Resources.ButtonHoverBackground;

        private void SettingsButton_MouseLeave(object sender, EventArgs e)
            => SettingsButton.BackgroundImage = null;

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
            => OpenPage("webwatcher://settings", ConfigManager.ConfigURL);

        private void downloadsToolStripMenuItem_Click(object sender, EventArgs e)
            => OpenPage("webwatcher://downloads", ConfigManager.DownloadsURL);

        private void aboutWebwatcherToolStripMenuItem_Click(object sender, EventArgs e)
            => OpenPage("webwatcher://about", ConfigManager.AboutURL);

        private void developerToolsToolStripMenuItem_Click(object sender, EventArgs e)
            => WebBrowser.ShowDevTools();

        private async void UrlTextBox_Enter(object sender, EventArgs e)
        {
            if (UrlTextBox.Text != _urlTextBoxDefaultText)
            {
                await Task.Delay(1);

                UrlTextBox.SelectAll();
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

        static string GetDaySuffix(int day)
        {
            if (day >= 11 && day <= 13)
            {
                return "th";
            }
            switch (day % 10)
            {
                case 1: return "st";
                case 2: return "nd";
                case 3: return "rd";
                default: return "th";
            }
        }
    }
}