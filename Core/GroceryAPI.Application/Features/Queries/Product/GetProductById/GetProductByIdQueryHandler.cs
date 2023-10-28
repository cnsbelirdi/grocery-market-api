using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.Product.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        readonly IProductService _productService;

        public GetProductByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _productService.GetProductByIdAsync(request.Id);
           
            return new()
            {
                Product = data.Product
            };
        }
    }
}
