using EF7Demo.CoffeeStore.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF7Demo.CoffeeStore.Tests
{
    public class TestBase
    {
        protected IServiceProvider _provider = null;

        public TestBase()
        {
            var fixture = new InMemoryFixture();
            _provider = fixture.GetServiceProvider();
        }
    }
}
