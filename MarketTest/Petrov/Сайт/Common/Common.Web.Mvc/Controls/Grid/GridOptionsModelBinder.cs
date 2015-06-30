using System;
using System.Web.Mvc;
using Autofac.Integration.Mvc;

namespace Common.Web.Mvc.Controls
{
    [ModelBinderType(typeof(IGridOptions))]
    public class GridOptionsModelBinder : DefaultModelBinder, IFilteredModelBinder
    {
        public static string GridKey(ControllerContext controllerContext)
        {
            return controllerContext.Controller.GetType().Name;
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            IGridOptions options;

            var key = GridKey(controllerContext);

            if (bindingContext.ValueProvider.GetValue("options") != null && typeof(IGridOptions).IsAssignableFrom(bindingContext.ValueProvider.GetValue("options").RawValue.GetType()))
                options = bindingContext.ValueProvider.GetValue("options").RawValue as IGridOptions;
            else if (controllerContext.HttpContext.Session[key] != null && typeof(IGridOptions).IsAssignableFrom(controllerContext.HttpContext.Session[key].GetType()))
                options = controllerContext.HttpContext.Session[key] as IGridOptions;
            else if (bindingContext.ModelType.IsClass && !bindingContext.ModelType.IsAbstract)
                options = Activator.CreateInstance(bindingContext.ModelType, null) as IGridOptions;
            else
                options = new GridOptions();

            //Если применен фильтр или выполнен поиск по подстроке, то нужно показать первую страницу
            if (!string.IsNullOrEmpty(controllerContext.HttpContext.Request["SearchString"]) || !string.IsNullOrEmpty(controllerContext.HttpContext.Request["ApplyFilter"]))
                options.Page = 1;

            return options;
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);

            SaveGridOptions(controllerContext, bindingContext.Model as GridOptions);
        }

        public bool IsMatch(ModelBindingContext context)
        {
            return typeof(IGridOptions).IsAssignableFrom(context.ModelType);
        }

        /// <summary>
        /// Принудительно заменить текущее состояние таблицы в сессии. Используется при загрузке сохраненных фильтров.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="options"></param>
        public static void SaveGridOptions(ControllerContext controllerContext, GridOptions options)
        {
            var key = GridKey(controllerContext);

            controllerContext.HttpContext.Session[key] = options;
        }

        /// <summary>
        /// Принудительно очистить текущее состояние таблицы в сессии. Используется при сбросе фильтра.
        /// </summary>
        /// <param name="controllerContext"></param>
        public static void ClearGridOptions(ControllerContext controllerContext)
        {
            var key = GridKey(controllerContext);

            controllerContext.HttpContext.Session.Remove(key);
        }
    }
}
