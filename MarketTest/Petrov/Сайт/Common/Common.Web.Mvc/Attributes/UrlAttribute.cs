using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Web.Mvc
{
    public class UrlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var text = value as string;
            Uri uri;

            return string.IsNullOrWhiteSpace(text) || Uri.TryCreate(text, UriKind.Absolute, out uri);
        }
    }
}