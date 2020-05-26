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

        public async Task<SIZE> LoadByID(int sizeID)
        {
            return await db.SIZEs.AsNoTracking().Where(x => x.SizeID == sizeID).FirstOrDefaultAsync();
        }

        public async Task<SIZE> LoadByIDProc(int sizeID)
        {
            try
            {
                var param = new SqlParameter("@id", sizeID);
                return await db.Database.SqlQuery<SIZE>("LoadSize_ByID @id", param).FirstOrDefaultAsync();
            }
            catch
            {
                return await LoadByID(sizeID);
            }
        }

        public async Task<int> CreateSizeProc(SIZE model)
        {
            try
            {
                var id = new SqlParameter("@id", model.SizeID);
                var size = new SqlParameter("@sizeName", model.Size1);
                var res = await db.Database.ExecuteSqlCommandAsync("Create_Size @id, @sizeName", id,size);
                return res;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteSizeProc(int ID)
        {
            try
            {
                var param = new SqlParameter("@sizeID", ID);
                var res = await db.Database.ExecuteSqlCommandAsync("Delete_Size @sizeID", param);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> EditSizeProc(SIZE model, int ID)
        {
            try
            {
                var id = new SqlParameter("@sizeID", ID);
                var name = new SqlParameter("@sizeName", model.Size1);
                var res = await db.Database.ExecuteSqlCommandAsync("Update_Size @sizeID,@sizeName", id, name);
                return res;
            }
            catch
            {
                return 0;
            }
        }
    }
}
