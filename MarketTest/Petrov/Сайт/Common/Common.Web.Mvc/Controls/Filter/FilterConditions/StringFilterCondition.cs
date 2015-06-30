using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Web.Mvc.Controls
{
    public class StringFilterCondition<TEntity, TProperty> : StringFilterCondition, IFilterCondition<TEntity>
        where TEntity : class
    {
        public StringFilterCondition(Expression<Func<TEntity, TProperty>> expression, string caption = null)
            : base(expression.Parameters, expression.Body, caption)
        {
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            if (Value == null || string.IsNullOrEmpty(Value.Condition))
                return query;

            var condition = (Condition)Enum.Parse(typeof(Condition), Value.Condition);

            if (condition == Condition.None)
                return query;

            Expression body = null;

            if (condition == Condition.Defined)
            {
                body = Expression.GreaterThan(Expression.Property(Property, "Length"), Expression.Constant(0));
            }
            else if (condition == Condition.Undefined)
            {
                var firstExpression = Expression.Equal(Property, Expression.Constant(null));

                var secondExpression = Expression.Equal(Expression.Property(Property, "Length"), Expression.Constant(0));

                body = Expression.OrElse(firstExpression, secondExpression);
            }
            else
            {
                var values = Value.Values;

                if (values != null && values.Any() && !string.IsNullOrEmpty(values[0]))
                {
                    var value = values[0];

                    switch (condition)
                    {
                        case Condition.Contains:
                        {
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                            body = Expression.Call(Property, containsMethod, Expression.Constant(value));

                            break;
                        }
                        case Condition.DoesNotContain:
                        {
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                            body = Expression.Not(Expression.Call(Property, containsMethod, Expression.Constant(value)));

                            break;
                        }
                        case Condition.Equal:
                        {
                            body = Expression.Equal(Property, Expression.Constant(value));

                            break;
                        }
                        case Condition.NotEqual:
                        {
                            body = Expression.NotEqual(Property, Expression.Constant(value));

                            break;
                        }
                        case Condition.StartsWith:
                        {
                            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

                            body = Expression.Call(Property, startsWithMethod, Expression.Constant(value));

                            break;
                        }
                        case Condition.EndsWith:
                        {
                            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

                            body = Expression.Call(Property, endsWithMethod, Expression.Constant(value));

                            break;
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

    public class StringFilterCondition : FilterConditionBase
    {
        protected enum Condition
        {
            None,
            Contains,
            DoesNotContain,
            Equal,
            NotEqual,
            StartsWith,
            EndsWith,
            Defined,
            Undefined
        }

        public StringFilterCondition(IEnumerable<ParameterExpression> expressionParameters, Expression expressionBody, string caption)
            : base(expressionParameters, expressionBody, caption)
        {
        }
    }
}