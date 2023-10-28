using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.DTOs.User;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.AppUser.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
    {
        readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse response = await _userService.UpdateAsync(new()
            {
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Username = request.Username
            });


            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
                AccessToken = response.AccessToken
            };
        }
    }
}
