using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using Prism.Services.Dialogs;

namespace StoreModul.ViewModels
{
    class ShowStoresViewModel : EntitiesViewModelBase<Store, SqlStoreRepository>
    {
        public ShowStoresViewModel(IDialogService dialogService) : base(dialogService, "ShowStore", "Удалить склад?", "Склад удален.")
        {
        }
    }
}