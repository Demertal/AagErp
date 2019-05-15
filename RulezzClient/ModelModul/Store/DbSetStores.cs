using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.Store
{
    public class DbSetStores : DbSetModel<Stores>
    {
        public async Task<int> Load()
        {
            List<Stores> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        db.Stores.Load();
                        return db.Stores.Local.ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<Stores> list = new ObservableCollection<Stores>();

            if (temp != null)
            {
                foreach (var item in temp)
                {
                    list.Add(item);
                }
            }

            List = list;
            return List.Count;
        }

        public override void Add(Stores obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(Stores obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
