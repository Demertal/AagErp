namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class warranties
    {
        public int id { get; set; }

        [Required]
        [StringLength(256)]
        public string malfunction { get; set; }

        [Column(TypeName = "date")]
        public DateTime dateReceipt { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dateDeparture { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dateIssue { get; set; }

        [StringLength(256)]
        public string info { get; set; }

        public long idSerialNumber { get; set; }

        public long? idSerialNumberСhange { get; set; }

        public virtual serialNumbers serialNumbers { get; set; }

        public virtual serialNumbers serialNumbers1 { get; set; }
    }
}
