using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Services.Dialogs;

namespace ReportModul.ViewModels
{
    class ShowSalesReportsViewModel : EntitiyReportsBaseMovementGoodsViewModel
    {
        public ShowSalesReportsViewModel(IDialogService dialogService) : base(dialogService, "sale")
        {
        }

        protected override async Task PreLoadAsync()
        {
            IRepository<Store> storeRepository = new SqlStoreRepository();
            IRepository<Counterparty> counterpartyRepository = new SqlCounterpartyRepository();

            var loadStore = Task.Run(() => storeRepository.GetListAsync(CancelTokenSource.Token),
                CancelTokenSource.Token);
            var loadCounterparty =
                Task.Run(
                    () => counterpartyRepository.GetListAsync(CancelTokenSource.Token,
                        CounterpartySpecification.GetCounterpartiesByType(ETypeCounterparties.Buyers)),
                    CancelTokenSource.Token);

            await Task.WhenAll(loadStore, loadCounterparty);

            DisposalStoresList = new ObservableCollection<Store>(loadStore.Result);
            CounterpartiesList = new ObservableCollection<Counterparty>(loadCounterparty.Result);
        }
    }
}