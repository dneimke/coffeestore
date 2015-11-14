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
            var searchTerm = "Caffe";
            var db = serviceProvider.GetService<ICoffeeStoreContext>();
            var coffees = db.FindCoffee(searchTerm);

            Console.WriteLine($"Found {coffees.Count()} result(s) for search term '{searchTerm}'");
            Console.Read();
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
