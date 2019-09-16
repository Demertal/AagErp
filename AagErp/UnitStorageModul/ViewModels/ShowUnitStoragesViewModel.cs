using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using Prism.Services.Dialogs;

namespace UnitStorageModul.ViewModels
{
    class ShowUnitStoragesViewModel : EntitiesViewModelBase<UnitStorage, SqlUnitStorageRepository>
    {
        public ShowUnitStoragesViewModel(IDialogService dialogService) : base(dialogService, "ShowUnitStorage", "Удалить ед. хр.?", "Ед. хр. удалена.")
        {
        }
    }
}

