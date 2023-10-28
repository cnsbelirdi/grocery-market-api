using GroceryAPI.Application.Abstractions.Hubs;
using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.DTOs.Product;
using GroceryAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        readonly IProductService _productService;
        readonly IProductHubService _productHubService;

        public CreateProductCommandHandler(IProductService productService, IProductHubService productHubService)
        {
            _productService = productService;
            _productHubService = productHubService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.CreateProductAsync(new()
            {
                Name = request.Name,
                Stock = request.Stock,
                Price = request.Price,
                Barcode = request.Barcode,
                Description = request.Description,
                CategoryId = request.CategoryId,
            });

            await _productHubService.ProductAddedMessageAsync($"{request.Name} added!");
            return new();
        }
    }
}
