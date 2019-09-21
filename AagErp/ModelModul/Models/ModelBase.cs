using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ModelModul.Annotations;
using ModelModul.Specifications.BasisSpecifications;

namespace ModelModul.Models
{
    public abstract class ModelBase<TEntity> : INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo, ICloneable
        where TEntity : ModelBase<TEntity>
    {

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo

        public virtual string this[string columnName] => throw new NotImplementedException();

        public string Error { get; }

        #endregion

        #region INotifyDataErrorInfo

        protected readonly Dictionary<string, ICollection<string>>
            ValidationErrors = new Dictionary<string, ICollection<string>>();


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !ValidationErrors.ContainsKey(propertyName))
                return null;

            return ValidationErrors[propertyName];
        }

        public bool HasErrors => ValidationErrors.Count > 0;

        #endregion

        #region ICloneable

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion

        public abstract bool IsValid { get; }

    }
}
