using Models.DAO;
using Models.EF;
using CNWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;

namespace CNWeb.Controllers
{
    public class CustomerController : Controller
    {
        [Route("login")]
        public ActionResult Login()
        {
            var name = HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(name))
            {
                return RedirectToAction("CustomerProfile", "Customer", new { username = name });
            }
            return View();
        }

        [HttpPost]
        public JsonResult ValidateUser(LoginModel model, bool RememberMe)
        {
            if (Membership.ValidateUser(model.CustomerUsername, model.CustomerPassword) && ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(model.CustomerUsername, RememberMe);
                return Json(new { Success = true, Username = model.CustomerUsername }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> RegisterCustomer(RegisterCustomer model)
        {
            var dao = new CustomerDAO();
            if (!await dao.CheckUser(model.CustomerUsername))
            {
                try
                {
                    int result = await new CustomerDAO().Register(new CUSTOMER()
                    {
                        CustomerUsername = model.CustomerUsername,
                        CustomerPassword = model.CustomerPassword,
                        CustomerName = model.CustomerName,
                        CustomerPhone = model.CustomerPhone,
                        CustomerEmail = model.CustomerEmail,
                        CreatedDate = DateTime.Now
                    });
                    return Json(new { ReturnID = 1 }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { ReturnID = 2 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { ReturnID = 0 }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [ActionName("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Customer");
        }

        [Authorize]
        [Route("profile/{username}")]
        public async Task<ActionResult> CustomerProfile(string username)
        {
            var membername = HttpContext.User.Identity.Name;
            if (!membername.Equals(username))
            {
                return View(await new CustomerDAO().LoadByUsername(membername));
            }
            return View(await new CustomerDAO().LoadByUsername(username));
        }
    }
}