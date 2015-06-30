using System.ComponentModel.DataAnnotations;

namespace IShop.Models.Attributes
{
    public class PhoneRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phone = value as string;

            if (string.IsNullOrWhiteSpace(phone))
            {
                var propertyEmail = validationContext.ObjectType.GetProperty("Email");

                if (propertyEmail != null)
                {
                    var email = propertyEmail.GetValue(validationContext.ObjectInstance, null) as string;

                    if (string.IsNullOrWhiteSpace(email))
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }
            }
            return null;
        }
    }
}
