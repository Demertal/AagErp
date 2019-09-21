using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.ViewModels;
using Prism.Services.Dialogs;

namespace TransportationGoods.ViewModels
{
    public class TransportationGoodsViewModel : ViewModelBaseMovementGoods<TransportationMovementGoodsInfoViewModel>
    {
        public TransportationGoodsViewModel(IDialogService dialogService) : base(dialogService, "moving", "Вы уверены, что хотите провести отчет о перемещении товара?")
        {
        }

        protected override void MovementGoodsReportOnPropertyChanged(object sender, PropertyChangedEventArgs ea)
        {
            if(ea.PropertyName == "IdDisposalStore")
            {
                foreach (var movementGoodsInfo in MovementGoodsInfosList)
                {
                    ((TransportationMovementGoodsInfoViewModel) movementGoodsInfo).ValidateCount();
                    foreach (SerialNumberViewModel serialNumber in movementGoodsInfo.Product.SerialNumbersCollection)
                    {
                        serialNumber.IdStore = MovementGoodsReport.IdDisposalStore;
                    }
                }
            }

            if (ea.PropertyName == "MovementGoodsInfosCollection")
                RaisePropertyChanged(nameof(MovementGoodsInfosList));
            RaisePropertyChanged(nameof(MovementGoodsReport));
        }

        protected override async Task InLoad()
        {
            IRepository<Store> storeRepository = new SqlStoreRepository();

            var temp = await storeRepository.GetListAsync(CancelTokenLoad.Token);
            var enumerable = temp.ToList();

            ArrivalStoresList = new ObservableCollection<Store>(enumerable);
            DisposalStoresList = new ObservableCollection<Store>(enumerable);
        }

        protected override async Task<TransportationMovementGoodsInfoViewModel> CreateMovementGoodInfoAsync(Product product)
        {
            return await Task.Run(() =>
                new TransportationMovementGoodsInfoViewModel { Count = 0, IdProduct = product.Id, Price = null, MovementGoods = MovementGoodsReport, Product = product });
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
