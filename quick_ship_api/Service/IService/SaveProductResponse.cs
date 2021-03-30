using quick_ship_api.Models.Regular;

namespace quick_ship_api.Service.IService
{
    public class SaveProductResponse : BaseResponse<Product>
    {
        public SaveProductResponse(Product product) : base(product) { }

        public SaveProductResponse(string message) : base(message) { }
    }
}
