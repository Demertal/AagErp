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

namespace PropertyModul.ViewModels
{
    public class ShowPropertiesViewModel : ViewModelBase, IDropTarget
    {
        private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();
        public ObservableCollection<Category> CategoriesList
        {
            get => _categoriesList;
            set => SetProperty(ref _categoriesList, value);
        }

        private ObservableCollection<PropertyName> _propertyNamesList = new ObservableCollection<PropertyName>();
        public ObservableCollection<PropertyName> PropertyNamesList
        {
            get => _propertyNamesList;
            set => SetProperty(ref _propertyNamesList, value);
        }

        public DelegateCommand<Category> DeleteCategoryCommand { get; }
        public DelegateCommand<Category> AddCategoryCommand { get; }
        public DelegateCommand<Category> RenameCategoryCommand { get; }

        public DelegateCommand<PropertyName> DeletePropertyNameCommand { get; }
        public DelegateCommand<Category> AddPropertyNameCommand { get; }
        public DelegateCommand<PropertyName> SelectedPropertyNameCommand { get; }

        public ShowPropertiesViewModel(IDialogService dialogService) : base(dialogService)
        {
            AddCategoryCommand = new DelegateCommand<Category>(AddCategory);
            RenameCategoryCommand = new DelegateCommand<Category>(RenameCategory);
            DeleteCategoryCommand = new DelegateCommand<Category>(DeleteCategoryAsync);

            DeletePropertyNameCommand = new DelegateCommand<PropertyName>(DeletePropertyNameAsync);
            AddPropertyNameCommand = new DelegateCommand<Category>(AddPropertyNameAsync);
            SelectedPropertyNameCommand = new DelegateCommand<PropertyName>(SelectedPropertyName);
        }

