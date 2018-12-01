using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RulezzClient
{
    public class WarrantyPeriod
    {
        [Column(Name = "per")]
        public string Period { get; set; }
    }

    public class WarrantyPeriodDataContext : DataContext
    {
        public WarrantyPeriodDataContext(string connectionString)
            : base(connectionString)
        {

        }
        //Получение продукта по id номенклатурной группы
        [Function(Name = "FindWarrantyPeriod")]
        [return: Parameter(DbType = "nvarchar")]
        public string GetWarrantyPeriod(
            [Parameter(Name = "id", DbType = "int")] int id,
            [Parameter(Name = "period", DbType = "nvarchar(4)")] ref string period)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), id, period);
            if (result == null) return "";
            period = (string)result.GetParameterValue(2);
            return (string)result.ReturnValue;
        }

        //Получение периода
        [Function(Name = "FunViewWarrantyPeriod", IsComposable = true)]
        public IQueryable<WarrantyPeriod> GetListWarrantyPeriod()
        {
            return CreateMethodCallQuery<WarrantyPeriod>(this, (MethodInfo)MethodBase.GetCurrentMethod());
        }
    }
}