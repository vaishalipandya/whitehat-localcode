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

      
        
        private void helpMenuItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://na27.salesforce.com/sfc/p/#3000000007SE/a/33000000Phxa/MWav20lz_18YaGE9ilg6fQ9k7vlrdbobGIrksBrrq7c");
        }
        
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
       
        private void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            SetDefaultColors();
        }
        private void TabPages_DrawItem(object sender, DrawItemEventArgs e)
        {
            //e.DrawBackground();

            Brush backBrush;
            Brush foreBrush;

            Graphics g = e.Graphics;
            Pen p = new Pen(CurrentThemeBackColor, 10);
            g.DrawRectangle(p, this.WHSTabControl.Bounds);


            backBrush = new SolidBrush(CurrentThemeBackColor);
            foreBrush = new SolidBrush(CurrentThemeForColor);

            e.Graphics.FillRectangle(backBrush, e.Bounds);

            //You may need to write the label here also?
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;

            Rectangle r = e.Bounds;
            r = new Rectangle(r.X, r.Y + 3, r.Width + 20, r.Height - 3);
            e.Graphics.DrawString(WHSTabControl.TabPages[e.Index].Text, e.Font, foreBrush, r, sf);

            Rectangle lasttabrect = WHSTabControl.GetTabRect(WHSTabControl.TabPages.Count - 1);
            Rectangle background = new Rectangle();
            background.Location = new Point(lasttabrect.Right, 0);

            //pad the rectangle to cover the 1 pixel line between the top of the tabpage and the start of the tabs
            background.Size = new Size(WHSTabControl.Right - background.Left, lasttabrect.Height + 5);
            e.Graphics.FillRectangle(backBrush, background);
        }
        public void reNameManageVulnsTab(Boolean isLoggedIn)
        {
            if (!isLoggedIn)
            {
                logOutMenuItem.Visible = false;
                WHSTabControl.TabPages[0].Text = "Log In";
            }else
            {
                logOutMenuItem.Visible = true;
                WHSTabControl.TabPages[0].Text = "Manage Vulnerabilities";
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
            toolStripRadioButtonBottom.Checked = true;
            ToolStripRadioButtonRight.Checked = false;
            //LnkLogout.Visible = false;
            EnglishToolStripRadioButtonMenuItem.Checked = true;

            BaseInstance.VulnerabilityList.Clear();
            WhsFindingWindow.TxtFolderPath.Text = string.Empty;
            WhsFindingWindow.GenerateVulnTrace(0, Ascending);
            WhsFindingWindow.WebBrowserSnippet.DocumentText = string.Empty;
            WhsFindingWindow.WHSSolutionTabControl.SelectedIndex = 0;
            WhsFindingWindow.WebBrowserDescription.DocumentText = string.Empty;
            WhsFindingWindow.WebBrowserSolution.DocumentText = string.Empty;
            WhsFindingWindow.QaFilterInfo.LoadQuestionAnswer();
            WhsFindingWindow.QaFilterInfo.BindQaData("", Ascending);
            SentinelBrowserCookie = string.Empty;
            SentinelCookie = new CookieContainer();

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

        
        ///// <summary>
        /////     Handles the Click event of the whsComMenuItem_Click control to change culture to japanese.
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //private void whsComMenuItem_Click(object sender, EventArgs e)
        //{
        //    currentServiceLocation = ServiceLocation.WHS_US;
        //    Settings.Default.ServiceLocation = currentServiceLocation;
        //    Settings.Default.Save();
        //}

        //        /// <summary>
        /////     Handles the Click event of the whsEuMenuItem_Click control to change culture to japanese.
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        //private void whsEuMenuItem_Click(object sender, EventArgs e)
        //{
        //    currentServiceLocation = ServiceLocation.WHS_EU;
        //    Settings.Default.ServiceLocation = currentServiceLocation;
        //    Settings.Default.Save();
        //}

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
            //whitehatToolStripMenuItem1.BackColor = System.Drawing.ColorTranslator.FromHtml("#019EDF");
            whitehatToolStripMenuItem1.BackColor = Color.White;
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