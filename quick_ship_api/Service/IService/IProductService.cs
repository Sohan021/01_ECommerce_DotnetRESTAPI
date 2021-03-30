using quick_ship_api.Models.Regular;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> ListAsync();
        Task<Product> FindByIdAsync(int id);
        Task<SaveProductResponse> SaveAsync(Product product);
        Task<SaveProductResponse> UpdateAsync(int id, Product product);
        Task<SaveProductResponse> DeleteAsync(int id);
    }
}
