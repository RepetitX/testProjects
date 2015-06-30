using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;

namespace IntraVision.Web.Mvc.Controls
{
    public class ExistsFilterCondition<TEntity> : IFilterCondition<TEntity>
        where TEntity : class, new()
    {
        string _Column;
        string _Caption;
        int _Type = 0;

        public string Column
        {
            get { return _Column; }
            set { _Column = value; }
        }

        public bool Active
        {
            get { return _Type > 0; }
        }

        string[] _Types = { HTMLHelper.Resource("Filter", "Exists", "Есть"), HTMLHelper.Resource("Filter", "No", "Нет") };

        public ExistsFilterCondition(string column, string caption)
        {
            _Column = column;
            _Caption = caption;
        }

        #region IFilterCondition<TEntity> Members

        public void Init(NameValueCollection req)
        {
            if (!string.IsNullOrEmpty(req["ftt" + _Column]))
                Int32.TryParse(req["ftt" + _Column], out _Type);
        }

        public IQueryable<TEntity> Filter(IQueryable<TEntity> list)
        {
            switch (_Type)
            {
                case 1:
                    return list.Where(String.Format(CultureInfo.InvariantCulture, "{0}.Any()", _Column));
                case 2:
                    return list.Where(String.Format(CultureInfo.InvariantCulture, "{0}.Any() == false", _Column));
            }
            return list;
        }

        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(CultureInfo.InvariantCulture, @"<div class=""filterfield""><table width=""100%""><tr><td class=""tdlabel""><label for=""flt{0}"">{1}</label></td>", _Column, _Caption));
            sb.Append(string.Format(CultureInfo.InvariantCulture, @"<td class=""tdcondition""><select class=""flttype"" id=""ftt{0}"" name=""ftt{0}""><option value=""0"" />", _Column));
            for (int i = 0; i < _Types.Length; i++)
                sb.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"" {2}>{1}</option>", i + 1, _Types[i], ((_Type == (i + 1)) ? " selected" : "")));
            sb.Append("</select></td><td class=\"tdvalue\">&nbsp;</td></tr></table></div>");
            return sb.ToString();
        }

        #endregion

        public override string ToString()
        {
            if (_Type <= 0) return string.Empty; 
            return string.Format("ftt{1}={2}", _Column, _Type);
        }
    }
}
