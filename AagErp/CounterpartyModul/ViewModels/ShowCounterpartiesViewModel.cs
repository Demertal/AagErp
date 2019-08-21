using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace CounterpartyModul.ViewModels
{
    public class ShowCounterpartiesViewModel: ViewModelBase
    {
        #region Properties

        private readonly IRegionManager _regionManager;

        private readonly IDialogService _dialogService;

        private TypeCounterparties _type;

        private TypeCounterparties Type
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);
                RaisePropertyChanged("WhoShowText");
                LoadAsync();
            }
        }

        private ObservableCollection<Counterparty> _counterpartiesList = new ObservableCollection<Counterparty>();
        public ObservableCollection<Counterparty> CounterpartiesList
        {
            get => _counterpartiesList;
            set
            {
                if (_counterpartiesList != null)
                    _counterpartiesList.CollectionChanged -= CounterpartiesListOnCollectionChanged;
                SetProperty(ref _counterpartiesList, value);
                if (_counterpartiesList != null)
                    _counterpartiesList.CollectionChanged += CounterpartiesListOnCollectionChanged;
            }
        }


        #region CollectionChanged

        private void CounterpartiesListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (Counterparty item in e.OldItems)
                    {
                        //Removed items
                        item.PropertyChanged -= CountsProductItemChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (Counterparty item in e.NewItems)
                    {
                        //Added items
                        item.PropertyChanged += CountsProductItemChanged;
                    }

                    break;
            }

            RaisePropertyChanged("CounterpartiesList");
        }

        private void CountsProductItemChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("CounterpartiesList");
        }

        #endregion

        public string WhoShowText => Type == TypeCounterparties.Suppliers ? "Поставщики" : "Покупатели";

        public DelegateCommand<Counterparty> DeleteCounterpartyCommand { get; }
        public DelegateCommand AddCounterpartyCommand { get; }
        #endregion

        public ShowCounterpartiesViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _dialogService = dialogService;
            DeleteCounterpartyCommand = new DelegateCommand<Counterparty>(DeletCounterpartyAsync);
            AddCounterpartyCommand = new DelegateCommand(AddCounterparty);
        }

        private void AddCounterparty()
        {
            _dialogService.ShowDialog("AddCounterparty", new DialogParameters { { "type", Type } }, result => LoadAsync());
        }

        private async void DeletCounterpartyAsync(Counterparty obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить контрагента?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();
                await counterpartyRepository.DeleteAsync(obj);
                LoadAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadAsync()
        {
            try
            {
                IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();
                CounterpartiesList = new ObservableCollection<Counterparty>(
                    await counterpartyRepository.GetListAsync(
                        CounterpartySpecification.GetCounterpartiesByType(_type)));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Type = (TypeCounterparties)navigationContext.Parameters["type"];
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove("CounterpartyInfo");
        }

        #endregion
    }
}
