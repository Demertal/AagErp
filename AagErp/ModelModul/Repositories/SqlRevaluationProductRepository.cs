using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using ModelModul.Models;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Repositories
{
    public class SqlRevaluationProductRepository: SqlRepository<RevaluationProduct>
    {
        public async Task<IEnumerable<RevaluationProduct>> GetReportRevaluation(CancellationToken cts = new CancellationToken(), ISpecification<RevaluationProduct> where = null, Dictionary<string, SortingTypes> order = null, int skip = 0, int take = -1)
        {
            var reader = await Task.Run(async () =>
            {
                var conn = Db.Database.GetDbConnection();
                await conn.OpenAsync(cts);
                var command = conn.CreateCommand();
                string query = "SELECT r.id idr, r.dateRevaluation, pp.id idp, pp.idProduct, pp.idRevaluation, pp.price, p.title,  " +
                               "ISNULL((SELECT TOP 1 pp2.price FROM dbo.revaluationProducts r2 INNER JOIN dbo.priceProducts pp2 ON(r2.id = pp2.idRevaluation) WHERE r2.dateRevaluation < r.dateRevaluation AND pp2.idProduct = pp.idProduct ORDER BY r2.dateRevaluation DESC), 0) prevPrice " +
                               "FROM dbo.revaluationProducts r " +
                               "INNER JOIN dbo.priceProducts pp ON(r.id = pp.idRevaluation) " +
                               "INNER JOIN dbo.products p ON(pp.idProduct = p.id) " +
                               "ORDER BY r.dateRevaluation DESC";
                command.CommandText = query;
                return command.ExecuteReaderAsync(cts);
            }, cts);
            List<RevaluationProduct> result = new List<RevaluationProduct>();
            RevaluationProduct temp = null;
            while (await reader.Result.ReadAsync(cts))
            {
                if(temp?.Id != reader.Result.GetInt64(0))
                {
                    if(temp != null)
                        result.Add(temp);
                    temp = new RevaluationProduct
                    {
                        Id = reader.Result.GetValueOrDefault<long>("idr"),
                        DateRevaluation = reader.Result.GetValueOrDefault<DateTime>("dateRevaluation")
                    };
                }
                temp.PriceProductsCollection.Add(new PriceProduct
                {
                    Id = reader.Result.GetValueOrDefault<long>("idp"),
                    IdProduct = reader.Result.GetValueOrDefault<long>("idProduct"),
                    IdRevaluation = reader.Result.GetValueOrDefault<long>("idRevaluation"),
                    Price = reader.Result.GetValueOrDefault<decimal>("price"),
                    Product = new Product
                    {
                        Title = reader.Result.GetValueOrDefault<string>("title"),
                        Price = reader.Result.GetValueOrDefault<decimal>("prevPrice")
                    }
                });
            }
            reader.Result.Close();
            if (temp != null)
                result.Add(temp);
            return result;
        }
    }
}
