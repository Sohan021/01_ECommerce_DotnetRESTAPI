using quick_ship_api.Models.Regular;
using quick_ship_api.Presistence.Context;
using quick_ship_api.Service.IRepository;
using quick_ship_api.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quick_ship_api.Service.Service
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;


        public ProductService(IProductRepository productRepository, AppDbContext context)
        {
            _productRepository = productRepository;
            _context = context;

        }

        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await _productRepository.ListAsync();
        }


        public async Task<SaveProductResponse> SaveAsync(Product product)
        {
            try
            {
                await _productRepository.AddAsync(product);
                await _context.SaveChangesAsync();

                return new SaveProductResponse(product);
            }
            catch (Exception ex)
            {
                return new SaveProductResponse($"An error occurred when saving the Product: {ex.Message}");
            }
        }

        public async Task<SaveProductResponse> UpdateAsync(int id, Product product)
        {
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct == null)
                return new SaveProductResponse("Product not found.");
            var cat = _context.Categories.Where(_ => _.Id == product.CategoryId).FirstOrDefault();
            var subCat = _context.SubCategories.Where(_ => _.Id == product.SubCategoryId).FirstOrDefault();


            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Category = cat;
            existingProduct.SubCategory = subCat;

            try
            {
                _productRepository.Update(existingProduct);
                await _context.SaveChangesAsync();


                return new SaveProductResponse(existingProduct);
            }
            catch (Exception ex)
            {
                return new SaveProductResponse($"An error occurred when updating the Product: {ex.Message}");
            }
        }

        public async Task<SaveProductResponse> DeleteAsync(int id)
        {
            var existingProduct = await _productRepository.FindByIdAsync(id);


            if (existingProduct == null)
                return new SaveProductResponse("Product not found.");


            try
            {
                _productRepository.Remove(existingProduct);
                await _context.SaveChangesAsync();



                return new SaveProductResponse(existingProduct);
            }
            catch (Exception ex)
            {

                return new SaveProductResponse($"An error occurred when deleting the Product: {ex.Message}");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _productRepository.FindByIdAsync(id);
        }
    }
}
