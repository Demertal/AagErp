using System;
using System.Collections.Generic;

namespace ModelModul.Models
{
    public class SerialNumber : ModelBase<SerialNumber>
    {
        public SerialNumber()
        {
            SerialNumberLogsCollection = new List<SerialNumberLog>();
            WarrantiesCollection = new List<Warranty>();
            ChangesCollection = new List<Warranty>();
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

        private string _value;
        public virtual string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private long _idProduct;
        public long IdProduct
        {
            get => _idProduct;
            set
            {
                _idProduct = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dateCreated;
        public DateTime? DateCreated
        {
            get => _dateCreated;
            set
            {
                _dateCreated = value;
                OnPropertyChanged();
            }
        }

        private Product _product;
        public virtual Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        private ICollection<SerialNumberLog> _serialNumberLogsCollection;
        public virtual ICollection<SerialNumberLog> SerialNumberLogsCollection
        {
            get => _serialNumberLogsCollection;
            set
            {
                _serialNumberLogsCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<Warranty> _warrantiesCollection;
        public virtual ICollection<Warranty> WarrantiesCollection
        {
            get => _warrantiesCollection;
            set
            {
                _warrantiesCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<Warranty> _changesCollection;
        public virtual ICollection<Warranty> ChangesCollection
        {
            get => _changesCollection;
            set
            {
                _changesCollection = value;
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
                    case "Value":
                        if (string.IsNullOrEmpty(Value))
                        {
                            error = "Номер должен быть указан";
                        }

                        break;
                }

                return error;
            }
        }

        public override object Clone()
        {
            return new SerialNumber
            {
                Id = Id,
                IdProduct = IdProduct,
                Value = Value,
                DateCreated = DateCreated
            };
        }

        public override bool IsValid => !string.IsNullOrEmpty(Value) && !HasErrors;
    }
}