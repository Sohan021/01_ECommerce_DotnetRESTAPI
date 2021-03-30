using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quick_ship_api.Models.Regular;
using quick_ship_api.Presistence.Context;
using System.Linq;
using System.Threading.Tasks;

namespace quick_ship_api.Controllers.Regular
{
    [Route("/api/[controller]")]
    public class SubCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public SubCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var subCategories = await _context.SubCategories.ToListAsync();

            return Ok(subCategories);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var subCategory = await _context.SubCategories.Where(_ => _.Id == id).FirstOrDefaultAsync(_ => _.Id == id);
            return Ok(subCategory);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]SubCategory subCategory)
        {

            var scat = new SubCategory
            {
                Name = subCategory.Name,
                Description = subCategory.Description
            };
            await _context.AddAsync(scat);
            await _context.SaveChangesAsync();

            return Ok(scat);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]SubCategory subCategory)
        {

            var subCat = _context.SubCategories.Where(_ => _.Id == id).FirstOrDefault();

            subCat.Name = subCategory.Name;
            subCat.Description = subCategory.Description;

            _context.Update(subCat);
            await _context.SaveChangesAsync();

            return Ok(subCat);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsycn(int id)
        {

            var subCat = _context.SubCategories.Where(_ => _.Id == id).FirstOrDefault();

            _context.Remove(subCat);
            await _context.SaveChangesAsync();

            return Ok(subCat);
        }

    }
}
