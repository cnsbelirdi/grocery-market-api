﻿using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
    {

        readonly IProductService _productService;

        public RemoveProductImageCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.RemoveProductImageAsync(request.Id, request.ImageId);
            return new();
        }
    }
}
