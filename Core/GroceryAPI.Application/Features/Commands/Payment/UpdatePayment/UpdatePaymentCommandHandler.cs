using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.Payment.UpdatePayment
{
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommandRequest, UpdatePaymentCommandResponse>
    {
        readonly IPaymentService _paymentService;

        public UpdatePaymentCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<UpdatePaymentCommandResponse> Handle(UpdatePaymentCommandRequest request, CancellationToken cancellationToken)
        {
            await _paymentService.UpdatePaymentAsync(new()
            {
                Id = request.Id,
                Amount = request.Amount,
                Information = request.Information,
                Type = request.Type,
            });
            return new();
        }
    }
}
