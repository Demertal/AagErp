using System;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace CategoryModul.ViewModels
{
    public class RenameCategoryViewModel : DialogViewModelBase
    {
        #region Properties

        private Category _category = new Category();

        public Category Category
        {
            get => _category;
            set
            {
                SetProperty(ref _category, value);
                if(Category != null)
                    Category.PropertyChanged += (o, e) => { RaisePropertyChanged("Category"); };
            }
        }

        public DelegateCommand UpdateCategoryCommand { get; }

        #endregion

        public RenameCategoryViewModel()
        {
            Title = "Добавить категорию";
            UpdateCategoryCommand = new DelegateCommand(UpdateCategory).ObservesCanExecute(() => Category.IsValidate);
        }

        public async void UpdateCategory()
        {
            try
            {
                IRepository<Category> sqlCategoryRepository = new SqlCategoryRepository();
                await sqlCategoryRepository.UpdateAsync(_category);
                MessageBox.Show("Категория переименована", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Category = parameters.GetValue<Category>("category");
            }
            catch (Exception ex)
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Abort));
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}