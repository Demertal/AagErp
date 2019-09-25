using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications.BasisSpecifications;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ReportModul.ViewModels
{
    class ShowProfitStatementViewModel : ViewModelBase
    {
        #region Property

        private DateTime _from;
        public DateTime From
        {
            get => _from;
            set
            {
                SetProperty(ref _from, value);
                LoadAsync();
            }
        }

        private DateTime _to;
        public DateTime To
        {
            get => _to;
            set
            {
                SetProperty(ref _to, value);
                LoadAsync();
            }
        }

        private ObservableCollection<ProfitStatement> _reportList;
        public ObservableCollection<ProfitStatement> ReportList
        {
            get => _reportList;
            set => SetProperty(ref _reportList, value);
        }

        private Currency _currency;
        public Currency Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        #endregion

        public ShowProfitStatementViewModel(IDialogService dialogService) : base(dialogService)
        {
            From = DateTime.Today;
            To = DateTime.Today;
        }

        private async void LoadAsync()
        {
            try
            {
                SqlProfitStatementRepository profitStatementRepository = new SqlProfitStatementRepository();
                ReportList = new ObservableCollection<ProfitStatement>(await profitStatementRepository.GetProfitStatement(From, To));
                IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
                Currency = await currencyRepository.GetItemAsync(
                    where: new ExpressionSpecification<Currency>(c => c.IsDefault));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadAsync();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
