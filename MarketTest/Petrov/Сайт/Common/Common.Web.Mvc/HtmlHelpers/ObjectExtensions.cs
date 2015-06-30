using System;
using System.Linq;
using System.Web;

namespace Common.Web.Mvc
{
    public static class ObjectExtensions
    {
        public static HtmlString ToHtmlAttributeString(this Object attributes)
        {
            var props = attributes.GetType().GetProperties();

            var pairs = props.Select(x => string.Format(@"{0}=""{1}""", x.Name, x.GetValue(attributes, null))).ToArray();

            return new HtmlString(string.Join(" ", pairs));
        }
    }
}