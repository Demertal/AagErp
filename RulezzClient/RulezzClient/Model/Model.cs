using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RulezzClient.Model
{
    public abstract class Model<T>
    {
        protected readonly Dispatcher Dispatcher;
        protected readonly string ConnectionString;

        protected readonly ObservableCollection<T> Collection = new ObservableCollection<T>();
        private readonly ReadOnlyObservableCollection<T> _readCollection;

        protected Model(Dispatcher dispatcher, string connectionString)
        {
            Dispatcher = dispatcher;
            ConnectionString = connectionString;
            _readCollection = new ReadOnlyObservableCollection<T>(Collection);
        }

        public ReadOnlyObservableCollection<T> ReadCollection
        {
            get
            {
                Load();
                return _readCollection;
            }
        }

        public NotifyCollectionChangedEventHandler CollectionChanged
        {
            set => Collection.CollectionChanged += value;
        }

        protected abstract void Load();

        public async void Update(int id = -1)
        {
            await Task.Run(() => Load());
        }
    }
}
