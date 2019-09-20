using System;
using System.Windows;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.ViewModels;
using Prism.Services.Dialogs;

namespace CategoryModul.ViewModels
{
    public class ShowCategoryViewModel : EntityViewModelBase<Category, CategoryViewModel, SqlCategoryRepository>
    {
        public ShowCategoryViewModel() : base("Категория добавлена", "Категория переименована")
        {
        }

        public override void PropertiesTransfer(Category fromEntity, Category toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.IdParent = fromEntity.IdParent;
            toEntity.Title = fromEntity.Title;
            toEntity.Parent = fromEntity.Parent;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Backup = parameters.GetValue<Category>("entity");
                Entity = new CategoryViewModel();
                if (Backup == null)
                {
                    Title = "Добавить";
                    Entity.Parent = parameters.GetValue<Category>("parent");
                    Entity.IdParent = Entity.Parent?.Id;
                    IsAdd = true;
                }
                else
                {
                    Title = "Изменить";
                    IsAdd = false;
                    PropertiesTransfer(Backup, Entity);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                RaiseRequestClose(new DialogResult(ButtonResult.Abort));
            }
        }
    }
}