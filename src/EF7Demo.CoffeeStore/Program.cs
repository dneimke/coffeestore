using EF7Demo.CoffeeStore.Extensions;
using EF7Demo.CoffeeStore.Model;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EF7Demo.CoffeeStore
{
    public class Program
    {
        IServiceProvider serviceProvider = null;

        public Program()
        {
            var startup = new Startup();
            
            serviceProvider = startup.ServiceProvider;
        }


        public void Main(string[] args)
        {
            // Make sure you have created a database called CoffeeStore

            serviceProvider.EnsureMigrationsApplied();  // NOTE: this is a custom extension method
            serviceProvider.EnsureDevelopmentData();    // NOTE: this is a custom extension method

            SearchOperation(); // Take a look inside of this method for further info
            Console.Read();
        }


        void BulkOperation()
        {
            var db = serviceProvider.GetService<ICoffeeStoreContext>();
            var coffees = db.Coffee.ToArray();

            db.Coffee.Remove(coffees[0]);
            db.Coffee.Remove(coffees[1]);

            coffees[2].Name = "Modified Coffee";
            coffees[3].Name = "Modified Other Coffee";

            db.Coffee.Add(new Coffee { Name = "Coffee 4", Retail = 5.55M });
            db.Coffee.Add(new Coffee { Name = "Coffee 2", Retail = 3.30M });
            db.Coffee.Add(new Coffee { Name = "Coffee 3", Retail = 1.55M });
            db.Coffee.Add(new Coffee { Name = "Coffee 1", Retail = 4.25M });
            db.Coffee.Add(new Coffee { Name = "Coffee 5", Retail = 3M });

            db.Commit();
        }


        void SearchOperation()
        {
            var searchTerm = "C%";
            var db = serviceProvider.GetService<ICoffeeStoreContext>();

            var coffees = ((CoffeeStoreContext)db).Coffee
                                .FromSql("SELECT * FROM [dbo].[Coffee] WHERE [Name] LIKE {0}", searchTerm)
                                .OrderByDescending(e => e.Retail)
                                .ToList();

            // Uncomment and run the app with the following code instead of the above FromSql
            // - To get this running, you will need to add the MigrationExtensions Up and Down
            // to the generated Initial migration in the Migrations folder.  To start with, delete and recreate
            // the target database as Migrations will have already been created.

            // After adding the Migration Extensions, run the following command to see how the 
            // custom Stored Procedure gets added to as part of the process:
            //   > dnx ef migrations script > deployment.sql

            //var coffees = db.FindCoffee(searchTerm)
            //    .OrderByDescending(e => e.Retail);

            Console.WriteLine($"Found {coffees.Count()} result(s) for search term '{searchTerm}'");
            foreach (var coffee in coffees)
            {
                Console.WriteLine("- {0} - {1}", coffee.Name, coffee.Retail);
            }
        }
    }

}
