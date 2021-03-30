using System;
using System.Collections.Generic;

namespace quick_ship_api.Models.Regular
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public int CountInStock { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int? SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}
