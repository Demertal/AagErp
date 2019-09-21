using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.ViewModels;
using Prism.Services.Dialogs;

namespace CashierWorkplaceModul.ViewModels
{
    class CashierWorkplaceViewModel : ViewModelBaseMovementGoods<SaleMovementGoodsInfoViewModel>
    {
        public CashierWorkplaceViewModel(IDialogService dialogService) : base(dialogService, "sale", "Вы уверены, что хотите провести продажу?")
        {
        }

        //private async void Post()
        //{
        //    if (_errorPostCount == 0 && MessageBox.Show(
        //            "Вы уверены, что хотите провести продажу? Этот отчет невозможно будет изменить после.",
        //            "Проведение продажи", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
        //        MessageBoxResult.Yes) return;
        //    try
        //    {
        //        SqlSerialNumberRepository serialNumberRepository = new SqlSerialNumberRepository();
        //        MovementGoods temp = (MovementGoods)SalesGood.Clone();
        //        temp.DateClose = null;
        //        temp.DateCreate = null;
        //        foreach (var movementGoodsInfo in temp.MovementGoodsInfosCollection)
        //        {
        //            foreach (var serialNumber in movementGoodsInfo.Product.SerialNumbersCollection)
        //            {
        //                List<long> freeSerialNumbers =
        //                    await serialNumberRepository.GetFreeSerialNumbers(movementGoodsInfo.Product.Id,
        //                        serialNumber.Value, temp.IdDisposalStore.Value);
        //                temp.SerialNumberLogsCollection.Add(new SerialNumberLog { IdSerialNumber = freeSerialNumbers.First(fs => temp.SerialNumberLogsCollection.All(s => s.IdSerialNumber != fs)) });
        //            }

        //            movementGoodsInfo.Product = null;
        //        }

        //        IRepository<MovementGoods> movementGoodsRepository = new SqlMovementGoodsRepository();
        //        await movementGoodsRepository.CreateAsync(temp);
        //        MessageBox.Show("Отчет о продаже добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        //        _errorPostCount = 0;
        //        NewMovementGood();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        if (ex.InnerException is SqlException sex && (sex.State == 2 || sex.State == 3))
        //        {
        //            if (_errorPostCount == 5)
        //            {
        //                _errorPostCount = 0;
        //                MessageBox.Show("Произошла ошибка проверьте данные", "Ошибка", MessageBoxButton.OK,
        //                    MessageBoxImage.Error);
        //                return;
        //            }
        //            foreach (var movementGoodsInfo in SalesGood.MovementGoodsInfosCollection)
        //            {
        //                movementGoodsInfo.Count = movementGoodsInfo.Count;
        //                foreach (var serialNumber in movementGoodsInfo.Product.SerialNumbersCollection)
        //                {
        //                    serialNumber.Value = serialNumber.Value;
        //                }
        //                foreach (var serialNumber in movementGoodsInfo.Product.SerialNumbersCollection)
        //                {
        //                    serialNumber.Id = 0;
        //                }
        //            }
        //            RaisePropertyChanged("IsValidate");
        //            if (IsValidate)
        //            {
        //                _errorPostCount++;
        //                Post();
        //            }
        //            else
        //            {
        //                _errorPostCount = 0;
        //                MessageBox.Show("Произошла ошибка проверьте данные", "Ошибка", MessageBoxButton.OK,
        //                    MessageBoxImage.Error);
        //            }
        //            return;
        //        }
        //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        protected override void MovementGoodsReportOnPropertyChanged(object sender, PropertyChangedEventArgs ea)
        {
            if (ea.PropertyName == "MovementGoodsInfosCollection")
                RaisePropertyChanged(nameof(MovementGoodsInfosList));
            RaisePropertyChanged(nameof(MovementGoodsReport));
        }

        protected override async Task InLoad()
        {
            IRepository<Store> storeRepository = new SqlStoreRepository();

            DisposalStoresList = new ObservableCollection<Store>(await storeRepository.GetListAsync(CancelTokenLoad.Token));

            MovementGoodsReport.IdDisposalStore = DisposalStoresList.FirstOrDefault()?.Id;
        }

        protected override async Task<SaleMovementGoodsInfoViewModel> CreateMovementGoodInfoAsync(Product product)
        {
            SqlProductRepository productRepository = new SqlProductRepository();

            return new SaleMovementGoodsInfoViewModel
            {
                IdProduct = product.Id,
                Price = await productRepository.GetCurrentPrice(product.Id),
                MovementGoods = MovementGoodsReport,
                Product = product,
                Count = 0
            };
        }

        protected override async Task<MovementGoods> PreparingReport()
        {
            MovementGoods temp = (MovementGoods)MovementGoodsReport.Clone();

            Task<(long id, Task<List<long>> serialNumbers)>[] tasksGetFreeSerialNumbers = temp.MovementGoodsInfosCollection.SelectMany(m => m.Product.SerialNumbersCollection.Select(s => Task.Run(() =>
            {
                SqlSerialNumberRepository serialNumberRepository = new SqlSerialNumberRepository();

                return (m.Product.Id, serialNumberRepository.GetFreeSerialNumbers(m.Product.Id, s.Value, temp.IdDisposalStore.Value));
            }))).ToArray();

            await Task.WhenAll(tasksGetFreeSerialNumbers);

            List<(long id, Task<List<long>> serialNumbers)> resultTask =
                tasksGetFreeSerialNumbers.Select(t => t.Result).ToList();

            foreach (var movementGoodsInfo in temp.MovementGoodsInfosCollection)
            {
                if (movementGoodsInfo.Product.KeepTrackSerialNumbers)

                    foreach (var valueTuple in resultTask.Where(r => r.id == movementGoodsInfo.Product.Id))
                    {
                        temp.SerialNumberLogsCollection.Add(new SerialNumberLog
                        {
                            IdSerialNumber = valueTuple.serialNumbers.Result.First(s =>
                                temp.SerialNumberLogsCollection.All(ss => ss.IdSerialNumber != s))
                        });
                    }

                movementGoodsInfo.Product = null;
            }

            return temp;
        }

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
        //    //        SqlSerialNumberRepository dbSetSerialNumbers = new SqlSerialNumberRepository();
        //    //        ObservableCollection<SerialNumber> serialNumberses = dbSetSerialNumbers.FindFreeSerialNumbers(_barcode);

