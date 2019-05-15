using System.Collections.ObjectModel;

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

        public abstract void Add(T obj);
        public abstract void Update(T obj);
        public abstract void Delete(int objId);
    }
}
