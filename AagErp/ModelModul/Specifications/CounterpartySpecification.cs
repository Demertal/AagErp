using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Specifications
{
    public static class CounterpartySpecification
    {
        public static ExpressionSpecification<Counterparty> GetCounterpartiesByType(ETypeCounterparties type)
        {
            return new ExpressionSpecification<Counterparty>(obj => obj.WhoIsIt == type);
        }
    }
}
