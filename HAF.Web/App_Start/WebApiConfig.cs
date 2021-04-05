using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Routing;
using Elmah.Contrib.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
//using HAF.Web.Properties;
using HAF.Web.Security;
using HAF.Domain;
using System.Configuration;

namespace HAF.Web
{
    public class WebApiConfig : IApplicationStartUpHandler
    {
        private readonly HttpConfiguration _config;
        private readonly CrowdAuthenticationFilter _crowdAuthenticationFilter;
        //private readonly TokenAuthenticationFilter _tokenAuthenticationFilter;

        public WebApiConfig(
            HttpConfiguration config
            //TokenAuthenticationFilter tokenAuthenticationFilter,
            //CrowdAuthenticationFilter crowdAuthenticationFilter
            )
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            //if (tokenAuthenticationFilter == null)
            //    throw new ArgumentNullException(nameof(tokenAuthenticationFilter));
            //if (crowdAuthenticationFilter == null)
            //    throw new ArgumentNullException(nameof(crowdAuthenticationFilter));
            _config = config;
            //_tokenAuthenticationFilter = tokenAuthenticationFilter;
            //_crowdAuthenticationFilter = crowdAuthenticationFilter;
        }

        public void Handle()
        {
            _config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            //_config.Filters.Add(new AuthorizeAttribute());
            //_config.Filters.Add(_tokenAuthenticationFilter);
            //_config.Filters.Add(_crowdAuthenticationFilter);
            _config.Filters.Add(new ElmahHandleErrorApiAttribute());
            if (ConfigurationManager.AppSettings["RequiresSsl"]=="true")
                _config.MessageHandlers.Add(new RequireHttpsHandler());
            _config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;

            _config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            RouteTable.Routes.Ignore("errors/{*pathInfo}");

            _config.Routes.MapHttpRoute(
                "DefaultApi",
                Globals.ApiRoutesPrefix + "{controller}/{id}",
                new { id = RouteParameter.Optional });

            _config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            _config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling =
                PreserveReferencesHandling.Objects;
            _config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}