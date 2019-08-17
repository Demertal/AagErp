using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;

namespace CounterpartyModul.ViewModels
{
    public class ShowCounterpartiesViewModel: ViewModelBase
    {
        #region Properties

        private readonly IRegionManager _regionManager;

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

        //private ObservableCollection<CounterpartyViewModel> _counterpartiesList = new ObservableCollection<CounterpartyViewModel>();
        //public ObservableCollection<CounterpartyViewModel> CounterpartiesList
        //{
        //    get => _counterpartiesList;
        //    set => SetProperty(ref _counterpartiesList, value);
        //}

        public string WhoShowText => Type == TypeCounterparties.Suppliers ? "Поставщики" : "Покупатели";

        public InteractionRequest<INotification> AddCounterpartyPopupRequest { get; set; }

        //public DelegateCommand<CounterpartyViewModel> DeleteCounterpartyCommand { get; }
        public DelegateCommand AddCounterpartyCommand { get; }
        #endregion

        public ShowCounterpartiesViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            AddCounterpartyPopupRequest = new InteractionRequest<INotification>();
            //DeleteCounterpartyCommand = new DelegateCommand<CounterpartyViewModel>(DeleteSuppliersAsync);
            AddCounterpartyCommand = new DelegateCommand(AddCounterparty);
        }

        private void AddCounterparty()
        {
            AddCounterpartyPopupRequest.Raise(new Confirmation { Title = "Добавить контрагента", Content = Type }, Callback);
        }

        private void Callback(INotification obj)
        {
            if (!((Confirmation)obj).Confirmed) return;
            LoadAsync();
        }

        //private async void DeleteSuppliersAsync(CounterpartyViewModel obj)
        //{
        //    if (obj == null) return;
        //    if (MessageBox.Show("Удалить контрагента?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
        //        MessageBoxResult.Yes) return;
        //    try
        //    {
        //        SqlCounterpartyRepository dbSet = new SqlCounterpartyRepository();
        //        await dbSet.DeleteAsync(obj.Counterparty);
        //        LoadAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private async void LoadAsync()
        {
            try
            {
                SqlCounterpartyRepository dbSet = new SqlCounterpartyRepository();
                //CounterpartiesList = new ObservableCollection<CounterpartyViewModel>((await dbSet.GetListAsync(CounterpartySpecification.GetCounterpartiesByType(_type)))
                //    .Select(obj => new CounterpartyViewModel {Counterparty = obj}));
                RaisePropertyChanged("CounterpartiesList");
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
