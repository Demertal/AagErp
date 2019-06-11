﻿using System;
using System.Windows;
using ModelModul;
using ModelModul.RevaluationProduct;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class ShowRevaluationReportsViewModel: BufferRead<RevaluationProductsReports>
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
                DbSetRevaluationProducts dbSet = new DbSetRevaluationProducts();
                Count = dbSet.GetCount();
                ReportsList = dbSet.Load(Left, Step);
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