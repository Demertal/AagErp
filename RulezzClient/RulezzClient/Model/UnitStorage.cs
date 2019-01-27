using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;

namespace RulezzClient.Model
{
    public class UnitStorage : BindableBase, IObject
    {
        private string _title;

        private int _id;

        public UnitStorage()
        {
            Title = "";
            Id = -1;
        }

        public UnitStorage(UnitStorage unitStorage)
        {
            Title = unitStorage.Title;
            Id = unitStorage.Id;
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(UnitStorage other)
        {
            if (other == null) return false;
            return Id == other.Id && Title == other.Title;
        }

        private static List<UnitStorage> Load(string connectionString)
        {
            List<UnitStorage> listUnitStorages = new List<UnitStorage>();
            try
            {
                UnitStorageDataContext dc = new UnitStorageDataContext(connectionString);
                var unitStorages = dc.Load();
                foreach (var unit in unitStorages)
                {
                    listUnitStorages.Add(unit);
                }
                return listUnitStorages;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<List<UnitStorage>> AsyncLoad(string connectionString)
        {
            return await Task.Run(() => Load(connectionString));
        }
    }

    public class UnitStorageDataContext : DataContext
    {
        public UnitStorageDataContext(string connectionString)
            : base(connectionString)
        {

        }

        [Function(Name = "FunViewUnitStorage", IsComposable = true)]
        public IQueryable<UnitStorage> Load()
        {
            return CreateMethodCallQuery<UnitStorage>(this, (MethodInfo)MethodBase.GetCurrentMethod());
        }
    }
}
