using System;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Web;
using System.Windows.Forms;
using WhiteHatSec.Entity;
using WhiteHatSec.Localization.Culture.Resource;
using WhiteHatSec.VSIX.Utility;
using WhiteHatSec.Services;
using log4net;
using WhiteHatSec.Shared;
using System.Text;
using System.Collections.Specialized;
using EnvDTE80;
using Microsoft.VisualStudio.PlatformUI;
using System.Drawing;
using System.Collections.Generic;

namespace WhiteHatSec.VSIX.UserControls
{
    /// <summary>
    ///     Whs main window control
    /// </summary>
    public partial class WhsMainWindow : BaseControl.BaseControl
    {
        #region "Constructor"

        /// <summary>
        ///     Initializes a new instance of the <see cref="WhsMainWindow" /> class.
        /// </summary>
        public WhsMainWindow()
        {
            try
            {
                InitializeComponent();
                this.currentServiceLocation = Settings.Default.ServiceLocation;
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                languageToolStripMenuItem.Enabled = false;
                languageToolStripMenuItem.Visible = false;
                GetDetailsToolStripMenuItem.Enabled = false;
                GetDetailsToolStripMenuItem.Visible = false;
                SetDefaultColors();
                VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
                whsMenuStrip.BringToFront();
                //Assign parent Form to child control
                WhsFindingWindow.ParentWhsWindow = this;
                WHSTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
                WHSTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabPages_DrawItem);
                //Load Login Form
                LoadLogin();
                if (currentServiceLocation == ServiceLocation.WHS_US)
                {
                    login.whsCOMradioButton.Checked = true;
                }
                else
                {
                    login.whsEUradioButton.Checked = true;
                }
                logOutMenuItem.Click += LogOutMenuItem_Click;
                toolStripMenuItemHelp.Click += helpMenuItemClick;                   
                reNameManageVulnsTab(false);
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On InitializeComponent*****", ex);
            }
        }

      
        /// <summary>
        /// Handle click event of Help context menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://na27.salesforce.com/sfc/p/#3000000007SE/a/33000000Phxa/MWav20lz_18YaGE9ilg6fQ9k7vlrdbobGIrksBrrq7c");
        }
        /// <summary>
        /// Handles log out menu click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOutMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                logOut();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error("****Error Occured On Logout Click.*****", ex);
                MessageBox.Show("Error occured during LogOut", MessageLog.ErrorMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

    #endregion

    #region "Properties and varibles declaration"

    private SentinelLogin sentinelLogin;

        public ServiceLocation currentServiceLocation;

        /// <summary>
        ///     The object zip project
        /// </summary>
        public UploadProject UploadProjectInfo;
        /// <summary>
        /// Http text
        /// </summary>
        public const string HttpText = "http://";
        /// <summary>
        /// Https text
        /// </summary>
        public const string HttpsText = "https://";
        /// <summary>
        /// Ascending sort ditection
        /// </summary>
        public const string Ascending = "Ascending";

        private DTE2 visualStudioCurrentInstance;
        /// <summary>
        ///     The current instance of Visual studio.
        /// </summary>
        public DTE2 VisualStudioCurrentInstance { get { return visualStudioCurrentInstance; } set { visualStudioCurrentInstance = value; } }

        /// <summary>
        /// Logger Instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);

        private Login login;

        #endregion

        #region "Events"
       /// <summary>
       /// Handle the theme change event of Visual studio.
       /// </summary>
       /// <param name="e"></param>
        private void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            SetDefaultColors();
        }

        /// <summary>
        /// Handle Tab draw event of Tabs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabPages_DrawItem(object sender, DrawItemEventArgs e)
        {

            Brush backBrush;
            Brush foreBrush;

            Graphics g = e.Graphics;
            Pen p = new Pen(CurrentThemeBackColor, 10);
            g.DrawRectangle(p, this.WHSTabControl.Bounds);


            backBrush = new SolidBrush(CurrentThemeBackColor);
            foreBrush = new SolidBrush(CurrentThemeForColor);

            e.Graphics.FillRectangle(backBrush, e.Bounds);

            //You may need to write the label here also?
            StringFormat tabSurroundingformat = new StringFormat();
            tabSurroundingformat.Alignment = StringAlignment.Near;

            Rectangle rect = e.Bounds;
            rect = new Rectangle(rect.X, rect.Y + 3, rect.Width + 20, rect.Height - 3);
            e.Graphics.DrawString(WHSTabControl.TabPages[e.Index].Text, e.Font, foreBrush, rect, tabSurroundingformat);

            Rectangle remainngEmptyRect = WHSTabControl.GetTabRect(WHSTabControl.TabPages.Count - 1);
            Rectangle background = new Rectangle();
            background.Location = new Point(remainngEmptyRect.Right, 0);

            //pad the rectangle to cover the 1 pixel line between the top of the tabpage and the start of the tabs
            background.Size = new Size(WHSTabControl.Right - background.Left, remainngEmptyRect.Height + 5);
            e.Graphics.FillRectangle(backBrush, background);
        }
        /// <summary>
        /// Renames Login tab to ManageVulnerabilities tab.
        /// </summary>
        /// <param name="isLoggedIn"></param>
        public void reNameManageVulnsTab(Boolean isLoggedIn)
        {
            if (!isLoggedIn)
            {
                logOutMenuItem.Visible = false;              
                WHSTabControl.TabPages[0].Text = "Log In";
            }else
            {
                logOutMenuItem.Visible = true;              
                WHSTabControl.TabPages[0].Text = MessageLog.ManageVulnerabilities; ;
            }
        }
        /// <summary>
        /// Show/Hide display vulnerabilities tab based on condition.
        /// </summary>
        /// <param name="show"></param>
        public void showHideDebugVulnTab(bool show)
        {
            if (show)
            {
                WHSTabControl.TabPages.Add(whsFindingTabPage);
            }
            else
            {
                WHSTabControl.TabPages.Remove(whsFindingTabPage);
            }
        }
        /// <summary>
        ///     Handles the Load event of the WhiteHatMainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void WhiteHatMainWindow_Load(object sender, EventArgs e)
        {
            try
            {
                whsMenuStrip.BringToFront();
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Load of Window*****", ex);
            }
        }

        /// <summary>
        ///     Handles the LinkClicked event of the LnkLogout control for LogOut.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs" /> instance containing the event data.</param>
        private void LnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                logOut();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error("****Error Occured On Logout Click.*****", ex);
                MessageBox.Show("Error occured during LogOut", MessageLog.ErrorMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Log out processing method.
        /// </summary>
        private void logOut()
        {
            BaseInstance.SentinelServerName = string.Empty;
            BaseControl.BaseControl.Reset();
            WHSTabControl.SelectedTab = findVulnsTabPage;

            Login login = new Login();
            login.Controls.Clear();
            UserName = string.Empty;
            Password = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            CultureManager.ApplicationUiCulture = new CultureInfo("en");

            FindVulnsPanel.Controls.Clear();
            VulnerabilityTrace vulnerabilityTrace = new VulnerabilityTrace { ParentWhsWindow = this };
            UploadProject uploadProject = new UploadProject { ParentWhsWindow = this };
            WHSTabControl.SelectedIndex = 0;
            LoadLogin();
            GetDetailsToolStripMenuItem.Checked = true;
            toolStripRadioButtonBottom.Checked = false;
            ToolStripRadioButtonRight.Checked = true;
            EnglishToolStripRadioButtonMenuItem.Checked = true;
            
            SentinelBrowserCookie = string.Empty;
            SentinelCookie = new CookieContainer();
            WhsFindingWindow.ClearFindingsData();
            WhsFindingWindow.ChangeOrientation(Orientation.Vertical);
            Cursor.Current = Cursors.Default;
            reNameManageVulnsTab(false);
          }

        #region "Context Menu Events"

        /// <summary>
        ///     Handles the Checked Changed event of the ToolStripRadioButtonRight control for Right orientation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ToolStripRadioButtonRight_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                WhsFindingWindow.ChangeOrientation(ToolStripRadioButtonRight.Checked
                    ? Orientation.Vertical
                    : Orientation.Horizontal);
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Checking show solution right*****", ex);
                MessageBox.Show("Error occured while checking show solution to right.", MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Checked Changed event of the toolStripRadioButtonBottom control  Right orientation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ToolStripRadioButtonBottom_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (toolStripRadioButtonBottom.Checked)
                {
                    WhsFindingWindow.ChangeOrientation(Orientation.Horizontal);
                }
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Checking show solution bottom*****", ex);
                MessageBox.Show(
                    "Error occured while checking show solution to bottom.",
                    MessageLog.ErrorMessage,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        /// <summary>
        ///     Handles the Click event of the JapaneseToolStripRadioButtonMenuItem control to change culture to japanese.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void JapaneseToolStripRadioButtonMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeAppCulture("ja");

                if (BaseInstance.IsAuthenticatedByApiKey || BaseInstance.IsAuthenticated)
                {
                    reNameManageVulnsTab(true);
                    //LnkLogout.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Checking language japanese*****", ex);
                MessageBox.Show("Error occured while changing language to japanese.", MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Click event of the germanToolStripRadioButtonMenuItem control to change culture to german.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void GermanToolStripRadioButtonMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeAppCulture("de");

                if (BaseInstance.IsAuthenticatedByApiKey || BaseInstance.IsAuthenticated)
                {
                    reNameManageVulnsTab(true);
                    //LnkLogout.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Checking show solution German*****", ex);
                MessageBox.Show("Error occured while changing language to german.", MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Click event of the EnglishToolStripRadioButtonMenuItem control to change culture to english.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void EnglishToolStripRadioButtonMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                ChangeAppCulture("en");
                if (BaseInstance.IsAuthenticatedByApiKey || BaseInstance.IsAuthenticated)
                {
                    reNameManageVulnsTab(true);
                    //LnkLogout.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Checking show solution english*****", ex);
                MessageBox.Show("Error occured while changing language to english.", MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Click event of the uploadProjectToolStripMenuItem control for upload project.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void UploadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowUploadProject();

            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Checking Upload project*****", ex);
                MessageBox.Show("Error occured while checking upload project.", MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Click event of the ImportSentinelResultsToolStripMenuItem control to import file manually.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ImportSentinelResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (WhsFindingWindow == null)
                {
                    return;
                }

                WhsFindingWindow.ImportSentinelResultsToolStripMenuItem_Click(
                    ImportSentinelResultsToolStripMenuItem, e);
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Import Sentinel*****", ex);
                MessageBox.Show("Error occured while importing sentinel", MessageLog.ErrorMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Checked Changed event of the GetDetailsToolStripMenuItem control to display solution and description.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void GetDetailsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                WhsFindingWindow.GetDetailsToolStripMenuItem_CheckedChanged(null, null);
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured Checking or Unchecking  get Details Menu Context.*****", ex);
            }
        }

        #endregion


        #endregion

        #region "Methods"
       
        /// <summary>
        /// Get Vulerability Detail By Vuln Id
        /// </summary>
        /// <param name="vulnId">vuln Id</param>
        ///  <param name="sentinelServer">sentinel Server</param>
        public VulnerabilityInfo.Vulnerability GetVulnDetail(string vulnId, string sentinelServer)
        {
            VulnerabilityInfo.Vulnerability vulnerability = new VulnerabilityInfo.Vulnerability();
            try
            {
                Log.InfoFormat("**** Clicked on Vuln. Id : {0} in sentinel tab ****", vulnId);
                vulnerability = VulnerabilityService.GetSentinelVulnDetail(sentinelServer, vulnId,
                    SentinelBrowserCookie, this.GetDetailsToolStripMenuItem.Checked);

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message, MessageLog.ErrorMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Log.Error(ex.Message);
                Cursor.Current = Cursors.Default;
            }
            return vulnerability;
        }
        /// <summary>
        ///   Load  Logins the form.
        /// </summary>
        public void LoadLogin()
        {
            try
            {
                WHSTabControl.SelectedIndex = 0;
                string server = "";
                if (login != null && login.serverTextBox != null)
                {
                    server = login.serverTextBox.Text;
                }else
                {
                    server = Constant.whsUsUrl;
                }
                login = new Login
                {
                    Dock = DockStyle.Fill,
                    ParentWhsWindow = this
                };
                login.serverTextBox.Text = server;
                showHideDebugVulnTab(false);
                FindVulnsPanel.Controls.Add(login);
                FindVulnsPanel.BackColor = CurrentThemeBackColor;
                FindVulnsPanel.ForeColor = CurrentThemeForColor;
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Loading Login Form*****", ex);
            }
        }

        /// <summary>
        ///     Shows the sentinel login pop up.
        /// </summary>
        public void ShowLogin()
        {
            try
            {
                if (string.IsNullOrEmpty(SentinelServerName) || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {
                    sentinelLogin = new SentinelLogin { StartPosition = FormStartPosition.CenterParent };
                    sentinelLogin.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }
        /// <summary>
        /// Changes the User experience based on theme.
        /// </summary>
        void SetDefaultColors()
        {

            FindVulnsPanel.BackColor = CurrentThemeBackColor;
            FindVulnsPanel.ForeColor = CurrentThemeForColor;
            whiteHatToolStripMenuItem.BackColor = CurrentThemeBackColor;
            whiteHatToolStripMenuItem.ForeColor = CurrentThemeForColor;
            toolStripMenuItem1.BackColor = CurrentThemeBackColor;
            toolStripMenuItem1.ForeColor = CurrentThemeForColor;
            toolStripMenuItemHelp.BackColor = CurrentThemeBackColor;
            toolStripMenuItemHelp.ForeColor = CurrentThemeForColor;
            logOutMenuItem.BackColor = CurrentThemeBackColor;
            logOutMenuItem.ForeColor = CurrentThemeForColor;
            showSolutionToolStripMenuItem.BackColor = CurrentThemeBackColor;
            showSolutionToolStripMenuItem.ForeColor = CurrentThemeForColor;
            whiteHatToolStripMenuItem.BackColor = CurrentThemeBackColor;
            whiteHatToolStripMenuItem.ForeColor = CurrentThemeForColor;
            ToolStripRadioButtonRight.BackColor = CurrentThemeBackColor;
            ToolStripRadioButtonRight.ForeColor = CurrentThemeForColor;
            toolStripRadioButtonBottom.BackColor = CurrentThemeBackColor;
            toolStripRadioButtonBottom.ForeColor = CurrentThemeForColor;
            languageToolStripMenuItem.BackColor = CurrentThemeBackColor;
            languageToolStripMenuItem.ForeColor = CurrentThemeForColor;
            EnglishToolStripRadioButtonMenuItem.BackColor = CurrentThemeBackColor;
            EnglishToolStripRadioButtonMenuItem.ForeColor = CurrentThemeForColor;
            JapaneseToolStripRadioButtonMenuItem.BackColor = CurrentThemeBackColor;
            JapaneseToolStripRadioButtonMenuItem.ForeColor = CurrentThemeForColor;
            germanToolStripRadioButtonMenuItem.BackColor = CurrentThemeBackColor;
            germanToolStripRadioButtonMenuItem.ForeColor = CurrentThemeForColor;
            BackColor = CurrentThemeBackColor;
            ForeColor = CurrentThemeForColor;
            WHSTabControl.Invalidate();
        }

        /// <summary>
        /// Change culture 
        /// </summary>
        /// <param name="cultureName">Culture name</param>
        public void ChangeAppCulture(string cultureName)
        {
            CultureManager.ApplicationUiCulture = new CultureInfo(cultureName);
        }
        /// <summary>
        /// Show dialog for  upload project
        /// </summary>
        public void ShowUploadProject()
        {
            UploadProjectInfo = new UploadProject { StartPosition = FormStartPosition.CenterParent, ParentWhsWindow = this };
            UploadProjectInfo.ShowDialog();
        }


        #endregion
        
        private void whsMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void sentinelToolTip_Popup(object sender, PopupEventArgs e)
        {

        }
        /// <summary>
        /// Change Selected vuln tab based on selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_tab_debugVulns(object sender, EventArgs e)
        {
            if (!BaseInstance.IsAuthenticated && !BaseInstance.IsAuthenticatedByApiKey)
            {
                WHSTabControl.SelectedTab = findVulnsTabPage;
                MessageBox.Show(MessageLog.SentinelLoginRequired, MessageLog.Message, MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }
    }
}