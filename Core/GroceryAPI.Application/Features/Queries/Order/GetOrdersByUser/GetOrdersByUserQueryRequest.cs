using MediatR;

namespace GroceryAPI.Application.Features.Queries.Order.GetOrdersByUser
{
    public class GetOrdersByUserQueryRequest : IRequest<GetOrdersByUserQueryResponse>
    {
        public string UserId { get; set; }
    }
}
