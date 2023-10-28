using GroceryAPI.Application.DTOs.Product;

namespace GroceryAPI.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryResponse
    {
        public string BasketItemId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public ProductImageFileDTO ProductImageFile { get; set; }
        public string ProductId { get; set; }
    }
}
