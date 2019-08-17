using System;
using System.Linq.Expressions;
using ModelModul.Specifications.BasisSpecifications.Utilities;

namespace ModelModul.Specifications.BasisSpecifications
{
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private readonly ISpecification<T> _leftSpecification;
        private readonly ISpecification<T> _rightSpecification;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _leftSpecification = left;
            _rightSpecification = right;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            return SpecificationUtility.BuildAnd(_leftSpecification.IsSatisfiedBy(), _rightSpecification.IsSatisfiedBy());
        }
    }
}
