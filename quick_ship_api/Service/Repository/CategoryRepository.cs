using Microsoft.EntityFrameworkCore;
using quick_ship_api.Models.Regular;
using quick_ship_api.Presistence.Context;
using quick_ship_api.Service.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quick_ship_api.Service.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            var category = await _context.Categories.Where(_ => _.Id == id).FirstOrDefaultAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
}
