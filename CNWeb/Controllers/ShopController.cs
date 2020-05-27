using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CNWeb.Controllers
{
    public class ShopController : Controller
    {
        [HttpGet]
        [Route("shop")]
        public ActionResult Shop(string search, string sort)
        {
            ViewBag.search = HttpUtility.UrlEncode(search);
            ViewBag.sort = sort;
            return View();
        }

        [HttpGet]
        [Route("cate/{url}-{id:int}")]
        public async Task<ActionResult> ShopCategory(string id, string url, string sort)
        {
            var cate = await new CategoryDAO().LoadByIDProc(id);
            if (cate == null)
            {
                return HttpNotFound();
            }
            ViewBag.meta = cate;
            ViewBag.sort = sort;
            return View();
        }

        [HttpGet]
        [Route("san-pham/{url}-{id:int}")]
        public async Task<ActionResult> Detail(string id, string url)
        {
            try
            {
                var prod = await new ProductDAO().LoadByIDProc(id);
                prod.ViewCount = await new ProductDAO().UpdateView(id);
                if (prod == null)
                {
                    return HttpNotFound();
                }
                ViewData["Size"] = await new ProductDetailDAO().LoadSizeProc(id);
                return View(prod);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> ShopPartial(int? cate, string search, string sort, int pageindex)
        {
            search = HttpUtility.UrlDecode(search);
            int pagesize = 16;
            return PartialView("ShopPartial",
                await new ProductDAO().LoadProductProc(cate, search, sort, pagesize, pageindex));
        }

        [HttpGet]
        public async Task<ActionResult> SelectTop(int number = 5)
        {
            var list = await new ProductDAO().SelectTop(number);
            return PartialView(list);
        }

        [HttpPost]
        public async Task<JsonResult> GetProductName(string prefix)
        {
            var prod = await new ProductDAO().LoadName(prefix);

            return Json(prod, JsonRequestBehavior.AllowGet);
        }
    }
}