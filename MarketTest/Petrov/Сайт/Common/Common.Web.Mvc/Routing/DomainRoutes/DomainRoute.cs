using System;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Common.Web.Mvc.Routing
{
    public class DomainRoute : Route
    {
        private Regex _domainRegex;
        private Regex _pathRegex;

        public string Domain { get; set; }

        public DomainRoute(string domain, string url, object defaults)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, object defaults, IRouteHandler routeHandler)
            : base(url, new RouteValueDictionary(defaults), routeHandler)
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, object defaults, object constraints, IRouteHandler routeHandler)
            : base(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), routeHandler)
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, RouteValueDictionary defaults)
            : base(url, defaults, new MvcRouteHandler())
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
            Domain = domain;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Build regex
            _domainRegex = CreateRegex(Domain);
            _pathRegex = CreateRegex(Url);

            // Request information
            var requestDomain = httpContext.Request.Headers["host"];

            if (!string.IsNullOrEmpty(requestDomain))
            {
                if (requestDomain.IndexOf(":", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":", StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                requestDomain = httpContext.Request.Url.Host;
            }

            var requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

            // Match domain and route
            var domainMatch = _domainRegex.Match(requestDomain);
            var pathMatch = _pathRegex.Match(requestPath);

            // Route data
            RouteData data = null;

            if (domainMatch.Success && pathMatch.Success)
            {
                data = base.GetRouteData(httpContext);

                if (data == null)
                    return null;

                // Iterate matching domain groups
                for (var i = 1; i < domainMatch.Groups.Count; i++)
                {
                    var group = domainMatch.Groups[i];

                    if (group.Success)
                    {
                        var key = _domainRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                                data.Values[key] = group.Value;
                        }
                    }
                }

                // Iterate matching path groups
                for (var i = 1; i < pathMatch.Groups.Count; i++)
                {
                    var group = pathMatch.Groups[i];

                    if (group.Success)
                    {
                        var key = _pathRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                                data.Values[key] = group.Value;
                        }
                    }
                }
            }

            return data;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, RemoveDomainTokens(values));
        }

        public DomainData GetDomainData(RequestContext requestContext, RouteValueDictionary values)
        {
            // Build hostname
            var hostname = Domain;

            foreach (var pair in values)
                hostname = hostname.Replace("{" + pair.Key + "}", pair.Value.ToString());

            // Return domain data
            return new DomainData
                {
                    Protocol = "http",
                    HostName = hostname,
                    Fragment = ""
                };
        }

        private static Regex CreateRegex(string source)
        {
            // Perform replacements
            source = source.Replace("/", @"\/?");
            source = source.Replace(".", @"\.");
            source = source.Replace("-", @"\-");
            source = source.Replace("{", @"(?<");
            source = source.Replace("}", @">([a-zA-Z0-9.-]*))");

            return new Regex("^" + source + "$");
        }

        private RouteValueDictionary RemoveDomainTokens(RouteValueDictionary values)
        {
            var tokenRegex = new Regex(@"({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?");
            var tokenMatch = tokenRegex.Match(Domain);
            
            for (var i = 0; i < tokenMatch.Groups.Count; i++)
            {
                var group = tokenMatch.Groups[i];

                if (group.Success)
                {
                    var key = group.Value.Replace("{", "").Replace("}", "");

                    if (values.ContainsKey(key))
                        values.Remove(key);
                }
            }

            return values;
        }
    }
}
