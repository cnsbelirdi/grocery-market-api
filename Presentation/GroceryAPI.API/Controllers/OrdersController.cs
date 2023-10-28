using GroceryAPI.Application.Constants;
using GroceryAPI.Application.CustomAttributes;
using GroceryAPI.Application.Enums;
using GroceryAPI.Application.Features.Commands.Order.CancelOrder;
using GroceryAPI.Application.Features.Commands.Order.CompleteOrder;
using GroceryAPI.Application.Features.Commands.Order.CreateOrder;
using GroceryAPI.Application.Features.Commands.Product.RemoveProduct;
using GroceryAPI.Application.Features.Queries.Order.GetAllOrders;
using GroceryAPI.Application.Features.Queries.Order.GetOrderByNumber;
using GroceryAPI.Application.Features.Queries.Order.GetOrdersByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroceryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Writing, Definition = "Create Order")]
        public async Task<ActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
        {
            CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequest);
            return Ok(response);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get All Orders")]
        public async Task<ActionResult> GetAllOrders([FromQuery]GetAllOrdersQueryRequest getAllOrdersQueryRequest)
        {
            GetAllOrdersQueryResponse response = await _mediator.Send(getAllOrdersQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]/{UserId}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get Orders By User")]
        public async Task<ActionResult> GetOrdersByUser([FromRoute] GetOrdersByUserQueryRequest getOrdersByUserQueryRequest)
        {
            GetOrdersByUserQueryResponse response = await _mediator.Send(getOrdersByUserQueryRequest);
            return Ok(response);
        }
        [HttpGet("{Number}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get Order By Number")]
        public async Task<ActionResult> GetOrderByNumber([FromRoute] GetOrderByNumberQueryRequest getOrderByNumberQueryRequest)
        {
            GetOrderByNumberQueryResponse response = await _mediator.Send(getOrderByNumberQueryRequest);
            return Ok(response);
        }

        [HttpGet("complete-order/{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Updating, Definition = "Complete Order")]
        public async Task<ActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest completeOrderCommandRequest)
        {
            CompleteOrderCommandResponse response = await _mediator.Send(completeOrderCommandRequest);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Deleting, Definition = "Cancel Order")]
        public async Task<IActionResult> Delete([FromRoute] CancelOrderCommandRequest cancelOrderCommandRequest)
        {
            CancelOrderCommandResponse response = await _mediator.Send(cancelOrderCommandRequest);
            return Ok();
        }

    }
}
