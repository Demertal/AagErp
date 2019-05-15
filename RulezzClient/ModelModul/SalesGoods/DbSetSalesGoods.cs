using System;
using System.Collections.ObjectModel;

namespace ModelModul.SalesGoods
{
    class DbSetSalesGoods : DbSetModel<SalesReports>
    {
        public override ObservableCollection<SalesReports> List => null;

        public override void Add(SalesReports obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(SalesReports obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
