using System.ComponentModel;
using System.Windows.Forms;
using WhiteHatSec.Localization.Culture.Resource;
using WhiteHatSec.VSIX.Utility;
using WhiteHatSec.Entity;
namespace WhiteHatSec.VSIX.UserControls
{
    public partial class WhsMainWindow : BaseControl.BaseControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhsMainWindow));
            WhiteHatSec.Entity.ApplicationDetail.ApplicationInfo applicationInfo1 = new WhiteHatSec.Entity.ApplicationDetail.ApplicationInfo();
            this.whsMenuStrip = new System.Windows.Forms.MenuStrip();
            this.whiteHatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EnglishToolStripRadioButtonMenuItem = new WhiteHatSec.VSIX.Utility.ToolStripRadioButtonMenuItem();
            this.JapaneseToolStripRadioButtonMenuItem = new WhiteHatSec.VSIX.Utility.ToolStripRadioButtonMenuItem();
            this.germanToolStripRadioButtonMenuItem = new WhiteHatSec.VSIX.Utility.ToolStripRadioButtonMenuItem();
            this.ImportSentinelResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripRadioButtonRight = new WhiteHatSec.VSIX.Utility.ToolStripRadioButtonMenuItem();
            this.toolStripRadioButtonBottom = new WhiteHatSec.VSIX.Utility.ToolStripRadioButtonMenuItem();
            this.GetDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.logOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whitehatToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.findVulnsTabPage = new System.Windows.Forms.TabPage();
            this.FindVulnsPanel = new System.Windows.Forms.Panel();
            this.WHSTabControl = new System.Windows.Forms.TabControl();
            this.whsFindingTabPage = new System.Windows.Forms.TabPage();
            this.WhsFindingWindow = new WhiteHatSec.VSIX.UserControls.VulnerabilityTrace();
            this.cultureManager1 = new WhiteHatSec.Localization.Culture.Resource.CultureManager(this.components);
            this.sentinelToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LnkLogout = new System.Windows.Forms.LinkLabel();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.whsMenuStrip.SuspendLayout();
            this.findVulnsTabPage.SuspendLayout();
            this.WHSTabControl.SuspendLayout();
            this.whsFindingTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // whsMenuStrip
            // 
            resources.ApplyResources(this.whsMenuStrip, "whsMenuStrip");
            this.whsMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.whsMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.whsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whiteHatToolStripMenuItem,
            this.whitehatToolStripMenuItem1});
            this.whsMenuStrip.Name = "whsMenuStrip";
            this.whsMenuStrip.ShowItemToolTips = true;
            this.whsMenuStrip.Stretch = false;
            this.whsMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.whsMenuStrip_ItemClicked);
            // 
            // whiteHatToolStripMenuItem
            // 
            this.whiteHatToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.whiteHatToolStripMenuItem.AutoToolTip = true;
            this.whiteHatToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.whiteHatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageToolStripMenuItem,
            this.ImportSentinelResultsToolStripMenuItem,
            this.uploadProjectToolStripMenuItem,
            this.showSolutionToolStripMenuItem,
            this.GetDetailsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItemHelp,
            this.logOutMenuItem});
            this.whiteHatToolStripMenuItem.Image = global::WhiteHatSec.VSIX.Resources.Resources.DownArrow;
            resources.ApplyResources(this.whiteHatToolStripMenuItem, "whiteHatToolStripMenuItem");
            this.whiteHatToolStripMenuItem.Name = "whiteHatToolStripMenuItem";
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EnglishToolStripRadioButtonMenuItem,
            this.JapaneseToolStripRadioButtonMenuItem,
            this.germanToolStripRadioButtonMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            // 
            // EnglishToolStripRadioButtonMenuItem
            // 
            this.EnglishToolStripRadioButtonMenuItem.Checked = true;
            this.EnglishToolStripRadioButtonMenuItem.CheckOnClick = true;
            this.EnglishToolStripRadioButtonMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnglishToolStripRadioButtonMenuItem.Name = "EnglishToolStripRadioButtonMenuItem";
            resources.ApplyResources(this.EnglishToolStripRadioButtonMenuItem, "EnglishToolStripRadioButtonMenuItem");
            this.EnglishToolStripRadioButtonMenuItem.Click += new System.EventHandler(this.EnglishToolStripRadioButtonMenuItem_Click);
            // 
            // JapaneseToolStripRadioButtonMenuItem
            // 
            this.JapaneseToolStripRadioButtonMenuItem.CheckOnClick = true;
            this.JapaneseToolStripRadioButtonMenuItem.Name = "JapaneseToolStripRadioButtonMenuItem";
            resources.ApplyResources(this.JapaneseToolStripRadioButtonMenuItem, "JapaneseToolStripRadioButtonMenuItem");
            this.JapaneseToolStripRadioButtonMenuItem.Click += new System.EventHandler(this.JapaneseToolStripRadioButtonMenuItem_Click);
            // 
            // germanToolStripRadioButtonMenuItem
            // 
            this.germanToolStripRadioButtonMenuItem.CheckOnClick = true;
            this.germanToolStripRadioButtonMenuItem.Name = "germanToolStripRadioButtonMenuItem";
            resources.ApplyResources(this.germanToolStripRadioButtonMenuItem, "germanToolStripRadioButtonMenuItem");
            this.germanToolStripRadioButtonMenuItem.Click += new System.EventHandler(this.GermanToolStripRadioButtonMenuItem_Click);
            // 
            // ImportSentinelResultsToolStripMenuItem
            // 
            resources.ApplyResources(this.ImportSentinelResultsToolStripMenuItem, "ImportSentinelResultsToolStripMenuItem");
            this.ImportSentinelResultsToolStripMenuItem.Name = "ImportSentinelResultsToolStripMenuItem";
            this.ImportSentinelResultsToolStripMenuItem.Click += new System.EventHandler(this.ImportSentinelResultsToolStripMenuItem_Click);
            // 
            // uploadProjectToolStripMenuItem
            // 
            resources.ApplyResources(this.uploadProjectToolStripMenuItem, "uploadProjectToolStripMenuItem");
            this.uploadProjectToolStripMenuItem.Name = "uploadProjectToolStripMenuItem";
            this.uploadProjectToolStripMenuItem.Click += new System.EventHandler(this.UploadProjectToolStripMenuItem_Click);
            // 
            // showSolutionToolStripMenuItem
            // 
            this.showSolutionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripRadioButtonRight,
            this.toolStripRadioButtonBottom});
            this.showSolutionToolStripMenuItem.Name = "showSolutionToolStripMenuItem";
            resources.ApplyResources(this.showSolutionToolStripMenuItem, "showSolutionToolStripMenuItem");
            // 
            // ToolStripRadioButtonRight
            // 
            this.ToolStripRadioButtonRight.Checked = true;
            this.ToolStripRadioButtonRight.CheckOnClick = true;
            this.ToolStripRadioButtonRight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripRadioButtonRight.Name = "ToolStripRadioButtonRight";
            resources.ApplyResources(this.ToolStripRadioButtonRight, "ToolStripRadioButtonRight");
            this.ToolStripRadioButtonRight.CheckedChanged += new System.EventHandler(this.ToolStripRadioButtonRight_CheckedChanged);
            // 
            // toolStripRadioButtonBottom
            // 
            this.toolStripRadioButtonBottom.CheckOnClick = true;
            this.toolStripRadioButtonBottom.Name = "toolStripRadioButtonBottom";
            resources.ApplyResources(this.toolStripRadioButtonBottom, "toolStripRadioButtonBottom");
            this.toolStripRadioButtonBottom.CheckedChanged += new System.EventHandler(this.ToolStripRadioButtonBottom_CheckedChanged);
            // 
            // GetDetailsToolStripMenuItem
            // 
            this.GetDetailsToolStripMenuItem.Checked = true;
            this.GetDetailsToolStripMenuItem.CheckOnClick = true;
            this.GetDetailsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GetDetailsToolStripMenuItem.Name = "GetDetailsToolStripMenuItem";
            resources.ApplyResources(this.GetDetailsToolStripMenuItem, "GetDetailsToolStripMenuItem");
            this.GetDetailsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.GetDetailsToolStripMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // logOutMenuItem
            // 
            this.logOutMenuItem.Name = "logOutMenuItem";
            resources.ApplyResources(this.logOutMenuItem, "logOutMenuItem");
            // 
            // whitehatToolStripMenuItem1
            // 
            this.whitehatToolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.whitehatToolStripMenuItem1, "whitehatToolStripMenuItem1");
            this.whitehatToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.whitehatToolStripMenuItem1.Image = global::WhiteHatSec.VSIX.Resources.Resources.WhiteHatTextLogo;
            this.whitehatToolStripMenuItem1.Name = "whitehatToolStripMenuItem1";
            this.whitehatToolStripMenuItem1.Padding = new System.Windows.Forms.Padding(0);
            // 
            // findVulnsTabPage
            // 
            resources.ApplyResources(this.findVulnsTabPage, "findVulnsTabPage");
            this.findVulnsTabPage.Controls.Add(this.FindVulnsPanel);
            this.findVulnsTabPage.Name = "findVulnsTabPage";
            this.findVulnsTabPage.UseVisualStyleBackColor = true;
            // 
            // FindVulnsPanel
            // 
            resources.ApplyResources(this.FindVulnsPanel, "FindVulnsPanel");
            this.FindVulnsPanel.Name = "FindVulnsPanel";
            // 
            // WHSTabControl
            // 
            this.WHSTabControl.Controls.Add(this.findVulnsTabPage);
            this.WHSTabControl.Controls.Add(this.whsFindingTabPage);
            this.WHSTabControl.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.WHSTabControl, "WHSTabControl");
            this.WHSTabControl.HotTrack = true;
            this.WHSTabControl.Multiline = true;
            this.WHSTabControl.Name = "WHSTabControl";
            this.WHSTabControl.SelectedIndex = 0;
            this.WHSTabControl.TabStop = false;
            this.WHSTabControl.Click += new System.EventHandler(this.click_tab_debugVulns);
            // 
            // whsFindingTabPage
            // 
            resources.ApplyResources(this.whsFindingTabPage, "whsFindingTabPage");
            this.whsFindingTabPage.BackColor = System.Drawing.Color.White;
            this.whsFindingTabPage.Controls.Add(this.WhsFindingWindow);
            this.whsFindingTabPage.Name = "whsFindingTabPage";
            // 
            // WhsFindingWindow
            // 
            this.WhsFindingWindow.ActiveVulnId = "";
            this.WhsFindingWindow.ApiKey = "";
            applicationInfo1.Collection = null;
            applicationInfo1.ResponseMessage = null;
            this.WhsFindingWindow.AppsDetail = applicationInfo1;
            this.WhsFindingWindow.BackColor = System.Drawing.SystemColors.ActiveCaption;
            resources.ApplyResources(this.WhsFindingWindow, "WhsFindingWindow");
            this.WhsFindingWindow.IsAuthenticated = false;
            this.WhsFindingWindow.IsAuthenticatedByApiKey = false;
            this.WhsFindingWindow.Name = "WhsFindingWindow";
            this.WhsFindingWindow.Password = null;
            this.WhsFindingWindow.SentinelBrowserCookie = "";
            this.WhsFindingWindow.SentinelCookie = ((System.Net.CookieContainer)(resources.GetObject("WhsFindingWindow.SentinelCookie")));
            this.WhsFindingWindow.SentinelServerName = "";
            this.WhsFindingWindow.UserName = null;
            // 
            // cultureManager1
            // 
            this.cultureManager1.ManagedControl = this;
            // 
            // sentinelToolTip
            // 
            this.sentinelToolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.sentinelToolTip_Popup);
            // 
            // LnkLogout
            // 
            this.LnkLogout.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.LnkLogout, "LnkLogout");
            this.LnkLogout.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.LnkLogout.Name = "LnkLogout";
            this.LnkLogout.TabStop = true;
            this.LnkLogout.UseCompatibleTextRendering = true;
            this.LnkLogout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkLogout_LinkClicked);
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            resources.ApplyResources(this.toolStripMenuItemHelp, "toolStripMenuItemHelp");
            // 
            // WhsMainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LnkLogout);
            this.Controls.Add(this.WHSTabControl);
            this.Controls.Add(this.whsMenuStrip);
            this.Name = "WhsMainWindow";
            this.Load += new System.EventHandler(this.WhiteHatMainWindow_Load);
            this.whsMenuStrip.ResumeLayout(false);
            this.whsMenuStrip.PerformLayout();
            this.findVulnsTabPage.ResumeLayout(false);
            this.WHSTabControl.ResumeLayout(false);
            this.whsFindingTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// WHS Menu strip
        /// </summary>
        private MenuStrip whsMenuStrip;
        private ToolStripMenuItem whiteHatToolStripMenuItem;
        /// <summary>
        /// WhiteHat Tool strip Menu item
        /// </summary>
        private ToolStripMenuItem whitehatToolStripMenuItem1;
        /// <summary>
        /// Language Tool strip item
        /// </summary>
        private ToolStripMenuItem languageToolStripMenuItem;
        /// <summary>
        /// The import sentinel results tool strip menu item
        /// </summary>
        public ToolStripMenuItem ImportSentinelResultsToolStripMenuItem;
        /// <summary>
        /// Upload project Tool strip Menu item
        /// </summary>
        private ToolStripMenuItem uploadProjectToolStripMenuItem;
        /// <summary>
        /// Show solution tool strip menu item
        /// </summary>
        private ToolStripMenuItem showSolutionToolStripMenuItem;
        /// <summary>
        /// The get details tool strip menu item
        /// </summary>
        public ToolStripMenuItem GetDetailsToolStripMenuItem;
        /// <summary>
        /// The Tool Strip Radio Button Right
        /// </summary>
        public ToolStripRadioButtonMenuItem ToolStripRadioButtonRight;
        /// <summary>
        /// Tool strip Radio Button Bottom
        /// </summary>
        private ToolStripRadioButtonMenuItem toolStripRadioButtonBottom;
        /// <summary>
        /// English Tool strip Menu item
        /// </summary>
        public ToolStripRadioButtonMenuItem EnglishToolStripRadioButtonMenuItem;
        /// <summary>
        /// Japanese tool strip menu item
        /// </summary>
        public ToolStripRadioButtonMenuItem JapaneseToolStripRadioButtonMenuItem;
        /// <summary>
        /// German tool strip menu item
        /// </summary>
        private ToolStripRadioButtonMenuItem germanToolStripRadioButtonMenuItem;
        /// <summary>
        /// Find Vuln tab page
        /// </summary>
        private TabPage findVulnsTabPage;
        /// <summary>
        /// The WHS tab control
        /// </summary>
        public TabControl WHSTabControl;
        /// <summary>
        /// WHS finding tab page
        /// </summary>
        private TabPage whsFindingTabPage;
        /// <summary>
        /// The uc WHS finding window
        /// </summary>
        public VulnerabilityTrace WhsFindingWindow;
        /// <summary>
        /// The find vulns panel
        /// </summary>
        public Panel FindVulnsPanel;
        /// <summary>
        /// Culture manager
        /// </summary>
        private CultureManager cultureManager1;
        /// <summary>
        /// Sentinel Tooltip
        /// </summary>
        private ToolTip sentinelToolTip;
        /// <summary>
        /// Logout Button
        /// </summary>
        public LinkLabel LnkLogout;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem logOutMenuItem;
        private ToolStripMenuItem toolStripMenuItemHelp;
    }
}
