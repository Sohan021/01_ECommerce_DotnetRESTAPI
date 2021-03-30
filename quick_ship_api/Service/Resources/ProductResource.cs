using Microsoft.AspNetCore.Http;
using quick_ship_api.Models.Regular;

namespace quick_ship_api.Service.Resources
{
    public class ProductResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public int CountInStock { get; set; }

        public IFormFile File { get; set; }

        public string ImageUrl { get; set; }

        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        public int? SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }

    }
}
