using MediatR;

namespace GroceryAPI.Application.Features.Queries.Category.GetCategoryProductsById
{
    public class GetCategoryProductsByIdQueryRequest : IRequest<GetCategoryProductsByIdQueryResponse>
    {
        public string Id { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
