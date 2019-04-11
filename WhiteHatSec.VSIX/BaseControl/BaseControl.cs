using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using WhiteHatSec.Entity;
using System;
using System.ComponentModel;
using Microsoft.VisualStudio.PlatformUI;
using System.Drawing;

namespace WhiteHatSec.VSIX.BaseControl
{
    /// <summary>
    ///     Base Control.
    /// </summary>
    public class BaseControl : UserControl
    {
        // singleton implementation

        private static BaseControl baseInstance;
        /// <summary>
        /// Create instance
        /// </summary>
        public static BaseControl BaseInstance
        {
            get { return baseInstance ?? (baseInstance = new BaseControl()); }
        }
       
        /// <summary>
        /// Base control
        /// </summary>
        public BaseControl()
        {

            sentinelCookie = new CookieContainer();
            isAuthenticated = false;
            apiKey = string.Empty;
            sentinelServerName = string.Empty;
            isAuthenticatedByApiKey = false;
            vulnerabilityList = new List<VulnerabilityInfo.Vulnerability>();
            browserCookie = string.Empty;
            appsDetail = new ApplicationDetail.ApplicationInfo();
            activeVulnId = string.Empty;
            userName = string.Empty;
            password = string.Empty;
        }
        // end singleton implementation
       
        private CookieContainer sentinelCookie = new CookieContainer();
        private bool isAuthenticated;
        private string apiKey;
        private string sentinelServerName;
        private bool isAuthenticatedByApiKey;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private List<VulnerabilityInfo.Vulnerability> vulnerabilityList = new List<VulnerabilityInfo.Vulnerability>();
      
        private string browserCookie;
        private ApplicationDetail.ApplicationInfo appsDetail = new ApplicationDetail.ApplicationInfo();
        private string activeVulnId;
        private string userName;
        private string password;

        /// <summary>
        ///     The Sentinel Cookie to get data in next request.
        /// </summary>
        public CookieContainer SentinelCookie { get { return sentinelCookie; } set { sentinelCookie = value; } }

        /// <summary>
        ///     The is authenticated.
        /// </summary>
        public bool IsAuthenticated { get { return isAuthenticated; } set { isAuthenticated = value; } }

        /// <summary>
        ///     The API key.
        /// </summary>
        public string ApiKey { get { return apiKey; } set { apiKey = value; } }

        /// <summary>
        ///     The  Sentinel Server Name.
        /// </summary>
        public string SentinelServerName { get { return sentinelServerName; } set { sentinelServerName = value; } }

        /// <summary>
        ///     The is authenticated by API key.
        /// </summary>
        public bool IsAuthenticatedByApiKey { get { return isAuthenticatedByApiKey; } set { isAuthenticatedByApiKey = value; } }

        /// <summary>
        ///     The  vulenabirity list.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        public List<VulnerabilityInfo.Vulnerability> VulnerabilityList { get { return vulnerabilityList; } set { vulnerabilityList = value; } }

        /// <summary>
        ///     The  browser cookie for sentinel page.
        /// </summary>
        public string SentinelBrowserCookie { get { return browserCookie; } set { browserCookie = value; } }
        /// <summary>
        ///     The apps detail.
        /// </summary>
        public ApplicationDetail.ApplicationInfo AppsDetail { get { return appsDetail; } set { appsDetail = value; } }
        /// <summary>
        ///     The  active vuln identifier.
        /// </summary>
        public string ActiveVulnId { get { return activeVulnId; } set { activeVulnId = value; } }

        /// <summary>
        ///     Gets or sets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get { return userName; } set { userName = value; } }

        /// <summary>
        ///     Gets or sets the  password.
        /// </summary>
        /// <value>
        ///     The  password.
        /// </value>
        public string Password { get { return password; } set { password = value; } }
        /// <summary>
        /// Reset data
        /// </summary>
        public static void Reset()
        {
            baseInstance = null;
        }

        public Color CurrentThemeForColor { get { return VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey); } }
        public Color CurrentThemeBorderColor { get {return VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTabBorderColorKey); } }
        public Color CurrentThemeDropDownBorderColor { get { return VSColorTheme.GetThemedColor(EnvironmentColors.DropDownBorderColorKey); } }
        public Color CurrentThemeBackColor { get { return VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey); } }
    }
}