using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CNWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public ActionResult About()
        {

            return View();
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> NavbarCategory()
        {
            return PartialView("_NavbarCategory", await new CategoryDAO().LoadData());
        }
    }
}