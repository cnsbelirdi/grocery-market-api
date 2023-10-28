using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.Payment.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommandRequest, CreatePaymentCommandResponse>
    {
        readonly IPaymentService _paymentService;

        public CreatePaymentCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<CreatePaymentCommandResponse> Handle(CreatePaymentCommandRequest request, CancellationToken cancellationToken)
        {
            await _paymentService.CreatePaymentAsync(new()
            {
                Information = request.Information,
                Amount = request.Amount,
                Type = request.Type,
            });
            return new();
        }
    }
}
