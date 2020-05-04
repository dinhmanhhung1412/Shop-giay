namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRODUCTDETAIL")]
    public partial class PRODUCTDETAIL
    {
        public int ProductDetailID { get; set; }

        public int? ProductID { get; set; }

        public int? SizeID { get; set; }

        public virtual PRODUCT PRODUCT { get; set; }

        public virtual SIZE SIZE { get; set; }
    }
}
