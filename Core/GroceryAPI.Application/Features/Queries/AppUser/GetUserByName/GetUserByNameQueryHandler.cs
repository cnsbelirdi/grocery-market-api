using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.AppUser.GetUserByName
{
    internal class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQueryRequest, GetUserByNameQueryResponse>
    {
        readonly IUserService _userService;

        public GetUserByNameQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUserByNameQueryResponse> Handle(GetUserByNameQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByNameAsync(request.Name);
            return new()
            {
                User = user
            };
        }
    }
}
