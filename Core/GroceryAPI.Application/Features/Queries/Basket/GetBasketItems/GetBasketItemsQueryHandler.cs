using GroceryAPI.Application.Abstractions.Services;
using MediatR;

namespace GroceryAPI.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
    {
        readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<List<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await _basketService.GetBasketItemsAsync();
            return basketItems.Select(ba => new GetBasketItemsQueryResponse
            {
                BasketItemId = ba.Id.ToString(),
                Name = ba.Product.Name,
                Price = ba.Product.Price,
                Quantity = ba.Quantity,
                ProductImageFile = new DTOs.Product.ProductImageFileDTO
                {
                    FileName = ba.Product.ProductImageFiles.First().FileName,
                    Path = ba.Product.ProductImageFiles.First().Path,
                    Showcase = ba.Product.ProductImageFiles.First().Showcase,
                },
                ProductId = ba.ProductId.ToString()
            }).ToList();
        }
    }
}
