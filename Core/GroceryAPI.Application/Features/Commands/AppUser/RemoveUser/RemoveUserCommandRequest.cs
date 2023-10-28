using MediatR;

namespace GroceryAPI.Application.Features.Commands.AppUser.RemoveUser
{
    public class RemoveUserCommandRequest : IRequest<RemoveUserCommandResponse>
    {
        public string Id { get; set; }
    }
}
