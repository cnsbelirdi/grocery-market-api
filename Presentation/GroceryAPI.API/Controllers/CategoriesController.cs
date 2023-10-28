using GroceryAPI.Application.Constants;
using GroceryAPI.Application.CustomAttributes;
using GroceryAPI.Application.Enums;
using GroceryAPI.Application.Features.Commands.Category.CreateCategory;
using GroceryAPI.Application.Features.Commands.Category.RemoveCategory;
using GroceryAPI.Application.Features.Queries.Category.GetAllCategories;
using GroceryAPI.Application.Features.Queries.Category.GetCategoryProductsById;
using GroceryAPI.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Categories", Menu = AuthorizeDefinitionConstants.Categories)]
        public async Task<IActionResult> Get([FromQuery]GetAllCategoriesQueryRequest getAllCategoriesQueryRequest)
        {
            GetAllCategoriesQueryResponse response = await _mediator.Send(getAllCategoriesQueryRequest);
            return Ok(response);
        }

        [HttpGet("get-category-products")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Category Products By Id", Menu = AuthorizeDefinitionConstants.Categories)]
        public async Task<IActionResult> Get([FromQuery] GetCategoryProductsByIdQueryRequest getCategoryProductsByIdQueryRequest)
        {
            GetCategoryProductsByIdQueryResponse response = await _mediator.Send(getCategoryProductsByIdQueryRequest);
            return Ok(response);
        }

        [HttpPost()]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Categories, ActionType = ActionType.Writing, Definition = "Create Category")]
        public async Task<IActionResult> Post([FromBody]CreateCategoryCommandRequest createCategoryCommandRequest)
        {
            CreateCategoryCommandResponse response = await _mediator.Send(createCategoryCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Categories, ActionType = ActionType.Deleting, Definition = "Remove Category")]
        public async Task<IActionResult> Delete([FromRoute]RemoveCategoryCommandRequest removeCategoryCommandRequest)
        {
            RemoveCategoryCommandResponse response = await _mediator.Send(removeCategoryCommandRequest);
            return Ok();
        }
    }
}
