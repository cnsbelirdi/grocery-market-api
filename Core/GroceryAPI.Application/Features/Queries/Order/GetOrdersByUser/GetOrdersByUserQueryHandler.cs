using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.Order.GetOrdersByUser
{
    public class GetOrdersByUserQueryHandler : IRequestHandler<GetOrdersByUserQueryRequest, GetOrdersByUserQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetOrdersByUserQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrdersByUserQueryResponse> Handle(GetOrdersByUserQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _orderService.GetOrdersByUser(request.UserId);

            return new()
            {
                TotalOrderCount = data.TotalOrderCount,
                Orders = data.Orders
            };
        }
    }
}
