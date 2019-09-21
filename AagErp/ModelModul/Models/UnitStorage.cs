using System.Collections.Generic;

namespace ModelModul.Models
{
    public class UnitStorage : ModelBase<UnitStorage>
    {
        public UnitStorage()
        {
            ProductsCollection = new List<Product>();
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

        private string _title;
        public virtual string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private bool _isWeightGoods;
        public bool IsWeightGoods
        {
            get => _isWeightGoods;
            set
            {
                _isWeightGoods = value;
                OnPropertyChanged();
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
                    case "Title":
                        if (string.IsNullOrEmpty(Title))
                        {
                            error = "������������ ������ ���� �������";
                        }

                        break;
                }

                return error;
            }
        }

        public override object Clone()
        {
            return new UnitStorage {Id = Id, Title = Title, IsWeightGoods = IsWeightGoods};
        }

        public override bool IsValid => !string.IsNullOrEmpty(Title) && !HasErrors;
    }
}
