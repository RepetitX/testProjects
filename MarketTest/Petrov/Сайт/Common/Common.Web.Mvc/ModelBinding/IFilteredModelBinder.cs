using System.Web.Mvc;

namespace Common.Web.Mvc
{
    public interface IFilteredModelBinder : IModelBinder
    {
        bool IsMatch(ModelBindingContext bindingContext);
    }
}