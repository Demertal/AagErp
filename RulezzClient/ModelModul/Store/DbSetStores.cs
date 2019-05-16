using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ModelModul.Store
{
    public class DbSetStores : DbSetModel<Stores>
    {
        public async Task LoadAsync()
        {
            using (StoreEntities db = new StoreEntities())
            {
                await db.Stores.LoadAsync();
                List = db.Stores.Local;
            }
        }

        public override async Task AddAsync(Stores obj)
        {
            throw new NotImplementedException();
        }

        public override async Task UpdateAsync(Stores obj)
        {
            throw new NotImplementedException();
        }

        public override async Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
