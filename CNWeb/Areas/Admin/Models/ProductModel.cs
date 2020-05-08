using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CNWeb.Areas.Admin.Models
{
    public class ProductModel
    {
        [Display(Name = "ID")]
        [Required]
        public int ProductID { get; set; }

        [Display(Name = "Name")]
        [Required]
        [StringLength(250)]
        public string ProductName { get; set; }

        [Display(Name = "Price")]
        [Required]
        public decimal ProductPrice { get; set; }


        [Display(Name = "Decription")]
        public string ProductDescription { get; set; }

        [Display(Name = "Promotion Price")]
        public decimal? PromotionPrice { get; set; }

        [Display(Name = "Stock")]
        public int? ProductStock { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Select Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Show Image 1")]
        [StringLength(250)]
        public string ShowImage_1 { get; set; }

        [Display(Name = "Show Image 1")]
        [StringLength(250)]
        public string ShowImage_2 { get; set; }

        [Required]
        [Display(Name = "Size Option")]
        public List<string> Size { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool ProductStatus { get; set; }
    }
}