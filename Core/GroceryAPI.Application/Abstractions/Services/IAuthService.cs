using GroceryAPI.Application.Abstractions.Services.Authentication;

namespace GroceryAPI.Application.Abstractions.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
        Task PasswordResetAsync(string email);
        Task<bool> VeriftResetTokenAsync(string resetToken, string userId);
    }
}
