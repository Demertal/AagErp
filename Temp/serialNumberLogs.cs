namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class serialNumberLogs
    {
        public long id { get; set; }

        public long idSerialNumber { get; set; }

        public Guid idMovmentGood { get; set; }

        public virtual movementGoods movementGoods { get; set; }

        public virtual serialNumbers serialNumbers { get; set; }
    }
}
