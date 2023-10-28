using MediatR;

namespace GroceryAPI.Application.Features.Queries.Category.GetAllCategories
{
    public class GetAllCategoriesQueryRequest : IRequest<GetAllCategoriesQueryResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
