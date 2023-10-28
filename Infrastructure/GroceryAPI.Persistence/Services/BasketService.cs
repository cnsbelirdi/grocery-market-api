using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Repositories;
using GroceryAPI.Application.ViewModels.Baskets;
using GroceryAPI.Domain.Entities;
using GroceryAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GroceryAPI.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        readonly IProductReadRepository _productReadRepository;

        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository, IProductReadRepository productReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
            _productReadRepository = productReadRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username)) 
            {
                AppUser? user = await _userManager.Users
                    .Include(u => u.Baskets)
                    .FirstOrDefaultAsync(u => u.UserName == username);

                var _basket = from basket in user?.Baskets
                              join order in _orderReadRepository.Table
                              on basket.Id equals order.Id into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order
                              };
                Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null))
                {
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
                }
                else
                {
                    targetBasket = new();
                    user?.Baskets.Add(targetBasket);
                }
                await _basketWriteRepository.SaveAsync();

                return targetBasket;
            }
            throw new Exception("An unexpected error was encountered.");
        }

        public async Task<bool> AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();
            if (basket != null)
            {
                BasketItem? _basketItem = await _basketItemReadRepository.Table
                    .FirstOrDefaultAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));

                Product _product = await _productReadRepository.GetByIdAsync(basketItem.ProductId);

                if (_basketItem != null)
                {
                    if(_basketItem.Product.Stock >= basketItem.Quantity + _basketItem.Quantity)
                    {
                        _basketItem.Quantity += basketItem.Quantity;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if(_product.Stock >= basketItem.Quantity)
                    {
                        await _basketItemWriteRepository.AddAsync(new()
                        {
                            BasketId = basket.Id,
                            ProductId = Guid.Parse(basketItem.ProductId),
                            Quantity = basketItem.Quantity
                        });
                    }
                    else
                    {
                        return false;
                    }
                }
                await _basketItemWriteRepository.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket = await ContextUser();
            Basket? result = await _basketReadRepository.Table
                .Include(b => b.BasketItems)
                    .ThenInclude(bi => bi.Product)
                        .ThenInclude(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(b => b.Id == basket.Id);

            List<BasketItem> basketItems = result.BasketItems.ToList();

            foreach (var basketItem in basketItems)
            {
                Product product = basketItem.Product;
                product.ProductImageFiles = product.ProductImageFiles
                    .Where(pif => pif.Showcase)
                    .ToList();
            }

            return basketItems;
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task<bool> UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem? _basketItem = await _basketItemReadRepository.Table
                .Include(bi => bi.Product)
                .FirstOrDefaultAsync(bi => bi.Id == Guid.Parse(basketItem.BasketItemId));

            if (_basketItem != null)
            {
                if(_basketItem.Product.Stock >= basketItem.Quantity)
                {
                    _basketItem.Quantity = basketItem.Quantity;
                    await _basketItemWriteRepository.SaveAsync();
                    return true;
                }
            }
            return false;
        }

        public Basket? GetUserActiveBasket
        {
            get 
            {
                Basket? basket = ContextUser().Result;
                return basket;
            }
        }
    }
}
