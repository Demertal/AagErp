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

namespace ProductModul.ViewModels
{
    class ShowNomenclatureViewModel : EntitiesViewModelBaseCategory<Product, SqlProductRepository>
    {
        #region Properties

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

        #endregion

        public ShowNomenclatureViewModel(IDialogService dialogService) : base(dialogService, "ShowProduct", "Удалить товар?", "Товар удален.", "Товар перемещен.")
        {
        }

        protected override async void LoadAsync()
        {
            CancelTokenSource?.Cancel();
            CancellationTokenSource newCts = new CancellationTokenSource();
            CancelTokenSource = newCts;

            try
            {
                IRepository<Category> categoryRepository = new SqlCategoryRepository();
                IRepository<UnitStorage> unitStorageRepository = new SqlUnitStorageRepository();
                IRepository<WarrantyPeriod> warrantyPeriodRepository = new SqlWarrantyPeriodRepository();

                var loadUnitStorage = Task.Run(() => unitStorageRepository.GetListAsync(CancelTokenSource.Token),
                    CancelTokenSource.Token);
                var loadWarrantyPeriod = Task.Run(() => warrantyPeriodRepository.GetListAsync(CancelTokenSource.Token),
                    CancelTokenSource.Token);
                var loadCategory =
                    Task.Run(
                        () => categoryRepository.GetListAsync(CancelTokenSource.Token, null, null, 0, -1,
                            (c => c.ChildCategoriesCollection, null), (c => c.ProductsCollection, null)), CancelTokenSource.Token);

                await Task.WhenAll(loadUnitStorage, loadWarrantyPeriod, loadCategory);

                UnitStoragesList = new ObservableCollection<UnitStorage>(loadUnitStorage.Result);
                WarrantyPeriodsList = new ObservableCollection<WarrantyPeriod>(loadWarrantyPeriod.Result);
                EntitiesList = new ObservableCollection<Category>(
                    loadCategory.Result.Where(CategorySpecification.GetCategoriesByIdParent().IsSatisfiedBy()
                        .Compile()));
            }
            catch (OperationCanceledException) {}
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (CancelTokenSource == newCts)
                CancelTokenSource = null;
        }

        protected override void AfterAddEntityBaseCategory(Product obj)
        {
            FindCategory(EntitiesList, obj.IdCategory).ProductsCollection.Add(obj);
        }

        protected override void DeleteEntityBaseCategoryFromEntitiesList(Product obj)
        {
            FindCategory(EntitiesList, obj.IdCategory).ProductsCollection.Remove(obj);
        }

        #region IDropTarget

        protected override void DragOverBaseCategory(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is Product sourceItem) ||
                (!(dropInfo.TargetItem is Category targetItemCategory) ||
                 targetItemCategory.Id == sourceItem.IdCategory) &&
                (!(dropInfo.TargetItem is Product targetItemProduct) ||
                 targetItemProduct.IdCategory == sourceItem.IdCategory)) return;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        protected override void DropBaseCategory(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is Product sourceItem)) return;
            if (MessageBox.Show(
                    "Вы уверены что хотите переместить товар? При перемещении товара в другую категорию все отсуствующие параметры будут удалены!",
                    "Перемещение", MessageBoxButton.YesNo, MessageBoxImage.Question) !=
                MessageBoxResult.Yes) return;

            FindCategory(EntitiesList, sourceItem.IdCategory).ProductsCollection.Remove(sourceItem);

            switch (dropInfo.TargetItem)
            {
                case Category targetItemCategory:
                    sourceItem.IdCategory = targetItemCategory.Id;
                    targetItemCategory.ProductsCollection.Add(sourceItem);
                    break;
                case Product targetItemProduct:
                    sourceItem.IdCategory = targetItemProduct.IdCategory;
                    FindCategory(EntitiesList, targetItemProduct.IdCategory).ProductsCollection
                        .Add(sourceItem);
                    break;
            }

            MovingEntityBaseCategoryAsync(sourceItem);
        }

        #endregion
    }
}
