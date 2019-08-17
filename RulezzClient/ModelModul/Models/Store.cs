using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class Store : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Store()
        {
            ArrivalMovementGoodsReports = new List<MovementGoods>();
            DisposalMovementGoodsReports = new List<MovementGoods>();
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
        [StringLength(50)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private ICollection<MovementGoods> _arrivalMovementGoodsReports;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementGoods> ArrivalMovementGoodsReports
        {
            get => _arrivalMovementGoodsReports;
            set
            {
                _arrivalMovementGoodsReports = value;
                OnPropertyChanged("ArrivalMovementGoodsReports");
            }
        }

        private ICollection<MovementGoods> _disposalMovementGoodsReports;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementGoods> DisposalMovementGoodsReports
        {
            get => _disposalMovementGoodsReports;
            set
            {
                _disposalMovementGoodsReports = value;
                OnPropertyChanged("DisposalMovementGoodsReports");
            }
        }
    }
}
