using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CNWeb.Areas.Admin.Controllers
{
    public class CustomerController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CustomerList()
        {
            return PartialView("CustomerList", await new CustomerDAO().LoadCustomerProc());
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCustomer(int id)
        {
            if (await new CustomerDAO().LoadByIDProc(id) == null)
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (!(await new CustomerDAO().DeleteCustomerProc(id)))
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}