using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Elmah;

namespace HAF.Web.Security
{
    public class TokenAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        private readonly CrowdAuthenticationService _crowdAuthenticationService;

        public TokenAuthenticationFilter(CrowdAuthenticationService crowdAuthenticationService)
        {
            if (crowdAuthenticationService == null)
                throw new ArgumentNullException(nameof(crowdAuthenticationService));
            _crowdAuthenticationService = crowdAuthenticationService;
        }

        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context.Principal?.Identity?.IsAuthenticated == true)
                return;

            var request = context.Request;
            var authorization = request.Headers.Authorization;

            string token;
            if (authorization == null || authorization.Scheme != "Bearer")
            {
                var cookies = request.Headers.GetCookies().FirstOrDefault();
                if (cookies == null || !cookies.Cookies.Any())
                    return;
                token = cookies.Cookies.FirstOrDefault(x => x.Name == "simple_dms.id_token")?.Value;
                if (token == null)
                    return;
            }
            else
            {
                if (string.IsNullOrEmpty(authorization.Parameter))
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing token", request);
                    return;
                }

                token = authorization.Parameter;
            }

            IPrincipal principal = null;
            try
            {
                principal = _crowdAuthenticationService.DecryptJwtToken(token);
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
            else
                context.Principal = principal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Bearer");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }
    }
}