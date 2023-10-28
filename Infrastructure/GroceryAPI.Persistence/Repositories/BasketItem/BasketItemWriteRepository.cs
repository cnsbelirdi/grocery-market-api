using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using GroceryAPI.Persistence.Contexts;

namespace GroceryAPI.Persistence.Repositories
{
    public class BasketItemWriteRepository : WriteRepository<BasketItem>, IBasketItemWriteRepository
    {
        public BasketItemWriteRepository(GroceryAPIDbContext context) : base(context)
        {
        }
    }
}
