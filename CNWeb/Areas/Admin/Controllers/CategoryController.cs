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
    public class CategoryController : BaseController
    {
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CATEGORY model)
        {
            if (ModelState.IsValid)
            {
                model.MetaKeyword = SlugGenerator.SlugGenerator.GenerateSlug(model.CategoryName);
                
                int result = await new CategoryDAO().CreateCateProc(model);
                return RedirectToAction("CreateCategory");
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> EditCategory(CATEGORY model, int id)
        {
            if (ModelState.IsValid)
            {
                model.MetaKeyword = SlugGenerator.SlugGenerator.GenerateSlug(model.CategoryName);
                int result = await new CategoryDAO().EditCateProc(model, id);
                if (result == 0)
                {
                    return Json(new { Success = false, id }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CategoryList()
        {
            return PartialView("CategoryList", await new CategoryDAO().LoadDataProc());
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCategory(int id)
        {
            if (await new CategoryDAO().LoadByID(id) == null)
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (!(await new CategoryDAO().DeleteCateProc(id)))
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}