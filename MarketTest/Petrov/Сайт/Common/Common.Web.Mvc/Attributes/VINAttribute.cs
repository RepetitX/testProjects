using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Web.Mvc
{
    public class VINAttribute : RegularExpressionAttribute
    {
        public VINAttribute()
            : base("^[0-9a-zA-Z]+$")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is string)
            {
                var vin = (string)value;
                if (!base.IsValid(value) || vin.Length != 17)
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }
}
