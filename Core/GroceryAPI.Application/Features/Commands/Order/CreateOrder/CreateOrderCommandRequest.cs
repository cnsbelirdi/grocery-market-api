using MediatR;

namespace GroceryAPI.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandRequest : IRequest<CreateOrderCommandResponse>
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public string DeliveryTime { get; set; }
        public string PaymentOption { get; set; }
    }
}
