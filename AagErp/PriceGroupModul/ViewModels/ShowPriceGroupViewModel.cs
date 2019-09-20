using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.ViewModels;

namespace PriceGroupModul.ViewModels
{
    class ShowPriceGroupViewModel : EntityViewModelBase<PriceGroup, PriceGroupViewModel, SqlPriceGroupRepository>
    {
        public ShowPriceGroupViewModel() : base("Ценовая группа добавлена", "Ценовая группа изменена")
        {
        }

        public override void PropertiesTransfer(PriceGroup fromEntity, PriceGroup toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Markup = fromEntity.Markup;
        }
    }
}