using GroceryAPI.Application.DTOs.Configuration;

namespace GroceryAPI.Application.Abstractions.Services.Configurations
{
    public interface IApplicationService
    {
        List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
