namespace Webwatcher
{
    partial class Browser
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
            this.MenuBar = new System.Windows.Forms.Panel();
            this.DevToolsButton = new System.Windows.Forms.Button();
            this.Fullscreen = new System.Windows.Forms.CheckBox();
            this.Settings = new System.Windows.Forms.Button();
            this.NewTab = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AddressBar = new System.Windows.Forms.RichTextBox();
            this.GoogleBar = new System.Windows.Forms.RichTextBox();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.Link_Changelog = new System.Windows.Forms.LinkLabel();
            this.Forward = new System.Windows.Forms.Button();
            this.Return = new System.Windows.Forms.Button();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.OpacityTimer = new System.Windows.Forms.Timer(this.components);
            this.WidthTimer = new System.Windows.Forms.Timer(this.components);
            this.HeightTimer = new System.Windows.Forms.Timer(this.components);
            this.Chromium = new CefSharp.WinForms.ChromiumWebBrowser();
            this.MenuBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuBar
            // 
            resources.ApplyResources(this.MenuBar, "MenuBar");
            this.MenuBar.BackColor = System.Drawing.SystemColors.Control;
            this.MenuBar.Controls.Add(this.DevToolsButton);
            this.MenuBar.Controls.Add(this.Fullscreen);
            this.MenuBar.Controls.Add(this.Settings);
            this.MenuBar.Controls.Add(this.NewTab);
            this.MenuBar.Controls.Add(this.panel1);
            this.MenuBar.Controls.Add(this.VersionLabel);
            this.MenuBar.Controls.Add(this.Link_Changelog);
            this.MenuBar.Controls.Add(this.Forward);
            this.MenuBar.Controls.Add(this.Return);
            this.MenuBar.Name = "MenuBar";
            // 
            // DevToolsButton
            // 
            resources.ApplyResources(this.DevToolsButton, "DevToolsButton");
            this.DevToolsButton.Name = "DevToolsButton";
            this.DevToolsButton.UseVisualStyleBackColor = true;
            this.DevToolsButton.Click += new System.EventHandler(this.DevToolsButton_Click);
            // 
            // Fullscreen
            // 
            resources.ApplyResources(this.Fullscreen, "Fullscreen");
            this.Fullscreen.Name = "Fullscreen";
            this.Fullscreen.UseVisualStyleBackColor = true;
            this.Fullscreen.CheckedChanged += new System.EventHandler(this.FullscreenCheck_CheckedChanged);
            // 
            // Settings
            // 
            resources.ApplyResources(this.Settings, "Settings");
            this.Settings.Name = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // NewTab
            // 
            resources.ApplyResources(this.NewTab, "NewTab");
            this.NewTab.Name = "NewTab";
            this.NewTab.UseVisualStyleBackColor = true;
            this.NewTab.Click += new System.EventHandler(this.NewTab_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.AddressBar);
            this.panel1.Controls.Add(this.GoogleBar);
            this.panel1.Name = "panel1";
            // 
            // AddressBar
            // 
            resources.ApplyResources(this.AddressBar, "AddressBar");
            this.AddressBar.BackColor = System.Drawing.SystemColors.Window;
            this.AddressBar.DetectUrls = false;
            this.AddressBar.ForeColor = System.Drawing.Color.Silver;
            this.AddressBar.Name = "AddressBar";
            this.AddressBar.Enter += new System.EventHandler(this.URL_Box_Enter);
            this.AddressBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.URL_Box_KeyPress);
            this.AddressBar.Leave += new System.EventHandler(this.URL_Box_Leave);
            // 
            // GoogleBar
            // 
            resources.ApplyResources(this.GoogleBar, "GoogleBar");
            this.GoogleBar.DetectUrls = false;
            this.GoogleBar.ForeColor = System.Drawing.Color.Silver;
            this.GoogleBar.Name = "GoogleBar";
            this.GoogleBar.Enter += new System.EventHandler(this.Google_Search_Box_Enter);
            this.GoogleBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Google_Search_Box_KeyPress);
            this.GoogleBar.Leave += new System.EventHandler(this.Google_Search_Box_Leave);
            // 
            // VersionLabel
            // 
            resources.ApplyResources(this.VersionLabel, "VersionLabel");
            this.VersionLabel.Name = "VersionLabel";
            // 
            // Link_Changelog
            // 
            resources.ApplyResources(this.Link_Changelog, "Link_Changelog");
            this.Link_Changelog.Name = "Link_Changelog";
            this.Link_Changelog.TabStop = true;
            this.Link_Changelog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_Changelog_LinkClicked);
            // 
            // Forward
            // 
            resources.ApplyResources(this.Forward, "Forward");
            this.Forward.Name = "Forward";
            this.Forward.UseVisualStyleBackColor = true;
            this.Forward.Click += new System.EventHandler(this.Button_Forward_Click);
            // 
            // Return
            // 
            resources.ApplyResources(this.Return, "Return");
            this.Return.Name = "Return";
            this.Return.UseVisualStyleBackColor = true;
            this.Return.Click += new System.EventHandler(this.Button_Back_Click);
            // 
            // MainTimer
            // 
            this.MainTimer.Enabled = true;
            this.MainTimer.Interval = 10;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // OpacityTimer
            // 
            this.OpacityTimer.Interval = 5;
            this.OpacityTimer.Tick += new System.EventHandler(this.OpacityTimer_Tick);
            // 
            // WidthTimer
            // 
            this.WidthTimer.Interval = 1;
            this.WidthTimer.Tick += new System.EventHandler(this.WidthTimer_Tick);
            // 
            // HeightTimer
            // 
            this.HeightTimer.Interval = 1;
            this.HeightTimer.Tick += new System.EventHandler(this.HeightTimer_Tick);
            // 
            // Chromium
            // 
            this.Chromium.ActivateBrowserOnCreation = false;
            resources.ApplyResources(this.Chromium, "Chromium");
            this.Chromium.Name = "Chromium";
            this.Chromium.AddressChanged += new System.EventHandler<CefSharp.AddressChangedEventArgs>(this.Chromium_AddressChanged);
            this.Chromium.TitleChanged += new System.EventHandler<CefSharp.TitleChangedEventArgs>(this.Chromium_TitleChanged);
            this.Chromium.LoadError += new System.EventHandler<CefSharp.LoadErrorEventArgs>(this.Chromium_LoadError);
            this.Chromium.LoadingStateChanged += new System.EventHandler<CefSharp.LoadingStateChangedEventArgs>(this.Chromium_LoadingStateChanged);
            // 
            // Browser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Chromium);
            this.Controls.Add(this.MenuBar);
            this.Name = "Browser";
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel MenuBar;
        private System.Windows.Forms.Button Forward;
        private System.Windows.Forms.Button Return;
        private System.Windows.Forms.LinkLabel Link_Changelog;
        private System.Windows.Forms.RichTextBox AddressBar;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.RichTextBox GoogleBar;
        private System.Windows.Forms.Button NewTab;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.CheckBox Fullscreen;
        private System.Windows.Forms.Timer OpacityTimer;
        private System.Windows.Forms.Timer HeightTimer;
        private System.Windows.Forms.Timer WidthTimer;
        private System.Windows.Forms.Button DevToolsButton;
        private CefSharp.WinForms.ChromiumWebBrowser Chromium;
    }
}

