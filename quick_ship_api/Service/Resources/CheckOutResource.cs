using quick_ship_api.Models.Regular;
using System.Collections.Generic;

namespace quick_ship_api.Service.Resources
{
    public class CheckOutResource
    {
        public CheckOutResource()
        {
            Products = new List<Product>();
        }

        public string CurrentUserId { get; set; }

        public double Amount { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
