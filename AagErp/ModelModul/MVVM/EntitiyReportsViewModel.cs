using System.Collections.ObjectModel;
using System.Threading;
using ModelModul.Models;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public abstract class EntitiyReportsViewModel<TReports> : ViewModelBase
        where TReports : ModelBase<TReports>
    {
        #region Properties

        protected int Step, Count;
        protected CancellationTokenSource CancelTokenSource;

        private ObservableCollection<TReports> _reportsList = new ObservableCollection<TReports>();
        public ObservableCollection<TReports> ReportsList
        {
            get => _reportsList;
            set => SetProperty(ref _reportsList, value);
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

        public bool IsEnabledLeftCommand => Left > 0;
        public bool IsEnabledRightCommand => Right < Count;

        #endregion

        #region Command

        public DelegateCommand GoLeftCommand { get; }
        public DelegateCommand GoRightCommand { get; }

        #endregion

        protected EntitiyReportsViewModel(IDialogService dialogService) : base(dialogService)
        {
            Step = 10;
            Left = 0;
            Right = Step;
            GoLeftCommand = new DelegateCommand(GoLeft).ObservesCanExecute(() => IsEnabledLeftCommand);
            GoRightCommand = new DelegateCommand(GoRight).ObservesCanExecute(() => IsEnabledRightCommand);
        }

        #region CommandMethod

        private void GoRight()
        {
            Left += Step;
            Right += Step;
            LoadAsync();
        }

        private void GoLeft()
        {
            Left -= Step;
            Right -= Step;
            LoadAsync();
        }

        #endregion

        protected abstract void LoadAsync();

        #region INavigationAware

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
            CancelTokenSource?.Cancel();
        }

        #endregion
    }
}

