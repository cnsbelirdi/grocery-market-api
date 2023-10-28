using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Application.DTOs.Order
{
    public class CompletedOrder
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
