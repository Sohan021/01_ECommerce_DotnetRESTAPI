using quick_ship_api.Models.User;
using System;
using System.Collections.Generic;

namespace quick_ship_api.Models.Regular
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();

        }
        public int Id { get; set; }

        public int OrderNo { get; set; }

        public string UserName { get; set; }

        public DateTime OrderDate { get; set; }

        public string PhoneNo { get; set; }

        public double TotalAmount { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
