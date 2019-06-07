using System;
using System.ComponentModel;
using System.Windows;
using ModelModul;
using ModelModul.Counterparty;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace CounterpartyModul.ViewModels
{
    class AddCounterpartyViewModel: BindableBase, IInteractionRequestAware
    {
        #region Parametrs

        private TypeCounterparties _type;

        private TypeCounterparties Type
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);
                RaisePropertyChanged("WhoShowText");
            }
        }

        private CounterpartyViewModel _counterparty = new CounterpartyViewModel();

        public CounterpartyViewModel Counterparty
        {
            get => _counterparty;
            set => SetProperty(ref _counterparty, value);
        }

        public string WhoShowText => Type == TypeCounterparties.Suppliers ? "Поставщик" : "Покупатель";

        private Confirmation _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Confirmation);
                Type = (TypeCounterparties)_notification.Content;
                Counterparty.Counterparty = new Counterparties();
            }
        }
        public Action FinishInteraction { get; set; }

        public DelegateCommand AddCounterpartyCommand { get; }

        #endregion

        public AddCounterpartyViewModel()
        {
            AddCounterpartyCommand = new DelegateCommand(AddCounterparty).ObservesCanExecute(() => Counterparty.IsValidate);
            Counterparty.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args) {RaisePropertyChanged(args.PropertyName); };
        }

        public async void AddCounterparty()
        {
            try
            {
                Counterparty.TypeCounterparty = Type;
                DbSetCounterparties dbSetCounterparties = new DbSetCounterparties();
                await dbSetCounterparties.AddAsync(Counterparty.Counterparty);
                MessageBox.Show("Контрагент добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (_notification != null)
                    _notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
