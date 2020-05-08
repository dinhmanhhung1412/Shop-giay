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

        public async Task<bool> DeleteProductProc<T>(int ID)
        {
            try
            {
                var prod = LoadByIDProc(ID);
                if (prod != null)
                {
                    var id = new SqlParameter("@id", ID);
                    await new CNWebDbContext().Database.SqlQuery<T>("Delete_Product @id", id).FirstOrDefaultAsync();
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

        public async Task<PRODUCT> LoadByIDProc(int ID)
        {
            var param = new SqlParameter("@id", ID);
            return await db.Database.SqlQuery<PRODUCT>("LoadProd_ByID @id", param).FirstOrDefaultAsync();
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

        public async Task<PRODUCT>  LoadByMeta(string meta)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.MetaKeyword == meta).FirstOrDefaultAsync();
        }

        public async Task<PRODUCT> LoadByMetaProc(string meta)
        {
            var param = new SqlParameter("@meta", meta);
            return await db.Database.SqlQuery<PRODUCT>("LoadByMeta_Prod @meta", param).FirstOrDefaultAsync();
        }

        public string checkdes(string des)
        {
            if (des == null) return ""; else return des;
        }
        public string checkimg1(string img)
        {
            if (img == null) return ""; else return img;
        }
        public string checkimg2(string img)
        {
            if (img == null) return ""; else return img;
        }

    }
}
