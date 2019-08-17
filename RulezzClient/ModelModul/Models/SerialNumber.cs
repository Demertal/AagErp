using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class SerialNumber : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SerialNumber()
        {
            SerialNumberLogs = new List<SerialNumberLog>();
            Warranties = new List<Warranty>();
            Change = new List<Warranty>();
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

        private string _value;
        [Required]
        [StringLength(20)]
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        private long _idProduct;
        public long IdProduct
        {
            get => _idProduct;
            set
            {
                _idProduct = value;
                OnPropertyChanged("IdProduct");
            }
        }

        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set
            {
                _dateCreated = value;
                OnPropertyChanged("DateCreated");
            }
        }

        private Product _product;
        public virtual Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged("Product");
            }
        }

        private ICollection<SerialNumberLog> _serialNumberLogs;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SerialNumberLog> SerialNumberLogs
        {
            get => _serialNumberLogs;
            set
            {
                _serialNumberLogs = value;
                OnPropertyChanged("SerialNumberLogs");
            }
        }

        private ICollection<Warranty> _warranties;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warranty> Warranties
        {
            get => _warranties;
            set
            {
                _warranties = value;
                OnPropertyChanged("Warranties");
            }
        }

        private ICollection<Warranty> _change;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warranty> Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged("Change");
            }
        }

        public object Clone()
        {
            return new SerialNumber
            {
                Id = Id,
                IdProduct = IdProduct,
                Value = Value,
                DateCreated = DateCreated
            };
        }
    }
}
