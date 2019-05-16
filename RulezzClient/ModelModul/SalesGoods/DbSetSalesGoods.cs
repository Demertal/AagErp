using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ModelModul.SalesGoods
{
    class DbSetSalesGoods : DbSetModel<SalesReports>
    {
        public override ObservableCollection<SalesReports> List => null;

        public override async Task AddAsync(SalesReports obj)
        {
            throw new NotImplementedException();
        }

        public override async Task UpdateAsync(SalesReports obj)
        {
            throw new NotImplementedException();
        }

        public override async Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
