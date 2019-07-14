using System;

namespace ModelModul
{
    public enum TypeCounterparties
    {
        Suppliers = 0,
        Buyers = 1
    }

    public partial class Counterparties: ICloneable
    {
        public object Clone()
        {
            return new Counterparties
            {
                Id = Id,
                Address = Address,
                ContactPerson = ContactPerson,
                ContactPhone = ContactPhone,
                Props = Props,
                Title = Title,
                Debt = Debt,
                WhoIsIt = WhoIsIt
            };
        }
    }
}
