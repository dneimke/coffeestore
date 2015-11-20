using EF7Demo.CoffeeStore.Extensions;
using EF7Demo.CoffeeStore.Model;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF7Demo.CoffeeStore
{
    public class Startup
    {

        public IConfiguration Configuration = null;
        public IServiceProvider ServiceProvider = null;

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json");
            Configuration = builder.Build();

            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            Configure(ServiceProvider);
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<CoffeeStoreContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });


            services.AddScoped<ICoffeeStoreContext>(provider => provider.GetService<CoffeeStoreContext>());
        }


        public void Configure(IServiceProvider provider)
        {
            provider.EnsureMigrationsApplied();
            provider.EnsureDevelopmentData();
        }
    }
}
