using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Prism.Mvvm;

namespace RulezzClient
{
    class MMain : BindableBase
    {
        public enum ChoiceUpdate : byte
        {
            Store,
            NomenclatureGroup,
            NomenclatureSubgroup,
            Product,
            UnitStorage,
            WarrantyPeriod
        }

        private readonly Dispatcher _dispatcher;
        private readonly string _connectionString;
        private Visibility _isProductGroupVisible;

        private Store _selectedStore;
        private NomenclatureGroup _selectedNomenclatureGroup;
        private NomenclatureSubgroup _selectedNomenclatureSubgroup;
        private Product _selectedProduct;
        private UnitStorage _selectedUnitStorage;
        private WarrantyPeriod _selectedWarrantyPeriod;

        private readonly ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        private readonly ObservableCollection<NomenclatureGroup> _nomenclatureGroups = new ObservableCollection<NomenclatureGroup>();
        private readonly ObservableCollection<NomenclatureSubgroup> _nomenclatureSubgroups = new ObservableCollection<NomenclatureSubgroup>();
        private readonly ObservableCollection<Product> _products = new ObservableCollection<Product>();
        private readonly ObservableCollection<WarrantyPeriod> _warrantyPeriods = new ObservableCollection<WarrantyPeriod>();
        private readonly ObservableCollection<UnitStorage> _unitStorages = new ObservableCollection<UnitStorage>();

        public readonly ReadOnlyObservableCollection<Store> Stores;
        public readonly ReadOnlyObservableCollection<NomenclatureGroup> NomenclatureGroups;
        public readonly ReadOnlyObservableCollection<NomenclatureSubgroup> NomenclatureSubgroups;
        public readonly ReadOnlyObservableCollection<Product> Products;
        public readonly ReadOnlyObservableCollection<WarrantyPeriod> WarrantyPeriods;
        public readonly ReadOnlyObservableCollection<UnitStorage> UnitStorages;

        public MMain(Dispatcher dispatcher, string connectionString)
        {
            _isProductGroupVisible = Visibility.Collapsed;
            _dispatcher = dispatcher;
            _connectionString = connectionString;
            Stores = new ReadOnlyObservableCollection<Store>(_stores);
            NomenclatureGroups = new ReadOnlyObservableCollection<NomenclatureGroup>(_nomenclatureGroups);
            NomenclatureSubgroups = new ReadOnlyObservableCollection<NomenclatureSubgroup>(_nomenclatureSubgroups);
            Products = new ReadOnlyObservableCollection<Product>(_products);
            WarrantyPeriods = new ReadOnlyObservableCollection<WarrantyPeriod>(_warrantyPeriods);
            UnitStorages = new ReadOnlyObservableCollection<UnitStorage>(_unitStorages);
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
            get => _selectedStore;
            set
            {
                _selectedStore = value;
                RaisePropertyChanged();
                Update(ChoiceUpdate.NomenclatureGroup);
            }
        }

        public NomenclatureGroup SelectedNomenclatureGroup
        {
            get => _selectedNomenclatureGroup;
            set
            {
                _selectedNomenclatureGroup = value;
                RaisePropertyChanged();
                Update(ChoiceUpdate.NomenclatureSubgroup);
            }
        }

        public NomenclatureSubgroup SelectedNomenclatureSubgroup
        {
            get => _selectedNomenclatureSubgroup;
            set
            {
                _selectedNomenclatureSubgroup = value;
                RaisePropertyChanged();
                Update(ChoiceUpdate.Product);
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                RaisePropertyChanged();
            }
        }

        public UnitStorage SelectedUnitStorage
        {
            get => _selectedUnitStorage;
            set
            {
                _selectedUnitStorage = value;
                RaisePropertyChanged();
            }
        }

        public WarrantyPeriod SelectedWarrantyPeriod
        {
            get => _selectedWarrantyPeriod;
            set
            {
                _selectedWarrantyPeriod = value;
                RaisePropertyChanged();
            }
        }

        public void Show(ChoiceUpdate choice)
        {
            Update(choice);
        }

