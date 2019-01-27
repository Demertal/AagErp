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
    public class WarrantyPeriod : BindableBase
    {
        private string _period;

        private int _id;

        public WarrantyPeriod()
        {
            Period = "";
            Id = -1;
        }

        public WarrantyPeriod(WarrantyPeriod warrantyPeriod)
        {
            Period = warrantyPeriod._period;
            Id = warrantyPeriod._id;
        }

        [Column(Name = "per")]
        public string Period
        {
            get => _period;
            set
            {
                _period = value;
                RaisePropertyChanged();
            }
        }

        [Column(Name = "id")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(WarrantyPeriod other)
        {
            if (other == null) return false;
            return Id == other.Id && Period == other.Period;
        }

        private static List<WarrantyPeriod> Load(string connectionString)
        {
            List<WarrantyPeriod> listStore = new List<WarrantyPeriod>();
            try
            {
                WarrantyPeriodDataContext dc = new WarrantyPeriodDataContext(connectionString);
                var warrantyPeriods = dc.Load();
                foreach (var warranty in warrantyPeriods)
                {
                    listStore.Add(warranty);
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

        public static async Task<List<WarrantyPeriod>> AsyncLoad(string connectionString)
        {
            return await Task.Run(() => Load(connectionString));
        }
    }

    public class WarrantyPeriodDataContext : DataContext
    {
        public WarrantyPeriodDataContext(string connectionString)
            : base(connectionString)
        {

        }
        //Получение периода
        [Function(Name = "FunViewWarrantyPeriod", IsComposable = true)]
        public IQueryable<WarrantyPeriod> Load()
        {
            return CreateMethodCallQuery<WarrantyPeriod>(this, (MethodInfo)MethodBase.GetCurrentMethod());
        }
    }
}