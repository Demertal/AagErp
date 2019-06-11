using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.SerialNumber
{
    public class DbSetSerialNumbers: IDbSetModel<SerialNumbers>
    {
        public ObservableCollection<SerialNumbers> Load(string findString = null)
        {
            if (string.IsNullOrEmpty(findString))
            {
                return new ObservableCollection<SerialNumbers>(AutomationAccountingGoodsEntities.GetInstance().SerialNumbers.Include(objSer => objSer.Counterparties));
            }

            if (AutomationAccountingGoodsEntities.GetInstance().SerialNumbers.Count(obj => obj.Value.Contains(findString)) != 0)
            {
                return new ObservableCollection<SerialNumbers>(AutomationAccountingGoodsEntities.GetInstance()
                    .SerialNumbers.Where(obj => obj.Value.Contains(findString)).Include(objSer => objSer.Counterparties)
                    .OrderBy(obj => obj.PurchaseDate).Include(objSer => objSer.Products)
                    .OrderBy(obj => obj.PurchaseDate));
            }

            ObservableCollection<SerialNumbers> temp = new ObservableCollection<SerialNumbers>();

            AutomationAccountingGoodsEntities.GetInstance().Products
                .Where(obj =>
                    obj.Title.Contains(findString) || obj.Barcode.Contains(findString) ||
                    obj.VendorCode.Contains(findString)).Include(obj => obj.SerialNumbers).ToList()
                .ForEach(obj => temp.AddRange(obj.SerialNumbers));
            
            return temp;
        }

        public ObservableCollection<SerialNumbers> FindFreeSerialNumbers(string value)
        {
            return new ObservableCollection<SerialNumbers>(AutomationAccountingGoodsEntities.GetInstance().SerialNumbers
                .Where(obj => obj.Value.Contains(value) && obj.SelleDate == null).OrderBy(obj => obj.PurchaseDate));
        }

        public ObservableCollection<SerialNumbers> FindFreeSerialNumbers(string value, int idProduct)
        {
            return new ObservableCollection<SerialNumbers>(AutomationAccountingGoodsEntities.GetInstance().SerialNumbers
                .Where(obj => obj.Value.Contains(value) && obj.SelleDate == null && obj.IdProduct == idProduct)
                .OrderBy(obj => obj.PurchaseDate));
        }

        public void Add(SerialNumbers obj)
        {
            throw new NotImplementedException();
        }

        public void Update(SerialNumbers obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
