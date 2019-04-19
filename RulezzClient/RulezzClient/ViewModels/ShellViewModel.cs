using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class ShellViewModel : BindableBase
    {
        #region Parametr

        public InteractionRequest<INotification> AddProductPopupRequest { get; set; }

        public DelegateCommand AddProductCommand { get; }

        //private readonly IUiDialogService _dialogService = new DialogService();
        //private Visibility _isProductGroupVisible;
        //private Visibility _isStructGroupVisible;
        //private Visibility _isUnitStorageVisible;
        //private ShowProductViewModel _showProduct;
        //private ShowStructurVM _showStructur;
        //private ShowUnitStorageVM _showUnitStorage;

        #endregion

        public ShellViewModel()
        {
            AddProductPopupRequest = new InteractionRequest<INotification>();
            AddProductCommand = new DelegateCommand(AddProduct);
            //IsProductGroupVisible = Visibility.Collapsed;
            //IsStructGroupVisible = Visibility.Collapsed;
            //IsUnitStorageVisible = Visibility.Collapsed;
            //ShowProductCommand = new DelegateCommand(() =>
            //{
            //    IsUnitStorageVisible = Visibility.Collapsed;
            //    IsStructGroupVisible = Visibility.Collapsed;
            //    IsProductGroupVisible = Visibility.Visible;
            //    ShowProduct = new ShowProductViewModel();
            //});
            //AddProductMainCommand = new DelegateCommand(() =>
            //{
            //    _dialogService.ShowDialog(DialogService.ChoiceView.AddProduct, null, true, b => { });
            //    if (IsProductGroupVisible == Visibility.Visible) ShowProduct.Update(ShowProductViewModel.ChoiceUpdate.Product);
            //});
            //ShowStructCommand = new DelegateCommand(() =>
            //{
            //    IsUnitStorageVisible = Visibility.Collapsed;
            //    IsProductGroupVisible = Visibility.Collapsed;
            //    IsStructGroupVisible = Visibility.Visible;
            //    ShowStructur = new ShowStructurVM();
            //});
            //RevaluationCommand = new DelegateCommand(() =>
            //{
            //    _dialogService.ShowDialog(DialogService.ChoiceView.Revaluation, null, false, b => { });
            //    if (IsProductGroupVisible == Visibility.Visible) ShowProduct.Update(ShowProductViewModel.ChoiceUpdate.Product);
            //});
            //ShowUnitStorageCommand = new DelegateCommand(() =>
            //{
            //    IsProductGroupVisible = Visibility.Collapsed;
            //    IsStructGroupVisible = Visibility.Collapsed;
            //    IsUnitStorageVisible = Visibility.Visible;
            //    ShowUnitStorage = new ShowUnitStorageVM();
            //});
            //PurchaseInvoiceCommand = new DelegateCommand(() =>
            //{
            //    _dialogService.ShowDialog(DialogService.ChoiceView.PurchaseInvoice, null, false, b => { });
            //    if (IsProductGroupVisible == Visibility.Visible) ShowProduct.Update(ShowProductViewModel.ChoiceUpdate.Product);
            //});
        }

        private void AddProduct()
        {
            AddProductPopupRequest.Raise(new Confirmation { Title = "Добавить товар" }, Callback);
        }

        private void Callback(INotification notification)
        {
            if (((Confirmation)notification).Confirmed)
            {
                //Reload();
            }
        }

        #region GestSetMethd

        //public Visibility IsProductGroupVisible
        //{
        //    get => _isProductGroupVisible;
        //    set
        //    {
        //        _isProductGroupVisible = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public Visibility IsStructGroupVisible
        //{
        //    get => _isStructGroupVisible;
        //    set
        //    {
        //        _isStructGroupVisible = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public Visibility IsUnitStorageVisible
        //{
        //    get => _isUnitStorageVisible;
        //    set
        //    {
        //        _isUnitStorageVisible = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public ShowProductViewModel ShowProduct
        //{
        //    get => _showProduct;
        //    set
        //    {
        //        _showProduct = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public ShowStructurVM ShowStructur
        //{
        //    get => _showStructur;
        //    set
        //    {
        //        _showStructur = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public ShowUnitStorageVM ShowUnitStorage
        //{
        //    get => _showUnitStorage;
        //    set
        //    {
        //        _showUnitStorage = value;
        //        RaisePropertyChanged();
        //    }
        //}

        #endregion

        #region DelegateCommand

        public DelegateCommand ShowProductCommand { get; }

        public DelegateCommand ShowStructCommand { get; }

        public DelegateCommand ShowUnitStorageCommand { get; }

        public DelegateCommand RevaluationCommand { get; }

        public DelegateCommand PurchaseInvoiceCommand { get; }

        #endregion
    }
}