using Microsoft.EntityFrameworkCore;
using POS_ITS.MODEL.Entities;

namespace POS_ITS.DATA
{
    public class DataDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Inventory { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SalesProducts { get; set; }

        public DataDbContext() { }
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("ITS-POS");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            modelBuilder.Entity<Sale>().HasKey(s => s.SaleId);
            modelBuilder.Entity<SaleProduct>().HasKey(sp => sp.SaleProductId);

            //modelBuilder.Entity<SaleProduct>()
            //    .HasOne(sp => sp.Product)
            //    .WithMany()
            //    .HasForeignKey(sp => sp.ProductId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasMany(sp => sp.SaleProducts)
                .WithOne()
                .HasForeignKey(sp => sp.SaleProductId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
