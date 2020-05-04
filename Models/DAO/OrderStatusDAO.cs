using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderStatusDAO
    {
        CNWebDbContext db = null;
        public OrderStatusDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<List<ORDERSTATU>> LoadStatus()
        {
            return await db.ORDERSTATUS.AsNoTracking().ToListAsync();
        }
    }
}
