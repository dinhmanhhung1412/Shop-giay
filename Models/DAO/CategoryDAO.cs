﻿using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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

        public async Task<List<CATEGORY>> LoadDataProc()
        {
            return await db.CATEGORies.SqlQuery("CategoryList").ToListAsync();
        }

        public async Task<List<CATEGORY>> LoadData()
        {
            return await db.CATEGORies.ToListAsync();
        }

        public async Task<CATEGORY> LoadByIDProc(string id)
        {
            var param = new SqlParameter("@id", id);
            return await db.Database.SqlQuery<CATEGORY>("LoadMeta_ByID @id", param).SingleOrDefaultAsync();
        }
        public async Task<CATEGORY> LoadByID(int id)
        {
            return await db.CATEGORies.FindAsync(id);
        }

        public async Task<CATEGORY> LoadByMetaProc(string meta)
        {
            var param = new SqlParameter("@meta", meta);
            return await db.Database.SqlQuery<CATEGORY>("LoadByMeta_Cate @meta", param).SingleOrDefaultAsync();
        }

        public async Task<int> CreateCateProc(CATEGORY cate)
        {
            try
            {
                var id = new SqlParameter("@id", cate.CategoryID);
                var name = new SqlParameter("@name", cate.CategoryName);
                var meta = new SqlParameter("@meta", cate.MetaKeyword);
                return await db.Database.ExecuteSqlCommandAsync("Create_Category  @id,@name,@meta", id, name, meta);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteCateProc(string ID)
        {
            try
            {
                var param = new SqlParameter("@id", ID);
                var res = await db.Database.ExecuteSqlCommandAsync("Delete_Category @id", param);
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        public async Task<int> EditCateProc(CATEGORY cate, string ID)
        {
            try
            {
                var id = new SqlParameter("@id", ID);
                var name = new SqlParameter("@name", cate.CategoryName);
                var meta = new SqlParameter("@meta", cate.MetaKeyword);
                return await db.Database.ExecuteSqlCommandAsync("Edit_Category @id,@name,@meta", id, name, meta);
            }
            catch
            {
                return 0;
            }
        }
    }
}
