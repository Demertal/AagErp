using System;

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
                Title = Title
            };
        }

        //public override bool Equals(Object obj)
        //{
        //    if (obj == null || !GetType().Equals(obj.GetType()))
        //    {
        //        return false;
        //    }
        //    return Id == ((Groups)obj).Id && Title == ((Groups)obj).Title && IdParentGroup == ((Groups)obj).IdParentGroup;
        //}
    }
}
