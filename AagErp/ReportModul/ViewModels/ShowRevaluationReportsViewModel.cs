using System;
using System.Windows;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class ShowRevaluationReportsViewModel: BufferRead<RevaluationProducts>
    {
        #region Properties

        private readonly IRegionManager _regionManager;

        #endregion

        public ShowRevaluationReportsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Load();
        }

        protected sealed override void Load()
        {
            try
            {
                SqlRevaluationProductsReportRepository sql = new SqlRevaluationProductsReportRepository();
                //Count = sql.GetCount();
                //ReportsList = sql.Load(Left, Step);
                RaisePropertyChanged("IsEnabledRightCommand");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _regionManager.Regions.Remove("RevaluationReportInfo");
        }
    }
}