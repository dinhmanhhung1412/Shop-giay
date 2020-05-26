using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Models.DAO
{
    public class UserDAO
    {
        CNWebDbContext db = null;
        public UserDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }
        public async Task<bool> LoginAsync(string username, string password)
        {
            string encrypt = EncryptPassword(password);
            var result = await db.USERs.AsNoTracking().CountAsync(x => x.UserUsername.Equals(username) && x.UserPassword.Equals(encrypt));
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> LoginProc(string username, string password)
        {
            string encrypt = EncryptPassword(password);
            var usrname = new SqlParameter("@username", username);
            var pass = new SqlParameter("@pass", encrypt);
            var res = await db.Database.SqlQuery<bool>("Login_Admin @username,@pass", usrname, pass).FirstOrDefaultAsync();
            return res;
        }

        public async Task<int> RegisterProc(string username, string password, string name)
        {
            string encrypt = EncryptPassword(password);
            var usrname = new SqlParameter("@username", username);
            var pass = new SqlParameter("@pass", encrypt);
            var namee = new SqlParameter("@name", name);
            var res = await db.Database.ExecuteSqlCommandAsync("Create_User @username,@pass,@name",usrname,pass,namee);
            return res;
        }

        public async Task<bool> CheckUser(string username)
        {
            return await db.CUSTOMERs.AsNoTracking().AnyAsync(x => x.CustomerUsername == username);
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
