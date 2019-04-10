using System;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using WhiteHatSec.Localization.Culture.Resource;
using WhiteHatSec.VSIX.UserControls.Search;
using WhiteHatSec.VSIX.Utility;
using WhiteHatSec.Services;
using log4net;
using WhiteHatSec.Shared;
using WhiteHatSec.Entity;
using Microsoft.VisualStudio.PlatformUI;

namespace WhiteHatSec.VSIX.UserControls
{
    /// <summary>
    ///     Login Control
    /// </summary>
    public partial class Login
    {
        #region "Constructor"

        /// <summary>
        ///     Login
        /// </summary>
        public Login()
        {
            try
            {
                InitializeComponent();
                //if (ParentWhsWindow.currentServiceLocation == ServiceLocation.WHS_US)
                //{
                //    whsCOMradioButton.Checked = true;
                //}
                //else
                //{
                //    whsEUradioButton.Checked = true;
                //}
                SetDefaultColors();
                VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                serverTextBox.Text = Constant.whsUsUrl;
                txtUserName.Text = userNameRetain;
                UserName = userNameRetain;
            }
            catch (Exception ex)
            {
                Log.Error("**** Error Occured On InitializeComponent ***** ", ex);
            }
        }

        #endregion

        #region "Properties"

        private static string userNameRetain = "";

        /// <summary>
        ///     Gets the form that the container control is assigned to parent whiteHat window.
        /// </summary>     
        public WhsMainWindow ParentWhsWindow;
        /// <summary>
        /// Logger Instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);

        private string serverName;

        #endregion

