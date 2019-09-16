using System;
using System.Collections.Generic;
using System.ComponentModel;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;

namespace CounterpartyModul.ViewModels
{
    public class CounterpartyViewModel : Counterparty, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, ICollection<string>>
            _validationErrors = new Dictionary<string, ICollection<string>>();

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

        private async void ValidateTitle()
        {
            const string propertyKey = "Title";
            IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();
            if (await counterpartyRepository.AnyAsync(new ExpressionSpecification<Counterparty>(w => w.Title == Title && w.WhoIsIt == WhoIsIt && w.Id != Id)))
            {
                _validationErrors[propertyKey] =
                    new List<string> { "Контрагент такого типа уже есть" };
                RaiseErrorsChanged(propertyKey);
            }
            else if (_validationErrors.ContainsKey(propertyKey))
            {
                _validationErrors.Remove(propertyKey);
                RaiseErrorsChanged(propertyKey);
            }
        }
    }
}
