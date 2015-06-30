using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;

namespace Common.Web.Mvc
{
    public class SessionModelBinderAttribute : CustomModelBinderAttribute
    {
        private readonly string _sessionKey;

        public SessionModelBinderAttribute(string sessionKey)
        {
            _sessionKey = sessionKey;
        }

        public override IModelBinder GetBinder()
        {
            return new SessionModelBinder(_sessionKey);
        }
    }
}
