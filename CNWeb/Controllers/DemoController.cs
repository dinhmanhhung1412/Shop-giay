using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CNWeb.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult SelectProduct()
        {
            CNWebDbContext db = new CNWebDbContext();
            //Entity framework
            //var list = db.PRODUCTs.ToList();

            //procedure
            var list = db.Database.SqlQuery<PRODUCT>("SelectAllProduct").ToList();
            return View(list);
        }
    }
}