using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;

namespace ModelModul.ViewModels
{
    public class TransportationMovementGoodsInfoViewModel : MovementGoodsInfo
    {
        #region Properties

        private CancellationTokenSource _cancelTokenSource;

        private decimal _count;
        public override decimal Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
                OnCountChanged();
                ValidateCount();
            }
        }

        #endregion

        private void OnCountChanged()
        {
            if (Product == null || !Product.KeepTrackSerialNumbers) return;
            int count = (int)Count - Product.SerialNumbersCollection.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Product.SerialNumbersCollection.Add(new SerialNumberViewModel{Product = Product, IdProduct = Product.Id, IdStore = MovementGoods.IdDisposalStore});
                }
            }
            else if (count < 0)
            {
                for (int i = 0; i > count; i--)
                {
                    var temp = Product.SerialNumbersCollection.FirstOrDefault(s =>
                        string.IsNullOrEmpty(s.Value));
                    Product.SerialNumbersCollection.Remove(temp ?? Product.SerialNumbersCollection.Last());
                }
            }
        }

        public async void ValidateCount()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            const string propertyKey = "Count";

            try
            {
                SqlProductRepository productRepository = new SqlProductRepository();
                if(IdProduct != 0 && MovementGoods != null)
                {
                    decimal count = (await productRepository.GetCountsProduct(IdProduct, _cancelTokenSource.Token)).First(c => c.StoreId == MovementGoods.IdDisposalStore).Count;
                    if (Count > count)
                    {
                       ValidationErrors[propertyKey] =
                            new List<string> {"Кол-во товара указано больше чем его есть на складе"};
                        RaiseErrorsChanged(propertyKey);
                    }
                    else if (ValidationErrors.ContainsKey(propertyKey))
                    {
                        ValidationErrors.Remove(propertyKey);
                        RaiseErrorsChanged(propertyKey);
                    }
                }
                else if (ValidationErrors.ContainsKey(propertyKey))
                {
                    ValidationErrors.Remove(propertyKey);
                    RaiseErrorsChanged(propertyKey);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RaiseErrorsChanged(propertyKey);
            OnPropertyChanged(nameof(IsValid));

            if (_cancelTokenSource == newCts)
                _cancelTokenSource = null;
        }
    }
}
