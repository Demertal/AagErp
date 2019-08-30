using System.Collections.Generic;

namespace ModelModul.Models
{
    public class MoneyTransferType : ModelBase
    {
        public MoneyTransferType()
        {
            MoneyTransfersCollection = new List<MoneyTransfer>();
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private ICollection<MoneyTransfer> _moneyTransfersCollection;
        public virtual ICollection<MoneyTransfer> MoneyTransfersCollection
        {
            get => _moneyTransfersCollection;
            set
            {
                _moneyTransfersCollection = value;
                OnPropertyChanged("MoneyTransfersCollection");
            }
        }
    }
}
