using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelModul;
using ModelModul.RevaluationProduct;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    class ShowRevaluationReportsViewModel: BindableBase, INavigationAware
    {
        #region Properties

        private readonly IRegionManager _regionManager;

        private ObservableCollection<RevaluationProductsReports> _revaluationProductsReportsList = new ObservableCollection<RevaluationProductsReports>();
        public ObservableCollection<RevaluationProductsReports> RevaluationProductsReportsList
        {
            get => _revaluationProductsReportsList;
            set => SetProperty(ref _revaluationProductsReportsList, value);
        }

        private int _left;
        public int Left
        {
            get => _left;
            set
            {
                SetProperty(ref _left, value);
                RaisePropertyChanged("IsEnabledLeftCommand");
            }
        }

        private int _right;
        public int Right
        {
            get => _right;
            set
            {
                SetProperty(ref _right, value);
                RaisePropertyChanged("IsEnabledRightCommand");
            }
        }

        private int _step;

        private int _count;

        public bool IsEnabledLeftCommand => Left > 0;
        public bool IsEnabledRightCommand => Right < _count;

        public DelegateCommand GoLeftCommand { get; }
        public DelegateCommand GoRightCommand { get; }

        #endregion

        public ShowRevaluationReportsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _step = 10;
            Left = 0;
            Right = _step;
            GoLeftCommand = new DelegateCommand(GoLeft).ObservesCanExecute(() => IsEnabledLeftCommand);
            GoRightCommand = new DelegateCommand(GoRight).ObservesCanExecute(() => IsEnabledRightCommand);
            Load();
        }

        private void GoRight()
        {
            Left += _step;
            Right += _step;
            Load();
        }

        private void GoLeft()
        {
            Left -= _step;
            Right -= _step;
            Load();
        }

        private async void Load()
        {
            try
            {
                DbSetRevaluationProducts dbSet = new DbSetRevaluationProducts();
                _count = await dbSet.GetCount();
                RevaluationProductsReportsList = await dbSet.LoadAsync(Left, _step);
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