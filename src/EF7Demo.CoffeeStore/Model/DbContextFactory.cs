using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EF7Demo.CoffeeStore.Model
{
    public class CoffeeStoreContextFactory : IDbContextFactory<CoffeeStoreContext>
    {
        IServiceProvider _provider;


        public CoffeeStoreContextFactory()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json");
            var _configuration = builder.Build();

            var services = new ServiceCollection();

            var connectionString = _configuration["Data:DefaultConnection:ConnectionString"];
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<CoffeeStoreContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

            _provider = services.BuildServiceProvider();
        }


        public CoffeeStoreContext Create()
        {
            return _provider.GetService<CoffeeStoreContext>();
        }
    }
}
