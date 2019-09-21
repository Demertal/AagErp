using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ModelModul.Models
{
    public class MovementGoods : ModelBase<MovementGoods>
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
                OnPropertyChanged();
            }
        }

        private DateTime? _dateCreate;
        public DateTime? DateCreate
        {
            get => _dateCreate;
            set
            {
                _dateCreate = value;
                OnPropertyChanged();
            }
        }

        private decimal? _rate;
        public decimal? Rate
        {
            get => _rate;
            set
            {
                _rate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private decimal? _equivalentRate;
        public decimal? EquivalentRate
        {
            get => _equivalentRate;
            set
            {
                _equivalentRate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private string _textInfo;
        public string TextInfo
        {
            get => _textInfo;
            set
            {
                _textInfo = value;
                OnPropertyChanged();
            }
        }

        private int? _idArrivalStore;
        public int? IdArrivalStore
        {
            get => _idArrivalStore;
            set
            {
                _idArrivalStore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int? _idDisposalStore;
        public int? IdDisposalStore
        {
            get => _idDisposalStore;
            set
            {
                _idDisposalStore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int? _idCounterparty;
        public int? IdCounterparty
        {
            get => _idCounterparty;
            set
            {
                _idCounterparty = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int? _idCurrency;
        public int? IdCurrency
        {
            get => _idCurrency;
            set
            {
                _idCurrency = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private int? _idEquivalentCurrency;
        public int? IdEquivalentCurrency
        {
            get => _idEquivalentCurrency;
            set
            {
                _idEquivalentCurrency = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private DateTime? _dateClose;
        public DateTime? DateClose
        {
            get => _dateClose;
            set
            {
                _dateClose = value;
                OnPropertyChanged();
            }
        }

        private bool _isGoodsIssued;
        public bool IsGoodsIssued
        {
            get => _isGoodsIssued;
            set
            {
                _isGoodsIssued = value;
                OnPropertyChanged();
            }
        }

        private int _idType;
        public int IdType
        {
            get => _idType;
            set
            {
                _idType = value;
                OnPropertyChanged();
            }
        }

        private Guid? _idMovementGood;
        public Guid? IdMovementGood
        {
            get => _idMovementGood;
            set
            {
                _idMovementGood = value;
                OnPropertyChanged();
            }
        }

        private Counterparty _counterparty;
        public virtual Counterparty Counterparty
        {
            get => _counterparty;
            set
            {
                _counterparty = value;
                OnPropertyChanged();
            }
        }

        private Currency _equivalentcurrency;
        public virtual Currency EquivalentCurrency
        {
            get => _equivalentcurrency;
            set
            {
                _equivalentcurrency = value;
                OnPropertyChanged();
            }
        }

        private Currency _currency;
        public virtual Currency Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                OnPropertyChanged();
            }
        }

        private Store _arrivalStore;
        public virtual Store ArrivalStore
        {
            get => _arrivalStore;
            set
            {
                _arrivalStore = value;
                OnPropertyChanged();
            }
        }

        private Store _disposalStore;
        public virtual Store DisposalStore
        {
            get => _disposalStore;
            set
            {
                _disposalStore = value;
                OnPropertyChanged();
            }
        }

        private MovmentGoodType _movmentGoodType;
        public MovmentGoodType MovmentGoodType
        {
            get => _movmentGoodType;
            set
            {
                _movmentGoodType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private MovementGoods _movementGood;
        public MovementGoods MovementGood
        {
            get => _movementGood;
            set
            {
                _movementGood = value;
                OnPropertyChanged();
            }
        }

        private ICollection<MovementGoodsInfo> _movementGoodsInfosCollection;
        public virtual ICollection<MovementGoodsInfo> MovementGoodsInfosCollection
        {
            get => _movementGoodsInfosCollection;
            set
            {
                if (_movementGoodsInfosCollection is ObservableCollection<MovementGoodsInfo> infos)
                    infos.CollectionChanged -= OnMovementGoodsInfosCollectionChanged;
                _movementGoodsInfosCollection = value;
                if (_movementGoodsInfosCollection is ObservableCollection<MovementGoodsInfo> collection)
                    collection.CollectionChanged += OnMovementGoodsInfosCollectionChanged;
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

        private ICollection<MovementGoods> _movementGoodsCollection;
        public ICollection<MovementGoods> MovementGoodsCollection
        {
            get => _movementGoodsCollection;
            set
            {
                _movementGoodsCollection = value;
                OnPropertyChanged();
            }
        }

        private ICollection<MoneyTransfer> _moneyTransfersCollection;
        public virtual ICollection<MoneyTransfer> MoneyTransfersCollection
        {
            get => _moneyTransfersCollection;
            set
            {
                _moneyTransfersCollection = value;
                OnPropertyChanged();
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
                                    if (Rate == null || Rate <= 0)
                                    {
                                        error = "Курс не может быть отрицательным";
                                    }

                                    break;
                                case "EquivalentRate":
                                    if (EquivalentRate == null || Rate <= 0)
                                    {
                                        error = "Курс не может быть отрицательным";
                                    }

                                    break;
                                case "IdArrivalStore":
                                    if (IdArrivalStore == null || IdArrivalStore <= 0)
                                    {
                                        error = "Требуется указать склад";
                                    }

                                    break;
                                case "IdCounterparty":
                                    if (IdCounterparty == null || IdCounterparty <= 0)
                                    {
                                        error = "Требуется указать поставщика";
                                    }

                                    break;
                                case "IdCurrency":
                                    if (IdCurrency == null || IdCurrency <= 0)
                                    {
                                        error = "Требуется указать валюту закупки";
                                    }

                                    break;
                                case "IdEquivalentCurrency":
                                    if (IdEquivalentCurrency == null || IdEquivalentCurrency <= 0)
                                    {
                                        error = "Требуется указать валюту эквивалента";
                                    }

                                    break;
                            }

                            break;
                        case "sale":
                            switch (columnName)
                            {
                                case "IdDisposalStore":
                                    if (IdDisposalStore == null || IdDisposalStore <= 0)
                                    {
                                        error = "Требуется указать склад";
                                    }

                                    break;
                            }

                            break;
                        case "moving":
                            switch (columnName)
                            {
                                case "IdArrivalStore":
                                    if (IdArrivalStore == null || IdArrivalStore <= 0)
                                    {
                                        error = "Требуется указать склад";
                                    }
                                    break;
                                case "IdDisposalStore":
                                    if (IdDisposalStore == null || IdDisposalStore <= 0)
                                    {
                                        error = "Требуется указать склад";
                                    }

                                    break;
                            }

                            break;
                    }

                return error;
            }
        }

        public override object Clone()
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

        #region PropertyChanged

        private void OnMovementGoodsInfosCollectionChanged(object sender, NotifyCollectionChangedEventArgs ea)
        {
            switch (ea.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (MovementGoodsInfo item in ea.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= MovementGoodsInfoOnPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (MovementGoodsInfo item in ea.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += MovementGoodsInfoOnPropertyChanged;
                    }
                    break;
            }
            OnPropertyChanged(nameof(IsValid));
        }

        private void MovementGoodsInfoOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnMovementGoodsInfoPropertyChanged(sender, e);
            OnPropertyChanged(nameof(MovementGoodsInfosCollection));
            OnPropertyChanged(nameof(IsValid));
        }

        protected virtual void OnMovementGoodsInfoPropertyChanged(object sender, PropertyChangedEventArgs e) { }

        #endregion
        
        public override bool IsValid
        {
            get
            {
                if (HasErrors || MovmentGoodType == null || MovementGoodsInfosCollection == null ||
                    MovementGoodsInfosCollection.Count == 0 ||
                    MovementGoodsInfosCollection.Any(mi => !mi.IsValid)) return false;
                if (MovmentGoodType.Code == "purchase")
                {
                    return Rate != null && Rate > 0 && EquivalentRate != null && EquivalentRate > 0 &&
                           IdArrivalStore != null && IdArrivalStore > 0 && IdCurrency != null && IdCurrency > 0 &&
                           IdEquivalentCurrency != null && IdEquivalentCurrency > 0 && IdCounterparty != null &&
                           IdCounterparty > 0;
                }
                if (MovmentGoodType.Code == "sale")
                {
                    return IdDisposalStore != null && IdDisposalStore > 0;
                }
                if (MovmentGoodType.Code == "moving")
                {
                    return IdArrivalStore != null && IdArrivalStore > 0 && IdDisposalStore != null && IdDisposalStore > 0 && IdArrivalStore != IdDisposalStore;
                }

                return true;
            }
        }
    }
}