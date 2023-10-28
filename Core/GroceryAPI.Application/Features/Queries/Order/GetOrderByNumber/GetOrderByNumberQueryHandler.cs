using GroceryAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Application.Features.Queries.Order.GetOrderByNumber
{
    public class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQueryRequest, GetOrderByNumberQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetOrderByNumberQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByNumberQueryResponse> Handle(GetOrderByNumberQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _orderService.GetOrderByNumber(request.Number);
            return new()
            {
                Id = data.Id,
                OrderNumber = data.OrderNumber,
                Address = data.Address,
                DeliveryTime = data.DeliveryTime,
                PaymentOption = data.PaymentOption,
                TotalPrice = data.TotalPrice,
                BasketItems = data.BasketItems,
                CreatedDate = data.CreatedDate,
                Description = data.Description,
                Status = data.Status,
                Fullname = data.Fullname,
                PhoneNumber = data.PhoneNumber
            };
        }
    }
}
