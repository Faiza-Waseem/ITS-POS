﻿using System;
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

            modelBuilder.Entity<SaleProduct>()
                .HasOne(sp => sp.Product)
                .WithMany()
                .HasForeignKey(sp => sp.ProductId);

            modelBuilder.Entity<Sale>()
               .HasMany(s => s.SaleProducts)
               .WithOne()
               .HasForeignKey(sp => sp.SaleProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
