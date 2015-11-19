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

    public class PriceServiceFixture : TestBase
    {
        [Fact]
        public void ShouldMarkUpAllPrices()
        {
            using (var db = _provider.GetService<ICoffeeStoreContext>())
            {
                // Arrange
                db.EnsureTestData();
                var expectedCount = db.Coffee.Count();
                var calculator = _provider.GetService<IPriceService>();

                // Act
                var results = calculator.UpdatePrices(1.1M).ToList();

                // Assert
                Assert.Equal(expectedCount, results.Count());
                Assert.Equal(4.8M, db.Coffee.FirstOrDefault(c => c.Name == "Caffe Latte").Retail);
                Assert.Equal(4.4M, db.Coffee.FirstOrDefault(c => c.Name == "Cappuccino").Retail);
            }
        }


        [Theory,
            InlineData(1),
            InlineData(2),
            InlineData(3)]
        public void ShouldRoundUpPrice(decimal value)
        {
            // Arrange
            var calculator = _provider.GetService<IPriceService>();
            var multiplier = 1.1M;
            var expected = (value * multiplier * 20)/20;

            // Act
            var result = calculator.RoundedPrice(value, multiplier);

            // Assert
            Assert.Equal(expected, result);

        }
    }
}
