using Prism.Mvvm;

namespace RulezzClient.Model
{
    class MMain : BindableBase
    {
    }
    //    public enum ChoiceUpdate : byte
    //    {
    //        Store,
    //        NomenclatureGroup,
    //        NomenclatureSubgroup,
    //        Product
    //    }

    //    private readonly string _connectionString;
        
    //    private readonly StoreList _storesList;
    //    private readonly NomenclatureGroupList _nomenclatureGroupsList;
    //    private readonly NomenclatureSubgroup _nomenclatureSubgroups;
    //    private readonly MProduct _products;

    //    private Store _selectedStore;
    //    private NomenclatureGroup _selectedNomenclatureGroup;
    //    private NomenclatureSubgroup _selectedNomenclatureSubgroup;
    //    private Product _selectedProduct;

    //    public StoreList StoresList => _storesList;
    //    public NomenclatureSubgroup NomenclatureSubgroups => _nomenclatureSubgroups;
    //    public NomenclatureGroupList NomenclatureGroupsList => _nomenclatureGroupsList;
    //    public MProduct Products => _products;

    //    public MMain(Dispatcher dispatcher, string connectionString)
    //    {
    //        _connectionString = connectionString;
    //        _storesList = new StoreList(dispatcher, connectionString);
    //        _nomenclatureGroupsList = new NomenclatureGroupList(dispatcher, connectionString);
    //        _nomenclatureGroupsList.CollectionChanged = NomenclatureGroups_CollectionChanged;
    //        _nomenclatureSubgroups = new NomenclatureSubgroup(dispatcher, connectionString);
    //        _nomenclatureSubgroups.CollectionChanged = NomenclatureSubgroups_CollectionChanged;
    //        _products = new MProduct(dispatcher, connectionString);
    //    }

    //    public Store SelectedStore
    //    {
    //        get => _selectedStore;
    //        set
    //        {
    //            _selectedStore = value;
    //            if (_selectedStore != null)
    //                Update(ChoiceUpdate.NomenclatureGroup);
    //        }
    //    }

    //    public NomenclatureGroup SelectedNomenclatureGroup
    //    {
    //        get => _selectedNomenclatureGroup;
    //        set
    //        {
    //            _selectedNomenclatureGroup = value;
    //            RaisePropertyChanged();
    //            if (_selectedNomenclatureGroup != null)
    //                Update(ChoiceUpdate.NomenclatureSubgroup);
    //        }
    //    }

    //    public NomenclatureSubgroup SelectedNomenclatureSubgroup
    //    {
    //        get => _selectedNomenclatureSubgroup;
    //        set
    //        {
    //            _selectedNomenclatureSubgroup = value;
    //            RaisePropertyChanged();
    //            //Update(ChoiceUpdate.Product);
    //        }
    //    }

    //    public Product SelectedProduct
    //    {
    //        get => _selectedProduct;
    //        set
    //        {
    //            _selectedProduct = value;
    //            RaisePropertyChanged();
    //        }
    //    }

    //    public void Show(ChoiceUpdate choice)
    //    {
    //        Update(choice);
    //    }

    //    public void Add(int nomenclatureSubgroup, string name, string itemNum, string barcode, int unitStorage, int warrantyPeriod)
    //    {
    //        try
    //        {
    //            using (SqlConnection connection = new SqlConnection(_connectionString))
    //            {
    //                connection.Open();
    //                SqlTransaction transaction = connection.BeginTransaction();

    //                SqlCommand command = connection.CreateCommand();
    //                command.Transaction = transaction;

    //                try
    //                {
    //                    int exchangeRates = 0;
    //                    ExchangeRatesDataContext db = new ExchangeRatesDataContext(_connectionString);
    //                    db.FindExchangeRatesId("грн", ref exchangeRates);

    //                    command.CommandText =
    //                        "INSERT INTO Product (id_nomenclature_subgroup, title, vendor_code, barcode, id_unit_storage, count, purchase_price, id_exchange_rates, id_warranty, sales_price)" +
    //                        $" VALUES({nomenclatureSubgroup}, '{name}', '{itemNum}', '{barcode}', {unitStorage}, 0, 0, {exchangeRates}, {warrantyPeriod}, 0)";
    //                    if (command.ExecuteNonQuery() == 1)
    //                    {
    //                        command.ExecuteNonQuery();
    //                        transaction.Commit();
    //                        MessageBox.Show("Товар добавлен.", "Успех", MessageBoxButton.OK,
    //                            MessageBoxImage.Information);
    //                    }
    //                    else
    //                    {
    //                        MessageBox.Show("При попытке добавить товар произошла ошибка.", "Ошибка", MessageBoxButton.OK,
    //                            MessageBoxImage.Error);
    //                        transaction.Rollback();
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
    //                        MessageBoxImage.Error);
    //                    transaction.Rollback();
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message, ex.HResult.ToString(), MessageBoxButton.OK,
    //                MessageBoxImage.Error);
    //        }
    //    }

    //    private void Update(ChoiceUpdate choice)
    //    {
    //        switch (choice)
    //        {
    //            case ChoiceUpdate.Store:
    //                StoresList.Update();
    //                break;
    //            case ChoiceUpdate.NomenclatureGroup:
    //                NomenclatureGroupsList.Update(SelectedStore.Id);
    //                break;
    //            case ChoiceUpdate.NomenclatureSubgroup:
    //                NomenclatureSubgroups.Update(SelectedNomenclatureGroup.Id);
    //                break;
    //            case ChoiceUpdate.Product:
    //               // Products.Update();
    //                break;
    //        }
    //    }

    //    private void NomenclatureGroups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        if (_nomenclatureGroupsList.ReadCollection.Count != 0)
    //        {
    //            if (SelectedNomenclatureGroup != null)
    //            {
    //                if (SelectedNomenclatureGroup.IdStore != SelectedStore.Id)
    //                    SelectedNomenclatureGroup = _nomenclatureGroupsList.ReadCollection[0];
    //            }
    //            else
    //            {
    //                SelectedNomenclatureGroup = _nomenclatureGroupsList.ReadCollection[0];
    //            }
    //        }
    //        else
    //        {
    //            SelectedNomenclatureGroup = null;
    //        }
    //    }

    //    private void NomenclatureSubgroups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        if (_nomenclatureSubgroups.ReadCollection.Count != 0)
    //        {
    //            if (SelectedNomenclatureSubgroup != null)
    //            {
    //                if (SelectedNomenclatureSubgroup.Id != SelectedNomenclatureGroup.Id)
    //                    SelectedNomenclatureSubgroup = _nomenclatureSubgroups.ReadCollection[0];
    //            }
    //            else
    //            {
    //                SelectedNomenclatureSubgroup = _nomenclatureSubgroups.ReadCollection[0];
    //            }
    //        }
    //        else
    //        {
    //            SelectedNomenclatureSubgroup = null;
    //        }
    //    }
    //}
}
