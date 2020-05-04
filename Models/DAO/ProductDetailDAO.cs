using Models.EF;
using System;
using System.Collections.Generic;
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
                    db.PRODUCTDETAILs.Add(new PRODUCTDETAIL { ProductID = prodID, SizeID = Int32.Parse(item) });
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<SIZE>> LoadSize(int prodID)
        {
            var list = new List<SIZE>();
            var dbs = new SizeDAO();
            foreach (var item in db.PRODUCTDETAILs.AsNoTracking().Where(x => x.ProductID == prodID).ToList())
            {
                list.Add(await dbs.LoadByID(item.SizeID.Value));
            }
            return list;
        }
    }
}
