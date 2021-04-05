using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Crowd.Rest.Client;
using Elmah;

namespace HAF.Web.Security
{
    public class CrowdAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        private readonly CrowdRestClientManager _crowd;

        public CrowdAuthenticationFilter(CrowdRestClientManager crowd)
        {
            if (crowd == null)
                throw new ArgumentNullException(nameof(crowd));
            _crowd = crowd;
        }

        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context.Principal?.Identity?.IsAuthenticated == true)
                return;

            var request = context.Request;
            var authorization = request.Headers.Authorization;
            if (authorization == null || authorization.Scheme != "Basic")
            {
                var cookies = request.Headers.GetCookies().FirstOrDefault();
                if (cookies == null || !cookies.Cookies.Any())
                    return;
                var token = cookies.Cookies.FirstOrDefault(x => x.Name == "crowd.token_key")?.Value;
                if (token == null)
                    return;
                await HandleSsoToken(context, request, cancellationToken, token);
            }
            else
                await HandleBasicAuthentication(context, request, cancellationToken, authorization);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

        private Task<CrowdPrincipal> AuthenticateCrowdUser(
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
                {
                    try
                    {
                        var user = _crowd.AuthenticateUser(userName, password);
                        var groups = _crowd.GetGroupsForUser(userName);
                        return new CrowdPrincipal(new CrowdIdentity(user), groups);
                    }
                    catch (CrowdException e)
                    {
                        ErrorLog.GetDefault(null).Log(new Error(e));
                        return null;
                    }
                },
                cancellationToken);
        }

        private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            try
            {
                var result = Encoding.ASCII.GetString(Convert.FromBase64String(authorizationParameter)).Split(':');
                if (result.Length < 2)
                    return null;
                return Tuple.Create(result[0], result[1]);
            }
            catch (Exception e)
            {
                ErrorLog.GetDefault(null).Log(new Error(e));
                return null;
            }
        }

        private async Task HandleBasicAuthentication(
            HttpAuthenticationContext context,
            HttpRequestMessage request,
            CancellationToken cancellationToken,
            AuthenticationHeaderValue authorization)
        {
            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            var userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);
            if (userNameAndPasword == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
            else
            {
                var userName = userNameAndPasword.Item1;
                var password = userNameAndPasword.Item2;

                var principal = await AuthenticateCrowdUser(userName, password, cancellationToken);
                if (principal == null)
                    context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
                else
                    context.Principal = principal;
            }
        }

        private async Task HandleSsoToken(
            HttpAuthenticationContext context,
            HttpRequestMessage request,
            CancellationToken cancellationToken,
            string token)
        {
            var principal = await ValidateSsoToken(cancellationToken, token);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid SSO token. It might have expired.", request);
            }
            else
                context.Principal = principal;
        }

        private Task<CrowdPrincipal> ValidateSsoToken(CancellationToken cancellationToken, string token)
        {
            return Task.Run(
                () =>
                {
                    try
                    {
                        var session = _crowd.ValidateSSOAuthenticationAndGetSession(token);
                        var groups = _crowd.GetGroupsForUser(session.User.Name);
                        return new CrowdPrincipal(new CrowdIdentity(session.User), groups);
                    }
                    catch (CrowdException e)
                    {
                        ErrorLog.GetDefault(null).Log(new Error(e));
                        return null;
                    }
                },
                cancellationToken);
        }
    }
}