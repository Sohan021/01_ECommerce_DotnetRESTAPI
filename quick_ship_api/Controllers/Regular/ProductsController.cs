using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quick_ship_api.Models.Regular;
using quick_ship_api.Presistence.Context;
using quick_ship_api.Service.IService;
using quick_ship_api.Service.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace quick_ship_api.Controllers.Regular
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private IHostingEnvironment _env;
        private AppDbContext _context;

        public ProductsController(AppDbContext context,
                                  IProductService productService,

                                  IHostingEnvironment env

                                  )
        {

            _productService = productService;
            _env = env;
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _productService.ListAsync();

            return products;
            //var products = await _context.Products

            //    .Include(_ => _.Category)
            //    .Include(_ => _.SubCategory)
            //    .ToListAsync();

            //return products;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var product = await _productService.FindByIdAsync(id);
            return Ok(product);

        }

        [HttpGet("{categoryId}/{subCategoryId}")]
        public async Task<IActionResult> GetProductsByCategoryAndSubCat([FromRoute]int categoryId, [FromRoute]int subCategoryId)
        {
            var products = await _context.Products
                .Where(_ => _.CategoryId == categoryId && _.SubCategoryId == subCategoryId)
                .Include(_ => _.Category)
                .Include(_ => _.SubCategory)
                .ToListAsync();

            return Ok(products);

        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory([FromRoute]int categoryId)
        {
            var products = await _context.Products
                .Where(_ => _.CategoryId == categoryId)
                .Include(_ => _.Category)
                .Include(_ => _.SubCategory)
                .ToListAsync();

            return Ok(products);

        }





        [HttpPost, DisableRequestSizeLimit]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> SavePhoto()
        {
            var files = Request.Form.Files as List<IFormFile>;
            string imageUrl = ImageUrl(files[0]);
            return Ok(await Task.FromResult(imageUrl));
        }


        [HttpPost]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync(ProductResource product)
        {
            var cat = _context.Categories.Where(_ => _.Id == product.CategoryId).FirstOrDefault();
            var subCat = _context.SubCategories.Where(_ => _.Id == product.SubCategoryId).FirstOrDefault();

            var webRoot = _env.WebRootPath;
            var PathWithFolderName = Path.Combine(webRoot, "Image");

            var productt = new Product
            {
                Name = product.Name,
                Price = product.Price,
                CountInStock = product.CountInStock,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                Category = cat,
                SubCategory = subCat,

            };
            //_context.Products.Add(productt);
            //_context.SaveChanges();
            var result = await _productService.SaveAsync(productt);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            return Ok(productt);




        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, ProductResource resource)
        {

            //var webRoot = _env.WebRootPath;
            //var PathWithFolderName = System.IO.Path.Combine(webRoot, "Image");
            //var item = resource.File;

            //var imageUrl = ImageUrl(item);

            var cat = _context.Categories.Where(_ => _.Id == resource.CategoryId).FirstOrDefault();
            var subCat = _context.SubCategories.Where(_ => _.Id == resource.SubCategoryId).FirstOrDefault();

            var webRoot = _env.WebRootPath;
            var PathWithFolderName = Path.Combine(webRoot, "Image");

            var product = new Product
            {
                Name = resource.Name,
                Price = resource.Price,
                CountInStock = resource.CountInStock,
                UpdatedAt = DateTime.Now,
                ImageUrl = resource.ImageUrl,
                Description = resource.Description,
                CategoryId = resource.CategoryId,
                SubCategoryId = resource.SubCategoryId

            };


            var result = await _productService.UpdateAsync(id, product);


            if (!result.Success)
                return BadRequest(result.Message);



            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _productService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }


        public string ImageUrl(IFormFile file)
        {


            if (file == null || file.Length == 0) return null;
            string extension = Path.GetExtension(file.FileName);

            string path_Root = _env.WebRootPath;

            string path_to_Images = path_Root + "\\Image\\" + file.FileName;

            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {

                file.CopyTo(stream);
                string revUrl = Reverse.reverse(path_to_Images);
                int count = 0;
                int flag = 0;

                for (int i = 0; i < revUrl.Length; i++)
                {
                    if (revUrl[i] == '\\')
                    {
                        count++;

                    }
                    if (count == 2)
                    {
                        flag = i;
                        break;
                    }
                }

                string sub = revUrl.Substring(0, flag + 1);
                string finalString = Reverse.reverse(sub);

                string f = finalString.Replace("\\", "/");
                return f;

            }


        }
    }

    public static class Reverse
    {
        public static string reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

    }
}
