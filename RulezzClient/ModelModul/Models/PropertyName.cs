using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class PropertyName : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyName()
        {
            PropertyProducts = new List<PropertyProduct>();
            PropertyValues = new List<PropertyValue>();
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

        private string _title;
        [Required]
        [StringLength(20)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private int? _idCategory;
        public int? IdCategory
        {
            get => _idCategory;
            set
            {
                _idCategory = value;
                OnPropertyChanged("IdCategory");
            }
        }

        private Category _category;
        public virtual Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged("Category");
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

        private ICollection<PropertyValue> _propertyValues;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertyValue> PropertyValues
        {
            get => _propertyValues;
            set
            {
                _propertyValues = value;
                OnPropertyChanged("PropertyValues");
            }
        }
    }
}
