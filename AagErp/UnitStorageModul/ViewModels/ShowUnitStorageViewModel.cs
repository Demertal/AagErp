using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;

namespace UnitStorageModul.ViewModels
{
    public class ShowUnitStorageViewModel : EntityViewModelBase<UnitStorage, UnitStorageViewModel, SqlUnitStorageRepository>
    {
        public ShowUnitStorageViewModel() : base("Ед. хр. добавлена", "Ед. хр. изменена")
        {
        }

        public override void PropertiesTransfer(UnitStorage fromEntity, UnitStorage toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Title = fromEntity.Title;
            toEntity.IsWeightGoods = fromEntity.IsWeightGoods;
        }
    }
}
