using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;


namespace RulezzClient
{
    class ViewModel : INotifyPropertyChanged
    {
        public string ConnectionString;
        private readonly Dispatcher _dispatcher;

        private NomenclatureGroup _selectedNomenclatureGroup;
        private NomenclatureSubgroup _selectedNomenclatureSubgroup;
        private Product _selectedProduct;

        public ObservableCollection<NomenclatureGroup> NomenclatureGroups { get; set; }
        public ObservableCollection<NomenclatureSubgroup> NomenclatureSubgroups { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        public ViewModel(Dispatcher dis)
        {
            _dispatcher = dis;
            NomenclatureGroups = new ObservableCollection<NomenclatureGroup>();
            NomenclatureSubgroups = new ObservableCollection<NomenclatureSubgroup>();
            Products = new ObservableCollection<Product>();
            ConnectionString =
                $"Data Source=;Initial Catalog=Rul_base;Integrated Security=true";
        }

        public NomenclatureGroup SelectedNomenclatureGroup
        {
            get => _selectedNomenclatureGroup;
            set
            {
                _selectedNomenclatureGroup = value;
                OnPropertyChanged();
                if (_selectedNomenclatureGroup != null)
                    ShowNomenclatureSubgroupFunction();
            }
        }
        
        public NomenclatureSubgroup SelectedNomenclatureSubgroup
        {
            get => _selectedNomenclatureSubgroup;
            set
            {
                _selectedNomenclatureSubgroup = value;
                OnPropertyChanged();
                if (_selectedNomenclatureSubgroup != null)
                    ShowProductFunction();
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private async void ShowNomenclatureSubgroupFunction()
        {
            await Task.Run(() =>
            {
                try
                {
                    bool check = false;
                    _dispatcher.BeginInvoke((Action) (() => NomenclatureSubgroups.Clear()));
                    NomenclatureSubgroupDataContext db = new NomenclatureSubgroupDataContext(ConnectionString);
                    var nomenclature = db.GetNomenclatureSubgroup(SelectedNomenclatureGroup.Id);
                    foreach (var nom in nomenclature)
                    {
                        _dispatcher.BeginInvoke((Action) (() => NomenclatureSubgroups.Add(nom)));
                        check = true;
                    }

                    if (check)
                        _dispatcher.BeginInvoke(
                            (Action) (() => SelectedNomenclatureSubgroup = NomenclatureSubgroups[0]));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
        }

        private async void ShowProductFunction()
        {
            await Task.Run(() =>
            {
                try
                {
                    _dispatcher.BeginInvoke((Action)(() => Products.Clear()));
                    ProductDataContext db = new ProductDataContext(ConnectionString);
                    var product = db.GetListProduct(SelectedNomenclatureSubgroup.Id, -1);
                    foreach (var pro in product)
                    {
                        _dispatcher.BeginInvoke((Action)(() => Products.Add(pro)));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
        }

    }
}