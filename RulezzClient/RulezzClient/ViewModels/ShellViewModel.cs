using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class ShellViewModel : BindableBase
    {
        private readonly IUiDialogService _dialogService = new DialogService();
        private Visibility _isProductGroupVisible;
        private ShowProductViewModel _showProduct;

        public ShellViewModel()
        {
            IsProductGroupVisible = Visibility.Collapsed;
            ShowProductCommand = new DelegateCommand(() =>
            {
                IsProductGroupVisible = Visibility.Visible;
                ShowProduct = new ShowProductViewModel();
            });
            AddProductMainCommand = new DelegateCommand(() =>
            {
                AddProductViewModel viewModel = new AddProductViewModel();
                _dialogService.ShowDialog(DialogService.ChoiceView.AddProduct, viewModel, true, b => { });
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

        public ShowProductViewModel ShowProduct
        {
            get => _showProduct;
            set
            {
                _showProduct = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ShowProductCommand { get; }

        public DelegateCommand AddProductMainCommand { get; }
    }
}