using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace Models.DAO
{
    public class ProductDAO
    {
        CNWebDbContext db = null;
        public ProductDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public int CreateProduct(PRODUCT model)
        {
            try
            {
                var db = new CNWebDbContext();
                var name = new SqlParameter("@name", model.ProductName);
                var des = new SqlParameter("@description", model.ProductDescription);
                var price = new SqlParameter("@price", model.ProductPrice);
                var promotion = new SqlParameter("@promotionprice", model.PromotionPrice);
                var img1 = new SqlParameter("@img1", model.ShowImage_1);
                var img2 = new SqlParameter("@img2", model.ShowImage_2);
                var stock = new SqlParameter("@stock", model.ProductStock);
                var meta = new SqlParameter("@meta", SlugGenerator.SlugGenerator.GenerateSlug(model.ProductName));
                var status = new SqlParameter("@status", model.ProductStatus);
                var cate = new SqlParameter("@cate", model.CategoryID);

                var result = db.Database.ExecuteSqlCommand("Create_Product @name,@description,@price,@promotionprice,@img1,@img2,@stock,@meta,@status,@cate", name, des, price, promotion, img1, img2, stock, meta, status, cate);
                return result;
            }
            catch
            {
                return 0;
            }
        }
        public bool DeleteProductProc<T>(int ID)
        {
            try
            {
                var prod = LoadByID(ID);
                if (prod != null)
                {
                    var id = new SqlParameter("@id", ID);
                    new CNWebDbContext().Database.SqlQuery<T>("Delete_Product @id", id).FirstOrDefaultAsync();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public PRODUCT LoadByID(int ID)
        {
            return db.PRODUCTs.AsNoTracking().Where(x => x.ProductID.Equals(ID)).FirstOrDefault();
        }

        public async Task<List<PRODUCT>> LoadProductProc()
        {
            return await db.Database.SqlQuery<PRODUCT>("ProductList").ToListAsync();
        }

        public async Task<List<PRODUCT>> LoadName(string prefix)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductName.Contains(prefix)).ToListAsync();
        }

        public async Task<PRODUCT> LoadNameByID(int id)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductID == id).FirstOrDefaultAsync();
        }

        public async Task<List<PRODUCT>> LoadProduct(string meta, string searchString, string sort, int pagesize, int pageindex)
        {
            // get list
            var list = (from s in db.PRODUCTs select s).AsNoTracking();

            if (!String.IsNullOrEmpty(meta))
            {
                var cate = db.CATEGORies.AsNoTracking().Where(c => c.MetaKeyword.Equals(meta)).First().CategoryID;
                list = list.Where(x => x.CategoryID.Equals(cate));
            }

            //filter
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.ProductName.Contains(searchString));
            }
            switch (sort)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.ProductName);
                    break;
                case "name_asc":
                    list = list.OrderBy(s => s.ProductName);
                    break;
                case "price_desc":
                    list = list.OrderByDescending(s => s.ProductPrice);
                    break;
                case "price_asc":
                    list = list.OrderBy(s => s.ProductPrice);
                    break;
                default:
                    list = list.OrderBy(s => s.ProductID);
                    break;
            }
            //return
            return await list.Skip(pageindex * pagesize).Take(pagesize).ToListAsync();
        }

        public PRODUCT LoadByMeta(string meta)
        {
            return db.PRODUCTs.AsNoTracking().Where(x => x.MetaKeyword == meta).FirstOrDefault();
        }
    }
}
