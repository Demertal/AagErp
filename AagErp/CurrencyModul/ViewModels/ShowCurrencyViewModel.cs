using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.ViewModels;

namespace CurrencyModul.ViewModels
{
    public class ShowCurrencyViewModel : EntityViewModelBase<Currency, CurrencyViewModel, SqlCurrencyRepository>
    {
        public ShowCurrencyViewModel() : base("Валюта добавлена", "Валюта изменена")
        {
        }

        public override void PropertiesTransfer(Currency fromEntity, Currency toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Title = fromEntity.Title;
            toEntity.Cost = fromEntity.Cost;
            toEntity.IsDefault = fromEntity.IsDefault;
        }
    }
}