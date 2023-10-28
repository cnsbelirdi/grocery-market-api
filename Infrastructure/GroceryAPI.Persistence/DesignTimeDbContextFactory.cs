using GroceryAPI.API;
using GroceryAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GroceryAPI.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GroceryAPIDbContext>
    {
        public GroceryAPIDbContext CreateDbContext(string[] args)
        {

            DbContextOptionsBuilder<GroceryAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);

        }
    }
}
