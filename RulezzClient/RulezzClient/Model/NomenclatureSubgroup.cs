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
    public class NomenclatureSubgroup : BindableBase, IObject
    {
        private int _id;

        private string _title;

        private int _idPriceGroup;

        public NomenclatureSubgroup()
        {
            _id = -1;
            _title = "";
            _idPriceGroup = -1;
        }

        public NomenclatureSubgroup(NomenclatureSubgroup nomenclatureSubgroup)
        {
            _id = nomenclatureSubgroup._id;
            _title = nomenclatureSubgroup._title;
            _idPriceGroup = nomenclatureSubgroup._idPriceGroup;
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

        [Column(Name = "id_price_group")]
        public int IdPriceGroup
        {
            get => _idPriceGroup;
            set
            {
                _idPriceGroup = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(NomenclatureSubgroup other)
        {
            if (other == null) return false;
            return _id == other._id && _title == other._title && _idPriceGroup == other._idPriceGroup;
        }

        private static List<NomenclatureSubgroup> Load(string connectionString, int idNomenclatureGroup)
        {
            List<NomenclatureSubgroup> listNomenclatureSubgroup = new List<NomenclatureSubgroup>();
            try
            {
                NomenclatureSubgroupDataContext dc = new NomenclatureSubgroupDataContext(connectionString);
                var nomenclatureSubgroup = dc.Load(idNomenclatureGroup);
                foreach (var ng in nomenclatureSubgroup)
                {
                    listNomenclatureSubgroup.Add(ng);
                }
                return listNomenclatureSubgroup;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<List<NomenclatureSubgroup>> AsyncLoad(string connectionString, int idNomenclatureGroup)
        {
            return await Task.Run(() => Load(connectionString, idNomenclatureGroup));
        }

        private static NomenclatureSubgroup Find(string connectionString, int id)
        {
            NomenclatureSubgroup nm = new NomenclatureSubgroup();
            try
            {
                NomenclatureSubgroupDataContext dc = new NomenclatureSubgroupDataContext(connectionString);
                var nomenclatureSubgroup = dc.Find(id);
                if (nomenclatureSubgroup != null && nomenclatureSubgroup.Count() != 0)
                {
                    nm._id = nomenclatureSubgroup.FirstOrDefault()._id;
                    nm._title = nomenclatureSubgroup.FirstOrDefault()._title;
                    nm._idPriceGroup = nomenclatureSubgroup.FirstOrDefault()._idPriceGroup;
                    return nm;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
        }

        public static async Task<NomenclatureSubgroup> AsyncFind(string connectionString, int id)
        {
            return await Task.Run(() => Find(connectionString, id));
        }
    }

    public class NomenclatureSubgroupDataContext : DataContext
    {
        public NomenclatureSubgroupDataContext(string connectionString)
            : base(connectionString)
        {

        }

        [Function(Name = "FunNomenclatureSubgroup", IsComposable = true)]
        public IQueryable<NomenclatureSubgroup> Find([Parameter(Name = "id", DbType = "int")] int id)
        {
            return CreateMethodCallQuery<NomenclatureSubgroup>(this, (MethodInfo)MethodBase.GetCurrentMethod(), id);
        }

        //Получение наименования и ценовой группы номенклатурной подгруппы по id номенклатурной группы
        [Function(Name = "FunViewNomenclatureSubgroup", IsComposable = true)]
        public IQueryable<NomenclatureSubgroup> Load([Parameter(Name = "id_nomenclature_group", DbType = "int")] int idNomenclatureGroup)
        {
            return CreateMethodCallQuery<NomenclatureSubgroup>(this, (MethodInfo)MethodBase.GetCurrentMethod(), idNomenclatureGroup);
        }
    }
}