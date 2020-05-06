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
using System.Data.Entity.Core.Common.CommandTrees;

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

        public string checknullphone(string phone)
        {
            if (phone == null) return ""; else return phone;            
        }
        public string checknullmail(string mail)
        {
            if (mail == null) return ""; else return mail;
        }

        [HttpPost]
        public  JsonResult RegisterCustomer(RegisterCustomer model)
        {
            var dao = new CustomerDAO();
            if (!dao.CheckUser(model.CustomerUsername))
            {
                try
                {
                    string checkmail = checknullmail(model.CustomerEmail);
                    string checkphone = checknullphone(model.CustomerPhone);
                    int result = new CustomerDAO().RegisterProc(new CUSTOMER()
                    {
                        CustomerUsername = model.CustomerUsername,
                        CustomerPassword = model.CustomerPassword,
                        CustomerName = model.CustomerName,
                        CustomerPhone = checkphone,
                        CustomerEmail = checkmail
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
        public  ActionResult CustomerProfile(string username)
        {
            var membername = HttpContext.User.Identity.Name;
            if (!membername.Equals(username))
            {
                return View( new CustomerDAO().LoadByUsernameProc(membername));
            }
            return View( new CustomerDAO().LoadByUsernameProc(username));
        }
    }
}