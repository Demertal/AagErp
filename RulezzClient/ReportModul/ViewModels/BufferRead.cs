using System.Collections.ObjectModel;
using ModelModul;
using Prism.Commands;
using Prism.Regions;

namespace ReportModul.ViewModels
{
    public class BufferRead<T>: ViewModelBase
    {
        private ObservableCollection<T> _reportsList = new ObservableCollection<T>();
        public ObservableCollection<T> ReportsList
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

        protected int Step;

        protected int Count;

        public bool IsEnabledLeftCommand => Left > 0;
        public bool IsEnabledRightCommand => Right < Count;

        public DelegateCommand GoLeftCommand { get; }
        public DelegateCommand GoRightCommand { get; }

        protected BufferRead()
        {
            Step = 10;
            Left = 0;
            Right = Step;
            GoLeftCommand = new DelegateCommand(GoLeft).ObservesCanExecute(() => IsEnabledLeftCommand);
            GoRightCommand = new DelegateCommand(GoRight).ObservesCanExecute(() => IsEnabledRightCommand);
        }

        private void GoRight()
        {
            Left += Step;
            Right += Step;
            Load();
        }

        private void GoLeft()
        {
            Left -= Step;
            Right -= Step;
            Load();
        }

        protected virtual void Load(){}

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
