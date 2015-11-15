using EF7Demo.CoffeeStore.Model;
using EF7Demo.CoffeeStore.Tests.Helpers;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EF7Demo.CoffeeStore.Tests
{

    public class LogicTests : TestBase
    {
        [Fact]
        public void SimpleConversion()
        {
            using (var db = _provider.GetService<ICoffeeStoreContext>())
            {
                db.EnsureTestData();
                var expectedCount = db.Coffee.Count();


                var calculator = _provider.GetService<IPriceService>();
                var results = calculator.UpdatePrices(1.1M).ToList();

                Assert.Equal(expectedCount, results.Count());
                Assert.Equal(4.8M, db.Coffee.FirstOrDefault(c => c.Name == "Caffe Latte").Retail);
                Assert.Equal(4.4M, db.Coffee.FirstOrDefault(c => c.Name == "Cappuccino").Retail);
            }
        }

    }
}
