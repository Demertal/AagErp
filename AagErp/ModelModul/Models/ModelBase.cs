using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ModelModul.Annotations;

namespace ModelModul.Models
{
    public abstract class ModelBase: INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual string this[string columnName] => throw new NotImplementedException();

        public string Error
        {
            get;
            protected set;
        }
    }
}
