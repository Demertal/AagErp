using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Services.Dialogs;

namespace PropertyModul.ViewModels
{
    public class ShowPropertiesViewModel : EntitiesViewModelBaseCategory<PropertyName, SqlPropertyNameRepository>
    {
        #region Properties

        private ObservableCollection<PropertyName> _propertyNamesList = new ObservableCollection<PropertyName>();
        public ObservableCollection<PropertyName> PropertyNamesList
        {
            get => _propertyNamesList;
            set => SetProperty(ref _propertyNamesList, value);
        }

        #endregion

        public ShowPropertiesViewModel(IDialogService dialogService) : base(dialogService, "ShowProperty",
            "Удалить параметр?" + " При удалении параметра он исчезнет у всего товара!", "Параметр удален.",
            "Товар перемещен.")
        {
        }

        protected override void AfterAddEntityBaseCategory(PropertyName obj)
        {
            if (obj.IdCategory != null)
                FindCategory(EntitiesList, obj.IdCategory.Value).PropertyNamesCollection.Add(obj);
            else
                PropertyNamesList.Add(obj);
        }

        protected override void DeleteEntityBaseCategoryFromEntitiesList(PropertyName obj)
        {
            if (obj.IdCategory != null)
                FindCategory(EntitiesList, obj.IdCategory.Value).PropertyNamesCollection.Remove(obj);
            else
                PropertyNamesList.Remove(obj);
        }

        protected override async void LoadAsync()
        {
            CancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenSource = newCts;

            try
            {
                IRepository<Category> categoryRepository = new SqlCategoryRepository();
                IRepository<PropertyName> propertyNameRepository = new SqlPropertyNameRepository();
                var loadCategory =
                    Task.Run(
                        () => categoryRepository.GetListAsync(CancelTokenSource.Token, null, null, 0, -1,
                            (c => c.ChildCategoriesCollection, null), (c => c.PropertyNamesCollection, null)),
                        CancelTokenSource.Token);
                var loadProperty =
                    Task.Run(
                        () => propertyNameRepository.GetListAsync(CancelTokenSource.Token,
                            PropertyNameSpecification.GetPropertyNamesByIdGroup(null)), CancelTokenSource.Token);

                await Task.WhenAll(loadCategory, loadProperty);

                PropertyNamesList = new ObservableCollection<PropertyName>(loadProperty.Result);
                EntitiesList = new ObservableCollection<Category>(
                    loadCategory.Result.Where(CategorySpecification.GetCategoriesByIdParent().IsSatisfiedBy()
                        .Compile()));
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (CancelTokenSource == newCts)
                CancelTokenSource = null;
        }

        #region IDropTarget

        protected override void DragOverBaseCategory(IDropInfo dropInfo)
        {
            Category targetItemCategory = dropInfo.TargetItem as Category;
            if (!(dropInfo.Data is PropertyName sourceItem) ||
                (!(dropInfo.TargetItem is PropertyName targetItemProperty) ||
                 !CheckPropertyByCategory(sourceItem, targetItemProperty.Category)) &&
                (dropInfo.TargetItem is PropertyName ||
                 !CheckPropertyByCategory(sourceItem, targetItemCategory))) return;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        private bool CheckPropertyByCategory(PropertyName sourceItem, Category targetItem)
        {
            if (sourceItem.Category == targetItem || !CheckPropertyInParentCategory(sourceItem, targetItem)) return false;
            return targetItem != null
                ? CheckPropertyInChildCategory(sourceItem, targetItem)
                : EntitiesList.All(c => CheckPropertyInChildCategory(sourceItem, c));
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

        protected override void DropBaseCategory(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is PropertyName sourceItem)) return;
            if (MessageBox.Show(
                    "Вы уверены что хотите переместить параметр? При перемещении товара в другую категорию у товара не лежащего в новой иегррахии категорий этот параметр будет удален!",
                    "Перемещение", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            if (sourceItem.IdCategory != null)
                FindCategory(EntitiesList, sourceItem.IdCategory.Value).PropertyNamesCollection.Remove(sourceItem);
            else
                PropertyNamesList.Remove(sourceItem);
            switch (dropInfo.TargetItem)
            {
                case Category targetItemCategory:
                    sourceItem.Category = targetItemCategory;
                    sourceItem.IdCategory = targetItemCategory.Id;
                    targetItemCategory.PropertyNamesCollection.Add(sourceItem);
                    break;
                case PropertyName targetItemPropertyName:
                    sourceItem.Category = targetItemPropertyName.Category;
                    sourceItem.IdCategory = targetItemPropertyName.IdCategory;
                    if (targetItemPropertyName.IdCategory != null)
                        FindCategory(EntitiesList, targetItemPropertyName.IdCategory.Value).PropertyNamesCollection.Add(sourceItem);
                    else
                        PropertyNamesList.Add(sourceItem);
                    break;
                case null:
                    sourceItem.Category = null;
                    sourceItem.IdCategory = null;
                    PropertyNamesList.Add(sourceItem);
                    break;
            }

            MovingEntityBaseCategoryAsync(sourceItem);
        }

        #endregion
    }
}
