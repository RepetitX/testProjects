using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Core;

namespace Common.Web.Mvc.Controls
{
    public class DateFilterCondition<TEntity, TProperty> : DateFilterCondition, IFilterCondition<TEntity>
        where TEntity : class
    {
        public DateFilterCondition(Expression<Func<TEntity, TProperty>> expression, string caption = null)
            : base(expression.Parameters, expression.Body, caption)
        {
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            if (Value == null || string.IsNullOrEmpty(Value.Condition))
                return query;

            var values = Value.Values;

            if (values != null && values.Any())
            {
                var condition = ((Condition)Enum.Parse(typeof(Condition), Value.Condition));

                Expression body = null;

                var firstValue = DateTime.MinValue;
                var secondValue = DateTime.MinValue;

                switch (condition)
                {
                    case Condition.Equal:
                        {
                            if (DateTime.TryParse(values[0], out firstValue))
                            {
                                if (firstValue.Hour > 0 || firstValue.Minute > 0)
                                {
                                    body = Expression.Equal(Property, Expression.Constant(firstValue).ToSameType(Property));
                                }
                                else
                                {
                                    var yearExp = Expression.Property(Property, "Year");
                                    var monthExp = Expression.Property(Property, "Month");
                                    var dayExp = Expression.Property(Property, "Day");

                                    body = Expression.Equal(yearExp, Expression.Constant(firstValue.Year).ToSameType(yearExp));
                                    body = Expression.AndAlso(body, Expression.Equal(monthExp, Expression.Constant(firstValue.Month).ToSameType(monthExp)));
                                    body = Expression.AndAlso(body, Expression.Equal(dayExp, Expression.Constant(firstValue.Day).ToSameType(dayExp)));
                                }
                            }

                            break;
                        }
                    case Condition.Between:
                        {
                            if (DateTime.TryParse(values[0], out firstValue))
                                body = Expression.GreaterThanOrEqual(Property, Expression.Constant(firstValue).ToSameType(Property));

                            if (values.Count >= 2 && DateTime.TryParse(values[1], out secondValue))
                            {
                                if (secondValue.Hour == 0 && secondValue.Minute == 0)
                                    secondValue = new DateTime(secondValue.Year, secondValue.Month, secondValue.Day, 23, 59, 59);

                                var expression = Expression.LessThanOrEqual(Property, Expression.Constant(secondValue).ToSameType(Property));

                                body = body != null ? Expression.AndAlso(body, expression) : expression;
                            }

                            break;
                        }
                }

                if (body != null)
                {
                    var predicate = Expression.Lambda<Func<TEntity, bool>>(body, Parameter);

                    query = query.Where(predicate);
                }
            }

            return query;
        }
    }

    public class DateFilterCondition : FilterConditionBase
    {
        protected enum Condition
        {
            Equal,
            Between,
        }

        public DateFilterCondition(IEnumerable<ParameterExpression> expressionParameters, Expression expressionBody, string caption)
            : base(expressionParameters, expressionBody, caption)
        {
        }
    }
}
