using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string encrypt = Models.DAO.CustomerDAO.EncryptPassword(password);
            var result = await db.USERs.AsNoTracking().CountAsync(x => x.UserUsername.Equals(username) && x.UserPassword.Equals(encrypt));
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> CheckUser(string username)
        {
            return await db.CUSTOMERs.AsNoTracking().AnyAsync(x => x.CustomerUsername == username);
        }

    }
}
