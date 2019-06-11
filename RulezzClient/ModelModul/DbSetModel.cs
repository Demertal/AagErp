namespace ModelModul
{
    public interface IDbSetModel<in T>
    {
        void Add(T obj);
        void Update(T obj);
        void Delete(int objId);
    }
}
