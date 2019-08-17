//using System;
//using System.Windows;
//using ModelModul;
//using Prism.Regions;

//namespace ReportModul.ViewModels
//{
//    class ShowSalesReportsViewModel : BufferRead<Purchase>
//    {
//        #region Properties

//        private readonly IRegionManager _regionManager;

//        #endregion

//        public ShowSalesReportsViewModel(IRegionManager regionManager)
//        {
//            _regionManager = regionManager;
//        }

//        protected sealed override void Load()
//        {
//            try
//            {
//                DbSetSalesGoods dbSet = new DbSetSalesGoods();
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
//            _regionManager.Regions.Remove("SalesReportInfo");
//        }
//    }
//}