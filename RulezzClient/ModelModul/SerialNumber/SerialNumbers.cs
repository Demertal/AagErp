using System;
using Prism.Mvvm;

namespace ModelModul
{
    public partial class SerialNumbers: BindableBase, ICloneable
    {
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public object Clone()
        {
            return new SerialNumbers
            {
                Id = Id,
                IdProduct = IdProduct,
                IdCounterparty = IdCounterparty,
                Value = Value
            };
        }
    }
}
