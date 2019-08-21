using System;
using System.Linq.Expressions;

namespace ModelModul.Specifications.BasisSpecifications
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> spec)
        {
            _specification = spec;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            var parameterExpression = Expression.Parameter(typeof(T));
            var spec = Expression.IsFalse(_specification.IsSatisfiedBy());
            return Expression.Lambda<Func<T, bool>>(spec, parameterExpression);
        }
    }
}
