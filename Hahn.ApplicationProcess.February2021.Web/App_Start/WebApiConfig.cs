using Elmah.Contrib.WebApi;
using HAF.Domain;
using Hahn.ApplicationProcess.February2021.Web.App_Start;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Routing;

namespace Hahn.ApplicationProcess.February2021.Web
{
    public class WebApiConfig: IApplicationStartUpHandler
    {
        private readonly HttpConfiguration _config;
        //private readonly CrowdAuthenticationFilter _crowdAuthenticationFilter;
        //private readonly TokenAuthenticationFilter _tokenAuthenticationFilter;
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }

        public void Handle()
        {
            _config.Filters.Add(new AuthorizeAttribute());
            _config.Filters.Add(new ElmahHandleErrorApiAttribute());
            //if (Settings.Default.RequiresSsl)
            //    _config.MessageHandlers.Add(new RequireHttpsHandler());
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
