using System;

namespace ModelModul
{
    public partial class UnitStorages: ICloneable, IEquatable<UnitStorages>
    {
        public object Clone()
        {
            return new UnitStorages
            {
                Id = Id,
                Title = Title
            };
        }

        public bool Equals(UnitStorages other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Title == other.Title;
        }
    }
}
