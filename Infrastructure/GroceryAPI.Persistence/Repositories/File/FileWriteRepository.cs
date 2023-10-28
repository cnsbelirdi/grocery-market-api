using GroceryAPI.Application.Repositories;
using GroceryAPI.Persistence.Contexts;

namespace GroceryAPI.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<GroceryAPI.Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(GroceryAPIDbContext context) : base(context)
        {
        }
    }
}
