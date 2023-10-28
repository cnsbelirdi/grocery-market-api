using GroceryAPI.Application.Repositories;
using GroceryAPI.Persistence.Contexts;

namespace GroceryAPI.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<GroceryAPI.Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(GroceryAPIDbContext context) : base(context)
        {
        }
    }
}
