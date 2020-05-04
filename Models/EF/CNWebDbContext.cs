namespace Models.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CNWebDbContext : DbContext
    {
        public CNWebDbContext()
            : base("name=CNWebDbContext")
        {
        }

        public virtual DbSet<CATEGORY> CATEGORies { get; set; }
        public virtual DbSet<CUSTOMER> CUSTOMERs { get; set; }
        public virtual DbSet<ORDER> ORDERs { get; set; }
        public virtual DbSet<ORDERDETAIL> ORDERDETAILs { get; set; }
        public virtual DbSet<ORDERSTATU> ORDERSTATUS { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTs { get; set; }
        public virtual DbSet<PRODUCTDETAIL> PRODUCTDETAILs { get; set; }
        public virtual DbSet<PRODUCTIMAGE> PRODUCTIMAGEs { get; set; }
        public virtual DbSet<SIZE> SIZEs { get; set; }
        public virtual DbSet<USER> USERs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CUSTOMER>()
                .HasMany(e => e.ORDERs)
                .WithOptional(e => e.CUSTOMER)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ORDER>()
                .HasMany(e => e.ORDERDETAILs)
                .WithOptional(e => e.ORDER)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ORDERSTATU>()
                .HasMany(e => e.ORDERs)
                .WithOptional(e => e.ORDERSTATU)
                .HasForeignKey(e => e.OrderStatusID);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.ProductPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.PromotionPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PRODUCT>()
                .HasMany(e => e.ORDERDETAILs)
                .WithOptional(e => e.PRODUCT)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PRODUCT>()
                .HasMany(e => e.PRODUCTDETAILs)
                .WithOptional(e => e.PRODUCT)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PRODUCT>()
                .HasMany(e => e.PRODUCTIMAGEs)
                .WithOptional(e => e.PRODUCT)
                .WillCascadeOnDelete();
        }
    }
}
