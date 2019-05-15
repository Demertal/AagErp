using Prism.Mvvm;

namespace ModelModul
{
    public partial class SerialNumbers: BindableBase
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
    }
}
