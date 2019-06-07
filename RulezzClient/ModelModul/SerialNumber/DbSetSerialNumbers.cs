using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.SerialNumber
{
    public class DbSetSerialNumbers: AutomationAccountingGoodsEntities, IDbSetModel<SerialNumbers>
    {
        public async Task<ObservableCollection<SerialNumbers>>LoadAsync(string findString = null)
        {
            if (string.IsNullOrEmpty(findString))
            {
                await SerialNumbers.Include(objSer => objSer.Counterparties).LoadAsync();
                return SerialNumbers.Local;
            }

            await SerialNumbers.Where(obj => obj.Value.Contains(findString)).Include(objSer => objSer.Counterparties)
                .OrderBy(obj => obj.PurchaseDate).Include(objSer => objSer.Products).OrderBy(obj => obj.PurchaseDate)
                .LoadAsync();
            if (SerialNumbers.Local.Count != 0) return SerialNumbers.Local;
            await Products.Where(obj =>
                obj.Title.Contains(findString) || obj.Barcode.Contains(findString) ||
                obj.VendorCode.Contains(findString)).Include(obj => obj.SerialNumbers).LoadAsync();
            ObservableCollection<SerialNumbers> temp = new ObservableCollection<SerialNumbers>();
            foreach (var prod in Products.Local)
            {
                temp.AddRange(prod.SerialNumbers);
            }
            return temp;
        }

        public async Task<ObservableCollection<SerialNumbers>> FindFreeSerialNumbersAsync(string value)
        {
            await SerialNumbers.Where(obj => obj.Value.Contains(value) && obj.SelleDate == null).OrderBy(obj => obj.PurchaseDate).LoadAsync();
            return SerialNumbers.Local;
        }

        public async Task<ObservableCollection<SerialNumbers>> FindFreeSerialNumbersAsync(string value, int idProduct)
        {
            await SerialNumbers.Where(obj => obj.Value.Contains(value) && obj.SelleDate == null && obj.IdProduct == idProduct).OrderBy(obj => obj.PurchaseDate).LoadAsync();
            return SerialNumbers.Local;
        }

        public Task AddAsync(SerialNumbers obj)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SerialNumbers obj)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
