using quick_ship_api.Models.Regular;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Service.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> ListAsync();
        Task<Category> FindByIdAsync(int id);
        Task AddAsync(Category category);
        void Update(Category category);
        void Remove(Category category);
    }
}
