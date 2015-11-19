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
            
            // Uncomment these and watch in SQL Profiler to see how SQL statements 
            // are run against the target database

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
                var coffee1 = new Coffee {
                    Name = "Graph Coffee 1",
                    Price = 3.33M,
                    Image = new Image { Description = "Test 1", ImageUrl = "/images/test1.png" }
                };

                // IncludeDependents is the default behavior
                db.Coffee.Add(coffee1, GraphBehavior.IncludeDependents);
                db.SaveChanges();


                // This won't create a record in the Db for the child Image object
                // due to GraphBehavior
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
            // FromSql is useful for callling out to Views, Procs, or for running ad-hoc SQL
            // FromSql takes a string and a params list of parameters.  
            // Use the params array as opposed to using string format as it will create
            // genuine Sql parameters which are safer and perform better
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
            // Querying Shadow State - take note of how this is configured and used in DbContext class
            // ShadowState could be useful for implementing a generic app-wide auditing feature
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
            // Relational - ThenInclude is now used to allow drill down through
            // child objects that hang off of collections.
            // Previously this was done with messy sub-SELECT statements to force
            // Eager loading
            using (var db = new CoffeeContext())
            {
                // var store = db.Store.FirstOrDefault();

                //var store = db.Store
                //                .Include(s => s.TradingDays)
                //                .FirstOrDefault();

                var store = db.Store
                    .Include(s => s.TradingDays)
                        .ThenInclude(td => td.CustomerOrders)
                    .FirstOrDefault();

                Console.WriteLine("This first trading day for {0} is {1}", store.Name, store.TradingDays[0].Day);
            }
        }

        void BatchingQuery()
        {
            // Batching - run this query and watch in Sql Profiler to 
            // see that it is all sent through as 1 batched statement
            // You can set MaxBatchSize() in OnConfiguring override of DbContext
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
            // Smart Queries - EF7 can look at certain queries and break them down into 
            // multiple executable SQL statements rather than having to produce 1 single 
            // SQL statement
            using (var db = new CoffeeContext())
            {
                var day = db.TradingDay
                        .Include(td => td.CustomerOrders)
                     .Where(td => td.CustomerOrders.Count > 0)
                     .FirstOrDefault();

                Console.WriteLine("Selected day: {0}, orders: {1}", day.Day.DayOfWeek, day.CustomerOrders.Count());

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
