using Microsoft.EntityFrameworkCore;
using quick_ship_api.Models.User;
using quick_ship_api.Presistence.Context;
using quick_ship_api.Service.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quick_ship_api.Service.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRole(ApplicationRole applicationRole)
        {
            await _context.ApplicationRoles.AddAsync(applicationRole);
        }

        public async Task<ApplicationRole> FindByIdAsync(string id)
        {
            return await _context.ApplicationRoles.FindAsync(id);
        }

        public async Task<IEnumerable<ApplicationRole>> ListOfRoles()
        {
            return await _context.ApplicationRoles.ToListAsync();
        }



        public async Task<IEnumerable<ApplicationUser>> ListOfUserInRole(string id)
        {
            return await _context.ApplicationUsers.Where(u => u.ApplicationRole.Id == id).ToListAsync();
        }

        public void RemoveRole(ApplicationRole applicationRole)
        {
            _context.ApplicationRoles.Remove(applicationRole);
        }

        public void UpdateRole(ApplicationRole applicationRole)
        {
            _context.ApplicationRoles.Update(applicationRole);
        }
    }
}
