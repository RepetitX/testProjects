using System;
using System.Linq.Expressions;

namespace Common.Core
{
    public static class ExpressionExtensions
    {
        public static Expression ToSameType(this Expression exp, Expression targetTypeExp)
        {
            if (IsNullableType(targetTypeExp.Type) && !IsNullableType(exp.Type))
                exp = Expression.Convert(exp, targetTypeExp.Type);

            return exp;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}