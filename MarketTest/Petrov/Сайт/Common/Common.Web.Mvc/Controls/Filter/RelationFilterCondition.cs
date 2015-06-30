using System;
using System.Collections;
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
    public class RelationFilterCondition<TEntity> : IFilterCondition<TEntity>
        where TEntity : class, new()
    {
        string _Caption;
        string _Value;
        string _ChildProperty;
        string _ParentField;

        string _ValueField;
        string _DisplayField;
        IEnumerable _Dictionary;

        int _Type = 0;
        string[] _Types = { HTMLHelper.Resource("Filter", "Equal", "равно"), HTMLHelper.Resource("Filter", "NotEqual", "не равно"), HTMLHelper.Resource("Filter", "ContainsAny", "содержит любое из"), HTMLHelper.Resource("Filter", "NotContainsAny", "не содержит любое из"), HTMLHelper.Resource("Filter", "ContainsAll", "содержит все из"), HTMLHelper.Resource("Filter", "ContainsNone", "не содержит все из"), HTMLHelper.Resource("Filter", "Defined", "задано"), HTMLHelper.Resource("Filter", "Undefined", "не задано") };

        public string Column
        {
            get { return _ParentField; }
            set { _ParentField = value; }
        }

        public bool Active
        {
            get { return _Type > 0 && !string.IsNullOrEmpty(_Value); }
        }

        public RelationFilterCondition(string childProperty, string parentField, string caption, IEnumerable dictionary)
            : this(childProperty, parentField, caption, dictionary, "Id", "Name")
        { }

        public RelationFilterCondition(string childProperty, string parentField, string caption, IEnumerable dictionary, string valuefield, string displayfield)
        {
            _Caption = caption;
            _ChildProperty = childProperty;
            _ParentField = parentField;

            _Dictionary = dictionary;
            _ValueField = valuefield;
            _DisplayField = displayfield;
        }

        #region IFilterCondition<TEntity> Members

        public IQueryable<TEntity> Filter(IQueryable<TEntity> list)
        {
            if (_Type < 1) return list;
            if (_Type == 7) return list.Where(string.Format(CultureInfo.InvariantCulture, "{0}.Count() > 0", _ChildProperty));
            if (_Type == 8) return list.Where(string.Format(CultureInfo.InvariantCulture, "{0}.Count() == 0", _ChildProperty));

            string clause = "";
            int[] ids = RequestHelper.GetIdsFromString(_Value);
            if (ids != null && ids.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                string op = "==";
                string dv = "||";
                switch (_Type)
                {
                    case 1:
                        clause = string.Format(CultureInfo.InvariantCulture, "=={0} && {1}.Count() == {0}", ids.Length, _ChildProperty);
                        break;
                    case 2:
                        clause = string.Format(CultureInfo.InvariantCulture, "<{0} || {1}.Count() > {0}", ids.Length, _ChildProperty);
                        break;
                    case 3:
                        clause = ">0";
                        break;
                    case 4:
                        clause = "==0";
                        break;
                    case 5:
                        clause = string.Format(CultureInfo.InvariantCulture, "=={0}", ids.Length);
                        break;
                    case 6:
                        clause = string.Format(CultureInfo.InvariantCulture, "<{0}", ids.Length);
                        break;
                }
                foreach (int id in ids)
                {
                    if (sb.Length > 0) sb.Append(dv);
                    sb.Append(_ParentField).Append(op).Append(id);
                }
                string where = string.Format(CultureInfo.InvariantCulture, "{0}.Where({1}).Count() {2}", _ChildProperty, sb.ToString(), clause);
                return list.Where(where);
            }
            return list;
        }

        #endregion

        #region IFilterCondition Members

        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""filterfield""><table width=""100%""><tr><td class=""tdlabel""><label>").Append(_Caption).AppendFormat("&nbsp;<a class=\"small\" href=\"{0}\" onclick=\"openDialog('{0}'); return false;\">[?]</a></label></td>", HTMLHelper.Resource("Filter", "RelatedFilterHelp", "help/related_filter.htm"));
            sb.Append(string.Format(CultureInfo.InvariantCulture, @"<td class=""tdcondition""><select class=""flttype fttrelation"" id=""ftt{0}"" name=""ftt{0}""><option value=""0"" />", _ChildProperty));
            for (int i = 0; i < _Types.Length; i++)
                sb.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"" {2}>{1}</option>", i + 1, _Types[i], ((_Type == (i + 1)) ? " selected" : "")));
            sb.Append("</select></td>");
            int[] ids = RequestHelper.GetIdsFromString(_Value);
            DataTable dt = DataBinder.EvalDataSource(_Dictionary, _DisplayField, _ValueField);
            sb.AppendFormat(CultureInfo.InvariantCulture, @"<td class=""tdvalue""><div {2}><select id=""flt{0}"" name=""flt{0}"" multiple=""multiple"" title=""{1}"" class=""fltselect multipleselect"">", _ChildProperty, _Caption, (_Type == 7 || _Type == 8) ? "class=\"hidden\"" : "");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(String.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"" {2}>{1}</option>", Convert.ToString(dr[_ValueField], CultureInfo.InvariantCulture), Convert.ToString(dr[_DisplayField], CultureInfo.InvariantCulture), ((ids != null && ids.Contains(Convert.ToInt32(dr[_ValueField], CultureInfo.InvariantCulture)) ? "selected=\"true\"" : ""))));
            }
            sb.Append("</select></div></td></tr></table></div>");
            return sb.ToString();
        }

        public void Init(NameValueCollection req)
        {
            _Value = req["flt" + _ChildProperty];
            if (!string.IsNullOrEmpty(req["ftt" + _ChildProperty]))
                Int32.TryParse(req["ftt" + _ChildProperty], out _Type);
        }

        #endregion

        public override string ToString()
        {
            if (_Type <= 0) return string.Empty;
            return string.Format("ftt{0}={1}&flt{0}={2}", _ChildProperty, _Type, _Value);
        }
    }
}
