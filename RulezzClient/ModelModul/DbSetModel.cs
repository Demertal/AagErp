using System.Threading.Tasks;

namespace ModelModul
{
    public interface IDbSetModel<in T>
    {
        Task AddAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(int objId);
    }
}
