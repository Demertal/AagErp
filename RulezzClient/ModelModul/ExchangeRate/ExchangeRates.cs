using System;

namespace ModelModul
{
    public partial class ExchangeRates: IEquatable<ExchangeRates>, ICloneable
    {
        public bool Equals(ExchangeRates other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Title == other.Title && Course == other.Course;
        }

        public object Clone()
        {
            return new ExchangeRates
            {
                Id = Id,
                Title = Title,
                Course = Course
            };
        }
    }
}
