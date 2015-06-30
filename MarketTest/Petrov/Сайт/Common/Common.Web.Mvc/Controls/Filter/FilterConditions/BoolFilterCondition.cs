using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Web.Mvc.Controls
{
    public class BoolFilterCondition<TEntity, TProperty> : BoolFilterCondition, IFilterCondition<TEntity>
        where TEntity : class
    {
        public BoolFilterCondition(Expression<Func<TEntity, TProperty>> expression, string caption = null, string noneText = "", string trueText = "Да", string falseText = "Нет")
            : base(expression.Parameters, expression.Body, caption, noneText, trueText, falseText)
        { 
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            if (Value == null || string.IsNullOrEmpty(Value.Condition))
                return query;

            var condition = (Condition)Enum.Parse(typeof(Condition), Value.Condition);

            if (condition != Condition.None)
            {
                var conditionValue = condition == Condition.True;

                var body = Expression.Equal(Property, Expression.Constant(conditionValue));

                var predicate = Expression.Lambda<Func<TEntity, bool>>(body, Parameter);

                query = query.Where(predicate);
            }

            return query;
        }
    }

    public class BoolFilterCondition : FilterConditionBase
    {
        protected enum Condition
        {
            None,
            True,
            False
        }

        public string NoneText { get; private set; }
        public string TrueText { get; private set; }
        public string FalseText { get; private set; }

        public BoolFilterCondition(IEnumerable<ParameterExpression> expressionParameters, Expression expressionBody, string caption, string noneText, string trueText, string falseText)
            : base(expressionParameters, expressionBody, caption)
        {
            NoneText = noneText;
            TrueText = trueText;
            FalseText = falseText;
        }
    }
}