using System;
using System.Net;
using System.Security.Authentication;
using System.Web.Http;
using Crowd.Rest.Client;
using Elmah;
using HAF.Web.Security;

namespace HAF.Web.Controllers
{
    public class AuthenticationControllerBase : ApiController
    {
        protected IHttpActionResult Authenticate<TResult>(Func<TResult> getResult)
        {
            try
            {
                return Content(HttpStatusCode.OK, getResult());
            }
            catch (TokenGenerationException e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                return InternalServerError();
            }
            catch (CrowdException e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                return Unauthorized();
            }
            catch (AuthenticationException e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                return Unauthorized();
            }
        }
    }
}