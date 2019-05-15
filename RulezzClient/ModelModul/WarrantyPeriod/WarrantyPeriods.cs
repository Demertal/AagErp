using System;

namespace ModelModul
{
    public partial class WarrantyPeriods : ICloneable, IEquatable<WarrantyPeriods>
    {
        public object Clone()
        {
            return new WarrantyPeriods{Id = Id, Period = Period};
        }

        public bool Equals(WarrantyPeriods other)
        {
            return other != null && Id == other.Id && Period == other.Period;
        }
    }
}
