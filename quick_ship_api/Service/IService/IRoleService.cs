using quick_ship_api.Models.User;
using quick_ship_api.Service.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Service.IService
{
    public interface IRoleService
    {
        Task<IEnumerable<ApplicationRole>> ListAsync();
        Task<ApplicationRole> FindByIdAsync(string id);
        Task<SaveRoleResponse> SaveAsync(ApplicationRole applicationRole);
        Task<SaveRoleResponse> UpdateAsync(string id, RoleResource roleResource);
        Task<SaveRoleResponse> DeleteAsync(string id);
    }
}
