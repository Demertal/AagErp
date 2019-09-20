using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.ViewModels
{
    public class WarrantyPeriodViewModel : WarrantyPeriod
    {
        #region Properties

        private CancellationTokenSource _cancelTokenSource;
        
        private string _period;
        public override string Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged("Period");
                ValidatePeriod();
            }
        }

        #endregion

        public WarrantyPeriodViewModel() { }

        public WarrantyPeriodViewModel(WarrantyPeriod obj)
        {
            Id = obj.Id;
            Period = obj.Period;
        }

        private async void ValidatePeriod()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            const string propertyKey = "Period";

            try
            {
                IRepository<WarrantyPeriod> warrantyPeriodRepository = new SqlWarrantyPeriodRepository();
                if (await warrantyPeriodRepository.AnyAsync(_cancelTokenSource.Token,
                    new ExpressionSpecification<WarrantyPeriod>(w => w.Period == Period && w.Id != Id)))
                {
                    ValidationErrors[propertyKey] =
                        new List<string> {"Такой период уже есть"};
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