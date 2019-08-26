using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModelModul.Models
{
    public class RevaluationProduct : ModelBase, ICloneable
    {
        public RevaluationProduct()
        {
            PriceProducts = new ObservableCollection<PriceProduct>();
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

        private DateTime? _dateRevaluation;
        public DateTime? DateRevaluation
        {
            get => _dateRevaluation;
            set
            {
                _dateRevaluation = value;
                OnPropertyChanged("DateRevaluation");
            }
        }

        private ICollection<PriceProduct> _priceProducts;
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
            return new RevaluationProduct
            {
                Id = Id,
                DateRevaluation = DateRevaluation,
                PriceProducts = new List<PriceProduct>(PriceProducts.Select(pp => (PriceProduct) pp.Clone()).ToList())
            };
        }
    }
}
