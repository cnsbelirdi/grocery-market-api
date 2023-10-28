using GroceryAPI.Application.Constants;
using GroceryAPI.Application.CustomAttributes;
using GroceryAPI.Application.Enums;
using GroceryAPI.Application.Features.Commands.Payment.CreatePayment;
using GroceryAPI.Application.Features.Commands.Payment.RemovePayment;
using GroceryAPI.Application.Features.Commands.Payment.UpdatePayment;
using GroceryAPI.Application.Features.Commands.Product.CreateProduct;
using GroceryAPI.Application.Features.Commands.Product.RemoveProduct;
using GroceryAPI.Application.Features.Commands.Product.UpdateProduct;
using GroceryAPI.Application.Features.Queries.Payment.GetAllPayments;
using GroceryAPI.Application.Features.Queries.Payment.GetPaymentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GroceryAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Payments, ActionType = ActionType.Reading, Definition = "Get All Payments")]
        public async Task<IActionResult> Get([FromQuery]GetAllPaymentsQueryRequest getAllPaymentsQueryRequest)
        {
            GetAllPaymentsQueryResponse response = await _mediator.Send(getAllPaymentsQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Payments, ActionType = ActionType.Reading, Definition = "Get Payment By Id")]
        public async Task<IActionResult> Get([FromRoute] GetPaymentByIdQueryRequest getPaymentByIdQueryRequest)
        {
            GetPaymentByIdQueryResponse response = await _mediator.Send(getPaymentByIdQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Payments, ActionType = ActionType.Writing, Definition = "Create Payment")]
        public async Task<IActionResult> Post(CreatePaymentCommandRequest createPaymentCommandRequest)
        {
            CreatePaymentCommandResponse response = await _mediator.Send(createPaymentCommandRequest);
            return Ok(response);
        }

        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Payments, ActionType = ActionType.Updating, Definition = "Update Payment")]
        public async Task<IActionResult> Put([FromBody] UpdatePaymentCommandRequest updatePaymentCommandRequest)
        {
            UpdatePaymentCommandResponse response = await _mediator.Send(updatePaymentCommandRequest);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Payments, ActionType = ActionType.Deleting, Definition = "Delete Payment")]
        public async Task<IActionResult> Delete([FromRoute] RemovePaymentCommandRequest removePaymentCommandRequest)
        {
            RemovePaymentCommandResponse response = await _mediator.Send(removePaymentCommandRequest);
            return Ok(response);
        }

    }
}
