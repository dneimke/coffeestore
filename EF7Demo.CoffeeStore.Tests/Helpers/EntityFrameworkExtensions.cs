using EF7Demo.CoffeeStore.Model;
using System.Linq;


namespace EF7Demo.CoffeeStore.Tests.Helpers
{
    public static class EntityFrameworkExtensions
    {
        public static void EnsureTestData(this ICoffeeStoreContext dbContext)
        {
            if (!dbContext.Coffee.Any(f => f.Name == "Caffe Latte"))
            {
                dbContext.Coffee.AddRange(
                    new Coffee { Coffee_Id = 1, Name = "Caffe Latte", Retail = 4.35M },
                    new Coffee { Coffee_Id = 2, Name = "Brewed Coffee", Retail = 4.60M },
                    new Coffee { Coffee_Id = 3, Name = "Cappuccino", Retail = 4M },
                    new Coffee { Coffee_Id = 4, Name = "Espresso", Retail = 4.50M },
                    new Coffee { Coffee_Id = 5, Name = "Flat White", Retail = 4M }
                    );
                dbContext.Commit();
            }
        }
    }
}
