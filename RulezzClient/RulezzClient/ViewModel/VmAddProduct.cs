using Prism.Mvvm;
using RulezzClient.Model;

namespace RulezzClient
{
    class VmAddProduct : BindableBase
    {
        private readonly MMain _model;

        //public ReadOnlyObservableCollection<WarrantyPeriod> WarrantyPeriods => _model.WarrantyPeriods;
        //public ReadOnlyObservableCollection<UnitStorage> UnitStorages => _model.UnitStorages;
        //public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroupsList => _model.NomenclatureGroupsList;
        //public ReadOnlyObservableCollection<NomenclatureSubgroup> NomenclatureSubgroups => _model.NomenclatureSubgroups;

        public VmAddProduct(MMain model)
        {
            //_model = MMain.Copy(model);
            //_model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            //_model.Show(MMain.ChoiceUpdate.UnitStorage);
            //_model.Show(MMain.ChoiceUpdate.WarrantyPeriod);
            //AddProductommand = new DelegateCommand<string>(str => {
            //    AddProduct ad = new AddProduct { DataContext = new VmAddProduct(_model), Title = "Добавить товар" };
            //    ad.ShowDialog();
            //});
        }

        //public UnitStorage SelectedUnitStorage
        //{
        //    get => _model.SelectedUnitStorage;
        //    set => _model.SelectedUnitStorage = value;
        //}

        //public WarrantyPeriod SelectedWarrantyPeriod
        //{
        //    get => _model.SelectedWarrantyPeriod;
        //    set => _model.SelectedWarrantyPeriod = value;
        //}

        //public NomenclatureGroup SelectedNomenclatureGroup
        //{
        //    get => _model.SelectedNomenclatureGroup;
        //    set => _model.SelectedNomenclatureGroup = value;
        //}

        //public NomenclatureSubgroup SelectedNomenclatureSubgroup
        //{
        //    get => _model.SelectedNomenclatureSubgroup;
        //    set => _model.SelectedNomenclatureSubgroup = value;
        //}

        //public DelegateCommand<string> AddProductommand { get; }
    }
}
