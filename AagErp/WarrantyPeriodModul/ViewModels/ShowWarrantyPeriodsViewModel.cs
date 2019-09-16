using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using Prism.Services.Dialogs;

namespace WarrantyPeriodModul.ViewModels
{
    class ShowWarrantyPeriodsViewModel: EntitiesViewModelBase<WarrantyPeriod, SqlWarrantyPeriodRepository>
    {
        public ShowWarrantyPeriodsViewModel(IDialogService dialogService) : base(dialogService, "ShowWarrantyPeriod", "Удалить гарантийный период?", "Гарантийный период удален.")
        {
        }
    }
}
