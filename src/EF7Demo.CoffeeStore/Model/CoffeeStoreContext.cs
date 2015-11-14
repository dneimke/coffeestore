using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace EF7Demo.CoffeeStore.Model
{
    public class CoffeeStoreContext : DbContext, ICoffeeStoreContext
    {
        public DbSet<Coffee> Coffee { get; set; }

        public DbSet<Order> Order { get; set; }


        public IEnumerable<Coffee> FindCoffee(string searchTerm)
        {
            return this.Coffee.FromSql("[dbo].[SearchForCoffee] @searchTerm = {0}", searchTerm);
        }

        public int Commit()
        {
            return base.SaveChanges();
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CoffeeShop;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coffee>()
                .HasKey(c => c.Coffee_Id);
        }

        
    }
}
