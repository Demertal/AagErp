using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;


namespace RulezzClient
{
    class MainViewVm : BindableBase
    {
        private readonly MMain _model;
        private Visibility _isProductGroupVisible;

        public ReadOnlyObservableCollection<Store> Stores => _model.Stores;
        public ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups => _model.NomenclatureGroups;
        public ReadOnlyObservableCollection<NomenclatureSubgroup> NomenclatureSubgroups => _model.NomenclatureSubgroups;
        public ReadOnlyObservableCollection<Product> Products => _model.Products;

        public MainViewVm(Dispatcher dis, string connectionString)
        {
            _isProductGroupVisible = Visibility.Collapsed;
            _model = new MMain(dis, connectionString);
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            ShowProductCommand = new DelegateCommand(() =>
            {
                IsProductGroupVisible = Visibility.Visible;
                _model.Show(MMain.ChoiceUpdate.Store);

            });
            AddProductMainCommand = new DelegateCommand(() =>
            {
                AddProduct ad = new AddProduct { DataContext = new VmAddProduct(_model), Title = "Добавить товар" };
                ad.ShowDialog();
            });
        }

        public Visibility IsProductGroupVisible
        {
            get => _isProductGroupVisible;
            set
            {
                _isProductGroupVisible = value;
                RaisePropertyChanged();
            }
        }

        public Store SelectedStore
        {
            get => _model.SelectedStore;
            set => _model.SelectedStore = value;
        }

        public NomenclatureGroup SelectedNomenclatureGroup
        {
            get => _model.SelectedNomenclatureGroup;
            set => _model.SelectedNomenclatureGroup = value;
        }
        
        public NomenclatureSubgroup SelectedNomenclatureSubgroup
        {
            get => _model.SelectedNomenclatureSubgroup;
            set => _model.SelectedNomenclatureSubgroup = value;
        }

        public Product SelectedProduct
        {
            get => _model.SelectedProduct;
            set => _model.SelectedProduct = value;
        }

        public DelegateCommand ShowProductCommand { get; }

        public DelegateCommand AddProductMainCommand { get; }
    }
}