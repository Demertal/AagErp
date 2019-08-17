using System;
using System.Collections.Generic;

namespace ModelModul.Models
{
    public class RevaluationProducts : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RevaluationProducts()
        {
            PriceProducts = new List<PriceProduct>();
        }

        private long _id;
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private DateTime _dateRevaluation;
        public DateTime DateRevaluation
        {
            get => _dateRevaluation;
            set
            {
                _dateRevaluation = value;
                OnPropertyChanged("DateRevaluation");
            }
        }

        private ICollection<PriceProduct> _priceProducts;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PriceProduct> PriceProducts
        {
            get => _priceProducts;
            set
            {
                _priceProducts = value;
                OnPropertyChanged("PriceProducts");
            }
        }

        public object Clone()
        {
            return new RevaluationProducts
            {
                Id = Id,
                DateRevaluation = DateRevaluation
            };
        }
    }
}
