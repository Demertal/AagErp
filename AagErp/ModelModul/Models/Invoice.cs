using System.Collections.Generic;

namespace ModelModul.Models
{
    public class Invoice : ModelBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            InvoiceInfos = new List<InvoiceInfo>();
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

        private ICollection<InvoiceInfo> _invoiceInfos;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceInfo> InvoiceInfos
        {
            get => _invoiceInfos;
            set
            {
                _invoiceInfos = value;
                OnPropertyChanged("InvoiceInfos");
            }
        }
    }
}
