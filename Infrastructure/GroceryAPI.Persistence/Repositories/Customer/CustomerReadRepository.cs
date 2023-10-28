using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using GroceryAPI.Persistence.Contexts;

namespace GroceryAPI.Persistence.Repositories
{
    public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
    {
        public CustomerReadRepository(GroceryAPIDbContext context) : base(context) { }
    }
}
