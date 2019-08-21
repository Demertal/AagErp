using System;
using System.Collections.ObjectModel;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace CounterpartyModul.ViewModels
{
    public class AddCounterpartyViewModel: DialogViewModelBase
    {
        #region Parametrs

        private ObservableCollection<PaymentType> _paymentTypesList = new ObservableCollection<PaymentType>();
        public ObservableCollection<PaymentType> PaymentTypesList
        {
            get => _paymentTypesList;
            set => SetProperty(ref _paymentTypesList, value);
        }

        private Counterparty _counterparty = new Counterparty();

        public Counterparty Counterparty => _counterparty;

        public string WhoShowText => Counterparty?.WhoIsIt == TypeCounterparties.Suppliers ? "Поставщик" : "Покупатель";

        public Action FinishInteraction { get; set; }

        public DelegateCommand AddCounterpartyCommand { get; }

        #endregion

        public AddCounterpartyViewModel()
        {
            Counterparty.PropertyChanged += (o, e) => RaisePropertyChanged("Counterparty");
            AddCounterpartyCommand = new DelegateCommand(AddCounterpartyAsync).ObservesCanExecute(() => Counterparty.IsValidate);
        }

        public async void AddCounterpartyAsync()
        {
            try
            {
                IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();
                await counterpartyRepository.CreateAsync(Counterparty);
                MessageBox.Show("Контрагент добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void LoadAsync()
        {
            IRepository<PaymentType> paymentTypeRepository = new SqlPaymentTypeRepository();
            PaymentTypesList = new ObservableCollection<PaymentType>(await paymentTypeRepository.GetListAsync());
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                LoadAsync();
                Counterparty.WhoIsIt = parameters.GetValue<TypeCounterparties>("type");
                RaisePropertyChanged("WhoShowText");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                RaiseRequestClose(new DialogResult(ButtonResult.Abort));
            }
        }
    }
}
