using GroceryAPI.Application.Constants;
using GroceryAPI.Application.CustomAttributes;
using GroceryAPI.Application.Enums;
using GroceryAPI.Application.Features.Commands.AppUser.AssignRoleToUser;
using GroceryAPI.Application.Features.Commands.AppUser.CreateUser;
using GroceryAPI.Application.Features.Commands.AppUser.RemoveUser;
using GroceryAPI.Application.Features.Commands.AppUser.UpdatePassword;
using GroceryAPI.Application.Features.Commands.AppUser.UpdateUser;
using GroceryAPI.Application.Features.Commands.Category.RemoveCategory;
using GroceryAPI.Application.Features.Queries.AppUser.GetAllUsers;
using GroceryAPI.Application.Features.Queries.AppUser.GetRolesToUser;
using GroceryAPI.Application.Features.Queries.AppUser.GetUserByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create New User", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("update-password")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update User Password", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> CreateUser([FromBody]UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            UpdatePasswordCommandResponse response = await _mediator.Send(updatePasswordCommandRequest);
            return Ok(response);
        }

        [HttpPost("update-user")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update User Info", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> UpdateUser(UpdateUserCommandRequest updateUserCommandRequest)
        {
            UpdateUserCommandResponse response = await _mediator.Send(updateUserCommandRequest);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes ="Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Users", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> GetAllUsers([FromQuery]GetAllUsersQueryRequest getAllUsersQueryRequest)
        {
            GetAllUsersQueryResponse response = await _mediator.Send(getAllUsersQueryRequest);
            return Ok(response);
        }

        [HttpGet("[action]/{Name}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get User By Name", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> GetUserByName([FromRoute] GetUserByNameQueryRequest getUserByNameQueryRequest)
        {
            GetUserByNameQueryResponse response = await _mediator.Send(getUserByNameQueryRequest);
            return Ok(response);
        }

        [HttpGet("get-roles-to-user/{UserId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To User", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> GetRolesToUser([FromRoute] GetRolesToUserQueryRequest getRolesToUserQuery)
        {
            GetRolesToUserQueryResponse response = await _mediator.Send(getRolesToUserQuery);
            return Ok(response);
        }

        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Assign Role To User", Menu = AuthorizeDefinitionConstants.Users)]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserCommandRequest assignRoleToUserCommandRequest)
        {
            AssignRoleToUserCommandResponse response = await _mediator.Send(assignRoleToUserCommandRequest);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Deleting, Definition = "Remove User")]
        public async Task<IActionResult> Delete([FromRoute] RemoveUserCommandRequest removeUserCommandRequest)
        {
            RemoveUserCommandResponse response = await _mediator.Send(removeUserCommandRequest);
            return Ok();
        }
    }
}
