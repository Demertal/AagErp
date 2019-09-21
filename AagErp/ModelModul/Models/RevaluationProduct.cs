using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModelModul.Models
{
    public class RevaluationProduct : ModelBase<RevaluationProduct>
    {
        public RevaluationProduct()
        {
            PriceProductsCollection = new ObservableCollection<PriceProduct>();
        }

        private long _id;
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dateRevaluation;
        public DateTime? DateRevaluation
        {
            get => _dateRevaluation;
            set
            {
                _dateRevaluation = value;
                OnPropertyChanged();
            }
        }

        private ICollection<PriceProduct> _priceProductsCollection;
        public virtual ICollection<PriceProduct> PriceProductsCollection
        {
            get => _priceProductsCollection;
            set
            {
                _priceProductsCollection = value;
                OnPropertyChanged();
            }
        }

        public override object Clone()
        {
            return new RevaluationProduct
            {
                Id = Id,
                DateRevaluation = DateRevaluation,
                PriceProductsCollection = new List<PriceProduct>(PriceProductsCollection.Select(pp => (PriceProduct) pp.Clone()).ToList())
            };
        }

        public override bool IsValid => true;
    }
}
