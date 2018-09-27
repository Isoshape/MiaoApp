using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace WDHTracker.Basic
{
    /// <summary>
    /// Basic class to connect with Web API.
    /// </summary>
    public class BasicRestClientBackup : CoreRestClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="auth">Can be set to NULL but user shuld remeber to set 'withToken' on false in 'send' function.</param>
        public BasicRestClientBackup(HttpClient httpClient, IOAuth2ClientAuthentication auth) : base(httpClient, auth)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="baseUrl">API base url. E.g: 'http://0.0.0.0/'.</param>
        //public BasicRestClient(string baseUrl) : base(baseUrl)
        //{
        //}

        /// <summary>
        /// </summary>
        /// <param name="baseUrl">API base url. E.g: 'http://0.0.0.0/'.</param>
        /// <param name="userLogin">User login.</param>
        /// <param name="userPassword">User password.</param>
        /// <param name="loginRelativePath">Login relative path</param>
        //public BasicRestClient(string baseUrl, string userLogin, string userPassword, string loginRelativePath = "Token") : base(baseUrl, userLogin, userPassword, loginRelativePath)
        //{
        //}


        protected virtual T DeserializeResponseObj<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


        /// <summary>
        /// Send 'httpPost' request to API.
        /// </summary>
        /// <param name="relativeUrl">API action, relative address. E.g: 'API/CONTROLLER/ACTON'.</param>
        /// <param name="content">Post content. Eg.:  new StringContent("Skip=0&Take=10", Encoding.UTF8, "application/x-www-form-urlencoded")</param>
        /// <param name="withToken">Flag - add authentication token.</param>
        /// <returns></returns>
        protected virtual HttpResponseMessage Post(string relativeUrl, HttpContent content, bool withToken = true)
        {
            return (this.SendRequest(HttpMethod.Post, relativeUrl, withToken, content));
        }

        /// <summary>
        /// Send 'httpPost' request to API.
        /// </summary>
        /// <typeparam name="T">Target deserialization model/Class.</typeparam>
        /// <param name="relativeUrl">API action, relative address. E.g: 'API/CONTROLLER/ACTON'.</param>
        /// <param name="content">Post content. Eg.:  new StringContent("Skip=0&Take=10", Encoding.UTF8, "application/x-www-form-urlencoded")</param>
        /// <param name="withToken">Flag - add authentication token.</param>
        /// <returns></returns>
        protected T Post<T>(string relativeUrl, HttpContent content, bool withToken = true)
        {
            HttpResponseMessage response = this.Post(relativeUrl, content, withToken);
            return (this.DeserializeResponseObj<T>(response.Content.ReadAsStringAsync().Result));
        }


        /// <summary>
        /// Send 'httpGet' request to API.
        /// </summary>
        /// <param name="relativeUrl">API action, realative address. E.g: 'API/CONTROLLER/ACTON'.</param>
        /// <param name="withToken">Flag - add authentication token?</param>
        /// <returns></returns>
        protected virtual HttpResponseMessage Get(string relativeUrl, bool withToken = true)
        {
            return (this.SendRequest(HttpMethod.Get, relativeUrl, withToken));
        }

        /// <summary>
        /// Send 'httpGet' request to API.
        /// </summary>
        /// <typeparam name="T">Target deserialization model/Class.</typeparam>
        /// <param name="relativeUrl">API action, realative address. E.g: 'API/CONTROLLER/ACTON'.</param>
        /// <param name="withToken">Flag - add authentication token?</param>
        /// <returns></returns>
        protected T Get<T>(string relativeUrl, bool withToken = true)
        {
            HttpResponseMessage response = this.Get(relativeUrl, withToken);
            return (this.DeserializeResponseObj<T>(response.Content.ReadAsStringAsync().Result));
        }


        /// <summary>
        /// Send 'httpPut' request to API.
        /// </summary>
        /// <param name="relativeUrl">API action, relative address. E.g: 'API/CONTROLLER/ACTON'.</param>
        /// <param name="content">Put content. Eg.: new StringContent("id=0&Name=test", Encoding.UTF8, "application/x-www-form-urlencoded")</param>
        /// <param name="withToken">Flag - add authentication token.</param>
        /// <returns></returns>
        protected HttpResponseMessage Put(string relativeUrl, HttpContent content, bool withToken = true)
        {
            return (this.SendRequest(HttpMethod.Put, relativeUrl, withToken));
        }


        /// <summary>
        /// Send 'httpDelete' request to API.
        /// </summary>
        /// <param name="relativeUrl">API action, realative address. E.g: 'API/CONTROLLER/ACTON?id=0'.</param>
        /// <param name="withToken">Flag - add authentication token?</param>
        /// <returns></returns>
        protected HttpResponseMessage Delete(string relativeUrl, bool withToken = true)
        {
            return (this.SendRequest(HttpMethod.Delete, relativeUrl, withToken));
        }
    }

}
