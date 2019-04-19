using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.UnitStorage
{
    public class DbSetUnitStoragesModel : DbSetModel<UnitStorages>
    {
        public async Task<int> Load()
        {
            List<UnitStorages> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                         return db.UnitStorages.Local.ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<UnitStorages> list = new ObservableCollection<UnitStorages>();

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

        public override void Add(UnitStorages obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(UnitStorages obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
