using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CNWeb.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult>  ValidateAdmin(USER model)
        {
            if (ModelState.IsValid)
            {
                var result = await new UserDAO().LoginAsync(model.UserUsername, model.UserPassword);
                if (result == true)
                {
                    Session["AdminLogin"] = model;
                    return Json(new { Success = true, Username = model.UserUsername }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["AdminLogin"] = null;
            return RedirectToAction("Login", "Admin");
        }
    }
}