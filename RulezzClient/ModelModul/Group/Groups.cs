using System;
using System.Collections.Generic;

namespace ModelModul
{
    public partial class Groups : ICloneable
    {
        public object Clone()
        {
            return new Groups
            {
                Id = Id,
                IdParentGroup = IdParentGroup,
                IdUnitStorage = IdUnitStorage,
                Title = Title,
                Groups2 = (Groups) Groups2?.Clone()
            };
        }
    }
}
