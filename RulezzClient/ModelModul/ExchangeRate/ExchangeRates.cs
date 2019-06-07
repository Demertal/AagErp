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
                Course = Course
            };
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            return Id == ((ExchangeRates)obj).Id && Title == ((ExchangeRates)obj).Title && Course == ((ExchangeRates)obj).Course;
        }
    }
}
