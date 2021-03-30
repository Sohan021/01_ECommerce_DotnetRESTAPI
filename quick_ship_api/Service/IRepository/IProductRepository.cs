using quick_ship_api.Models.Regular;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Service.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> ListAsync();
        Task<Product> FindByIdAsync(int id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Remove(Product product);
    }
}
