using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using Crowd.Rest.Client;
using Crowd.Rest.Client.Entity;
using Microsoft.IdentityModel.Tokens;
using HAF.Web.Properties;
//using HAF.Web.Resources;
using HAF.Security;

namespace HAF.Web.Security
{
    public class CrowdAuthenticationService
    {
        private readonly CrowdRestClientManager _crowd;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IRsaKeyProvider _rsaKeyProvider;

        public CrowdAuthenticationService(IRsaKeyProvider rsaKeyProvider, CrowdRestClientManager crowd)
        {
            if (rsaKeyProvider == null)
                throw new ArgumentNullException(nameof(rsaKeyProvider));
            if (crowd == null)
                throw new ArgumentNullException(nameof(crowd));
            _rsaKeyProvider = rsaKeyProvider;
            _crowd = crowd;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public CrowdSsoToken AuthenticateSsoUser(string username, string password)
        {
            var token = _crowd.AuthenticateSSOUser(new AuthenticationContext { UserName = username, Password = password });
            var session = _crowd.ValidateSSOAuthenticationAndGetSession(token);
            return new CrowdSsoToken(token, _crowd.GetCookieConfiguration(), session.ExpiryDate);
        }

        public IPrincipal DecryptJwtToken(string jwtToken)
        {
            var securityTokenHandler = new JwtSecurityTokenHandler();
            var publicAndPrivate = new RSACryptoServiceProvider();

            var publicAndPrivateKey = _rsaKeyProvider.GetPrivateAndPublicKey();
            if (publicAndPrivateKey == null)
                return null;

            publicAndPrivate.FromXmlString(publicAndPrivateKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = Settings.Default.JwtIssuer,
                ValidAudience = Settings.Default.JwtAudience,
                IssuerSigningKey = new RsaSecurityKey(publicAndPrivate)
            };

            SecurityToken securityToken;
            return securityTokenHandler.ValidateToken(jwtToken, validationParameters, out securityToken);
        }

        public string GenerateJwtToken(string crowdSsoToken)
        {
            return GenerateJwtToken(() => crowdSsoToken);
        }

        public string GenerateJwtToken(string username, string password)
        {
            return GenerateJwtToken(() => AuthenticateSsoUser(username, password).Token);
        }

        private string CreateTokenFromClaims(IEnumerable<Claim> claims)
        {
            var publicAndPrivateKey = _rsaKeyProvider.GetPrivateAndPublicKey();
            if (publicAndPrivateKey == null)
                throw new TokenGenerationException("Error getting signing key");

            var publicAndPrivate = new RSACryptoServiceProvider();
            publicAndPrivate.FromXmlString(publicAndPrivateKey);

            var expires = DateTime.Now.AddDays(1);
            var subject = new ClaimsIdentity(claims);

            var signingCredentials = new SigningCredentials(
                new RsaSecurityKey(publicAndPrivate),
                SecurityAlgorithms.RsaSha256Signature);

            return _jwtSecurityTokenHandler.CreateEncodedJwt(
                Settings.Default.JwtIssuer,
                Settings.Default.JwtAudience,
                subject,
                null,
                expires,
                DateTime.Now,
                signingCredentials);
        }

        private string GenerateJwtToken(Func<string> getCrowdSsoToken)
        {
            List<Claim> claims;
            try
            {
                var token = getCrowdSsoToken();

                var session = _crowd.ValidateSSOAuthenticationAndGetSession(token);
                var user = session.User;
                var groups = _crowd.GetGroupsForUser(user.Name)
                    .Where(x => x.Name.StartsWith(Settings.Default.CrowdGroupsPrefix));

                claims = new List<Claim>(groups.Select(x => new Claim(ClaimTypes.Role, x.Name)))
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.GivenName, user.DisplayName),
                    new Claim(ClaimTypes.Email, user.Email)
                };
            }
            catch (Exception e)
            {
                throw new AuthenticationException("Error authenticating user or getting its details", e);
            }

            return CreateTokenFromClaims(claims);
        }
    }
}