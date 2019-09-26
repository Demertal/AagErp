namespace ModelModul.Models
{
    public class User : ModelBase<User>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private int? _idStore;
        public int? IdStore
        {
            get => _idStore;
            set
            {
                _idStore = value;
                OnPropertyChanged();
            }
        }

        private Store _store;
        public virtual Store Store
        {
            get => _store;
            set
            {
                _store = value;
                OnPropertyChanged();
            }
        }


        public override bool IsValid { get; }
    }
}
