using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelModul.Models
{
    public class Currency : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Currency()
        {
            MovementGoods = new List<MovementGoods>();
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
        [StringLength(10)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private decimal _cost;

        [Column(TypeName = "money")]
        public decimal Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                OnPropertyChanged("Cost");
            }
        }

        private bool _isDefault;

        public bool IsDefault
        {
            get => _isDefault;
            set
            {
                _isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        private ICollection<MovementGoods> _movementGoods;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<MovementGoods> MovementGoods
        {
            get => _movementGoods;
            set
            {
                _movementGoods = value;
                OnPropertyChanged("MovementGoods");
            }
        }
    }
}
