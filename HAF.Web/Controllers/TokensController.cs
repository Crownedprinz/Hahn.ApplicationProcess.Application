using System;
using System.Web.Http;
using HAF.Web.Security;
using HAF.Security;

namespace HAF.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix(Globals.ApiRoutesPrefix + "tokens")]
    public class TokensController : AuthenticationControllerBase
    {
        private readonly CrowdAuthenticationService _crowdAuthenticationService;
        private readonly IRsaKeyProvider _rsaKeyProvider;

        public TokensController(CrowdAuthenticationService crowdAuthenticationService, IRsaKeyProvider rsaKeyProvider)
        {
            if (crowdAuthenticationService == null)
                throw new ArgumentNullException(nameof(crowdAuthenticationService));
            if (rsaKeyProvider == null)
                throw new ArgumentNullException(nameof(rsaKeyProvider));
            _crowdAuthenticationService = crowdAuthenticationService;
            _rsaKeyProvider = rsaKeyProvider;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Authenticate(string crowdSsoToken)
        {
            return Authenticate(() => _crowdAuthenticationService.GenerateJwtToken(crowdSsoToken));
        }

        [Route("public-key")]
        [HttpGet]
        public string GetPublicKey() => _rsaKeyProvider.GetPublicKey();
    }
}