using System;
using System.Threading.Tasks;
using ModelModul.Models;

namespace ModelModul.Repositories
{
    public class SqlSerialNumberRepository: SqlRepository<SerialNumber>
    {
        public override Task UpdateAsync(SerialNumber obj)
        {
            throw new NotImplementedException();
        }

        //public override Task DeleteAsync(SerialNumber item)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
