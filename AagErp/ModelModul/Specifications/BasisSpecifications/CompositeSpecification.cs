using System;
using System.Linq.Expressions;

namespace ModelModul.Specifications.BasisSpecifications
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> IsSatisfiedBy();

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }
        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }
        public ISpecification<T> Not(ISpecification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }
    }
}
