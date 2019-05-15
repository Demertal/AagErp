using System;
using ModelModul;
using ModelModul.PropertyName;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace PropertiesModul.ViewModels
{
    class ShowPropertiesViewModel : BindableBase, IInteractionRequestAware
    {
        #region Properties

        public DbSetPropertyNames DbSetPropertyNames = new DbSetPropertyNames();

        //public ObservableCollection<PropertyNames> PropertyNames => DbSetPropertyNames.List;

        private Groups _group;
        public Groups Group
        {
            get => _group;
            set
            {
                _group = value;
                RaisePropertyChanged();
            }
        }

        private PropertyNames _propertyName;
        public PropertyNames PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                RaisePropertyChanged();
            }
        }

        private Notification _notification;
        public INotification Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value as Notification);
                Group = (Groups)_notification.Content;
            }
        }

        public Action FinishInteraction { get; set; }

        #endregion
    }
}
