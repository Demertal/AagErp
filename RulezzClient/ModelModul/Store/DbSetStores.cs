using System;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace ModelModul.Store
{
    public class DbSetStores : IDbSetModel<Stores>
    {
        public ObservableCollection<Stores> Load()
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return new ObservableCollection<Stores>(db.Stores);
            }
        }

        public void Add(Stores obj)
        {
            throw new NotImplementedException();
        }

        public void Update(Stores obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
