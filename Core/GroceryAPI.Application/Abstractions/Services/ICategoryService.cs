using GroceryAPI.Application.DTOs.Category;
using GroceryAPI.Application.DTOs.Product;
using GroceryAPI.Domain.Entities;

namespace GroceryAPI.Application.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<ListCategory> GetAllCategoriesAsync(int page, int size);

        Task<SingleCategory> GetCategoryByIdAsync(string id);
        Task<ListProduct> GetCategoryProductsByIdAsync(string id, int page, int size);
        Task CreateCategoryAsync(string name);
        Task RemoveCategoryAsync(string id);
    }
}
