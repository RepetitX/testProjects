using System.Web.Mvc;

namespace Common.Web.Mvc
{
    /// <summary>
    /// Substitute for a standard DefaultModelBinder. This model binder is provided with an array of FilteredModelBinders, 
    /// on each model binding request it tries to choose an appropriate model binder by calling IsMatch(bindingContext).
    /// First matched model binder is used. If none of model binders matched the modelType, DefaultModelBinder is utilized.
    /// 
    /// The purpose of this class is to support model binding to interfaces and generic types. E.g. you have class BaseModel,
    /// ConcreteModel1 : BaseModel and ConcreteModel2 : BaseModel. If you are using custom model binder, you have to add two keys
    /// to ModelBinders collection. If you are using SmartBinder, you can create one IFilteredModelBinder, which will check, if class can be
    /// cast to BaseModel. Only one string of configuration is required for any amount of classes. 
    /// 
    /// IFilteredModelBinder[] collection is provided by IoC in Configuration project.
    /// </summary>
    public class SmartBinder : DefaultModelBinder
    {
        private readonly IFilteredModelBinder[] _filteredModelBinders;

        public SmartBinder(params IFilteredModelBinder[] filteredModelBinders)
        {
            _filteredModelBinders = filteredModelBinders;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            foreach (var filteredModelBinder in _filteredModelBinders)
            {
                if (filteredModelBinder.IsMatch(bindingContext))
                    return filteredModelBinder.BindModel(controllerContext, bindingContext);
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}