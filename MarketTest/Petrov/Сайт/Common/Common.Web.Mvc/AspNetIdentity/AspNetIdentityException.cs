using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Web.Mvc.AspNetIdentity
{
    public class AspNetIdentityException : Exception
    {
        public IReadOnlyList<string> Messages { get; private set; }

        public AspNetIdentityException()
        {
        }

        public AspNetIdentityException(string message)
            : base(message)
        {
            Messages = new List<string> { message };
        }

        public AspNetIdentityException(IEnumerable<string> messages)
            : base(messages.Aggregate((m1, m2) => m1 + " " + m2))
        {
            Messages = messages.ToList();
        }
    }
}