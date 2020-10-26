using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HoanBookListData.Extensions
{
    public static class LinqFilterExtensions
    {
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }

    public static class LinqSortingExtensions
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string propertyName, bool ascending)
        {
            if (string.IsNullOrEmpty(propertyName))
                return query;

            var type = typeof(T);

            var propInfo = type.GetProperty(propertyName);

            if (propInfo == null)
                throw new Exception("Wrong PropertyName");

            ParameterExpression prm = Expression.Parameter(type);

            Expression property = Expression.Property(prm, propertyName);

            Type propertyType = property.Type;

            MethodInfo method = typeof(LinqSortingExtensions).GetMethod(nameof(OrderByProperty), BindingFlags.Static | BindingFlags.NonPublic)
                                                  .MakeGenericMethod(typeof(T), propertyType);

            return (IEnumerable<T>)method.Invoke(null, new object[] { query, prm, property, ascending });
        }

        private static IEnumerable<T> OrderByProperty<T, P>(this IEnumerable<T> query, ParameterExpression prm, Expression property, bool ascending)
        {
            Func<IEnumerable<T>, Func<T, P>, IEnumerable<T>> orderBy = (q, p) => ascending ? q.OrderBy(p) : q.OrderByDescending(p);

            return orderBy(query, Expression.Lambda<Func<T, P>>(property, prm).Compile());
        }
    }
}