using MediatR;

namespace GroceryAPI.Application.Features.Queries.Payment.GetPaymentById
{
    public class GetPaymentByIdQueryRequest : IRequest<GetPaymentByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}
