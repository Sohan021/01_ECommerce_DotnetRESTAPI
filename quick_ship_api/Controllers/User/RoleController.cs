using Microsoft.AspNetCore.Mvc;
using quick_ship_api.Models.User;
using quick_ship_api.Service.IService;
using quick_ship_api.Service.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Controllers.User
{

    [Route("/api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync()
        {
            var roles = await _roleService.ListAsync();
            return roles;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(string id)
        {
            var role = await _roleService.FindByIdAsync(id);

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ApplicationRole applicationRole)
        {

            var result = await _roleService.SaveAsync(applicationRole);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] RoleResource role)
        {


            var result = await _roleService.UpdateAsync(id, role);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(role);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _roleService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
