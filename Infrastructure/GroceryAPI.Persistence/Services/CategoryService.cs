using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.DTOs.Category;
using GroceryAPI.Application.DTOs.Product;
using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Persistence.Services
{
    public class CategoryService : ICategoryService
    {
        readonly ICategoryReadRepository _categoryReadRepository;
        readonly ICategoryWriteRepository _categoryWriteRepository;
        readonly IProductReadRepository _productReadRepository;

        public CategoryService(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository, IProductReadRepository productReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task CreateCategoryAsync(string name)
        {
            await _categoryWriteRepository.AddAsync(new() { Name = name });
            await _categoryWriteRepository.SaveAsync();
        }

        public async Task<ListCategory> GetAllCategoriesAsync(int page, int size)
        {
            var query = _categoryReadRepository.GetAll(false);

            IQueryable<Category> data = null;

            if (page != -1 && size != -1)
                data = query.Skip(page * size).Take(size);
            else
                data = query;

            return new()
            {
                TotalCategoryCount = await query.CountAsync(),
                Categories = data.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.CreatedDate
                })
            };
        }

        public async Task<SingleCategory> GetCategoryByIdAsync(string id)
        {
            Category category = await _categoryReadRepository.GetByIdAsync(id);
            return new()
            {
                Id = category.Id.ToString(),
                Name = category.Name
            };
        }

        public async Task<ListProduct> GetCategoryProductsByIdAsync(string id, int page, int size)
        {
            var query = _productReadRepository.GetAll(false)
                .Where(p => p.Stock > 0)
                .Include(p => p.Category)
                .Include(p => p.ProductImageFiles).Where(p => p.Category.Id == new Guid(id));


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
                    p.CreatedDate,
                    p.Active,
                    p.ProductImageFiles
                })
            };
        }

        public async Task RemoveCategoryAsync(string id)
        {
            await _categoryWriteRepository.RemoveAsync(id);
            await _categoryWriteRepository.SaveAsync();
        }
    }
}
