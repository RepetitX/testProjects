using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Common.Web.Mvc.Controls
{
    public class SingleSelectFilterCondition<TEntity, TProperty> : SingleSelectFilterCondition, IFilterCondition<TEntity>
        where TEntity : class
    {
        public SingleSelectFilterCondition(Expression<Func<TEntity, TProperty>> expression, IEnumerable<SelectListItem> dictionary, string caption = null)
            : base(expression.Parameters, expression.Body, caption, dictionary)
        {
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            if (Value == null || string.IsNullOrEmpty(Value.Condition))
                return query;

            var values = Value.Values;

            if (values != null && values.Any() && !string.IsNullOrEmpty(values[0]))
            {
                var condition = (Condition)Enum.Parse(typeof(Condition), Value.Condition);

                var typedValue = Convert.ChangeType(values[0], typeof(TProperty));

                Expression body = null;

                if (condition == Condition.NotEqual)
                    body = Expression.NotEqual(Property, Expression.Constant(typedValue));

                if (condition == Condition.Equal)
                    body = Expression.Equal(Property, Expression.Constant(typedValue));

                if (body != null)
                {
                    var predicate = Expression.Lambda<Func<TEntity, bool>>(body, Parameter);

                    query = query.Where(predicate);
                }
            }

            return query;
        }
    }

    public class SingleSelectFilterCondition : FilterConditionBase
    {
        protected enum Condition
        {
            Equal,
            NotEqual      
        }

        public IEnumerable<SelectListItem> Dictionary;

        public SingleSelectFilterCondition(IEnumerable<ParameterExpression> expressionParameters, Expression expressionBody, string caption, IEnumerable<SelectListItem> dictionary)
            : base(expressionParameters, expressionBody, caption)
        {
            Dictionary = dictionary;
        }
    }
}