using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CategoryDAO
    {
        CNWebDbContext db = null;
        public CategoryDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<List<CATEGORY>> LoadData()
        {
            return await db.CATEGORies.AsNoTracking().ToListAsync();
        }

        public async Task<CATEGORY> LoadByID(int id)
        {
            return await db.CATEGORies
                .AsNoTracking()
                .Where(x => x.CategoryID.Equals(id))
                .SingleOrDefaultAsync();
        }

        public async Task<CATEGORY> LoadByMeta(string meta)
        {
            return await db.CATEGORies
                .AsNoTracking()
                .Where(x => x.MetaKeyword.Equals(meta))
                .SingleOrDefaultAsync();
        }

        public async Task<int> CreateCate(CATEGORY cate)
        {
            try
            {
                db.CATEGORies.Add(cate);
                await db.SaveChangesAsync();
                return cate.CategoryID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteCate(int ID)
        {
            try
            {
                var prod = await db.CATEGORies.Where(x => x.CategoryID == ID).SingleOrDefaultAsync();
                db.CATEGORies.Remove(prod);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> EditCate(CATEGORY cate, int ID)
        {
            try
            {
                var item = db.CATEGORies.Find(ID);
                if (item == null)
                {
                    return 0;
                }
                item.CategoryName = cate.CategoryName;
                item.MetaKeyword = cate.MetaKeyword;
                await db.SaveChangesAsync();
                return item.CategoryID;
            }
            catch
            {
                return 0;
            }
        }
    }
}
