using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace RulezzClient
{
    public class ExchangeRates
    {
        [Column(Name = "currency")]
        public string Currency { get; set; }

        [Column(Name = "quotation")]
        public double Quotation { get; set; }
    }

    public class ExchangeRatesDataContext : DataContext
    {
        public ExchangeRatesDataContext(string connectionString)
            : base(connectionString)
        {

        }
        //Получение id валюты по названию валюты
        [Function(Name = "FindExchangeRatesId")]
        [return: Parameter(DbType = "int")]
        public int FindExchangeRatesId(
            [Parameter(Name = "currency", DbType = "nvarchar(10)")] string currency,
            [Parameter(Name = "id", DbType = "int")] ref int id)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), currency, id);
            if (result == null) return -1;
            id = (int)result.GetParameterValue(1);
            return (int)result.ReturnValue;
        }

        //Получение валюты по id
        [Function(Name = "FunViewExchangeRates", IsComposable = true)]
        public IQueryable<ExchangeRates> Load()
        {
            return CreateMethodCallQuery<ExchangeRates>(this, (MethodInfo)MethodBase.GetCurrentMethod());
        }
    }
}
