using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Hahn.ApplicationProcess.February2021.Web.App_Start
{
    public class CustomDirectRouteProvider: DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>
           GetActionRouteFactories(HttpActionDescriptor actionDescriptor) =>
           actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(true);
    }
}