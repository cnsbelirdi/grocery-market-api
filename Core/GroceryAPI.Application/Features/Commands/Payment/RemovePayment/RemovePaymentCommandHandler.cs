using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.Payment.RemovePayment
{
    public class RemovePaymentCommandHandler : IRequestHandler<RemovePaymentCommandRequest, RemovePaymentCommandResponse>
    {
        readonly IPaymentService _paymentService;

        public RemovePaymentCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<RemovePaymentCommandResponse> Handle(RemovePaymentCommandRequest request, CancellationToken cancellationToken)
        {
            await _paymentService.RemovePaymentAsync(request.Id);
            return new();
        }
    }
}
