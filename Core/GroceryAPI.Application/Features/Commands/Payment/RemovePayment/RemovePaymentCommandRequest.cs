using MediatR;

namespace GroceryAPI.Application.Features.Commands.Payment.RemovePayment
{
    public class RemovePaymentCommandRequest : IRequest<RemovePaymentCommandResponse>
    {
        public string Id { get; set; }
    }
}
