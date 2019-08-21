using System;
using System.Linq;
using System.Linq.Expressions;

namespace ModelModul.Specifications.BasisSpecifications.Utilities
{
    public static class SpecificationUtility
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> BuildAnd<T>(params Expression<Func<T, bool>>[] conditions)
        {
            return conditions.Aggregate<Expression<Func<T, bool>>, Expression<Func<T, bool>>>(null, (current, expression) => current == null ? expression : current.And(expression));
        }

        public static Expression<Func<T, bool>> BuildOr<T>(params Expression<Func<T, bool>>[] conditions)
        {
            return conditions.Aggregate<Expression<Func<T, bool>>, Expression<Func<T, bool>>>(null, (current, expression) => current == null ? expression : current.Or(expression));
        }

        public static Expression<Func<T, bool>> BuildOrElse<T>(params Expression<Func<T, bool>>[] conditions)
        {
            return conditions.Aggregate<Expression<Func<T, bool>>, Expression<Func<T, bool>>>(null, (current, expression) => current == null ? expression : current.OrElse(expression));
        }

    }
}
