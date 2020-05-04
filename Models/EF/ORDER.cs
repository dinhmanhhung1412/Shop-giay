namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ORDER")]
    public partial class ORDER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ORDER()
        {
            ORDERDETAILs = new HashSet<ORDERDETAIL>();
        }

        public int OrderID { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal? Total { get; set; }

        public int? OrderStatusID { get; set; }

        public int? CustomerID { get; set; }

        public virtual CUSTOMER CUSTOMER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDERDETAIL> ORDERDETAILs { get; set; }

        public virtual ORDERSTATU ORDERSTATU { get; set; }
    }
}
