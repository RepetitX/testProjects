using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Web.Mvc
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var passwordPattern = new Regex(@"(?=^.{8,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)[0-9a-zA-Z!@#$%^&*()]*$");

            if (value == null || passwordPattern.IsMatch(value.ToString()))
                return null;

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
