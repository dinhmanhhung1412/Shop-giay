namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CUSTOMER")]
    public partial class CUSTOMER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CUSTOMER()
        {
            ORDERs = new HashSet<ORDER>();
        }

        public int CustomerID { get; set; }

        [Required]
        [StringLength(250)]
        public string CustomerUsername { get; set; }

        [Required]
        [StringLength(250)]
        public string CustomerPassword { get; set; }

        [StringLength(250)]
        public string CustomerEmail { get; set; }

        [Required]
        [StringLength(250)]
        public string CustomerName { get; set; }

        [StringLength(20)]
        public string CustomerPhone { get; set; }

        [StringLength(250)]
        public string CustomerAdress { get; set; }

        public DateTime? CreatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDER> ORDERs { get; set; }
    }
}
