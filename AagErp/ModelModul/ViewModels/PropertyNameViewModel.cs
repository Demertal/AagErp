using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;

namespace ModelModul.ViewModels
{
    public class PropertyNameViewModel : PropertyName
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

        private int? _idCategory;
        public override int? IdCategory
        {
            get => _idCategory;
            set
            {
                _idCategory = value;
                OnPropertyChanged("IdCategory");
                ValidateTitle();
            }
        }

        #endregion

        public PropertyNameViewModel() { }

        public PropertyNameViewModel(PropertyName obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            IdCategory = obj.IdCategory;
            Category = (Category)obj.Category?.Clone();
        }

        private async void ValidateTitle()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            const string propertyKey = "Title";

            try
            {
                SqlPropertyNameRepository propertyNameRepository = new SqlPropertyNameRepository();
                if (!await propertyNameRepository.CheckProperty(Id, IdCategory, Title, _cancelTokenSource.Token))
                {
                    ValidationErrors[propertyKey] =
                        new List<string> { "Параметр с таким названием уже есть в этой категории" };
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
