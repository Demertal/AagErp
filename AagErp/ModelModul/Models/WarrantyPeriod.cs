using System.Collections.Generic;

namespace ModelModul.Models
{
    public class WarrantyPeriod : ModelBase
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
                OnPropertyChanged("Id");
            }
        }

        private string _period;
        public virtual string Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged("Period");
                OnPropertyChanged("IsValidate");
            }
        }

        private ICollection<Product> _productsCollection;
        public virtual ICollection<Product> ProductsCollection
        {
            get => _productsCollection;
            set
            {
                _productsCollection = value;
                OnPropertyChanged("ProductsCollection");
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

        public override bool IsValidate => !string.IsNullOrEmpty(Period);

        public override object Clone()
        {
            return new WarrantyPeriod {Id = Id, Period = Period};
        }
    }
}
