using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModelModul.RevaluationProduct
{
    public class DbSetRevaluationProducts: DbSetModel<RevaluationProducts>
    {
        public override ObservableCollection<RevaluationProducts> List => null;

        public override void Add(RevaluationProducts obj)
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

        public void Add(List<RevaluationProducts> obj)
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

        public override void Update(RevaluationProducts obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int objId)
        {
            throw new NotImplementedException();
        }
    }
}
