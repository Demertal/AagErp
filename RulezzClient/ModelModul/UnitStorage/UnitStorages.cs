using System;

namespace ModelModul
{
    public partial class UnitStorages: ICloneable
    {
        public object Clone()
        {
            return new UnitStorages
            {
                Id = Id,
                Title = Title
            };
        }
    }
}
