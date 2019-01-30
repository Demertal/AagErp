using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class ShellViewModel : BindableBase
    {
        private readonly IUiDialogService _dialogService = new DialogService();
        private Visibility _isProductGroupVisible;
        private Visibility _isStructGroupVisible;
        private ShowProductViewModel _showProduct;
        private ShowStructurVM _showStructur;

        public ShellViewModel()
        {
            IsProductGroupVisible = Visibility.Collapsed;
            IsStructGroupVisible = Visibility.Collapsed;
            ShowProductCommand = new DelegateCommand(() =>
            {
                IsStructGroupVisible = Visibility.Collapsed;
                IsProductGroupVisible = Visibility.Visible;
                ShowProduct = new ShowProductViewModel();
            });
            AddProductMainCommand = new DelegateCommand(() =>
            {
                _dialogService.ShowDialog(DialogService.ChoiceView.AddProduct, null, true, b => { });
                if (IsProductGroupVisible == Visibility.Visible) ShowProduct.Update(ShowProductViewModel.ChoiceUpdate.Product);
            });
            ShowStructCommand = new DelegateCommand(() =>
            {
                IsProductGroupVisible = Visibility.Collapsed;
                IsStructGroupVisible = Visibility.Visible;
                ShowStructur = new ShowStructurVM();
                if (IsProductGroupVisible == Visibility.Visible) ShowProduct.Update(ShowProductViewModel.ChoiceUpdate.Product);
            });
            RevaluationCommand = new DelegateCommand(() =>
            {
                _dialogService.ShowDialog(DialogService.ChoiceView.Revaluation, null, true, b => { });
                if (IsProductGroupVisible == Visibility.Visible) ShowProduct.Update(ShowProductViewModel.ChoiceUpdate.Product);
            });
        }

        public Visibility IsProductGroupVisible
        {
            get => _isProductGroupVisible;
            set
            {
                _isProductGroupVisible = value;
                RaisePropertyChanged();
            }
        }

        public Visibility IsStructGroupVisible
        {
            get => _isStructGroupVisible;
            set
            {
                _isStructGroupVisible = value;
                RaisePropertyChanged();
            }
        }

        public ShowProductViewModel ShowProduct
        {
            get => _showProduct;
            set
            {
                _showProduct = value;
                RaisePropertyChanged();
            }
        }

        public ShowStructurVM ShowStructur
        {
            get => _showStructur;
            set
            {
                _showStructur = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ShowProductCommand { get; }

        public DelegateCommand ShowStructCommand { get; }

        public DelegateCommand AddProductMainCommand { get; }

        public DelegateCommand RevaluationCommand { get; }
    }
}