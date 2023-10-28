using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.Payment.GetPaymentById
{
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQueryRequest, GetPaymentByIdQueryResponse>
    {
        readonly IPaymentService _paymentService;

        public GetPaymentByIdQueryHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<GetPaymentByIdQueryResponse> Handle(GetPaymentByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _paymentService.GetPaymentByIdAsync(request.Id);
            return new GetPaymentByIdQueryResponse
            {
                Id = data.Id,
                Amount = data.Amount,
                Information = data.Information,
                Type = data.Type,
                CreatedDate = data.CreatedDate
            };
        }
    }
}
