using System.Web.Http;
using HAF.Web.Security;

namespace HAF.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix(Globals.ApiRoutesPrefix + "sso")]
    public class SingleSignOnController : AuthenticationControllerBase
    {
        private readonly CrowdAuthenticationService _crowdAuthenticationService;

        public SingleSignOnController(CrowdAuthenticationService crowdAuthenticationService)
        {
            _crowdAuthenticationService = crowdAuthenticationService;
        }

        [Route("crowd")]
        [HttpGet]
        public IHttpActionResult AuthenticateCrowdSsoToken(string userName, string password)
        {
            return Authenticate(() => _crowdAuthenticationService.AuthenticateSsoUser(userName, password));
        }
    }
}