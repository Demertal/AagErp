using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using Prism.Services.Dialogs;

namespace CurrencyModul.ViewModels
{
    class ShowCurrenciesViewModel : EntitiesViewModelBase<Currency, SqlCurrencyRepository>
    {
        public ShowCurrenciesViewModel(IDialogService dialogService) : base(dialogService, "ShowCurrency", "Удалить валюту?", "Валюта удалена.")
        {
        }
    }
}