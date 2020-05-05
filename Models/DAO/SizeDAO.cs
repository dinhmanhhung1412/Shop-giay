using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<List<SIZE>> LoadData()
        {
            return await db.SIZEs.AsNoTracking().ToListAsync();
        }

        public async Task<List<SIZE>> LoadDataProc()
        {
            return await db.SIZEs.SqlQuery("SizeList").ToListAsync();
        }

        public async Task<SIZE> LoadByID(int sizeID)
        {
            return await db.SIZEs.AsNoTracking().Where(x => x.SizeID == sizeID).FirstOrDefaultAsync();
        }

        public async Task<int> CreateSize(SIZE model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
                db.SIZEs.Add(model);
                await db.SaveChangesAsync();
                return model.SizeID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteSize(int ID)
        {
            try
            {
                var prod = await db.SIZEs.Where(x => x.SizeID == ID).SingleOrDefaultAsync();
                db.SIZEs.Remove(prod);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> EditSize(SIZE model, int ID)
        {
            try
            {
                var item = await db.SIZEs.FindAsync(ID);
                if (item == null)
                {
                    return 0;
                }
                item.Size1 = model.Size1;
                await db.SaveChangesAsync();
                return item.SizeID;
            }
            catch
            {
                return 0;
            }
        }
    }
}
