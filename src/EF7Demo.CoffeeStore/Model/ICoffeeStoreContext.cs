using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EF7Demo.CoffeeStore.Model
{
    public interface ICoffeeStoreContext : IDisposable
    {
        DbSet<Coffee> Coffee { get; set; }
        DbSet<Order> Order { get; set; }


        IEnumerable<Coffee> FindCoffee(string searchTerm);


        int Commit();
    }
}