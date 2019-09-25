//using System;
//using System.Windows;
//using ModelModul.MVVM;
//using ModelModul.Report;
//using Prism.Services.Dialogs;

//namespace ReportModul.ViewModels
//{
//    class ShowProfitStatementViewModel : EntitiyReportsViewModel<>
//    {
//        #region Property

//        private DateTime _startDate;
//        public DateTime StartDate
//        {
//            get => _startDate;
//            set
//            {
//                SetProperty(ref _startDate, value);
//                LoadAsync();
//            }
//        }

//        private DateTime _endDate;
//        public DateTime EndDate
//        {
//            get => _endDate;
//            set
//            {
//                SetProperty(ref _endDate, value);
//                LoadAsync();
//            }
//        }

//        //public string FinalSum
//        //{
//        //    get { return "Итого прибыль: " + ReportList.Sum(obj => obj.FinalSum).ToString("C", new CultureInfo("UA-ua")); }
//        //}

//        #endregion

//        public ShowProfitStatementViewModel(IDialogService dialogService) : base(dialogService)
//        {
//            EndDate = DateTime.Today;
//            StartDate = DateTime.Today;
//        }

//        private void Load()
//        {
//            try
//            {
//                DbSetReports dbSet = new DbSetReports();
//                //ReportList = dbSet.GetFinalReport(StartDate, EndDate);
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//            RaisePropertyChanged("FinalSum");
//        }

//        protected override void LoadAsync()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
