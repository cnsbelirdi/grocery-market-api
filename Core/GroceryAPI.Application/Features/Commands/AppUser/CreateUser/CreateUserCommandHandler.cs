using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.DTOs.User;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                FullName = request.FullName,
                Username = request.Username,
                PhoneNumber = request.PhoneNumber,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
            });


            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded
            };
        }
    }
}
