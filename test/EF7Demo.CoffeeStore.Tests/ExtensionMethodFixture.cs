using EF7Demo.CoffeeStore.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using EF7Demo.CoffeeStore.Model;

namespace EF7Demo.CoffeeStore.Tests
{
    public class ExtensionMethodFixture : TestBase
    {
        [Fact]
        public void TestExtensionMethod()
        {
            var fixture = new InMemoryFixture();

            using (var db = _provider.GetService<ICoffeeStoreContext>())
            {
                db.EnsureTestData();
                
                var result = db.Coffee.Count();

                Assert.Equal(5, result);
            }
        }
    }
}
