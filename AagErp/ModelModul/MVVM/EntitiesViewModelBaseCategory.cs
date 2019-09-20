using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace ModelModul.MVVM
{
    public abstract class
        EntitiesViewModelBaseCategory<TEntity, TRepository> : EntitiesViewModelBase<Category, SqlCategoryRepository>, IDropTarget
        where TEntity : ModelBase<TEntity>
        where TRepository : IRepository<TEntity>, new()
    {
        #region Properties

        protected readonly string DialogNamBaseCategory, AskDeleteBaseCategory, SuccessDeleteBaseCategory, SuccessMovingBaseCategory;

        #endregion

        #region Command

        public DelegateCommand<Category> AddEntityBaseCategoryCommand { get; }
        public DelegateCommand<TEntity> SelectedEntityBaseCategoryCommand { get; }
        public DelegateCommand<TEntity> DeleteEntityBaseCategoryCommand { get; }

        #endregion

        protected EntitiesViewModelBaseCategory(IDialogService dialogService, string dialogName, string askDelete,
            string successDelete, string successMoving) : base(dialogService, "ShowCategory",
            "Вы уверены что хотите удалить категорию?" +
            " При удалении категории будут также удалены все дочерние категории, товар в них и свойства!",
            "Категория удалена.")
        {
            DialogNamBaseCategory = dialogName;
            AskDeleteBaseCategory = askDelete;
            SuccessDeleteBaseCategory = successDelete;
            SuccessMovingBaseCategory = successMoving;
            AddEntityBaseCategoryCommand = new DelegateCommand<Category>(AddEntityBaseCategory);
            SelectedEntityBaseCategoryCommand = new DelegateCommand<TEntity>(SelectedEntityBaseCategory);
            DeleteEntityBaseCategoryCommand = new DelegateCommand<TEntity>(DeleteEntityBaseCategory);
        }

        protected abstract override void LoadAsync();

        #region BaseCategoryMethod

        protected override void AddEntity(Category obj)
        {
            DialogService.ShowDialog(DialogName, new DialogParameters { { "parent", obj } }, CallbackEntity);
        }

        protected override void AfterAddEntity(Category obj)
        {
            if (obj.IdParent == null) EntitiesList.Add(obj);
            else
            {
                Category parent = FindCategory(EntitiesList, obj.IdParent.Value);
                parent.ChildCategoriesCollection.Add(obj);
                obj.Parent = parent;
            }
        }

        protected override async void DeleteEntity(Category obj)
        {
            try
            {
                if (MessageBox.Show(AskDelete, "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                    MessageBoxResult.Yes) return;
                IRepository<Category> categoryRepository = new SqlCategoryRepository();
                await categoryRepository.DeleteAsync(obj);
                MessageBox.Show(SuccessDelete, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                if (obj.IdParent == null) EntitiesList.Remove(obj);
                else FindCategory(EntitiesList, obj.IdParent.Value).ChildCategoriesCollection.Remove(obj);
            }
            catch (Exception e)
            {
                LoadAsync();
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

        protected Category FindCategory(ICollection<Category> categories, int id)
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

        #endregion

        #region Method

        private void AddEntityBaseCategory(Category obj)
        {
            DialogService.ShowDialog(DialogNamBaseCategory, new DialogParameters { { "category", obj } }, CallbackEntityBaseCategory);
        }

        private void CallbackEntityBaseCategory(IDialogResult dialogResult)
        {
            TEntity temp = dialogResult.Parameters.GetValue<TEntity>("entity");
            if (temp == null) return;
            AfterAddEntityBaseCategory(temp);
        }

        protected abstract void AfterAddEntityBaseCategory(TEntity obj);

        private void SelectedEntityBaseCategory(TEntity obj)
        {
            DialogService.Show(DialogNamBaseCategory, new DialogParameters { { "entity", obj } }, null);
        }

        protected async void DeleteEntityBaseCategory(TEntity obj)
        {
            if (obj == null) return;
            if (MessageBox.Show(AskDeleteBaseCategory, "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;
            try
            {
                IRepository<TEntity> repository = new TRepository();
                await repository.DeleteAsync(obj);
                DeleteEntityBaseCategoryFromEntitiesList(obj);
                MessageBox.Show(SuccessDelete, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException?.Message ?? e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected abstract void DeleteEntityBaseCategoryFromEntitiesList(TEntity obj);

        protected async void MovingEntityBaseCategoryAsync(TEntity obj)
        {
            try
            {
                TEntity temp = (TEntity)obj.Clone();
                IRepository<TEntity> repository = new TRepository();
                await repository.UpdateAsync(temp);
                MessageBox.Show(SuccessMovingBaseCategory, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                LoadAsync();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region IDropTarget

        public void DragOver(IDropInfo dropInfo)
        {
            Category targetItemCategory = dropInfo.TargetItem as Category;
            if (dropInfo.Data is Category sourceItem && !(dropInfo.TargetItem is PropertyName) &&
                CheckCategoryByCategory(sourceItem, targetItemCategory))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
            else DragOverBaseCategory(dropInfo);
        }

        protected abstract void DragOverBaseCategory(IDropInfo dropInfo);

        private bool CheckCategoryByCategory(Category sourceItem, Category targetItem)
        {
            if (sourceItem == targetItem || sourceItem.Parent == targetItem ||
                sourceItem.ChildCategoriesCollection.Contains(targetItem)) return false;
            return sourceItem.ChildCategoriesCollection.All(category => CheckCategoryByCategory(category, targetItem));
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is Category sourceItemCategory))
            {
                DropBaseCategory(dropInfo);
                return;
            }
            if (MessageBox.Show(
                    "Вы уверены что хотите переместить категорию? При перемещении категории все родительские параметры будут изменены!",
                    "Перемещение", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;

            if (sourceItemCategory.Parent != null)
                sourceItemCategory.Parent.ChildCategoriesCollection.Remove(sourceItemCategory);
            else
                EntitiesList.Remove(sourceItemCategory);

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
                    EntitiesList.Add(sourceItemCategory);
                    break;
            }

            MovingCategoryAsync(sourceItemCategory);
        }

        protected abstract void DropBaseCategory(IDropInfo dropInfo);

        #endregion
    }
}