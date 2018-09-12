using System;

namespace WDHTracker.Basic.Models
{
    public class AccessToken
    {
        public AccessToken(string type, string token, DateTime expiresDate)
        {
            this.Type = type;
            this.Token = token;
            this.ExpiresDate = expiresDate;
        }

        public readonly string Token;
        public readonly string Type;
        public readonly DateTime ExpiresDate;
    }
}
