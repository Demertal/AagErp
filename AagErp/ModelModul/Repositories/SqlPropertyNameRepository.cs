using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlPropertyNameRepository : SqlRepository<PropertyName>
    {
        public async Task<bool> CheckProperty(int idProperty, int? idCategory, string title, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return Db.MovmentGoodTypes.Take(1)
                    .Select(p => AutomationAccountingGoodsContext.CheckProperty(idProperty, idCategory, title))
                    .SingleOrDefaultAsync(cts);
            }, cts);
        }
    }
}
