using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.WarrantyPeriod
{
    public class DbSetWarrantyPeriodsModel : DbSetModel<WarrantyPeriods>
    {
        public async Task<int> Load()
        {
            List<WarrantyPeriods> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        db.WarrantyPeriods.Load();
                        return db.WarrantyPeriods.Local.ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<WarrantyPeriods> list = new ObservableCollection<WarrantyPeriods>();

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

        public override void Add(WarrantyPeriods obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(WarrantyPeriods obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
