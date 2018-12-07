using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace RulezzClient
{
    public class Store : IObject
    {
        public string Title { get; set; }

        public int Id { get; set; }
    }

    public class StoreDataContext : DataContext
    {
        public StoreDataContext(string connectionString)
            : base(connectionString)
        {

        }

        [Function(Name = "FunViewStore", IsComposable = true)]
        public IQueryable<Store> Load()
        {
            return CreateMethodCallQuery<Store>(this, (MethodInfo) MethodBase.GetCurrentMethod());
        }
    }

}
