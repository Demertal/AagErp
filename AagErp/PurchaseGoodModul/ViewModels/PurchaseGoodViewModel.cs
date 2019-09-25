using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using ModelModul.ViewModels;
using Prism.Services.Dialogs;

namespace PurchaseGoodModul.ViewModels
{
    public class PurchaseGoodViewModel : ViewModelBaseMovementGoods<PurchaseMovementGoodsInfoViewModel>
    {
        public PurchaseGoodViewModel(IDialogService dialogService) : base(dialogService, "purchase", "Вы уверены, что хотите провести отчет о закупке товара?")
        {
        }

        protected override async Task<MovementGoods> PreparingReport()
        {
            return await Task.Run(() =>
            {
                MovementGoods temp = (MovementGoods) MovementGoodsReport.Clone();
                foreach (var movementGoodsInfo in temp.MovementGoodsInfosCollection)
                {
                    movementGoodsInfo.EquivalentCost = movementGoodsInfo.Price / temp.EquivalentRate;
                    foreach (var serialNumber in movementGoodsInfo.Product.SerialNumbersCollection)
                    {
                        serialNumber.DateCreated = null;
                        serialNumber.IdProduct = movementGoodsInfo.Product.Id;
                        temp.SerialNumberLogsCollection.Add(new SerialNumberLog {SerialNumber = serialNumber});
                    }

                    movementGoodsInfo.Product = null;
                }

                return temp;
            });
        }


        protected override void MovementGoodsReportOnPropertyChanged(object sender, PropertyChangedEventArgs ea)
        {
            if (ea.PropertyName == "IdCurrency" && CurrenciesList != null)
                MovementGoodsReport.Rate = CurrenciesList.FirstOrDefault(c => c.Id == MovementGoodsReport.IdCurrency)?.Cost;

            if ((ea.PropertyName == "IdCurrency" || ea.PropertyName == "IdEquivalentCurrency") && MovementGoodsReport.IdCurrency == MovementGoodsReport.IdEquivalentCurrency)
                MovementGoodsReport.EquivalentRate = 1;
            if(ea.PropertyName == "MovementGoodsInfosCollection")
                RaisePropertyChanged(nameof(MovementGoodsInfosList));
            RaisePropertyChanged(nameof(MovementGoodsReport));
        }

        protected override async Task InLoad()
        {
            IRepository<Store> storeRepository = new SqlStoreRepository();
            IRepository<Currency> currencyRepository = new SqlCurrencyRepository();
            IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();

            var loadStore = Task.Run(() => storeRepository.GetListAsync(CancelTokenLoad.Token),
                CancelTokenLoad.Token);
            var loadCurrency = Task.Run(() => currencyRepository.GetListAsync(CancelTokenLoad.Token),
                CancelTokenLoad.Token);
            var loadCounterparty =
                Task.Run(
                    () => counterpartyRepository.GetListAsync(CancelTokenLoad.Token,
                        CounterpartySpecification.GetCounterpartiesByType(ETypeCounterparties.Suppliers)),
                    CancelTokenLoad.Token);

            await Task.WhenAll(loadStore, loadCurrency, loadCounterparty);

            ArrivalStoresList = new ObservableCollection<Store>(loadStore.Result);
            CurrenciesList = new ObservableCollection<Currency>(loadCurrency.Result);
            EquivalentCurrenciesList = new ObservableCollection<Currency>(loadCurrency.Result);
            CounterpartiesList = new ObservableCollection<Counterparty>(loadCounterparty.Result);
        }

        protected override async Task<PurchaseMovementGoodsInfoViewModel> CreateMovementGoodInfoAsync(Product product)
        {
            return await Task.Run(() =>
                new PurchaseMovementGoodsInfoViewModel
                {
                    Count = 0,
                    IdProduct = product.Id,
                    Price = 0,
                    MovementGoods = MovementGoodsReport,
                    Product = product
                });
        }

        //private void Navigate(List<Product> revaluationProducts)
        //{
        //    //NavigationParameters navigationParameters =
        //    //    new NavigationParameters { { "RevaluationProducts", revaluationProducts } };
        //    //_regionManager.RequestNavigate("ContentRegion", "RevaluationProductsView", navigationParameters);
        //}

        //private void ListenKeyboard(KeyEventArgs obj)
        //{
        //    //if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
        //    //{
        //    //    _barcode += obj.Key.ToString()[1].ToString();
        //    //}
        //    //else if (obj.Key >= Key.A && obj.Key <= Key.Z)
        //    //{
        //    //    _barcode += obj.Key.ToString();
        //    //}
        //    //else if (obj.Key == Key.Enter)
        //    //{
        //    //    try
        //    //    {
        //    //        if (string.IsNullOrEmpty(_barcode) || _barcode.Length < 8 || _barcode.Length > 13)
        //    //        {
        //    //            _barcode = "";
        //    //            return;
        //    //        }
        //    //        SqlProductRepository dbSetProducts = new SqlProductRepository();
        //    //        Product product = dbSetProducts.FindProductByBarcode(_barcode);
        //    //        if (product == null) throw new Exception("Товар не найден");
        //    //        AddProduct(product);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        _barcode = "";
        //    //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //    }
        //    //    _barcode = "";
        //    //}
        //    //else
        //    //{
        //    //    _barcode = "";
        //    //}
        //}
    }
}
