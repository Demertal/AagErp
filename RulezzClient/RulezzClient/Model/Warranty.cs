using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace RulezzClient.Model
{
    public class WarrantyPeriod
    {
        [Column(Name = "per")]
        public string Period { get; set; }

        [Column(Name = "id")]
        public int Id { get; set; }
    }

    public class WarrantyPeriodDataContext : DataContext
    {
        public WarrantyPeriodDataContext(string connectionString)
            : base(connectionString)
        {

        }
        //Получение периода
        [Function(Name = "FunViewWarrantyPeriod", IsComposable = true)]
        public IQueryable<WarrantyPeriod> Load()
        {
            return CreateMethodCallQuery<WarrantyPeriod>(this, (MethodInfo)MethodBase.GetCurrentMethod());
        }
    }
}