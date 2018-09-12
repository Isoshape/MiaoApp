using WDHTracker.Basic.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WDHTracker.Basic.Authentication
{
    /// <summary>
    /// Class handling OAuth2 authentication for Web API by login and password values.
    /// </summary>
    public class OAuth2UserLoginPasswordAuth : IOAuth2ClientAuthentication
    {
        protected class ResponseAccessToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; } //In seconds.
            [JsonProperty(".expires")]
            public string expires { get; set; }
        }

        protected readonly string _userLogin;
        protected readonly string _userPassword;
        protected readonly string _tokenEndpoint;
        protected readonly HttpClient _httpClient;

        /// <summary> 
        /// </summary>
        /// <param name="tokenEndpoint">API token url. E.g: 'http://0.0.0.0/token'.</param>
        /// <param name="userLogin">User login.</param>
        /// <param name="userPassword">User password.</param>
        public OAuth2UserLoginPasswordAuth(HttpClient httpClient, string loginRelativePath, string userLogin, string userPassword)
        {
            this._userLogin = userLogin;
            this._userPassword = userPassword;
            this._tokenEndpoint = loginRelativePath;
            this._httpClient = httpClient;
        }


        /// <summary>
        /// Prepare http request to get access data.
        /// </summary>
        /// <returns></returns>
        protected virtual HttpRequestMessage PrepareHttpRequestMessage()
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint);
            message.Content = new StringContent(
                string.Format("grant_type=password&username={0}&password={1}", HttpUtility.UrlEncode(this._userLogin), HttpUtility.UrlEncode(this._userPassword)),
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            return (message);
        }

        public AccessToken GetUserAccessToken()
        {
            throw new NotImplementedException();
        }

        public Task<AccessToken> GetUserAccessTokenAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Authentication user in API and getting access token.
        /// </summary>
        /// <returns>Information about access token.</returns>
        //public AccessToken GetUserAccessToken()
        //{
        //    HttpResponseMessage response = _httpClient.SendAsync(PrepareHttpRequestMessage()).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        ResponseAccessToken result = JsonConvert.DeserializeObject<ResponseAccessToken>(response.Content.ReadAsStringAsync().Result);
        //        return new AccessToken(result.token_type, result.access_token, DateTime.Parse(result.expires));
        //    }
        //    else
        //    {

        //        //throw new HttpRequestException((int)response.StatusCode, response.ReasonPhrase, new Exception(response.ToString()));,
        //       // throw new HttpRequestException();
        //    }
        //}

        /// <summary>
        /// Authentication user in API and getting access token (Async).
        /// </summary>
        /// <returns>Information about access token.</returns>
        //public async Task<AccessToken> GetUserAccessTokenAsync()
        //{
        //    HttpResponseMessage response = await _httpClient.SendAsync(PrepareHttpRequestMessage());
        //    if (response.IsSuccessStatusCode)
        //    {
        //        ResponseAccessToken result = JsonConvert.DeserializeObject<ResponseAccessToken>(await response.Content.ReadAsStringAsync());
        //        return new AccessToken(result.token_type, result.access_token, DateTime.Parse(result.expires));
        //    }
        //    else
        //    {
        //        //throw new HttpException((int)response.StatusCode, response.ReasonPhrase, new Exception(response.ToString()));,
        //       throw new HttpException();
        //    }
        //}
    }
}
