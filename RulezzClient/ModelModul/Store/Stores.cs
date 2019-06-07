using System;

namespace ModelModul
{
    public partial class Stores: ICloneable
    {
        public object Clone()
        {
            return  new Stores
            {
                Id = Id,
                Title = Title
            };
        }
    }
}
