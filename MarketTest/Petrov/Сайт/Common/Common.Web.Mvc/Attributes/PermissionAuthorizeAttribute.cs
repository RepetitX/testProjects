using System;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace Common.Web.Mvc
{
    /// <summary>
    /// Атрибут для авторизации, которому можно передать список ролей-элементов enum'a в конструктор. 
    /// Если пользователь не принадлежит ни к одной из переданных ролей, его переадресуют на страницу 404.
    /// </summary>
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(params object[] roles)
        {
            Roles = string.Join(",", (roles.Select(r => r.ToString())).ToArray());
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            var allowedRoles = Roles.Split(',');
            return allowedRoles.Any(role => httpContext.User.IsInRole(role));
        }
    }

}
