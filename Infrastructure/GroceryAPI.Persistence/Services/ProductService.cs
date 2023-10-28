using GroceryAPI.Application.Abstractions.Hubs;
using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Abstractions.Storage;
using GroceryAPI.Application.DTOs.Product;
using GroceryAPI.Application.Exceptions;
using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace GroceryAPI.Persistence.Services
{
    public class ProductService : IProductService
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;
        readonly ICategoryReadRepository _categoryReadRepository;
        readonly IConfiguration _configuration;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IStorageService _storageService;

        public ProductService(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, ICategoryReadRepository categoryReadRepository, IProductHubService productHubService, IConfiguration configuration, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _configuration = configuration;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
        }

        public int TotalProductCount => _productReadRepository.Table.Count();
        public int TotalStockCount => _productReadRepository.GetAll(false).SumAsync(p => p.Stock).Result;
        public int TotalActiveProductCount => _productReadRepository.Table.Where(p => p.Active).Count();
        public async Task CreateProductAsync(CreateProduct createProduct)
        {
            Category category = await _categoryReadRepository.GetByIdAsync(createProduct.CategoryId);
            await _productWriteRepository.AddAsync(new()
            {
                Name = createProduct.Name,
                Stock = createProduct.Stock,
                Price = createProduct.Price,
                Barcode = createProduct.Barcode,
                Description = createProduct.Description,
                Category = category,
                Active = createProduct.Stock > 0 ? true : false
            }); ;
            await _productWriteRepository.SaveAsync();
        }

        public async Task<ListProduct> GetAllProductsAsync(int page, int size)
        {
            var query = _productReadRepository.GetAll(false)
                .Include(p => p.Category)
                .Include(p => p.ProductImageFiles)
                .OrderByDescending(p => p.CreatedDate);

            IQueryable<Product> data = null;

            if (page != -1 && size != -1)
                data = query.Skip(page * size).Take(size);
            else
                data = query;

            return new()
            {
                TotalProductCount = await query.CountAsync(),
                Products = data.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Stock,
                    p.Price,
                    p.Description,
                    p.Barcode,
                    Category = p.Category.Name,
                    CategoryId = p.Category.Id,
                    CreatedDate = p.CreatedDate.ToString("dd.MM.yyyy"),
                    p.Active,
                    p.ProductImageFiles
                })
            };
        }

        public async Task<SingleProduct> GetProductByBarcode(string barcode)
        {
            Product? product = await _productReadRepository.Table.FirstOrDefaultAsync(p => p.Barcode == barcode);
            if (product != null)
            {
                return new()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    Description = product.Description,
                    Barcode = product.Barcode,
                    Category = product.Category.Name,
                    Active = product.Active,
                    CreatedDate = product.CreatedDate,
                };
            }
            throw new NotFoundProductException();
            
        }

        public async Task<SingleProductById> GetProductByIdAsync(string id)
        {
            Product? product = await _productReadRepository.Table
                        .Include(p => p.Category)
                        .Include(p => p.ProductImageFiles)
                        .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            if (product != null)
            {
                var productDTO = new
                {
                    product.Name,
                    product.Price,
                    product.Stock,
                    product.Description,
                    product.Barcode,
                    Category = product.Category?.Name,
                    product.Active,
                    ProductImageFiles = product.ProductImageFiles.Select(p => new ProductImageFileDTO
                    {
                        Showcase = p.Showcase,
                        Path = p.Path,
                        FileName = p.FileName
                    }).ToList()
                };

                return new SingleProductById
                {
                    Product = productDTO
                };
            }

            throw new NotFoundProductException();
        }

        public async Task RemoveProductAsync(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
        }

        public async Task UpdateProductAsync(UpdateProduct updateProduct)
        {
            Product product = await _productReadRepository.GetByIdAsync(updateProduct.Id);
            Category category = await _categoryReadRepository.GetByIdAsync(updateProduct.CategoryId);
            product.Stock = updateProduct.Stock;
            product.Name = updateProduct.Name;
            product.Price = updateProduct.Price;
            product.Description = updateProduct.Description;
            product.Barcode = updateProduct.Barcode;
            product.Active = product.Stock <= 0 ? false : true;
            product.Category = category;
            product.UpdatedDate = DateTime.UtcNow;
            await _productWriteRepository.SaveAsync();
        }

        public async Task<List<ListProductImage>> GetProductImagesAsync(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return product.ProductImageFiles.Select(p => new ListProductImage
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                FileName = p.FileName,
                Id = p.Id
            }).ToList();

        }

        public async Task UploadProductImageAsync(string id , IFormFileCollection? files)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", files);

            Product product = await _productReadRepository.GetByIdAsync(id);

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product },
                Showcase = true
            }).ToList()); ;

            await _productImageFileWriteRepository.SaveAsync();
        }

        public async Task RemoveProductImageAsync(string id, string? imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            ProductImageFile? productImageFile = product?.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));

            if (productImageFile != null)
                product?.ProductImageFiles.Remove(productImageFile);

            await _productWriteRepository.SaveAsync();
        }

        public async Task ChangeProductShowcaseImage(string imageId, string productId)
        {
            var query = _productImageFileWriteRepository.Table
                .Include(p => p.Products)
                .SelectMany(p => p.Products, (pif, p) => new { pif, p });
            var data = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(productId) && p.pif.Showcase);

            if (data != null)
                data.pif.Showcase = false;


            var image = await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(imageId));
            if (image != null)
                image.pif.Showcase = true;

            await _productImageFileWriteRepository.SaveAsync();
        }

        public async Task SetActiveAsync(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id);
            product.Active = !product.Active;
            await _productWriteRepository.SaveAsync();
        }
    }
}
