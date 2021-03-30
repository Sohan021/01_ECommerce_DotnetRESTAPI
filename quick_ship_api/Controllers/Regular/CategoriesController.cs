using Microsoft.AspNetCore.Mvc;
using quick_ship_api.Models.Regular;
using quick_ship_api.Service.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Controllers.Regular
{
    [Route("/api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllAsync()
        {

            var categories = await _categoryRepository.ListAsync();

            return categories;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var category = await _categoryRepository.FindByIdAsync(id);
            return Ok(category);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Category category)
        {

            await _categoryRepository.AddAsync(category);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Category resource)
        {

            var category = await _categoryRepository.FindByIdAsync(id);

            category.Name = resource.Name;
            category.Description = resource.Description;


            _categoryRepository.Update(category);



            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _categoryRepository.FindByIdAsync(id);

            _categoryRepository.Remove(category);

            return Ok();
        }
    }
}
