using quick_ship_api.Models.User;
using quick_ship_api.Presistence.Context;
using quick_ship_api.Service.IRepository;
using quick_ship_api.Service.IService;
using quick_ship_api.Service.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace quick_ship_api.Service.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly AppDbContext _context;

        public RoleService(IRoleRepository roleRepository, AppDbContext context)
        {
            _roleRepository = roleRepository;
            _context = context;
        }

        public async Task<SaveRoleResponse> DeleteAsync(string id)
        {
            var existingRole = await _roleRepository.FindByIdAsync(id);

            if (existingRole == null)
                return new SaveRoleResponse("Role Not Found");

            try
            {
                _roleRepository.RemoveRole(existingRole);
                await _context.SaveChangesAsync();
                return new SaveRoleResponse(existingRole);
            }
            catch (Exception ex)
            {
                return new SaveRoleResponse($"An error occurred when deleting the role: { ex.Message }");
            }
        }

        public async Task<ApplicationRole> FindByIdAsync(string id)
        {
            return await _roleRepository.FindByIdAsync(id);
        }

        public async Task<IEnumerable<ApplicationRole>> ListAsync()
        {
            return await _roleRepository.ListOfRoles();
        }

        public async Task<SaveRoleResponse> SaveAsync(ApplicationRole applicationRole)
        {
            try
            {
                await _roleRepository.AddRole(applicationRole);
                await _context.SaveChangesAsync();

                return new SaveRoleResponse(applicationRole);
            }
            catch (Exception ex)
            {
                return new SaveRoleResponse($"An error occurd when saving the role: {ex.Message}");
            }
        }

        public async Task<SaveRoleResponse> UpdateAsync(string id, RoleResource roleResource)
        {
            var existingRole = await _roleRepository.FindByIdAsync(id);

            if (existingRole == null)
                return new SaveRoleResponse("Role Not Found");

            existingRole.Name = roleResource.Name;
            existingRole.RoleName = roleResource.RoleName;
            existingRole.Description = roleResource.Description;

            try
            {
                _roleRepository.UpdateRole(existingRole);
                await _context.SaveChangesAsync();

                return new SaveRoleResponse(existingRole);
            }
            catch (Exception ex)
            {
                return new SaveRoleResponse($"An error occurred when updating the role: {ex.Message}");
            }

        }
    }
}
