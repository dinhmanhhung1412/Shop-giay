namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USER")]
    public partial class USER
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(250)]
        public string UserUsername { get; set; }

        [Required]
        [StringLength(250)]
        public string UserPassword { get; set; }

        [StringLength(250)]
        public string UserName { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
