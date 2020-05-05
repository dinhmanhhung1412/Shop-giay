using Models.DAO;
using Models.EF;
using CNWeb.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;

namespace CNWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        /// <summary>
        /// Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult DeleteProduct(int id)
        //{
        //    if (new ProductDAO().LoadByID(id) == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    if (!(new ProductDAO().Delete(id)))
        //    {
        //        return HttpNotFound();
        //    }
        //    return RedirectToAction("ProductList");
        //}

        [HttpPost]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            if (await new ProductDAO().LoadByID(id) == null)
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (!(await new ProductDAO().DeleteProductProc<PRODUCT>(id)))
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = 1 }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ProductDetail(int id)
        {
            return View(await new ProductDAO().LoadByID(id));
        }

        public async Task<ActionResult> ProductList()
        {
            return View(await new ProductDAO().LoadProductProc());
        }

        public async Task<ActionResult> CreateProduct()
        {
            ViewBag.Cate = await new CategoryDAO().LoadDataProc();
            ViewBag.Size = await new SizeDAO().LoadData();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var prod = new PRODUCT
                {                 
                    ProductName = model.ProductName,
                    ProductPrice = model.ProductPrice,
                    ProductDescription = model.ProductDescription,
                    PromotionPrice = model.PromotionPrice,
                    ProductStock = model.ProductStock,
                    CategoryID = model.CategoryID,
                    MetaKeyword = SlugGenerator.SlugGenerator.GenerateSlug(model.ProductName),
                    ShowImage_1 = model.ShowImage_1,
                    ShowImage_2 = model.ShowImage_2,
                    ProductStatus = model.ProductStatus,
                    CreatedDate = DateTime.Now
                };
                int result = await new ProductDAO().CreateProductProc(prod);
                var res = new ProductDetailDAO().AddProductDetail(result, model.Size);

                //int result = await new ProductDAO().CreateProductProc(prod);
                //var res = new ProductDetailDAO().AddProductDetail(result, model.Size);
                return Json(new { Success = true, id = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Size
        /// </summary>
        /// <returns></returns>
        [ActionName("Size")]
        public ActionResult CreateSize()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateSize(SIZE model)
        {
            if (ModelState.IsValid)
            {
                int result = await new SizeDAO().CreateSize(model);
                return RedirectToAction("Size");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SizeList()
        {
            return PartialView("SizeList", await new SizeDAO().LoadData());
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSize(int id)
        {
            if (await new SizeDAO().LoadByID(id) == null)
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (!(await new SizeDAO().DeleteSize(id)))
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = 1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> EditSize(SIZE model, int id)
        {
            if (ModelState.IsValid)
            {
                int result = await new SizeDAO().EditSize(model, id);
                if (result == 0)
                {
                    return Json(new { Success = false, id }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, id }, JsonRequestBehavior.AllowGet);
        }
    }
}