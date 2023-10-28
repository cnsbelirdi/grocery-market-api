using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Commands.Product.ChangeProductActive
{
    public class ChangeProductActiveCommandHandler : IRequestHandler<ChangeProductActiveCommandRequest, ChangeProductActiveCommandResponse>
    {
        readonly IProductService _productService;

        public ChangeProductActiveCommandHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ChangeProductActiveCommandResponse> Handle(ChangeProductActiveCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.SetActiveAsync(request.Id);
            return new();
        }
    }
}
