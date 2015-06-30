using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Web.Mvc.Controls
{
    public class FilterConditionBase : IFilterCondition
    {
        public string Key { get; private set; }
        public string Caption { get; private set; }
        public FilterConditionValue Value { get; set; }

        protected ParameterExpression Parameter { get; set; }
        protected MemberExpression Property { get; set; }

        protected FilterConditionBase(IEnumerable<ParameterExpression> expressionParameters, Expression expressionBody, string caption)
        {
            // Проверяем корректность построенного выражения.

            var memberExpression = expressionBody as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException(string.Format("Выражение '{0}' ссылается на метод, а не на свойство.", expressionBody));

            var propertyInfo = memberExpression.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException(string.Format("Выражение '{0}' ссылается на поле, а не на свойство.", expressionBody));

            // Получаем название поля фильтра

            if (caption == null)
            {
                var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayAttribute));

                caption = displayAttribute != null ? displayAttribute.Name : propertyInfo.Name;
            }

            Key = string.Concat("__", memberExpression.ToString());
            Caption = caption;
            Parameter = expressionParameters.Single();
            Property = memberExpression;
        }

        public virtual string GetInputName(int index, string field)
        {
            return string.Format("FilterConditionValues[{0}].{1}", index, field);
        }

        public virtual string GetInputId(int index, string field)
        {
            return string.Format("FilterConditionValues{0}_{1}", index, field);
        }

        public static T ParseToPropertyType<T>(object value)
        {
            try
            {
                return (T)System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value.ToString());
            }
            catch
            {
                return default(T);
            }
        }
    }
}