using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace ProductModul.ViewModels
{
    class ShowNomenclatureViewModel : ViewModelBase, IDropTarget
    {
        private ObservableCollection<WarrantyPeriod> _warrantyPeriods = new ObservableCollection<WarrantyPeriod>();
        public ObservableCollection<WarrantyPeriod> WarrantyPeriodsList
        {
            get => _warrantyPeriods;
            set => SetProperty(ref _warrantyPeriods, value);
        }

        private ObservableCollection<UnitStorage> _unitStorages = new ObservableCollection<UnitStorage>();
        public ObservableCollection<UnitStorage> UnitStoragesList
        {
            get => _unitStorages;
            set => SetProperty(ref _unitStorages, value);
        }

        private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();
        public ObservableCollection<Category> CategoriesList
        {
            get => _categoriesList;
            set => SetProperty(ref _categoriesList, value);
        }

        public DelegateCommand<Category> DeleteCategoryCommand { get; }
        public DelegateCommand<Category> AddCategoryCommand { get; }
        public DelegateCommand<Category> RenameCategoryCommand { get; }

        public DelegateCommand<Product> DeleteProductCommand { get; }
        public DelegateCommand<Category> AddProductCommand { get; }
        public DelegateCommand<Product> SelectedProductCommand { get; }

        public ShowNomenclatureViewModel(IDialogService dialogService) : base(dialogService)
        {
            AddCategoryCommand = new DelegateCommand<Category>(AddCategory);
            RenameCategoryCommand = new DelegateCommand<Category>(RenameCategory);
            DeleteCategoryCommand = new DelegateCommand<Category>(DeleteCategoryAsync);
            DeleteProductCommand = new DelegateCommand<Product>(DeleteProductAsync);
            AddProductCommand = new DelegateCommand<Category>(AddProduct);
            SelectedProductCommand = new DelegateCommand<Product>(SelectedProduct);
        }

        private void LoadAsync()
        {
            try
            {
                IRepository<Category> categoryRepository = new SqlCategoryRepository();
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> warrantyPeriodRepository = new SqlWarrantyPeriodRepository();

                var loadUnitStorage = Task.Run(() => unitStorageRepository.GetListAsync());
                var loadWarrantyPeriod = Task.Run(() => warrantyPeriodRepository.GetListAsync());
                var loadCategory = Task.Run(() => categoryRepository.GetListAsync(null, null, 0, -1, c => c.ChildCategoriesCollection, c => c.ProductsCollection));

                Task.WaitAll(loadUnitStorage, loadWarrantyPeriod, loadCategory);

               
                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(loadWarrantyPeriod.Result);
                CategoriesList = new ObservableCollection<Category>(
                    loadCategory.Result.Where(CategorySpecification.GetCategoriesByIdParent().IsSatisfiedBy()
                        .Compile()));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCategory(Category obj)
        {
            DialogService.ShowDialog("AddCategory", new DialogParameters { { "id", obj?.Id } }, CallbackCategory);
        }

        private void RenameCategory(Category obj)
        {
            DialogService.ShowDialog("RenameCategory", new DialogParameters { { "category", obj } }, null);
        }

        private void CallbackCategory(IDialogResult dialogResult)
        {
            Category temp = dialogResult.Parameters.GetValue<Category>("category");
            if (temp == null) return;
            if(temp.IdParent == null) CategoriesList.Add(temp);
            else
            {
                Category parent = FindCategory(CategoriesList, temp.IdParent.Value);
                parent.ChildCategoriesCollection.Add(temp);
                temp.Parent = parent;
            }
        }

        private async void DeleteCategoryAsync(Category obj)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите удалить категорию? При удалении категории будут также удалены все дочерние категории, товар в них и свойства!", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                    MessageBoxResult.Yes) return;
                SqlRepository<Category> sqlCategoryRepository = new SqlCategoryRepository();
                await sqlCategoryRepository.DeleteAsync(obj);
                MessageBox.Show("Категория удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (obj.IdParent == null) CategoriesList.Remove(obj);
                else FindCategory(CategoriesList, obj.IdParent.Value).ChildCategoriesCollection.Remove(obj);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void MovingCategoryAsync(Category obj)
        {
            try
            {
                Category temp = (Category)obj.Clone();
                temp.Parent = null;
                temp.ChildCategoriesCollection = null;
                temp.ProductsCollection = null;
                SqlRepository<Category> sqlCategoryRepository = new SqlCategoryRepository();
                await sqlCategoryRepository.UpdateAsync(temp);
                MessageBox.Show("Категория перемещена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                LoadAsync();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct(Category obj)
        {
            DialogService.ShowDialog("ShowProduct", new DialogParameters { { "category", obj } }, CallbackProduct);
        }

        private void CallbackProduct(IDialogResult dialogResult)
        {
            Product temp = dialogResult.Parameters.GetValue<Product>("product");
            if (temp != null)
            {
                FindCategory(CategoriesList, temp.IdCategory).ProductsCollection.Add(temp);
            }
        }

        private Category FindCategory(ICollection<Category> categories, int id)
        {
            Category temp = categories.FirstOrDefault(c => c.Id == id);
            if (temp != null) return temp;
            foreach (var category in categories)
            {
                temp = FindCategory(category.ChildCategoriesCollection, id);
                if (temp != null) break;
            }
            return temp;
        }

        private async void DeleteProductAsync(Product obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить товар?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                SqlRepository<Product> sqlProductRepository = new SqlProductRepository();
                await sqlProductRepository.DeleteAsync(obj);
                FindCategory(CategoriesList, obj.IdCategory).ProductsCollection.Remove(obj);
                MessageBox.Show("Товар удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void MovingProductAsync(Product obj)
        {
            try
            {
                Product temp = (Product) obj.Clone();
                temp.Category = null;
                SqlRepository<Product> sqlProductRepository = new SqlProductRepository();
                await sqlProductRepository.UpdateAsync(temp);
                MessageBox.Show("Товар перемещен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                LoadAsync();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectedProduct(Product obj)
        {
            DialogService.Show("ShowProduct", new DialogParameters { { "product", obj } }, null);
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
           LoadAsync();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion

        public void DragOver(IDropInfo dropInfo)
        {
            Category targetItemCategory = dropInfo.TargetItem as Category;
            switch (dropInfo.Data)
            {
                case Category sourceItemCategory when !(dropInfo.TargetItem is PropertyName) && CheckCategoryByCategory(sourceItemCategory, targetItemCategory):
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Copy;
                    break;
                case Product sourceItemProduct when targetItemCategory != null && targetItemCategory.Id != sourceItemProduct.IdCategory ||
                                                    dropInfo.TargetItem is Product targetItemProduct && targetItemProduct.IdCategory != sourceItemProduct.IdCategory:
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Copy;
                    break;
            }
        }

        private bool CheckCategoryByCategory(Category sourceItem, Category targetItem)
        {
            if (sourceItem == targetItem || sourceItem.Parent == targetItem ||
                sourceItem.ChildCategoriesCollection.Contains(targetItem)) return false;
            return sourceItem.ChildCategoriesCollection.All(category => CheckCategoryByCategory(category, targetItem));
        }

        public void Drop(IDropInfo dropInfo)
        {
            switch (dropInfo.Data)
            {
                case Category sourceItemCategory:
                    if (MessageBox.Show(
                            "Вы уверены что хотите переместить категорию? При перемещении категории все родительские параметры будут изменены!",
                            "Перемещение", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;

                    if (sourceItemCategory.Parent != null)
                        sourceItemCategory.Parent.ChildCategoriesCollection.Remove(sourceItemCategory);
                    else
                        CategoriesList.Remove(sourceItemCategory);

                    switch (dropInfo.TargetItem)
                    {
                        case Category targetItem:
                            sourceItemCategory.Parent = targetItem;
                            sourceItemCategory.IdParent = targetItem.Id;
                            targetItem.ChildCategoriesCollection.Add(sourceItemCategory);
                            break;
                        case null:
                            sourceItemCategory.Parent = null;
                            sourceItemCategory.IdParent = null;
                            CategoriesList.Add(sourceItemCategory);
                            break;
                    }

                    MovingCategoryAsync(sourceItemCategory);

                    break;
                case Product sourceItemProduct:
                    if (MessageBox.Show(
                            "Вы уверены что хотите переместить товар? При перемещении товара в другую категорию все отсуствующие параметры будут удалены!",
                            "Перемещение", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;
                    FindCategory(CategoriesList, sourceItemProduct.IdCategory).ProductsCollection.Remove(sourceItemProduct);
                    switch (dropInfo.TargetItem)
                    {
                        case Category targetItemCategory:
                            sourceItemProduct.IdCategory = targetItemCategory.Id;
                            targetItemCategory.ProductsCollection.Add(sourceItemProduct);
                            break;
                        case Product targetItemProduct:
                            sourceItemProduct.IdCategory = targetItemProduct.IdCategory;
                            FindCategory(CategoriesList, targetItemProduct.IdCategory).ProductsCollection.Add(sourceItemProduct);
                            break;
                    }

                    MovingProductAsync(sourceItemProduct);

                    break;
            }
        }
    }
}
