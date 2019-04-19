using System;

namespace ModelModul
{
    public partial class WarrantyPeriods : ICloneable
    {
        public object Clone()
        {
            return new WarrantyPeriods{Id = Id, Period = Period};
        }
    }
}
