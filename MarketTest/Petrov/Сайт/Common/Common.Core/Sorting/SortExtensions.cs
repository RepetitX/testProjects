using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Core.Sorting
{
    /// <summary>
    /// Extension methods for sorting.
    /// </summary>
    public static class SortExtensions
    {
        /// <summary>
        /// Orders a datasource by a property with the specified name in the specified direction
        /// </summary>
        /// <param name="datasource">The datasource to order</param>
        /// <param name="propertyName">The name of the property to order by</param>
        /// <param name="direction">The direction</param>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> datasource, string propertyName, SortDirection direction)
        {
            return datasource.AsQueryable().OrderBy(propertyName, direction);
        }

        /// <summary>
        /// Orders a datasource by a property with the specified name in the specified direction
        /// </summary>
        /// <param name="datasource">The datasource to order</param>
        /// <param name="propertyName">The name of the property to order by</param>
        /// <param name="direction">The direction</param>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> datasource, string propertyName, SortDirection direction)
        {
            return direction == SortDirection.Ascending ? datasource.OrderBy(propertyName) : datasource.OrderByDescending(propertyName);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> datasource, string propertyName, SortDirection direction)
        {
            return direction == SortDirection.Ascending ? datasource.ThenBy(propertyName) : datasource.ThenByDescending(propertyName);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");

            Expression expr = arg;

            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable)
                .GetMethods()
                .Single(method => method.Name == methodName
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 2
                    && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result;
        }
    }
}