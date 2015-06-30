using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Web.Mvc.Controls
{
    public class NumericFilterCondition<TEntity, TProperty> : NumericFilterCondition, IFilterCondition<TEntity>
        where TEntity : class
    {
        public NumericFilterCondition(Expression<Func<TEntity, TProperty>> expression, string caption = null)
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
                values = values.Where(v => !string.IsNullOrEmpty(v)).ToList();

                var condition = (Condition)Enum.Parse(typeof(Condition), Value.Condition);

                Expression body = null;

                switch (condition)
                {
                    case Condition.Equal:
                        {
                            try
                            {
                                var typedValue = Convert.ChangeType(values[0], typeof(TProperty));

                                body = Expression.Equal(Property, Expression.Constant(typedValue));
                            }
                            catch
                            {
                            }

                            break;
                        }
                    case Condition.Between:
                        {
                            try
                            {
                                var typedFirstValue = Convert.ChangeType(values[0], typeof(TProperty));

                                body = Expression.GreaterThanOrEqual(Property, Expression.Constant(typedFirstValue));
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (values.Count >= 2)
                                {
                                    var typedSecondValue = Convert.ChangeType(values[1], typeof(TProperty));

                                    var expression = Expression.LessThanOrEqual(Property, Expression.Constant(typedSecondValue));

                                    body = body != null ? Expression.AndAlso(body, expression) : expression;
                                }
                            }
                            catch
                            {
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

    public class NumericFilterCondition : FilterConditionBase
    {
        protected enum Condition
        {
            Between,
            Equal
        }

        public NumericFilterCondition(IEnumerable<ParameterExpression> expressionParameters, Expression expressionBody, string caption)
            : base(expressionParameters, expressionBody, caption)
        {
        }
    }
}