using System;
using System.Collections.ObjectModel;
using System.Windows;
using ModelModul;
using ModelModul.Supplier;
using Prism.Commands;
using Prism.Mvvm;

namespace SupplierModul.ViewModels
{
    public class ShowSuppliersViewModel: BindableBase
    {
        #region Properties

        private readonly DbSetSuppliers _dbSetSuppliers = new DbSetSuppliers();

        public ObservableCollection<Suppliers> SuppliersList => _dbSetSuppliers.List;

        public DelegateCommand<object> AddSuppliersCommand { get; }

        public DelegateCommand<Suppliers> DeleteSuppliersCommand { get; }

        #endregion

        public ShowSuppliersViewModel()
        {
            Load();
            AddSuppliersCommand = new DelegateCommand<object>(AddSuppliers);
            DeleteSuppliersCommand = new DelegateCommand<Suppliers>(DeleteSuppliers);
        }

        private async void DeleteSuppliers(Suppliers obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить поставщика?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                await _dbSetSuppliers.DeleteAsync(obj.Id);
                Load();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddSuppliers(object obj)
        {
            try
            {
                if(obj == null) return;
                if (((Suppliers) obj).Id == 0)
                {
                    await _dbSetSuppliers.AddAsync((Suppliers) obj);
                }
                else
                {
                    await _dbSetSuppliers.UpdateAsync((Suppliers) obj);
                }

                Load();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Load()
        {
            try
            {
                await _dbSetSuppliers.LoadAsync();
                RaisePropertyChanged("SuppliersList");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
