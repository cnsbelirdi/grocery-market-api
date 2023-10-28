using MediatR;

namespace GroceryAPI.Application.Features.Commands.Payment.CreatePayment
{
    public class CreatePaymentCommandRequest : IRequest<CreatePaymentCommandResponse>
    {
        public string Type { get; set; }
        public string? Information { get; set; }
        public float Amount { get; set; }
    }
}
