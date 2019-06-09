﻿using System;
using System.Windows;
using ModelModul;
using ModelModul.RevaluationProduct;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class ShowRevaluationReportsViewModel: BufferRead<RevaluationProductsReports>, INavigationAware
    {
        #region Properties

        private readonly IRegionManager _regionManager;

        #endregion

        public ShowRevaluationReportsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Load();
        }

        protected sealed override async void Load()
        {
            try
            {
                DbSetRevaluationProducts dbSet = new DbSetRevaluationProducts();
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
            _regionManager.Regions.Remove("RevaluationReportInfo");
        }
    }
}