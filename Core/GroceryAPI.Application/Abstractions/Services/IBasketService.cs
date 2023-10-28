using GroceryAPI.Application.ViewModels.Baskets;
using GroceryAPI.Domain.Entities;

namespace GroceryAPI.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task<List<BasketItem>> GetBasketItemsAsync();
        public Task<bool> AddItemToBasketAsync(VM_Create_BasketItem basketItem);
        public Task<bool> UpdateQuantityAsync(VM_Update_BasketItem basketItem);
        public Task RemoveBasketItemAsync(string basketItemId);
        public Basket GetUserActiveBasket { get; }
    }
}
