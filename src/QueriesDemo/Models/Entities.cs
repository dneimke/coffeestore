using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary1.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TradingDay> TradingDays { get; set; }
    }


    public class TradingDay
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }

        public int StoreId { get; set; }

        public Store Store { get; set; }

        public List<CustomerOrder> CustomerOrders { get; set; }
    }

    public class CustomerOrder
    {
        public int Id { get; set; }

        public int TradingDayId { get; set; }

        public TradingDay TradingDay { get; set; }

        public List<CoffeeCustomerOrder> Coffees { get; set; }
    }

    public class CoffeeCustomerOrder
    {
        public int Id { get; set; }
        public int CoffeeId { get; set; }
        public int CustomerOrderId { get; set; }

        public Coffee Coffee { get; set; }
        public CustomerOrder CustomerOrder { get; set; }
    }


    public class Coffee
    {
        public int Coffee_Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public int? ImageId { get; set; }
        public Image Image { get; set; }
    }


    public class Image
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int Coffee_Id { get; set; }
        public Coffee Coffee { get; set; }
    }
}
