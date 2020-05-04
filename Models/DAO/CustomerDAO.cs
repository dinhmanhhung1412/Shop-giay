using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

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

        public async Task<List<CUSTOMER>> LoadCustomer()
        {
            return await db.CUSTOMERs.AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteCustomer(int ID)
        {
            try
            {
                var cus = await db.CUSTOMERs.Where(x => x.CustomerID== ID).SingleOrDefaultAsync();
                db.CUSTOMERs.Remove(cus);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckUser(string username)
        {
            return await db.CUSTOMERs.AsNoTracking().AnyAsync(x => x.CustomerUsername == username);
        }

        public async Task<CUSTOMER> LoadByID(int id)
        {
            return await db.CUSTOMERs.AsNoTracking().Where(x => x.CustomerID == id).SingleOrDefaultAsync();
        }

        public async Task<CUSTOMER> LoadByUsername(string username)
        {
            return await db.CUSTOMERs.AsNoTracking().Where(x => x.CustomerUsername.Equals(username)).SingleOrDefaultAsync();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            string encrypt = EncryptPassword(password);
            var result = await db.CUSTOMERs.AsNoTracking().CountAsync(x => x.CustomerUsername.Equals(username)  && x.CustomerPassword.Equals(encrypt));
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

        public static string EncryptPassword(string text)
        {
            string password = "";
            if (!string.IsNullOrEmpty(text))
            {
                byte[] data = new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

                foreach (byte item in data)
                {
                    password += item;
                }
            }
            return password;
        }
    }
}
