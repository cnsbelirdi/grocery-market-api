using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.Order.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommandRequest, CancelOrderCommandResponse>
    {
        readonly IOrderService _orderService;

        public CancelOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CancelOrderCommandResponse> Handle(CancelOrderCommandRequest request, CancellationToken cancellationToken)
        {
            await _orderService.CancelOrderAsync(request.Id);
            return new();
        }
    }
}
