using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class CounterpartySpecification
    {
        public static ExpressionSpecification<Counterparty> GetCounterpartiesByType(TypeCounterparties type)
        {
            return new ExpressionSpecification<Counterparty>(obj => obj.WhoIsIt == type);
        }
    }
}
