using GroceryAPI.Application.CustomAttributes;
using GroceryAPI.Application.Enums;
using GroceryAPI.Application.Features.Commands.AppUser.FacebookLogin;
using GroceryAPI.Application.Features.Commands.AppUser.GoogleLogin;
using GroceryAPI.Application.Features.Commands.AppUser.LoginUser;
using GroceryAPI.Application.Features.Commands.AppUser.PasswordReset;
using GroceryAPI.Application.Features.Commands.AppUser.RefreshTokenLogin;
using GroceryAPI.Application.Features.Commands.AppUser.VerifyResetToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GroceryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Login to the System", Menu = "Auth")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse response = await _mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("google-login")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Login to the System with Google", Menu = "Auth")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse response = await _mediator.Send(googleLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("facebook-login")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Login to the System with Facebook", Menu = "Auth")]
        public async Task<IActionResult> FacebookLogin(FacebookLoginCommandRequest facebookLoginCommandRequest)
        {
            FacebookLoginCommandResponse response = await _mediator.Send(facebookLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Refresh Token", Menu = "Auth")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("password-reset")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Password Reset", Menu = "Auth")]
        public async Task<IActionResult> PasswordReset([FromBody]PasswordResetCommandRequest passwordResetCommandRequest)
        {
            PasswordResetCommandResponse response = await _mediator.Send(passwordResetCommandRequest);
            return Ok(response);
        }

        [HttpPost("verify-reset-token")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Verify Reset Token", Menu = "Auth")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyResetTokenCommandRequest verifyResetTokenCommandRequest)
        {
            VerifyResetTokenCommandResponse response = await _mediator.Send(verifyResetTokenCommandRequest);
            return Ok(response);
        }
    }
}
