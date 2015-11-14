using EF7Demo.CoffeeStore.Extensions;
using EF7Demo.CoffeeStore.Model;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Linq;

namespace EF7Demo.CoffeeStore
{
    public class Program
    {
        public readonly IConfiguration Configuration;
        IServiceProvider serviceProvider = null;

        public Program(IApplicationEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json");
            Configuration = builder.Build();

            var services = new ServiceCollection();
            
            ConfigureServices(services);

            serviceProvider = services.BuildServiceProvider();

            Configure(serviceProvider);
        }

        public void Main(string[] args)
        {
            // BulkOperation();
            SearchOperation();
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

            //var coffees = db.FindCoffee(searchTerm)
            //    .OrderByDescending(e => e.Retail);



            Console.WriteLine($"Found {coffees.Count()} result(s) for search term '{searchTerm}'");
            foreach (var coffee in coffees)
            {
                Console.WriteLine("- {0} - {1}", coffee.Name, coffee.Retail);
            }
        }


        private void ConfigureServices(ServiceCollection services)
        {
            var connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<CoffeeStoreContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                }
                );


            services.AddScoped<ICoffeeStoreContext>(provider => provider.GetService<CoffeeStoreContext>());
        }


        public void Configure(IServiceProvider provider)
        {
            // per 'Pattern for seeding database with EF7 in ASP.NET 5': 
            // https://github.com/aspnet/EntityFramework/issues/3070
            provider.EnsureMigrationsApplied();
            provider.EnsureDevelopmentData();
        }
    }
}
