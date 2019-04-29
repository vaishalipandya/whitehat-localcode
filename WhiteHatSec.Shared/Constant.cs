using System;
using EnvDTE80;

using System.Web;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using EnvDTE;

namespace WhiteHatSec.Shared
{

    /// <summary>
    ///     All constant data
    /// </summary>
    public class Constant
    {
        public static string whsUsUrl = "sentinel.whitehatsec.com";
        public static string whsEuUrl = "sentinel.whitehatsec.eu";

        public static DTE2 dte { get; set; }
        /// <summary>
        ///     The line bonudry for match code in file.
        /// </summary>
        public static int BoundryLines = 30;

        public static string getMetricsString()
        {
            DTE dte = null;
            foreach (var item in GetCurrentVisualStudioInstance())
            {
                dte = item;
                break;
            }
            var dte2 = (DTE2)dte;
            string vsVersion = dte2.Version;
            string osVersion = Environment.OSVersion.Version.ToString();
            string rawMetrics = string.Format("\"{{\"source_name\": \"Visual Studio\",\"source_version\":\"{0}\",\"plugin_version\" : \"1.1\",\"OS\":\"Windows_{1}\"}}\"", vsVersion, osVersion);
            string urlEncodedMetrics = HttpUtility.UrlEncode(rawMetrics);
            return string.Format("&source={0}", urlEncodedMetrics);
        }
        public static IEnumerable<DTE> GetCurrentVisualStudioInstance()
        {
            IRunningObjectTable rot;
            IEnumMoniker enumMoniker;
            int retVal = GetRunningObjectTable(0, out rot);

            if (retVal == 0)
            {
                rot.EnumRunning(out enumMoniker);

                IntPtr fetched = IntPtr.Zero;
                IMoniker[] moniker = new IMoniker[1];
                while (enumMoniker.Next(1, moniker, fetched) == 0)
                {
                    IBindCtx bindCtx;
                    CreateBindCtx(0, out bindCtx);
                    string displayName;
                    moniker[0].GetDisplayName(bindCtx, null, out displayName);
                    Console.WriteLine("Display Name: {0}", displayName);
                    bool isVisualStudio = displayName.StartsWith("!VisualStudio");
                    if (isVisualStudio)
                    {
                        object obj;
                        rot.GetObject(moniker[0], out obj);
                        var dte = obj as DTE;
                        yield return dte;
                    }
                }
            }
        }
        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);
        public static string PatchLibDownloadUrl = "https://na27.salesforce.com/sfc/p/#3000000007SE/a/33000000Pi6m/16L001nzRFgSMacw1PCThaW7BzU9d.HU7lk98ZWWvu0";
        public static string LicenseURL = "https://www.whitehatsec.com/terms-conditions/ide-plugin";

        public static string CleanUrl(string url)
        {
            if (url.Contains("://"))
            {
                int first = url.IndexOf("://") + 3;
                int length = url.Length - first;
                url = url.Substring(first, length);
            }
            return url;
        }
        /// <summary>
        ///     The sentine default login.
        /// </summary>
        public static string SentineDefaultLogin = "https://sentinel.whitehatsec.com/gateway/login.html";

        /// <summary>
        ///     The sentinel default server.
        /// </summary>
        public static string DefaultSentinelServerUrl = "sentinel.whitehatsec.com";

        /// <summary>
        ///     The accept file type.
        /// </summary>
        public static string AcceptFileType = "text/html, application/xhtml+xml, */*";

        /// <summary>
        ///     The user agent.
        /// </summary>
        public static string UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";

        /// <summary>
        ///     The content type.
        /// </summary>
        public static string ContentType = "application/x-www-form-urlencoded";

        /// <summary>
        ///     The accept language.
        /// </summary>
        public static string AcceptLanguage = "en-US,en;q=0.8,de-DE;q=0.5,de;q=0.3";

