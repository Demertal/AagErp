using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient
{
    class MainVm : BindableBase
    {
        private Visibility _isProductGroupVisible;
        private ShowProductVm _showProduct;

        public MainVm()
        {
            IsProductGroupVisible = Visibility.Collapsed;
            ShowProductCommand = new DelegateCommand(() =>
            {
                IsProductGroupVisible = Visibility.Visible;
                ShowProduct = new ShowProductVm();
            });
            //AddProductMainCommand = new DelegateCommand(() =>
            //{
            //    AddProduct ad = new AddProduct { DataContext = new VmAddProduct(_model), Title = "Добавить товар" };
            //    ad.ShowDialog();
            //});
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

        public ShowProductVm ShowProduct
        {
            get => _showProduct;
            set
            {
                _showProduct = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ShowProductCommand { get; }

        //public DelegateCommand AddProductMainCommand { get; }
    }
}