        public void Add(int nomenclatureSubgroup, string name, string itemNum, string barcode, int unitStorage, int warrantyPeriod)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    try
                    {
                        int exchangeRates = 0;
                        ExchangeRatesDataContext db = new ExchangeRatesDataContext(_connectionString);
                        db.FindExchangeRatesId("грн", ref exchangeRates);

                        command.CommandText =
                            "INSERT INTO Product (id_nomenclature_subgroup, title, vendor_code, barcode, id_unit_storage, count, purchase_price, id_exchange_rates, id_warranty, sales_price)" +
                            $" VALUES({nomenclatureSubgroup}, '{name}', '{itemNum}', '{barcode}', {unitStorage}, 0, 0, {exchangeRates}, {warrantyPeriod}, 0)";
                        if (command.ExecuteNonQuery() == 1)
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            MessageBox.Show("Товар добавлен.", "Успех", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("При попытке добавить товар произошла ошибка.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadStore()
        {
            try
            {
                bool check = false;
                _dispatcher.BeginInvoke((Action)(() => _stores.Clear()));
                StoreDataContext db = new StoreDataContext(_connectionString);
                var store = db.Load();
                foreach (var st in store)
                {
                    _dispatcher.BeginInvoke((Action)(() => _stores.Add(st)));
                    check = true;
                }
                if (check)
                    _dispatcher.BeginInvoke((Action)(() => SelectedStore = _stores[0]));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadNomenclatureGroup()
        {
            try
            {
                bool check = false;
                _dispatcher.BeginInvoke((Action)(() => _nomenclatureGroups.Clear()));
                if (SelectedStore == null) return;
                NomenclatureGroupDataContext db = new NomenclatureGroupDataContext(_connectionString);
                var nomenclature = db.Load(SelectedStore.Id);
                foreach (var nom in nomenclature)
                {
                    _dispatcher.BeginInvoke((Action)(() => _nomenclatureGroups.Add(nom)));
                    check = true;
                }
                if (check)
                    _dispatcher.BeginInvoke((Action)(() => SelectedNomenclatureGroup = _nomenclatureGroups[0]));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadNomenclatureSubgroup()
        {
            try
            {
                bool check = false;
                _dispatcher.BeginInvoke((Action)(() => _nomenclatureSubgroups.Clear()));
                if (SelectedNomenclatureGroup == null) return;
                NomenclatureSubgroupDataContext db = new NomenclatureSubgroupDataContext(_connectionString);
                var nomenclature = db.Load(SelectedNomenclatureGroup.Id);
                foreach (var nom in nomenclature)
                {
                    _dispatcher.BeginInvoke((Action)(() => _nomenclatureSubgroups.Add(nom)));
                    check = true;
                }

                if (check)
                    _dispatcher.BeginInvoke(
                        (Action)(() => SelectedNomenclatureSubgroup = _nomenclatureSubgroups[0]));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadProduct()
        {
            try
            {
                _dispatcher.BeginInvoke((Action)(() => _products.Clear()));
                if (SelectedNomenclatureSubgroup == null) return;
                ProductDataContext db = new ProductDataContext(_connectionString);
                var product = db.Load(SelectedNomenclatureSubgroup.Id);
                foreach (var pro in product)
                {
                    _dispatcher.BeginInvoke((Action)(() => _products.Add(pro)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadUnitStorage()
        {
            try
            {
                bool check = false;
                _dispatcher.BeginInvoke((Action)(() => _unitStorages.Clear()));
                UnitStorageDataContext db = new UnitStorageDataContext(_connectionString);
                var unit = db.Load();
                foreach (var un in unit)
                {
                    _dispatcher.BeginInvoke((Action)(() => _unitStorages.Add(un)));
                    check = true;
                }
                if (check)
                    _dispatcher.BeginInvoke((Action)(() => SelectedUnitStorage = _unitStorages[0]));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadWarrantyPeriod()
        {
            try
            {
                bool check = false;
                _dispatcher.BeginInvoke((Action)(() => _warrantyPeriods.Clear()));
                WarrantyPeriodDataContext db = new WarrantyPeriodDataContext(_connectionString);
                var period = db.Load();
                foreach (var per in period)
                {
                    _dispatcher.BeginInvoke((Action)(() => _warrantyPeriods.Add(per)));
                    check = true;
                }
                if (check)
                    _dispatcher.BeginInvoke((Action)(() => SelectedWarrantyPeriod = _warrantyPeriods[0]));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void Update(ChoiceUpdate choice)
        {
            await Task.Run(() =>
            {
                switch (choice)
                {
                    case ChoiceUpdate.Store:
                        LoadStore();
                        break;
                    case ChoiceUpdate.NomenclatureGroup:
                        LoadNomenclatureGroup();
                        break;
                    case ChoiceUpdate.NomenclatureSubgroup:
                        LoadNomenclatureSubgroup();
                        break;
                    case ChoiceUpdate.Product:
                        LoadProduct();
                        break;
                    case ChoiceUpdate.UnitStorage:
                        LoadUnitStorage();
                        break;
                    case ChoiceUpdate.WarrantyPeriod:
                        LoadWarrantyPeriod();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
                }
            });
        }

        public static MMain Copy(MMain copy)
        {
            MMain result = new MMain(copy._dispatcher, copy._connectionString)
            {
                SelectedStore = copy.SelectedStore,
                SelectedNomenclatureSubgroup = copy.SelectedNomenclatureSubgroup,
                SelectedNomenclatureGroup = copy.SelectedNomenclatureGroup,
                SelectedProduct = copy.SelectedProduct
            };
            return result;
        }
    }
}
