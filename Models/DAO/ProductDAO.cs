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
            //db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<bool> DeleteProductProc<T>(string ID)
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

        public async Task<PRODUCT> LoadByIDProc(string ID)
        {
            var param = new SqlParameter("@id", ID);
            return await db.Database.SqlQuery<PRODUCT>("LoadProd_ByID @id", param).FirstOrDefaultAsync();
        }

        public async Task<PRODUCT> LoadByID(int ID)
        {
            return await db.PRODUCTs.FindAsync(ID);
        }

        public async Task<List<PRODUCT>> LoadProductProc()
        {
            return await db.Database.SqlQuery<PRODUCT>("ProductList").ToListAsync();
        }

        public async Task<List<PRODUCT>> LoadName(string prefix)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductName.Contains(prefix)).ToListAsync();
        }

        public async Task<PRODUCT> LoadNameByID(string id)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductID == id).FirstOrDefaultAsync();
        }

        public async Task<List<PRODUCT>> LoadProduct(string cate, string searchString, string sort, int pagesize, int pageindex)
        {
            //get list
            var list = (from s in db.PRODUCTs select s).AsNoTracking();

            if (!String.IsNullOrEmpty(cate.ToString()))
            {
                list = list.Where(x => x.CategoryID == cate);
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

            return await list.Skip(pagesize * pageindex).Take(pagesize).ToListAsync();
        }

        public async Task<List<PRODUCT>> LoadProductProc(int? cate, string searchString, string sort, int pagesize, int pageindex)
        {
            var PageSize = new SqlParameter("@PageSize", pagesize);
            var PageIndex = new SqlParameter("@PageIndex", pageindex);
            var Sort = new SqlParameter("@Sort", sort ?? (object)DBNull.Value);
            var Search = new SqlParameter("@Search", searchString ?? (object)DBNull.Value);
            var Cate = new SqlParameter("@Cate", cate ?? (object)DBNull.Value);

            return await db.Database.SqlQuery<PRODUCT>("SelectPaging @PageSize, @PageIndex, @Sort, @Search, @Cate", PageSize, PageIndex, Sort, Search, Cate).ToListAsync();
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

        public async Task<List<PRODUCT>> SelectTop(int number)
        {
            //return  await db.Database.SqlQuery<PRODUCT>("").ToListAsync();
            return await db.PRODUCTs.ToListAsync();
        }

        public async Task <int> UpdateView(string prodID)
        {
            var id = new SqlParameter("@id", prodID);
            return await db.Database.SqlQuery<int>("Update_ViewCount @id",id).FirstOrDefaultAsync();
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
