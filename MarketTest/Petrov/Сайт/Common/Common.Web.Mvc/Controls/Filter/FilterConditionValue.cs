using System;
using System.Collections.Generic;

namespace Common.Web.Mvc.Controls
{
    [Serializable]
    public class FilterConditionValue
    {
        public string Key { get; set; }
        public string Condition { get; set; }
        public List<string> Values { get; set; }
    }
}