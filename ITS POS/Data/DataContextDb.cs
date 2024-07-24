using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITS_POS.Entities;

using Microsoft.EntityFrameworkCore;

namespace ITS_POS.Data
{
    public class DataContextDb : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Inventory { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }

        public DataContextDb() { }
        public DataContextDb(DbContextOptions<DataContextDb> options): base(options) { }

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

            modelBuilder.Entity<SaleProduct>().HasOne(sp => sp.Sale)
                .WithMany(s => s.SaleProducts).HasForeignKey(sp => sp.SaleId);

            modelBuilder.Entity<SaleProduct>().HasOne(sp => sp.Product)
                .WithMany(p => p.Sales).HasForeignKey(sp => sp.ProductId);
        }
    }
}
