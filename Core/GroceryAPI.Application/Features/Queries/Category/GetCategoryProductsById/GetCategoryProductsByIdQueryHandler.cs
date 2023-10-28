using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.Category.GetCategoryProductsById
{
    public class GetCategoryProductsByIdQueryHandler : IRequestHandler<GetCategoryProductsByIdQueryRequest, GetCategoryProductsByIdQueryResponse>
    {
        readonly ICategoryService _categoryService;

        public GetCategoryProductsByIdQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<GetCategoryProductsByIdQueryResponse> Handle(GetCategoryProductsByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _categoryService.GetCategoryProductsByIdAsync(request.Id, request.Page, request.Size);
            return new()
            {
                Products = data.Products,
                TotalProductCount = data.TotalProductCount,
            };
        }
    }
}
