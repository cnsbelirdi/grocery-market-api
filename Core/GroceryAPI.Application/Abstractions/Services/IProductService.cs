using GroceryAPI.Application.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace GroceryAPI.Application.Abstractions.Services
{
    public interface IProductService
    {
        int TotalProductCount { get; }
        int TotalStockCount { get; }
        int TotalActiveProductCount { get; }
        Task<ListProduct> GetAllProductsAsync(int page, int size);
        Task<SingleProductById> GetProductByIdAsync(string id);
        Task<SingleProduct> GetProductByBarcode(string barcode);
        Task CreateProductAsync(CreateProduct createProduct);
        Task UpdateProductAsync(UpdateProduct updateProduct);
        Task RemoveProductAsync(string id);
        Task SetActiveAsync(string id);
        Task<List<ListProductImage>> GetProductImagesAsync(string id);
        Task UploadProductImageAsync(string id, IFormFileCollection? files);
        Task RemoveProductImageAsync(string id, string? imageId);
        Task ChangeProductShowcaseImage(string imageId, string productId);
    }
}
