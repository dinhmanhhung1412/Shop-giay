using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDetailDAO
    {
        CNWebDbContext db = null;
        public OrderDetailDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<int> AddOrderDetail(int OrderID, int ProductID, int SizeID, int Quanity)
        {
            try {
                var order = new ORDERDETAIL()
                {
                    OrderID = OrderID,
                    ProductID = ProductID,
                    Quantity = Quanity,
                    SizeID = SizeID
                };
                db.ORDERDETAILs.Add(order);
                await db.SaveChangesAsync();
                return order.DetailID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<List<ORDERDETAIL>> LoadOrderDetail(int OrderID)
        {
            return await db.ORDERDETAILs.AsNoTracking().Where(x => x.OrderID == OrderID).ToListAsync();
        }
    }
}
