namespace GroceryAPI.Application.Features.Commands.AppUser.UpdateUser
{
    public class UpdateUserCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string? AccessToken { get; set; }
    }
}