        #region "Events"
        void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            SetDefaultColors();
        }
        /// <summary>
        ///     Handles the Click event of the btnLogin control to login.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // empty user Id password
                if ((!string.IsNullOrEmpty(txtUserName.Text.Trim()) && !string.IsNullOrEmpty(txtPassword.Text)) &&
                      !string.IsNullOrEmpty(txtAPIKey.Text.Trim()))
                {
                    MessageBox.Show("Sorry, you can either use the Sentinel credentials or use the API key to login, but not both.", MessageLog.PleaseEnterCredentialDetail, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ParentWhsWindow.reNameManageVulnsTab(false);
                    return;
                }

                // empty user Id password
                if ((string.IsNullOrEmpty(txtUserName.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Text)) &&
                      string.IsNullOrEmpty(txtAPIKey.Text.Trim()))
                {
                    MessageBox.Show(MessageLog.PleaseEnterCredentialDetail, MessageLog.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ParentWhsWindow.reNameManageVulnsTab(false);
                    return;
                }

                //Check authentication by UserId and password
                if (!string.IsNullOrEmpty(txtUserName.Text.Trim()) && !string.IsNullOrEmpty(txtPassword.Text))
                {
                    if (ValidateUser(txtUserName.Text.Trim(), txtPassword.Text.Trim()))
                    {
                        ParentWhsWindow.reNameManageVulnsTab(true);
                    }
                    return;
                }

                //Check authentication By API key
                if (ValidateApiKey(txtAPIKey.Text.Trim()))
                {
                    ParentWhsWindow.reNameManageVulnsTab(true);
                }
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ParentWhsWindow.reNameManageVulnsTab(false);
                Cursor.Current = Cursors.Default;
                Log.Error("**** Error Occured On Login of sentinel ***** ", ex);
                MessageBox.Show("Error Occured on Login of sentinel.", MessageLog.ErrorMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the DocumentCompleted event of the webBrowserSentinel control for auto login sentinel functionality.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="WebBrowserDocumentCompletedEventArgs" /> instance containing the event data.</param>
        private void WebBrowserSentinel_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (!BaseInstance.IsAuthenticated)
                {
                    return;
                }

                WebBrowser sentinelBrowser = (WebBrowser)sender;
                HtmlElement userName = Common.GetInputElementByName("username", sentinelBrowser);
                if (userName == null) return;
                userName.SetAttribute("value", ParentWhsWindow.UserName);
                HtmlElement password = Common.GetInputElementByName("password", sentinelBrowser);
                password.SetAttribute("value", ParentWhsWindow.Password);
                HtmlElement login = Common.GetInputElementByValue("Login", sentinelBrowser);
                login.InvokeMember("click");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error("****Error Occured On sentinel browser DocumentCompleted event***** ", ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        ///     A <see cref="T:System.Windows.Forms.Message" />, Press Enter key to login so override forms method
        ///     message to process.
        /// </param>
        /// <param name="keyPressed">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
        /// <returns>
        ///     true if the character was processed by the control; otherwise, false.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message message, Keys keyPressed)
        {
            try
            {
                //Press Enter key
                if (keyPressed == Keys.Enter)
                {
                    BtnLogin_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On PressEnterKeyForLogin event***** ", ex);
            }
            return base.ProcessCmdKey(ref message, keyPressed);
        }

        #endregion

        #region "Methods"

        private string getServerLocation()
        {
            return serverTextBox.Text;
            //string ret = Constant.whsUsUrl;
            //switch (ParentWhsWindow.currentServiceLocation)
            //{
            //    case ServiceLocation.WHS_EU:
            //        ret = Constant.whsEuUrl;
            //        break;
            //    case ServiceLocation.WHS_US:
            //        ret = Constant.whsUsUrl;
            //        break;
            //}
            //return ret;
        }
        void SetDefaultColors()
        {
            serverTextBox.BackColor = CurrentThemeBackColor;
            serverTextBox.ForeColor = CurrentThemeForColor;
            txtAPIKey.BackColor = CurrentThemeBackColor;
            txtAPIKey.ForeColor = CurrentThemeForColor;
            txtUserName.BackColor = CurrentThemeBackColor;
            txtUserName.ForeColor = CurrentThemeForColor;
            txtPassword.BackColor = CurrentThemeBackColor;
            txtPassword.ForeColor = CurrentThemeForColor;
            txtVersion.BackColor = CurrentThemeBackColor;
            txtVersion.ForeColor = CurrentThemeForColor;
            groupBox2.BackColor = CurrentThemeBackColor;
            groupBox2.ForeColor = CurrentThemeForColor;
            btnLogin.BackColor = CurrentThemeBackColor;
            btnLogin.ForeColor = CurrentThemeForColor;
            ForeColor = CurrentThemeForColor;
            BackColor = CurrentThemeBackColor;
            lblOr.ForeColor = CurrentThemeForColor;
            lblOr.BackColor = CurrentThemeBackColor;
        }
        /// <summary>
        /// Validate User By API key
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="apiKey"></param>
        public bool ValidateApiKey(string apiKey)
        {
            string serverName = getServerLocation();

            try
            {
                if (string.IsNullOrEmpty(apiKey))
                {
                    MessageBox.Show(MessageLog.PleaseEnterCredentialDetail, MessageLog.Message, MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return false;
                }

                BaseInstance.AppsDetail = GetApplicationsByKey(serverName, apiKey);
                if (BaseInstance.AppsDetail == null || BaseInstance.AppsDetail.ResponseMessage != "true")
                {
                    //Invalid Api key 
                    BaseInstance.IsAuthenticatedByApiKey = false;

                    if (BaseInstance.AppsDetail.ResponseMessage.Equals(MessageLog.AuthenticationFailed))
                    {
                        BaseInstance.AppsDetail.ResponseMessage = "Sorry, the API key isn't correct. Get API key from https://source.whitehatsec.com/profile.html#/my/profile.";
                    }
                    //else
                    //{
                    //    BaseInstance.AppsDetail.ResponseMessage = MessageLog.AuthenticationFailed;
                    //}

                    
                    MessageBox.Show(BaseInstance.AppsDetail.ResponseMessage, MessageLog.Message,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    BaseControl.BaseControl.Reset();
                }
                else
                {
                    //Load sentinel find vuln tab
                    BaseInstance.SentinelServerName = serverName.Trim();
                    LoadFindings(serverName);
                    BaseInstance.IsAuthenticatedByApiKey = true;
                    BaseInstance.ApiKey = apiKey;
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageLog.AuthenticationFailedPleaseEnterCorrectCredential, MessageLog.AuthenticationFailed, MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
            }
            return false;
        }

        /// <summary>
        /// Validate User by username and password
        /// </summary>
        /// <param name="sentinelServer"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>       
        public bool ValidateUser( string username, string password)
        {
            string sentinelServer = getServerLocation();

            try
            {
                CookieContainer sentinelCookie = new CookieContainer();
                string isValid = ValidUser(sentinelServer, username, password, sentinelCookie);
                if (isValid != "true")
                {
                    if (string.IsNullOrEmpty(isValid))
                    {
                        isValid = MessageLog.AuthenticationFailed;
                    }
                    MessageBox.Show(isValid, MessageLog.Message, MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    userNameRetain = username;
                    BaseInstance.SentinelCookie = sentinelCookie;
                    BaseInstance.SentinelServerName = sentinelServer;
                    BaseInstance.IsAuthenticated = true;
                    ParentWhsWindow.UserName = username;
                    ParentWhsWindow.Password = password;
                    BaseInstance.AppsDetail = GetApplications(BaseInstance.SentinelServerName, BaseInstance.SentinelCookie);
                    LoadFindings(BaseInstance.SentinelServerName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageLog.AuthenticationFailed, MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
            }
            return false;
        }
        /// <summary>
        /// Check for Valid User
        /// </summary>
        /// <param name="sentinelServer"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="sentinelCookie"></param>
        /// <returns></returns>
        public string ValidUser(string sentinelServer, string username, string password, CookieContainer sentinelCookie)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return "false";
            }
            return AuthenticationService.Authentication(sentinelServer, username, password, sentinelCookie);
        }
        /// <summary>
        /// Get application By sentinel cookie
        /// </summary>
        public ApplicationDetail.ApplicationInfo GetApplications(string server, CookieContainer sentinelCookie)
        {
            if (sentinelCookie == null)
            {
                return new ApplicationDetail.ApplicationInfo();
            }
            return ApplicationService.GetAppsByCookie(server, sentinelCookie);

        }
        /// <summary>
        /// Get application by Valid Api key
        /// </summary>
        /// <param name="server"></param>
        /// <param name="apiKey"></param>
        public ApplicationDetail.ApplicationInfo GetApplicationsByKey(string server, string apiKey)
        {
            return ApplicationService.GetApps(server, apiKey.Trim());
        }
        /// <summary>
        ///     Redirects after login to find vuls page.
        /// </summary>
        /// <param name="server">The server.</param>
        public void LoadFindings(string server)
        {
            try
            {
                Log.Info("****Going to redirect after login in to Sentinel****");
                //ParentWhsWindow.LnkLogout.Visible = true;
                ParentWhsWindow.FindVulnsPanel.Controls.Clear();
                SearchVulnerability findVulnsSearch = new SearchVulnerability
                {
                    Dock = DockStyle.Fill,
                    SearchWhiteHatWindow = ParentWhsWindow,
                    VulnerabilityTraceInfo = ParentWhsWindow.WhsFindingWindow
                };
                ParentWhsWindow.FindVulnsPanel.Controls.Add(findVulnsSearch);
                BaseInstance.SentinelServerName = server;

                Log.Info("****Successfully redirect after login in to Sentinel****");
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured during Redirect to vunerability form***** ", ex);
            }
        }

        #endregion

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(whsCOMradioButton.Checked == true)
            {
                ParentWhsWindow.currentServiceLocation = ServiceLocation.WHS_US;
                Settings.Default.ServiceLocation = ParentWhsWindow.currentServiceLocation;
                Settings.Default.Save();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (whsEUradioButton.Checked == true)
            {
                ParentWhsWindow.currentServiceLocation = ServiceLocation.WHS_EU;
                Settings.Default.ServiceLocation = ParentWhsWindow.currentServiceLocation;
                Settings.Default.Save();
            }
        }

        private void serverTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}