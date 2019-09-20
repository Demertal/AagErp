using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.ViewModels
{
    public class CounterpartyViewModel : Counterparty
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

        private ETypeCounterparties _whoIsIt;
        public override ETypeCounterparties WhoIsIt
        {
            get => _whoIsIt;
            set
            {
                _whoIsIt = value;
                OnPropertyChanged("WhoIsIt");
                ValidateTitle();
            }
        }

        #endregion

        public CounterpartyViewModel() { }

        public CounterpartyViewModel(Counterparty obj)
        {
            Id = obj.Id;
            Title = obj.Title;
            Address = obj.Address;
            ContactPerson = obj.ContactPerson;
            ContactPhone = obj.ContactPhone;
            IdPaymentType = obj.IdPaymentType;
            Props = obj.Props;
            WhoIsIt = obj.WhoIsIt;
        }

        private async void ValidateTitle()
        {
            _cancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            _cancelTokenSource = newCts;

            const string propertyKey = "Title";

            try
            {
                IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();
                if (await counterpartyRepository.AnyAsync(_cancelTokenSource.Token,
                    new ExpressionSpecification<Counterparty>(w => w.Title == Title && w.WhoIsIt == WhoIsIt && w.Id != Id))
                )
                {
                    ValidationErrors[propertyKey] =
                        new List<string> { "Контрагент такого типа уже есть" };
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
