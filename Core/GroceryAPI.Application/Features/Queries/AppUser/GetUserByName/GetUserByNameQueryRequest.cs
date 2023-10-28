using MediatR;

namespace GroceryAPI.Application.Features.Queries.AppUser.GetUserByName
{
    public class GetUserByNameQueryRequest : IRequest<GetUserByNameQueryResponse>
    {
        public string Name { get; set; }
    }
}
