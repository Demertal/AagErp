using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;

namespace WarrantyPeriodModul.ViewModels
{
    class ShowWarrantyPeriodViewModel : EntityViewModelBase<WarrantyPeriod, WarrantyPeriodViewModel, SqlWarrantyPeriodRepository>
    {
        public ShowWarrantyPeriodViewModel() : base("Гарантийный период добавлен", "Гарантийный период изменен")
        {
        }

        public override void PropertiesTransfer(WarrantyPeriod fromEntity, WarrantyPeriod toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Period = fromEntity.Period;
        }
    }
}
