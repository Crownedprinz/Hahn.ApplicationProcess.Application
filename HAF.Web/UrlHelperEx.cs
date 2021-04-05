using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Ploeh.Hyprlinkr;
using HAF.Domain;

namespace HAF.Web
{
    public static class UrlHelperEx
    {
        public static Uri GetLink<T, TResult>(HttpRequestMessage request, Expression<Func<T, TResult>> method) =>
            GetUri(request, method.GetMethodCallExpression()) ?? new RouteLinker(request).GetUri(method);

        public static Uri GetLink<T>(HttpRequestMessage request, Expression<Action<T>> method) =>
            GetUri(request, method.GetMethodCallExpression()) ?? new RouteLinker(request).GetUri(method);

        private static Uri GetBaseUri(HttpRequestMessage request)
        {
            var authority = request.RequestUri.GetLeftPart(UriPartial.Authority);
            return new Uri(authority);
        }

        private static Uri GetUri(HttpRequestMessage request, MethodCallExpression method)
        {
            var routeAttribute = method.Method.GetCustomAttribute<RouteAttribute>(false);
            if (routeAttribute == null)
                return null;
            var routePrefixAttribute = method.Object?.Type.GetCustomAttribute<RoutePrefixAttribute>(false);
            var routeTemplate = routeAttribute.Template;
            if (routePrefixAttribute != null)
                routeTemplate = $"{routePrefixAttribute.Prefix}/{routeTemplate}";

            var subRoutes = request.GetConfiguration()
                .Routes.Where(x => x is IEnumerable<IHttpRoute>)
                .Cast<IEnumerable<IHttpRoute>>()
                .SelectMany(x => x);
            var matchingSubRoutes = subRoutes.Where(
                x => x.RouteTemplate == routeTemplate && ((HttpActionDescriptor[])x.DataTokens["actions"])[0]
                     .SupportedHttpMethods.Contains(request.Method));
            var routeValues = new ScalarRouteValuesQuery().GetRouteValues(method);
            routeValues.Add("httproute", null);
            var uris = matchingSubRoutes.Select(x => x.GetVirtualPath(request, routeValues)).ToArray();

            var relativeUri = uris.SingleOrDefault()?.VirtualPath;
            if (relativeUri == null)
                return null;

            var baseUri = GetBaseUri(request);
            return new Uri(baseUri, relativeUri);
        }
    }
}