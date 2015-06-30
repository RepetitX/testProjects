using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web;
using IntraVision.Web.Toolkit;

namespace IntraVision.Web.Mvc.Controls
{
    public class ManualFilterCondition<TEntity> : IFilterCondition<TEntity>
        where TEntity : class, new()
    {
        string _Column;
        string _Caption;
        string _Value;

        public string Column
        {
            get { return _Column; }
            set { _Column = value; }
        }

        public bool Active
        {
            get { return !string.IsNullOrEmpty(_Value); }
        }

        public ManualFilterCondition(string column, string caption)
        {
            _Column = column;
            _Caption = caption;
        }

        #region IFilterCondition<TEntity> Members

        public void Init(NameValueCollection req)
        {
            _Value = req["flt" + _Column];
        }

        public IQueryable<TEntity> Filter(IQueryable<TEntity> list)
        {
            if (string.IsNullOrEmpty(_Value)) return list;
            if (HttpContext.Current.Session["user"] != null)
            {
                IUser user = HttpContext.Current.Session["user"] as IUser;
                if (user != null)
                    return list.Where(_Value.Replace("%userid%", user.Id.ToString(CultureInfo.InvariantCulture)));
            }
            return list.Where(_Value);
        }

        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(CultureInfo.InvariantCulture, @"<div class=""filterfield""><table width=""100%""><tr><td class=""tdlabel""><label for=""flt{0}"">{1}&nbsp;<a class=""small"" href=""{2}"" onclick=""openDialog('{2}'); return false;"">[?]</a></label></td>", _Column, _Caption, HTMLHelper.Resource("Filter", "ManualFilterHelp", "help/manual_filter.htm")));
            sb.Append(string.Format(CultureInfo.InvariantCulture, @"<td><textarea class=""manualfilter"" id=""flt{0}"" name=""flt{0}"">{1}</textarea></td></tr></table></div>", _Column, _Value));
            return sb.ToString();
        }

        #endregion

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_Value)) return string.Format("flt{0}={1}", _Column, _Value);
            return string.Empty;
        }
    }
}
