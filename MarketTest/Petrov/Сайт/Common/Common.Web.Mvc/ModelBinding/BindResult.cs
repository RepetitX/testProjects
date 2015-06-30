using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;

namespace Common.Web.Mvc
{
    public class BindResult
    {
        public object Value { get; private set; }
        public ValueProviderResult ValueProviderResult { get; private set; }

        public BindResult(object value, ValueProviderResult valueProviderResult)
        {
            Value = value;
            ValueProviderResult = valueProviderResult ??
                new ValueProviderResult(null, string.Empty, CultureInfo.CurrentCulture);
        }
    }
}
