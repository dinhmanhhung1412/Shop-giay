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
    public class ProductController : BaseController
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
        public async Task<JsonResult>  DeleteProduct(int id)
        {
            if ( new ProductDAO().LoadByIDProc(id) == null)
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (!(await new ProductDAO().DeleteProductProc<PRODUCT>(id)))
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = 1 }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ProductList()
        {
            return View(await new ProductDAO().LoadProductProc());
        }

        public async Task<ActionResult> CreateProduct()
        {
            ViewBag.Cate = await new CategoryDAO().LoadDataProc();
            ViewBag.Size = await new SizeDAO().LoadDataProc();
            return View();
        }
       
        [HttpPost]
        public async Task<JsonResult> CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                string descript = new ProductDAO().checkdes(model.ProductDescription);
                string img_1 = new ProductDAO().checkimg1(model.ShowImage_1);
                string img_2 = new ProductDAO().checkimg2(model.ShowImage_2);
                var db = new CNWebDbContext();
                var name = new SqlParameter("@name", model.ProductName);
                var des = new SqlParameter("@description", descript);
                var price = new SqlParameter("@price", model.ProductPrice);
                var promotion = new SqlParameter("@promotionprice", model.PromotionPrice);
                var img1 = new SqlParameter("@img1", img_1);
                var img2 = new SqlParameter("@img2",img_2);
                var stock = new SqlParameter("@stock", model.ProductStock);
                var meta = new SqlParameter("@meta", SlugGenerator.SlugGenerator.GenerateSlug(model.ProductName));
                var status = new SqlParameter("@status", model.ProductStatus);
                var cate = new SqlParameter("@cate", model.CategoryID);

                var result = await db.Database.ExecuteSqlCommandAsync("Create_Product @name,@description,@price,@promotionprice,@img1,@img2,@stock,@meta,@status,@cate", name, des, price, promotion, img1, img2, stock, meta, status, cate);

                var res = new ProductDetailDAO().AddProductDetailProc(result, model.Size);

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
                int result = await new SizeDAO().CreateSizeProc(model);
                return RedirectToAction("Size");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SizeList()
        {
            return PartialView("SizeList", await new SizeDAO().LoadDataProc());
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSize(int id)
        {
            if ( await new SizeDAO().LoadByIDProc(id) == null)
            {
                return Json(new { Success = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (!( await new SizeDAO().DeleteSizeProc(id)))
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
                int result = await new SizeDAO().EditSizeProc(model, id);
                if (result == 0)
                {
                    return Json(new { Success = false, id }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, id }, JsonRequestBehavior.AllowGet);
        }

        //public async Task<ActionResult> EditProduct(int id)
        //{
        //    ViewBag.Cate = await new CategoryDAO().LoadDataProc();
        //    ViewBag.Size = await new SizeDAO().LoadDataProc();
        //    return View();
        //}

        //[HttpPost]
        //public async Task<JsonResult> EditProduct(ProductModel model,int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var prod = new ProductDAO().LoadNameByID(id);
                
        //        string descript = checkdes(model.ProductDescription);
        //        string img_1 = checkimg1(model.ShowImage_1);
        //        string img_2 = checkimg2(model.ShowImage_2);
        //        var db = new CNWebDbContext();
        //        var name = new SqlParameter("@name", model.ProductName);
        //        var des = new SqlParameter("@description", descript);
        //        var price = new SqlParameter("@price", model.ProductPrice);
        //        var promotion = new SqlParameter("@promotionprice", model.PromotionPrice);
        //        var img1 = new SqlParameter("@img1", img_1);
        //        var img2 = new SqlParameter("@img2", img_2);
        //        var stock = new SqlParameter("@stock", model.ProductStock);
        //        var meta = new SqlParameter("@meta", SlugGenerator.SlugGenerator.GenerateSlug(model.ProductName));
        //        var status = new SqlParameter("@status", model.ProductStatus);
        //        var cate = new SqlParameter("@cate", model.CategoryID);

        //        var result = await db.Database.ExecuteSqlCommandAsync("Create_Product @name,@description,@price,@promotionprice,@img1,@img2,@stock,@meta,@status,@cate", name, des, price, promotion, img1, img2, stock, meta, status, cate);

        //        var res = new ProductDetailDAO().AddProductDetail(result, model.Size);

        //        return Json(new { Success = true, id = 1 }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        //}
    }
}