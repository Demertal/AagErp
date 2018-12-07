using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace RulezzClient
{
    public abstract class Model<T>
    {
        protected readonly Dispatcher Dispatcher;
        protected readonly MyDataContext<T> DataContext;

        protected readonly ObservableCollection<T> Collection = new ObservableCollection<T>();
        public readonly ReadOnlyObservableCollection<T> ReadCollection;

        protected Model(Dispatcher dispatcher, MyDataContext<T> dataContext)
        {
            Dispatcher = dispatcher;
            DataContext = dataContext;
            ReadCollection = new ReadOnlyObservableCollection<T>(Collection);
        }

        protected void Load()
        {
            try
            {
                Dispatcher.BeginInvoke((Action)(() => Collection.Clear()));
                var store = DataContext.Load();
                foreach (var st in store)
                {
                    Dispatcher.BeginInvoke((Action)(() => Collection.Add(st)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        public void Update()
        {
            Load();
        }
    }
}
