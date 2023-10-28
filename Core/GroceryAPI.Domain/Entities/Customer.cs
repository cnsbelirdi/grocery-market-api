using GroceryAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        //ICollection<Order> Orders { get; set; }
    }
}
