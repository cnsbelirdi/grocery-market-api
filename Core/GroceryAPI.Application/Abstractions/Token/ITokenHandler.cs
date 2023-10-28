using GroceryAPI.Domain.Entities.Identity;

namespace GroceryAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int second, AppUser user, string role);
        string CreateRefreshToken();
    }
}