        private void LoadAsync()
        {
            try
            {
                IRepository<Category> categoryRepository = new SqlCategoryRepository();
                IRepository<PropertyName> propertyNameRepository = new SqlPropertyNameRepository();
                var loadCategory = Task.Run(() => categoryRepository.GetListAsync(null, null, 0, -1, c => c.ChildCategoriesCollection, c => c.PropertyNamesCollection));
                var loadProperty = Task.Run(() => propertyNameRepository.GetListAsync(PropertyNameSpecification.GetPropertyNamesByIdGroup(null)));

                Task.WaitAll(loadCategory, loadProperty);

                PropertyNamesList = new ObservableCollection<PropertyName>(loadProperty.Result);
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
            if (temp.IdParent == null) CategoriesList.Add(temp);
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

        private void AddPropertyNameAsync(Category obj)
        {
            DialogService.ShowDialog("ShowProperty", new DialogParameters { { "category", obj } }, CallbackProperty);
        }

        private void CallbackProperty(IDialogResult dialogResult)
        {
            PropertyName temp = dialogResult.Parameters.GetValue<PropertyName>("entity");
            if (temp == null) return;
            if(temp.IdCategory != null)
                FindCategory(CategoriesList, temp.IdCategory.Value).PropertyNamesCollection.Add(temp);
            else
                PropertyNamesList.Add(temp);
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

        private async void DeletePropertyNameAsync(PropertyName obj)
        {
            if (obj == null) return;
            if (MessageBox.Show("Удалить параметр? При удалении параметра он исчезнет у всего товара!", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                SqlRepository<PropertyName> propertyNameRepository = new SqlPropertyNameRepository();
                await propertyNameRepository.DeleteAsync(obj);
                if (obj.IdCategory != null)
                    FindCategory(CategoriesList, obj.IdCategory.Value).PropertyNamesCollection.Remove(obj);
                else
                    PropertyNamesList.Remove(obj);
                MessageBox.Show("Параметр удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void MovingPropertyAsync(PropertyName obj)
        {
            try
            {
                PropertyName temp = (PropertyName) obj.Clone();
                temp.Category = null;
                temp.PropertyValuesCollection = null;
                SqlRepository<PropertyName> propertyNameRepository = new SqlPropertyNameRepository();
                await propertyNameRepository.UpdateAsync(temp);
                MessageBox.Show("Параметр перемещен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                LoadAsync();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectedPropertyName(PropertyName obj)
        {
            DialogService.Show("ShowProperty", new DialogParameters { { "entity", obj } }, null);
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

        #region IDropTarget

        public void DragOver(IDropInfo dropInfo)
        {
            Category targetItemCategory = dropInfo.TargetItem as Category;
            switch (dropInfo.Data)
            {
                case Category sourceItemCategory when !(dropInfo.TargetItem is PropertyName) && CheckCategoryByCategory(sourceItemCategory, targetItemCategory):
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Copy;
                    break;
                case PropertyName sourceItemProperty when dropInfo.TargetItem is PropertyName targetItemProperty && CheckPropertyByCategory(sourceItemProperty, targetItemProperty.Category) ||
                                                          !(dropInfo.TargetItem is PropertyName) && CheckPropertyByCategory(sourceItemProperty, targetItemCategory):
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Copy;
                    break;
            }
        }

        private bool CheckPropertyByCategory(PropertyName sourceItem, Category targetItem)
        {
            if (sourceItem.Category == targetItem || !CheckPropertyInParentCategory(sourceItem, targetItem)) return false;
            if (targetItem == null)
                if (CategoriesList.Any(c => !CheckPropertyInChildCategory(sourceItem, c)))
                    return false;
                else
                    return true;
            return CheckPropertyInChildCategory(sourceItem, targetItem);
        }

        private bool CheckPropertyInParentCategory(PropertyName sourceItem, Category targetItem)
        {
            if(targetItem == null)
                if (PropertyNamesList.Any(p => p.Title == sourceItem.Title && p.Id != sourceItem.Id))
                    return false;
                else
                    return true;
            if (targetItem.PropertyNamesCollection.Any(p => p.Title == sourceItem.Title && p.Id != sourceItem.Id))
                return false;
            return CheckPropertyInParentCategory(sourceItem, targetItem.Parent);
        }

        private bool CheckPropertyInChildCategory(PropertyName sourceItem, Category targetItem)
        {
            if (targetItem.PropertyNamesCollection.Any(p => p.Title == sourceItem.Title && p.Id != sourceItem.Id))
                return false;
            if (targetItem.ChildCategoriesCollection.Any(
                category => !CheckPropertyInChildCategory(sourceItem, category))) return false;
            return true;
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
                case PropertyName sourceItemProperty:
                    if (MessageBox.Show(
                            "Вы уверены что хотите переместить параметр? При перемещении товара в другую категорию у товара не лежащего в новой иегррахии категорий этот параметр будет удален!",
                            "Перемещение", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                        MessageBoxResult.Yes) return;
                    if (sourceItemProperty.IdCategory != null)
                        FindCategory(CategoriesList, sourceItemProperty.IdCategory.Value).PropertyNamesCollection.Remove(sourceItemProperty);
                    else
                        PropertyNamesList.Remove(sourceItemProperty);
                    switch (dropInfo.TargetItem)
                    {
                        case Category targetItemCategory:
                            sourceItemProperty.Category = targetItemCategory;
                            sourceItemProperty.IdCategory = targetItemCategory.Id;
                            targetItemCategory.PropertyNamesCollection.Add(sourceItemProperty);
                            break;
                        case PropertyName targetItemPropertyName:
                            sourceItemProperty.Category = targetItemPropertyName.Category;
                            sourceItemProperty.IdCategory = targetItemPropertyName.IdCategory;
                            if (targetItemPropertyName.IdCategory != null)
                                FindCategory(CategoriesList, targetItemPropertyName.IdCategory.Value).PropertyNamesCollection.Add(sourceItemProperty);
                            else
                                PropertyNamesList.Add(sourceItemProperty);
                            break;
                        case null:
                            sourceItemProperty.Category = null;
                            sourceItemProperty.IdCategory = null;
                            PropertyNamesList.Add(sourceItemProperty);
                            break;
                    }

                    MovingPropertyAsync(sourceItemProperty);

                    break;
            }
        }

        #endregion
    }
}
