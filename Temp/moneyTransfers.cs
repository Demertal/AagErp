namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class moneyTransfers
    {
        public long id { get; set; }

        public DateTime date { get; set; }

        public int idCounterparty { get; set; }

        [Column(TypeName = "money")]
        public decimal moneyAmount { get; set; }

        public int idType { get; set; }

        public Guid idMovementGoods { get; set; }

        public virtual counterparties counterparties { get; set; }

        public virtual movementGoods movementGoods { get; set; }

        public virtual moneyTransferTypes moneyTransferTypes { get; set; }
    }
}
