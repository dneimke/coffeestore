﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF7Demo.CoffeeStore.Model
{
    public class PriceService : IPriceService
    {
        private ICoffeeStoreContext _context;

        public PriceService(ICoffeeStoreContext context)
        {
            _context = context;
        }


        public IEnumerable<Coffee> UpdatePrices(decimal multiplier)
        {
            var coffees = _context.Coffee.ToList();

            foreach (var item in coffees)
            {
                item.Retail = RoundedPrice(item.Retail, multiplier);
            }

            _context.Commit();

            return coffees;
        }


        public decimal RoundedPrice(decimal price, decimal multiplier)
        {
            var newPrice = price * multiplier;
            var rounded = Math.Round(newPrice * 20, 0) / 20;

            if (rounded % 1 == 0)
            {
                rounded -= 0.05M;
            }

            return rounded;
        }
    }



    public interface IPriceService
    {
        IEnumerable<Coffee> UpdatePrices(decimal multiplier);
        decimal RoundedPrice(decimal price, decimal multiplier);
    }
}
