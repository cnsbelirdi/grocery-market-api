using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.Product.GetProductByBarcode
{
    public class GetProductByBarcodeQueryHandler : IRequestHandler<GetProductByBarcodeQueryRequest, GetProductByBarcodeQueryResponse>
    {
        readonly IProductService _productService;

        public GetProductByBarcodeQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductByBarcodeQueryResponse> Handle(GetProductByBarcodeQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _productService.GetProductByBarcode(request.Barcode);
            return new()
            {
                Name = data.Name,
                Price = data.Price,
                Stock = data.Stock,
                Description = data.Description,
                Barcode = data.Barcode,
                Category = data.Category,
                Active = data.Active,
                CreatedDate = data.CreatedDate,
            };
        }
    }
}
