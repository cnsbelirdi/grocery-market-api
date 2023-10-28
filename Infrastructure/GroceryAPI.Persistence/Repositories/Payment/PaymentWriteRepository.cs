using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using GroceryAPI.Persistence.Contexts;

namespace GroceryAPI.Persistence.Repositories
{
    public class PaymentWriteRepository : WriteRepository<Payment>, IPaymentWriteRepository
    {
        public PaymentWriteRepository(GroceryAPIDbContext context) : base(context)
        {
        }
    }
}
