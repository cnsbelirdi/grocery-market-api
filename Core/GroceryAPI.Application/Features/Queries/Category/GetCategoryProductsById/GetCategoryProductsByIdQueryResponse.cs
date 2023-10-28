namespace GroceryAPI.Application.Features.Queries.Category.GetCategoryProductsById
{
    public class GetCategoryProductsByIdQueryResponse
    {
        public int TotalProductCount { get; set; }
        public object Products { get; set; }
    }
}
