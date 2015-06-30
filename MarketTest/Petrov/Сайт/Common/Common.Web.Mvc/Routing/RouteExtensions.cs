using System;
using System.Web.Routing;
using System.Web.Mvc;

namespace Common.Web.Mvc.Routing
{
    public static class RouteExtensions
    {
        public static string GetRouteName(this Route route)
        {
            if (route == null)
                return null;

            return route.DataTokens.GetRouteName();
        }

        public static string GetRouteName(this RouteData routeData)
        {
            if (routeData == null)
                return null;

            return routeData.DataTokens.GetRouteName();
        }

        public static string GetRouteName(this RouteValueDictionary routeValues)
        {
            if (routeValues == null)
                return null;

            object routeName = null;

            routeValues.TryGetValue("__RouteName", out routeName);

            return routeName as string;
        }

        public static Route SetRouteName(this Route route, string routeName)
        {
            if (route == null)
                throw new ArgumentNullException("route");
            if (route.DataTokens == null)
                route.DataTokens = new RouteValueDictionary();

            route.DataTokens["__RouteName"] = routeName;

            return route;
        }

        public static Route Map(this RouteCollection routes, string name, string url)
        {
            return routes.Map(name, url, null, null, null);
        }

        public static Route Map(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.Map(name, url, defaults, null, null);
        }

        public static Route Map(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return routes.Map(name, url, defaults, constraints, null);
        }

        public static Route Map(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return Map(routes, name, url, null, null, namespaces);
        }

        public static Route Map(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return Map(routes, name, url, defaults, null, namespaces);
        }

        public static Route Map(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            return routes.MapRoute(name, url, defaults, constraints, namespaces).SetRouteName(name);
        }

        public static void MapDomain(this RouteCollection routes, string name, string domain, string url, object defaults)
        {
            MapDomain(routes, name, domain, url, new RouteValueDictionary(defaults), null, new MvcRouteHandler());
        }

        public static void MapDomain(this RouteCollection routes, string name, string domain, string url, object defaults, object constraints)
        {
            MapDomain(routes, name, domain, url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new MvcRouteHandler());
        }

        public static void MapDomain(this RouteCollection routes, string name, string domain, string url, object defaults, object constraints, IRouteHandler routeHandler)
        {
            MapDomain(routes, name, domain, url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), routeHandler);
        }

        public static void MapDomain(this RouteCollection routes, string name, string domain, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
        {
            routes.Add(name, new DomainRoute(domain, url, defaults, constraints, routeHandler).SetRouteName(name));
        }
    }
}
