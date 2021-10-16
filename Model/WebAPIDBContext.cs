using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Model
{
    public partial class WebAPIDBContext : DbContext
    {
        public WebAPIDBContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<EVoucher> EVoucher { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<Transcation> Transcation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EVoucher>(entity =>
            {
                entity.ToTable("EVoucher", "dbo");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment", "dbo");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase", "dbo");
            });

            modelBuilder.Entity<Transcation>(entity =>
            {
                entity.ToTable("Transcation", "dbo");
            });
        }
    }
}
