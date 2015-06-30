using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Common.Core;

namespace Common.Web.Mvc.Controls
{
    public class SelectFilterCondition<TEntity, TProperty> : SelectFilterCondition, IFilterCondition<TEntity>
        where TEntity : class
    {
        public SelectFilterCondition(Expression<Func<TEntity, TProperty>> expression, IEnumerable<SelectListItem> dictionary, string caption = null)
            : base(expression.Parameters, expression.Body, caption, dictionary)
        {
        }

        public virtual IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            if (Value == null || string.IsNullOrEmpty(Value.Condition))
                return query;

            var condition = (Condition)Enum.Parse(typeof(Condition), Value.Condition);

            Expression body = null;

            if (condition == Condition.Defined)
                body = Expression.NotEqual(Property, Expression.Constant(null));

            if (condition == Condition.Undefined)
                body = Expression.Equal(Property, Expression.Constant(null));

            if (condition == Condition.IsIn || condition == Condition.IsNotIn)
            {
                var values = Value.Values;

                if (values != null && values.Any())
                {
                    foreach (var value in values)
                    {
                        if (string.IsNullOrEmpty(value))
                            continue;

                        var typedValue = ParseToPropertyType<TProperty>(value);

                        switch (condition)
                        {
                            case Condition.IsIn:
                                {
                                    var expression = Expression.Equal(Property, Expression.Constant(typedValue).ToSameType(Property));

                                    body = body != null ? Expression.OrElse(body, expression) : expression;

                                    break;
                                }
                            case Condition.IsNotIn:
                                {
                                    var expression = Expression.NotEqual(Property, Expression.Constant(typedValue).ToSameType(Property));

                                    body = body != null ? Expression.AndAlso(body, expression) : expression;

                                    break;
                                }
                        }
                    }
                }
            }

            if (body != null)
            {
                var predicate = Expression.Lambda<Func<TEntity, bool>>(body, Parameter);

                query = query.Where(predicate);
            }

            return query;
        }
    }

    public class SelectFilterCondition : FilterConditionBase
    {
        protected enum Condition
        {
            IsIn,
            IsNotIn,
            Defined,
            Undefined
        }

        public IEnumerable<SelectListItem> Dictionary;

        public SelectFilterCondition(IEnumerable<ParameterExpression> expressionParameters, Expression expressionBody, string caption, IEnumerable<SelectListItem> dictionary)
            : base(expressionParameters, expressionBody, caption)
        {
            Dictionary = dictionary;
        }
    }
}
