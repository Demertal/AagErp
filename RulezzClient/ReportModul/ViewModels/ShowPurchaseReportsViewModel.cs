//using System;
//using System.Windows;
//using ModelModul;
//using ModelModul.PurchaseGoods;
//using Prism.Regions;

//namespace ReportModul.ViewModels
//{
//    class ShowPurchaseReportsViewModel : BufferRead<PurchaseReports>, INavigationAware
//    {
//        #region Properties

//        private readonly IRegionManager _regionManager;

//        #endregion

//        public ShowPurchaseReportsViewModel(IRegionManager regionManager)
//        {
//            _regionManager = regionManager;
//            Load();
//        }

//        protected sealed override void Load()
//        {
//            try
//            {
//                SqlMovementGoodsReportRepository dbSet = new SqlMovementGoodsReportRepository();
//                Count = dbSet.GetCount();
//                ReportsList = dbSet.Load(Left, Step);
//                RaisePropertyChanged("IsEnabledRightCommand");
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        public override void OnNavigatedFrom(NavigationContext navigationContext)
//        {
//            _regionManager.Regions.Remove("PurchaseReportInfo");
//        }
//    }
//}