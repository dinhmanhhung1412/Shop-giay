using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Models.DAO
{
    public class CustomerDAO
    {
        CNWebDbContext db = null;
        public CustomerDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<List<CUSTOMER>> LoadCustomerProc()
        {
            return await db.Database.SqlQuery<CUSTOMER>("Load_Customer").ToListAsync();
        }

        public async Task<bool> DeleteCustomer(int ID)
        {
            try
            {
                var cus = await db.CUSTOMERs.Where(x => x.CustomerID == ID).SingleOrDefaultAsync();
                db.CUSTOMERs.Remove(cus);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCustomerProc(int ID)
        {
            try
            {
                var param = new SqlParameter("@id", ID);
                var res = await db.Database.ExecuteSqlCommandAsync("Delete_Customer @id", param);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckUser(string username)
        {
            return db.CUSTOMERs.AsNoTracking().Any(x => x.CustomerUsername == username);
        }

        public async Task<CUSTOMER> LoadByIDProc(int id)
        {
            var param = new SqlParameter("@id", id);
            return await db.Database.SqlQuery<CUSTOMER>("LoadCustomer_ByID @id", param).SingleOrDefaultAsync();
        }

        public async Task<CUSTOMER> LoadByUsernameProc(string username)
        {
            var param = new SqlParameter("@username", username);
            return await db.CUSTOMERs.SqlQuery("LoadByUserName @username", param).SingleOrDefaultAsync();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            string encrypt = EncryptPassword(password);
            var result = await db.CUSTOMERs.AsNoTracking().CountAsync(x => x.CustomerUsername.Equals(username) && x.CustomerPassword.Equals(encrypt));
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool Login(string username, string password)
        {
            string encrypt = EncryptPassword(password);
            var result = db.CUSTOMERs.AsNoTracking().Count(x => x.CustomerUsername.Equals(username) && x.CustomerPassword.Equals(encrypt));
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<int> Register(CUSTOMER cus)
        {
            cus.CustomerPassword = EncryptPassword(cus.CustomerPassword);
            db.CUSTOMERs.Add(cus);
            await db.SaveChangesAsync();
            return cus.CustomerID;
        }
        public async Task<int> RegisterProc(CUSTOMER cus)
        {
            var db = new CNWebDbContext();
            var username = new SqlParameter("@username", cus.CustomerUsername);
            var pass = new SqlParameter("@pass", EncryptPassword(cus.CustomerPassword));
            var mail = new SqlParameter("@mail", cus.CustomerEmail);
            var name = new SqlParameter("@name", cus.CustomerName);
            var phone = new SqlParameter("@phone", cus.CustomerPhone);
            var res = await db.Database.ExecuteSqlCommandAsync("Create_Customer @username,@pass,@name,@phone,@mail", username, pass, name, phone, mail);
            return res;
        }
        public static string EncryptPassword(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
