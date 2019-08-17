namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class movementGoodsInfos
    {
        public long id { get; set; }

        public double count { get; set; }

        [Column(TypeName = "money")]
        public decimal? price { get; set; }

        public Guid idReport { get; set; }

        public long idProduct { get; set; }

        public virtual movementGoods movementGoods { get; set; }

        public virtual products products { get; set; }
    }
}
