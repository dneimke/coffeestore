using Microsoft.Data.Entity;
using System;
using System.Linq;

namespace ClassLibrary1.Models
{
    public class CoffeeContext : DbContext
    {
        public DbSet<Store> Store { get; set; }
        public DbSet<TradingDay> TradingDay { get; set; }
        public DbSet<CustomerOrder> CustomerOrder { get; set; }
        public DbSet<CoffeeCustomerOrder> CoffeeCustomerOrder { get; set; }
        public DbSet<Coffee> Coffee { get; set; }
        public DbSet<Image> Image { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString =
                @"Server=(localdb)\MSSQLLocalDB;Database=CoffeeShop;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coffee>()
                .HasKey(c => c.Coffee_Id);

            modelBuilder.Entity<Coffee>()
                .HasOne(p => p.Image)
                .WithOne()
                .HasForeignKey<Image>(e => e.Coffee_Id);

            modelBuilder.Entity<Coffee>()
                .Property<DateTime>("LastModified");
        }


        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var modifications = ChangeTracker
                .Entries<Coffee>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var mod in modifications)
            {
                mod.Property("LastModified").CurrentValue = DateTime.Now;
            }

            return base.SaveChanges();
        }
    }
}
