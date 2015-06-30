using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using IntraVision.Web.Toolkit;

namespace IntraVision.Web.Mvc.Controls
{
    public class UserSelectFilterCondition<TEntity> : IFilterCondition<TEntity>
        where TEntity : class, new()
    {
        string _Column;
        string _Caption;
        string _Value;
        int _UserId;
        string _AutoCompleteUrl;
        string _SelectUrl;
        string _ValueField;
        string _DisplayField;
        IQueryable _Dictionary;
        
        int _Type = 0;

        public string Column
        {
            get { return _Column; }
            set { _Column = value; }
        }

        public bool Active
        {
            get { return _Type > 0 && !string.IsNullOrEmpty(_Value); }
        }

        string[] _Types = { HTMLHelper.Resource("Filter", "IsIn", "входит в"), HTMLHelper.Resource("Filter", "NotIsIn", "не входит в"), HTMLHelper.Resource("Filter", "Defined", "задано"), HTMLHelper.Resource("Filter", "Undefined", "не задано") };

        public UserSelectFilterCondition(string column, string caption, IQueryable dictionary,int userid, string acurl,string selurl)
            : this(column, caption, dictionary, userid, acurl, selurl, "Id", "Name")
        { }

        public UserSelectFilterCondition(string column, string caption, IQueryable dictionary, int userid, string acurl, string selurl, string valuefield, string displayfield)
        {
            _Column = column;
            _Caption = caption;
            _Dictionary = dictionary;
            _UserId = userid;
            _AutoCompleteUrl = acurl;
            _SelectUrl = selurl;
            _ValueField = valuefield;
            _DisplayField = displayfield;
        }

        #region IFilterCondition<TEntity> Members

        public IQueryable<TEntity> Filter(IQueryable<TEntity> list)
        {
            if (_Type == 3) return list.Where(_Column + "!=null");
            if (_Type == 4) return list.Where(_Column + "==null");
            if (_Type < 1 || string.IsNullOrEmpty(_Value)) return list;

            int[] ids = RequestHelper.GetIdsFromString(_Value);
            if (ids != null && ids.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                string op = (_Type == 1) ? "==" : "!=";
                string dv = (_Type == 1) ? "||" : "&&";
                foreach (int id in ids)
                {
                    if (sb.Length > 0) sb.Append(dv);
                    sb.Append(_Column).Append(op).Append(id>0?id:_UserId);
                }
                return list.Where(sb.ToString());
            }
            return list;
        }

        #endregion

        #region IFilterCondition Members

        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""filterfield""><table width=""100%""><tr><td class=""tdlabel""><label>").Append(_Caption).Append("</label></td>");
            sb.Append(string.Format(CultureInfo.InvariantCulture, @"<td class=""tdcondition""><select class=""flttype fttselect"" id=""ftt{0}"" name=""ftt{0}""><option value=""0"" />", _Column));
            for (int i = 0; i < _Types.Length; i++)
                sb.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"" {2}>{1}</option>", i + 1, _Types[i], ((_Type == (i + 1)) ? " selected" : "")));
            sb.Append("</select></td>");
            int[] ids = RequestHelper.GetIdsFromString(_Value);
            DataTable dt = DataBinder.EvalDataSource(_Dictionary, _DisplayField, _ValueField);
            //В зависимости от количества пользователей возможны два варианта: 
            //1.Вывести сразу всех, если их мало
            //2.Вывести только выбранных, если их много
            bool few = _Dictionary.Count() <= 10;
            string hidden = (_Type == 3 || _Type == 4) ? "class=\"hidden\"" : "";
            if (!few)
                sb.AppendFormat(CultureInfo.InvariantCulture, @"<td class=""tdvalue""><div {4}><select id=""flt{0}"" name=""flt{0}"" multiple=""multiple"" title=""{1}"" class=""fltselect multipleselect"" acurl=""{2}"" selurl=""{3}"">", _Column, _Caption, _AutoCompleteUrl, _SelectUrl, hidden);
            else
                sb.AppendFormat(CultureInfo.InvariantCulture, @"<td class=""tdvalue""><div {2}><select id=""flt{0}"" name=""flt{0}"" multiple=""multiple"" title=""{1}"" class=""fltselect multipleselect"">", _Column, _Caption, hidden);
            if (_UserId > 0) sb.AppendFormat(CultureInfo.InvariantCulture, @"<option value=""0"" {0}>{1}</option>", ids.Contains(0) ? " selected=\"selected\"" : "", HTMLHelper.Resource("Filter", "CurrentUser", "Текущий пользователь"));
            foreach (DataRow dr in dt.Rows)
            {
                int id = Convert.ToInt32(dr[_ValueField], CultureInfo.InvariantCulture);
                bool selected = ids.Contains(id);
                if (!few && !selected) continue;
                sb.Append(String.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"" {2}>{1}</option>", Convert.ToString(dr[_ValueField], CultureInfo.InvariantCulture), Convert.ToString(dr[_DisplayField], CultureInfo.InvariantCulture), selected ? "selected=\"true\"" : ""));
            }
            sb.Append("</select></div></td></tr></table></div>");
            return sb.ToString();
        }

        public void Init(NameValueCollection req)
        {
            _Value = req["flt" + _Column];
            if (!string.IsNullOrEmpty(req["ftt" + _Column]))
                Int32.TryParse(req["ftt" + _Column], out _Type);
        }

        #endregion

        public override string ToString()
        {
            if (_Type <= 0) return string.Empty;
            return string.Format("ftt{0}={1}&flt{0}={2}", _Column, _Type, _Value);
        }

    }
}
