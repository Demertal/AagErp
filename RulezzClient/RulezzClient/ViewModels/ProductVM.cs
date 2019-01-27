using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Mvvm;
using RulezzClient.Model;

namespace RulezzClient.ViewModels
{
    class ProductListVm : BindableBase
    {
        private readonly ObservableCollection<Product> _productList =
            new ObservableCollection<Product>();

        public ReadOnlyObservableCollection<Product> Products;

        public ProductListVm()
        {
            Products = new ReadOnlyObservableCollection<Product>(_productList);
        }

        public async Task<List<Product>> GetListProduct(int idNomenclatureSubgroup)
        {
            List<Product> tempM =
                await Task.Run(() => Product.AsyncLoad(Properties.Settings.Default.СconnectionString, idNomenclatureSubgroup));
            if (tempM == null){ _productList.Clear(); return null;}
            _productList.Clear();
            foreach (var t in tempM)
            {
                _productList.Add(t);
            }
            return tempM;
        }
    }
}
