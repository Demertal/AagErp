using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.ViewModels
{
    public class StoreViewModel : Store
    {
        #region Properties

        private CancellationTokenSource _cancelTokenSource;

        private string _title;
        public override string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
                ValidateTitle();
            }
        }

        #endregion

        public StoreViewModel() { }

        public StoreViewModel(Store obj)
        {
            Id = obj.Id;
            Title = obj.Title;
        }

        private async void ValidateTitle()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            const string propertyKey = "Title";

            try
            {
                IRepository<Store> storeRepository = new SqlStoreRepository();
                if (await storeRepository.AnyAsync(_cancelTokenSource.Token,
                    new ExpressionSpecification<Store>(w => w.Title == Title && w.Id != Id))) 
                {
                    ValidationErrors[propertyKey] =
                        new List<string> { "Такой склад уже есть" };
                }
                else if (ValidationErrors.ContainsKey(propertyKey))
                {
                    ValidationErrors.Remove(propertyKey);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RaiseErrorsChanged(propertyKey);
            OnPropertyChanged("IsValid");

            if (_cancelTokenSource == newCts)
                _cancelTokenSource = null;
        }
    }
}
