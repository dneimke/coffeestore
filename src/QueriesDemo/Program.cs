using ClassLibrary1.Models;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    public class Program
    {
        public void Main()
        {
            CreateDatabase();
            SeedData();
            
            Console.WriteLine("Press any key to run queries...");
            Console.ReadKey();
            // GraphBehaviorQuery();
            // FromSqlQuery();
            // ShadowStateQuery();
            // RelationalQuery();
            // BatchingQuery(); 
            // SmartQuery();

            Console.ReadKey();
        }

        void GraphBehaviorQuery()
        {
            using (var db = new CoffeeContext())
            {
                // GitHub Issue: 
                // determines what is brought into the Graph dependencies
                var coffee1 = new Coffee {
                    Name = "Graph Coffee 1",
                    Price = 3.33M,
                    Image = new Image { Description = "Test 1", ImageUrl = "/images/test1.png" }
                };

                db.Coffee.Add(coffee1, GraphBehavior.IncludeDependents);


                var coffee2 = new Coffee
                {
                    Name = "Graph Coffee 2",
                    Price = 3.33M,
                    Image = new Image { Description = "Test 2", ImageUrl = "/images/test2.png" }
                };

                db.Coffee.Add(coffee2, GraphBehavior.SingleObject);

                db.SaveChanges();
            }
        }

        void FromSqlQuery()
        {
            // FromSql
            using (var db = new CoffeeContext())
            {
                var searchTerm = "Caffe%";
                var results = db.Coffee.FromSql("select * from dbo.Coffee where [Name] Like {0}", searchTerm)
                    .OrderByDescending(e => e.Price);

                Console.WriteLine("{0} records found using FromSql", results.Count());
            }
        }

        void ShadowStateQuery()
        {
            // Querying Shadow State
            using (var db = new CoffeeContext())
            {
                db.Coffee.Add(new Coffee { Name = "Last Added Coffee" });
                db.SaveChanges();

                var results = db.Coffee
                    .OrderByDescending(c => EF.Property<DateTime>(c, "LastModified"))
                    .Select(c => new { Name = c.Name, LastModified = EF.Property<DateTime>(c, "LastModified") });

                foreach(var coffee in results)
                {
                    Console.WriteLine("{0} - Last modified: {1}", coffee.Name, coffee.LastModified);
                }
            }
        }

        void RelationalQuery()
        {
            // Relational - ThenInclude
            using (var db = new CoffeeContext())
            {
                var store = db.Store.FirstOrDefault();

                //var store = db.Store
                //                .Include(s => s.TradingDays)
                //                .FirstOrDefault();

                //var store = db.Store
                //    .Include(s => s.TradingDays)
                //        .ThenInclude(td => td.CustomerOrders)
                //    .FirstOrDefault();

                Console.WriteLine("This first trading day for {0} is {1}", store.Name, store.TradingDays[0].Day);
            }
        }

        void BatchingQuery()
        {
            // Batching
            using (var db = new CoffeeContext())
            {
                var coffees = db.Coffee.ToArray();

                db.Coffee.Remove(coffees[0]);
                db.Coffee.Remove(coffees[1]);

                coffees[2].Name = "Modified in Batch";
                coffees[3].Name = "Modified again in Batch";

                db.Coffee.Add(new Coffee { Name = "Batch 1", Price = 6M });
                db.Coffee.Add(new Coffee { Name = "Batch 2", Price = 6.5M });
                db.Coffee.Add(new Coffee { Name = "Batch 3", Price = 7M });
                db.Coffee.Add(new Coffee { Name = "Batch 4", Price = 7.5M });

                db.SaveChanges();
            }
        }

        void SmartQuery()
        {
            // Smart Queries - e.g. Multiple Includes == multiple queries
            using (var db = new CoffeeContext())
            {
                var custOrder = db.CustomerOrder
                    .Include(c => c.Coffees)
                        .ThenInclude(c => c.Coffee)
                            .ThenInclude(c => c.Image)
                    .FirstOrDefault(co => co.Id == 1);
            }
        }

        private void CreateDatabase()
        {
            using (var db = new CoffeeContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        private void SeedData()
        {
            using (var db = new CoffeeContext())
            {
                if (!db.Coffee.Any(f => f.Name == "Caffe Latte"))
                {
                    Console.WriteLine("Seeding...");
                    var store = new Store { Name = "Ignition Cafe" };
                    db.Store.Add(store);

                    var sunday = new TradingDay { Day = new DateTime(2015, 11, 15), Store = store };
                    var monday = new TradingDay { Day = new DateTime(2015, 11, 15), Store = store };
                    var tuesday = new TradingDay { Day = new DateTime(2015, 11, 15), Store = store };
                    var wednesday = new TradingDay { Day = new DateTime(2015, 11, 15), Store = store };
                    var thursday = new TradingDay { Day = new DateTime(2015, 11, 15), Store = store };
                    var friday = new TradingDay { Day = new DateTime(2015, 11, 15), Store = store };
                    var saturday = new TradingDay { Day = new DateTime(2015, 11, 15), Store = store };
                    db.TradingDay.AddRange(sunday, monday, tuesday, wednesday, thursday, friday, saturday);


                    var latte = new Coffee { Name = "Caffe Latte", Price = 4.35M };
                    var brewed = new Coffee { Name = "Brewed Coffee", Price = 4.60M };
                    var cappuccino = new Coffee { Name = "Cappuccino", Price = 4M };
                    var espresso = new Coffee { Name = "Espresso", Price = 4.50M };
                    var flatwhite = new Coffee { Name = "Flat White", Price = 4M };
                    db.Coffee.AddRange(latte, brewed, cappuccino, espresso, flatwhite);

                    var order = new CustomerOrder { TradingDay = tuesday };
                    db.CustomerOrder.Add(order);


                    var line1 = new CoffeeCustomerOrder { CustomerOrder = order, Coffee = latte };
                    var line2 = new CoffeeCustomerOrder { CustomerOrder = order, Coffee = latte };
                    var line3 = new CoffeeCustomerOrder { CustomerOrder = order, Coffee = flatwhite };

                    db.CoffeeCustomerOrder.AddRange(line1, line2, line3);

                    db.SaveChanges();
                }
            }
        }
    }
}
