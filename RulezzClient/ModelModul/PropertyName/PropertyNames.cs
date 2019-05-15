using System;

namespace ModelModul
{
    public partial class PropertyNames: ICloneable
    {
        public object Clone()
        {
            return new PropertyNames
            {
                Id = Id,
                Groups = Groups,
                IdGroup = IdGroup,
                Title = Title
            };
        }
    }
}
