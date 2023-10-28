using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GroceryAPI.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
            IdentityResult result = await _roleManager.CreateAsync(new() { Id = Guid.NewGuid().ToString() ,Name = name });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string id)
        {
            AppRole appRole = await _roleManager.FindByIdAsync(id);
            IdentityResult result = await _roleManager.DeleteAsync(appRole);
            return result.Succeeded;
        }
        public async Task<bool> UpdateRole(string id, string name)
        {
            AppRole appRole = await _roleManager.FindByIdAsync(id);
            appRole.Name = name;
            IdentityResult result = await _roleManager.UpdateAsync(appRole);
            return result.Succeeded;
        }

        public (object,int) GetAllRoles(int page, int size)
        {
            var query = _roleManager.Roles;

            IQueryable<AppRole> _query = null;

            if(page != -1 && size != -1)
                _query = query.Skip(page * size).Take(size);
            else
                _query = query;

            return (_query.Select(r => new { r.Id, r.Name }), query.Count());
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            AppRole appRole = await _roleManager.FindByIdAsync(id);
            return (appRole.Id, appRole.Name);
        }

    }
}
