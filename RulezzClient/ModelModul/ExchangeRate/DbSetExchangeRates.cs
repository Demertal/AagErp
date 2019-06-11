using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModelModul.ExchangeRate
{
    public class DbSetExchangeRates: IDbSetModel<ExchangeRates>
    {
        public ObservableCollection<ExchangeRates> Load()
        {
            return new ObservableCollection<ExchangeRates>(AutomationAccountingGoodsEntities.GetInstance().ExchangeRates
                .ToList());
        }

        public ExchangeRates Load(string title)
        {
            return AutomationAccountingGoodsEntities.GetInstance().ExchangeRates
                .FirstOrDefault(ex => ex.Title == title);
        }

        public void Add(ExchangeRates obj)
        {
            throw new NotImplementedException();
        }

        public void Update(ExchangeRates obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
