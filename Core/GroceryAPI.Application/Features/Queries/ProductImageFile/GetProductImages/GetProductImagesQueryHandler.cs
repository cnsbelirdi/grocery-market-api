using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        readonly IProductService _productService;

        public GetProductImagesQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _productService.GetProductImagesAsync(request.Id);
            return data.Select(p => new GetProductImagesQueryResponse
            {
                Path = p.Path,
                FileName = p.FileName,
                Id = p.Id
            }).ToList();
        }
    }
}
