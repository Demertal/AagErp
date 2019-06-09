using System;
using System.Windows;
using ModelModul;
using ModelModul.SalesGoods;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class ShowSalesReportsViewModel : BufferRead<SalesReports>, INavigationAware
    {
        #region Properties

        private readonly IRegionManager _regionManager;

        #endregion

        public ShowSalesReportsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Load();
        }

        protected sealed override async void Load()
        {
            try
            {
                DbSetSalesGoods dbSet = new DbSetSalesGoods();
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
            _regionManager.Regions.Remove("SalesReportInfo");
        }
    }
}