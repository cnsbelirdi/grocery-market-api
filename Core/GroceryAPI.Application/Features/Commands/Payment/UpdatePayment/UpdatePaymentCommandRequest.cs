using MediatR;

namespace GroceryAPI.Application.Features.Commands.Payment.UpdatePayment
{
    public class UpdatePaymentCommandRequest : IRequest<UpdatePaymentCommandResponse>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string? Information { get; set; }
        public float Amount { get; set; }
    }
}
