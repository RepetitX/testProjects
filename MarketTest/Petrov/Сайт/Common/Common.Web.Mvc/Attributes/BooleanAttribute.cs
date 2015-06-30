using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Common.Web.Mvc
{
    public class BooleanAttribute : ValidationAttribute, IClientValidatable
    {
        public bool Value { get; set; }

        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value == Value;
        }
 
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context) {
            return new ModelClientValidationRule[] { new ModelClientValidationRule { ValidationType = "boolrequired", ErrorMessage = this.ErrorMessage } };
        }
    }
}
