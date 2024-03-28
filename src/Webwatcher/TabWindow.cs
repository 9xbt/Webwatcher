using System;
using EasyTabs;
using CefSharp;
using System.IO;
using System.Net;
using System.Linq;
using System.Drawing;
using CefSharp.WinForms;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Webwatcher
{
    public partial class TabWindow : Form
    {
        private class NewTabLifespanHandler : ILifeSpanHandler
        {
            private TabWindow _tab;

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

        private bool _faviconLoaded = false;
        public readonly ChromiumWebBrowser WebBrowser;

        protected TitleBarTabs ParentTabs => ParentForm as TitleBarTabs;

        public TabWindow(string url = null)
        {
            InitializeComponent();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

            WebBrowser = new ChromiumWebBrowser(url != null ? url : (ConfigLoader.Config.UseDefaultHomepage ? "http://google.com/" : ConfigLoader.Config.Homepage))
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
            WebBrowser.JavascriptObjectRepository.Register("configInterface", new ConfigInterface(this), true, BindingOptions.DefaultBinder);
            WebBrowser.TitleChanged += WebBrowser_TitleChanged;
            WebBrowser.AddressChanged += WebBrowser_AddressChanged;
            WebBrowser.LoadingStateChanged += WebBrowser_DocumentCompleted;

            Controls.Add(WebBrowser);

            WebBrowser.WaitForInitialLoadAsync();
        }

        private void WebBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            Invoke(new Action(() => UrlTextBox.Text = e.Address.Replace(ConfigLoader.ConfigURL,
                "webwatcher://settings").Replace(ConfigLoader.AboutURL, "webwatcher://settings")));

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
            if (UrlTextBox.Text == "about:blank")
            {
                Invoke(new Action(() => Icon = Resources.GenericGlobe));
            }
            else if (WebBrowser.Address == ConfigLoader.ConfigURL)
            {
                WebBrowser.ExecuteScriptAsync(
                    "document.querySelector(\"#homepage_url\").value = \"" + ConfigLoader.Config.Homepage + "\";" +
                    "document.querySelector(\"#homepage_url\").disabled = " + (ConfigLoader.Config.UseDefaultHomepage ? "true" : "false") + ";" +
                    "document.querySelector(\"#homepage_type_def\").checked = " + (ConfigLoader.Config.UseDefaultHomepage ? "true" : "false") + ";" +
                    "document.querySelector(\"#homepage_type_man\").checked = " + (ConfigLoader.Config.UseDefaultHomepage ? "false" : "true") + ";"
                );
            }

            if (!WebBrowser.JavascriptObjectRepository.IsBound("configInterface"))
            {
                WebBrowser.JavascriptObjectRepository.Register("configInterface", new ConfigInterface(this), true, BindingOptions.DefaultBinder);
            }
        }

        private void BackButton_MouseEnter(object sender, EventArgs e)
            => BackButton.BackgroundImage = Resources.ButtonHoverBackground;

        private void BackButton_MouseLeave(object sender, EventArgs e)
            => BackButton.BackgroundImage = null;

        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var fullUrl = UrlTextBox.Text;

            if (fullUrl == "webwatcher://settings")
            {
                fullUrl = ConfigLoader.ConfigURL;
            }

            if (!Regex.IsMatch(fullUrl, "^[a-zA-Z0-9]+\\://"))
                fullUrl = "http://" + fullUrl;

            _faviconLoaded = false;
            WebBrowser.Load(fullUrl);
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
                Content = new TabWindow(ConfigLoader.ConfigURL)
            });
            ParentTabs.SelectedTabIndex++;
        }

        private void SettingsButton_MouseEnter(object sender, EventArgs e)
            => SettingsButton.BackgroundImage = Resources.ButtonHoverBackground;

        private void SettingsButton_MouseLeave(object sender, EventArgs e)
            => SettingsButton.BackgroundImage = null;
    }
}