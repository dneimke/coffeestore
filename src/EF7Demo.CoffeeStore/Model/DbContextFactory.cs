namespace EF7Demo.CoffeeStore.Model
{

    // This is required so that EF.Commands can correctly identify the database
    // This would not be required in an Web application
    //public class CoffeeStoreContextFactory : IDbContextFactory<CoffeeStoreContext>
    //{
    //    IServiceProvider _provider;


    //    public CoffeeStoreContextFactory()
    //    {
    //        var builder = new ConfigurationBuilder()
    //            .AddJsonFile("config.json");
    //        var _configuration = builder.Build();

    //        var services = new ServiceCollection();

    //        var connectionString = _configuration["Data:DefaultConnection:ConnectionString"];
    //        services.AddEntityFramework()
    //            .AddSqlServer()
    //            .AddDbContext<CoffeeStoreContext>(options =>
    //            {
    //                options.UseSqlServer(connectionString);
    //            });

    //        _provider = services.BuildServiceProvider();
    //    }


    //    public CoffeeStoreContext Create()
    //    {
    //        return _provider.GetService<CoffeeStoreContext>();
    //    }
    //}
}
