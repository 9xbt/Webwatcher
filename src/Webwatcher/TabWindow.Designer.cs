namespace Webwatcher
{
    partial class TabWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabWindow));
            this.toolbarBackground = new System.Windows.Forms.Panel();
            this.ForwardButton = new System.Windows.Forms.PictureBox();
            this.BackButton = new System.Windows.Forms.PictureBox();
            this.SettingsButton = new System.Windows.Forms.PictureBox();
            this.urlBoxBackground = new System.Windows.Forms.Panel();
            this.urlBoxRight = new System.Windows.Forms.PictureBox();
            this.urlBoxLeft = new System.Windows.Forms.PictureBox();
            this.UrlTextBox = new System.Windows.Forms.TextBox();
            this.toolbarBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ForwardButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsButton)).BeginInit();
            this.urlBoxBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.urlBoxRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.urlBoxLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // toolbarBackground
            // 
            this.toolbarBackground.BackgroundImage = global::Webwatcher.Resources.ToolbarBackground;
            this.toolbarBackground.Controls.Add(this.ForwardButton);
            this.toolbarBackground.Controls.Add(this.BackButton);
            this.toolbarBackground.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbarBackground.Location = new System.Drawing.Point(0, 0);
            this.toolbarBackground.Name = "toolbarBackground";
            this.toolbarBackground.Size = new System.Drawing.Size(326, 38);
            this.toolbarBackground.TabIndex = 2;
            // 
            // ForwardButton
            // 
            this.ForwardButton.BackColor = System.Drawing.Color.Transparent;
            this.ForwardButton.Image = global::Webwatcher.Resources.ForwardActive;
            this.ForwardButton.Location = new System.Drawing.Point(38, 5);
            this.ForwardButton.Margin = new System.Windows.Forms.Padding(4, 4, 3, 3);
            this.ForwardButton.Name = "ForwardButton";
            this.ForwardButton.Size = new System.Drawing.Size(28, 28);
            this.ForwardButton.TabIndex = 3;
            this.ForwardButton.TabStop = false;
            this.ForwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            this.ForwardButton.MouseEnter += new System.EventHandler(this.forwardButton_MouseEnter);
            this.ForwardButton.MouseLeave += new System.EventHandler(this.forwardButton_MouseLeave);
            // 
            // BackButton
            // 
            this.BackButton.BackColor = System.Drawing.Color.Transparent;
            this.BackButton.Image = global::Webwatcher.Resources.BackActive;
            this.BackButton.Location = new System.Drawing.Point(6, 5);
            this.BackButton.Margin = new System.Windows.Forms.Padding(4, 4, 3, 3);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(28, 28);
            this.BackButton.TabIndex = 2;
            this.BackButton.TabStop = false;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            this.BackButton.MouseEnter += new System.EventHandler(this.BackButton_MouseEnter);
            this.BackButton.MouseLeave += new System.EventHandler(this.BackButton_MouseLeave);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsButton.BackColor = System.Drawing.Color.Transparent;
            this.SettingsButton.Image = global::Webwatcher.Resources.ToolsActive;
            this.SettingsButton.Location = new System.Drawing.Point(293, 5);
            this.SettingsButton.Margin = new System.Windows.Forms.Padding(4, 4, 3, 3);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(28, 28);
            this.SettingsButton.TabIndex = 4;
            this.SettingsButton.TabStop = false;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            this.SettingsButton.MouseEnter += new System.EventHandler(this.SettingsButton_MouseEnter);
            this.SettingsButton.MouseLeave += new System.EventHandler(this.SettingsButton_MouseLeave);
            // 
            // urlBoxBackground
            // 
            this.urlBoxBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlBoxBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(243)))), ((int)(((byte)(244)))));
            this.urlBoxBackground.Controls.Add(this.urlBoxRight);
            this.urlBoxBackground.Controls.Add(this.urlBoxLeft);
            this.urlBoxBackground.Controls.Add(this.UrlTextBox);
            this.urlBoxBackground.ForeColor = System.Drawing.Color.Silver;
            this.urlBoxBackground.Location = new System.Drawing.Point(71, 5);
            this.urlBoxBackground.Name = "urlBoxBackground";
            this.urlBoxBackground.Size = new System.Drawing.Size(215, 28);
            this.urlBoxBackground.TabIndex = 2;
            // 
            // urlBoxRight
            // 
            this.urlBoxRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.urlBoxRight.Image = global::Webwatcher.Resources.UrlBoxRight;
            this.urlBoxRight.Location = new System.Drawing.Point(203, 0);
            this.urlBoxRight.Name = "urlBoxRight";
            this.urlBoxRight.Size = new System.Drawing.Size(12, 28);
            this.urlBoxRight.TabIndex = 4;
            this.urlBoxRight.TabStop = false;
            // 
            // urlBoxLeft
            // 
            this.urlBoxLeft.Image = global::Webwatcher.Resources.UrlBoxLeft;
            this.urlBoxLeft.Location = new System.Drawing.Point(0, 0);
            this.urlBoxLeft.Name = "urlBoxLeft";
            this.urlBoxLeft.Size = new System.Drawing.Size(12, 28);
            this.urlBoxLeft.TabIndex = 3;
            this.urlBoxLeft.TabStop = false;
            // 
            // UrlTextBox
            // 
            this.UrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UrlTextBox.BackColor = System.Drawing.Color.FromArgb((int)(byte)(241), (int)(byte)(243), (int)(byte)(244));
            this.UrlTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UrlTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UrlTextBox.ForeColor = System.Drawing.Color.FromArgb((int)(byte)(180), (int)(byte)(182), (int)(byte)(183));
            this.UrlTextBox.Location = new System.Drawing.Point(19, 5);
            this.UrlTextBox.Margin = new System.Windows.Forms.Padding(9);
            this.UrlTextBox.Name = "UrlTextBox";
            this.UrlTextBox.Size = new System.Drawing.Size(177, 18);
            this.UrlTextBox.TabIndex = 1;
            this.UrlTextBox.Text = "Search Google or type a URL";
            this.UrlTextBox.WordWrap = false;
            this.UrlTextBox.Enter += new System.EventHandler(this.UrlTextBox_Enter);
            this.UrlTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UrlTextBox_KeyDown);
            this.UrlTextBox.Leave += new System.EventHandler(this.UrlTextBox_Leave);
            // 
            // TabWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(326, 289);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.urlBoxBackground);
            this.Controls.Add(this.toolbarBackground);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "TabWindow";
            this.Text = "TabWindow";
            this.toolbarBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ForwardButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsButton)).EndInit();
            this.urlBoxBackground.ResumeLayout(false);
            this.urlBoxBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.urlBoxRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.urlBoxLeft)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel toolbarBackground;
        private System.Windows.Forms.PictureBox BackButton;
        private System.Windows.Forms.PictureBox ForwardButton;
        private System.Windows.Forms.Panel urlBoxBackground;
        private System.Windows.Forms.TextBox UrlTextBox;
        private System.Windows.Forms.PictureBox urlBoxLeft;
        private System.Windows.Forms.PictureBox urlBoxRight;
        private System.Windows.Forms.PictureBox SettingsButton;
    }
}