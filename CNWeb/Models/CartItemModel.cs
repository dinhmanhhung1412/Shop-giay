using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.DAO;
using Models.EF;

namespace CNWeb.Models
{
    public class CartItemModel
    {
        public PRODUCT product { get; set; }
        public int quantity { get; set; }

        public SIZE size { get; set; }

        public CartItemModel()
        { }

        public CartItemModel(PRODUCT product, SIZE size, int quantity)
        {
            this.product = product;
            this.size = size;
            this.quantity = quantity;
        }
    }
    public class CartSession
    {
        public string ProductID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }

        public CartSession()
        { }

        public CartSession(string ProductID, int SizeID, int Quantity)
        {
            this.ProductID = ProductID;
            this.SizeID = SizeID;
            this.Quantity = Quantity;
        }
    }
}