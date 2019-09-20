using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.ViewModels;

namespace StoreModul.ViewModels
{
    class ShowStoreViewModel : EntityViewModelBase<Store, StoreViewModel, SqlStoreRepository>
    {
        public ShowStoreViewModel() : base("Склад добавлен", "Склад изменен")
        {
        }

        public override void PropertiesTransfer(Store fromEntity, Store toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Title = fromEntity.Title;
        }
    }
}