        /// <summary>
        ///     The accept encoding.
        /// </summary>
        public static string AcceptEncodingType = "gzip, deflate";

        /// <summary>
        ///     The cache control.
        /// </summary>
        public static string CacheControl = "no-cache";

        /// <summary>
        ///     The content type JSON.
        /// </summary>
        public static string ContentTypeJson = "application/json";

        /// <summary>
        ///     Application Finding URL
        /// </summary>
        public static string AppFindingsUrl = "application_findings_detail.html";

        /// <summary>
        ///     Summary URL
        /// </summary>
        public static string SummaryUrl = "summary.html";

        /// <summary>
        ///     Finding URL
        /// </summary>
        public static string FindingUrl = "findings.html";

        /// <summary>
        ///     Login URL
        /// </summary>
        public static string LoginUrl = "https://{0}/gateway/login.html";

        /// <summary>
        ///     Import File type filter
        /// </summary>
        public static string ImportFileTypeFilter = "Text file (*.txt)|*.txt|JSON documents (.json)|*.json";

        /// <summary>
        /// User Login URL
        /// </summary>
        public static string UserLoginUrl = "https://{0}/api/user/{1}/login";

        /// <summary>
        ///     Application By API key URL
        /// </summary>
        public static string AppsByApiKeyUrl = "https://{0}//api/application/?key={1}&format=json";

        /// <summary>
        ///      Application   URL By Sentinel Cookie
        /// </summary>
        public static string AppsByCookieUrl = "https://{0}//api/application/?format=json";

        /// <summary>
        ///      Application Vuln By Sentinel Cookie URL
        /// </summary>
        public static string VulnByAppUrl = "https://{0}//api/application/{1}/vuln?query_risk={2}&query_status={3}&query_start_date={4}&query_end_date={5}&format=json";

        /// <summary>
        ///     Application Vuln By Api key
        /// </summary>

       public static string VulnByApiKeyUrl =  "https://{0}/api/application/{1}/vuln?query_risk={2}&query_status={3}&query_start_date={4}&query_end_date={5}&key={6}&format=application/json";

        /// <summary>
        ///     Vuln detail by APi key
        /// </summary>
        public static string VulnDetailKeyUrl =
            "https://{0}//api/source_vuln/{1}/?display_traces=1&display_steps=1&format=json&key={2}&display_solution={3}&display_description={4}";

        /// <summary>
        ///     vuln detail by Cookie
        /// </summary>
        public static string VulnDetailCookieUrl =
            "https://{0}//api/source_vuln/{1}/?display_traces=1&display_steps=1&format=json&display_solution={2}&display_description={3}";

        /// <summary>
        ///    Vuln Detail By Sentinel Login URL
        /// </summary>
        public static string VulnDetailSentinelCookieUrl =
            "https://{0}/api/source_vuln/{1}/?display_traces=1&display_steps=1&format=json&display_solution={2}&display_description={3}";

        /// <summary>
        ///    Question Answer By Login URL
        /// </summary>
        public static string QuestionAnswerCookieUrl =
            "https://{0}/api/discussion?query_resource=/api/source_vuln/{1}&page:limit=50&page:offset=0&page:order_by=created_asc&display_responses=1&_=1421980580382&format=json";

        /// <summary>
        ///    Question Answer By Api key URL
        /// </summary>
        public static string QuestionAnswerApiKeyUrl =
            "https://{0}/api/discussion?query_resource=/api/source_vuln/{1}&page:limit=50&page:offset=0&page:order_by=created_asc&display_responses=1&_=1421980580382&key={2}&format=json";

        /// <summary>
        ///     Post Question URL
        /// </summary>
        public static string PostQuestionCookieUrl = "https://{0}/api/discussion/";

        /// <summary>
        ///     Post Question By API key
        /// </summary>
        public static string PostQuestionApiKeyUrl = "https://{0}/api/discussion/?key={1}&format=json";
    }
}