using EF7Demo.CoffeeStore.Model;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Migrations;
using System.Linq;
using Microsoft.Data.Entity;

namespace EF7Demo.CoffeeStore.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static void EnsureMigrationsApplied(this IServiceProvider provider)
        {
            var db = provider.GetService<ICoffeeStoreContext>();
            if (db.AllMigrationsApplied() == false)
            {
                ((DbContext)db).Database.Migrate();
            }
        }


        public static void EnsureDevelopmentData(this IServiceProvider provider)
        {
            var db = provider.GetService<ICoffeeStoreContext>();
            if (db.AllMigrationsApplied())
            {
                if (!db.Coffee.Any(f => f.Name == "Caffe Latte"))
                {
                    db.Coffee.AddRange(
                    new Coffee { Name = "Caffe Latte", Retail = 4.35M },
                    new Coffee { Name = "Brewed Coffee", Retail = 4.60M },
                    new Coffee { Name = "Cappuccino", Retail = 4M },
                    new Coffee { Name = "Espresso", Retail = 4.50M },
                    new Coffee { Name = "Flat White", Retail = 4M }
                    );
                    db.Commit();
                }
            }
        }


        public static bool AllMigrationsApplied(this ICoffeeStoreContext context)
        {
            var historyRepo = ((IInfrastructure<IServiceProvider>)context).GetService<IHistoryRepository>();
            var migrationsAssembly = ((IInfrastructure<IServiceProvider>)context).GetService<IMigrationsAssembly>();

            var applied = historyRepo.GetAppliedMigrations().Select(m => m.MigrationId);
            var total = migrationsAssembly.Migrations.Select(m => m.Key);
            return !total.Except(applied).Any();
        }
    }
}
