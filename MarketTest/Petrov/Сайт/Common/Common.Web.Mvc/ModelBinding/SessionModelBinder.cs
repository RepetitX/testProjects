using System;
using System.Web.Mvc;
using System.Web;

namespace Common.Web.Mvc
{
    public class SessionModelBinder : DefaultModelBinder, IModelBinder
    {
        private readonly string _sessionKey;

        public SessionModelBinder(string sessionKey)
        {
            _sessionKey = sessionKey;
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            object model = null;

            if (bindingContext.ValueProvider.GetValue("options") != null && modelType.IsAssignableFrom(bindingContext.ValueProvider.GetValue("options").RawValue.GetType()))
                model = bindingContext.ValueProvider.GetValue("options").RawValue;
            else if (controllerContext.HttpContext.Session[_sessionKey] != null && modelType.IsAssignableFrom(controllerContext.HttpContext.Session[_sessionKey].GetType()))
                model = controllerContext.HttpContext.Session[_sessionKey];
            else
                model = Activator.CreateInstance(modelType);

            return model;
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);
            controllerContext.HttpContext.Session[_sessionKey] = bindingContext.Model;
        }

        public static void ClearSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
    }
}
