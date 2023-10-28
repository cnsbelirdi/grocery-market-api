using MediatR;

namespace GroceryAPI.Application.Features.Queries.Payment.GetAllPayments
{
    public class GetAllPaymentsQueryRequest : IRequest<GetAllPaymentsQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
