using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductDetailDAO
    {
        CNWebDbContext db = null;
        public ProductDetailDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<bool> AddProductDetail(int prodID, List<string> sizesID)
        {
            try
            {
                foreach (var item in sizesID)
                {
                    db.PRODUCTDETAILs.Add(new PRODUCTDETAIL() { ProductID = prodID, SizeID = int.Parse(item) });
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddProductDetailProc(int prodID, List<string> sizesID)
        {
            try
            {
                foreach (var item in sizesID)
                {
                    var prod = new SqlParameter("@prodID", prodID);
                    var size = new SqlParameter("@sizeID", int.Parse(item));
                    await db.Database.ExecuteSqlCommandAsync("Add_ProductDetail @prodID, @sizeID", prod, size);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProductDetailProc(int prodID, List<string> sizesID)
        {
            try
            {
                foreach (var item in sizesID)
                {
                    SqlParameter[] param =
                    {
                         new SqlParameter {ParameterName = "@prodID", Value = prodID },
                         new SqlParameter {ParameterName = "@sizeID", Value = int.Parse(item) }
                    };
                    await db.Database.ExecuteSqlCommandAsync("Update_ProductDetail @prodID, @sizeID", param);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteProductDetail(int prodID)
        {
            try
            {
                var param = new SqlParameter("@prodID", prodID);
                await db.Database.ExecuteSqlCommandAsync("Delete_ProductDetail @prodID", prodID);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //public async Task<List<SIZE>> LoadSize(int prodID)
        //{
        //    var list = new List<SIZE>();
        //    var dbs = new SizeDAO();
        //    foreach (var item in db.PRODUCTDETAILs.AsNoTracking().Where(x => x.ProductID == prodID).ToList())
        //    {
        //        list.Add(await dbs.LoadByID(item.SizeID.Value));
        //    }
        //    return list;
        //}

        public List<PRODUCTDETAIL> LoadByProductID(int prodID)
        {
            var param = new SqlParameter("@prodID", prodID);
            return db.Database.SqlQuery<PRODUCTDETAIL>("LoadSize_ByProdID @prodID", param).ToList();
        }

        public async Task<List<SIZE>> LoadSizeProc(int prodID)
        {
            var list = new List<SIZE>();
            var dbs = new SizeDAO();
            foreach (var item in LoadByProductID(prodID))
            {
                list.Add(await dbs.LoadByIDProc(item.SizeID.Value));
            }
            return list;
        }


    }
}
