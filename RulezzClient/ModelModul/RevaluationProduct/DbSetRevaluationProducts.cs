using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.RevaluationProduct
{
    public class DbSetRevaluationProducts: AutomationAccountingGoodsEntities, IDbSetModel<RevaluationProductsReports>
    {
        public async Task<ObservableCollection<RevaluationProductsReports>> LoadAsync(int start, int end)
        {
            await RevaluationProductsReports.OrderByDescending(obj => obj.DataRevaluation)
                .ThenByDescending(obj => obj.Id).Skip(start).Take(end).Include(obj => obj.RevaluationProductsInfos)
                .LoadAsync();
            return RevaluationProductsReports.Local;
        }

        public async Task<int> GetCount()
        {
            return await RevaluationProductsReports.CountAsync();
        }

        public async Task AddAsync(RevaluationProductsReports obj)
        {
            using (var transaction = Database.BeginTransaction())
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
                    RevaluationProductsReports.Add(obj);
                    await SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdateAsync(RevaluationProductsReports obj)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
