namespace ModelModul
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class propertyProducts
    {
        public long id { get; set; }

        public long idProduct { get; set; }

        public int? idPropertyName { get; set; }

        public int? idPropertyValue { get; set; }

        public virtual products products { get; set; }

        public virtual propertyNames propertyNames { get; set; }

        public virtual propertyValues propertyValues { get; set; }
    }
}
