using System.Collections.ObjectModel;
using EventAggregatorLibrary;
using ModelModul;
using ModelModul.Product;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ProductModul.ViewModels
{
    class ShowProductViewModel : BindableBase
    {
        #region Properties

        readonly IEventAggregator _ea;

        public DbSetProductsModel DbSetProductsModel = new DbSetProductsModel();

        public ObservableCollection<Products> Products => DbSetProductsModel.List;

        private string _findString;
        public string FindString
        {
            get => _findString;
            set
            {
                _findString = value;
                RaisePropertyChanged();
                Load(-1);
            }
        }

        #endregion

        public ShowProductViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<GroupsEventAggregator>().Subscribe(GetSelectedGroup);
            FindString = "";
            //DeleteProduct = new DelegateCommand(() =>
            //{
            //    if (MessageBox.Show("Вы уверены что хотите удалить товар?", "Удаление", MessageBoxButton.YesNo,
            //            MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            //    try
            //    {
            //        DbSetGroupsModel.Delete(SelectedProduct.Id);
            //        Update(ChoiceUpdate.Product);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
            //            MessageBoxImage.Error);
            //    }
            //});
            //UpdateProduct = new DelegateCommand(() =>
            //{
            //    //if (SelectedProduct == null) return;
            //    //object[] param = new object[1];
            //    //param[0] = SelectedProduct;
            //    //_dialogService.ShowDialog(DialogService.ChoiceView.UpdateProduct, param, true, b => { });
            //    //Update(ChoiceUpdate.Product);
            //});
        }

        //public ShowProductViewModel(object obj, Window wnd)
        //{
        //    SelectionMode = SelectionMode.Multiple;
        //    IsSelectedProduct = Visibility.Visible;
        //    IsNotSelectedProduct = Visibility.Collapsed;
        //    Update(ChoiceUpdate.Store);
        //    FindString = "";
        //    DeleteProduct = null;
        //    UpdateProduct = new DelegateCommand(() =>
        //    {
        //        using (StoreEntities db = new StoreEntities())
        //        {
        //            if (obj.GetType().ToString() == "RulezzClient.ViewModels.RevaluationVM")
        //            {
        //                RevaluationProductModel revItem =
        //                    new RevaluationProductModel(db.Products.Find(SelectedProduct?.Id));
        //                if (!(obj as RevaluationVM).AllProduct.Contains(revItem)) (obj as RevaluationVM).AllProduct.Add(revItem);
        //            }
        //            else if (obj.GetType().ToString() == "RulezzClient.ViewModels.PurchaseInvoiceVM")
        //            {
        //                PurchaseInvoiceProductVM purItem =
        //                    new PurchaseInvoiceProductVM(
        //                        new PurchaseInvoiceProductModel(db.Products.Find(SelectedProduct?.Id)));
        //                if (!(obj as PurchaseInvoiceVM).PurchaseInvoiceProduct.Contains(purItem)) (obj as PurchaseInvoiceVM).PurchaseInvoiceProduct.Add(purItem);
        //            }
        //        }
        //        wnd.Close();
        //    });
        //    SelectedProductCommand = new DelegateCommand<object>(sel =>
        //    {
        //        using (StoreEntities db = new StoreEntities())
        //        {
        //            foreach (var item in (IList)sel)
        //            {
        //                if (obj.GetType().ToString() == "RulezzClient.ViewModels.RevaluationVM")
        //                {
        //                    RevaluationProductModel revItem =
        //                        new RevaluationProductModel(db.Products.Find((item as ProductView)?.Id));
        //                    if ((obj as RevaluationVM).AllProduct.Contains(revItem)) continue;
        //                    (obj as RevaluationVM).AllProduct.Add(revItem);
        //                }
        //                else if (obj.GetType().ToString() == "RulezzClient.ViewModels.PurchaseInvoiceVM")
        //                {
        //                    PurchaseInvoiceProductVM purItem =
        //                        new PurchaseInvoiceProductVM(
        //                            new PurchaseInvoiceProductModel(db.Products.Find((item as ProductView)?.Id)));
        //                    if ((obj as PurchaseInvoiceVM).PurchaseInvoiceProduct.Contains(purItem)) continue;
        //                    (obj as PurchaseInvoiceVM).PurchaseInvoiceProduct.Add(purItem);
        //                }
        //            }
        //        }
        //        wnd.Close();
        //    });
        //}

        #region Load

        public async void Load(int idGroup)
        {
            if (!string.IsNullOrEmpty(FindString))
            {
                await DbSetProductsModel.LoadByFindString(FindString);
            }
            else
            {
                await DbSetProductsModel.Load(idGroup);
            }
            RaisePropertyChanged("Products");
        }

        private void GetSelectedGroup(Groups obj)
        {
            if (obj == null) Load(-1);
            else Load(obj.Id);
        }

        #endregion

        public DelegateCommand DeleteProduct { get; }

        public DelegateCommand UpdateProduct { get; }

        public DelegateCommand<object> SelectedProductCommand { get; }
    }
}
