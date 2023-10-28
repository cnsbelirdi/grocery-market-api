using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Exceptions;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.AppUser.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        readonly IUserService _userService;

        public UpdatePasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.Password.Equals(request.PasswordConfirm))
            {
                await _userService.UpdatePasswordAsync(request.UserId, request.ResetToken, request.Password);
                return new();
            }
            throw new PasswordChangeFailedException();
        }
    }
}
