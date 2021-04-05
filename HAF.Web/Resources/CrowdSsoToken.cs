using System;
using Crowd.Rest.Client.Entity;

namespace HAF.Web.Resources
{
    public class CrowdSsoToken
    {
        public CrowdSsoToken(string token, CookieConfiguration cookieConfiguration, DateTime expirationDate)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            if (cookieConfiguration == null)
                throw new ArgumentNullException(nameof(cookieConfiguration));
            Token = token;
            CookieConfiguration = cookieConfiguration;
            ExpirationDate = expirationDate;
        }

        public CookieConfiguration CookieConfiguration { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Token { get; set; }
    }
}