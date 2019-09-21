using System.Collections.Generic;
namespace ModelModul.Models
{
    public class WarrantyPeriod : ModelBase<WarrantyPeriod>
    {
        public WarrantyPeriod()
        {
            ProductsCollection = new HashSet<Product>();
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _period;
        public virtual string Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private ICollection<Product> _productsCollection;
        public virtual ICollection<Product> ProductsCollection
        {
            get => _productsCollection;
            set
            {
                _productsCollection = value;
                OnPropertyChanged();
            }
        }

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Period":
                        if (string.IsNullOrEmpty(Period))
                        {
                            error = "Период должен быть указан";
                        }

                        break;
                }

                return error;
            }
        }

        public override object Clone()
        {
            return new WarrantyPeriod {Id = Id, Period = Period};
        }

        public override bool IsValid => !string.IsNullOrEmpty(Period) && !HasErrors;
    }
}
