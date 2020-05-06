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
    public class SizeDAO
    {
        CNWebDbContext db = null;
        public SizeDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<List<SIZE>> LoadDataProc()
        {
            return await db.SIZEs.ToListAsync();
        }

        public  SIZE LoadByID(int sizeID)
        {
            return  db.SIZEs.AsNoTracking().Where(x => x.SizeID == sizeID).FirstOrDefault();
        }

        public int CreateSizeProc(SIZE model)
        {
            try
            {
                var param = new SqlParameter("@sizeName", model.Size1);
                var res = db.Database.ExecuteSqlCommand("Create_Size @sizeName", param);
                return res;
            }
            catch
            {
                return 0;
            }
        }

        public bool DeleteSizeProc(int ID)
        {
            try
            {
                var param = new SqlParameter("@sizeID", ID);
                var res = db.Database.ExecuteSqlCommand("Delete_Size @sizeID", param);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int EditSizeProc(SIZE model, int ID)
        {
            try
            {
                var id = new SqlParameter("@sizeID", ID);
                var name = new SqlParameter("@sizeName", model.Size1);
                var res = db.Database.ExecuteSqlCommand("Update_Size @sizeID,@sizeName", id, name);
                return res;
            }
            catch
            {
                return 0;
            }
        }
    }
}
