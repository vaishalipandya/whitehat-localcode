using WhiteHatSec.Localization.Culture.Resource;
using WhiteHatSec.Shared;
using System.Net;
using System.Text;
using System;
using System.IO;
namespace WhiteHatSec.Services
{
    /// <summary>
    ///     Call REST API of WhiteHat Sentinel authentication
    /// </summary>
    public class AuthenticationService
    {
        #region "Authentication WebService"

        
        /// <summary>
        ///  Check Authentication of User and assign cookie for valid User
        /// </summary>
        /// <param name="sentinelServerUrl">The sentinel server URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="sentinelCookie">The cookieContainer.</param>
        /// <returns></returns>
        public static string Authentication(string sentinelServerUrl, string userName, string password, CookieContainer sentinelCookie)
        {
            string resultMessage = string.Empty;
            try
            {
                //DO NOT UNCOMMENT - this ignores certificate errors
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.CheckCertificateRevocationList = false;



                sentinelServerUrl = Constant.CleanUrl(sentinelServerUrl);
                string userLoginUrl = string.Format(Constant.UserLoginUrl, sentinelServerUrl, userName);
                //userLoginUrl+= Constant.getMetricsString();

                HttpWebRequest authenticationRequest = WebRequest.CreateHttp(userLoginUrl);
                authenticationRequest.AllowAutoRedirect = false;
                authenticationRequest.CookieContainer = sentinelCookie;
                authenticationRequest.Accept = Constant.AcceptFileType;
                authenticationRequest.Headers[HttpRequestHeader.AcceptEncoding] = Constant.AcceptEncodingType;
                authenticationRequest.ContentType = Constant.ContentType;
                authenticationRequest.UserAgent = Constant.UserAgent;
                authenticationRequest.Headers[HttpRequestHeader.AcceptLanguage] = Constant.AcceptLanguage;
                authenticationRequest.Headers[HttpRequestHeader.CacheControl] = Constant.CacheControl;
               
                string requestPassword = "password=" + password;
                authenticationRequest.Method = "POST";
                UTF8Encoding utfEncoding = new UTF8Encoding();
                byte[] passwordBytes = utfEncoding.GetBytes(requestPassword);
                authenticationRequest.ContentLength = passwordBytes.Length;
                using (Stream streamWriter = authenticationRequest.GetRequestStream())
                {
                    streamWriter.Write(passwordBytes, 0, passwordBytes.Length);
                    streamWriter.Flush();
                }

                HttpWebResponse authenticationResponse = (HttpWebResponse)authenticationRequest.GetResponse();

                foreach (Cookie cookie in authenticationResponse.Cookies)
                {
                    if (!string.IsNullOrEmpty(cookie.Value))
                    {
                        sentinelCookie.Add(new Uri(authenticationResponse.ResponseUri.GetLeftPart(UriPartial.Authority)), cookie);
                        resultMessage = "true";
                    }
                    else
                    {
                        resultMessage =
                            MessageLog.AuthenticationFailed;
                    }
                }
                authenticationResponse.Close();
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
                                resultMessage = MessageLog.AuthenticationFailed;
                        }
                        break;
                    case WebExceptionStatus.NameResolutionFailure:
                    case WebExceptionStatus.ConnectFailure:
                        resultMessage =
                            MessageLog.AuthenticationFailedPleaseEnterCorrectServerDetail;
                        break;
                    default:
                        resultMessage = ex.Message;
                        break;
                }
            }

            return resultMessage;
        }


        #endregion


    }
}
