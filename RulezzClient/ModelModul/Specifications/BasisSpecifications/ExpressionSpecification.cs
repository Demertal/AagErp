using System;
using System.Linq.Expressions;

namespace ModelModul.Specifications.BasisSpecifications
{
    public class ExpressionSpecification<T> : CompositeSpecification<T>
    {
        private readonly Expression<Func<T, bool>> _expression;
        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException();
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            return _expression;
        }
    }
}
