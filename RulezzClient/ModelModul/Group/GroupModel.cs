using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModelModul.Group
{
    public class GroupModel : INotifyPropertyChanged
    {
        #region Properties

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

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private int? _idParentGroup;
        public int? IdParentGroup
        {
            get => _idParentGroup;
            set
            {
                _idParentGroup = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion

        #region Converts

        public Groups ConvertToGroups()
        {
            return new Groups
            {
                Id = Id,
                Title = Title,
                IdParentGroup = IdParentGroup
            };
        }

        #endregion
    }
}
