using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.AppUser.RemoveUser
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommandRequest, RemoveUserCommandResponse>
    {
        readonly IUserService _userService;

        public RemoveUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<RemoveUserCommandResponse> Handle(RemoveUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _userService.RemoveUserAsync(request.Id);
            return new();
        }
    }
}
