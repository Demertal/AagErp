using System;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace ModelModul.Store
{
    public class DbSetStores : IDbSetModel<Stores>
    {
        public ObservableCollection<Stores> Load()
        {
            return new ObservableCollection<Stores>(AutomationAccountingGoodsEntities.GetInstance().Stores);
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
