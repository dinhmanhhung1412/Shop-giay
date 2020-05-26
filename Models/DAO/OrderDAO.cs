using Models.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDAO
    {
        CNWebDbContext db = null;
        public OrderDAO()
        {
            db = new CNWebDbContext();

        }

        public async Task<List<ORDER>> LoadOrderProc()
        {
            return await db.ORDERs.SqlQuery("Load_Order").AsNoTracking().ToListAsync();
        }

        public async Task<int> AddOrderProc(int CustomerID, decimal total)
        {
            try
            {
                var returnID = new SqlParameter("@Return", SqlDbType.Int);
                returnID.Direction = ParameterDirection.Output;

                var cusID = new SqlParameter("@cusID", CustomerID);
                var tot = new SqlParameter("@total", total);
                var OuputID = new SqlParameter("@ReturnID", SqlDbType.Int);
                OuputID.Direction = ParameterDirection.Output;
            https://us04web.zoom.us/j/79701717043?pwd=ZFFWV1hwZVU2VWg0bkpFUlo2dVVTUT09
                var data = db.Database.SqlQuery<int>(@"exec @Return = Add_Order_Alt @cusID, @total, @ReturnID OUTPUT", returnID, cusID, tot, OuputID);

                var result = await data.FirstAsync();
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<List<ORDER>> LoadOrder(int CustomerID)
        {
            var param = new SqlParameter("@cusID", CustomerID);
            return await db.Database.SqlQuery<ORDER>("Load_CustomerOrder @cusID", param).ToListAsync();
        }

        public async Task<List<T>> LoadOrder<T>(int CustomerID)
        {
            var Param = new SqlParameter("@CustomerID", CustomerID);

            return await new CNWebDbContext().Database.SqlQuery<T>("SelectOrder @CustomerID", Param).ToListAsync();
        }

        public async Task<List<T>> LoadProductOrder<T>(int OrderID)
        {
            var Param = new SqlParameter("@OrderID", OrderID);

            return await new CNWebDbContext().Database
                 .SqlQuery<T>("SelectOrderProduct @OrderID", Param)
                 .ToListAsync();
        }

        public async Task<int> CancelOrderProc(int OrderID)
        {
            try
            {
                var ID = new SqlParameter("@orderID", OrderID);
                return await db.Database.ExecuteSqlCommandAsync("Cancel_Order @orderID", ID);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> ChangeOrderProc(int OrderID, int StatusID)
        {
            try
            {
                var orderID = new SqlParameter("@orderID", OrderID);
                var statusID = new SqlParameter("@statusID", StatusID);
                return await db.Database.ExecuteSqlCommandAsync("Change_Order @orderID,@statusID", orderID, statusID);
            }
            catch
            {
                return 0;
            }
        }
    }
}
