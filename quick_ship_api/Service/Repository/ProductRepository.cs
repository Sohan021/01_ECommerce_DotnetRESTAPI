
using Microsoft.EntityFrameworkCore;
using quick_ship_api.Models.Regular;
using quick_ship_api.Presistence.Context;
using quick_ship_api.Service.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Service.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await _context.Products
                        .Include(_ => _.Category)
                        .Include(_ => _.SubCategory)
                        .ToListAsync();

        }
        public async Task<Product> FindByIdAsync(int id)
        {
            return await _context.Products
                        .Include(_ => _.Category)
                        .Include(_ => _.SubCategory)
                        .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            //await _context.SaveChangesAsync();

        }
        public void Update(Product product)
        {
            _context.Products.Update(product);
            //_context.SaveChanges();
        }
        public void Remove(Product product)
        {
            _context.Products.Remove(product);
            //_context.SaveChanges();
        }

    }
}
