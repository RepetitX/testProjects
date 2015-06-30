using System.ComponentModel.DataAnnotations;

namespace IShop.Models.Attributes
{
    public class EmailRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;

            if (string.IsNullOrWhiteSpace(email))
            {
                var propertyPhone = validationContext.ObjectType.GetProperty("Phone");

                if (propertyPhone != null)
                {
                    var phone = propertyPhone.GetValue(validationContext.ObjectInstance, null) as string;

                    if (string.IsNullOrWhiteSpace(phone))
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }
            }
            return null;
        }
    }
}
