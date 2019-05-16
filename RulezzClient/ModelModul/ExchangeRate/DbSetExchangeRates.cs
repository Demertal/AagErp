using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModelModul.ExchangeRate
{
    public class DbSetExchangeRates: DbSetModel<ExchangeRates>
    {
        public async Task LoadAsync()
        {
            using (StoreEntities db = new StoreEntities())
            {
                await db.ExchangeRates.LoadAsync();
                List = db.ExchangeRates.Local;
            }
        }

        public override Task AddAsync(ExchangeRates obj)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(ExchangeRates obj)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
