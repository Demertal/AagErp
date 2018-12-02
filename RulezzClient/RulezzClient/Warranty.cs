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

        public void GetWarrantyPeriodId(string period, ref int id)
        {
            int setPer = period == "Нет" ? 0 : int.Parse(period);
            FindWarrantyPeriodId(setPer, ref id);
        }

        //Получение id гарантии по названию периода
        [Function(Name = "FindWarrantyPeriodId")]
        [return: Parameter(DbType = "int")]
        public int FindWarrantyPeriodId(
            [Parameter(Name = "period", DbType = "int")] int period,
            [Parameter(Name = "id", DbType = "int")] ref int id)
        {
            IExecuteResult result = ExecuteMethodCall(this, (MethodInfo)MethodBase.GetCurrentMethod(), period, id);
            if (result == null) return -1;
            id = (int)result.GetParameterValue(1);
            return (int)result.ReturnValue;
        }

        //Получение периода
        [Function(Name = "FunViewWarrantyPeriod", IsComposable = true)]
        public IQueryable<WarrantyPeriod> GetListWarrantyPeriod([Parameter(Name = "id_war", DbType = "int")] int idWar)
        {
            return CreateMethodCallQuery<WarrantyPeriod>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idWar);
        }
    }
}