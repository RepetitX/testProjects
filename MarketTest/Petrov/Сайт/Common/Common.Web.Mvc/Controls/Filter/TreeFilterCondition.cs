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
    public class TreeFilterCondition<TEntity> : IFilterCondition<TEntity>
        where TEntity : class, new()
    {
        string _Column;
        string _Caption;
        string _Value;

        string _ValueField;
        string _DisplayField;
        IQueryable _Dictionary;

        int _Type = 0;

        public string Column
        {
            get { return _Column; }
            set { _Column = value; _ColumnId = value.Replace('.', '_'); }
        }

        private string _ColumnId;

        public bool Active
        {
            get { return _Type > 0 && !string.IsNullOrEmpty(_Value); }
        }

        string[] _Types = { HTMLHelper.Resource("Filter", "Equal", "равно"), HTMLHelper.Resource("Filter", "EqualOrChild", "равно или дочернее"), HTMLHelper.Resource("Filter", "IsIn", "входит в"), HTMLHelper.Resource("Filter", "NotIsIn", "не входит в") };

        public TreeFilterCondition(string column, string caption, IQueryable dictionary)
            : this(column, caption, dictionary, "Path", "FullName")
        { }

        public TreeFilterCondition(string column, string caption, IQueryable dictionary, string valuefield, string displayfield)
        {
            Column = column;
            _Caption = caption;
            _Dictionary = dictionary;

            _ValueField = valuefield;
            _DisplayField = displayfield;
        }

        #region IFilterCondition<TEntity> Members

        public IQueryable<TEntity> Filter(IQueryable<TEntity> list)
        {
            if (_Type < 1 || string.IsNullOrEmpty(_Value)) return list;

            string[] ids = _Value.Split(',');
            if (_Type <= 2)
            {
                if (ids.Length == 0) return list;
                if (_Type == 1)
                    return list.Where(string.Format("{0}==\"{1}\"", Column, ids[0]));
                else
                    return list.Where(string.Format("{0}.StartsWith(\"{1}\")",Column,ids[0]));
            }
            else
            {
                if (ids != null && ids.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string op = (_Type == 3) ? "==" : "!=";
                    string dv = (_Type == 3) ? "||" : "&&";
                    foreach (string id in ids)
                    {
                        if (sb.Length > 0) sb.Append(dv);
                        sb.Append(_Column).Append(op).Append('"').Append(id).Append('"');
                    }
                    return list.Where(sb.ToString());
                }
            }
            return list;
        }

        #endregion

        #region IFilterCondition Members

        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""filterfield""><table><tr><td class=""tdlabel""><label>").Append(_Caption).Append("</label></td>");
            sb.Append(string.Format(CultureInfo.InvariantCulture, @"<td class=""tdcondition""><select class=""flttype ftttree"" id=""ftt{0}"" name=""ftt{0}""><option value=""0"" />", _ColumnId));
            for (int i = 0; i < _Types.Length; i++)
                sb.Append(string.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"" {2}>{1}</option>", i + 1, _Types[i], ((_Type == (i + 1)) ? " selected" : "")));
            sb.Append("</select></td>");
            string[] ids = !string.IsNullOrEmpty(_Value) ? _Value.Split(',') : null;
            DataTable dt = DataBinder.EvalDataSource(_Dictionary, _DisplayField, _ValueField);
            sb.AppendFormat(CultureInfo.InvariantCulture, @"<td class=""tdvalue""><div><select id=""flt{0}"" name=""flt{0}"" multiple=""multiple"" title=""{1}"" class=""flttree multipleselect multitree"">", _ColumnId, _Caption);
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(String.Format(CultureInfo.InvariantCulture, @"<option value=""{0}"" {2}>{1}</option>", Convert.ToString(dr[_ValueField], CultureInfo.InvariantCulture), Convert.ToString(dr[_DisplayField], CultureInfo.InvariantCulture), ((ids != null && ids.Contains(Convert.ToString(dr[_ValueField], CultureInfo.InvariantCulture)) ? "selected=\"true\"" : ""))));
            }
            sb.Append("</select></div></td></tr></table></div>");
            return sb.ToString();
        }

        public void Init(NameValueCollection req)
        {
            _Value = req["flt" + _ColumnId];
            if (!string.IsNullOrEmpty(req["ftt" + _ColumnId]))
                Int32.TryParse(req["ftt" + _ColumnId], out _Type);
        }

        #endregion

        public override string ToString()
        {
            if (_Type <= 0) return string.Empty;
            return string.Format("ftt{0}={1}&flt{0}={2}", _ColumnId, _Type, _Value);
        }
    }
}
