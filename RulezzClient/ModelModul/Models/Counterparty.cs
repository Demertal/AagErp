using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelModul.Models
{
    public class Counterparty : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Counterparty()
        {
            MoneyTransfers = new List<MoneyTransfer>();
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
        [StringLength(40)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _contactPerson;
        [StringLength(40)]
        public string ContactPerson
        {
            get => _contactPerson;
            set
            {
                _contactPerson = value;
                OnPropertyChanged("ContactPerson");
            }
        }

        private string _contactPhone;
        [StringLength(50)]
        public string ContactPhone
        {
            get => _contactPhone;
            set
            {
                _contactPhone = value;
                OnPropertyChanged("ContactPhone");
            }
        }

        private string _props;
        [StringLength(40)]
        public string Props
        {
            get => _props;
            set
            {
                _props = value;
                OnPropertyChanged("Props");
            }
        }

        private string _address;
        [StringLength(40)]
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private TypeCounterparties _whoIsIt;
        public TypeCounterparties WhoIsIt
        {
            get => _whoIsIt;
            set
            {
                _whoIsIt = value;
                OnPropertyChanged("WhoIsIt");
            }
        }

        private int? _idPaymentType;
        public int? IdPaymentType
        {
            get => _idPaymentType;
            set
            {
                _idPaymentType = value;
                OnPropertyChanged("IdPaymentType");
            }
        }

        private PaymentType _paymentType;
        public virtual PaymentType PaymentType
        {
            get => _paymentType;
            set
            {
                _paymentType = value;
                OnPropertyChanged("PaymentType");
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

        private ICollection<MovementGoods> _movementGoods;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementGoods> MovementGoods
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
