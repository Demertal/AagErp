using System;
using System.Windows;
using ModelModul;
using ModelModul.PurchaseGoods;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class ShowPurchaseReportsViewModel : BufferRead<PurchaseReports>, INavigationAware
    {
        #region Properties

        private readonly IRegionManager _regionManager;

        #endregion

        public ShowPurchaseReportsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Load();
        }

        protected sealed override async void Load()
        {
            try
            {
                DbSetPurchaseGoods dbSet = new DbSetPurchaseGoods();
                Count = await dbSet.GetCount();
                ReportsList = await dbSet.LoadAsync(Left, Step);
                RaisePropertyChanged("IsEnabledRightCommand");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove("PurchaseReportInfo");
        }
    }
}