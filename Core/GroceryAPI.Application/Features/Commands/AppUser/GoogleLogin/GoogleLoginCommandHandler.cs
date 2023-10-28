using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Abstractions.Services.Authentication;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly IExternalAuthentication _authService;

        public GoogleLoginCommandHandler(IExternalAuthentication authService)
        {
            _authService = authService;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.GoogleLoginAsync(request.IdToken, 2000);
            return new()
            {
                Token = token,
            };
        }
    }
}
