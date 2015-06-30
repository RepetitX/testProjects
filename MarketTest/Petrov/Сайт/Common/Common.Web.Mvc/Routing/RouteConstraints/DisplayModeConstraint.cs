using System.Web;
using System.Web.Routing;

namespace Common.Web.Mvc.Routing
{
    public class DisplayModeConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return DisplayModeHelper.IsDisplayModeRoute(values);
        }
    }
}