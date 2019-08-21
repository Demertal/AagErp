using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelModul.Models
{
    public class MovementGoods : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MovementGoods()
        {
            MovementGoodsInfos = new List<MovementGoodsInfo>();
            SerialNumberLogs = new List<SerialNumberLog>();
            MoneyTransfers = new List<MoneyTransfer>();
        }

        private Guid _id;
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private DateTime _dateCreate;
        public DateTime DateCreate
        {
            get => _dateCreate;
            set
            {
                _dateCreate = value;
                OnPropertyChanged("DateCreate");
            }
        }

        private decimal? _rate;
        [Column(TypeName = "money")]
        public decimal? Rate
        {
            get => _rate;
            set
            {
                _rate = value;
                OnPropertyChanged("Rate");
            }
        }

        private string _textInfo;
        [StringLength(50)]
        public string TextInfo
        {
            get => _textInfo;
            set
            {
                _textInfo = value;
                OnPropertyChanged("TextInfo");
            }
        }

        private int? _idArrivalStore;
        public int? IdArrivalStore
        {
            get => _idArrivalStore;
            set
            {
                _idArrivalStore = value;
                OnPropertyChanged("IdArrivalStore");
            }
        }

        private int? _idDisposalStore;
        public int? IdDisposalStore
        {
            get => _idDisposalStore;
            set
            {
                _idDisposalStore = value;
                OnPropertyChanged("IdDisposalStore");
            }
        }

        private int? _idCounterparty;
        public int? IdCounterparty
        {
            get => _idCounterparty;
            set
            {
                _idCounterparty = value;
                OnPropertyChanged("IdCounterparty");
            }
        }

        private int? _idCurrency;
        public int? IdCurrency
        {
            get => _idCurrency;
            set
            {
                _idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        private DateTime? _dateClose;
        public DateTime? DateClose
        {
            get => _dateClose;
            set
            {
                _dateClose = value;
                OnPropertyChanged("DateClose");
            }
        }

        private bool _isGoodsIssued;
        public bool IsGoodsIssued
        {
            get => _isGoodsIssued;
            set
            {
                _isGoodsIssued = value;
                OnPropertyChanged("IsGoodsIssued");
            }
        }

        private int _idType;
        public int IdType
        {
            get => _idType;
            set
            {
                _idType = value;
                OnPropertyChanged("IdType");
            }
        }

        private Counterparty _counterparty;
        public virtual Counterparty Counterparty
        {
            get => _counterparty;
            set
            {
                _counterparty = value;
                OnPropertyChanged("Counterparty");
            }
        }

        private Currency _currency;
        public virtual Currency Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                OnPropertyChanged("Currency");
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

        private Store _arrivalStore;
        public virtual Store ArrivalStore
        {
            get => _arrivalStore;
            set
            {
                _arrivalStore = value;
                OnPropertyChanged("ArrivalStore");
            }
        }

        private Store _disposalStore;
        public virtual Store DisposalStore
        {
            get => _disposalStore;
            set
            {
                _disposalStore = value;
                OnPropertyChanged("DisposalStore");
            }
        }

        private MovmentGoodType _movmentGoodType;
        public MovmentGoodType MovmentGoodType
        {
            get => _movmentGoodType;
            set
            {
                _movmentGoodType = value;
                OnPropertyChanged("MovmentGoodType");
            }
        }

        private ICollection<MovementGoodsInfo> _movementGoodsInfos;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovementGoodsInfo> MovementGoodsInfos
        {
            get => _movementGoodsInfos;
            set
            {
                _movementGoodsInfos = value;
                OnPropertyChanged("MovementGoodsInfos");
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
    }
}
