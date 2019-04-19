using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace ModelModul
{
    public abstract class DbSetModel<T> : BindableBase
    {
        #region Properties

        private ObservableCollection<T> _list = new ObservableCollection<T>();

        public ObservableCollection<T> List
        {
            get => _list;
            set => SetProperty(ref _list, value);
        }

        #endregion

        protected DbSetModel()
        {
            List = new ObservableCollection<T>();
        }

        public abstract void Add(T obj);
        public abstract void Update(T obj);
        public abstract void Delete(int objId);
    }
}
