using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ModelModul.RevaluationProduct
{
    public class DbSetRevaluationProducts: IDbSetModel<RevaluationProductsReports>
    {
        public ObservableCollection<RevaluationProductsReports> Load(int start, int end)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                List<RevaluationProductsReports> temp = new List<RevaluationProductsReports>(db.RevaluationProductsReports
                    .OrderByDescending(obj => obj.DataRevaluation).ThenByDescending(obj => obj.Id).Skip(start).Take(end)
                    .Include(obj => obj.RevaluationProductsInfos).ToList());
                temp.ForEach(obj => obj.RevaluationProductsInfos.ToList().ForEach(objInfo => db.Entry(objInfo).Reference(objInf => objInf.Products).Load()));

                return new ObservableCollection<RevaluationProductsReports>(temp);
            }
        }

        public int GetCount()
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                return db.RevaluationProductsReports.Count();
            }
        }

        public void Add(RevaluationProductsReports obj)
        {
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var revaluationProductsInfo in obj.RevaluationProductsInfos)
                        {
                            if (revaluationProductsInfo.Products == null) continue;
                            revaluationProductsInfo.IdProduct = revaluationProductsInfo.Products.Id;
                            revaluationProductsInfo.Products = null;
                        }

                        db.RevaluationProductsReports.Add(obj);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
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
