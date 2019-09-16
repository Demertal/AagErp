using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ModelModul.Annotations;

namespace ModelModul.Models
{
    public abstract class ModelBase: INotifyPropertyChanged, IDataErrorInfo, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected ModelBase() {}

        protected ModelBase(ModelBase obj) { }

        public virtual string this[string columnName] => throw new NotImplementedException();

        public string Error
        {
            get;
            protected set;
        }

        public virtual bool IsValidate => true;
        public virtual object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
