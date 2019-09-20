using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlSerialNumberRepository: SqlRepository<SerialNumber>
    {
        public async Task<List<long>> GetFreeSerialNumbers(long idProduct, string value, int idStore, CancellationToken cts = new CancellationToken())
        {
            return await Task.Run(() =>
            {
                cts.ThrowIfCancellationRequested();
                return Db.SerialNumbers
                    .FromSql("select * from getFreeSerialNumbers({0}, {1}, {2})", idProduct, value, idStore)
                    .Select(s => s.Id).ToListAsync(cts);
            }, cts);
        }
    }
}
