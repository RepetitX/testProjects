using System;
using System.Web;

namespace IShop.Helpers
{
    public class CurrentRequest
    {
        public static T GetCachedResult<T>(Func<T> originMethod)
        {
            var cacheKey = originMethod.Method.Name;

            T result;

            if (HttpContext.Current.Items.Contains(cacheKey))
            {
                result = (T)HttpContext.Current.Items[cacheKey];
            }
            else
            {
                result = originMethod.Invoke();

                HttpContext.Current.Items[cacheKey] = result;
            }

            return result;
        }
    }
}
