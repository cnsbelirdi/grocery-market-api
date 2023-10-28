using GroceryAPI.Application.Repositories;
using GroceryAPI.Domain.Entities;
using GroceryAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Persistence.Repositories
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
    {
        public ProductWriteRepository(GroceryAPIDbContext context) : base(context) { }
    }
}
