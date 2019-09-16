using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using Prism.Services.Dialogs;

namespace PriceGroupModul.ViewModels
{
    class ShowPriceGroupsViewModel : EntitiesViewModelBase<PriceGroup, SqlPriceGroupRepository>
    {
        public ShowPriceGroupsViewModel(IDialogService dialogService) : base(dialogService, "ShowPriceGroup", "Удалить ценовую группу?", "Ценовая группа удалена.")
        {
        }
    }
}