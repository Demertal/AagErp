using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using ModelModul.ViewModels;
using Prism.Services.Dialogs;

namespace CashierWorkplaceModul.ViewModels
{
    class CashierWorkplaceViewModel : ViewModelBaseMovementGoods<TransportationMovementGoodsInfoViewModel>
    {
        public CashierWorkplaceViewModel(IDialogService dialogService) : base(dialogService, "sale", "Вы уверены, что хотите провести продажу?")
        {
        }

        protected override void MovementGoodsReportOnPropertyChanged(object sender, PropertyChangedEventArgs ea)
        {
            if (ea.PropertyName == "MovementGoodsInfosCollection")
                RaisePropertyChanged(nameof(MovementGoodsInfosList));
            RaisePropertyChanged(nameof(MovementGoodsReport));
            RaisePropertyChanged(nameof(CanAdd));
        }

        protected override async Task InLoad()
        {
            IRepository<Store> storeRepository = new SqlStoreRepository();

            DisposalStoresList = new ObservableCollection<Store>(await storeRepository.GetListAsync(CancelTokenLoad.Token));

            MovementGoodsReport.IdDisposalStore = DisposalStoresList.FirstOrDefault()?.Id;
        }

        protected override async void HandlingBarcode(string barcode)
        {
            if(string.IsNullOrEmpty(barcode)) return;
            try
            {
                IRepository<SerialNumber> serialNumberRepository = new SqlSerialNumberRepository();
                bool isSerialNumber =
                    await serialNumberRepository.AnyAsync(
                        where: SerialNumberSpecification.GetSerialNumbersValue(barcode));

                MovementGoodsInfo temp = MovementGoodsInfosList
                    .FirstOrDefault(m => m.Product.SerialNumbersCollection.Any(s => string.IsNullOrEmpty(s.Value)));

                if (temp != null && !isSerialNumber)
                {
                    throw new Exception("Сначала нужно указать серийный номер к товару: \"" +
                                        temp.Product.Title +
                                        "\".\nЕсли это был серийный номер попробуйте указать его еще раз, использовать другой серийный номер, или ввести вручную");
                }

                if (temp != null)
                {
                    temp.Product.SerialNumbersCollection.FirstOrDefault(s => string.IsNullOrEmpty(s.Value)).Value =
                        barcode;
                    return;
                }

                MovementGoodsInfo movementGoodsInfo;

                if (isSerialNumber)
                {
                    SerialNumberViewModel serialNumber = new SerialNumberViewModel(
                        await serialNumberRepository.GetItemAsync(
                            where: SerialNumberSpecification.GetSerialNumbersValue(barcode), include: (s => s.Product, new Expression<Func<object, object>>[] { p => ((Product)p).UnitStorage } )), MovementGoodsReport.IdDisposalStore);
                    movementGoodsInfo =
                        MovementGoodsInfosList.FirstOrDefault(m => m.IdProduct == serialNumber.IdProduct);
                    if (movementGoodsInfo == null)
                    {
                        movementGoodsInfo = await CreateMovementGoodInfoAsync(serialNumber.Product);
                        MovementGoodsInfosList.Add(movementGoodsInfo);
                    }
                    serialNumber.Product =
                        movementGoodsInfo.Product;
                    movementGoodsInfo.Product.SerialNumbersCollection.Add(serialNumber);
                    movementGoodsInfo.Count++;
                    movementGoodsInfo.Product.SerialNumbersCollection.Last().Value =
                        movementGoodsInfo.Product.SerialNumbersCollection.Last().Value;
                    return;

                }
                IRepository<Product> productRepository = new SqlProductRepository();
                Product product = await productRepository.GetItemAsync(
                    where: ProductSpecification.GetProductsByBarcode(barcode), include: (p => p.UnitStorage, null));

                if (product == null) throw new Exception("Товар не найден");

                movementGoodsInfo =
                    MovementGoodsInfosList.FirstOrDefault(m => m.IdProduct == product.Id);
                if (movementGoodsInfo == null)
                {
                    movementGoodsInfo = await CreateMovementGoodInfoAsync(product);
                    MovementGoodsInfosList.Add(movementGoodsInfo);
                }
                if(!product.UnitStorage.IsWeightGoods)
                    movementGoodsInfo.Count++;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override async Task<TransportationMovementGoodsInfoViewModel> CreateMovementGoodInfoAsync(Product product)
        {
            SqlProductRepository productRepository = new SqlProductRepository();

            return new TransportationMovementGoodsInfoViewModel
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
    }
}
