using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;

namespace ModelModul.ViewModels
{
    public class SerialNumberViewModel : SerialNumber
    {
        #region Properties

        private CancellationTokenSource _cancelTokenSource;

        private int? _idStore;
        public int? IdStore
        {
            get => _idStore;
            set
            {
                _idStore = value;
                ValidateValue();
            }
        }

        private string _value;
        public override string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
                ValidateValue();
            }
        }

        #endregion

        private async void ValidateValue()
        {
            //_cancelTokenSource?.Cancel();
            //CancellationTokenSource newCts = new CancellationTokenSource();
            //_cancelTokenSource = newCts;

            const string propertyKey = "Value";

            try
            {

                SqlSerialNumberRepository serialNumberRepository = new SqlSerialNumberRepository();
                if(IdStore != null)
                {
                    int freeSerialNumbers =
                    (await serialNumberRepository.GetFreeSerialNumbers(IdProduct, Value, IdStore.Value)).Count;
                    if (freeSerialNumbers != 0 && Product != null)
                    {
                        if (freeSerialNumbers - Product.SerialNumbersCollection.Count(s => s.Value == Value) < 0)
                        {
                            ValidationErrors[propertyKey] = new List<string> {"Нет доступных серийных номеров"};
                        }
                        else if (ValidationErrors.ContainsKey(propertyKey))
                        {
                            ValidationErrors.Remove(propertyKey);
                        }
                    }
                    else if (freeSerialNumbers == 0)
                    {
                        ValidationErrors[propertyKey] = new List<string> {"Серийный номер не найден"};
                    }
                    else if (ValidationErrors.ContainsKey(propertyKey))
                    {
                        ValidationErrors.Remove(propertyKey);
                    }
                }
                else if (ValidationErrors.ContainsKey(propertyKey))
                {
                    ValidationErrors.Remove(propertyKey);
                }
            }
            //catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RaiseErrorsChanged(propertyKey);
            OnPropertyChanged(nameof(IsValid));

            //if (_cancelTokenSource == newCts)
            //    _cancelTokenSource = null;
        }
    }
}
