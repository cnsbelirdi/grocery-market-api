using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using GroceryAPI.Persistence.Contexts;

namespace GroceryAPI.Persistence.Repositories
{
    public class PaymentReadRepository : ReadRepository<Payment>, IPaymentReadRepository
    {
        public PaymentReadRepository(GroceryAPIDbContext context) : base(context)
        {
        }
    }
}
