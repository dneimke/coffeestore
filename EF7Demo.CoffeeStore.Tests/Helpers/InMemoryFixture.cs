using System;
using EF7Demo.CoffeeStore.Model;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EF7Demo.CoffeeStore.Tests.Helpers
{
    public class InMemoryFixture
    {
        private readonly IServiceProvider _serviceProvider;

        public InMemoryFixture()
        {
            var services = new ServiceCollection();

            services
                .AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<CoffeeStoreContext>(options =>
                    options.UseInMemoryDatabase()
                );

            services.AddScoped<ICoffeeStoreContext>(provider => provider.GetService<CoffeeStoreContext>());
            services.AddScoped<IPriceService, PriceService>();

            // services.AddInstance<>  // Wire-up logging

            _serviceProvider = services.BuildServiceProvider();

        }

        public IServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }
    }
}
