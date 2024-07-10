using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> WhereIn<T>(this IQueryable<T> source, string propertyName, List<string> values)
        {
            if (values == null || !values.Any())
            {
                return source;
            }

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, propertyName);

            var predicates = values.Select(value =>
                Expression.Call(
                    property,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                    Expression.Constant(value)
                )
            );

            var body = predicates.Aggregate<Expression>((accumulate, predicate) =>
                Expression.OrElse(accumulate, predicate)
            );

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
            return source.Where(lambda);
        }
    }
}
