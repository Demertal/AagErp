using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Invoice : ModelBase<Invoice>
    {
        public Invoice()
        {
            InvoiceInfosCollection = new List<InvoiceInfo>();
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

        private ICollection<InvoiceInfo> _invoiceInfosCollection;
        public virtual ICollection<InvoiceInfo> InvoiceInfosCollection
        {
            get => _invoiceInfosCollection;
            set
            {
                _invoiceInfosCollection = value;
                OnPropertyChanged("InvoiceInfosCollection");
            }
        }

        public override bool IsValid => true;
    }
}
