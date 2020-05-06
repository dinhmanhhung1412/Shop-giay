using Models.DAO;
using CNWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Models.EF;
using System.Data.SqlClient;

namespace CNWeb.Controllers
{
    public class CartController : Controller
    {
        [AllowAnonymous]
        [Route("cart")]
        public ActionResult Cart()
        {
            return View(GetCartItem());
        }

        [Authorize]
        [Route("checkout")]
        public  ActionResult Checkout()
        {
            if (Session["cart"] == null)
            {
                return RedirectToAction("Cart", "Cart");
            }
            var customer = HttpContext.User.Identity.Name;
            CustomerViewModel model = null;
            if (!string.IsNullOrEmpty(customer))
            {
                var item =  new CustomerDAO().LoadByUsernameProc(customer);
                model = new CustomerViewModel()
                {
                    CustomerID = item.CustomerID,
                    CustomerUsername = item.CustomerUsername,
                    CustomerName = item.CustomerName,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone
                };
            }
            ViewData["Customer"] = model;
            return View(GetCartItem());
        }

        [HttpPost]
        public JsonResult OrderNow(int prodId, int sizeId, int quantity)
        {
            if (Session["cart"] == null)
            {
                var cart = new List<CartSession>();
                cart.Add(new CartSession(prodId, sizeId, quantity));
                Session["cart"] = cart;

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cart = (List<CartSession>)Session["cart"];
                int index = IsExist(prodId);
                if (index == -1)
                {
                    cart.Add(new CartSession(prodId, sizeId, quantity));
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Delete(int id)
        {
            int index = IsExist(id);
            List<CartSession> cart = (List<CartSession>)Session["cart"];
            cart.RemoveAt(index);
            if (cart.Count == 0)
            {
                Session["cart"] = null;
            }
            return RedirectToAction("Cart", "Cart");
        }

        private int IsExist(int id)
        {
            var cart = (List<CartSession>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].ProductID == id)
                    return i;
            return -1;
        }

        public  ActionResult CartPartial()
        {
            return PartialView("_Cart",  GetCartItem());
        }

        public  List<CartItemModel> GetCartItem()
        {
            var list = new List<CartItemModel>();
            var session = (List<CartSession>)Session["cart"];
            if (session != null)
            {
                foreach (var item in session)
                {
                    list.Add(new CartItemModel(
                         new ProductDAO().LoadByID(item.ProductID),
                         new SizeDAO().LoadByID(item.SizeID),
                        item.Quantity));
                }
            }
            return list;
        }

        public  JsonResult SubmitCheckout()
        {
            try
            {      
                var customer = new CustomerDAO().LoadByUsernameProc(HttpContext.User.Identity.Name);           
                var order = new OrderDAO().AddOrderProc(customer.CustomerID, GetTotal());
                if (order!= 0)
                {
                    var orderdetail = new OrderDetailDAO();
                    foreach (var item in (List<CartSession>)Session["cart"])
                    {
                        _ = orderdetail.AddOrderDetailProc(order, item.ProductID, item.SizeID, item.Quantity);
                    }
                    Session["cart"] = null;
                    return Json(new { Success = true, ID = order }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public decimal GetTotal()
        {
            decimal total = 0;
            
            var cart = (List<CartSession>)Session["cart"];
            if (cart != null)
            {
                var dao = new ProductDAO();
                foreach (var item in cart)
                {
                    var product = dao.LoadByID(item.ProductID);
                    if (product.PromotionPrice.HasValue)
                    {
                        total += product.PromotionPrice.Value * item.Quantity;
                    } else
                    {
                        total += product.ProductPrice * item.Quantity;
                    }
                }
            }
            return total;
        }
    }
}