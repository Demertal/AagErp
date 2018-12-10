using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace RulezzClient
{
    public class Store : IObject, IEquatable<Store>
    {
        public string Title { get; set; }

        public int Id { get; set; }

        public bool Equals(Store other)
        {
            if (other == null) return false;
            return Id == other.Id && Title == other.Title;
        }

        private static List<Store> Load(string connectionString)
        {
            List<Store> listStore = new List<Store>();
            try
            {
                StoreDataContext dc = new StoreDataContext(connectionString);
                var store = dc.Load();
                foreach (var st in store)
                {
                    listStore.Add(st);
                }
                return listStore;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<List<Store>> AsyncLoad(string connectionString)
        {
            return await Task.Run(() => Load(connectionString));
        }
    }

    public class StoreDataContext : DataContext
    {
        public StoreDataContext(string connectionString)
            : base(connectionString)
        {

        }

        [Function(Name = "FunViewStore", IsComposable = true)]
        public IQueryable<Store> Load()
        {
            return CreateMethodCallQuery<Store>(this, (MethodInfo) MethodBase.GetCurrentMethod());
        }
    }

}
