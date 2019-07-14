using System;

namespace ModelModul
{
    public partial class ExchangeRates: ICloneable
    {
        public object Clone()
        {
            return new ExchangeRates
            {
                Id = Id,
                Title = Title,
                Course = Course,
                IsDefault =  IsDefault
            };
        }
    }
}
