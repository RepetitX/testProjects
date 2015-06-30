using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Common.Web.Mvc.Routing
{
    public static class DisplayModeHelper
    {
        public static bool IsDisplayModeRoute(HttpContextBase context, DisplayMode displayMode)
        {
            var routeValues = context.Request.RequestContext.RouteData.Values;

            if (!routeValues.Any() || !routeValues.ContainsKey("displayMode"))
                return false;

            DisplayMode routeDisplayMode;

            return Enum.TryParse((string)routeValues["displayMode"], true, out routeDisplayMode) && routeDisplayMode == displayMode;
        }

        public static bool IsDisplayModeRoute(RouteValueDictionary routeValues)
        {
            if (!routeValues.Any() || !routeValues.ContainsKey("displayMode"))
                return false;

            DisplayMode routeDisplayMode;

            return Enum.TryParse((string)routeValues["displayMode"], true, out routeDisplayMode);
        }

        public static DisplayMode? GetCurrentDisplayMode(HttpContextBase context)
        {
            var routeValues = context.Request.RequestContext.RouteData.Values;

            if (!routeValues.Any() || !routeValues.ContainsKey("displayMode"))
                return null;

            DisplayMode routeDisplayMode;

            return Enum.TryParse((string)routeValues["displayMode"], true, out routeDisplayMode) ? routeDisplayMode : (DisplayMode?)null;
        }
    }

    public enum DisplayMode
    {
        Mobile,
        Tab
    }
}