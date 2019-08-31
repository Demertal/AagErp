using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
using Prism.Regions;

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

        public ShowCategoriesViewModel()
        {
        }

        private async void LoadAsync()
        {
            try
            {
                IRepository<Category> saCategoryRepository = new SqlCategoryRepository();

                CategoriesList = new ObservableCollection<Category>(
                    (await saCategoryRepository.GetListAsync(include: c => c.ChildCategoriesCollection)).Where(
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
