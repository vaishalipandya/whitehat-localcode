using Newtonsoft.Json;
using System.IO;
using System.Net;
using WhiteHatSec.Entity;
using WhiteHatSec.Localization.Culture.Resource;
using WhiteHatSec.Shared;

namespace WhiteHatSec.Services
{
    /// <summary>
    ///     Call REST API of WhiteHat Sentinel application
    /// </summary>
    public class ApplicationService
    {
        #region "Get Applications  By API key WebService"

        /// <summary>
        ///    Check Authentication By API key and get application
        /// </summary>
        public static ApplicationDetail.ApplicationInfo GetApps(string sentinelServerUrl, string apiKey)
        {
            ApplicationDetail.ApplicationInfo application = new ApplicationDetail.ApplicationInfo();
            try
            {
                sentinelServerUrl = Constant.CleanUrl(sentinelServerUrl);
                string applicationByApiKeyurl = string.Format(Constant.AppsByApiKeyUrl, sentinelServerUrl, apiKey);
                applicationByApiKeyurl += Constant.getMetricsString();

                HttpWebRequest applicationByApiKeyRequest = WebRequest.CreateHttp(applicationByApiKeyurl);
                applicationByApiKeyRequest.AllowAutoRedirect = false;
                applicationByApiKeyRequest.ContentType = Constant.ContentType;
                applicationByApiKeyRequest.UserAgent = Constant.UserAgent;
                applicationByApiKeyRequest.Headers[HttpRequestHeader.AcceptLanguage] = Constant.AcceptLanguage;
                applicationByApiKeyRequest.Headers[HttpRequestHeader.CacheControl] = Constant.CacheControl;
                applicationByApiKeyRequest.Method = "GET";

                string applicationResponseData;
                HttpWebResponse response = (HttpWebResponse)applicationByApiKeyRequest.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    applicationResponseData = reader.ReadToEnd();
                }

                application = JsonConvert.DeserializeObject<ApplicationDetail.ApplicationInfo>(applicationResponseData);
                application.ResponseMessage = "true";
                response.Close();
                return application;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        HttpWebResponse response = ex.Response as HttpWebResponse;
                        if (response != null)
                        {
                            if ((int)response.StatusCode == 401)
                            {
                                application.ResponseMessage = MessageLog.AuthenticationFailed;
                            }
                        }
                        break;
                    case WebExceptionStatus.NameResolutionFailure:

                        application.ResponseMessage =
                            MessageLog.AuthenticationFailedPleaseEnterCorrectServerDetail;
                        break;
                    default:
                        application.ResponseMessage =
                            ex.Message;
                        break;
                }

                return application;
            }
        }

        /// <summary>
        ///     Gets the applications by Sentinel Cookie.
        /// </summary>
        /// <param name="sentinelServerUrl">The server.</param>
        /// <param name="sentinelCookie">The cookie.</param>
        /// <returns></returns>
        public static ApplicationDetail.ApplicationInfo GetAppsByCookie(string sentinelServerUrl, CookieContainer sentinelCookie)
        {
            sentinelServerUrl = Constant.CleanUrl(sentinelServerUrl);
            string applicationUrl = string.Format(Constant.AppsByCookieUrl, sentinelServerUrl);
            applicationUrl += Constant.getMetricsString();

            HttpWebRequest applicationRequest = WebRequest.CreateHttp(applicationUrl);
            applicationRequest.AllowAutoRedirect = false;
            applicationRequest.CookieContainer = sentinelCookie;

            applicationRequest.UserAgent = Constant.UserAgent;
            applicationRequest.Headers[HttpRequestHeader.AcceptLanguage] = Constant.AcceptLanguage;
            applicationRequest.Headers[HttpRequestHeader.CacheControl] = Constant.CacheControl;

            applicationRequest.Method = "GET";

            string application;
            HttpWebResponse response = (HttpWebResponse)applicationRequest.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                application = reader.ReadToEnd();
            }

            response.Close();
            return JsonConvert.DeserializeObject<ApplicationDetail.ApplicationInfo>(application);

        }

        #endregion
    }
}
