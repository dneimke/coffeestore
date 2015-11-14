using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF7Demo.CoffeeStore.Model
{
    public class Coffee
    {
        public int Coffee_Id { get; set; }
        public string Name { get; set; }
        public decimal Retail { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
