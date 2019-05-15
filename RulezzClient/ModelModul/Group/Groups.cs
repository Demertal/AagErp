using System;

namespace ModelModul
{
    public partial class Groups : ICloneable, IEquatable<Groups>
    {
        public object Clone()
        {
            return new Groups
            {
                Id = Id,
                IdParentGroup = IdParentGroup,
                Title = Title,
                Groups2 = (Groups) Groups2?.Clone()
            };
        }

        public bool Equals(Groups other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Title == other.Title && IdParentGroup == other.IdParentGroup;
        }
    }
}
