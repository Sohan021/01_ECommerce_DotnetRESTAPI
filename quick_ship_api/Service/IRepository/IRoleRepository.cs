using quick_ship_api.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Service.IRepository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<ApplicationRole>> ListOfRoles();
        Task<IEnumerable<ApplicationUser>> ListOfUserInRole(string id);

        Task<ApplicationRole> FindByIdAsync(string id);
        Task AddRole(ApplicationRole applicationRole);
        void UpdateRole(ApplicationRole applicationRole);
        void RemoveRole(ApplicationRole applicationRole);
    }
}
