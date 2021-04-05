using System.Web;
using System.Web.Http;

namespace HAF.Web
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(ApplicationStartup.Execute);
        }
    }
}