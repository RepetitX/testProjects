using System.Collections.Specialized;
using System.Linq;
using System.Web.Routing;

namespace Common.Web.Mvc
{
    public static class CollectionExtensions
    {
        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection)
        {
            var routeValueDictionary = new RouteValueDictionary();

            foreach (var key in collection.AllKeys.Where(k => k != null))
            {
                routeValueDictionary.Add(key, collection[key]);
            }

            return routeValueDictionary;
        }
    }
}
