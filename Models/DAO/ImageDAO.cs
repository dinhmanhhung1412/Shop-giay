using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ImageDAO
    {
        CNWebDbContext db = null;
        public ImageDAO()
        {
            db = new CNWebDbContext();
            db.Configuration.ProxyCreationEnabled = false;
        }

        public PRODUCTIMAGE LoadImage(int id)
        {
            return db.PRODUCTIMAGEs.ToList().Find(x => x.ProductID == id);
        }

        public int InsertImage(PRODUCTIMAGE img)
        {
            db.PRODUCTIMAGEs.Add(img);
            db.SaveChanges();
            return img.ImageID;
        }

        public int Update(PRODUCTIMAGE img)
        {
            var Img = db.PRODUCTIMAGEs.ToList().Find(s => s.ProductID == img.ProductID);
            if (Img == null)
            {
                InsertImage(img);
            }
            else
            {
                Img = img;
                db.SaveChanges();
            }
            return img.ImageID;
        }
    }
}
