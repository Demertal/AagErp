﻿using System;
using System.Windows;
using CustomControlLibrary.MVVM;
using ModelModul.Models;
using ModelModul.Repositories;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace CategoryModul.ViewModels
{
    public class AddCategoryViewModel : DialogViewModelBase
    {
        #region Properties

        private Category _category = new Category();
        public Category Category => _category;

        public DelegateCommand AddCategoryCommand { get; }

        #endregion

        public AddCategoryViewModel()
        {
            Title = "Добавить категорию";
            Category.PropertyChanged += (o, e) => { RaisePropertyChanged("Category"); };
            AddCategoryCommand = new DelegateCommand(AddCategoryAsync).ObservesCanExecute(() => Category.IsValidate);
        }

        public async void AddCategoryAsync()
        {
            try
            {
                IRepository<Category> sqlCategoryRepository = new SqlCategoryRepository();
                await sqlCategoryRepository.CreateAsync(_category);
                MessageBox.Show("Категория добавлена", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
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
                Category.Title = "";
                _category.IdParent = parameters.GetValue<int?>("id");
            }
            catch (Exception ex)
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Abort));
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}