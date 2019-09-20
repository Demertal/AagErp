using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.ViewModels
{
    public class PriceGroupViewModel : PriceGroup
    {
        #region Properties

        private CancellationTokenSource _cancelTokenSource;

        private decimal _markup;
        public override decimal Markup
        {
            get => _markup;
            set
            {
                _markup = value;
                OnPropertyChanged("Markup");
                ValidateMarkup();
            }
        }

        #endregion

        public PriceGroupViewModel() { }

        public PriceGroupViewModel(PriceGroup obj)
        {
            Id = obj.Id;
            Markup = obj.Markup;
        }

        private async void ValidateMarkup()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            const string propertyKey = "Markup";

            try
            {
                IRepository<PriceGroup> priceGroupRepository = new SqlPriceGroupRepository();
                if (await priceGroupRepository.AnyAsync(_cancelTokenSource.Token,
                    new ExpressionSpecification<PriceGroup>(w => w.Markup == Markup && w.Id != Id)))
                {
                    ValidationErrors[propertyKey] =
                        new List<string> { "Такая наценка уже есть" };
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
