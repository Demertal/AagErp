namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class priceProducts
    {
        public long id { get; set; }

        public long idProduct { get; set; }

        public long idRevaluation { get; set; }

        [Column(TypeName = "money")]
        public decimal price { get; set; }

        public virtual products products { get; set; }

        public virtual revaluationProducts revaluationProducts { get; set; }
    }
}
