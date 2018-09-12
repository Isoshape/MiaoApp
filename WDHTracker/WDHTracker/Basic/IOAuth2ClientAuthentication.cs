using WDHTracker.Basic.Models;
using System;
using System.Threading.Tasks;

namespace WDHTracker.Basic
{
    public interface IOAuth2ClientAuthentication
    {
        /// <summary>
        /// Authentication user in API and getting access token.
        /// </summary>
        /// <returns>Information about access token.</returns>
        AccessToken GetUserAccessToken();

        /// <summary>
        /// Authentication user in API and getting access token (Async).
        /// </summary>
        /// <returns>Information about access token.</returns>
        Task<AccessToken> GetUserAccessTokenAsync();
    }
}
