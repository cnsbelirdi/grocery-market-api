using GroceryAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Application.Repositories
{
    public interface ICategoryWriteRepository : IWriteRepository<Category>
    {
    }
}
