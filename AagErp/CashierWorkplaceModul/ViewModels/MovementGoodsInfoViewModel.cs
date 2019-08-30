using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ModelModul.Models;
using ModelModul.Repositories;

namespace CashierWorkplaceModul.ViewModels
{
    public class MovementGoodsInfoViewModel : MovementGoodsInfo, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

        private decimal _count;
        public override decimal Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
                ValidateCount();
            }
        }

        #region INotifyDataErrorInfo members

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors => _validationErrors.Count > 0;

        #endregion

        private async void ValidateCount()
        {
            const string propertyKey = "Count";
            SqlProductRepository productRepository = new SqlProductRepository();
            if(IdProduct != 0 && MovementGoods != null)
            {
                decimal count = (await productRepository.GetCountsProduct(IdProduct)).First(c => c.StoreId == MovementGoods.IdDisposalStore).Count;
                if (Count > count)
                {
                    _validationErrors[propertyKey] =
                        new List<string> {"Кол-во товара указано больше чем его есть на складе"};
                    RaiseErrorsChanged(propertyKey);
                }
                else if (_validationErrors.ContainsKey(propertyKey))
                {
                    _validationErrors.Remove(propertyKey);
                    RaiseErrorsChanged(propertyKey);
                }
            }
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                _validationErrors.Remove(propertyKey);
                RaiseErrorsChanged(propertyKey);
            }
        }
    }
}
