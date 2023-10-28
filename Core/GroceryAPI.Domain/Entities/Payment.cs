using GroceryAPI.Domain.Entities.Common;

namespace GroceryAPI.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string? Information { get; set; }
        public float Amount { get; set; }
    }
}
