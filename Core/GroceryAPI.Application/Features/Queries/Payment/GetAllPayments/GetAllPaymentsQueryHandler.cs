using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.Payment.GetAllPayments
{
    public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQueryRequest, GetAllPaymentsQueryResponse>
    {
        readonly IPaymentService _paymentService;

        public GetAllPaymentsQueryHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<GetAllPaymentsQueryResponse> Handle(GetAllPaymentsQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _paymentService.GetAllPaymentsAsync(request.Page, request.Size);
            return new GetAllPaymentsQueryResponse
            {
                TotalPaymentCount = data.TotalPaymentCount,
                Payments = data.Payments
            };
        }
    }
}
