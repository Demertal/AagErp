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

        public override bool Equals(Object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            return Id == ((UnitStorages)obj).Id && Title == ((UnitStorages)obj).Title;
        }
    }
}
