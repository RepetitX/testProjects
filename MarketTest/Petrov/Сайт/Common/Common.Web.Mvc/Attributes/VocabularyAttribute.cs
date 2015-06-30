using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;
using Common.Repository;

namespace Common.Web.Mvc
{
    public class VocabularyAttribute : Attribute
   {
        public Type SourceType { get; set; }
        public string NameValue { get; set; }
        public string NameText { get; set; }

        public List<SelectListItem> GetDictionary<TTargetProperty>(TTargetProperty targetProperty)
        {
            if (SourceType != null)
            {
                var lifetimeScope = DependencyResolver.Current.GetService<ILifetimeScope>();
                if (lifetimeScope != null)
                {
                    var repository = lifetimeScope.Resolve(SourceType);
                    if (repository != null && repository.GetType().IsAssignableFrom(typeof (IRepository<,>)))
                    {
                        var dictionary = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };

                        return dictionary;
                    }
                }
            }
            return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
        }
   }
}
