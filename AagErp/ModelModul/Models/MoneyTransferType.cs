using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class MoneyTransferType : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MoneyTransferType()
        {
            MoneyTransfers = new List<MoneyTransfer>();
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

        private string _code;
        [Required]
        [StringLength(20)]
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        private string _description;
        [Required]
        [StringLength(50)]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private ICollection<MoneyTransfer> _moneyTransfers;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MoneyTransfer> MoneyTransfers
        {
            get => _moneyTransfers;
            set
            {
                _moneyTransfers = value;
                OnPropertyChanged("MoneyTransfers");
            }
        }
    }
}
