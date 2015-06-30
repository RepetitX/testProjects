using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Common.Web.Mvc
{
    public class CaptchaValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var validationText = HttpContext.Current.Session["CaptchaValidationText"];

            HttpContext.Current.Session["CaptchaValidationText"] = new Random().Next();

            return value.ToString() == validationText.ToString();
        }
    }
}