        //    //        SerialNumber freeSerialNumbers = null;
        //    //        foreach (var objSer in serialNumberses)
        //    //        {
        //    //            if (SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == objSer.Id) != null) continue;
        //    //            freeSerialNumbers = objSer;
        //    //            break;
        //    //        }

        //    //        SalesInfosViewModel saleInfos = SalesInfos.FirstOrDefault(objSale =>
        //    //            objSale.Product.WarrantyPeriod.Period != "Нет" && objSale.SerialNumber.Id == 0);

        //    //        if (freeSerialNumbers == null && saleInfos != null)
        //    //            throw new Exception("Сначала нужно указать серийный номер к товару: \"" +
        //    //                                saleInfos.Product.Title +
        //    //                                "\".\nЕсли это был серийный номер попробуйте указать его еще раз или используйте другой серийный номер, или ввести вручную");
        //    //        if (freeSerialNumbers != null && saleInfos != null)
        //    //        {
        //    //            saleInfos.SerialNumber = freeSerialNumbers;
        //    //            _barcode = "";
        //    //            return;
        //    //        }
        //    //        SqlProductRepository dbSetProducts = new SqlProductRepository();
        //    //        Product product = null;
        //    //        if (freeSerialNumbers != null)
        //    //        {
        //    //            product = dbSetProducts.FindProductById(freeSerialNumbers.IdProduct);
        //    //            if (product == null) throw new Exception("Товар не найден");
        //    //            SalesInfos.AddAsync(new SalesInfosViewModel
        //    //            {
        //    //                IdProduct = product.Id,
        //    //                Product = product,
        //    //                SellingPrice = product.SalesPrice,
        //    //                Count = 1,
        //    //                SerialNumber = freeSerialNumbers
        //    //            });
        //    //        }

        //    //        if (freeSerialNumbers == null)
        //    //        {
        //    //            if (serialNumberses.Count != 0) throw new Exception("Нет свободного серийного номера: " + _barcode);
        //    //            product = dbSetProducts.FindProductByBarcode(_barcode);
        //    //            if (product == null) throw new Exception("Товар не найден");
        //    //            saleInfos = SalesInfos.FirstOrDefault(objSale => objSale.IdProduct == product.Id);
        //    //            SalesInfosViewModel temp = new SalesInfosViewModel
        //    //            {
        //    //                IdProduct = product.Id,
        //    //                Product = product,
        //    //                SellingPrice = product.SalesPrice,
        //    //                Count = 1,
        //    //                SerialNumber = new SerialNumber()
        //    //            };
        //    //            if (saleInfos == null)
        //    //            {
        //    //                SalesInfos.AddAsync(temp);
        //    //            }
        //    //            else
        //    //            {
        //    //                if (product.WarrantyPeriod.Period == "Нет") saleInfos.Count++;
        //    //                else SalesInfos.AddAsync(temp);
        //    //            }
        //    //        }
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

        //private void ListenKeyboardSerialNumbers(KeyEventArgs obj)
        //{
        //    //if (obj.Key >= Key.D0 && obj.Key <= Key.D9)
        //    //{
        //    //    _serialNumber += obj.Key.ToString()[1].ToString();
        //    //}
        //    //else if (obj.Key >= Key.A && obj.Key <= Key.Z)
        //    //{
        //    //    _serialNumber += obj.Key.ToString();
        //    //}
        //    //else if (obj.Key == Key.Enter)
        //    //{
        //    //    try
        //    //    {
        //    //        SalesInfosViewModel sale = SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == 0 && objSale.Product.WarrantyPeriod.Period != "Нет");
        //    //        if (sale == null) throw new Exception("Возникла непредвиденная ошибка");
        //    //        SqlSerialNumberRepository dbSetSerialNumbers = new SqlSerialNumberRepository();
        //    //        ObservableCollection<SerialNumber> serialNumberses = dbSetSerialNumbers.FindFreeSerialNumbers(_serialNumber, sale.IdProduct);
        //    //        if (serialNumberses.Count == 0) throw new Exception("Cерийный номер: \"" + _serialNumber + "\" для этого товара не найден: ");
        //    //        SerialNumber freeSerialNumbers = null;
        //    //        foreach (var objSer in serialNumberses)
        //    //        {
        //    //            if (SalesInfos.FirstOrDefault(objSale => objSale.SerialNumber.Id == objSer.Id) != null) continue;
        //    //            freeSerialNumbers = objSer;
        //    //            break;
        //    //        }
        //    //        sale.SerialNumber = freeSerialNumbers ?? throw new Exception("Не свободного серийного номера: " + _serialNumber);
        //    //        _serialNumber = "";
        //    //        _barcode = "";
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        _barcode = "";
        //    //        _serialNumber = "";
        //    //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    _barcode = "";
        //    //    _serialNumber = "";
        //    //}
        //}
    }
}
