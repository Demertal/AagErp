using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using Prism.Services.Dialogs;

namespace ReportModul.ViewModels
{
    class ShowTransportationGoodsReportsViewModel : EntitiyReportsBaseMovementGoodsViewModel
    {
        public ShowTransportationGoodsReportsViewModel(IDialogService dialogService) : base(dialogService, "moving")
        {
        }

        protected override async Task PreLoadAsync()
        {
            IRepository<Store> storeRepository = new SqlStoreRepository();

            var temp = await storeRepository.GetListAsync(CancelTokenSource.Token);
            var enumerable = temp.ToList();

            ArrivalStoresList = new ObservableCollection<Store>(enumerable);
            DisposalStoresList = new ObservableCollection<Store>(enumerable);
        }
    }
}
