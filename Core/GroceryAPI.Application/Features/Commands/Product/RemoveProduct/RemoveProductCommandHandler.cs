﻿using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Repositories;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.Product.RemoveProduct
{
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
    {
        readonly IProductService _productService;

        public RemoveProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.RemoveProductAsync(request.Id);
            return new();
        }
    }
}
