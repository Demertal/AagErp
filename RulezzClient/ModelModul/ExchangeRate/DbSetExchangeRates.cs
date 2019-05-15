using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.ExchangeRate
{
    public class DbSetExchangeRates: DbSetModel<ExchangeRates>
    {
        public async Task<int> Load()
        {
            List<ExchangeRates> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        db.ExchangeRates.Load();
                        return db.ExchangeRates.Local.ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<ExchangeRates> list = new ObservableCollection<ExchangeRates>();

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

        public override void Add(ExchangeRates obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(ExchangeRates obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
