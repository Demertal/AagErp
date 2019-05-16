using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ModelModul.RevaluationProduct
{
    public class DbSetRevaluationProducts: DbSetModel<RevaluationProducts>
    {
        public override ObservableCollection<RevaluationProducts> List => null;

        public override async Task AddAsync(RevaluationProducts obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.Products != null)
                        {
                            obj.IdProduct = obj.Products.Id;
                            obj.Products = null;
                        }
                        db.RevaluationProducts.Add(obj);
                        await db.SaveChangesAsync();
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

        public async Task AddAsync(List<RevaluationProducts> obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var revaluationProduct in obj)
                        {
                            if (revaluationProduct.Products != null)
                            {
                                revaluationProduct.IdProduct = revaluationProduct.Products.Id;
                                revaluationProduct.Products = null;
                            }
                            db.RevaluationProducts.Add(revaluationProduct);
                        }
                        await db.SaveChangesAsync();
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

        public override async Task UpdateAsync(RevaluationProducts obj)
        {
            throw new NotImplementedException();
        }

        public override async Task DeleteAsync(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
