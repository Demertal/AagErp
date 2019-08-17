namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class invoiceInfos
    {
        public int id { get; set; }

        public long idProduct { get; set; }

        public double count { get; set; }

        public int idInvoice { get; set; }

        public virtual invoices invoices { get; set; }

        public virtual products products { get; set; }
    }
}
