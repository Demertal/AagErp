using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModelModul.Store
{
    public class DbSetStores : AutomationAccountingGoodsEntities, IDbSetModel<Stores>
    {
        public async Task<ObservableCollection<Stores>> LoadAsync()
        {
            await Stores.LoadAsync();
            return Stores.Local;
        }

        public async Task AddAsync(Stores obj)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Stores obj)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
