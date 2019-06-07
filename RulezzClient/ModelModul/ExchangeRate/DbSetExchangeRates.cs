using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.ExchangeRate
{
    public class DbSetExchangeRates: AutomationAccountingGoodsEntities, IDbSetModel<ExchangeRates>
    {
        public async Task<ObservableCollection<ExchangeRates>> LoadAsync()
        {
            await ExchangeRates.LoadAsync();
            return ExchangeRates.Local;
        }

        public async Task<ExchangeRates> LoadAsync(string title)
        {
            await ExchangeRates.Where(ex => ex.Title == title).LoadAsync();
            return ExchangeRates.Local.FirstOrDefault();
        }

        public Task AddAsync(ExchangeRates obj)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ExchangeRates obj)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
