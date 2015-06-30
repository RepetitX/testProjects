using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Routing;

namespace Common.Web.Mvc.Routing
{
    public class FastRouteConstraint : IRouteConstraint
    {
        private readonly Expression<Func<RouteValueDictionary, bool>> _expression;

        public FastRouteConstraint(Expression<Func<RouteValueDictionary, bool>> expression)
        {
            _expression = expression;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return _expression.Compile().Invoke(values);
        }
    }
}