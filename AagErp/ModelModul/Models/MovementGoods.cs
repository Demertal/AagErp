using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelModul.Models
{
    public class MovementGoods : ModelBase, ICloneable
    {
        public MovementGoods()
        {
            MovementGoodsInfosCollection = new List<MovementGoodsInfo>();
            SerialNumberLogsCollection = new List<SerialNumberLog>();
            MoneyTransfersCollection = new List<MoneyTransfer>();
            MovementGoodsCollection = new List<MovementGoods>();
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

        private DateTime? _dateCreate;
        public DateTime? DateCreate
        {
            get => _dateCreate;
            set
            {
                _dateCreate = value;
                OnPropertyChanged("DateCreate");
            }
        }

        private decimal? _rate;
        public decimal? Rate
        {
            get => _rate;
            set
            {
                _rate = value;
                OnPropertyChanged("Rate");
            }
        }

        private decimal? _equivalentRate;
        public decimal? EquivalentRate
        {
            get => _equivalentRate;
            set
            {
                _equivalentRate = value;
                OnPropertyChanged("EquivalentRate");
            }
        }

        private string _textInfo;
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

        private int? _idEquivalentCurrency;
        public int? IdEquivalentCurrency
        {
            get => _idEquivalentCurrency;
            set
            {
                _idEquivalentCurrency = value;
                OnPropertyChanged("IdEquivalentCurrency");
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

        private Guid? _idMovementGood;
        public Guid? IdMovementGood
        {
            get => _idMovementGood;
            set
            {
                _idMovementGood = value;
                OnPropertyChanged("IdMovementGood");
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

        private Currency _equivalentcurrency;
        public virtual Currency EquivalentCurrency
        {
            get => _equivalentcurrency;
            set
            {
                _equivalentcurrency = value;
                OnPropertyChanged("EquivalentCurrency");
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

        private MovementGoods _movementGood;
        public MovementGoods MovementGood
        {
            get => _movementGood;
            set
            {
                _movementGood = value;
                OnPropertyChanged("MovementGood");
            }
        }

        private ICollection<MovementGoodsInfo> _movementGoodsInfosCollection;
        public virtual ICollection<MovementGoodsInfo> MovementGoodsInfosCollection
        {
            get => _movementGoodsInfosCollection;
            set
            {
                _movementGoodsInfosCollection = value;
                OnPropertyChanged("MovementGoodsInfosCollection");
            }
        }

        private ICollection<SerialNumberLog> _serialNumberLogsCollection;
        public virtual ICollection<SerialNumberLog> SerialNumberLogsCollection
        {
            get => _serialNumberLogsCollection;
            set
            {
                _serialNumberLogsCollection = value;
                OnPropertyChanged("SerialNumberLogsCollection");
            }
        }

        private ICollection<MovementGoods> _movementGoodsCollection;
        public ICollection<MovementGoods> MovementGoodsCollection
        {
            get => _movementGoodsCollection;
            set
            {
                _movementGoodsCollection = value;
                OnPropertyChanged("MovementGoods");
            }
        }

        private ICollection<MoneyTransfer> _moneyTransfersCollection;
        public virtual ICollection<MoneyTransfer> MoneyTransfersCollection
        {
            get => _moneyTransfersCollection;
            set
            {
                _moneyTransfersCollection = value;
                OnPropertyChanged("MoneyTransfersCollection");
            }
        }

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if(MovmentGoodType == null)
                    error = "Нужно указать тип движения товара";
                else 
                    switch (MovmentGoodType.Code)
                    {
                        case "purchase" :
                            switch (columnName)
                            {
                                case "Rate":
                                    if (Rate == null || Rate < 0)
                                    {
                                        error = "Курс не может быть отрицательным";
                                    }

                                    break;
                                case "EquivalentRate":
                                    if (EquivalentRate == null || Rate < 0)
                                    {
                                        error = "Курс не может быть отрицательным";
                                    }

                                    break;
                                case "IdArrivalStore":
                                    if (IdArrivalStore == null || IdArrivalStore < 0)
                                    {
                                        error = "Требуется указать склад";
                                    }

                                    break;
                                case "IdCounterparty":
                                    if (IdCounterparty == null || IdCounterparty < 0)
                                    {
                                        error = "Требуется указать поставщика";
                                    }

                                    break;
                                case "IdCurrency":
                                    if (IdCurrency == null || IdCurrency < 0)
                                    {
                                        error = "Требуется указать валюту закупки";
                                    }

                                    break;
                                case "IdEquivalentCurrency":
                                    if (IdEquivalentCurrency == null || IdEquivalentCurrency < 0)
                                    {
                                        error = "Требуется указать валюту эквивалента";
                                    }

                                    break;
                            }
                        break;
                    }

                Error = error;
                return error;
            }
        }

        public object Clone()
        {
            return new MovementGoods
            {
                Id = Id,
                IdArrivalStore = IdArrivalStore,
                IdDisposalStore = IdDisposalStore,
                IdCounterparty = IdCounterparty,
                IdCurrency = IdCurrency,
                IdEquivalentCurrency = IdEquivalentCurrency,
                IdType = IdType,
                DateClose = DateClose,
                DateCreate = DateCreate,
                EquivalentRate = EquivalentRate,
                IsGoodsIssued = IsGoodsIssued,
                Rate = Rate,
                TextInfo = TextInfo,
                MovementGoodsInfosCollection =
                    new List<MovementGoodsInfo>(MovementGoodsInfosCollection.Select(m => (MovementGoodsInfo) m.Clone()))
            };
        }
    }
}
