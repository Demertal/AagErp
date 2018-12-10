using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace RulezzClient.Model
{
    public class UnitStorage : IObject
    {
        public string Title { get; set; }

        public int Id { get; set; }
    }

    public class UnitStorageDataContext : DataContext
    {
        public UnitStorageDataContext(string connectionString)
            : base(connectionString)
        {

        }

        [Function(Name = "FunViewUnitStorage", IsComposable = true)]
        public IQueryable<UnitStorage> Load()
        {
            return CreateMethodCallQuery<UnitStorage>(this, (MethodInfo)MethodBase.GetCurrentMethod());
        }
    }
}
