using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ModelModul
{
    public abstract class DbSetModel<T>
    {
        private ObservableCollection<T> _list = new ObservableCollection<T>();

        public virtual ObservableCollection<T> List
        {
            get => _list;
            set => _list = value;
        }

        public abstract Task AddAsync(T obj);
        public abstract Task UpdateAsync(T obj);
        public abstract Task DeleteAsync(int objId);
    }
}
