using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.RevaluationProduct
{
    public class DbSetRevaluationProducts: IDbSetModel<RevaluationProductsReports>
    {
        public ObservableCollection<RevaluationProductsReports> Load(int start, int end)
        {
            return new ObservableCollection<RevaluationProductsReports>(AutomationAccountingGoodsEntities.GetInstance()
                .RevaluationProductsReports.OrderByDescending(obj => obj.DataRevaluation)
                .ThenByDescending(obj => obj.Id).Skip(start).Take(end).Include(obj => obj.RevaluationProductsInfos));
        }

        public int GetCount()
        {
            return AutomationAccountingGoodsEntities.GetInstance().RevaluationProductsReports.Count();
        }

        public void Add(RevaluationProductsReports obj)
        {
            using (var transaction = AutomationAccountingGoodsEntities.GetInstance().Database.BeginTransaction())
            {
                try
                {
                    foreach (var revaluationProductsInfo in obj.RevaluationProductsInfos)
                    {
                        if (revaluationProductsInfo.Products != null)
                        {
                            revaluationProductsInfo.IdProduct = revaluationProductsInfo.Products.Id;
                            revaluationProductsInfo.Products = null;
                        }
                    }
                    AutomationAccountingGoodsEntities.GetInstance().RevaluationProductsReports.Add(obj);
                    AutomationAccountingGoodsEntities.GetInstance().SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Update(RevaluationProductsReports obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
