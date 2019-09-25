using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace CategoryModul.ViewModels
{
    class ShowCategoriesViewModel : ViewModelBase
    {
        private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();

        public ObservableCollection<Category> CategoriesList
        {
            get => _categoriesList;
            set => SetProperty(ref _categoriesList, value);
        }

        public ShowCategoriesViewModel(IDialogService dialogService) : base(dialogService)
        {
        }

        private async void LoadAsync()
        {
            try
            {
                IRepository<Category> categoryRepository = new SqlCategoryRepository();

                CategoriesList = new ObservableCollection<Category>(
                    (await categoryRepository.GetListAsync(include: (c => c.ChildCategoriesCollection, null))).Where(
                        CategorySpecification.GetCategoriesByIdParent().IsSatisfiedBy().Compile()));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
    }
}
