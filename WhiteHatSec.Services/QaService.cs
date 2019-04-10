using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using WhiteHatSec.Entity;
using WhiteHatSec.Shared;
namespace WhiteHatSec.Services
{
    /// <summary>
    ///     Call REST API of WhiteHat Sentinel QA 
    /// </summary>
    public class QaService
    {
        #region "Get Question Answer Data"

        /// <summary>
        ///     Gets the question answer By Vuln Id.
        /// </summary>
        /// <param name="sentinelServerUrl">The sentinel server.</param>
        /// <param name="sentinelCookies">The  sentinel cookies.</param>      
        /// <param name="vulnId">The vuln id.</param>
        ///  <param name="apiKey">The api key.</param>
        /// <returns></returns>
        public static QuestionAnswerInfo.QuestionAnswerData GetQuestionAnswers(string sentinelServerUrl, CookieContainer sentinelCookies, string vulnId, string apiKey)
        {
            sentinelServerUrl = Constant.CleanUrl(sentinelServerUrl);
            string questionAnswerUrl;


            questionAnswerUrl = apiKey == string.Empty ? string.Format(Constant.QuestionAnswerCookieUrl, sentinelServerUrl, vulnId) : 
                string.Format(Constant.QuestionAnswerApiKeyUrl, sentinelServerUrl, vulnId, apiKey);
            questionAnswerUrl += Constant.getMetricsString();

            HttpWebRequest questionAnswerRequest = WebRequest.CreateHttp(questionAnswerUrl);
            if (sentinelCookies!=null)
                questionAnswerRequest.CookieContainer = sentinelCookies;
            questionAnswerRequest.AllowAutoRedirect = false;
            questionAnswerRequest.UserAgent = Constant.UserAgent;
            questionAnswerRequest.Headers[HttpRequestHeader.AcceptLanguage] = Constant.AcceptLanguage;
            questionAnswerRequest.Headers[HttpRequestHeader.CacheControl] = Constant.CacheControl;
            questionAnswerRequest.Method = "GET";
            string questionAnswerResult;
            HttpWebResponse questionAnswerResponse = (HttpWebResponse)questionAnswerRequest.GetResponse();

            using (StreamReader reader = new StreamReader(questionAnswerResponse.GetResponseStream()))
            {
                questionAnswerResult = reader.ReadToEnd();
            }

            questionAnswerResponse.Close();
            return JsonConvert.DeserializeObject<QuestionAnswerInfo.QuestionAnswerData>(questionAnswerResult);
        }

        #endregion

        #region "Post Question"

        /// <summary>
        ///     Post the question by Vuln Id.
        /// </summary>
        /// <param name="sentinelServerUrl">The sentinel server.</param>
        /// <param name="sentinelCookies">The sentinel cookies.</param>
        /// <param name="vulnId">The vuln id.</param>
        /// <param name="question">The question.</param>
        /// <param name="apiKey">The api key.</param>
        /// <returns></returns>
        public static int PostQuestion(string sentinelServerUrl, CookieContainer sentinelCookies, string vulnId, string question, string apiKey)
        {
            sentinelServerUrl = Constant.CleanUrl(sentinelServerUrl);
            string postQuestionUrl;

            postQuestionUrl = apiKey == string.Empty ? string.Format(Constant.PostQuestionCookieUrl, sentinelServerUrl) :
                string.Format(Constant.PostQuestionApiKeyUrl, sentinelServerUrl, apiKey);
            //postQuestionUrl+= Constant.getMetricsString();
            HttpWebRequest postQuestionRequest = WebRequest.CreateHttp(postQuestionUrl);
            if (sentinelCookies != null)
            {
                postQuestionRequest.CookieContainer = sentinelCookies;
            }
            postQuestionRequest.AllowAutoRedirect = false;
            postQuestionRequest.ContentType = Constant.ContentTypeJson;
            postQuestionRequest.UserAgent = Constant.UserAgent;
            postQuestionRequest.Headers[HttpRequestHeader.AcceptLanguage] = Constant.AcceptLanguage;
            postQuestionRequest.Headers[HttpRequestHeader.CacheControl] = Constant.CacheControl;
            postQuestionRequest.Method = "Post";
            using (StreamWriter streamWriter = new StreamWriter(postQuestionRequest.GetRequestStream()))
            {
                string[] questionUrl = { "/api/source_vuln/" + vulnId };

                string questionJsonData = new JavaScriptSerializer().Serialize(new
                {
                    resources = questionUrl,
                    topic = question
                });

                streamWriter.Write(questionJsonData);
            }

            HttpWebResponse postQuestionResponse = (HttpWebResponse)postQuestionRequest.GetResponse();

            using (StreamReader reader = new StreamReader(postQuestionResponse.GetResponseStream()))
            {
                reader.ReadToEnd();
            }

            postQuestionResponse.Close();
            return Convert.ToInt32(postQuestionResponse.StatusCode);
        }

        #endregion
    }
}
