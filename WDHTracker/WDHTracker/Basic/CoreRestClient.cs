using WDHTracker.Basic.Authentication;
using WDHTracker.Basic.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WDHTracker.Controller;
using Xamarin.Forms;
using System.Reflection;


namespace WDHTracker.Basic
{
    /// <summary>
    /// Core abstract class to connect with Web API.
    /// </summary>
    public abstract class CoreRestClient
    {
        protected HttpClient _httpClient { get; set; }
        protected IOAuth2ClientAuthentication _auth { get; set; }
        protected AccessToken _token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="auth">Can be set to NULL but user shuld remeber to set 'withToken' on false in 'send' function.</param>
        public CoreRestClient(HttpClient httpClient, IOAuth2ClientAuthentication auth)
        {
            _httpClient = httpClient;
            _auth = auth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl">API base url. E.g: 'http://0.0.0.0/'.</param>
        //public CoreRestClient(string baseUrl)
        //{
        //    if (string.IsNullOrWhiteSpace(baseUrl) || !Uri.IsWellFormedUriString(baseUrl, UriKind.RelativeOrAbsolute))
        //        throw new ArgumentNullException("Invalid base url.");

        //    _auth = null;

        //    _httpClient = new HttpClient(new WebRequestHandler { ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; } });
        //    _httpClient.BaseAddress = new Uri(baseUrl);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl">API base url. E.g: 'http://0.0.0.0/'.</param>
        /// <param name="userLogin">User login.</param>
        /// <param name="userPassword">User password.</param>
        /// <param name="loginRelativePath">Login relative path</param>
        //public CoreRestClient(string baseUrl, string userLogin, string userPassword, string loginRelativePath = "Token")
        //{
        //    if (string.IsNullOrWhiteSpace(baseUrl) || !Uri.IsWellFormedUriString(baseUrl, UriKind.RelativeOrAbsolute))
        //        throw new ArgumentNullException("Invalid base url.");

        //    if (string.IsNullOrWhiteSpace(userLogin))
        //        throw new ArgumentNullException("Invalid OAuth2 login.");

        //    if (string.IsNullOrWhiteSpace(userPassword))
        //        throw new ArgumentNullException("Invalid password.");

        //    if (string.IsNullOrWhiteSpace(loginRelativePath))
        //        throw new ArgumentNullException("Invalid login path.");

        //    _httpClient = new HttpClient(new WebRequestHandler { ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; } });
        //    _httpClient.BaseAddress = new Uri(baseUrl);

        //    _auth = new OAuth2UserLoginPasswordAuth(_httpClient, loginRelativePath, userLogin, userPassword);
        //}


        public virtual HttpResponseMessage SendRequest(HttpMethod httpMethod, string relativeUrl, bool withToken = true, HttpContent content = null)
        {
            if (withToken)
            {
                this.CheckValidationToken();
            }

            HttpRequestMessage message = new HttpRequestMessage(httpMethod, relativeUrl);
            if (content != null)
            {
                message.Content = content;
            }
            HttpResponseMessage response = null;
            
            //THIS ONE!
             response = this._httpClient.SendAsync(message).Result;
            String test = response.StatusCode.ToString();
            if(test.Equals("OK"))
            {
                NetworkCheck.setLogged(true);
            }
            else
            {
                MessagingCenter.Send<CoreRestClient>(this, "wrongPassword");
               
            }
            
            
                
                this.ThrowIfNotSuccess(response);
            
            

            return (response);
        }

        public virtual async Task<HttpResponseMessage> SendRequestAsync(HttpMethod httpMethod, string relativeUrl, bool withToken = true, HttpContent content = null)
        {
            if (withToken)
            {
                await this.CheckValidationTokenAsync();
            }

            HttpRequestMessage message = new HttpRequestMessage(httpMethod, relativeUrl);
            if (content != null)
            {
                message.Content = content;
            }

            HttpResponseMessage response = await this._httpClient.SendAsync(message);
            this.ThrowIfNotSuccess(response);

            return (response);
        }


        #region Protected methods
        protected virtual void ThrowIfNotSuccess(HttpResponseMessage response)
        {
            
                if (!response.IsSuccessStatusCode)
                {
                //DETTE SKAL SES PÅ IFT: NÅR UDSTYR LÅNES!
        
                  //  throw new HttpException((int)response.StatusCode, response.ReasonPhrase, new Exception(response.Content.ReadAsStringAsync().Result));
                }
           
        }
        #endregion

        #region Validation
        protected virtual void SetValidationToken()
        {
            if (this._httpClient.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
            {
                this._httpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            this._httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", this._token.Type + " " + this._token.Token);
        }


        protected virtual void CheckValidationToken()
        {
            if (this._token == null || DateTime.Now >= this._token.ExpiresDate)
            {
                this._token = this._auth.GetUserAccessToken();
                this.SetValidationToken();
            }
        }

        protected virtual async Task CheckValidationTokenAsync()
        {
            if (this._token == null || DateTime.Now >= this._token.ExpiresDate)
            {
                this._token = await this._auth.GetUserAccessTokenAsync();
                this.SetValidationToken();
            }
        }
        #endregion
    }
}
