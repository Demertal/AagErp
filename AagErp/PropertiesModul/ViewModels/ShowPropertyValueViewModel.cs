using System;
using System.Windows;
using ModelModul.Models;
using ModelModul.MVVM;
using ModelModul.Repositories;
using ModelModul.ViewModels;
using Prism.Services.Dialogs;

namespace PropertyModul.ViewModels
{
    class ShowPropertyValueViewModel : EntityViewModelBase<PropertyValue, PropertyValueViewModel, SqlPropertyValueRepository>
    {
        public ShowPropertyValueViewModel() : base("Значение добавлено", "Значение изменено")
        {
        }

        public override void PropertiesTransfer(PropertyValue fromEntity, PropertyValue toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.Value = fromEntity.Value;
            toEntity.IdPropertyName = fromEntity.IdPropertyName;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                Backup = parameters.GetValue<PropertyValue>("entity");
                Entity = new PropertyValueViewModel();
                if (Backup == null)
                {
                    Title = "Добавить";
                    Entity.IdPropertyName = parameters.GetValue<int>("propertyName");
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
