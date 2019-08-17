namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class movementGoods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public movementGoods()
        {
            moneyTransfers = new HashSet<moneyTransfers>();
            movementGoodsInfos = new HashSet<movementGoodsInfos>();
            serialNumberLogs = new HashSet<serialNumberLogs>();
        }

        public Guid id { get; set; }

        public DateTime dateCreate { get; set; }

        [Column(TypeName = "money")]
        public decimal? rate { get; set; }

        [StringLength(50)]
        public string textInfo { get; set; }

        public int? idArrivalStore { get; set; }

        public int? idDisposalStore { get; set; }

        public int? idCounterparty { get; set; }

        public int? idCurrency { get; set; }

        public int idType { get; set; }

        public DateTime? dateClose { get; set; }

        public bool isGoodsIssued { get; set; }

        public virtual counterparties counterparties { get; set; }

        public virtual currencies currencies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<moneyTransfers> moneyTransfers { get; set; }

        public virtual stores stores { get; set; }

        public virtual stores stores1 { get; set; }

        public virtual movmentGoodTypes movmentGoodTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<movementGoodsInfos> movementGoodsInfos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<serialNumberLogs> serialNumberLogs { get; set; }
    }
}
