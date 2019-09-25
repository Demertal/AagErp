using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using Prism.Services.Dialogs;

namespace ReportModul.ViewModels
{
    class ShowRevaluationReportsViewModel : EntitiyReportsViewModel<RevaluationProduct>
    {
        public ShowRevaluationReportsViewModel(IDialogService dialogService) : base(dialogService) {}

        protected override async void LoadAsync()
        {
            CancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenSource = newCts;

            try
            {
                SqlRevaluationProductRepository repository = new SqlRevaluationProductRepository();
                ReportsList = new ObservableCollection<RevaluationProduct>(await repository.GetReportRevaluation(
                    CancelTokenSource.Token));
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (CancelTokenSource == newCts)
                CancelTokenSource = null;
        }
    }
}