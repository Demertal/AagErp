using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class PropertyValue : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyValue()
        {
            PropertyProducts = new HashSet<PropertyProduct>();
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

        private string _value;
        [Required]
        [StringLength(50)]
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        private int _idPropertyName;
        public int IdPropertyName
        {
            get => _idPropertyName;
            set
            {
                _idPropertyName = value;
                OnPropertyChanged("IdPropertyName");
            }
        }

        private PropertyName _propertyName;
        public virtual PropertyName PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                OnPropertyChanged("PropertyName");
            }
        }

        private ICollection<PropertyProduct> _propertyProducts;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertyProduct> PropertyProducts
        {
            get => _propertyProducts;
            set
            {
                _propertyProducts = value;
                OnPropertyChanged("PropertyProducts");
            }
        }
    }
}
